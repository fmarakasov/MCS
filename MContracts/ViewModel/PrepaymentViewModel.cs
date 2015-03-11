#region

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;

#endregion

namespace MContracts.ViewModel
{
    /// <summary>
    ///   Модель представления для работы с авансом договора
    /// </summary>
    public class PrepaymentViewModel : ContractdocBaseViewModel
    {
        private readonly RelayCommand _calcPercentsCommand;
        private readonly RelayCommand _calcSumCommand;
        private readonly ObservableCollection<Ndsalgorithm> _ndsAlgorithms;
        private PropertyObserver<Contractdoc> _handler1;
        private IBindingList _prepayments;

        public PrepaymentViewModel(IContractRepository repository) : base(repository)
        {
            _ndsAlgorithms = new ObservableCollection<Ndsalgorithm>(repository.Ndsalgorithms);
            _calcPercentsCommand = new RelayCommand(x => CalcPercents(), x => CanCalcPercents());
            _calcSumCommand = new RelayCommand(x => CalcSum(), x => CanCalcSum());
            
        }

        
        /// <summary>
        ///   Получает объект с инофрмацией о деньгах договора
        /// </summary>
        public MoneyModel ContractMoneyInfo
        {
            get { return ContractObject.ContractMoney; }
        }

        protected decimal PrepaymentPriceSelector
        {
            get
            {
                if (ContractObject.PrepaymentPrecentCalcType == PrepaymentCalcType.ProvidedWithFromNds)
                    return ContractMoneyInfo.WithNdsValue;
                if (ContractObject.PrepaymentPrecentCalcType == PrepaymentCalcType.ProvidiedWithFromPure)
                    return ContractMoneyInfo.PureValue;
                return ContractObject.Price.GetValueOrDefault();
            }
        }

        public bool SumEnabled
        {
            get { return ContractObject.PrepaymentPrecentCalcType == PrepaymentCalcType.ProvidedWithSum; }
        }

        public bool PercentsEnabled
        {
            get
            {
                return ContractObject.PrepaymentPrecentCalcType != PrepaymentCalcType.ProvidedWithSum &&
                       ContractObject.PrepaymentPrecentCalcType != PrepaymentCalcType.NotProvided;
            }
        }

        public float? Percents
        {
            get { return ContractObject.Prepaymentpercent; }
            set
            {
                if (value == ContractObject.Prepaymentpercent) return;
                ContractObject.Prepaymentpercent = value;
                OnPropertyChanged(()=>Percents);
            }
        }

        public decimal? Sum
        {
            get { return ContractObject.Prepaymentsum; }
            set
            {
                if (value == ContractObject.Prepaymentsum) return;
                ContractObject.Prepaymentsum = value;
                OnPropertyChanged(()=>Sum);
            }
        }

        /// <summary>
        ///   Получает признак доступности выбора алгоритма НДС для аванса
        /// </summary>
        public bool NdsAlgorithmEnabled
        {
            get
            {
                Contract.Requires(ContractObject != null);
                return ContractObject.Ndsalgorithm != null && ContractObject.Ndsalgorithm.NdsType != TypeOfNds.NoNds;
            }
        }

