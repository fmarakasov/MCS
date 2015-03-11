
#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MCDomain.Services;
using MContracts.Classes;
using McReports.Common;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;
using McUIBase.Factories;

#endregion

namespace MContracts.ViewModel
{
  
    
    public class ActDesignerViewModel : RepositoryViewModel, ICacheble
    {
        private readonly EventHandler<RateOnDateEventArgs> _currencyRateCallbackhander;
        private readonly CurrencyService _currencyService = new CurrencyService();
// ReSharper disable NotAccessedField.Local
        private PropertyObserver<Act> _actPropertyChangedObjerver;
// ReSharper restore NotAccessedField.Local
        private decimal? _closedAmount;
        private int _closedSelectedCount;
        private ObservableCollection<Stage> _closedStages;
        private Contractdoc _currentContract;
        private int _openSelectedCount;
        private ObservableCollection<Stage> _openStages;
        private Schedule _schedule;

        /// <summary>
        /// Служит для хранения суммы кредита перед редактированием акта
        /// </summary>
        private decimal _initialCredited;

        public ActDesignerViewModel(IContractRepository repository)
            : base(repository)
        {
            _currencyRateCallbackhander = InstanceCurrencyRateOnDateCompleted;
            _currencyService.CurrencyRateOnDateCompleted += _currencyRateCallbackhander;

        }

        /// <summary>
        /// Сохраняет значение типа акта закрытия для подставновки в качестве значения по умолчанию в диалогах
        /// </summary>
        private static long? _prevActTypeMemorize;
        /// <summary>
        /// Сохраняет значение даты, введённой пользователем для подстановки в качестве значения по умолчанию в диалогах
        /// </summary>
        private static DateTime? _prevActDateMemorize;

        /// <summary>
        /// Сохраняет значение состояния подписания акта для подстановки в качестве значения по умолчанию в диалогах
        /// </summary>
        private static bool? _prevActIssignMemorizer;
 
        /// <summary>
        ///   Вызывает событие запроса редактирования справочника оснований
        /// </summary>
        public ICommand ShowEnterpriseAuthorityEditor
        {
            get { return new RelayCommand(x => OnRequestShowEnterpriseAuthorityEditor(EventArgs.Empty), x => true); }
        }

        /// <summary>
        ///   Вызывает событие запроса редактирования справочника регионов
        /// </summary>
        public ICommand ShowRegionsEditor
        {
            get { return new RelayCommand(x => OnRequestShowRegionsEditor(EventArgs.Empty), x => true); }
        }

        /// <summary>
        /// Внести рекомендуемое значение аванса
        /// </summary>
        public ICommand SetRecomendedPrepaymentCommand
        {
            get { return new RelayCommand(x => Credited = RecomendedPrepayment, x => true); }
        }

        public ICommand CreateActReportCommand
        {
            get { return new RelayCommand(x => CreateActReport(), x => CurrentAct.Realacttype != null && CurrentAct.Realacttype.WellKnownType != WellKnownActtypes.Undefined && CanSave()); }
        }

        public event EventHandler<EventParameterArgs<MessageBoxResult>> QuerySaveChanges;

        public void OnQuerySaveChanges(EventParameterArgs<MessageBoxResult> e)
        {
            EventHandler<EventParameterArgs<MessageBoxResult>> handler = QuerySaveChanges;
            if (handler != null) handler(this, e);
        }

        private void CreateActReport()
        {
           
            if (UnitOfWork.Context.IsModified)
            {
                var param = new EventParameterArgs<MessageBoxResult>(MessageBoxResult.Yes);
                OnQuerySaveChanges(param);
                if (param.Parameter != MessageBoxResult.Yes) SaveChanges();
                else
                    return;
            }
            ActReportCreatorHelper.CreateActReport(CurrentAct, CurrentContract, 
                                                null, new DefaultTemplateProvider(AppDomain.CurrentDomain.BaseDirectory, "Templates\\", Properties.Settings.Default));

        }

