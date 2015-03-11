using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using CommonBase;
using UOW;

namespace MCDomain.Model
{
    public partial class Contractrepositoryview : IDataErrorInfo, IContractStateData, ICacheble, IRepositoryEntity<long>
    {
        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает профиль (учёную степень, И. О. Фамилия) руководителя договора со стороны контрагента
        /// </summary>
        public string ContractorPersonProfile
        {
            get
            {
                var sb = new StringBuilder();
                if (EntityBase.IsReal(Degreeid))
                {
                    sb.AppendFormat("{0}, ", Degree.ConvertToShort(Persondegree));
                }
                sb.Append(Person.ToShortName(Personfamilyname, Personfirstname, Personmiddlename));
                return sb.ToString();
            }

        }

        /// <summary>
        /// Получает профиль (должность, И.О. Фамилия) руководителя договора со стороны Промгаза
        /// </summary>
        public string ManagerProfile
        {
            get
            {

                return Managernames;
            }
        }


        public string FullInternalnum
        {
            get
            {
                if (!IsAgreement)
                    return Internalnum;
                if (Agreementnum.HasValue)
                    return Internalnum + "(" + Agreementnum + ")";
                return Internalnum;
            }
        }



        /// <summary>
        /// Получает модель представления денег для договора
        /// </summary>
        public MoneyModel PriceMoneyModel
        {
            get
            {
                //if (_priceMoneyModel == null)
                var priceMoneyModel = new MoneyModel(Ndsalgorithm.TypeIdToObject(Ndsalgorithmid),
                                                             Nds.FromValue(Ndspercents.GetValueOrDefault()),
                                                             Currency.FromCulture(Culture),
                                                             Currencymeasure.FormValue(Factor), Price, Currencyrate);
                return priceMoneyModel;
            }
        }

        /// <summary>
        /// Получает признак, что версия договора явялется актуальной на которую нет ни одной ссылки с доп. соглашений
        /// </summary>
        public bool IsLastVersion
        {
            get { return Agreementreferencecount == 0; }
        }

        /// <summary>
        /// Получает признак, что договор является генеральным
        /// </summary>
        public bool IsGeneral
        {
            get { return Generalreferencecount == 0; }
        }

        /// <summary>
        /// Получает состояние подписания договора
        /// </summary>
        public WellKnownContractStates State
        {
            get { return Contractstate.IdToState(Contractstateid); }
        }



        /// <summary>
        /// Получает признак доп. соглашения
        /// </summary>
        public bool IsAgreement
        {
            get { return Origincontractid.GetValueOrDefault().ToBoolean(); }
        }

        /// <summary>
        /// Получает объект алгоритма НДС
        /// </summary>
        public Ndsalgorithm NdsAlgorithm
        {
            get { return Ndsalgorithm.TypeIdToObject(Ndsalgorithmid); }
        }

        //public IContractRepository ContractRepository { get; set; }

        //public Contractdoc Contract
        //{
        //    get
        //    {
        //        if (ContractRepository != null)
        //        {
        //            return ContractRepository.GetContractdoc(Id);
        //        }
        //        return null;
        //    }
        //}

        //private MoneyValue _moneyDisbursed;
        //private MoneyValue _moneyLeft;

        public MoneyValue FundsDisbursed
        {
            get
            {
                return new MoneyValue(DisbursedCache.GetValueOrDefault());
                //       (_moneyDisbursed = new MoneyValue(Contract.Stages.Where(x => x.GeneralStage == null).Sum(
                //           x => x.StageMoneyModel.Factor.National.PriceWithNdsValue)));
            }
        }

        public MoneyValue FundsLeft
        {
            get
            {
                return new MoneyValue(LeftCache.GetValueOrDefault());
                //return new MoneyValue(PriceMoneyModel.Factor.National.PriceWithNdsValue - FundsDisbursed.Price);
            }
        }

        partial void OnDisbursedCacheChanged()
        {
            SendPropertyChanged("FundsDisbursed");
            SendPropertyChanged("FundsLeft");
        }

        //public IEnumerable<Contractdoc> SubContracts
        //{
        //    get { return Contract.SubContracts; }
        //}

        public IEnumerable<Contractrepositoryview> Subcontractview
        {
            get { yield return this; }
        }

        public bool ValidPrice
        {
            get
            {
                return !Errors.Any(x => x.Code == NoPriceCode || x.Code == NoScheduleCode);
               
            }
        }

        public bool IsClosedOnCurrentDate
        {
            get
            {
                return ((Brokeat <= DateTime.Today) || (Reallyfinishedat <= DateTime.Today) || (Outofcontrolat <= DateTime.Today));
            }
        }

