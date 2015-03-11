#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using MContracts.Classes;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using MContracts.Dialogs;
using MContracts.ViewModel.Helpers;
using MediatorLib;

#endregion

namespace MContracts.ViewModel
{
    /// <summary>
    ///   Модель представления для ввода данных по календарным планам договора
    /// </summary>
    public class ScheduleViewModel : ContractWorkspaceViewModel,  IContractdocRefHolder,  IContractCaption
    {
        private RelayCommand _autoCreateScheduleCommand;
        private RelayCommand _closeStageCommand;
        private RelayCommand _createResultCommand;
        private RelayCommand _createScheduleCommand;
        private RelayCommand _createStageCommand;
        private RelayCommand _deleteResultCommand;
        private RelayCommand _deleteScheduleCommand;
        private RelayCommand _deleteStageCommand;
        private RelayCommand _downStageCommand;
        private RelayCommand _indentStageCommand;
        private RelayCommand _linkStageCommandGeneral;
        private RelayCommand _linkStageCommandSubcontracts;
        private RelayCommand _orderstagescommand;
        private RelayCommand _outdentStageCommand;

        private ObservableCollection<Schedulecontract> _schedulesBindingList;
        private RelayCommand _showContributorsImporterDialogCommand;
        private RelayCommand _showImporterDialogCommand;
        private IBindingList _stagesBindingList;
        private RelayCommand _unbindScheduleCommand;
        private RelayCommand _upStageCommand;
        private RelayCommand _autobindscheduleoriginCommand;
        private RelayCommand _simpledeletestagecommand;


        public ScheduleViewModel(IContractRepository repository)
            : base(repository)
        {
            ViewMediator.Register(this);
        }

        public override bool IsClosable
        {
            get { return true; }
        }

        /// <summary>
        ///   Событие при попытке удаления этапа календарного плана
        /// </summary>
        public event EventHandler<CancelEventArgs> DeletingStage;

        [MediatorMessageSink(RequestRepository.REFRESH_REFRESH_APPROVAL, ParameterType = typeof (ISupportStateApproval))
        ]
        public void ApprovalUpdated(ISupportStateApproval approval)
        {
            if (approval is Stage)
                StageApprovalUpdated(approval);
            if (approval is Stageresult)
                StageResultApprovalUpdated(approval);
        }

        private void StageResultApprovalUpdated(ISupportStateApproval approval)
        {
            var stageResult = approval as Stageresult;
            if (stageResult == null) return;
            var stageResultApproval =
                Repository.TryGetContext().Approvalstates.SingleOrDefault(x => stageResult.Approvalstate.Id == x.Id);
            var myStageResult = Repository.TryGetContext().Stageresults.SingleOrDefault(x => x.Id == stageResult.Id);

            Repository.Refresh(RefreshMode.KeepChanges, myStageResult);

            if (myStageResult != null && stageResultApproval != null)
                myStageResult.Approvalstate = stageResultApproval;
        }

        private void StageApprovalUpdated(ISupportStateApproval approval)
        {
            var stage = approval as Stage;
            if (stage == null) return;
            var stageApproval =
                Repository.TryGetContext().Approvalstates.SingleOrDefault(x => stage.Approvalstate.Id == x.Id);
            var myStage = Repository.TryGetContext().Stages.SingleOrDefault(x => x.Id == stage.Id);

            Repository.Refresh(RefreshMode.KeepChanges, myStage);

            if (myStage != null && stageApproval != null)
                myStage.Approvalstate = stageApproval;
        }

        [MediatorMessageSink(RequestRepository.REFRESH_RESULT, ParameterType = typeof (Stage))]
        public void AddedResult(Stage stage)
        {
            if (stage != null)
            {
                stage.OnResultsChanged();
            }
        }


     
        public void AddedAct1(Act act)
        {
            _stagesBindingList = null;
            OnPropertyChanged(() => StagesBindingList);
        }


        [MediatorMessageSink(RequestRepository.REFRESH_ACTS_SCHEDULE, ParameterType = typeof (Act))]
        public void AddedAct2(Act act)
        {
            AddedAct1(act);
        }

        //~ScheduleViewModel()
        //{
          //  DisposeImporterDialog();
        //}

        #region Properties

        private Stageresult _selectedResult;
        private Schedulecontract _selectedSchedule;
        private Stage _selectedStage;
        public bool IsFiltered { get; set; }

        public IEnumerable<Currencymeasure> Units
        {
            get { return Repository.Currencymeasures; }
        }

        public bool IsGeneralContract
        {
            get { return ContractObject.IsGeneral; }
        }

        public bool IsGeneralContractAndHasCoworkers
        {
            get { return ContractObject.IsGeneral && ContractObject.SubContracts.Count > 0; }
        }

        public bool IsSubContract
        {
            get { return ContractObject.IsSubContract; }
        }

        public bool IsSubContractOrHasSubcontracts
        {
            get { return IsSubContract || (ContractObject.SubContracts.Count() > 0); }
        }

        public bool IsAgreement
        {
            get { return ContractObject.IsAgreement; }
        }


        public bool IsAgreementOrHasAgreements
        {
            get { return IsAgreement || HasAgreements; }
        }


        public bool HasAgreements
        {
            get
            {
               return ContractObject.Agreements.Any() && ContractObject.Stages.Any(s => s.Stagecondition == StageCondition.Closed);
            } 
        }

        public override string DisplayName
        {
            get
            {
                if (ContractObject == null)
                    return base.DisplayName;
                return "КП - " + ContractObject;
            }
        }

