using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CommonBase;
using CommonBase.Progress;
using McReports.Common;
using McReports.ViewModel;
using UIShared.ViewModel;
using UIShared.Works;

namespace McReports
{
    public class ReportBuilder<T> where T : BaseReportViewModel 
    {
        private int _reportsCount;
        private int _currentViewModelIndex;
        public ViewModelBase ActiveViewModel { get; private set; }
        public BackgroundWorker Worker { get; private set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public IReportSourceProvider ReportSourceProvider { get; set; }
        public object AppParameters { get; set; }

        public ReportBuilder()
        {
            Worker = new ViewModelBackgroundWorker {WorkerReportsProgress = true};
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.DoWork += Worker_DoWork;
        }

        void Worker_DoWork(object sender, DoWorkEventArgs e)
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

        private void CreateReport(BaseReportViewModel reportViewModel, BackgroundWorker workder)
        {
            // Задаётся провайдер шаблонов отчёта. По умолчанию - на основе значения свойств Settings
            reportViewModel.TemplateProvider = TemplateProvider;
        
            // Источником данных для отчётов служат текущие договора в реестре
            // Это свойство устанавливается для всех отчётов, но может не использоваться в конкретных отчётах
            reportViewModel.ReportSource = ReportSourceProvider;

            reportViewModel.ApplicationParameters = AppParameters;

            reportViewModel.ProgressReporter = new WorkerProgressAdaptor(workder, reportViewModel);

            // Инициализация фабрики отчётов. Используется стандартное окно запроса параметров,
            // если отчёту необходимы данные от пользователя во время выполнения
            var factory = new ReportFactoryBase();
            //{
            //    UiQueryParametersProvider = new ReportParamsWindow {ViewModel = reportViewModel}
            //};
            // Вызывает построение отчёта
            factory.CreateReport(reportViewModel);
        }
        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void BuildReportAsync(T viewModel) 
        {
            
        }
    }
}