        [ApplicationCommand("Закрыть выделенные этапы", "/MContracts;component/Resources/arrow_fwd.png")]
        public ICommand CloseSelectedStages
        {
            get
            {
                return
                    new RelayCommand(
                        x =>
                            {
                                TransferListAndSetAct(PrepareOpenList(SelectedOpenStages), ClosedStages, CurrentAct,
                                                      OpenStages);

                                TransferOrphanedParents();
                            },
                        x => SelectedOpenStages != null && SelectedOpenStages.Count > 0);
            }
        }

        [ApplicationCommand("Открыть выделенные этапы", "/MContracts;component/Resources/arrow_back.png")]
        public ICommand OpenSelectedStages
        {
            get
            {
                return new RelayCommand(
                    x => TransferListAndSetAct(PrepareClosedList(SelectedClosedStages), OpenStages, null, ClosedStages),
                    x => SelectedClosedStages != null && SelectedClosedStages.Count > 0);
            }
        }

        [ApplicationCommand("Открыть все этапы", "/MContracts;component/Resources/arrows_back.png")]
        public ICommand OpenAllStages
        {
            get
            {
                return new RelayCommand(x => TransferListAndSetAct(ClosedStages, OpenStages, null),
                                        x => ClosedStages != null && ClosedStages.Count > 0);
            }
        }

        [ApplicationCommand("Закрыть все этапы", "/MContracts;component/Resources/arrows_fwd.png")]
        public ICommand CloseAllStages
        {
            get
            {
                return new RelayCommand(x => TransferListAndSetAct(OpenStages, ClosedStages, CurrentAct),
                                        x => OpenStages != null && OpenStages.Count > 0);
            }
        }

        /// <summary>
        ///   Получает или устанавливает текущий календарный план
        /// </summary>
        public Schedule CurrentSchedule
        {
            get { return _schedule; }
            set
            {
                if (_schedule == value) return;
                _schedule = value;
                RegisterActPropertyObserver();
                OnPropertyChanged(() => CurrentSchedule);
            }
        }

        /// <summary>
        ///   Получить рекомендуемый размер аванса для акта.
        ///   Расчёт аванса призводится с использоваием объекта договора, который создан в 
        ///   другом контексте, что бы были видимы изменения в платёжках
        ///   При расчёте не учитываются данные по авансу текущего акта, т.к.
        ///   они находятся на редактировании у пользователя.
        /// </summary>
        public decimal RecomendedPrepayment
        {
            get
            {
                Contract.Requires(CurrentContract != null);
                using (var ctx = RepositoryFactory.CreateContractRepository())
                {
                    var contract = ctx.GetContractdoc(CurrentContract.Id);
                    
                    var rests = contract.TransferedFunds - contract.Prepaymented + _initialCredited;
                    return GetRecomendedPrepayment(rests, ClosedStageAmount,
                                                   CurrentContract.Prepaymentpercent, !OpenStages.Any());
                }
            }
        }

        /// <summary>
        ///   Получает модель денег кредита акта
        /// </summary>
        public MoneyModel CreditedMoneyModel
        {
            get { return CurrentAct.CreditedMoneyModel; }
        }

        /// <summary>
        ///   Получает сумму по открытым этапам. Сумма представлена в валюте договора в ед.
        /// </summary>
        public decimal OpenStageAmount
        {
            get { return OpenStages.GetTotalAmount()*GetScheduleFactor(); }
        }

        /// <summary>
        ///   Получает сумму по закрытым этапам. Сумма представлена в валюте договора в ед.
        /// </summary>
        public decimal ClosedStageAmount
        {
            get
            {
                Contract.Requires(CurrentAct != null);
                if (!_closedAmount.HasValue)
                {
                    _closedAmount = ClosedStages.GetTotalAmount()*GetScheduleFactor();
                }
                return _closedAmount.Value;
            }
        }

        /// <summary>
        ///   Получает или устанавливает текущий акт
        /// </summary>
        public Act CurrentAct
        {
            get
            {
                return WrappedDomainObject as Act;
            }
            set
            {
                if (value == WrappedDomainObject) return;
                SubscribeActEvents(CurrentAct, value);
                WrappedDomainObject = value;
                SetCurrentActContractObject();
                CurrentAct.If(x => !x.Stages.Any())
                          .Do(x => x.Signdate = _prevActDateMemorize ?? DateTime.Today)
                          .Do(x => x.Issigned = _prevActIssignMemorizer ?? false)
                          .Do(x => x.Ndsalgorithm = GetPredefinedNdsAlgorithm())
                          .Do(x => x.Nds = GetPredefinedNds())
                          .Do(x=>x.Acttype = GetPredefinedActtype());
                OnPropertyChanged(() => CurrentAct);
            }
        }

