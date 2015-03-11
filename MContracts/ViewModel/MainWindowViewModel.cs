#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Controls.Dialogs;
using MContracts.Properties;
using MContracts.ViewModel.Helpers;
using McUIBase.Factories;
using McUIBase.ViewModel;
using MediatorLib;
using UIShared.Commands;
using UIShared.Common;
using UIShared.ViewModel;
using UIShared.Works;

#endregion

namespace MContracts.ViewModel
{
    /// <summary>
    ///     The ViewModel for the application's main window.
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        ///     Получает единственный экземпляр модели представления главного окна
        /// </summary>
        public static readonly MainWindowViewModel Instance = new MainWindowViewModel();

        /// <summary>
        ///     Получает объект команды закрытия
        /// </summary>
        public readonly Closable Closable;

        #region Constructor

        private MainWindowViewModel()
        {
            ViewMediator.Register(this);
            Closable = new Closable(this);
            Settings.Default.PropertyChanged += Default_PropertyChanged;
            ProceedStartupRequests();
        }

        public string CopyrightInfo
        {
            get { return App.AssemblyCopyrightInfo; }
        }
        public override string DisplayName
        {
            get { return string.Format("{0} {1} - [Cборка {2}]", ProductInfo, FileVersionInfo, AssemblyVersionInfo); }
            protected set { base.DisplayName = value; }
        }
       
        public ICommand UpdateStatisticsCommand
        {
            get { return new RelayCommand(x => UpdateStatistics()); }
        }

        public ICommand OpenScheduleResultsCommand
        {
            get { return new RelayCommand((x) => OpenScheduleResults(), x => CanOpenContractDetails() && !(ActiveWorkspace is StageResultsViewModel)); }
        }

        private void ProceedStartupRequests()
        {
            if (ProjectStartupInfo.UpdateStatistics)
                DoUpdateStatistics();
        }

        private void DoUpdateStatistics()
        {
            using (var repository = RepositoryFactory.CreateContractRepository())
            {
                repository.With(x => x.TryGetContext()).Do(x => x.UpdateFundsStatistics()).Do(x => x.SubmitChanges());
            }
        }

  
        private void OpenScheduleResults()
        {
            OpenContractWorkspace<StageResultsViewModel>(typeof (ScheduleViewModel), typeof (ActsViewModel));
        }

        public ICommand SaveSettingsCommand
        {
            get
            {
                return new RelayCommand(_ => Settings.Default.Save());
            }
        }

        public event EventHandler ReloadSettingsRequest;

