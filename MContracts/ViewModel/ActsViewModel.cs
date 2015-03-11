using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Properties;
using MContracts.ViewModel.Helpers;
using McReports.Common;
using McUIBase.Factories;
using MediatorLib;
using UIShared.Commands;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Класс расширений для работы с ActDto
    /// </summary>
    public static class ActDtoExtensions
    {
        /// <summary>
        /// Преобразует коллекцию актов в коллекцию ActDto
        /// </summary>
        /// <param name="source">Исходная коллекция актов</param>
        /// <param name="thisContract">Данные для определения, закрыт ли акт в указанном договоре или нет</param>
        /// <returns>Коллекция ActDto</returns>
        public static IEnumerable<ActDto> IntoActDto(this IEnumerable<Act> source, Schedulecontract schedulecontract=null, Contractdoc thisContract = null)
        {
            Contract.Requires(source != null);
           
            return source.Select(x => new ActDto
                {
                    Id = x.Id,
                    Issigned = x.Issigned,
                    Num = x.Num,
                    Signdate = x.Signdate,
                    Stages = x.StagesNumbers,
                    TotalSumWithNds = x.ActMoney.Factor.National.PriceWithNdsValue,
                    TotalSumWithNoNds = x.ActMoney.Factor.National.PureValue,
                    TotalSumNds = x.ActMoney.Factor.National.NdsValue,
                    TransferSum = x.TransferSumMoney.Factor.National.PriceWithNdsValue,
                    PrepaymentWithNds = x.CreditedMoneyModel.Factor.National.PriceWithNdsValue,
                    PrepaymentWithNoNds = x.CreditedMoneyModel.Factor.National.PureValue,
                    IsClosedByThisContract = thisContract.Return(x.IsAssignedForContractdoc,true),
                    Acttype = x.Realacttype.Return(a=>a.ToString(), "Не указан"),
                    CanEdit = (thisContract.Return(x.IsAssignedForContractdoc, true) || (schedulecontract!=null && schedulecontract.IsTheSameScheduleAsInOriginalContractdoc))

                }).ToList();
        }
    }

    /// <summary>
    /// Модель представления для ввода данных об актах по договору
    /// </summary>
    public class ActsViewModel : ContractWorkspaceViewModel
    {
        private ObservableCollection<ActDto> _acts;
        private ICommand _createactreportcommand;
        //private IEnumerable<Stage> _stages;

        private Schedulecontract _selectedschedule;
        private ICommand _setSignedCommand;
        private ICommand _setNotSignedCommand;
        private ICommand _setDateCommand;

        public ActsViewModel(IContractRepository repository)
            : base(repository)
        {
            ViewMediator.Register(this);
            IsUnchangable = true;
        }

        public IEnumerable<Schedulecontract> Schedulecontracts
        {
            get { return ContractObject.Schedulecontracts.Where(c => c.Schedule != null); }
        }

        /// <summary>
        ///     выбранный КП
        /// </summary>
        public Schedulecontract SelectedSchedule
        {
            get { return _selectedschedule ?? (_selectedschedule = Schedulecontracts.FirstOrDefault()); }
            set
            {
                _selectedschedule = value;
                _acts = null;
                //_stages = null;
                OnPropertyChanged(() => SelectedSchedule);
                //OnPropertyChanged(()=>Stages);
                OnPropertyChanged(() => Acts);
                OnPropertyChanged(() => StagePriceWithNDSColumnTitle);
                OnPropertyChanged(() => StagePriceWithNoNDSColumnTitle);
                OnPropertyChanged(() => NDSColumnTitle);
                OnPropertyChanged(() => PrepaymentColumnTitle);
                OnPropertyChanged(() => TransferColumnTitle);
            }
        }

        public ObservableCollection<ActDto> Acts
        {
            get
            {
                if (SelectedSchedule == null) return null;
                if (_acts == null)
                {
                    using (IContractRepository repository = RepositoryFactory.CreateContractRepository())
                    {
                        _acts = new ObservableCollection<ActDto>(repository.TryGetContext().Stages.Where(
                            x => x.Act != null && x.Schedule.Id == SelectedSchedule.Schedule.Id).Select(x => x.Act).
                                                                            Distinct().IntoActDto(SelectedSchedule, ContractObject));
                    }
                }
                return _acts;
            }
        }

        public ActDto SelectedAct { get; set; }

        [ApplicationCommand("Создать акт", "/MContracts;component/Resources/act_add.png")]
        public ICommand CreateActCommand
        {
            get { return new RelayCommand(CreateAct, x => CanCreateAct); }
        }


        public bool CanCreateAct
        {
            get { return SelectedSchedule != null; }
        }

        [ApplicationCommand("Удалить акт", "/MContracts;component/Resources/act_delete.png", AppCommandType.Confirm,
            "Выбранный вами акт будет удалён. Продолжить?")]
        public ICommand DeleteActCommand
        {
            get { return new RelayCommand(x => DeleteAct(), x => CanDeleteAct); }
        }

        public bool CanDeleteAct
        {
            get { return SelectedAct != null && SelectedAct.IsClosedByThisContract; }
        }

        [ApplicationCommand("Изменить акт", "/MContracts;component/Resources/act_edit.png")]
        public ICommand EditActCommand
        {
            get { return new RelayCommand(EditAct, x => CanEditAct); }
        }

        //[ApplicationCommand("Отметить как подписанные", "/MContracts;component/Resources/signed.png")]
        public ICommand SetSignedCommand
        {
            get
            {
                return _setSignedCommand ??
                       (_setSignedCommand =
                        new RelayCommand(x => SetSelectedActsStatus(true), x => CanEditAct));
            }
        }

        //[ApplicationCommand("Отметить как не подписанные", "/MContracts;component/Resources/notsigned.png")]
        public ICommand SetNotSignedCommand
        {
            get
            {
                return _setNotSignedCommand ??
                       (_setNotSignedCommand =
                        new RelayCommand(x => SetSelectedActsStatus(false), x => CanEditAct));
            }
        }

        [ApplicationCommand("Задать дату", "/MContracts;component/Resources/date_add.png")]
        public ICommand SetDateCommand
        {
            get
            {
                return _setDateCommand ??
                       (_setDateCommand =
                        new RelayCommand(x => SetActDate(), x => CanEditAct));
            }
        }

        public event EventHandler<EventParameterArgs<ActStatusArgs>> RequestActDate;

        protected virtual void OnRequestActDate(EventParameterArgs<ActStatusArgs> e)
        {
            var handler = RequestActDate;
            if (handler != null) handler(this, e);
        }

        private void SetActDate()
        {
            var args = new EventParameterArgs<ActStatusArgs>(new ActStatusArgs(){SignDate = null, IsSigned = null});
            OnRequestActDate(args);
            var argDate = args.Parameter;
            // Если пользователь не выбрал ни дату, ни статус подписания, то выход
            if (!(argDate.IsSigned.HasValue || argDate.SignDate.HasValue)) return;

            SetSelectedActsStatus(argDate.IsSigned, argDate.SignDate);
            
        }

        
        public bool CanEditAct
        {
            get
            {
                return SelectedAct != null
                    && SelectedAct.IsClosedByThisContract || (SelectedSchedule!=null && SelectedSchedule.IsTheSameScheduleAsInOriginalContractdoc);
            }
        }


        public string SumWithNDS
        {
            get
            {
                return
                    GetSumString(x => x.TotalSumWithNds);
            }
        }

        public string SumNDS
        {
            get
            {
                return
                    GetSumString(x => x.TotalSumNds);
            }
        }

        public string SumCredited
        {
            get
            {
                return
                    GetSumString(x => x.TotalSumWithNds - x.TransferSum);
            }
        }

        public string SumTransfer
        {
            get
            {
                return
                    GetSumString(x => x.TransferSum);
            }
        }

        public string ActsCount
        {
            get
            {
                if (Acts != null)
                    return Acts.Count.ToString(CultureInfo.InvariantCulture);
                return "0";
            }
        }


        public bool AutoIncNums { get; set; }

        public bool CanCreateActReport
        {
            get { return (SelectedAct != null); }
        }

        public string StagePriceWithNDSColumnTitle
        {
            get { return GetColumnTitle("Цена с НДС"); }
        }

        public string StagePriceWithNoNDSColumnTitle
        {
            get { return GetColumnTitle("Цена без НДС"); }
        }

        public string NDSColumnTitle
        {
            get { return GetColumnTitle("НДС"); }
        }

        public string PrepaymentColumnTitle
        {
            get { return GetColumnTitle("Зачтено авансом"); }
        }

        public string TransferColumnTitle
        {
            get { return GetColumnTitle("К перечислению"); }
        }

        [ApplicationCommand("Печать акта", "/MContracts;component/Resources/document.png")]
        public ICommand CreateActReportCommand
        {
            get
            {
                return _createactreportcommand ??
                       (_createactreportcommand =
                        new RelayCommand(x => CreateActReport(), x => CanCreateActReport));
            }
        }

        
        public ObservableCollection<object> SelectedActs { get; set; } 

        private void SetSelectedActsStatus(bool? isSigned, DateTime? date = default(DateTime?))
        {
            Contract.Assert(SelectedActs != null);
            foreach (var selectedAct in SelectedActs.Cast<ActDto>())
            {
                var act = Repository.Acts.Single(x => selectedAct.Id == x.Id);
                
                if (date.HasValue)
                    act.Signdate = date;
                if (isSigned.HasValue)
                    act.Issigned = isSigned;
            }
            Repository.SubmitChanges();
            _acts = null;
            OnPropertyChanged(() => Acts);
        }

        public override string DisplayName
        {
            get
            {
                if (ContractObject != null) return "Акты " + ContractObject;
                return base.DisplayName;
            }
        }

        public override bool IsClosable
        {
            get { return true; }
        }

        #region Overrides of RepositoryViewModel

        public override string Error
        {
            get { return null; }
        }

        internal Act CreateNewAct(ActDesignerViewModel vm)
        {
            Act act = vm.Repository.NewAct(vm.CurrentContract);
            if (AutoIncNums)
            {
                act.Num = GetNextActNum();
                act.Acttype = ContractObject.Acttype;
            }
            return act;
        }

        public static string GetNextActNumber(IEnumerable<ActDto> acts)
        {
            if (acts == null) throw new ArgumentNullException("acts");
            int result;
            if (!acts.Any()) return "1";
            return
                (acts.Select(x => int.TryParse(x.Num, out result) ? result : 1).Max() + 1).ToString(
                    CultureInfo.InvariantCulture);
        }

        private string GetNextActNum()
        {
            return GetNextActNumber(Acts);
        }

        /// <summary>
        ///     Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
            Repository.SubmitChanges();
            using (IContractRepository repository = RepositoryFactory.CreateContractRepository())
            {
                Contractdoc contract = repository.Contracts.SingleOrDefault(x => x.Id == ContractObject.Id);
                contract.Do(x => x.UpdateFundsStatistics());
                repository.SubmitChanges();
            }
        }

        /// <summary>
        ///     Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return true;
        }

        public void OnErrorChanged()
        {
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ERROR_CHANGED, Error);
        }

        #endregion

        public static IList<Act> GetScheduleActs(Schedule schedule)
        {
            if (schedule == null) throw new ArgumentNullException("schedule");
            return (schedule.Stages.Where(x => x.Act != null).Select(x => x.Act).Distinct().ToList());
        }

        public string GetSumString(Func<ActDto, decimal> selector)
        {
            if (Acts == null) return string.Empty;
            return Math.Round(Acts.Sum(selector), 2).
                        ToString("N2", Currency.National.CI);
        }

        public string SumPure(Func<ActDto, decimal> selector)
        {
            if (Acts == null)
                return string.Empty;

            return Math.Round(Acts.Sum(selector), 2).ToString("N2", Currency.National.CI);
        }

        public void CreateAct(object o)
        {
            ShowActDesignerDialog(null);
        }

        public event EventHandler<EventArgs> RefusedPrintAct;

        protected virtual void OnRefusedPrintAct()
        {
            var handler = RefusedPrintAct;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void CreateActReport()
        {

            if (SelectedActs != null && SelectedActs.Count() > 1)
            {
                IList<Act> _selacts = new  List<Act>();
                foreach (var selectedAct in SelectedActs.Cast<ActDto>())
                {
                    var act = Repository.Acts.Single(x => selectedAct.Id == x.Id);

                    _selacts.Add(act);
                }
                
                
                if (_selacts.Any(a => a.Realacttype == null || a.Realacttype.WellKnownType == WellKnownActtypes.Undefined))
                {
                    OnRefusedPrintAct();
                    return;
                }

                
                ActReportCreatorHelper.CreateActReports(_selacts, ContractObject,
                                                        null,
                                                        new DefaultTemplateProvider(AppDomain.CurrentDomain.BaseDirectory,
                                                                                    "Templates\\", Settings.Default));

            }
            else
            {
                var a = Repository.Acts.Single(aa => aa.Id == SelectedAct.Id);

                if (a.Realacttype == null || a.Realacttype.WellKnownType == WellKnownActtypes.Undefined)
                {
                    OnRefusedPrintAct();
                    return;
                }

                ActReportCreatorHelper.CreateActReport(a, ContractObject,
                                                    null,
                                                    new DefaultTemplateProvider(AppDomain.CurrentDomain.BaseDirectory,
                                                                                "Templates\\", Settings.Default));
            }
        }

        public void DeleteAct()
        {
            // Запоминается удаляемый объект акта и указывается его ContractObject
            // который используется для работы обновления репозитария договоров 
            Contract.Assert(Acts != null);
            Contract.Assert(SelectedAct != null);
            Repository.DeleteAct(SelectedAct.Id);
            Acts.Remove(SelectedAct);
            SaveChanges();
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ACT_DELETED, ContractObject);
            OnStatusChanged();
        }

        public event EventHandler<EventParameterArgs<ActDto>> RequestShowActDesigner;

        public void OnRequestShowActDesigner(EventParameterArgs<ActDto> e)
        {
            EventHandler<EventParameterArgs<ActDto>> handler = RequestShowActDesigner;
            if (handler != null) handler(this, e);
        }


        private void ShowActDesignerDialog(ActDto act)
        {
            OnRequestShowActDesigner(new EventParameterArgs<ActDto>(act));
        }

        public void EditAct(object o)
        {
            ShowActDesignerDialog(SelectedAct);
        }


        public event EventHandler StatusChanged;

        public void OnStatusChanged()
        {
            OnPropertyChanged(() => SumWithNDS);
            OnPropertyChanged(() => SumNDS);
            OnPropertyChanged(() => SumCredited);
            OnPropertyChanged(() => SumTransfer);
            OnPropertyChanged(() => ActsCount);

            EventHandler handler = StatusChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private string GetColumnTitle(string mainpart)
        {
            if (SelectedSchedule != null) return mainpart + ", " + Currency.National.CI.NumberFormat.CurrencySymbol;
            return mainpart;
        }

        [MediatorMessageSink(RequestRepository.REFRESH_ACTS_SCHEDULE, ParameterType = typeof (Act))]
        internal void ReloadActs(Act act)
        {
            _acts = null;
            OnPropertyChanged(() => Acts);
        }

        public virtual void SendPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}