        private Acttype GetPredefinedActtype()
        {
            var contractActtype = CurrentContract.Acttype;
            return contractActtype.With(x => Repository.Acttypes.SingleOrDefault(n => n.Id == x.Id)) ??
                Repository.Acttypes.SingleOrDefault(n => n.Id == _prevActTypeMemorize) ?? Repository.Acttypes.FirstOrDefault();
        }

        private Nds GetPredefinedNds()
        {
            var contractNds = CurrentContract.Nds;
            return contractNds.With(x=>Repository.Nds.SingleOrDefault(n=>n.Id == x.Id))??
                Repository.Nds.SingleOrDefault(n => n.Id == _prevNdsMemorize) ?? Repository.Nds.FirstOrDefault();
        }

        private Ndsalgorithm GetPredefinedNdsAlgorithm()
        {
            var contractAlg = CurrentContract.Ndsalgorithm;
            return NdsAlgorithms.SingleOrDefault(n => n.Id == _prevNdsAlgorithmMemorize)??
                   contractAlg.With(x=>NdsAlgorithms.SingleOrDefault(n => n.Id == x.Id))??NdsAlgorithms.FirstOrDefault();
        }

        private void SubscribeActEvents(Act currentAct, Act value)
        {
            currentAct.Do(x => x.PropertyChanged -= ActPropertyChanged);
            value.Do(x => x.PropertyChanged += ActPropertyChanged);
        }

        private void ActPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Signdate")
                _prevActDateMemorize = CurrentAct.Signdate;
            if (e.PropertyName == "Issigned")
                _prevActIssignMemorizer = CurrentAct.Issigned;
            if (e.PropertyName == "Ndsalgorithm")
                _prevNdsAlgorithmMemorize = CurrentAct.Ndsalgorithm.Return(x=>x.Id, default(long?));
            if (e.PropertyName == "Nds")
                _prevNdsMemorize = CurrentAct.Nds.Return(x=>x.Id, default(long?));
            if (e.PropertyName == "Acttype")
                _prevActTypeMemorize = CurrentAct.Acttype.Return(x => x.Id, default(long?));

        }

        public Contractdoc CurrentContract
        {
            get { return _currentContract; }
            set
            {
                if (_currentContract == value) return;
                _currentContract = value;
                SetCurrentActContractObject();
                OnPropertyChanged(() => CurrentContract);
            }
        }

        /// <summary>
        ///   Получает или устанавливает коллекцию выделенных этапов в коллекции закрытых этапов
        /// </summary>
        public IList SelectedClosedStages { get; set; }

        /// <summary>
        ///   Получает или устанавливает коллекцию выделенных этапов в коллекции открытых этапов
        /// </summary>
        public IList SelectedOpenStages { get; set; }

        public int SelectedOpenStagesCount
        {
            get { return _openSelectedCount; }
            set
            {
                if (_openSelectedCount == value) return;
                _openSelectedCount = value;
                OnPropertyChanged(() => SelectedOpenStagesCount);
            }
        }

        public int SelectedClosedStagesCount
        {
            get { return _closedSelectedCount; }
            set
            {
                if (_closedSelectedCount == value) return;
                _closedSelectedCount = value;
                OnPropertyChanged(() => SelectedClosedStagesCount);
            }
        }

        /// <summary>
        ///   Получает коллекцию закрытых этапов
        /// </summary>
        public ObservableCollection<Stage> ClosedStages
        {
            get
            {
                if (_closedStages == null)
                {
                    _closedStages = new ObservableCollection<Stage>(FetchStages(CurrentAct));
                    _closedStages.CollectionChanged += ClosedStagesListChanged;
                }

                return _closedStages;
            }
        }