        public override string Error
        {
            get
            {
                String error = String.Empty;

                foreach (var schedule in SchedulesBindingList)
                {
                    var schedulecontract = schedule;
                    if (schedulecontract != null && (!string.IsNullOrEmpty(schedulecontract.Error) ||
                                                     !(string.IsNullOrEmpty(schedulecontract.Schedule.Error))))
                    {
                        error += schedule.Error + "\n\n";
                        error += schedule.Schedule.Error + "\n\n";
                    }

                    if (StagesBindingList != null)
                    {
                        foreach (var stage in StagesBindingList)
                        {
                            var stage1 = stage as Stage;
                            if (stage1 != null && !string.IsNullOrEmpty(stage1.Error))
                            {
                                error += (stage as Stage).Error + "\n\n";
                            }
                            var stage2 = stage as Stage;
                            if (stage2 != null)
                                error = stage2.ResultsBindingList.Cast<object>().Where(stageresult =>
                                    {
                                        var stageresult1 = stageresult as Stageresult;
                                        return stageresult1 != null && !string.IsNullOrEmpty(stageresult1.Error);
                                    }).Aggregate(error, (current, stageresult) =>
                                        {
                                            var stageresult2 = stageresult as Stageresult;
                                            return stageresult2 != null ? current + (stageresult2.Error + "\n\n") : null;
                                        });
                        }
                    }
                }

                return error;
            }
        }

        /// <summary>
        ///   Получает коллекцию календарных планов договора
        /// </summary>
        public ObservableCollection<Schedulecontract> SchedulesBindingList
        {
            get
            {
                Contract.Requires(ContractObject != null);
                if (_schedulesBindingList == null)
                {
                    _schedulesBindingList = new ObservableCollection<Schedulecontract>();
                    _schedulesBindingList.AddRange(ContractObject.Schedulecontracts);
                    _schedulesBindingList.CollectionChanged += schedulesBindingList_ListChanged;
                }
                return _schedulesBindingList;
            }
        }

        public IEnumerable<Ntpsubview> Ntpsubviews
        {
            get { return Repository.Ntpsubviews; }
        }

        public void ReorderStageBindingList()
        {
            _stagesBindingList = null;
            OnPropertyChanged(()=>StagesBindingList);
        }

        public IBindingList StagesBindingList
        {
            get
            {
                Contract.Requires(ContractObject != null);

                if (_selectedSchedule != null)
                {
                    if (_stagesBindingList == null)
                    {
                        _stagesBindingList =
                            new BindingList<Stage>(
                                _selectedSchedule.Schedule.Stages.OrderBy(x => x.Num, new NaturSortComparer<Stage>()).ToList());
                        _stagesBindingList.ListChanged += stagesBindingList_ListChanged;
                        foreach (var s in _stagesBindingList)
                        {
                            var stage = s as Stage;
                            if (stage != null)
                            {
                                stage.SetLevel();
                                stage.PropertyChanged += stage_PropertyChanged;
                            }
                        }
                    }
                }
                return _stagesBindingList;
            }
        }

        public Schedulecontract SelectedSchedule
        {
            get { return _selectedSchedule; }
            set
            {
                _selectedSchedule = value;
                if (_stagesBindingList != null)
                {
                    foreach (Stage s in _stagesBindingList)
                        s.PropertyChanged -= stage_PropertyChanged;

                    _stagesBindingList.ListChanged -= stagesBindingList_ListChanged;
                }


                _stagesBindingList = null;
                NotifyScheduleChanged();
            }
        }

        private void NotifyScheduleChanged()
        {
            OnPropertyChanged(() => IsReadOnly);
            OnPropertyChanged(() => StagesBindingList);
            OnPropertyChanged(() => StagesVisibility);
            OnPropertyChanged(() => CanDeleteSchedule);
            OnPropertyChanged(() => StagePriceColumnTitle);
            OnPropertyChanged(() => StagePriceWithNoNDSColumnTitle);
            OnPropertyChanged(() => NDSColumnTitle);
            OnPropertyChanged(() => OverallColumnTitle);
            OnPropertyChanged(() => CoworkersColumnTitle);
        }

        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set
            {
                if (_selectedStage == value) return;
                _selectedStage = value;
                OnPropertyChanged(() => SelectedStage);
                ButtonEnabedChanged();
            }
        }

        public Stageresult SelectedResult
        {
            get { return _selectedResult; }
            set { _selectedResult = value; }
        }


        public Visibility StagesVisibility
        {
            get { return _selectedSchedule != null ? Visibility.Visible : Visibility.Collapsed; }
        }


        public IEnumerable<Worktype> Worktypes
        {
            get { return Repository.Worktypes; }
        }

        public IEnumerable<Nds> Ndses
        {
            get { return Repository.Nds; }
        }

        public IEnumerable<Stage> MainStages
        {
            get
            {
                if (IsAgreement)
                {
                    // в списке выпадают только закрытые этапы, совпадающие по условиям
                    if (ContractObject.OriginalContract.Stages != null)
                        return
                            ContractObject.OriginalContract.Stages.Where(
                            x => (x.Stagecondition == StageCondition.Closed && x.Schedule.Worktypeid == _selectedStage.Schedule.Worktypeid && 
                                 (x.Num == _selectedStage.Num || x.Subject == _selectedStage.Subject ||
                                 ((x.Code == _selectedStage.Code) && (x.Code != null) && (x.Code.Trim() != "")))) &&
                                 (x.Price == _selectedStage.Price) && (x.Startsat == _selectedStage.Startsat) &&
                                 (x.Endsat == _selectedStage.Endsat));
                    return null;
                }
                return null;
            }
        }

        public IEnumerable<Economefficiencytype> Economefficiencytypes
        {
            get { return Repository.Economefficiencytypes; }
        }

        public IEnumerable<Ntpview> Ntpviews
        {
            get { return Repository.Ntpviews; }
        }

        public IEnumerable<Ndsalgorithm> Ndsalgorithms
        {
            get { return Repository.Ndsalgorithms; }
        }

        public bool IsPIRContract
        {
            get
            {
                if (ContractObject.Contracttype != null)
                    return ContractObject.Contracttype.Name == "ПИР";
                return false;
            }
        }

        public string StagePriceColumnTitle
        {
            get { return GetColumnTitle("Цена этапа"); }
        }

        public string StagePriceWithNoNDSColumnTitle
        {
            get { return GetColumnTitle("Цена без НДС"); }
        }

        public string NDSColumnTitle
        {
            get { return GetColumnTitle("НДС"); }
        }

        public string CoworkersColumnTitle
        {
            get { return GetColumnTitle("Соисполнители"); }
        }

        public string OverallColumnTitle
        {
            get { return GetColumnTitle("Итого"); }
        }

