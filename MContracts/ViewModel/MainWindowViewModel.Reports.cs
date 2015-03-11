using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using CommonBase;
using CommonBase.Progress;
using MContracts.Classes;
using MContracts.Properties;
using McReports;
using McReports.Common;
using McReports.ViewModel;
using McUIBase.Factories;
using UIShared.Commands;
using UIShared.Works;

namespace MContracts.ViewModel
{
    /// <summary>
    ///     Определяет регион создания отчётов
    /// </summary>
    partial class MainWindowViewModel : IDateRange
    {
        private const string DefMessage = "Построение отчёта...";

        #region BuildReport

        private int _currentViewModelIndex;
        private int _reportsCount;

        /// <summary>
        ///     Запускает построение отчёта в фоновом режиме.
        ///     Принимает в качестве параметра модель представления отчёта. 
        ///     Важно: BuildReportAsync по окончанию работы вызовет Dispose() для viewModel
        /// </summary>
        /// <typeparam name="T">Тип модели представления отчёта</typeparam>
        /// <param name="viewModel">Объект модели представления</param>
        public void BuildReportAsync<T>(T viewModel) where T : BaseReportViewModel
        {
            Contract.Requires(viewModel != null);
            BuildReportAsync(viewModel.AsSingleElementCollection());
        }
        /// <summary>
        ///     Запускает построение отчёта в фоновом режиме.
        ///     Принимает в качестве параметра коллекцию моделей представления отчёта. 
        ///     Важно: BuildReportAsync по окончанию работы вызовет Dispose() для каждого элемента модели 
        /// </summary>
       
        /// <typeparam name="T">Тип модели представления отчёта</typeparam>
        /// <param name="viewModels">Объект модели представления</param>
   
        public void BuildReportAsync<T>(IEnumerable<T> viewModels) where T : BaseReportViewModel
        {
            ActiveWorkspace.BusyContent = DefMessage;

            var worker = new ViewModelBackgroundWorker {ActiveViewModel = ActiveWorkspace};
            worker.DoWork += EnqueueWorkerDoWork;
            worker.RunWorkerCompleted += EnqueueWorkerRunWorkerCompleted;
            worker.ProgressChanged += EnqueueworkerProgressChanged;
            worker.RunWorkerAsync(viewModels);
        }


        private void EnqueueworkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var totalPercents = _currentViewModelIndex*100 + e.ProgressPercentage;
            var percents = totalPercents/_reportsCount;
            Contract.Assert(_reportsCount >= _currentViewModelIndex);
            Contract.Assert(percents.Between(0, 100));

            ActiveWorkspace.BusyContent = string.Format("{0}{1}%",
                                                        e.UserState.NullCastTo<BaseReportViewModel>()
                                                         .Return(
                                                             x =>
                                                             string.IsNullOrEmpty(x.BusyContent)
                                                                 ? DefMessage
                                                                 : x.BusyContent, DefMessage),
                                                        percents);
        }