        /// <summary>
        ///   Получает коллекцию открытых этапов
        /// </summary>
        public ObservableCollection<Stage> OpenStages
        {
            get
            {
                if (_openStages == null)
                {
                    _openStages = new ObservableCollection<Stage>(FetchStages(null));
                    _openStages.CollectionChanged += OpenStagesListChanged;
                }
                return _openStages;
            }
        }

        /// <summary>
        ///   Получает число закрытых этапов
        /// </summary>
        public int ClosedStagesCount
        {
            get { return ClosedStages.Count; }
        }

        /// <summary>
        ///   Получает число открытых этапов
        /// </summary>
        public int OpenStagesCount
        {
            get { return OpenStages.Count; }
        }


        public override string DisplayName
        {
            get
            {
                return CurrentAct != null ? CurrentAct.FullName : base.DisplayName;
            }
            protected set { base.DisplayName = value; }
        }

        public ICommand ShowPaymentDocumentDialogCommand
        {
            get { return new RelayCommand(x => OnRequestShowPaymentDialog(EventArgs.Empty)); }
        }

        /// <summary>
        ///   Получает модель денег суммы к перечислению
        /// </summary>
        public MoneyModel TransferMoneyModel
        {
            get
            {
                Contract.Requires(CurrentAct != null);
                return CurrentAct.TransferSumMoney;
            }
        }

        public IEnumerable<Acttype> ActTypes
        {
            get { return Repository.Acttypes; }
        }

        /// <summary>
        ///   Возвращает единицы измерения денег в виде строки соответствие с данными КП Независимо от размерности валюты в КП, расчёт призводится в ед.
        /// </summary>
        public string ScheduleMeasure
        {
            get
            {
                Contract.Requires(CurrentContract != null);
                if (CurrentContract.Currency == null) return string.Empty;
                return CurrentContract.Currency.CI.NumberFormat.CurrencySymbol;
            }
        }

        /// <summary>
        ///   Получает символ национальной валюты. Независимо от валюты КП, расчёт итоговой суммы производится в национальной валюте
        /// </summary>
        public static string NationalMeasure
        {
            get { return Currency.National.CI.NumberFormat.CurrencySymbol; }
        }
        
        private bool CanQueryCurrencyRate
        {
            get { return CurrentContract != null && CurrentAct != null && CurrentAct.Ratedate.HasValue; }
        }

        /// <summary>
        ///   Получает команду запроса курса валют
        /// </summary>
        public ICommand QueryCurrencyRateCommand
        {
            get
            {
                return new RelayCommand(p => QueryCurrencyRate(),
                                        x =>
                                        CanQueryCurrencyRate);
            }
        }

        public IEnumerable<Region> Regions
        {
            get { return Repository.Regions; }
        }

        public IEnumerable<Enterpriseauthority> Enterpriseauthorities
        {
            get { return Repository.Enterpriseauthorities; }
        }

        public decimal Credited
        {
            get { return CurrentAct.Credited; }
            set
            {
                CurrentAct.Credited = value;
                OnPropertyChanged(() => Credited);
            }
        }

        /// <summary>
        /// Получает остатки по перечислениям
        /// </summary>
        public decimal PrepaymentRests
        {
            get
            {
                using (var ctx = RepositoryFactory.CreateContractRepository())
                {
                    //var contract = ctx.GetContractdoc(CurrentContract.Id);
                    //return contract.TransferedFunds - contract.Prepaymented + _initialCredited;
                    return ctx.GetPrepaymentRestsForAct(CurrentAct.Id, CurrentContract.Id);
                }
            }
        }

        #region ICacheble Members

        public void Invalidate()
        {
            _closedAmount = null;
        }

        #endregion

        public event EventHandler RequestShowEnterpriseAuthorityEditor;
        public event EventHandler RequestShowRegionsEditor;