        public static bool NoAnyBaseDates(IContractStateData stateData)
        {
            System.Diagnostics.Contracts.Contract.Requires(stateData != null);
            return !stateData.Startat.HasValue && !stateData.Endsat.HasValue && !stateData.Appliedat.HasValue && !stateData.Approvedat.HasValue;
        }

        public static bool SeemsToBeOverdue(IContractStateData stateData, DateTime onDate)
        {
            System.Diagnostics.Contracts.Contract.Requires(stateData != null);


            if (stateData.Endsat.HasValue && ConditionResolver.GetContractCondition(stateData, onDate, onDate) == ContractCondition.Actual)
                return (stateData.Endsat.Value < onDate);
            return false;

        }

        /// <summary>
        /// Получает максимальную серьёзность ошибок в договоре среди всех. 
        /// </summary>
        public ErrorSeverity IntegralErrorsSeverity
        {
            get
            {
                if (!Errors.Any()) return ErrorSeverity.None;
                return Errors.Min(x => x.Severity);
            }
        }

        private const long SystemCodeBase = 1000;
        private const long NoDatesCode = SystemCodeBase + 1;
        private const long NoScheduleCode = SystemCodeBase + 2;
        private const long NoPriceCode = SystemCodeBase + 3;
        private const long OverdueCode = SystemCodeBase + 4;
        private const long NoNumCode = SystemCodeBase + 5;
        private const long SchedulePriceNotEqualsCode = SystemCodeBase + 6;
        private const long ActExceedsContractPriceCode = SystemCodeBase + 7;
        private const long CoWorkersFundsExceedsCode = SystemCodeBase + 8;
        private const long NoContractTypeCode = SystemCodeBase + 9;
        private const long NoContractorsCode = SystemCodeBase + 10;

        public IList<ErrorState> _errors = null;

        /// <summary>
        /// Получает коллекцию ошибок в договоре
        /// </summary>
        public IList<ErrorState> Errors
        {
            get
            {
                if (_errors != null) return _errors;
                _errors = new List<ErrorState>();
        
                /*Critical*/

                /*Warnings*/
                if (NoAnyBaseDates(this)) AddError(ErrorSeverity.Warning, "Не заданы основные даты", NoDatesCode);
                if (string.IsNullOrEmpty(Contracttypename)) AddError(ErrorSeverity.Warning, "Не указан тип договора", NoContractTypeCode);
                if (PriceIsNullOrUndefined(this)) AddError(ErrorSeverity.Warning, "Сумма не указана или равна нулю.", NoPriceCode);
                if (SchedulePriceNotEqualsContractPrice(this))
                    AddError(ErrorSeverity.Warning, string.Format("Сумма по договору {0} не соответствует сумме работ по КП {1}",
                                                PriceMoneyModel.Factor.National.PriceWithNds,
                                                new MoneyValue(StagesTotalPriceCache.GetValueOrDefault()).National), SchedulePriceNotEqualsCode);
                if (DisbursedIsGreaterThenContractPrice(this))
                    AddError(ErrorSeverity.Warning, string.Format("Сумма выполненных работ ({0}) превышает сумму договора ({1})",
                        new MoneyValue(DisbursedCache.GetValueOrDefault()), PriceMoneyModel.Factor.National.PriceWithNds), ActExceedsContractPriceCode);

                if (CoWorkersDisbursedIsGreaterThenContractPrice(this))
                    AddError(ErrorSeverity.Warning, string.Format("Сумма выполненных работ по договорам подрядам ({0}) превышает сумму договора ({1})",
                        new MoneyValue(DisbursedCache.GetValueOrDefault()), PriceMoneyModel.Factor.National.PriceWithNds), CoWorkersFundsExceedsCode);

                if (Orphandparentcount > 0)
                    AddError(ErrorSeverity.Warning, "Имеются незакрытые этапы при полностью закрытых подэтапах", -1);
             
                if (Closedparentcount > 0)
                    AddError(ErrorSeverity.Warning, "Имеются закрытые этапы при не полностью закрытых подэтапах", -1);
          
                /*Hints*/
                if (Schedulecount == 0) AddError(ErrorSeverity.Hint, "Добавьте календарные планы", NoScheduleCode);                     
                if (SeemsToBeOverdue(this, DateTime.Today)) 
                    AddError(ErrorSeverity.Hint, string.Format("Сдача работ по договору задержана на {0} (дней)",
                        (DateTime.Today - Endsat.GetValueOrDefault()).Days), OverdueCode);
               
                if (NoInternalNum(this)) AddError(ErrorSeverity.Hint, "Укажите номер договора", NoNumCode);
                if (Contractorscount == 0) AddError(ErrorSeverity.Hint, "Укажите контрагента", NoContractorsCode);
                
              
                return _errors;
            }
        }

        private static bool NoManager(Contractrepositoryview contractrepositoryview)
        {
            return string.IsNullOrEmpty(contractrepositoryview.ManagerProfile);
        }