        private void EnqueueWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender.CastTo<BackgroundWorker>();
            worker.RunWorkerCompleted -= EnqueueWorkerRunWorkerCompleted;
            worker.DoWork -= EnqueueWorkerDoWork;
            worker.ProgressChanged -= EnqueueworkerProgressChanged;
            var reports = e.Result.CastTo<IEnumerable<BaseReportViewModel>>().ToList();
            reports.FirstOrDefault().Do(x => x.SaveChanges());
            reports.Apply(x => x.ShowReport());
            reports.Apply(obj => obj.Dispose());
        }

        private void EnqueueWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var reportViewModels = e.Argument.CastTo<IEnumerable<BaseReportViewModel>>().ToList();
            _reportsCount = reportViewModels.Count;
            _currentViewModelIndex = 0;
            foreach (var vm in reportViewModels)
            {
                CreateReport(vm, sender.CastTo<BackgroundWorker>());
                ++_currentViewModelIndex;
            }
            e.Result = e.Argument;
        }


        /// <summary>
        ///     Готовит отчёт к созданию
        /// </summary>
        /// <param name="reportViewModel">Модель представления отчёта</param>
        /// <param name="worker">Объект рабочего потока, в котором производится построение отчёта</param>
        private void CreateReport(BaseReportViewModel reportViewModel, BackgroundWorker worker)
        {
            // Задаётся провайдер шаблонов отчёта. По умолчанию - на основе значения свойств Settings
            reportViewModel.TemplateProvider = CreateTemplateProvider();
            // Источником данных для отчётов служат текущие договора в реестре
            // Это свойство устанавливается для всех отчётов, но может не использоваться в конкретных отчётах
            reportViewModel.ReportSource = CurrentContractRepositoryViewModel;
            // Задаёт в качестве объекта параметров отчёта коллекцию свойств приложения
            reportViewModel.ApplicationParameters = new PropertiesDecorator();
            // Устанавливает объект уведомления о прогрессе
            reportViewModel.ProgressReporter = new WorkerProgressAdaptor(worker, reportViewModel);

            // Инициализация фабрики отчётов. Используется стандартное окно запроса параметров,
            // если отчёту необходимы данные от пользователя во время выполнения
            var factory = new ReportFactoryBase();
            
            // Вызывает построение отчёта
            factory.CreateReport(reportViewModel);
        }

        #endregion

        #region Reports

        private void CreateContractInformationReport()
        {
            BuildReportAsync(
                new InformationConcludedContracts_ViewModel(RepositoryFactory.CreateContractRepository())
                    {
                        Period = Range
                    });
        }

        private void CreateExcelYearPlanReport_1()
        {
            BuildReportAsync(
                new ContractYearPlanReportViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }

        private void CreateNIOKRNTRUImplementationReport()
        {
            BuildReportAsync(
                new NIOKRImplementationReportViewModel(RepositoryFactory.CreateContractRepository()) { Period = Range });
        }

        private void CreateEconEffectReport()
        {
            BuildReportAsync(
                new NIOKREconomicEffectReportViewModel(RepositoryFactory.CreateContractRepository()) { Period = Range });
        }

        private void CreateNIOKRTroubleReport()
        {
            BuildReportAsync(new TroubleNIOKRReportViewModel(RepositoryFactory.CreateContractRepository()) { Period = Range });
        }
        /// <summary>
        ///     текущая справка по договорам
        /// </summary>
        private void CreateExcelInformationReport_2()
        {
            BuildReportAsync(
                new ContractRegisterReport_2_ViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }


        /// <summary>
        ///     о ходе подписания договоров
        /// </summary>
        private void CreateWorkPorgressReport()
        {
            BuildReportAsync(
                new WorkProgressReportViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }

        private static ITemplateProvider CreateTemplateProvider()
        {
            return new DefaultTemplateProvider
                (AppDomain.CurrentDomain.BaseDirectory, Settings.Default.ReportTemplateFolder, Settings.Default);
        }


        /// <summary>
        ///     текущая справка по договорам на переходный период
        /// </summary>
        private void CreateExcelContractsPeriodReport_3()
        {
            BuildReportAsync(
                new ContractPeriodReport_3_ViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }

        private void CreateExcelContractQuarterPlanReport_4()
        {
            BuildReportAsync(
                new ContractQuarterPlanRepotViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }

        private void CreateExcelSubContractReport_5()
        {
            BuildReportAsync(
                new SubContractRegisterReportViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }


        private void CreateExcelHandingWorkReport_6()
        {
            BuildReportAsync(
                new HandingWorkReportViewModel(RepositoryFactory.CreateContractRepository()) {Period = Range});
        }


        private void CreateExcelEfficientInformationReport_7()
        {
            BuildReportAsync(
                new EfficientInformationMonthReport_7_ViewModel(RepositoryFactory.CreateContractRepository())
                    {
                        Period = Range
                    });
        }

        private void CreateExcelEfficientInformationReport_7_1()
        {
            BuildReportAsync(
                new EfficientInformationQuarterReport_7_ViewModel(RepositoryFactory.CreateContractRepository())
                    {
                        Period = Range
                    });
        }


        private void CreateTransferActsContractsReport_8_1()
        {
            //var viewModel = new TransferActsContractsReport_8_1_ViewModel(RepositoryFactory.CreateContractRepository());
            //ReportFactory.CreateReport(viewModel);
        }

        private void CreateTransferContractsReport_8_2()
        {
            //var viewModel = new TransferContractsReport_8_2_ViewModel(RepositoryFactory.CreateContractRepository());
            //ReportFactory.CreateReport(viewModel);
        }

        #endregion

        #region ReportCommands

        public RelayCommand CreateWorkPorgressReportCommand
        {
            get
            {
                return createWorkPorgressReportCommand ??
                       (createWorkPorgressReportCommand =
                        new RelayCommand(p => CreateWorkPorgressReport(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsQuarter));
            }
        }

        public RelayCommand CreateInformationReportCommand
        {
            get
            {
                return _createInformationReport_2_Command ??
                       (_createInformationReport_2_Command =
                        new RelayCommand(p => CreateExcelInformationReport_2(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        public RelayCommand QuarterTemplanCommand
        {
            get
            {
                return _createExcelContractQuarterPlanReport_4_Command ??
                       (_createExcelContractQuarterPlanReport_4_Command =
                        new RelayCommand(p => CreateExcelContractQuarterPlanReport_4(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsQuarter));
            }
        }

        public RelayCommand CreateExcelSubContractReportCommand
        {
            get
            {
                return _createExcelSubContractReport_5_Command ??
                       (_createExcelSubContractReport_5_Command =
                        new RelayCommand(p => CreateExcelSubContractReport_5(),
                                         (x) => ActiveWorkspace is ContractRepositoryViewBasedViewModel));
            }
        }


        public RelayCommand CreateExcelHandingWorkReportCommand
        {
            get
            {
                return _createExcelHandingWorkReport_6_Command ??
                       (_createExcelHandingWorkReport_6_Command =
                        new RelayCommand(p => CreateExcelHandingWorkReport_6(),
                                         (x) => ActiveWorkspace is ContractRepositoryViewBasedViewModel));
            }
        }


        public RelayCommand CreateMonthExcelEfficientInformationReportCommand
        {
            get
            {
                return _createExcelEfficientInformationReport_7_1_Command ??
                       (_createExcelEfficientInformationReport_7_1_Command =
                        new RelayCommand(p => CreateExcelEfficientInformationReport_7_1(),
                                         (x) => ActiveWorkspace is ContractRepositoryViewBasedViewModel));
            }
        }

        public RelayCommand CreateTransferActsContractsReport_8_1_Command
        {
            get
            {
                return _createTransferActsContractsReport_8_1_Command ??
                       (_createTransferActsContractsReport_8_1_Command =
                        new RelayCommand(p => CreateTransferActsContractsReport_8_1(),
                                         (x) =>
                                         ((ActiveWorkspace is ContractViewModel) &&
                                          (ActiveWorkspace as ContractViewModel).Contractdoc != null)));
            }
        }

        public RelayCommand CreateTransferContractsReport_8_2_Command
        {
            get
            {
                return _createTransferContractsReport_8_2_Command ??
                       (_createTransferContractsReport_8_2_Command =
                        new RelayCommand(p => CreateTransferContractsReport_8_2(),
                                         (x) => ((ActiveWorkspace is ContractViewModel) &&
                                                 (ActiveWorkspace as ContractViewModel).Contractdoc != null)));
            }
        }

        public RelayCommand CreateExcelContractsPeriodReportCommand
        {
            get
            {
                return _createExcelContractsPeriodReport_3_Command ??
                       (_createExcelContractsPeriodReport_3_Command =
                        new RelayCommand(p => CreateExcelContractsPeriodReport_3(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        public RelayCommand CreateContractInformationReportCommand
        {
            get { return new RelayCommand(p => CreateContractInformationReport()); }
        }

        public RelayCommand YearTemplanCommand
        {
            get
            {
                return _createExcelYearPlanReport1Command ??
                       (_createExcelYearPlanReport1Command =
                        new RelayCommand(p => CreateExcelYearPlanReport_1(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }


        private void CreateContractInActionRegisterReport()
        {
            BuildReportAsync(
                new ContractInActionYearRegisterReportViewModel(RepositoryFactory.CreateContractRepository()) { Period = Range });
        }

        public RelayCommand _contractInActionRegisterReportCommand; 
        public RelayCommand ContractInActionRegisterReportCommand
        {
            get
            {
                return _contractInActionRegisterReportCommand ??
                       (_contractInActionRegisterReportCommand =
                        new RelayCommand(p => CreateContractInActionRegisterReport(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        public RelayCommand NIOKRNTRUImplementationReportCommand
        {
            get
            {
                return _NIOKRNTRUImplementationReportCommand ??
                       (_NIOKRNTRUImplementationReportCommand =
                        new RelayCommand(p => CreateNIOKRNTRUImplementationReport(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        public RelayCommand _econeffectreportcommand;

        public RelayCommand EconEffectReportCommand
        {
            get
            {
                return _econeffectreportcommand ??
                       (_econeffectreportcommand =
                        new RelayCommand(p => CreateEconEffectReport(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        private RelayCommand _NIOKRTroubleReportCommand;

        public RelayCommand NIOKRTroubleReportCommand
        {
            get
            {
                return _NIOKRTroubleReportCommand ??
                       (_NIOKRTroubleReportCommand =
                        new RelayCommand(p => CreateNIOKRTroubleReport(),
                                         (x) =>
                                         ActiveWorkspace is ContractRepositoryViewBasedViewModel &&
                                         SelectedFilterstate.IsYear));
            }
        }

        #endregion

        public DateRange Range
        {
            get { return new DateRange {Start = SelectedFilterStartDate, End = SelectedFilterEndDate}; }
            set { throw new NotImplementedException(); }
        }
    }
}