        /// <summary>
        /// Вызывает событие запроса редакции справочника регионов
        /// </summary>
        /// <param name="e"></param>
        public void OnRequestShowRegionsEditor(EventArgs e)
        {
            var handler = RequestShowRegionsEditor;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Вызывает событие запроса редакции справочников оснований для организации
        /// </summary>
        /// <param name="e"></param>
        public void OnRequestShowEnterpriseAuthorityEditor(EventArgs e)
        {
            EventHandler handler = RequestShowEnterpriseAuthorityEditor;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        /// Расчитывает рекомендованый аванс по формуле:
        /// 1) Если остатки по авансу меньше суммы по закрытым этапам, то перечислить все остатки
        /// 2) Если остатки превышают сумму по этапам, то перечислить % от стоимости закрытых этапов в соответствие с заданным в договоре
        /// 3) Если закрывается последний этап договора, то перечисляется вся сумма остатка
        /// </summary>
        /// <param name="rest">Остстки по перечислениям</param>
        /// <param name="closedAmount">Сумма закрытых этапов актом</param>
        /// <param name="prepaymentPercents">Процент аванса по договору</param>
        /// <param name="theLastStage">Признак, что закрывается последний этап</param>
        /// <returns></returns>
        private static decimal GetRecomendedPrepayment(decimal rest, decimal closedAmount, double? prepaymentPercents,
                                                       bool theLastStage = false)
        {
            if (closedAmount >= rest || theLastStage) return rest;
            Contract.Assert(prepaymentPercents.HasValue);
            return closedAmount*(decimal) prepaymentPercents.Value/100M;
        }

        private void SetCurrentActContractObject()
        {
            if (CurrentAct != null && CurrentContract != null)
            {
                CurrentAct.ContractObject = CurrentContract;
            }
        }

        private void InstanceCurrencyRateOnDateCompleted(object sender, RateOnDateEventArgs e)
        {
            if (e.Error == null)
                CurrentAct.Currencyrate = e.Rate;
            else
            {
                OnQueryCurrencyFailed(new EventParameterArgs<Exception>(e.Error));
            }
        }

        public event EventHandler<EventParameterArgs<Exception>> QueryCurrencyFailed;

        public void OnQueryCurrencyFailed(EventParameterArgs<Exception> e)
        {
            var handler = QueryCurrencyFailed;
            if (handler != null) handler(this, e);
        }


        private long GetScheduleFactor()
        {
            Contract.Requires(CurrentSchedule != null);
            return CurrentSchedule.Currencymeasure == null ? 0 : CurrentSchedule.Currencymeasure.Factor.GetValueOrDefault();
        }

        /// <summary>
        ///  Событие возникает при обнаружении в коллекции открытых этапов осиротевшего, все подэтапы которого закрыты другими актами
        /// </summary>
        public event EventHandler<EventParameterArgs<Stage>> ForeignOrhantFound;

        public void OnForeignOrhantFound(EventParameterArgs<Stage> e)
        {
            var handler = ForeignOrhantFound;
            if (handler != null) handler(this, e);
        }

        /// <summary>
        ///   Получает в коллекции открытых этапов осиротевших
        /// </summary>
        /// <returns> Коллекция осиротевших этапов </returns>
        private IEnumerable<Stage> GetOrphannedStages()
        {
            return
                OpenStages.Where(s => s.Stages.Any() && s.Stages.All(x => x.Act != null));
 
        }

        /// <summary>
        ///   Метод переносит осиротевших родителей, которые могут появиться в результате закрытия всех дочерних подэтапов
        /// </summary>
        private void TransferOrphanedParents()
        {
            TransferListAndSetAct(GetOrphannedStages().ToList(), ClosedStages, CurrentAct, OpenStages);
        }

        private IList<Ndsalgorithm> _ndsalgorithms;
        private static long? _prevNdsAlgorithmMemorize;
        private static long? _prevNdsMemorize;

        /// <summary>
        /// Получает коллекцию алгоритмов, исключая неподходящий для актов тип "Кроме того НДС"
        /// </summary>
        public IList<Ndsalgorithm> NdsAlgorithms
        {
            get
            {
                return _ndsalgorithms ?? (_ndsalgorithms =
                                          new List<Ndsalgorithm>(
                                              Repository.Ndsalgorithms.Where(x => x.NdsType != TypeOfNds.ExcludeNds)).
                                              ToList());
            }
        }

        /// <summary>
        ///   Метод для каждого элемента переданной коллекции строит список зависимых этапов и добавляет их в результирующий список
        /// </summary>
        /// <param name="selectedList"> </param>
        /// <returns> </returns>
        private IList PrepareOpenList(IList selectedList)
        {
            var list = PrepareList(selectedList);
            return list;
        }

        private List<Stage> PrepareList(IList selectedList)
        {
            return
                selectedList.Cast<Stage>().SelectMany(
                    x => x.RightDepthSearch(z => z.Stages, p => p.Act == null || p.Act == CurrentAct)).ToList();
        }

        /// <summary>
        ///   Метод для каждого элемента переданой коллекции сначала определяет все зависимые элементы, а после добавляются элементы от которых зависит каждый из элементов
        /// </summary>
        /// <param name="selectedList"> </param>
        /// <returns> </returns>
        private IList PrepareClosedList(IList selectedList)
        {
            var closed = PrepareOpenList(selectedList);
            var parent =
                selectedList.Cast<Stage>().SelectMany(
                    x => x.GetBackwardPath(z => z.ParentStage, p => p.Act == null || p.Act == CurrentAct)).ToList();
            return parent.Concat(closed.Cast<Stage>()).ToList();
        }

        /// <summary>
        ///   Переносит этапы из заданного списка в целевой и модияицирует значение Act в соответствие с переданным в качестве аргумента
        /// </summary>
        /// <param name="selectedStages"> Исходный список этапов </param>
        /// <param name="targetList"> Целевой список </param>
        /// <param name="act"> Новое значение свойства Act после переноса </param>
        /// <param name="selectedImpact"> Коллекция из которой будут удалены элементы, соответствующие коллекции selectedStages </param>
        private void TransferListAndSetAct(IList selectedStages, IList targetList, Act act, IList selectedImpact = null)
        {
            Contract.Requires(selectedStages != null);
            Contract.Requires(targetList != null);
            Contract.Requires(act == CurrentAct || act == null);
            Contract.Ensures(selectedStages.Count == 0);
            foreach (Stage item in selectedStages.Cast<Stage>().Where(item => !targetList.Contains(item)))
            {
                targetList.Add(item);
                item.Act = act;
            }


            if (selectedImpact != null)
            {
                // Так как selectedStages и impactList могут являться проекцией одной коллекции, то перед модификацией selectedImpact 
                // необходимо создать копию.
                var selectedCopy = new object[selectedStages.Count];
                selectedStages.CopyTo(selectedCopy, 0);

                foreach (var selectedStage in selectedCopy)
                {
                    selectedImpact.Remove(selectedStage);
                }
            }
            selectedStages.Clear();
        }

        private void ClosedStagesListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Invalidate();

            OnPropertyChanged(() => ClosedStagesCount);
            OnPropertyChanged(() => ClosedStageAmount);

            RecalculateActSum();
        }


        /// <summary>
        ///   Что бы Contractdoc.PrepaymentRests не ушёл в минус,
        ///  приходится выполнять двойную работу: Сначала расчитать сумму к перечислению равной общей сумме, 
        /// затем расчитать рекомендованный аванс, что приведёт к пересчёту сумме к перечислению
        /// </summary>
        private void RecalculateActSum()
        {
            var closedStageAmountNational = MoneyModel.CurrencyToCurrency(CurrentContract.Currency,
                                                                              Currency.National, ClosedStageAmount,
                                                                              CurrentAct.Currencyrate);

            CurrentAct.Totalsum = closedStageAmountNational;
            CurrentAct.Sumfortransfer = closedStageAmountNational;

            NotifyTransferMoneyChanged();
        }

        private void NotifyTransferMoneyChanged()
        {
            OnPropertyChanged(() => TransferMoneyModel);
        }

        private void OpenStagesListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(() => OpenStagesCount);
            OnPropertyChanged(() => OpenStageAmount);
        }