        protected virtual void OnReloadSettingsRequest()
        {
            EventHandler handler = ReloadSettingsRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public ICommand RevertSettingsCommand
        {
            get
            {
                return new RelayCommand(_ => { Settings.Default.Reload();
                                                 OnReloadSettingsRequest();
                });
            }
        }

        private void UpdateStatistics()
        {
            //var dlgResult =
            //    AppMessageBox.Show(
            //        Resources.UpdateStatisticsWarning,
            //        MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (dlgResult != MessageBoxResult.Yes) return;

            //this.Do(x => x.BusyContent = Resources.UpdateStatisticsBusyContent);
            //var worker = new ViewModelBackgroundWorker {ActiveViewModel = ActiveWorkspace};
            var worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private bool _updateStatisticsReady = true;
        public bool UpdateStatisticsReady
        {
            get { return _updateStatisticsReady; }
            set
            {
                if (_updateStatisticsReady == value) return;
                _updateStatisticsReady = value;
                OnPropertyChanged(()=>UpdateStatisticsReady);
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = sender.CastTo<BackgroundWorker>();
            worker.RunWorkerCompleted -= worker_RunWorkerCompleted;
            worker.DoWork -= worker_DoWork;
            UpdateStatisticsReady = true;
            //AppMessageBox.Show(
            //  Resources.UpdateStatisticsFinishedWarning, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateStatisticsReady = false;
            DoUpdateStatistics();
        }

        [MediatorMessageSink(RequestRepository.REQUEST_SELECTEDCONTRACT_CHANGED, ParameterType = typeof (bool))]
        public void SelectedContractChanged(bool flag)
        {
            OnPropertyChanged(() => SelectedContract);
        }

        private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_GLOBAL_PROPERTIES_CHANGED, e);
        }

        #endregion // Constructor

        #region Commands

        private readonly NewContractdocParams _newGeneralParams = new NewContractdocParams(null,
                                                                                           NewContractdocType.NewGeneral);

        private RelayCommand _NIOKRNTRUImplementationReportCommand;

        //private RelayCommand _createActReport_7_Command;
        //private RelayCommand _createContractQuarterPlanReport_4_Command;
        //private RelayCommand _createContractRegisterReport_2_Command;
        //private RelayCommand _createContractYearPlanReport_1_Command;
        //private RelayCommand _createContractsPeriodReport_3_Command;
        //private RelayCommand _createEfficientInformationReport_7_Command;
        private RelayCommand _createExcelContractQuarterPlanReport_4_Command;
        private RelayCommand _createExcelContractsPeriodReport_3_Command;
        private RelayCommand _createExcelEfficientInformationReport_7_1_Command;
        //private RelayCommand _createExcelEfficientInformationReport_7_Command;
        private RelayCommand _createExcelHandingWorkReport_6_Command;
        private RelayCommand _createExcelSubContractReport_5_Command;
        private RelayCommand _createExcelYearPlanReport1Command;
        //private RelayCommand _createHandingWorkReport_6_Command;
        private RelayCommand _createInformationReport_2_Command;
        //private RelayCommand _createSubContractReport_5_Command;
        private RelayCommand _createTransferActsContractsReport_8_1_Command;
        private RelayCommand _createTransferContractsReport_8_2_Command;
        //private RelayCommand _createWorkPlanQuarterReport_6_Command;
        private RelayCommand _deleteorcancelContractCommand;
        private RelayCommand _newAgreementCommand;
        private RelayCommand _newContractCommand;
        private RelayCommand _newSubgeneralCommand;
        private RelayCommand _openContractActsCommand;
        private RelayCommand _openContractCommand;
        private RelayCommand _openContractScheduleCommand;
        private RelayCommand _openToolsCommand;
        private RelayCommand _saveActiveWorkspaceCommand;
        public Filterstate _selectedfilterstate;
        //private DateTime _selectedFilterEndDate;
        //private DateTime _selectedFilterStartDate;
        private RelayCommand _showActRepositoryCommand;

        private RelayCommand _showContractRepositoryCommand;
        private RelayCommand _showContractTransferRepositoryCommand;
        private RelayCommand _showDisposalsRepositoryCommand;
        private RelayCommand _showOrdersRepositoryCommand;
        private RelayCommand acttypesShowCommand;
        private RelayCommand approvalGoalsShowCommand;
        private RelayCommand approvalStatesShowCommand;
        private RelayCommand authoritiesShowCommand;
        private RelayCommand contractStatesShowCommand;
        private RelayCommand contractTypesShowCommand;
        private RelayCommand contractorpositionShowCommand;
        private RelayCommand contractorsShowCommand;
        private RelayCommand contractortypesShowCommand;
        private RelayCommand createWorkPorgressReportCommand;
        private RelayCommand currenciesShowCommand;
        private RelayCommand currencymeasuresShowCommand;
        private RelayCommand degreesShowCommand;
        private RelayCommand departmentsShowCommand;
        private RelayCommand documentsShowCommand;
        private RelayCommand economefficiencyparametersShowCommand;
        private RelayCommand economefficiencytypesShowCommand;
        private RelayCommand efficienceparameterTypesShowCommand;
        private RelayCommand employeesShowCommand;
        private RelayCommand funccustomerpersonsShowCommand;
        private RelayCommand functionalcustomersShowCommand;
        private RelayCommand functionalcustomertypesShowCommand;
        private RelayCommand locationsShowCommand;
        private RelayCommand missiveTypesShowCommand;
        private RelayCommand ndsalgorithmesShowCommand;
        private RelayCommand ndsesShowCommand;
        private RelayCommand ntpsubviewsShowCommand;
        private RelayCommand ntpviewsShowCommand;
        private RelayCommand personsShowCommand;
        private RelayCommand positionsShowCommand;
        private RelayCommand prepaymentdocumenttypesShowCommand;
        private RelayCommand propertiesShowCommand;
        private RelayCommand regionsShowCommand;
        private RelayCommand rolesShowCommand;
        private RelayCommand saveLogFileCommand;
        private RelayCommand sightfuncpersonschemesShowCommand;
        private RelayCommand transferacttypedocumentsShowCommand;
        private RelayCommand transferacttypesShowCommand;
        private RelayCommand troublesShowCommand;
        private RelayCommand troublesregistresShowCommand;
        private RelayCommand worktypesShowCommand;
        private RelayCommand yearreportcolorsShowCommand;
        private RelayCommand сontractorpropertiesShowCommand;

        public ICommand LogonCommand
        {
            get { return new RelayCommand((param) => Logon()); }
        }

        public RelayCommand ShowContractRepositoryCommand
        {
            get
            {
                return _showContractRepositoryCommand ??
                       (_showContractRepositoryCommand = new RelayCommand(param => ShowContractRepository()));
            }
        }


        public Filterstate SelectedFilterstate
        {
            get { return _selectedfilterstate; }
            set
            {
                _selectedfilterstate = value;
                OnPropertyChanged(() => SelectedFilterstate);
                OnPropertyChanged(() => SelectedFilterStartDate);
                OnPropertyChanged(() => SelectedFilterEndDate);
            }
        }

        public DateTime SelectedFilterStartDate
        {
            get { return SelectedFilterstate.Return(x => x.Startdate, DateTimeExtensions.StartOfTheYear(DateTime.Now.Year)); }
        }

        public DateTime SelectedFilterEndDate
        {
            get { return SelectedFilterstate.Return(x => x.Finishdate, DateTimeExtensions.EndOfTheYear(DateTime.Now.Year)); }
        }

        public RelayCommand ShowActRepositoryCommand
        {
            get
            {
                return _showActRepositoryCommand ??
                       (_showActRepositoryCommand = new RelayCommand(param => ShowActRepository()));
            }
        }

        public RelayCommand ShowContractTransferRepositoryCommand
        {
            get
            {
                return _showContractTransferRepositoryCommand ??
                       (_showContractTransferRepositoryCommand =
                        new RelayCommand(param => ShowContractTransferRepository()));
            }
        }

        public RelayCommand OpenContractActsCommand
        {
            get
            {
                return _openContractActsCommand ??
                       (_openContractActsCommand =
                        new RelayCommand(param => OpenContractActs(), x => CanOpenContractDetails() && !(ActiveWorkspace is ActsViewModel)));
            }
        }

        public RelayCommand ShowDisposalsRepositoryCommand
        {
            get
            {
                return _showDisposalsRepositoryCommand ??
                       (_showDisposalsRepositoryCommand = new RelayCommand(param => ShowDisposalsRepository()));
            }
        }

        public RelayCommand ShowOrdersRepositoryCommand
        {
            get
            {
                return _showOrdersRepositoryCommand ??
                       (_showOrdersRepositoryCommand = new RelayCommand(param => ShowOrdersRepository()));
            }
        }


        public RelayCommand ContractTypesShowCommand
        {
            get
            {
                return contractTypesShowCommand ??
                       (contractTypesShowCommand =
                        new RelayCommand(param => ContractTypesShow(), x => CanContractTypesShow));
            }
        }

        public RelayCommand ContractorpropertiesShowCommand
        {
            get
            {
                return сontractorpropertiesShowCommand ??
                       (сontractorpropertiesShowCommand = new RelayCommand(param => ContractorpropertiesShow(),
                                                                           x => CanContractorpropertiesShow));
            }
        }

        public RelayCommand SightfuncpersonschemesShowCommand
        {
            get
            {
                return sightfuncpersonschemesShowCommand ??
                       (sightfuncpersonschemesShowCommand = new RelayCommand(param => SightfuncpersonschemesShow(),
                                                                             x => CanSightfuncpersonschemesShow));
            }
        }


        public RelayCommand OpenContractScheduleCommand
        {
            get
            {
                return _openContractScheduleCommand ??
                       (_openContractScheduleCommand =
                        new RelayCommand(p => OpenContractSchedule(), (x) => CanOpenContractDetails() && !(ActiveWorkspace is ScheduleViewModel)));
            }
        }


        public ICommand ExportRegistryToExcel
        {
            get { return new RelayCommand(x => ExportRegistry()); }
        }


        public RelayCommand OpenToolsCommand
        {
            get { return _openToolsCommand ?? (_openToolsCommand = new RelayCommand(p => OpenToolsDialog())); }
        }

        public ICommand DeleteOrCancelContractCommand
        {
            get
            {
                return _deleteorcancelContractCommand ?? (_deleteorcancelContractCommand = new RelayCommand(
                                                                                               x => SendDeleteContract(),
                                                                                               x => ContractSelected));
            }
        }

        public RelayCommand FunccustomerpersonsShowCommand
        {
            get
            {
                return funccustomerpersonsShowCommand ??
                       (funccustomerpersonsShowCommand = new RelayCommand(param => FunccustomerpersonsShow(),
                                                                          x => CanFunccustomerpersonsShow));
            }
        }


        public RelayCommand DocumentsShowCommand
        {
            get
            {
                return documentsShowCommand ??
                       (documentsShowCommand = new RelayCommand(param => DocumentsShow(), x => CanDocumentsShow));
            }
        }

        public RelayCommand TransferacttypesShowCommand
        {
            get
            {
                return transferacttypesShowCommand ??
                       (transferacttypesShowCommand = new RelayCommand(param => TransferacttypesShow(),
                                                                       x => CanTransferacttypesShow));
            }
        }

        public RelayCommand TransferacttypedocumentsShowCommand
        {
            get
            {
                return transferacttypedocumentsShowCommand ??
                       (transferacttypedocumentsShowCommand = new RelayCommand(param => TransferacttypedocumentsShow(),
                                                                               x => CanTransferacttypedocumentsShow));
            }
        }

        public RelayCommand FunctionalcustomersShowCommand
        {
            get
            {
                return functionalcustomersShowCommand ??
                       (functionalcustomersShowCommand = new RelayCommand(param => FunctionalcustomersShow(),
                                                                          x => CanFunctionalcustomersShow));
            }
        }

        public RelayCommand FunctionalcustomertypesShowCommand
        {
            get
            {
                return functionalcustomertypesShowCommand ??
                       (functionalcustomertypesShowCommand = new RelayCommand(param => FunctionalcustomertypesShow(),
                                                                              x => CanFunctionalcustomertypesShow));
            }
        }

        public RelayCommand PersonsShowCommand
        {
            get
            {
                return personsShowCommand ??
                       (personsShowCommand = new RelayCommand(param => PersonsShow(), x => CanPersonsShow));
            }
        }

        public RelayCommand EmployeesShowCommand
        {
            get
            {
                return employeesShowCommand ??
                       (employeesShowCommand = new RelayCommand(param => EmployeesShow(), x => CanEmployeesShow));
            }
        }

        public RelayCommand DepartmentsShowCommand
        {
            get
            {
                return departmentsShowCommand ??
                       (departmentsShowCommand = new RelayCommand(param => DepartmentsShow(), x => CanDepartmentsShow));
            }
        }

        public RelayCommand RegionsShowCommand
        {
            get
            {
                return regionsShowCommand ??
                       (regionsShowCommand = new RelayCommand(param => RegionsShow(), x => CanRegionsShow));
            }
        }

        public RelayCommand ActtypesShowCommand
        {
            get
            {
                return acttypesShowCommand ??
                       (acttypesShowCommand = new RelayCommand(param => ActtypesShow(), x => CanActtypesShow));
            }
        }

        public RelayCommand NtpviewsShowCommand
        {
            get
            {
                return ntpviewsShowCommand ??
                       (ntpviewsShowCommand = new RelayCommand(param => NtpviewsShow(), x => CanNtpviewsShow));
            }
        }

        public RelayCommand YearreportcolorsShowCommand
        {
            get
            {
                return yearreportcolorsShowCommand ??
                       (yearreportcolorsShowCommand =
                        new RelayCommand(param => YearreportcolorsShow(), x => CanYearreportcolorsShow));
            }
        }

        public RelayCommand LocationsShowCommand
        {
            get
            {
                return locationsShowCommand ??
                       (locationsShowCommand = new RelayCommand(param => LocationsShow(), x => CanTroublesregistresShow));
            }
        }

        public RelayCommand MissiveTypesShowCommand
        {
            get
            {
                return missiveTypesShowCommand ??
                       (missiveTypesShowCommand = new RelayCommand(param => MissiveTypesShow(),
                                                                   x => CanTroublesregistresShow));
            }
        }


        public RelayCommand ApprovalGoalsShowCommand
        {
            get
            {
                return approvalGoalsShowCommand ??
                       (approvalGoalsShowCommand = new RelayCommand(param => ApprovalGoalsShow(),
                                                                    x => CanTroublesregistresShow));
            }
        }

        public RelayCommand ApprovalStatesShowCommand
        {
            get
            {
                return approvalStatesShowCommand ??
                       (approvalStatesShowCommand = new RelayCommand(param => ApprovalStatesShow(),
                                                                     x => CanTroublesregistresShow));
            }
        }


        public RelayCommand TroublesregistresShowCommand
        {
            get
            {
                return troublesregistresShowCommand ??
                       (troublesregistresShowCommand = new RelayCommand(param => TroublesregistresShow(),
                                                                        x => CanTroublesregistresShow));
            }
        }

        public RelayCommand ContractortypesShowCommand
        {
            get
            {
                return contractortypesShowCommand ??
                       (contractortypesShowCommand = new RelayCommand(param => ContractortypesShow(),
                                                                      x => CanContractortypesShow));
            }
        }

        public RelayCommand ContractorsShowCommand
        {
            get
            {
                return contractorsShowCommand ??
                       (contractorsShowCommand = new RelayCommand(param => ContractorsShow(), x => CanContractorsShow));
            }
        }

        public RelayCommand NtpsubviewsShowCommand
        {
            get
            {
                return ntpsubviewsShowCommand ??
                       (ntpsubviewsShowCommand = new RelayCommand(param => NtpsubviewsShow(), x => CanNtpsubviewsShow));
            }
        }

        public RelayCommand CurrenciesShowCommand
        {
            get
            {
                return currenciesShowCommand ??
                       (currenciesShowCommand = new RelayCommand(param => CurrenciesShow(), x => CanCurrenciesShow));
            }
        }

        public RelayCommand ContractStatesShowCommand
        {
            get
            {
                return contractStatesShowCommand ??
                       (contractStatesShowCommand = new RelayCommand(param => ContractStatesShow(),
                                                                     x => CanContractStatesShow));
            }
        }

        public RelayCommand NdsesShowCommand
        {
            get
            {
                return ndsesShowCommand ??
                       (ndsesShowCommand = new RelayCommand(param => NdsesShow(), x => CanNdsesShow));
            }
        }

        public RelayCommand NdsalgorithmesShowCommand
        {
            get
            {
                return ndsalgorithmesShowCommand ??
                       (ndsalgorithmesShowCommand =
                        new RelayCommand(param => NdsalgorithmsShow(), x => CanNdsalgorithmsShow));
            }
        }

        public RelayCommand PropertiesShowCommand
        {
            get
            {
                return propertiesShowCommand ??
                       (propertiesShowCommand = new RelayCommand(param => PropertiesShow(), x => CanPropertiesShow));
            }
        }

        public RelayCommand RolesShowCommand
        {
            get
            {
                return rolesShowCommand ??
                       (rolesShowCommand = new RelayCommand(param => RolesShow(), x => CanRolesShow));
            }
        }

        public RelayCommand PrepaymentdocumenttypesShowCommand
        {
            get
            {
                return prepaymentdocumenttypesShowCommand ??
                       (prepaymentdocumenttypesShowCommand = new RelayCommand(param => PrepaymentdocumenttypesShow(),
                                                                              x => CanPrepaymentdocumenttypesShow));
            }
        }

        public RelayCommand PositionsShowCommand
        {
            get
            {
                return positionsShowCommand ??
                       (positionsShowCommand = new RelayCommand(param => PositionsShow(), x => CanPositionsShow));
            }
        }

        public RelayCommand WorktypesShowCommand
        {
            get
            {
                return worktypesShowCommand ??
                       (worktypesShowCommand = new RelayCommand(param => WorktypesShow(), x => CanWorktypesShow));
            }
        }

        public RelayCommand SaveLogFileCommand
        {
            get { return saveLogFileCommand ?? (saveLogFileCommand = new RelayCommand(param => SaveLogFile(), x => true)); }
        }

        public RelayCommand EconomefficiencytypesShowCommand
        {
            get
            {
                return economefficiencytypesShowCommand ??
                       (economefficiencytypesShowCommand = new RelayCommand(param => EconomefficiencytypesShow(),
                                                                            x => CanEconomefficiencytypesShow));
            }
        }

        public RelayCommand EfficienceparameterTypesShowCommand
        {
            get
            {
                return efficienceparameterTypesShowCommand ??
                       (efficienceparameterTypesShowCommand = new RelayCommand(param => EfficienceparameterTypesShow(),
                                                                               x => CanEfficienceparameterTypesShow));
            }
        }

        public RelayCommand EconomefficiencyparametersShowCommand
        {
            get
            {
                return economefficiencyparametersShowCommand ??
                       (economefficiencyparametersShowCommand =
                        new RelayCommand(param => EconomefficiencyparametersShow(),
                                         x => CanEconomefficiencyparametersShow));
            }
        }

        public RelayCommand DegreesShowCommand
        {
            get
            {
                return degreesShowCommand ??
                       (degreesShowCommand = new RelayCommand(param => DegreesShow(), x => CanDegreesShow));
            }
        }

        public RelayCommand TroublesShowCommand
        {
            get
            {
                return troublesShowCommand ??
                       (troublesShowCommand = new RelayCommand(param => TroublesShow(), x => CanTroublesShow));
            }
        }

        public RelayCommand CurrencymeasuresShowCommand
        {
            get
            {
                return currencymeasuresShowCommand ??
                       (currencymeasuresShowCommand = new RelayCommand(param => CurrencymeasuresShow(),
                                                                       x => CanCurrencymeasuresShow));
            }
        }

        public RelayCommand ContractorpositionsShowCommand
        {
            get
            {
                return contractorpositionShowCommand ??
                       (contractorpositionShowCommand = new RelayCommand(param => ContractorpositionShow(),
                                                                         x => CanContractorpositionShow));
            }
        }

        public RelayCommand AuthoritiesShowCommand
        {
            get
            {
                return authoritiesShowCommand ??
                       (authoritiesShowCommand = new RelayCommand(param => AuthoritiesShow(), x => CanAuthoritiesShow));
            }
        }

        public RelayCommand NewContractCommand
        {
            get
            {
                return _newContractCommand ??
                       (_newContractCommand = new RelayCommand(param => NewContract(_newGeneralParams)));
            }
        }

        public RelayCommand NewAgreementCommand
        {
            get { return CreateNewContractCommand(NewContractdocType.NewAgreement, ref _newAgreementCommand); }
        }


        public RelayCommand NewSubgeneralCommand
        {
            get { return CreateNewContractCommand(NewContractdocType.NewSubgeneral, ref _newSubgeneralCommand); }
        }

        private ContractRepositoryViewBasedViewModel CurrentContractRepositoryViewModel
        {
            get { return (ActiveWorkspace as ContractRepositoryViewBasedViewModel); }
        }

        public Contractdoc SelectedContract
        {
            get { return CurrentContractRepositoryViewModel == null ? null : CurrentContractRepositoryViewModel.Current; }
        }

        private bool ContractSelected
        {
            get
            {
                return (CurrentContractRepositoryViewModel != null) &&
                       (CurrentContractRepositoryViewModel.CurrentContractRepositoryView != null);
            }
        }

        public RelayCommand OpenContractCommand
        {
            get
            {
                return _openContractCommand ??
                       (_openContractCommand = new RelayCommand(param => OpenContract(SelectedContract),
                                                                x => ContractSelected));
            }
        }

        public RelayCommand SaveActiveWorkspaceCommand
        {
            get
            {
                return _saveActiveWorkspaceCommand ??
                       (_saveActiveWorkspaceCommand = new RelayCommand(x => SaveActiveWorkspace(),
                                                                       x => CanSaveActiveWorkspace()));
            }
        }

        private void ShowContractTransferRepository()
        {
            FindOrCreateThenShowWorkspace<DocumentTransferActsRepositoryViewModel>();
        }

        private bool CanOpenContractDetails()
        {
            var ws = ActiveWorkspace as IContractdocRefHolder;
            return ws != null && ws.ContractdocId.HasValue;
        }

        /// <summary>
        ///     Событие запроса сохранения рабочей области перед проведением следующей операции
        ///     Аргумент принимает значение Истина, если пользователь согласился на сохранение рабочей области и будет выполнять
        ///     следующую операцию. Ложь - при отказе от продолжения
        /// </summary>
        public event EventHandler<EventParameterArgs<bool>> RequestSaveModifiedWorkspace;

        public void OnRequestSaveModifiedWorkspace(EventParameterArgs<bool> e)
        {
            var handler = RequestSaveModifiedWorkspace;
            if (handler != null) handler(this, e);
        }

        private void OpenContractSchedule()
        {
            OpenContractWorkspace<ScheduleViewModel>(typeof(StageResultsViewModel), typeof(ActsViewModel), typeof(ScheduleViewModel));
        }

        /// <summary>
        ///     Запрашивает необходимость сохранения рабочей области через событие и сохраняет, если событие вернуло true
        /// </summary>
        /// <param name="ws">Рабочая область</param>
        /// <returns>Истина, если изменения рабочей области сохранены или рабочая область не была модифицирована с момента последнего сохранения </returns>
        private bool RequestAndSaveWorkspace(WorkspaceViewModel ws)
        {
            Contract.Requires(ws != null);
            if (!ws.IsModified) return true;
            var args = new EventParameterArgs<bool>(false);
            OnRequestSaveModifiedWorkspace(args);
            if (!args.Parameter) return false;
            SaveActiveWorkspace();
            return true;
        }

        private IEnumerable<ContractWorkspaceViewModel> FindConflictingContractWorkspaces(
            params Type[] conflictingWith)
        {
            if (conflictingWith == null || conflictingWith.Length == 0) yield break;

            var mainContractId = (ActiveWorkspace as IContractdocRefHolder).Return(x => x.Maincontractid, default(long?));

            foreach (var contractWorkspaceViewModel in conflictingWith)
            {
                foreach (var item in FindMainContractWorkspacesOfType(mainContractId, contractWorkspaceViewModel))
                    yield return item;
            }
        }

        /// <summary>
        ///     Получает рабочие области заданного типа, относящиеся к заданной группе договоров
        /// </summary>
        /// <param name="mainContractId">Идентификатор группы договоров (идентификатор генерального договора)</param>
        /// <param name="contractWorkspaceViewModel">Тип рабочей области для поиска</param>
        /// <returns>Коллекция рабочих областей, удовлетворяющих условию</returns>
        private IEnumerable<ContractWorkspaceViewModel> FindMainContractWorkspacesOfType(long? mainContractId,
                                                                                         Type contractWorkspaceViewModel)
        {
            return
                Workspaces.OfType<ContractWorkspaceViewModel>()
                          .Where(
                              x =>
                              x.ContractObject.Return(c => c.Maincontractid, default(long?)) == mainContractId &&
                              x.GetType() == contractWorkspaceViewModel);
        }

        /// <summary>
        ///     Открывает или создаёт новую рабочую область ContractWorkspaceViewModel, учитывая совместимостьтипов областей, которые уже открыты
        ///     Если одна из несовместимых областей открыта, то инициируется её закрытие с сохранением результатов, если она была модифицирована
        /// </summary>
        /// <typeparam name="TWorkspace">Тип рабочей области</typeparam>
        /// <param name="conflictingWith">Коллекция типов рабочих областей, с которыми имеется конфликт при открытии</param>
        private void OpenContractWorkspace<TWorkspace>(params Type[] conflictingWith)
            where TWorkspace : ContractWorkspaceViewModel
        {
            var contractdocHolder = ActiveWorkspace.CastTo<IContractdocRefHolder>();
            Contract.Assert(ActiveWorkspace != null);
            var oid = contractdocHolder.ContractdocId;
            Contract.Assert(oid.HasValue);
            var ws = FindContractWorkspace<TWorkspace>(oid.Value);
            if (ws == null)
            {
                // Запросить сохранение рабочей области, если были изменения 
                if (!RequestAndSaveWorkspace(ActiveWorkspace)) return;

                // Повторное получение идентификатора договора после сохранения.
                // Требуется, если объект был зарегестрирован на вставку и не имел первичного ключа
                oid = contractdocHolder.ContractdocId;

                // Закрыть все конфликтующие рабочие области, предварительно запросив сохранение данных
                var conflicts = FindConflictingContractWorkspaces(conflictingWith);
                foreach (var contractWorkspaceViewModel in conflicts.ToList())
                {
                    if (RequestAndSaveWorkspace(contractWorkspaceViewModel))
                        contractWorkspaceViewModel.CloseCommand.Execute(null);
                    else
                        return;
                }

                // Создать новую рабочую область 
                var repository = RepositoryFactory.CreateContractRepository();
                ws = (TWorkspace) Activator.CreateInstance(typeof (TWorkspace), repository);
                Debug.Assert(oid != null, "oid != null");
                ws.ContractObject = repository.GetContractdoc(oid.Value);
                AddWorkspace(ws);
            }
            SetActiveWorkspace(ws);
        }

        /// <summary>
        ///     Открыть рабочую область актов
        /// </summary>
        private void OpenContractActs()
        {
            OpenContractWorkspace<ActsViewModel>(typeof (ScheduleViewModel), typeof (StageResultsViewModel));
        }

        private T FindContractWorkspace<T>(long contractdocId) where T : ContractWorkspaceViewModel
        {
            return Workspaces.OfType<T>().SingleOrDefault(x => x.ContractObject.Id == contractdocId);
        }

        private void ExportRegistry()
        {
            CurrentContractRepositoryViewModel.ExportRegistry();
        }

        protected override void OnDispose()
        {
            foreach (var workspaceViewModel in Workspaces)
            {
                workspaceViewModel.Dispose();
            }

            base.OnDispose();
        }

        private void Logon()
        {
            var dlg = new ServerConnectionDialog();
            dlg.ShowDialog();
        }

        public event EventHandler<DeleteContractArgs> RequestDeleteContract;

        private void SendDeleteContract()
        {
            Contract.Requires(SelectedContract != null);
            bool canceled = false;
            if (RequestDeleteContract != null)
            {
                var args = new DeleteContractArgs(SelectedContract);
                RequestDeleteContract(this, args);
                canceled = args.Cancel;
            }

            if (canceled) return;

            if (ActiveWorkspace is ContractRepositoryViewBasedViewModel)
                ActiveWorkspace.CastTo<ContractRepositoryViewBasedViewModel>().DeleteContract(SelectedContract);


            OnPropertyChanged(() => SelectedContract);
        }

        private void OpenToolsDialog()
        {
            var dlg = new PropertyDialog {PropertyObject = new PropertiesDecorator()};
            if (dlg.ShowDialog().GetValueOrDefault())
                Settings.Default.Save();
            else
                Settings.Default.Reload();
        }

        private RelayCommand CreateNewContractCommand(NewContractdocType newContractdocType, ref RelayCommand instance)
        {
            return instance ?? (instance = new RelayCommand(
                                               param =>
                                               NewContract(new NewContractdocParams(SelectedContract,
                                                                                    newContractdocType)),
                                               x => SelectedContract!=null));
        }


        private bool CanSaveActiveWorkspace()
        {
            return (ActiveWorkspace != null) && (ActiveWorkspace.SaveCommand.CanExecute(null));
        }

        #endregion // Commands

        #region Properties

        public WorkspaceViewModel ActiveWorkspace
        {
            get { return Workspaces.SingleOrDefault(x => x.IsActive); }
        }

        public bool RepositoryShowed
        {
            set
            {
                if (value)
                {
                    ShowContractRepository();
                }
            }
        }

        #endregion

        #region Workspaces

        private bool _fInUpdate;

        /// <summary>
        ///     Используется для задания модели представления с которой был совершён переход
        ///     Служит для перехода на неё в случае, если пользователь закрыл вкладку,
        /// </summary>
        private WorkspaceViewModel _workspaceToBack;

        /// <summary>
        ///     Returns the collection of available workspaces to display.
        ///     A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        public override string Error
        {
            get { return ActiveWorkspace != null ? ActiveWorkspace.Error : null; }
        }

        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.NewItems)
                {
                    workspace.Closable.RequestClose += OnWorkspaceRequestClose;
                    workspace.PropertyChanged += OnPropertyChanged;
                    workspace.PropertyChanging += OnWorkspacePropertyChanging;
                }
            }


            if (e.OldItems != null && e.OldItems.Count != 0)
            {
                foreach (WorkspaceViewModel workspace in e.OldItems)
                {
                    workspace.Closable.RequestClose -= OnWorkspaceRequestClose;
                    workspace.PropertyChanged -= OnPropertyChanged;
                    workspace.PropertyChanging -= OnWorkspacePropertyChanging;
                }
            }
           
        }


        private void OnWorkspacePropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName != "IsActive") return;
            var ws = (sender as WorkspaceViewModel);
            Contract.Assert(ws != null);


            // Запомнить модель пердставления для возврата
            _workspaceToBack = ActiveWorkspace;

            // Предотвращает вход в обработку, через неявный вызов события в цикле (ws.IsActive)
            if (_fInUpdate) return;

            _fInUpdate = true;
            try
            {
                // Если модель рабочей области сейчас не активна (перед сменой состояния), то 
                // требуется сделать неактивными текущую активную рабочую область 
                if (ws.IsActive) return;
                foreach (var workspaceViewModel in Workspaces.Where(x => x != ws))
                {
                    // Для всех рабочих областей, кроме той, что инициировала событие
                    workspaceViewModel.IsActive = false;
                }
            }
            finally
            {
                _fInUpdate = false;
            }
        }

        public event EventHandler<CancelEventArgs> QueryCloseWorkspace;

        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            var workspace = sender as WorkspaceViewModel;
            if (workspace != null)
            {
                var args = new CancelEventArgs(false);
                OnQueryCloseWorkspace(workspace, args);
                if (!args.Cancel)
                    DisposeWorkspace(workspace);
            }
        }

        private void OnQueryCloseWorkspace(WorkspaceViewModel workspace, CancelEventArgs args)
        {
            if (QueryCloseWorkspace != null)
                QueryCloseWorkspace(workspace, args);
        }

        private void DisposeWorkspace(WorkspaceViewModel workspace)
        {
            workspace.Dispose();
            Workspaces.Remove(workspace);
            _workspaceToBack.Do(x => x.IsActive = true);
            //OnPropertyChanged(() => ActiveWorkspace);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "IsActive") return;

            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ACTIVE_WORKSPACE_CHANGED,
                                          sender.CastTo<WorkspaceViewModel>());

            OnPropertyChanged(() => ActiveWorkspace);
            OnPropertyChanged(() => Error);
        }

        [MediatorMessageSink(RequestRepository.REQUEST_ERROR_CHANGED, ParameterType = typeof (String))]
        public void ErrorChanged(String errorStr)
        {
            if (errorStr == null) throw new ArgumentNullException("errorStr");
            if (ActiveWorkspace is ContractViewModel)
            {
                OnPropertyChanged(() => Error);
            }
        }

        #endregion // Workspaces

        #region Private Helpers

        /// <summary>
        ///     Получить рабочую область реестра договоров
        /// </summary>
        public ContractRepositoryViewBasedViewModel ContractRepositoryViewBased
        {
            get
            {
                var workspace = Workspaces.OfType<ContractRepositoryViewBasedViewModel>().FirstOrDefault();
                if (workspace == null)
                {
                    var contractRepository = RepositoryFactory.CreateContractRepository();
                    Contract.Assert(contractRepository != null);
                    workspace = new ContractRepositoryViewBasedViewModel(contractRepository);
                    AddWorkspace(workspace);
                }
                return workspace;
            }
        }

        private bool CanPrepaymentdocumenttypesShow
        {
            get { return true; }
        }

        private bool CanRolesShow
        {
            get { return true; }
        }

        private bool CanSightfuncpersonschemesShow
        {
            get { return true; }
        }

        private bool CanContractorpropertiesShow
        {
            get { return true; }
        }

        private bool CanAuthoritiesShow
        {
            get { return true; }
        }

        private bool CanTransferacttypedocumentsShow
        {
            get { return true; }
        }

        private bool CanActtypesShow
        {
            get { return true; }
        }

        private bool CanPersonsShow
        {
            get { return true; }
        }

        private bool CanDocumentsShow
        {
            get { return true; }
        }

        private bool CanTransferacttypesShow
        {
            get { return true; }
        }

        private bool CanEmployeesShow
        {
            get { return true; }
        }

        private bool CanDepartmentsShow
        {
            get { return true; }
        }

        private bool CanTroublesShow
        {
            get { return true; }
        }

        private bool CanContractorpositionShow
        {
            get { return true; }
        }

        private bool CanEconomefficiencytypesShow
        {
            get { return true; }
        }

        private bool CanEconomefficiencyparametersShow
        {
            get { return true; }
        }

        private bool CanEfficienceparameterTypesShow
        {
            get { return true; }
        }

        private bool CanCurrencymeasuresShow
        {
            get { return true; }
        }

        private bool CanWorktypesShow
        {
            get { return true; }
        }

        private bool CanFunctionalcustomersShow
        {
            get { return true; }
        }

        private bool CanFunctionalcustomertypesShow
        {
            get { return true; }
        }

        private bool CanTroublesregistresShow
        {
            get { return true; }
        }

        private bool CanPositionsShow
        {
            get { return true; }
        }

        private bool CanPropertiesShow
        {
            get { return true; }
        }

        private bool CanFunccustomerpersonsShow
        {
            get { return true; }
        }

        private bool CanContractortypesShow
        {
            get { return true; }
        }

        private bool CanContractorsShow
        {
            get { return true; }
        }

        private bool CanNtpviewsShow
        {
            get { return true; }
        }

        private bool CanYearreportcolorsShow
        {
            get { return true; }
        }

        private bool CanNtpsubviewsShow
        {
            get { return true; }
        }

        private bool CanNdsesShow
        {
            get { return true; }
        }

        private bool CanRegionsShow
        {
            get { return true; }
        }

        private bool CanNdsalgorithmsShow
        {
            get { return true; }
        }

        private bool CanDegreesShow
        {
            get { return true; }
        }

        private bool CanCurrenciesShow
        {
            get { return true; }
        }

        private bool CanContractTypesShow
        {
            get { return true; }
        }

        private bool CanContractStatesShow
        {
            get { return true; }
        }

        /// <summary>
        ///     Получает коллекцию модифицированных рабочих областей
        /// </summary>
        public IEnumerable<WorkspaceViewModel> ModifiedWorkspaces
        {
            get { return Workspaces.Where(x => x.IsModified); }
        }

        public ICommand ShowRelationsEditorCommand
        {
            get { return new RelayCommand(x => OnShowRelationsEditor(), x => ContractSelected); }
        }

        public void ShowContractRepository(int position = -1)
        {
            SetActiveWorkspace(ContractRepositoryViewBased, position);
        }

        /// <summary>
        /// Метод ищет рабочую область заданного типа, в случае удачи активирует её. Если рабочая область не найдена,
        /// метод создаёт новый экземпляр и добавляет его в коллекцию рабочих областей
        /// </summary>
        /// <typeparam name="T">Тип модели представления рабочей области</typeparam>
        /// <param name="position">Порядковый номер в коллекции областей</param>
        /// <returns>Экземпляр заданной рабочей области</returns>
        private void FindOrCreateThenShowWorkspace<T>(int position = -1) where T : WorkspaceViewModel
        {
            var workspace = Workspaces.FirstOrDefault(vm => vm is T) as T;

            if (workspace == null)
            {
                var contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);
                workspace = Activator.CreateInstance(typeof (T), contractRepository) as T;
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace, position);
        }

        internal void ShowActRepository(int position = -1)
        {
            var workspace =
                Workspaces.FirstOrDefault(vm => vm is ActRepositoryViewBasedViewModel) as
                ActRepositoryViewBasedViewModel;

            if (workspace == null)
            {
                var contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);
                workspace = new ActRepositoryViewBasedViewModel(contractRepository);
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace, position);
        }


        private void ShowDisposalsRepository()
        {
            var workspace =
                Workspaces.FirstOrDefault(vm => vm is DisposalsRepositoryViewModel) as DisposalsRepositoryViewModel;

            if (workspace == null)
            {
                var contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);
                workspace = new DisposalsRepositoryViewModel(contractRepository);
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace);
        }

        private void ShowOrdersRepository()
        {
            var workspace = Workspaces.FirstOrDefault(vm => vm is OrderRepositoryViewModel) as OrderRepositoryViewModel;

            if (workspace == null)
            {
                var contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);
                workspace = new OrderRepositoryViewModel(contractRepository);
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace);
        }


        private void PrepaymentdocumenttypesShow()
        {
            ShowCatalog(CatalogType.Prepaymentdocumenttype);
        }

        private void RolesShow()
        {
            ShowCatalog(CatalogType.Role);
        }

        private void SightfuncpersonschemesShow()
        {
            ShowCatalog(CatalogType.Sightfuncpersonscheme);
        }

        private void ContractorpropertiesShow()
        {
            ShowCatalog(CatalogType.Contractorpropertiy);
        }

        private void AuthoritiesShow()
        {
            ShowCatalog(CatalogType.Authority);
        }

        private void ActtypesShow()
        {
            ShowCatalog(CatalogType.Acttype);
        }

        private void TransferacttypedocumentsShow()
        {
            ShowCatalog(CatalogType.Transferacttypedocument);
        }

        private void PersonsShow()
        {
            ShowCatalog(CatalogType.Person);
        }

        private void DocumentsShow()
        {
            ShowCatalog(CatalogType.Document);
        }

        private void TransferacttypesShow()
        {
            ShowCatalog(CatalogType.Transferacttype);
        }

        private void EmployeesShow()
        {
            ShowCatalog(CatalogType.Employee);
        }

        private void DepartmentsShow()
        {
            ShowHierarchicalCatalog(CatalogType.Department);
        }

        private void TroublesShow()
        {
            ShowHierarchicalCatalog(CatalogType.Trouble);
        }

        private void ContractorpositionShow()
        {
            ShowCatalog(CatalogType.Contractorposition);
        }

        private void EconomefficiencytypesShow()
        {
            ShowCatalog(CatalogType.Economefficiencytype);
        }

        private void EconomefficiencyparametersShow()
        {
            ShowCatalog(CatalogType.Economefficiencyparameter);
        }

        private void EfficienceparameterTypesShow()
        {
            ShowCatalog(CatalogType.Efficienceparametertype);
        }

        private void CurrencymeasuresShow()
        {
            ShowCatalog(CatalogType.Currencymeasure);
        }

        private void WorktypesShow()
        {
            ShowCatalog(CatalogType.Worktype);
        }

        private void FunctionalcustomersShow()
        {
            ShowHierarchicalCatalog(CatalogType.Functionalcustomer);
        }

        private void FunctionalcustomertypesShow()
        {
            ShowCatalog(CatalogType.Functionalcustomertype);
        }

        private void TroublesregistresShow()
        {
            ShowCatalog(CatalogType.Troublesregistry);
        }

        private void PositionsShow()
        {
            ShowCatalog(CatalogType.Position);
        }

        private void PropertiesShow()
        {
            ShowCatalog(CatalogType.Property);
        }

        private void FunccustomerpersonsShow()
        {
            ShowCatalog(CatalogType.Funccustomerperson);
        }

        private void ContractortypesShow()
        {
            ShowCatalog(CatalogType.Contractortype);
        }

        private void ContractorsShow()
        {
            ShowCatalog(CatalogType.Contractor);
        }

        private void NtpviewsShow()
        {
            ShowCatalog(CatalogType.Ntpview);
        }

        private void YearreportcolorsShow()
        {
            ShowCatalog(CatalogType.Yearreportcolor);
        }

        private void LocationsShow()
        {
            ShowCatalog(CatalogType.Location);
        }

        private void MissiveTypesShow()
        {
            ShowCatalog(CatalogType.MissiveType);
        }

        private void ApprovalGoalsShow()
        {
            ShowCatalog(CatalogType.ApprovalGoal);
        }

        private void ApprovalStatesShow()
        {
            ShowCatalog(CatalogType.ApprovalState);
        }


        private void NtpsubviewsShow()
        {
            ShowCatalog(CatalogType.Ntpsubview);
        }

        private void NdsesShow()
        {
            ShowCatalog(CatalogType.Nds);
        }

        private void RegionsShow()
        {
            ShowCatalog(CatalogType.Region);
        }

        private void SaveLogFile()
        {
            var contractRepository = RepositoryFactory.CreateContractRepository();
            var w = new OraExpWindow(contractRepository);
            w.ShowDialog();
        }

        private void NdsalgorithmsShow()
        {
            ShowCatalog(CatalogType.Ndsalgorithm);
        }

        private void DegreesShow()
        {
            ShowCatalog(CatalogType.Degree);
        }

        private void CurrenciesShow()
        {
            ShowCatalog(CatalogType.Currency);
        }

        private void ContractTypesShow()
        {
            ShowCatalog(CatalogType.ContractType);
        }

        private void ContractStatesShow()
        {
            ShowCatalog(CatalogType.ContractState);
        }

        private void ShowCatalog(CatalogType type)
        {
            App.LogMessage("Начало: ShowCatalog()");
            var workspace =
                Workspaces.Where(x => x is CatalogViewModel).FirstOrDefault(x =>
                    {
                        var catalogViewModel = x as CatalogViewModel;
                        return catalogViewModel != null && catalogViewModel.CatalogType == type;
                    }) as CatalogViewModel;

            if (workspace == null)
            {
                var contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);
                workspace = new CatalogViewModel(contractRepository)
                    {
                        CatalogType = type
                    };
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace);
            App.LogMessage("Конец: ShowCatalog()");
        }


        private void ShowHierarchicalCatalog(CatalogType type)
        {
            App.LogMessage("Начало: ShowHierarchicalCatalog()");
            var workspace =
                Workspaces.Where(x => x is HierarchicalCatalogViewModel).FirstOrDefault(x =>
                    {
                        var hierarchicalCatalogViewModel = x as HierarchicalCatalogViewModel;
                        return hierarchicalCatalogViewModel != null && hierarchicalCatalogViewModel.CatalogType == type;
                    }) as
                HierarchicalCatalogViewModel;

            if (workspace == null)
            {
                IContractRepository contractRepository = RepositoryFactory.CreateContractRepository();
                Contract.Assert(contractRepository != null);

                workspace = new HierarchicalCatalogViewModel(contractRepository)
                    {
                        CatalogType = type
                    };
                AddWorkspace(workspace);
            }

            SetActiveWorkspace(workspace);
            App.LogMessage("Конец: ShowHierarchicalCatalog()");
        }


        public event EventHandler<ContractArgs> ContractCreateRefused;

        public void OnContractCreateRefused(ContractArgs e)
        {
            var handler = ContractCreateRefused;
            if (handler != null) handler(this, e);
        }

        [MediatorMessageSink(RequestRepository.REQUEST_NEW_CONTRACTDOC, ParameterType = typeof (NewContractdocParams))]
        private void NewContract(NewContractdocParams newContractdocParams)
        {
            Contract.Requires(newContractdocParams != null);
            App.LogMessage("Начало: NewContract()");

            ContractViewModel workspace = null;

            if (newContractdocParams.Contractdoc != null)
                workspace = FindContractWorkspaceByMainContractdoc(newContractdocParams.Contractdoc.MainContract);

            if (workspace != null)
            {
                
                if (AppMessageBox.Show(
                    string.Format(
                        "Следует завершить работу с группой договоров {0} перед тем как создавать в ней новый договор.",
                        workspace.Contractdoc.MainContract.Num),
                    MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    SetActiveWorkspace(workspace);
                    return;
                }
            }

            var contractRepository = RepositoryFactory.CreateContractRepository();
            workspace = new ContractViewModel(contractRepository);
            Contractdoc contractdoc = null;

            if (CannotCreateSubreport(newContractdocParams))
            {
                OnContractCreateRefused(new ContractArgs(newContractdocParams.Contractdoc));
                return;
            }


            switch (newContractdocParams.ContractdocType)
            {
                case NewContractdocType.NewGeneral:
                    contractdoc = contractRepository.NewContractdoc();
                    break;

                    // И ДС к генеральному и ДС к СД сначала проходят создание нового соглашения к договору
                    // Если создаётся ДС к генеральному, то на этом заканчивается операция
                    // Если же создаётся ДС к СД, то производится вызов MakeAgreementToSubgeneral,
                    // В котором производится связывание с договором, который был возвращён в ответ на 
                    // запрос через событие SelectGeneralContractToBindStages
                case NewContractdocType.NewAgreement:
                    Contract.Assert(newContractdocParams.Contractdoc != null);
                    var actual = newContractdocParams.Contractdoc.Actual;
                    var agreementParams = new NewContractdocParams(actual, NewContractdocType.NewAgreement);
                    contractdoc = CreateNewAgreementContractdoc(contractRepository, agreementParams);
                    // Если создавали ДС к генеральному - выход
                    if (newContractdocParams.Contractdoc.IsGeneral) break;
                    // Передаётся созданное ДС для привязки к генеральному договору
                    var subGeneralAgr = MakeAgreementToSubgeneral(contractRepository, contractdoc);
                    // Если пользователь не выбрал генерального договора, то разрушить созданную рабочую область договора  
                    if (subGeneralAgr == null)
                    {
                        workspace.Dispose();
                        return;
                    }
                    break;
                case NewContractdocType.NewSubgeneral:
                    contractdoc = CreateNewSubgeneralContractdoc(contractRepository, newContractdocParams);
                    break;
            }
            Contract.Assert(contractdoc != null);
            workspace.WrappedDomainObject = contractdoc;
            AddWorkspace(workspace);


            SetActiveWorkspace(workspace);

            //Сохранить договор как только он создан для получения его Id
            //SaveActiveWorkspace();      


            App.LogMessage("Конец: NewContract()");
        }

        private Contractdoc MakeAgreementToSubgeneral(IContractRepository repository, Contractdoc contractdoc)
        {
            Contract.Assert(repository != null);
            Contract.Assert(contractdoc != null);
            var args =
                new EventParameterArgs<SelectContractdocArgs>(new SelectContractdocArgs(contractdoc,
                                                                                        FetchGeneralContracts(
                                                                                            repository, contractdoc)));
            OnSelectGeneralContractToBindStages(args);
            if (args.Parameter.General != null)
            {
                BindSubgeneral(repository, args.Parameter.General, contractdoc);
                return contractdoc;
            }
            return null;
        }

        private List<Contractdoc> FetchGeneralContracts(IContractRepository repository, Contractdoc contractdoc)
        {
            Contract.Assert(repository != null);
            Contract.Assert(contractdoc != null);
            Contract.Assert(contractdoc.OriginalContract != null); // Функция работает только для ДС
            return
                repository.Contracts.Where(
                    x => x.IsGeneral && x.MainContract == contractdoc.OriginalContract.MainContract).ToList();
        }

        /// <summary>
        ///     Событие для запроса договора, с которым будет производиться связка
        /// </summary>
        public event EventHandler<EventParameterArgs<SelectContractdocArgs>> SelectGeneralContractToBindStages;

        protected virtual void OnSelectGeneralContractToBindStages(EventParameterArgs<SelectContractdocArgs> e)
        {
            var handler = SelectGeneralContractToBindStages;
            if (handler != null) handler(this, e);
        }

        private bool CannotCreateSubreport(NewContractdocParams newContractdocParams)
        {
            return SelectedContract != null && !SelectedContract.IsGeneral &&
                   newContractdocParams.ContractdocType == NewContractdocType.NewSubgeneral;
        }

        private Contractdoc CreateNewSubgeneralContractdoc(IContractRepository contractRepository,
                                                           NewContractdocParams newContractdocParams)
        {
            var general = contractRepository.GetContractdoc(newContractdocParams.Contractdoc.Id);
            Contract.Assert(general != null);
            contractRepository.DebugPrintRepository();
            var contractdoc = contractRepository.NewContractdoc();

            BindSubgeneral(contractRepository, general, contractdoc);
            return contractdoc;
        }

        private static void BindSubgeneral(IContractRepository contractRepository, Contractdoc general,
                                           Contractdoc contractdoc)
        {
            var ch = new Contracthierarchy
                {
                    GeneralContractdoc = general,
                    SubContractdoc = contractdoc
                };

            general.Contracthierarchies.Add(ch);
            contractdoc.Generalcontracthierarchies.Add(ch);

            contractRepository.DebugPrintRepository();
        }

        private Contractdoc CreateNewAgreementContractdoc(IContractRepository contractRepository,
                                                          NewContractdocParams newContractdocParams)
        {
            var original = contractRepository.GetContractdoc(newContractdocParams.Contractdoc.Id);
            Contract.Assert(original != null);
            var agreement = (Contractdoc) original.CloneRecursively(null, null);
            original.AddAgreement(agreement);
            return agreement;
        }


        [MediatorMessageSink(RequestRepository.REQUEST_OPEN_CONTRACTDOC, ParameterType = typeof (Contractdoc))]
        private void OpenContract(Contractdoc selectedContract)
        {
            Contract.Requires(selectedContract != null);
            App.LogMessage("Начало: OpenContract()");


            // Найти рабочую область по переданному договору
            var workspace = FindContractWorkspace(selectedContract);

            // Если такой договор не открыт, то получить рабочую область для его генерального догоаора
            if (workspace == null)
                workspace = FindContractWorkspaceByMainContractdoc(selectedContract.MainContract);
            else
            {
                // Сделать активным рабочую область договора
                SetActiveWorkspace(workspace);
                return;
            }

            // Если такой договор не открыт в рабочих областях, то создать новую рабочую область договора
            // и назначить её активной
            if (workspace == null)
            {
                workspace = ContractContractViewModelWorkspace(selectedContract);
                AddWorkspace(workspace);
                SetActiveWorkspace(workspace);
            }
            else
            {
                // Для предотвращения конфликтов изменения одних и тех же данных в разных рабочих областях, реализация открытия вкладок предотвращает одновременную работу 
                // с договорами одной групп на в разных рабочих областях
                // Если рабочая область генерального договора найдена, то перед открытием новой области договора рабочая область закрывается

                // Если есть модификации в рабочей области договора, то производится переход
                // в рабочую область, где пользователь может либо сохранить изменения, либо просто закрыть ее
                if (workspace.IsModified &&
                    AppMessageBox.Show(
                        string.Format(
                            "Группа договоров {0} уже открыта и в ней не завершена работа с договором {1}. Перед открытием договора {2} сохраните изменения в договоре {1}.",
                            workspace.Contractdoc.MainContract.Num, workspace.Contractdoc.Num, selectedContract.Num),
                        MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
                {
                    SetActiveWorkspace(workspace);
                }
                    // Если рабочая область не модифицирована
                else if (!workspace.IsModified)
                {
                    // Закрыть рабочую область и создать новую под договор
                    workspace.CloseCommand.Execute(workspace);
                    var newworkspace = ContractContractViewModelWorkspace(selectedContract);
                    AddWorkspace(newworkspace);
                    SetActiveWorkspace(newworkspace);
                }
            }

            App.LogMessage("Конец: OpenContract()");
        }

        private static ContractViewModel ContractContractViewModelWorkspace(Contractdoc selectedContract)
        {
            if (selectedContract == null) throw new ArgumentNullException("selectedContract");
            var contractRepository = RepositoryFactory.CreateContractRepository();
            var workspace = new ContractViewModel(contractRepository)
                {WrappedDomainObject = contractRepository.GetContractdoc(selectedContract.Id)};
            Contract.Assume(workspace.WrappedDomainObject != null);
            return workspace;
        }

        private void AddWorkspace(WorkspaceViewModel workspace)
        {
            Contract.Assert(workspace != null);
            Contract.Assert(!Workspaces.Contains(workspace));

            if (workspace is MainViewModelWorkspace)
                (workspace as MainViewModelWorkspace).MainViewModel = this;

            Workspaces.Add(workspace);
        }


        public event EventHandler<ContractArgs> RequestEditRelation;

        public void OnRequestEditRelation(ContractArgs e)
        {
            EventHandler<ContractArgs> handler = RequestEditRelation;
            if (handler != null) handler(this, e);
        }

        private void OnShowRelationsEditor()
        {
            OnRequestEditRelation(new ContractArgs(SelectedContract));
        }


        private ContractViewModel FindContractWorkspace(Contractdoc contractdoc)
        {
            return
                Workspaces.OfType<ContractViewModel>().SingleOrDefault(
                    x =>
                    x.Contractdoc != null && ((x.Contractdoc == contractdoc) || (x.Contractdoc.Id == contractdoc.Id)));
        }

        /// <summary>
        ///     Получает рабочую область генерального договора для заданного договора
        /// </summary>
        /// <param name="contractdoc"></param>
        /// <returns></returns>
        private ContractViewModel FindContractWorkspaceByMainContractdoc(Contractdoc contractdoc)
        {
            return
                Workspaces.OfType<ContractViewModel>().SingleOrDefault(
                    x =>
                    x.Contractdoc != null &&
                    ((x.Contractdoc.MainContract == contractdoc) || (x.Contractdoc.MainContract.Id == contractdoc.Id)));
        }

        public void SetActiveWorkspace(WorkspaceViewModel workspace, int position = -1)
        {
            if (position > -1)
            {
                var nowPos = Workspaces.IndexOf(workspace);
                if (nowPos != position && position < Workspaces.Count)
                    Workspaces.Move(nowPos, position);
            }

            workspace.IsActive = true;
        }

        #endregion // Private Helpers

        public ICommand CloseCommand
        {
            get { return Closable.CloseCommand; }
        }

        private static string FileVersionInfo
        {
            get { return App.FileVersionInfo; }
        }

        private static string AssemblyVersionInfo
        {
            get { return App.AssemblyVersionInfo; }
        }

        private static string ProductInfo
        {
            get { return App.ProductInfo; }
        }

        public static string CompanyInfo
        {
            get { return App.CompanyInfo; }
        }

        public bool FastLoad { get; set; }

        #region Fields

        private ObservableCollection<WorkspaceViewModel> _workspaces;

        #endregion // Fields

        public event EventHandler<WrappedObjectCommandArgs<Contractdoc>> NewAgreementCreated;

        public void OnNewAgreementCreated(WrappedObjectCommandArgs<Contractdoc> e)
        {
            var handler = NewAgreementCreated;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<WrappedObjectCommandArgs<Contractdoc>> NewSubgeneralCreated;


        public void OnNewSubgeneralCreated(WrappedObjectCommandArgs<Contractdoc> e)
        {
            var handler = NewSubgeneralCreated;
            if (handler != null) handler(this, e);
        }

        public void SaveWorkspace(WorkspaceViewModel ws)
        {
            Contract.Assert(ws != null);

            try
            {
                ws.SaveCommand.Execute(null);
                ViewMediator.NotifyColleagues(RequestRepository.REQUEST_SAVE_WORKSPACE, ws);
            }

            catch (Exception exception)
            {
                var wrapper =
                    new SaveWorkspaceException(string.Format(Resources.SaveWorkspaceFailed,
                                                             exception.Return(x => x.Message, string.Empty)), exception);
                throw wrapper;
            }
        }

        private void SaveActiveWorkspace()
        {
            SaveWorkspace(ActiveWorkspace);
        }


        public ICommand QuitApplicationCommand
        {
            get
            {
                return new RelayCommand((x)=>QuitApplication());
            }
        }
        public ICommand SaveWorkspacesAndQuitApplicationCommand
        {
            get
            {
                return new RelayCommand((x) => SaveWorkspacesAndQuitApplication());
            }
        }

        private void SaveWorkspacesAndQuitApplication()
        {
            Workspaces.Where(x=>x.IsModified).Apply(x=>x.SaveChanges());
            QuitApplication();
        }

        private void QuitApplication()
        {
            Application.Current.Shutdown(0);
        }
    }
}