        public void RefreshScheduleBindingList()
        {
            //if (_schedulesBindingList != null) _schedulesBindingList.CollectionChanged -= schedulesBindingList_ListChanged;
            //_schedulesBindingList = null;

            if (_stagesBindingList != null) _stagesBindingList.ListChanged -= stagesBindingList_ListChanged;
            _stagesBindingList = null;

            _selectedSchedule = null;
            _selectedStage = null;
            _selectedResult = null;

            OnPropertyChanged(() => SchedulesBindingList);
            OnPropertyChanged(() => StagesBindingList);
        }

        private void schedulesBindingList_ListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnErrorChanged();
        }



        public void OnErrorChanged()
        {
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ERROR_CHANGED, Error);
        }

        protected override void OnWrappedDomainObjectChanged()
        {
            base.OnWrappedDomainObjectChanged();

            RefreshScheduleBindingList();
        }

        private void stagesBindingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            OnErrorChanged();
        }


        private void stage_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Repository.DebugPrintRepository();
            var stage = sender as Stage;
            
            stage.CheckChildParentProperties();
            if (e.PropertyName == "Nds" || e.PropertyName == "Ndsalgorithm" || e.PropertyName == "Price")
            {
                CalcNdsPrice(stage);
            }

            if (e.PropertyName == "Subject")
            {
                ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE_STAGE, stage);
            }


            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ERROR_CHANGED, Error);
        }

        private string GetColumnTitle(string mainpart)
        {
            if (_selectedSchedule != null) return _selectedSchedule.GetColumnTitle(mainpart);
            return mainpart;
        }

        #endregion

        #region Overrides of RepositoryViewModel

        /// <summary>
        ///   Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
            base.Save();
            Repository.DebugPrintRepository();
            Repository.SubmitChanges();
        }

        /// <summary>
        ///   Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns> </returns>
        protected override bool CanSave()
        {
            return string.IsNullOrEmpty(Error);
        }

        #endregion

        #region ScheduleCommands

        [ApplicationCommand("Создать календарный план", "/MContracts;component/Resources/calendar_add.png")]
        public ICommand CreateScheduleCommand
        {
            get
            {
                return _createScheduleCommand ??
                       (_createScheduleCommand =
                        new RelayCommand(CreateSchedule, x => CanCreateSchedule));
            }
        }


        [ApplicationCommand("Создать открепленный календарный план", "/MContracts;component/Resources/disconnect.png")]
        public ICommand UnbindScheduleCommand
        {
            get
            {
                return _unbindScheduleCommand ??
                       (_unbindScheduleCommand =
                        new RelayCommand(UnbindSchedule, x => CanUnbindSchedule));
            }
        }

        [ApplicationCommand("Удалить календарный план", "/MContracts;component/Resources/calendar_delete.png",
            AppCommandType.Confirm, "Выбранный вами календарный план будет удалён. Продолжить?")]
        public ICommand DeleteScheduleCommand
        {
            get
            {
                return _deleteScheduleCommand ??
                       (_deleteScheduleCommand =
                        new RelayCommand(DeleteSchedule, x => CanDeleteSchedule));
            }
        }

        
        [ApplicationCommand("Связать этапы оригинального договора - ДС", "/MContracts;component/Resources/hyperlink_blue.png",
                             AppCommandType.Silent, "", SeparatorType.Before)]
        public ICommand AutoBindAgreementOrigin
        {
            get
            {
                return _autobindscheduleoriginCommand ??
                       (_autobindscheduleoriginCommand =
                        new RelayCommand(ShowAutoBindAgreementOrigin, x => CanAutoBindAgreementOrigin()));
            }
        }


        public bool CanCreateSchedule
        {
            get { return CanCreateScheduleContract; }
        }

        public bool CanUnbindSchedule
        {
            get
            {
                if (SchedulesBindingList.Count == 0 || SelectedSchedule == null || SelectedSchedule.Schedule == null)
                    return false;

                return ContractObject.HasTheSameSchedule(SelectedSchedule.Schedule);
            }
        }

        public bool CanDeleteSchedule
        {
            get
            {
                if (CanEditScheduleContract)
                    return _selectedSchedule != null;
                return false;
            }
        }


        private void CreateSchedule(object o)
        {
            Contract.Requires(ContractObject != null);

         

            Repository.DebugPrintRepository();

            Schedule sch = new Schedule
            {
                Currencymeasure = ContractObject.Currencymeasure,
            };


            var sc = new Schedulecontract
            {
                Appnum = 1,
                Contractdoc = ContractObject
            };

            sch.Schedulecontracts.Add(sc);
            sc.Schedule = sch;
            ContractObject.Schedulecontracts.Add(sc);

            SchedulesBindingList.Add(sc);

            Repository.TryGetContext().Schedules.InsertOnSubmit(sch);
            Repository.TryGetContext().Schedulecontracts.InsertOnSubmit(sc);

            Repository.DebugPrintRepository();
            OnPropertyChanged(() => CanCreateSchedule);
            OnPropertyChanged(() => CanDeleteSchedule);


            RefreshScheduleBindingList(); 
            SendCreateLinkDeleteScheduleChanged();
        }

        private  void BindAgreementScheduleToOrigin(Schedulecontract agreementschedulecontract, Schedulecontract originschedule, 
                                                    bool UseNumbers, bool UseNames, bool UseObjectcodes, 
                                                    bool UseAdditionals, bool ClearBinding)
        {
            // связываем этапы ДС с этапами основного

            if (!ClearBinding)
            {
                foreach (Stage s in agreementschedulecontract.Schedule.Stages)
                {
                    s.ClosedGeneralStage =
                        originschedule.Schedule.Stages.FirstOrDefault(
                            ss => (UseNumbers && ss.Num.Trim() == s.Num.Trim() || !UseNumbers) &&
                                  (UseNames && ss.Subject.Trim() == s.Subject.Trim() || !UseNames) &&
                                  (UseObjectcodes && ss.Code.Trim() == s.Code.Trim() || !UseObjectcodes) &&
                                  (UseAdditionals && ss.Price == s.Price && ss.Startat == s.Startat &&
                                   ss.Endsat == s.Endsat || !UseAdditionals) &&
                                  (ss.Stagecondition == StageCondition.Closed));
                }
            }
            else
            {
                foreach (Stage s in agreementschedulecontract.Schedule.Stages)
                {
                    if (s.Stagecondition != StageCondition.Closed && !s.Stages.Any(ss => ss.Stagecondition == StageCondition.Closed))
                    {
                        s.ClosedGeneralStage = null;
                    }
                }
            }
        }

        private void BindSubcontractScheduleToGeneral(Schedulecontract subcontractschedulecontract, Schedulecontract generalschedule,
                                                      bool UseNumbers, bool UseNames, bool UseObjectcodes,
                                                      bool UseAdditionals)
        {
            // связываем этапы ДС с этапами основного

            foreach (Stage s in subcontractschedulecontract.Schedule.Stages)
            {
                s.GeneralStage =
                    generalschedule.Schedule.Stages.FirstOrDefault(
                        ss => (UseNumbers && ss.Num.Trim() == s.Num.Trim() || !UseNumbers) &&
                                (UseNames && ss.Subject.Trim() == s.Subject.Trim() || !UseNames) &&
                                (UseObjectcodes && ss.Code.Trim() == s.Code.Trim() || !UseObjectcodes) &&
                                (UseAdditionals && ss.Price == s.Price && ss.Startat == s.Startat &&
                                ss.Endsat == s.Endsat || !UseAdditionals));
            }
        }

        private void ShowAutoBindAgreementOrigin(object o)
        {
            var _bdlg = new AutoBindAgreementOriginStagesDialog();
            bool? dlgres = _bdlg.ShowDialog();

            if (dlgres.HasValue&&dlgres.Value)
            {
                BindAgreementScheduleToOrigin(SelectedSchedule, ContractObject.OriginalContract.Schedulecontracts.FirstOrDefault(), 
                                              _bdlg.UseStagenum, _bdlg.UseStagename, _bdlg.UseObjectcode, true, _bdlg.ClearBinding);

                OnPropertyChanged(()=> StagesBindingList);
            }
        }
        

        public  bool CanAutoBindAgreementOrigin()
        {
            return SelectedSchedule.Return(x => x.Contractdoc.IsAgreement, false)&&!CanUnbindSchedule&&StagesBindingList.Count > 0;
        }

        private void UnbindSchedule(object o)
        {
            Contract.Requires(ContractObject != null);

            var sc = SelectedSchedule.CloneRecursively(ContractObject, null) as Schedulecontract;

            BindAgreementScheduleToOrigin(sc, SelectedSchedule, true, false, false, false, false);
            if (ContractObject.IsSubContract)
                BindSubcontractScheduleToGeneral(sc, ContractObject.General.Schedulecontracts.FirstOrDefault(), 
                                                 true,false, false, false);

            int iIndex = 0;
            if (SchedulesBindingList.Contains(_selectedSchedule))
            {
                iIndex = SchedulesBindingList.IndexOf(_selectedSchedule);
                ContractObject.Schedulecontracts.Remove(_selectedSchedule);
                SchedulesBindingList.Remove(_selectedSchedule);
            }

            // привязываем новый
            if (sc != null)
            {
                sc.Schedule.Schedulecontracts.Add(sc);
                ContractObject.Schedulecontracts.Add(sc);
                SchedulesBindingList.Insert(iIndex,sc);
            }
            
            SendCreateLinkDeleteScheduleChanged();
            _stagesBindingList = null;
            OnPropertyChanged(() => SchedulesBindingList);
            OnPropertyChanged(() => StagesBindingList);
            //ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE_GENERAL, sc);
        }

        private void SendCreateLinkDeleteScheduleChanged()
        {
            OnPropertyChanged(() => CanCreateSchedule);
            OnPropertyChanged(() => CanDeleteSchedule);
        }


        private void DeleteSchedule(object o)
        {
            Contract.Requires(ContractObject != null);
            Contract.Requires(SelectedSchedule != null);

            var s = _selectedSchedule.Schedule;

            if (SchedulesBindingList.Contains(_selectedSchedule))
            {
                if (_selectedSchedule.Schedule != null)
                   Repository.DeleteShedule(_selectedSchedule.Schedule);

                SchedulesBindingList.Remove(_selectedSchedule);
            }

            SendCreateLinkDeleteScheduleChanged();
            // удаляем набор этапов если он был подгружен
            _stagesBindingList = null;
            OnPropertyChanged(() => StagesBindingList);
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE_GENERAL, s);
        }


        #endregion

        #region StageCommands

        private ContributorsImporterDialogWindow _cdlg;
        private ImportDialogWindow _dlg;

        public ICommand CreateStageCommand
        {
            get
            {
                return _createStageCommand ??
                       (_createStageCommand = new RelayCommand(CreateStage, x => CanCreateStage));
            }
        }


        public ICommand LinkStageCommandSubcontracts
        {
            get
            {
                return _linkStageCommandSubcontracts ??
                       (_linkStageCommandSubcontracts =
                        new RelayCommand(LinkStageSubcontracts, x => CanLinkStageSubcontracts));
            }
        }

        public ICommand LinkStageCommandGeneral
        {
            get
            {
                return _linkStageCommandGeneral ??
                       (_linkStageCommandGeneral =
                        new RelayCommand(LinkStageGeneral, x => CanLinkStageGeneral));
            }
        }


        public ICommand CloseStageCommand
        {
            get
            {
                return _closeStageCommand ??
                       (_closeStageCommand = new RelayCommand(CloseStage, x => CanCloseStage));
            }
        }

        public ICommand DeleteStageCommand
        {
            get
            {
                return _deleteStageCommand ??
                       (_deleteStageCommand = new RelayCommand(DeleteStageWithAutoNumber, x => CanDeleteStage));
            }
        }

        public ICommand SimpleDeleteStageCommand
        {
            get
            {
                return _simpledeletestagecommand ??
                       (_simpledeletestagecommand = new RelayCommand(DeleteStage, x => CanDeleteStage));
            }
        }

        public string StageApprovalAppearanceStatus
        {
            get
            {
                if (SelectedStage != null && SelectedStage.Id == 0)
                    return "Вы сможете работать с состоянием согласования после того, как этап будет сохранен";
                return "";
            }
        }

        public string ResultApprovalAppearanceStatus
        {
            get
            {
                if (SelectedResult != null && SelectedResult.Id == 0)
                    return "Вы сможете работать с состоянием согласования после того, как результат будет сохранен";
                return "";
            }
        }
        

        public ICommand ShowApprovalStateEditor
        {
            get
            {
                return
                    new RelayCommand(
                        x =>
                        OnRequestApprovalStateEditor(new EventParameterArgs<ISupportStateApproval>(SelectedStage)),
                        x => SelectedStage != null && SelectedStage.Id != 0 && CanEditScheduleContract);
            }
        }

        public ICommand ShowStageResultApprovalStateEditor
        {
            get
            {
                return
                    new RelayCommand(
                        x =>
                        OnRequestApprovalStateEditor(new EventParameterArgs<ISupportStateApproval>(SelectedResult)),
                        x => SelectedResult != null && SelectedResult.Id != 0 && CanEditScheduleContract);
            }
        }

        public ICommand UpStageCommand
        {
            get
            {
                return _upStageCommand ??
                       (_upStageCommand = new RelayCommand(UpStage, x => CanUpStage));
            }
        }

        public ICommand DownStageCommand
        {
            get
            {
                return _downStageCommand ??
                       (_downStageCommand = new RelayCommand(DownStage, x => CanDownStage));
            }
        }

        [ApplicationCommand("Автонумерация этапов", "/MContracts;component/Resources/edit-list-order.png")]
        public ICommand OrderStagesCommand
        {
            get { return _orderstagescommand ?? (_orderstagescommand = new RelayCommand(OrderStages, x => CanOrderStages)); }
        }

        public ICommand OutdentStageCommand
        {
            get
            {
                return _outdentStageCommand ??
                       (_outdentStageCommand = new RelayCommand(OutdentStage, x => CanOutdentStage));
            }
        }

        public ICommand IndentStageCommand
        {
            get
            {
                return _indentStageCommand ??
                       (_indentStageCommand = new RelayCommand(IndentStage, x => CanIndentStage));
            }
        }

        public ICommand ShowImporterDialogCommand
        {
            get
            {
                if (_showImporterDialogCommand == null)
                    _showImporterDialogCommand = new RelayCommand(ShowImporterDialog, x => CanShowImporterDialog);
                return _showImporterDialogCommand;
            }
        }


        public ICommand ShowContributorsImporterDialogCommand
        {
            get
            {
                if (_showContributorsImporterDialogCommand == null)
                    _showContributorsImporterDialogCommand = new RelayCommand(ShowContributorsImporterDialog,
                                                                              x => CanShowContributorsImporterDialog);
                return _showContributorsImporterDialogCommand;
            }
        }

        [ApplicationCommand("Создать календарный план автоматически", "/MContracts;component/Resources/automated.png")]
        public ICommand AutoCreateScheduleCommand
        {
            get {
                return _autoCreateScheduleCommand ??
                       (_autoCreateScheduleCommand = new RelayCommand(AutoCreateSchedule, x => CanAutoCreateSchedule));
            }
        }

        public bool CanLinkStageSubcontracts
        {
            get
            {
                if (CanEditScheduleContract)
                {
                    return  ContractObject.HasSubcontracts && ContractObject.Stages.Any() && ContractObject.SubContracts.SelectMany(s => s.Stages).Any();
                }
                return false;
            }
        }

        public bool CanLinkStageGeneral
        {
            get
            {
                if (CanEditScheduleContract)
                {
                    return ContractObject.IsSubContract && ContractObject.Stages.Any() && ContractObject.General.Stages.Any();
                }
                return false;
            }
        }

        public bool CanCreateStage
        {
            get { return CanEditScheduleContract && SelectedSchedule != null; }
        }

        public bool CanCloseStage
        {
            get
            {
                if (CanEditScheduleContract)
                {
                    return _selectedStage != null;
                }
                return false;
            }
        }

        public bool CanUpStage
        {
            get
            {
                if (CanEditScheduleContract && StagesBindingList != null && SelectedStage != null && !IsFiltered)
                {
                    int index = StagesBindingList.IndexOf(SelectedStage);

                    Stage stage;
                    if (index > 0) stage = StagesBindingList[index - 1] as Stage;
                    else return false;

                    if (stage != null && stage.Level != SelectedStage.Level) return false;

                    return SelectedStage != null;
                }
                return false;
            }
        }

        public bool CanDownStage
        {
            get
            {
                if (CanEditScheduleContract && StagesBindingList != null && SelectedStage != null && !IsFiltered)
                {
                    int index = StagesBindingList.IndexOf(SelectedStage);

                    Stage stage;
                    if (index < StagesBindingList.Count - 1)
                        stage = StagesBindingList[index + 1] as Stage;
                    else return false;

                    return stage != null && stage.Level == SelectedStage.Level;
                }
                return false;
            }
        }

        public bool CanOrderStages
        {
            get { return CanEditScheduleContract && StagesBindingList != null && StagesBindingList.Count > 0; }
        }

        public bool CanCreateScheduleContract
        {
            get
            {
                return true;
                
            }
        }


        public bool CanEditScheduleContract
        {
            get

            {
                return (SelectedSchedule!=null&&!ContractObject.HasTheSameSchedule(SelectedSchedule.Schedule));
                //|| ContractObject.IsAgreement;
                //!ContractObject.IsAgreement &&
                //    (SelectedSchedule == null || (SelectedSchedule != null && SelectedSchedule.Schedule != null && !(ContractObject.HasTheSameSchedule(SelectedSchedule.Schedule))));
            }
        }

        public bool IsReadOnly
        {
            get { return !CanEditScheduleContract; }
        }

        public bool CanOutdentStage
        {
            get
            {
                if (CanEditScheduleContract && SelectedStage != null && !IsFiltered)
                {
                    if (StagesBindingList != null)
                    {
                        int index = StagesBindingList.IndexOf(SelectedStage);
                        Stage stage = null;

                        if (index > 0)
                            stage = StagesBindingList[index - 1] as Stage;

                        if (stage != null && (index == -1 || index == 0 ||
                                              SelectedStage.Level - stage.Level >= 1))
                        {
                            return false;
                        }

                        return stage != null;
                    }
                    return false;
                }
                return false;
            }
        }

        public bool CanShowImporterDialog
        {
            get { return CanEditScheduleContract && SelectedSchedule != null; }
        }

        public bool CanIndentStage
        {
            get
            {
                if (CanEditScheduleContract && SelectedStage != null && !IsFiltered)
                {
                    if (StagesBindingList != null)
                    {
                        int index = StagesBindingList.IndexOf(SelectedStage);

                        Stage stage = null;
                        if (index > 0)
                            stage = StagesBindingList[index - 1] as Stage;

                        if (stage != null && (index == -1 || index == 0 ||
                                              stage.Level > SelectedStage.Level ||
                                              SelectedStage.Level == 0))
                        {
                            return false;
                        }

                        return stage != null;
                    }
                    return false;
                }
                return false;
            }
        }

        public bool CanDeleteStage
        {
            get
            {
                if (CanEditScheduleContract)
                    return (_selectedSchedule != null && _selectedStage != null);
                return false;
            }
        }


        private bool CanShowContributorsImporterDialog
        {
            get { return (_selectedSchedule != null) && (_selectedSchedule.Schedule.Stages.Count > 0) && _selectedSchedule.Contractdoc.IsGeneral; }
        }

        private bool CanAutoCreateSchedule
        {
            get
            {
                if (CanCreateScheduleContract)
                    return SchedulesBindingList.Count == 0;
                return false;
            }
        }

        public event EventHandler<EventParameterArgs<ISupportStateApproval>> RequestApprovalStateEditor;
        
        public void OnRequestApprovalStateEditor(EventParameterArgs<ISupportStateApproval> e)
        {
            EventHandler<EventParameterArgs<ISupportStateApproval>> handler = RequestApprovalStateEditor;
            if (handler != null) handler(this, e);
        }

        private void LinkStageSubcontracts(object o)
        {
            var dlg = new LinkStageFromGeneralWindow(Repository)
                {
                    viewModel = {GeneralStage = SelectedStage, Contract = ContractObject}
                };

            dlg.ShowDialog();


            OnPropertyChanged("StagesBindingList");
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_SUBGENERALS, ContractObject);
        }

        private void LinkStageGeneral(object o)
        {
            var dlg = new LinkStageFromSubcontractWindow(Repository) {viewModel = {Stages = new List<SelectedStage>()}};


            foreach (Stage stage in StagesBindingList)
            {
                dlg.viewModel.Stages.Add(new SelectedStage(stage));
            }
            dlg.viewModel.Contract = ContractObject;

            var firstOrDefault = ContractObject.Generals.FirstOrDefault();
            if (firstOrDefault != null)
            {
                Schedulecontract gs =
                    firstOrDefault.Schedulecontracts.FirstOrDefault(
                        p => p.Appnum == SelectedSchedule.Appnum);
                if (gs == null) gs = firstOrDefault.Schedulecontracts.FirstOrDefault();
                dlg.viewModel.GeneralSchedulecontract = gs;
            }

            
            dlg.ShowDialog();

            OnPropertyChanged(()=>StagesBindingList);
            //ViewMediator.NotifyColleagues(RequestRepository.REFRESH_GENERALS, ContractObject);

        }

        private void CreateStage(object o)
        {
            Contract.Requires(SelectedSchedule != null);
            Repository.DebugPrintRepository();


            Stage stage = CreateStage();
            stage.Startsat = new DateTime(DateTime.Today.Year, 1, 1);
            stage.Endsat = new DateTime(DateTime.Today.Year, 1, 1);
            stage.PropertyChanged += stage_PropertyChanged;
            StagesBindingList.Add(stage);

            ButtonEnabedChanged();
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());
            Repository.DebugPrintRepository();
        }

        private Stage CreateStage()
        {
            Contract.Requires(ContractObject != null);
            return new Stage
                {
                    Ndsalgorithm = ContractObject.Ndsalgorithm ?? Repository.Ndsalgorithms.GetReservedUndefined(),
                    Nds = ContractObject.Nds ?? Repository.Nds.FirstOrDefault(),
                    Schedule = _selectedSchedule.Schedule,
                };
        }

        private void DeleteStageWithAutoNumber(object o)
        {
            DeleteStage(o);
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());
        }

        private void DeleteStage(object o)
        {
            Contract.Requires((SelectedStage != null || o != null));

            var delstage = o as Stage ?? _selectedStage;

            Repository.DebugPrintRepository();

            if (FireDeletingStageCancel()) return;

            // удаляем подчиненные
            for (var i = delstage.Stages.Count - 1; i >= 0; i--)
            {
                var s = delstage.Stages[i];
                DeleteStage(s);
            }

            // удаляем из репозитория
            //selectedStage.Schedule = null;    
            if (delstage.Level == 0)
            {
                _selectedSchedule.Schedule.Stages.Remove(delstage);
            }
            else
            {
                delstage.ParentStage.Stages.Remove(delstage);
                _selectedSchedule.Schedule.Stages.Remove(delstage);
            }
            StagesBindingList.Remove(delstage);


            Repository.DebugPrintRepository();

            OnPropertyChanged("StagesBindingList");
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE_STAGE, delstage);

            ButtonEnabedChanged();

        }

        private bool FireDeletingStageCancel()
        {
            var args = new CancelEventArgs(false);
            if (DeletingStage != null)
                DeletingStage(this, args);
            return args.Cancel;
        }

        private void UpStage(object o)
        {
            Contract.Requires(SelectedStage != null);
            var index = StagesBindingList.IndexOf(_selectedStage);
            StagesBindingList.Move(index, index - 1);
            _selectedStage = StagesBindingList[index - 1] as Stage;

            ButtonEnabedChanged();
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());

            OnPropertyChanged(() => StagesBindingList);
            OnPropertyChanged(() => SelectedStage);
        }

        private void DownStage(object o)
        {
            Contract.Requires(SelectedStage != null);
            var index = StagesBindingList.IndexOf(_selectedStage);
            StagesBindingList.Move(index, index + 1);
            _selectedStage = StagesBindingList[index + 1] as Stage;

            ButtonEnabedChanged();
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());

            OnPropertyChanged(() => StagesBindingList);
            OnPropertyChanged(() => SelectedStage);
        }

        private void OrderStages(object o)
        {
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());
            OnPropertyChanged(() => StagesBindingList);
        }

        private void OutdentStage(object o)
        {
            Contract.Requires(SelectedStage != null);

            var index = StagesBindingList.IndexOf(_selectedStage);
            
            var parent = StagesBindingList.Where<Stage>(
                x => (_selectedStage.Level - x.Level == 0) && (index > StagesBindingList.IndexOf(x))).LastOrDefault();
            _selectedStage.ParentStage = parent;

            ButtonEnabedChanged();
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());

            OnPropertyChanged(() => SelectedStage);
            OnPropertyChanged(() => StagesBindingList);
        }

        private void IndentStage(object o)
        {
            Contract.Requires(SelectedStage != null);
            Repository.DebugPrintRepository();
            var index = StagesBindingList.IndexOf(_selectedStage);
            

            var parent =
                StagesBindingList.Where<Stage>(
                    x => (_selectedStage.Level - x.Level == 2) && (index > StagesBindingList.IndexOf(x))).LastOrDefault();
            _selectedStage.ParentStage = parent;

            ButtonEnabedChanged();
            OrderedStages(StagesBindingList.Where<Stage>(s => s.Level == 0).ToList());

            OnPropertyChanged(() => SelectedStage);
            OnPropertyChanged(() => StagesBindingList);

            Repository.DebugPrintRepository();
        }

        private void CloseStage(object o)
        {
            var dlg = new ClosingStagesWizzard(Repository) {viewModel = {Stages = StagesBindingList}};


            dlg.viewModel.ClosedStages.Add(SelectedStage);
            dlg.viewModel.Contract = ContractObject;
            if (dlg.ShowDialog() != false)
            {
                foreach (Stage stage in StagesBindingList)
                {
                    //stage.EndEdit();
                    if (stage != null)
                    {
                        stage.OnStageconditionChanged();
                        ViewMediator.NotifyColleagues(RequestRepository.REFRESH_ACTS_SCHEDULE, stage.Act);
                    }
                }
            }
        }

        private void ShowImporterDialog(object o)
        {

            if (_dlg == null) _dlg = new ImportDialogWindow(Repository);
            _dlg.CurrentSchedule = _selectedSchedule;

            var importerViewModel = _dlg.DataContext as ImporterViewModel;
            if (importerViewModel != null)
            {
                importerViewModel.GeneralContractdoc = ContractObject.General;
                importerViewModel.Originalcontractdoc = ContractObject.OriginalContract;
                
               
            }

            var viewModel = _dlg.DataContext as ImporterViewModel;
            if (viewModel != null)
                viewModel.DeleteStageCommand = SimpleDeleteStageCommand;
            _dlg.ShowDialog();

            var model = _dlg.DataContext as ImporterViewModel;
            if (model != null && model.NeedSave)
            {
                Repository.DebugPrintRepository();
                var contextProvider = Repository as IContextProvider<McDataContext>;
                if (contextProvider != null)
                    contextProvider.Context.GetChangeSet();


                // чистим и исходник и копию
                _stagesBindingList.Clear();


                foreach (var s in (_dlg.DataContext as ImporterViewModel).OutputStageBindingList.OrderBy(x => x.Num, new NaturSortComparer<Stage>()))
                {
                    s.PropertyChanged += stage_PropertyChanged;
                    s.Schedule = _selectedSchedule.Schedule;
                    StagesBindingList.Add(s);
                }

                
                ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE_GENERAL,
                                              _selectedSchedule.Schedule);

                var provider = Repository as IContextProvider<McDataContext>;
                if (provider != null)
                    provider.Context.GetChangeSet();
                Repository.DebugPrintRepository();


                OnPropertyChanged(() => StagesBindingList);
                NotifyStagesChanged();
            }
        }

        private void NotifyStagesChanged()
        {

            OnPropertyChanged(() => IsAgreement);
            OnPropertyChanged(() => IsGeneralContract);
            OnPropertyChanged(() => IsReadOnly);
            OnPropertyChanged(() => IsSubContractOrHasSubcontracts);
            OnPropertyChanged(() => IsSubContract);
            OnPropertyChanged(() => IsGeneralContractAndHasCoworkers);
            OnPropertyChanged(() => IsAgreement);
            OnPropertyChanged(() => HasAgreements);
        }


        private void ShowContributorsImporterDialog(object o)
        {
            if (_cdlg == null) _cdlg = new ContributorsImporterDialogWindow(Repository);
            _cdlg.CurrentSchedule = _selectedSchedule;
            _cdlg.ShowDialog();

            var contributorsImporterViewModel = _cdlg.DataContext as ContributorsImporterViewModel;
            if (contributorsImporterViewModel != null && contributorsImporterViewModel.NeedSave)
            {
                OnPropertyChanged(() => StagesBindingList);
                ViewMediator.NotifyColleagues(RequestRepository.REFRESH_SUBGENERALS, ContractObject);
                NotifyStagesChanged();
            }
        }


        private void AutoCreateSchedule(object o)
        {
            // создаем КП
            Contract.Requires(ContractObject != null);


            Repository.DebugPrintRepository();

            Schedule sch = new Schedule
                           {
                               Currencymeasure = ContractObject.Currencymeasure,
                               Worktype =
                                   Repository.Worktypes.FirstOrDefault(
                                       x =>
                                       x.Name.Trim().ToLower() ==
                                       ContractObject.Contracttype.Return(c => c.Name, "").Trim().ToLower()) ??
                                   Repository.Worktypes.FirstOrDefault()
                           };

            
            // создаем этап и копируем весь его из договора
            var s = new Stage
                {
                    Num = "1",
                    Subject = ContractObject.Subject,
                    Startsat = ContractObject.Startat,
                    Endsat = ContractObject.Endsat,
                    Ndsalgorithm = ContractObject.Ndsalgorithm ?? Repository.Ndsalgorithms.FirstOrDefault(),
                    Nds = ContractObject.Nds ?? Repository.Nds.FirstOrDefault(),
                    Price = ContractObject.Price
                };

            s.PropertyChanged += stage_PropertyChanged;
            s.Schedule = sch;
            

            var sc = new Schedulecontract
                {
                    Appnum = 1,
                    Contractdoc = ContractObject
                };
            sch.Schedulecontracts.Add(sc);
            sc.Schedule = sch;
            ContractObject.Schedulecontracts.Add(sc);

            SchedulesBindingList.Add(sc);

            Repository.TryGetContext().Schedules.InsertOnSubmit(sch);
            Repository.TryGetContext().Stages.InsertOnSubmit(s);
            Repository.TryGetContext().Schedulecontracts.InsertOnSubmit(sc);

            Repository.DebugPrintRepository();
            OnPropertyChanged(() => CanCreateSchedule);
            OnPropertyChanged(() => CanDeleteSchedule);

            
            RefreshScheduleBindingList();
        }

        public void DisposeImporterDialog()
        {
            if (_dlg != null)
            {
                _dlg.AllowClosing = true;
                _dlg.Dispose();
                _dlg = null;
            }

            if (_cdlg != null)
            {
                _cdlg.AllowClosing = true;
                _cdlg.Dispose();
                _cdlg = null;
            }
        }

        protected override void OnDispose()
        {
            DisposeImporterDialog();
            base.OnDispose();
        }

        #endregion

        #region ResultsCommands

        public ICommand CreateResultCommand
        {
            get
            {
                return _createResultCommand ??
                       (_createResultCommand = new RelayCommand(CreateResult, x => CanCreateResult));
            }
        }

        public ICommand DeleteResultCommand
        {
            get
            {
                return _deleteResultCommand ??
                       (_deleteResultCommand = new RelayCommand(DeleteResult, x => CanDeleteResult));
            }
        }

        public bool CanCreateResult
        {
            get { return CanEditScheduleContract && SelectedStage != null; }
        }

        public bool CanDeleteResult
        {
            get
            {
                if (CanEditScheduleContract && SelectedStage != null)
                    return _selectedResult != null;
                return false;
            }
        }

        private void CreateResult(object o)
        {
            var result = new Stageresult();
            result.PropertyChanged += result_PropertyChanged;
            _selectedStage.ResultsBindingList.Add(result);


            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE, result);

            //Repository.InsertStateResult(result);
        }

        private void result_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void DeleteResult(object o)
        {
            Repository.DebugPrintRepository();

            if (_selectedResult.Id != 0)
            {
                Repository.DeleteStateResult(_selectedResult);
                _selectedStage.ResultsBindingList.Remove(_selectedResult);
            }
            else
            {
                _selectedStage.ResultsBindingList.Remove(_selectedResult);
                Repository.DebugPrintRepository();
                //Repository.DeleteStateResult(selectedResult);
            }

            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE, _selectedResult);

            Repository.DebugPrintRepository();
        }

        #endregion

        #region HelperMethods

        private void OrderSingleStage(IList<Stage> stages, Stage stage)
        {
            //сортируем родительские этапы
            if (stage.Level == 0 && stages != null)
            {
                int index = stages.IndexOf(stage) + 1;
                stage.Num = index.ToString(CultureInfo.InvariantCulture);
            }

            //сортируем дочерние этапы
            foreach (var s in _stagesBindingList.Where<Stage>(x => x.Level != 0 && x.ParentStage == stage))
            {
                var q = StagesBindingList.Where<Stage>(x => x.ParentStage == stage).ToList();
                s.Num = s.ParentStage.Num + "." + (q.IndexOf(s) + 1).ToString(CultureInfo.InvariantCulture);
                
                OrderSingleStage(null, s);
            }
        }

        private void OrderedStages(IList<Stage> stagessublist)
        {
            //сортируем родительские этапы
            if (stagessublist != null)
            {
                foreach (var stage in stagessublist.Where(x => x.Level == 0))
                {
                    OrderSingleStage(stagessublist, stage);
                }
            }
        }

        private void ButtonEnabedChanged()
        {
            OnPropertyChanged(() => CanCreateStage);
            OnPropertyChanged(() => CanDeleteStage);
            OnPropertyChanged(() => CanOutdentStage);
            OnPropertyChanged(() => CanIndentStage);
            OnPropertyChanged(() => CanUpStage);
            OnPropertyChanged(() => CanDownStage);
        }

        public void CreateParameters()
        {
            var parameters = _selectedResult.Economefficiencytype.Efficienceparametertypes;

            foreach (var param in _selectedResult.ParametersBindingList)
            {
                var efparameterstageresult = param as Efparameterstageresult;
                if (efparameterstageresult != null && efparameterstageresult.Economefficiencyparameter != null)
                    Repository.DeleteEfparamStageresult(param as Efparameterstageresult);
            }

            _selectedResult.ParametersBindingList.Clear();

            foreach (var parameter in parameters)
            {
                _selectedResult.ParametersBindingList.Add(new Efparameterstageresult
                    {
                        Economefficiencyparameter =
                            parameter.Economefficiencyparameter,
                        Stageresult = _selectedResult
                    });
            }

            _selectedResult.ParametersBindingListChanged();
        }

        private static void CalcNdsPrice(Stage stage)
        {
            stage.OnNdsPriceChanged();
        }

        public void UpdateResult()
        {
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE, _selectedResult);
        }

        public virtual void SendPropertyChanged(string propertyName)
        {
            if (propertyName == "SelectedStageSubject")
            {
                if (SelectedStage != null)
                {
                    stage_PropertyChanged(SelectedStage, new PropertyChangedEventArgs("Subject"));
                }
            }
            else
            {
                OnPropertyChanged(propertyName);
            }
        }

        #endregion

        public long? ContractdocId
        {
            get { return ContractObject.Id; }
        }

        public long? Maincontractid {
            get { return ContractObject.Maincontractid; }
        }

        public string ContractCaption
        {
            get { return "КП " + ContractObject.ShortName;  }
        }
    }
}