        /// <summary>
        ///   Получает коллекцию этапов по заданному акту
        /// </summary>
        /// <param name="act"> акт или null, если требуется получить открытые этапы </param>
        /// <returns> Коллекция этапов </returns>
        private IEnumerable<Stage> FetchStages(Act act)
        {
            Contract.Requires(CurrentSchedule != null);
            return CurrentSchedule.Stages.Where(x => x.Act == act);
        }

        public event EventHandler RequestShowPaymentDialog;

        /// <summary>
        /// Вызывает событие запроса редактирования платёжных документов по договору
        /// </summary>
        /// <param name="e"></param>
        public void OnRequestShowPaymentDialog(EventArgs e)
        {
            var handler = RequestShowPaymentDialog;
            if (handler != null) handler(this, e);
        }

        protected override void Save()
        {
            Repository.SubmitChanges();
            Statistics.StatisticsUpdater.UpdateStatistics(CurrentContract.Id);
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_ACTS_SCHEDULE, CurrentAct);
        }

        protected override bool CanSave()
        {
            return CurrentAct != null && CurrentAct.Validate() == string.Empty;
        }

        protected override void OnWrappedDomainObjectChanged()
        {
            SetupActCurrency();
            RegisterActPropertyObserver();
            _openStages = null;
            _closedStages = null;
            OnPropertyChanged(() => ClosedStages);
            OnPropertyChanged(() => OpenStages);

        }

        private void SetupActCurrency()
        {
            if (CurrentContract == null || CurrentAct == null) return;
            CurrentAct.Currencymeasure = Repository.Currencymeasures.Single(x => x.Id == 1);
            CurrentAct.Currency = CurrentContract.Currency;
            _initialCredited = CurrentAct.Credited;
        }

        private void NotifyHeaderPropertyChanged()
        {
            OnPropertyChanged(() => DisplayName);
        }


        /// <summary>
        /// Получает или устанавливает свойство автоинкремента номера акта 
        /// </summary>
        public bool ActNumberAutoIncrement { get; set; }

        private void RegisterActPropertyObserver()
        {
            if (WrappedDomainObject == null || CurrentSchedule == null) return;


            // Если изменяются значения даты подписания, статуса подписания и номера акта, то уведомить об изменении заголовка (DisplayName)
            // Если изменяется алгоритм НДС или ставка НДС, то обновить модель денег суммы к перечислению
            // Если изменяется валюта, то пересчистать сумму к перечислению.
            _actPropertyChangedObjerver = new PropertyObserver<Act>(CurrentAct).RegisterHandler(x => x.Signdate,
                                                                                                x =>
                                                                                                NotifyHeaderPropertyChanged
                                                                                                    ()).
                RegisterHandler(x => x.Issigned, x => NotifyHeaderPropertyChanged()).RegisterHandler(x => x.Num,
                                                                                                  x =>
                                                                                                  NotifyHeaderPropertyChanged
                                                                                                      ()).
                RegisterHandler(x => x.Nds, x => NotifyTransferMoneyChanged()).RegisterHandler(x => x.Ndsalgorithm,
                                                                                             x =>
                                                                                             NotifyTransferMoneyChanged())
                .RegisterHandler(x => x.Currencyrate,
                                 x =>
                                      RecalculateActSum());
        }

        private void QueryCurrencyRate()
        {
            Contract.Assert(CurrentAct != null);
            Contract.Assert(CurrentContract != null);
            Contract.Assert(CurrentContract.Currency != null);
            Contract.Assert(!string.IsNullOrWhiteSpace(CurrentContract.Currency.Code));
            Contract.Assert(CurrentAct.Ratedate != null);
            Contract.Assert(CurrentAct.Ratedate.HasValue);

            _currencyService.AsyncGetCurrencyRateOnDate(CurrentAct.Ratedate.Value, CurrentContract.Currency.Code);
        }

        internal void ReloadRegions()
        {
            Repository.Refresh(RefreshMode.OverwriteCurrentValues, Repository.Regions);
            OnPropertyChanged(() => Regions);
        }

        internal void ReloadEnterpriseAuthority()
        {
            Repository.Refresh(RefreshMode.OverwriteCurrentValues, Repository.Enterpriseauthorities);
            OnPropertyChanged(() => Enterpriseauthorities);
        }

        internal void RefreshPaymnets()
        {
            Credited = 0;
            OnPropertyChanged(() => PrepaymentRests);
            RecalculateActSum();
        }



      
    }
}
