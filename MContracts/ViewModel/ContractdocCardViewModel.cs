using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MCDomain.Services;
using MContracts.Classes;
using UIShared.Commands;
using MContracts.View;
using MContracts.ViewModel.Helpers;
using MediatorLib;
using MContracts.Controls.Dialogs;
using UIShared.ViewModel;
using McUIBase.Factories;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Модель представления для ввода основных данных по договору: предмет, даты, цена, аванс, тип и состояние.
    /// </summary>
    public class ContractdocCardViewModel : ContractdocBaseViewModel
    {
        private readonly EventHandler<RateOnDateEventArgs> _currencyRateCallbackhander;
        private readonly CurrencyService _currencyService = new CurrencyService();
        private readonly PrepaymentViewModel _prepaymentViewModel;

        private ObservableCollection<Contractor> _contractors;
        private ObservableCollection<Contracttype> _contracttypes;
        private ObservableCollection<Currency> _currencies;
        private ObservableCollection<Currencymeasure> _currencymeasures;
        private Contractor _customerContractor;
        private ObservableCollection<Enterpriseauthority> _enterpriseauthorities;
        private ObservableCollection<Nds> _nds;
        private ObservableCollection<Ndsalgorithm> _ndsalgorithms;
        private RelayCommand _queryCurrencyRateCommand;

        private ObservableCollection<Contractstate> _states;

        public ContractdocCardViewModel(IContractRepository repository, ViewModelBase owner) : base(repository, owner)
        {
            _prepaymentViewModel = new PrepaymentViewModel(repository);
            _currencyRateCallbackhander = Instance_CurrencyRateOnDateCompleted;
            _currencyService.CurrencyRateOnDateCompleted += _currencyRateCallbackhander;
            
            
            var contractdocCardViewModel = Owner as ContractViewModel;
            Contract.Assert(contractdocCardViewModel != null);
            contractdocCardViewModel.Saved += ContractdocCardViewModel_Saved;
        }

        void ContractdocCardViewModel_Saved(object sender, EventArgs e)
        {
            _disposalsrepo = null;
            _contractobjectfordisposal = null;
            _disposal = null;
        }

        
        public string ContragentGroupboxName
        {
            get
            {
                if (ContractObject != null)
                    return ContractObject.IsGeneral ? "Заказчик" : "Исполнитель";
                return string.Empty;
            }
        }

        public string ContragentComboName
        {
            get
            {
                if (ContractObject != null)
                    return ContractObject.IsGeneral
                               ? "Со стороны Заказчика договор подписал"
                               : "Со стороны Исполнителя договор подписал";
                return string.Empty;
            }
        }


        /// <summary>
        /// Получает команду запроса курса валют
        /// </summary>
        public ICommand QueryCurrencyRateCommand
        {
            get
            {
                return _queryCurrencyRateCommand ??
                       (_queryCurrencyRateCommand = new RelayCommand(p => QueryCurrencyRate(),
                                                                     x =>
                                                                     CanQueryCurrencyRate));
            }
        }


        private ICommand _adddisposalcommand;

        public ICommand AddDisposalCommand
        {
            get
            {
                return _adddisposalcommand ??
                       (_adddisposalcommand = new RelayCommand(p => AddDisposal(), x => CanAddDisposal));
            }
        }

        private ICommand _editdisposalcommand;

        public ICommand EditDisposalCommand
        {
            get
            {
                return _editdisposalcommand ??
                       (_editdisposalcommand = new RelayCommand(p => EditDisposal(), x => CanEditDisposal));
            }
        }

        private ICommand _deletedisposalcommand;

        public ICommand DeleteDisposalCommand
        {
            get
            {
                return _deletedisposalcommand ??
                       (_deletedisposalcommand = new RelayCommand(p => DeleteDisposal(), p => CanDeleteDisposal));
            }
        }

        private bool CanQueryCurrencyRate
        {
            get
            {
                return ContractObject != null && ContractObject.Ratedate.HasValue && ContractObject.Currency != null &&
                       ContractObject.Currency.IsForeign;
            }
        }

        protected override void OnWrappedDomainObjectChanged()
        {
            Contract.Assert(ContractObject != null);
            PrepaymentViewModel.WrappedDomainObject = ContractObject;
            RegisterPropertyChangedHandlers();
            if (ContractObject.Contractor != null) Contractortype = ContractObject.Contractor.Contractortype;
            OnPropertyChanged(() => DisposalName);
            base.OnWrappedDomainObjectChanged();
        }

        private PropertyObserver<Contractdoc> _handler1;
        private PropertyObserver<Contractdoc> _handler2;

        private void RegisterPropertyChangedHandlers()
        {
            Contract.Assert(ContractObject != null);
            _handler1 = new PropertyObserver<Contractdoc>(ContractObject).RegisterHandler(x => x.Nds,
                                                                                          x => CurrencyChanged()).
                RegisterHandler(n => n.Currency, n => CurrencyChanged()).RegisterHandler(n => n.Price,
                                                                                         n => CurrencyChanged())
                .RegisterHandler(n => n.Ndsalgorithm, n => CurrencyChanged()).RegisterHandler(
                    x => x.Currencyrate,
                    x =>
                    CurrencyChanged()).RegisterHandler(x => x.Currencymeasure, x => CurrencyChanged()).RegisterHandler(
                        x => x.PrepaymentPrecentCalcType, x => CurrencyChanged());

            _handler2 = new PropertyObserver<Contractdoc>(ContractObject).RegisterHandler(x => x.Internalnum,
                                                                                          x => GlobalInfoChanged()).
                RegisterHandler(x => x.Contractornum, x => GlobalInfoChanged()).
                RegisterHandler(x => x.Approvedat, x => GlobalInfoChanged()).RegisterHandler(x => x.Subject,
                                                                                             x =>
                                                                                             GlobalInfoChanged())
                .RegisterHandler(x => x.Contractstate, x => GlobalInfoChanged()).RegisterHandler(x => x.Endsat,
                                                                                                 x =>
                                                                                                 GlobalInfoChanged
                                                                                                     ()).RegisterHandler
                (x => x.Agreementnum, x => GlobalInfoChanged());

        }

        /// <summary>
        /// Получает объект с инофрмацией о деньгах договора
        /// </summary>
        public MoneyModel ContractMoneyInfo
        {
            get { return ContractObject.ContractMoney; }
        }

        public bool IsPrepaymentViewEnabled
        {
            get { return ContractObject.PrepaymentPrecentCalcType != PrepaymentCalcType.NotProvided; }
        }

        /// <summary>
        /// Получает объект с инофрмацией о деньгах договора
        /// </summary>
        public MoneyModel RublesMoneyInfo
        {
            get
            {
                Contract.Requires(ContractMoneyInfo != null);
                return ContractMoneyInfo.Factor.AsNational(ContractObject.Currencyrate);
            }
        }

        /// <summary>
        /// Получает состояние видимости для строки цены договора в национальной валюте
        /// </summary>
        public Visibility FormatedRubblesPriceVisibility
        {
            get { return IsForeignCurrencyRateSet() ? Visibility.Visible : Visibility.Collapsed; }
        }

        /// <summary>
        /// Получает состояние видимости для элементов управления, связанных с договором в иностранной валюте
        /// </summary>
        public bool ForeignCurrencyVisibility
        {
            get
            {
                Contract.Requires(ContractObject != null);
                return (ContractObject.Currency != null) && (ContractObject.Currency.IsForeign);
            }
        }

        public Ndsalgorithm Ndsalgorithm
        {
            get { return ContractObject.Ndsalgorithm; }
            set
            {
                if (ContractObject.Ndsalgorithm == value) return;
                ContractObject.Ndsalgorithm = value;
                OnPropertyChanged(() => HasNds);
            }
        }

        /// <summary>
        /// Возвращает признак, что цена договра указана с НДС
        /// </summary>
        public bool HasNds
        {
            get
            {
                Contract.Requires(ContractObject != null);
                var alg = ContractObject.Ndsalgorithm;
                if (alg == null) return false;
                var type = alg.NdsType;
                return type != TypeOfNds.NoNds && type != TypeOfNds.Undefinded;
            }
        }

        private IEnumerable<string> _prepaymentCalcTypes;

        public IEnumerable<string> PrepaymentCalcTypes
        {
            get
            {
                return _prepaymentCalcTypes ??
                       (_prepaymentCalcTypes =
                        Enum.GetValues(typeof (PrepaymentCalcType)).Cast<PrepaymentCalcType>().Select(
                            x => x.Description()));
            }
        }

        /// <summary>
        /// Получает форматированную цену по договору
        /// </summary>
        public string FormatedPrice
        {
            get
            {
                Contract.Requires(ContractObject != null);
                return ContractObject.FormatedPrice;
            }
            set
            {
                Contract.Requires(ContractObject != null);
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ContractObject.Price = decimal.Parse(value, NumberStyles.Currency);
                }
            }
        }



        /// <summary>
        /// Получает или устанавливает контрагента по договору
        /// </summary>
        public Contractor CustomerContractor
        {
            get { return _customerContractor; }
            set
            {
                if (value == _customerContractor) return;
                _customerContractor = value;
                OnPropertyChanged("CustomerContractor");
            }
        }

        private ObservableCollection<Troublesregistry> _troublesRegistry;

        public ObservableCollection<Troublesregistry> TroublesRegistry
        {
            get
            {
                return _troublesRegistry ??
                       (_troublesRegistry = new ObservableCollection<Troublesregistry>(Repository.TroublesRegistry));
            }
        }

        private Troublesregistry _selectedTroublesRegistry;

        public Troublesregistry SelectedTroublesRegistry
        {
            get { return _selectedTroublesRegistry; }
            set
            {
                if (_selectedTroublesRegistry == value) return;
                _selectedTroublesRegistry = value;
                OnPropertyChanged("SelectedTroublesRegistry");
            }
        }


        /// <summary>
        /// Получает доступ к коллекции состояний договоров
        /// </summary>
        public ObservableCollection<Contractstate> States
        {
            get { return _states ?? (_states = new ObservableCollection<Contractstate>(Repository.States)); }
        }

        /// <summary>
        /// Получает доступ к коллекции ставок НДС
        /// </summary>
        public ObservableCollection<Nds> Ndses
        {
            get { return _nds ?? (_nds = new ObservableCollection<Nds>(Repository.Nds)); }
        }

        private ObservableCollection<Acttype> _acttypes;
        public ObservableCollection<Acttype> Acttypes
        {
            get { return _acttypes ?? (_acttypes = new ObservableCollection<Acttype>(Repository.Acttypes)); }
        }


        /// <summary>
        /// Получает доступ к коллекции валют
        /// </summary>
        public ObservableCollection<Currency> Currencies
        {
            get { return _currencies ?? (_currencies = new ObservableCollection<Currency>(Repository.Currencies)); }
        }

        /// <summary>
        /// Получает доступ к коллекции алгоритмов НДС
        /// </summary>
        public ObservableCollection<Ndsalgorithm> Ndsalgorithms
        {
            get
            {
                return _ndsalgorithms ??
                       (_ndsalgorithms = new ObservableCollection<Ndsalgorithm>(Repository.Ndsalgorithms));
            }
        }


        /// <summary>
        /// Получает доступ к коллекции оснований для предприятия
        /// </summary>
        public ObservableCollection<Enterpriseauthority> Enterpriseauthorities
        {
            get
            {
                return _enterpriseauthorities ??
                       (_enterpriseauthorities =
                        new ObservableCollection<Enterpriseauthority>(Repository.Enterpriseauthorities));
            }
        }

        /// <summary>
        /// Получает доступ к коллекции типов договоров
        /// </summary>
        public ObservableCollection<Contracttype> Contracttypes
        {
            get
            {
                return _contracttypes ??
                       (_contracttypes = new ObservableCollection<Contracttype>(Repository.Contracttypes));
            }
        }

        /// <summary>
        /// Получает доступ к коллекции единиц измерения денег
        /// </summary>
        public ObservableCollection<Currencymeasure> Currencymeasures
        {
            get
            {
                return _currencymeasures ??
                       (_currencymeasures = new ObservableCollection<Currencymeasure>(Repository.Currencymeasures));
            }
        }

        /// <summary>
        /// Получает доступ к коллекции контрагентов
        /// </summary>
        public ObservableCollection<Contractor> Contractors
        {
            get
            {
                return _contractors ??
                       (_contractors =
                        new ObservableCollection<Contractor>(Repository.Contractors.OrderBy(p => p.Name,
                                                                                            StringComparer.
                                                                                                CurrentCultureIgnoreCase)));
            }
        }

        private Contractortype _contractortype;

        public Contractortype Contractortype
        {
            get { return _contractortype; }
            set
            {
                _contractortype = value;
                OnPropertyChanged(() => Contractortype);
            }
        }

        private ObservableCollection<Contractortype> _contractortypes;

        /// <summary>
        /// Получает доступ к коллекции типов контрагентов
        /// </summary>
        public ObservableCollection<Contractortype> Contractortypes
        {
            get
            {
                return _contractortypes ??
                       (_contractortypes =
                        new ObservableCollection<Contractortype>(Repository.Contractortypes.OrderBy(p => p.Name,
                                                                                                    StringComparer.
                                                                                                        CurrentCultureIgnoreCase)));
            }
        }



        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof (CatalogType))]
        internal void RefreshContractors(CatalogType c)
        {
            if (c == CatalogType.Contractor)
            {
                _contractors = null;
                OnPropertyChanged("Contractors");
            }
        }



        public PrepaymentViewModel PrepaymentViewModel
        {
            get { return _prepaymentViewModel; }
        }

        public Visibility FormatedRubblesVisibility
        {
            get
            {
                Contract.Requires(ContractObject!=null);
                return IsForeignOrFactorMoreThenOne()
                           ? Visibility.Visible
                           : Visibility.Collapsed;
            }
        }

        private bool IsForeignOrFactorMoreThenOne()
        {
            return (((ContractObject.Currency != null) && (ContractObject.Currency.IsForeign)) ||
                    ((ContractObject.Currencymeasure != null) && (ContractObject.Currencymeasure.Factor != 1)));
        }

        private void QueryCurrencyRate()
        {
            Contract.Requires(ContractObject != null);
            Contract.Requires(ContractObject.Ratedate.HasValue);
            Contract.Requires(ContractObject.Currency != null);

            if (ContractObject.Ratedate != null)
                _currencyService.AsyncGetCurrencyRateOnDate(ContractObject.Ratedate.Value, ContractObject.Currency.Code);
        }


        private void Instance_CurrencyRateOnDateCompleted(object sender, RateOnDateEventArgs e)
        {
            if (e.Error != null)
            {
                var sb = new StringBuilder();
                sb.Append("Ошибка запроса данных службы валют ЦБР: ");
                sb.Append(e.Error.Message);
                if (e.OriginalCursOnDateEventArgs.Error != null)
                {
                    sb.Append(" Текст внутренней ошибки: " + e.OriginalCursOnDateEventArgs.Error.Message);
                }

                // Убрать MessageBox
                AppMessageBox.Show(sb.ToString(), MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            ContractObject.Currencyrate = e.Rate;
        }

        private void GlobalInfoChanged()
        {
            var owner = Owner as ContractViewModel;
            if (owner != null)
                owner.Invalidate();

            //OnPropertyChanged("DisplayName");            
        }

        private void CurrencyChanged()
        {
            OnPropertyChanged(()=>FormatedPrice);
            OnPropertyChanged(()=>ForeignCurrencyVisibility);
            OnPropertyChanged(()=>FormatedRubblesPriceVisibility);
            OnPropertyChanged(()=>ContractMoneyInfo);
            OnPropertyChanged(()=>FormatedRubblesVisibility);
            OnPropertyChanged(()=>RublesMoneyInfo);
            OnPropertyChanged(()=>IsPrepaymentViewEnabled);
        }

        private bool IsForeignCurrencyRateSet()
        {
            Contract.Assert(ContractObject != null);
            return IsForeignCurrency() && ContractObject.Currencyrate.HasValue;
        }


        private bool IsForeignCurrency()
        {
            Contract.Assert(ContractObject != null);
            return (ContractObject.Currency != null) && (ContractObject.Currency.IsForeign);
        }


        #region Overrides of RepositoryViewModel

        /// <summary>
        /// Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
            ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, Disposal);                     
        }

        /// <summary>
        /// Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return Error == String.Empty;
        }

        public override string Error
        {
            get
            {
                return ContractObject.Error;
            }
        }

        #endregion



        #region DisposalRegion

        [MediatorMessageSink(RequestRepository.DISPOSALS_CHANGED, ParameterType = typeof(Disposal))]
        internal void RefreshDisposal(Disposal disposal)
        {

            Disposal = disposal;
            OnPropertyChanged(() => DisposalName);
            OnPropertyChanged(() => CanAddDisposal);
            OnPropertyChanged(() => CanDeleteDisposal);
            OnPropertyChanged(() => CanEditDisposal);

        }

        private IContractRepository _disposalsrepo;
        public IContractRepository DisposalsRepository
        {
            get 
            { 
                return _disposalsrepo ?? (_disposalsrepo = RepositoryFactory.CreateContractRepository()); 
            }
        }

        private Disposal _disposal;
        public Disposal Disposal
        {
            get
            {
                return _disposal ??
                       (_disposal =
                        DisposalsRepository.Disposals.FirstOrDefault(
                            d =>
                            ContractObject != null && ContractObject.Disposal != null &&
                            d.Id == ContractObject.Disposal.Id));
            }
            set
            {
                if (_disposal == value) return;
                _disposal = value;
            }
        }

        private Contractdoc _contractobjectfordisposal;
        
        public Contractdoc ContractObjectForDisposal
        {
            get { return _contractobjectfordisposal ?? (_contractobjectfordisposal = DisposalsRepository.Contracts.FirstOrDefault(c => ContractObject != null && c.Id == ContractObject.Id)); }
        }

        private void DeleteDisposal()
        {
            if (
                AppMessageBox.Show("Вы действительно хотите безвозвратно удалить распоряжение?", 
                                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            if (Disposal != null)
            {
                DisposalsRepository.DeleteDisposal(Disposal);
                DisposalsRepository.SubmitChanges();
            }

            ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, (Disposal)null);
        }

        private void EditDisposal()
        {
            // AddActWindow dlg = new AddActWindow(RepositoryFactory.CreateContractRepository())
            // если мы находимся на оригинальном договоре
            // или на соглашении со своим распоряжением - просто редактируем его
            var dlg = new DialogShell();

            DisposalContentViewModel vm = null;
            // редактируем свое собственное распоряжение
            if (ContractObjectForDisposal.HasOwnDisposal)
            {
                vm = new DisposalContentViewModel(DisposalsRepository)
                {
                    Disposal = this.Disposal,
                    OriginDisposal = null,
                    ContractObject = ContractObjectForDisposal
                };
            }
            // редактируем распоряжение оригинального договора
            else
            {

                var mbresult =
                    AppMessageBox.Show(
                        "За текущим ДС автоматически закреплено распоряжение оригинального договора. Вы действительно хотите редактировать его?",
                         MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (mbresult == MessageBoxResult.No) return;

                vm = new DisposalContentViewModel(DisposalsRepository)
                {
                    Disposal =
                        DisposalsRepository.Disposals.FirstOrDefault(
                            d => ContractObject.OriginalContract != null && ContractObject.OriginalContract.Disposal != null && d.Id == ContractObject.OriginalContract.Disposal.Id),
                    OriginDisposal = null,
                    ContractObject = DisposalsRepository.Contracts.FirstOrDefault(c => ContractObject.OriginalContract != null && c.Id == ContractObject.OriginalContract.Id)
                };
            }

            dlg.ViewModel = vm;
            dlg.Content = new DisposalContentView { DataContext = vm };
            dlg.ShowDialog();


            OnPropertyChanged(()=>DisposalName);
            ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, vm.Disposal);

        }

        private void AddDisposal()
        {
            //AddActWindow dlg = new AddActWindow(RepositoryFactory.CreateContractRepository())
            // если мы находимся на оригинальном договоре
            // просто создаем новое распоряжение
            if (!ContractObjectForDisposal.IsAgreement || (ContractObjectForDisposal.IsAgreement && ContractObjectForDisposal.Disposal == null))
            {
                var dlg = new DialogShell();


                var vm = new DisposalContentViewModel(DisposalsRepository)
                {
                    Disposal = null,
                    OriginDisposal = null,
                    ContractObject = ContractObjectForDisposal
                };

                dlg.ViewModel = vm;
                dlg.Content = new DisposalContentView { DataContext = vm };
                dlg.ShowDialog();
                ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, vm.Disposal);
            }
            else // если это ДС - надо предупредить что будет создано новое распоряжение и спросить включить старые данные или нет
            {
                var mbresult =
                    AppMessageBox.Show(
                        "Для текущего ДС будет создано новое распоряжение. Скопировать в создаваемое распоряжение данные распоряжения основного договора?",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (mbresult != MessageBoxResult.Cancel)
                {
                    var dlg = new DialogShell();
                    DisposalContentViewModel vm = null;

                    if (mbresult == MessageBoxResult.No)
                    {

                        vm = new DisposalContentViewModel(DisposalsRepository)
                        {
                            Disposal = null,
                            OriginDisposal = null,
                            ContractObject = ContractObjectForDisposal
                        };


                    }
                    else
                    {
                        vm = new DisposalContentViewModel(DisposalsRepository)
                        {

                            Disposal = null,
                            OriginDisposal =
                                DisposalsRepository.Disposals.FirstOrDefault(
                                    d =>
                                    ContractObject.Disposal != null && d.Id == ContractObject.Disposal.Id),
                            ContractObject = ContractObjectForDisposal
                        };
                    }

                    dlg.ViewModel = vm;
                    dlg.Content = new DisposalContentView { DataContext = vm };
                    dlg.ShowDialog();
                    ViewMediator.NotifyColleagues(RequestRepository.DISPOSALS_CHANGED, vm.Disposal);
                }
            }
        }


        public bool CanAddDisposal
        {
            get
            {
                return ContractObjectForDisposal != null && (Disposal == null || !ContractObjectForDisposal.HasOwnDisposal);
            }
        }

        public bool CanEditDisposal
        {

            get
            {
                return (ContractObjectForDisposal != null && ((Disposal != null && ContractObjectForDisposal.HasOwnDisposal) || ContractObjectForDisposal.HasTheSameSchedules()));
            }
        }

        public bool CanDeleteDisposal
        {
            get
            {
                return (Disposal != null && ContractObjectForDisposal != null && ContractObjectForDisposal.HasOwnDisposal);
            }

        }

        public string DisposalName
        {
            get
            {
         
                var cr = ContractObjectForDisposal;

                if (cr == null || (!cr.HasOwnDisposal && cr.OriginalContract != null)) cr = DisposalsRepository.Contracts.FirstOrDefault(c => ContractObject != null && ContractObject.OriginalContract  != null && c.Id == ContractObject.OriginalContract.Id);
                if (cr != null) return cr.DisposalPersons;
                
                return "Вы сможете работать с распоряжением после того как сохраните созданный договор.";

            }
        }

        #endregion
        

    }
}