        /// <summary>
        ///   Получает заголовок поля Год в таблице авансов с учётом даты начала и окончания работ по договору
        /// </summary>
        public string PrepaymentYearHeader
        {
            get
            {
                Contract.Requires(ContractObject != null);
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                var sb = new StringBuilder();
                sb.Append("Год");
                DateTime? start = ContractObject.Startat;
                if ((start.HasValue) && (ContractObject.Endsat.HasValue))
                {
                    sb.Append(" (");
                    sb.Append(start.Value.Year.ToString(CultureInfo.InvariantCulture));
                    sb.Append(" - ");
                    sb.Append(ContractObject.Endsat.Value.Year.ToString(CultureInfo.InvariantCulture));
                    sb.Append(")");
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///   Получает заголовок поля Сумма в таблице авансов с учётом выбранногй валюты и единиц измерения
        /// </summary>
        public string PrepaymentSumHeader
        {
            get
            {
                Contract.Requires(ContractObject != null);
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
                var sb = new StringBuilder();
                sb.Append("Сумма аванса");
                sb.Append("(");

                sb.AppendFormat(
                    ContractObject.Currencymeasure != null
                        ? ContractObject.Currencymeasure.CurrencyMeasureFormat
                        : Currencymeasure.FormValue(1).CurrencyMeasureFormat,
                    ContractObject.CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
                sb.Append(")");


                if (ContractObject.Prepaymentndsalgorithm != null)
                {
                    sb.Append(" - " + ContractObject.Prepaymentndsalgorithm.Pricetooltip);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///   Получает сумму аванса по годам
        /// </summary>
        public decimal SumSpreaded
        {
            get
            {
                Contract.Requires(ContractObject != null);
                return PrepaymentsList.OfType<Prepayment>().Sum(x => x.Sum);
            }
        }

        /// <summary>
        ///   Получает процент аванса по годам
        /// </summary>
        public float PercentsSpreaded
        {
            get
            {
                Contract.Requires(ContractObject != null);
                return PrepaymentsList.OfType<Prepayment>().Sum(x => x.AutoPercent);
            }
        }

        public string SpreadedInfo
        {
            get
            {
                Contract.Requires(ContractObject != null);
                if (ContractObject.Currency != null)
                {
                    return string.Format("Распределено от суммы договора - {0} ({1:N2}%), от суммы аванса - {2:N2}% ",
                                         ContractObject.Currency.FormatMoney(SumSpreaded), PercentsSpreaded,
                                         PrepaymentPercentsSpreaded);
                }
                return string.Empty;
            }
        }

        public ObservableCollection<Ndsalgorithm> NdsAlgorithms
        {
            get { return _ndsAlgorithms; }
        }

        public IBindingList PrepaymentsList
        {
            get
            {
                if (_prepayments == null)
                {
                    _prepayments = ContractObject.Prepayments.GetNewBindingList();
                    _prepayments.ListChanged += PrepaymentsListChanged;
                }
                return _prepayments;
            }
        }

        /// <summary>
        ///   Получает команду расчёта процентов предоплаты
        /// </summary>
        public RelayCommand CalcPercentsCommand
        {
            get { return _calcPercentsCommand; }
        }

        /// <summary>
        ///   Получает команду расчёта суммы предоплаты
        /// </summary>
        public RelayCommand CalcSumCommand
        {
            get { return _calcSumCommand; }
        }

        /// <summary>
        ///   Получает объект с инофрмацией о деньгах аванса
        /// </summary>
        public MoneyModel PrepaymentMoneyInfo
        {
            get { return ContractObject.PrepaymentMoney; }
        }

        public float PrepaymentPercentsSpreaded
        {
            get
            {
                Contract.Requires(ContractObject != null);
                if (ContractObject.Prepaymentsum.HasValue && ContractObject.Prepaymentsum.Value > 0)
                    return Percent.GetPercent(SumSpreaded, ContractObject.Prepaymentsum.Value);
                return 0.0F;
            }
        }

        private bool CanCalcPercents()
        {
            return ContractObject.Prepaymentsum.HasValue && ContractObject.Price.HasValue;
        }

        private void CalcPercents()
        {
            Contract.Assert(ContractObject.Prepaymentsum.HasValue);
            Contract.Assert(ContractObject.Prepaymentsum.Value > 0);
            Contract.Assert(ContractObject.Price.HasValue);

            Percents = Percent.GetPercent(ContractObject.Prepaymentsum.Value,
                                          ContractObject.Price.Value);
        }

        private bool CanCalcSum()
        {
            return ContractObject.Prepaymentpercent.HasValue && ContractObject.Price.HasValue;
        }

        private void CalcSum()
        {
           
            if (ContractObject.Prepaymentpercent.HasValue)
                Sum = Percent.Inverse(ContractObject.Prepaymentpercent.Value,
                                      PrepaymentPriceSelector);
        }

        private void PrepaymentPropertyChanged()
        {
            NotifyPrepaymentPropertiesChanged();


            // Пересчитать зависимые данные: если вводятся проценты - обновить сумму, если вводится сумма - проценты
            RecalcDependables();
        }

        private void NotifyPrepaymentPropertiesChanged()
        {
            OnPropertyChanged(() => PrepaymentYearHeader);
            OnPropertyChanged(() => PrepaymentSumHeader);
            OnPropertyChanged(() => NdsAlgorithmEnabled);
            OnPropertyChanged(() => PrepaymentMoneyInfo);
            OnPropertyChanged(() => SpreadedInfo);
        }

        private void RecalcDependables()
        {
            if (SumEnabled)
            {
                CalcPercentsCommand.Execute(this);
            }
            if (PercentsEnabled)
            {
                CalcSumCommand.Execute(this);
            }
        }

        private void SendAutoPercentChanged()
        {
            NotifyPrepaymentPropertiesChanged();

            foreach (var item in ContractObject.Prepayments)
            {
                item.InvalidateAutoProperties();
            }
        }

        private void SetPrepaymentNdsAlgorithmFitContractAlg()
        {
            // Если нельзя изменять алгоритм НДС для аванса, то устноваить его в соответствие с алгоритмом договора
            if (!NdsAlgorithmEnabled)
                ContractObject.Prepaymentndsalgorithm = ContractObject.Ndsalgorithm;
        }


        protected override void OnWrappedDomainObjectChanged()
        {
            RegisterPropertyChangedHandler();
            base.OnWrappedDomainObjectChanged();
        }


        private void PrepaymentsListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                var bindingList = sender as IBindingList;
                if (bindingList != null)
                {
                    var prepayment = (Prepayment) (bindingList[e.NewIndex]);
                    prepayment.CurrenctContract = ContractObject;
                }
            }
            OnPropertyChanged(()=>SumSpreaded);
            OnPropertyChanged(()=>PercentsSpreaded);
            OnPropertyChanged(()=>SpreadedInfo);
        }

       

        private void RegisterPropertyChangedHandler()
        {
            _handler1 = new PropertyObserver<Contractdoc>(ContractObject).RegisterHandler(x => x.Startat,
                                                                                          x =>
                                                                                          ContractDatesChanged()).
                RegisterHandler(x => x.Endsat, x => ContractDatesChanged()).RegisterHandler(
                    x => x.Currencymeasure, x => PrepaymentPropertyChanged()).RegisterHandler(x => x.Currency,
                                                                                              x =>
                                                                                              PrepaymentPropertyChanged())
                .RegisterHandler(
                    x => x.Prepaymentndsalgorithm, x => PrepaymentPropertyChanged()).RegisterHandler(
                        x => x.Prepaymentsum,
                        x =>
                        PrepaymentPropertyChanged())
                .RegisterHandler(x => x.Prepaymentpercent, x => PrepaymentPropertyChanged()).RegisterHandler(
                    x => x.Price, x => PrepaymentPropertyChanged()).RegisterHandler(x => x.PrepaymentPrecentCalcType,
                                                                                    x => PrepaymentTypePropertyChanged())
                .
                RegisterHandler(x => x.Ndsalgorithm,
                                x => ContractNdsAlgorithmChanged());
        }

        private void ContractDatesChanged()
        {
            //Обновить проценты в авансировании по годам
            SendAutoPercentChanged();
        }

        private void ContractNdsAlgorithmChanged()
        {
            NotifyPrepaymentPropertiesChanged();
            SetPrepaymentNdsAlgorithmFitContractAlg();
        }

        /// <summary>
        ///   При изменеии типа процентов пересчитать сумму аванса
        /// </summary>
        private void PrepaymentTypePropertyChanged()
        {
            // Rule: Если тип договора НИОКР, то установить процентную ставку аванса в 30%, если выбран метод ввода процентов и проценты ещё не заданы
            SetDefaultPercents();

            OnPropertyChanged(()=>SumEnabled);
            OnPropertyChanged(()=>PercentsEnabled);

            if (CanCalcSum())
                CalcSum();
        }

        private void SetDefaultPercents()
        {
            if (IsPrepaymentPercentType() && IsNiokrType())
            {
                if (PercentsNotProvided())
                {
                    ContractObject.Prepaymentpercent = Properties.Settings.Default.DefaultPrepaymentPercent;
                }
            }
        }

        private bool IsNiokrType()
        {
            if (ContractObject.Contracttype!= null)
                return ContractObject.Contracttype.WellKnownType == WellKnownContractTypes.Niokr;
            return false;
        }

        private bool PercentsNotProvided()
        {
            return !ContractObject.Prepaymentpercent.HasValue;
        }

        private bool IsPrepaymentPercentType()
        {
            return PrepaymentCalcType.ProvidedWithFromNds == ContractObject.PrepaymentPrecentCalcType || PrepaymentCalcType.ProvidiedWithFromPure == ContractObject.PrepaymentPrecentCalcType;
        }

        #region Overrides of RepositoryViewModel

        /// <summary>
        ///   Переопределите для задания логики сохранения изменений в модели
        /// </summary>
        protected override void Save()
        {
            //Contract.Requires(ContractObject != null);
            //Repository.DeletePrepayments(ContractObject.Id);
            //Repository.SubmitChanges();
            //Repository.AddContractPrepayments(PrepaymentsList.AsEntities());
            //
            //Repository.SubmitChanges();
        }

        /// <summary>
        ///   Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return true;
        }

        #endregion
    }
}