        private static bool NoInternalNum(Contractrepositoryview contractrepositoryview)
        {
            return string.IsNullOrEmpty(contractrepositoryview.Internalnum);
        }

        private static bool PriceIsNullOrUndefined(Contractrepositoryview contractrepositoryview)
        {
            var price = contractrepositoryview.Price.GetValueOrDefault();
            return (price == 0.0M || price == default(decimal));
        }

        void AddError(ErrorSeverity severity, string message, long code)
        {
            _errors.Add(new ErrorState(severity, message, code));
        }

        public string _error = null;
        
        /// <summary>
        /// Получает сообщения об ошибках в договоре в виде одной строки. Ошибки разделяются \n 
        /// </summary>
        public string Error
        {
            get
            {
                if (_error != null) return _error;
                if (IntegralErrorsSeverity==ErrorSeverity.None) return string.Empty;
                return Errors.Select(x => x.Message).Aggregate((x, y) => x + "\n" + y);

            }
        }

        private bool CoWorkersDisbursedIsGreaterThenContractPrice(Contractrepositoryview contractrepositoryview)
        {
            return contractrepositoryview.DisbursedCoworkersCache >
              contractrepositoryview.PriceMoneyModel.Factor.National.PriceWithNdsValue;
        }

        private bool DisbursedIsGreaterThenContractPrice(Contractrepositoryview contractrepositoryview)
        {
            return contractrepositoryview.DisbursedCache >
                   contractrepositoryview.PriceMoneyModel.Factor.National.PriceWithNdsValue;
        }

        private static bool SchedulePriceNotEqualsContractPrice(Contractrepositoryview contractrepositoryview)
        {
            return contractrepositoryview.PriceMoneyModel.Factor.National.PriceWithNdsValue !=
                   contractrepositoryview.StagesTotalPriceCache.GetValueOrDefault();
        }

        private static IContractConditionResolver _resolver;
        public static IContractConditionResolver ConditionResolver
        {
            get { return _resolver ?? DefaultContractConditionReolver.Instance; }
            set { _resolver = value; }
        }

        public DateTime FilterStartDate { get; set; }

        public DateTime FilterEndDate { get; set; }

        public string this[string columnName]
        {
            get
            {
                //if (Status) return string.Empty;

                if (columnName == "Ndsalgorithmid") if (!Ndsalgorithmid.HasValue)
                        return Resource.Contractdoc_this_no_nds_algorithm_error;
                if (columnName == "Ndsid") if (!Ndsid.HasValue)
                        return Resource.Contractdoc_this_no_nds_error;
                if (columnName == "Currencymeasureid") if (!Currencymeasureid.HasValue)
                        return Resource.Contractdoc_this_no_measure_error;
                if (columnName == "Currencyid") if (!Currencyid.HasValue)
                        return Resource.Contractdoc_this_no_currency_error;
                if (columnName == "Price") if (!Price.HasValue)
                        return Resource.Contractdoc_this_no_price_error;
                if (columnName == "Currencyrate") if (!Currencyrate.HasValue)
                        if (Currency.FromCulture(Culture).IsForeign)
                            return Resource.Contractdoc_this_no_rate_and_ratedate_error;
                if (columnName == "Schedulecount")
                    if (Schedulecount == 0)
                        return "Договор не имеет календарных планов";



                return string.Empty;
            }

        }
        /// <summary>
        /// Получает состояние договора на контекстный диапазон дат, заданный FilterStartDate и FilterEndDate
        /// </summary>
        public ContractCondition Condition
        {
            get { return ConditionResolver.GetContractCondition(this, FilterStartDate, FilterEndDate); }
        }

        public void Invalidate()
        {

            _errors = null;
            _error = null;

            SendPropertyChanged("Startat");
            SendPropertyChanged("Endsat");
            SendPropertyChanged("Appliedat");
            SendPropertyChanged("Approvedat");
            SendPropertyChanged("Outofcontrolat");
            SendPropertyChanged("Brokeat");
            SendPropertyChanged("Reallyfinishedat");
            SendPropertyChanged("IsGeneral");
            SendPropertyChanged("IsAgreement");

            SendPropertyChanged("FundsDisbursed");
            SendPropertyChanged("FundsLeft");
            SendPropertyChanged("PriceMoneyModel");

            SendPropertyChanged("Condition");
            SendPropertyChanged("Status");
            SendPropertyChanged("Error");
            SendPropertyChanged("Errors");
            SendPropertyChanged("IsActual");


        }

        /// <summary>
        /// Проверяет актуальность договора на контекстные даты
        /// </summary>
        public bool IsActual
        {
            get { return (Condition & ContractCondition.Actual) == ContractCondition.Actual; }
        }
    }
}
