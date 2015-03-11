using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using CommonBase;
using UOW;

namespace MCDomain.Model
{
    public enum PrepaymentCalcType
    {
        /// <summary>
        /// Не предусмотрен
        /// </summary>
        [Description("не предусмотрен")]
        NotProvided,

        /// <summary>
        /// Предусмотрен от % от цены без НДС
        /// </summary>
        [Description("% от суммы договора без НДС")]
        ProvidiedWithFromPure,

        /// <summary>
        /// Предусмотрен % от цены с НДС
        /// </summary>
        [Description("% от суммы договора с НДС")]
        ProvidedWithFromNds,

        /// <summary>
        /// Предусмотрена сумма
        /// </summary>
        [Description("сумма")]
        ProvidedWithSum
    }

    partial class Contractdoc : ICloneable, IDataErrorInfo, IDateRange, ISupportMoneyModel, INull,
                                IContractStateData, IDocumentImageContainer, IUnderResponsibility, ISupportDeltaEndDate, IClonableRecursive, IMeasureSupport, IRepositoryEntity<long>
    {
        public const string ContractMoneyModelSubj = "ContractMoneyModel";
        public const string PrepaymentMoneyModelSubj = "PrepaymentMoneyModel";

        private readonly DataErrorHandlers<Contractdoc> _errorHandlers = new DataErrorHandlers<Contractdoc>
                                                                             {
                                                                                 new StartDateErrorHandler(),
                                                                                 new ContractstateErrorHandler(),
                                                                                 new CurrencyDataErrorHandler(),
                                                                                 new NdsDataErrorHandler(),
                                                                                 new PrepaymentDataErrorHandler(),
                                                                                 new DataErrorHandlerForNonNullable
                                                                                     <Contractdoc>()
                                                                                     {
                                                                                         ErrorMessage =
                                                                                             Resource.
                                                                                             Contractdoc_this_not_nullable_field_error
                                                                                     }
                                                                             };

        private IContractConditionResolver _contractConditionResolver;
        private IFundsResolver _fundsResolver;


        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает или устанавливает объект для вычисления состояния договора
        /// </summary>
        public IContractConditionResolver ConditionResolver
        {
            get { return _contractConditionResolver ?? DefaultContractConditionReolver.Instance; }
            set { _contractConditionResolver = value; }
        }

        /// <summary>
        /// Получает или устанавливает объект для вычисления сумм по договору
        /// </summary>
        public IFundsResolver FundsResolver
        {
            get { return _fundsResolver ?? DefaultFundsResolver.Instance; }
            set { _fundsResolver = value; }
        }

        /// <summary>
        /// Получает форматированную сумму договора в соответствие с указанной валютой договора.
        /// </summary>
        public string FormatedPrice
        {
            get
            {
                if (!Price.HasValue) return (0.0F).ToString("N2");

                if (Currency == null) return Price.Value.ToString("N2");
                return Currency.FormatMoney(Price.Value);
            }
        }


        /// <summary>
        /// Получает или устанавливает основной договор для соглашения
        /// </summary>
        public Contractdoc OriginalContract
        {
            get { return Contractdoc_Origincontractid; }
            set
            {
                if (value == Contractdoc_Origincontractid) return;
                Contractdoc_Origincontractid = value;
                SendPropertyChanged("OriginalContract");
            }
        }

        /// <summary>
        /// Получает признак того, что данный договор является дополнительным соглашением к существующему договору
        /// </summary>
        public bool IsAgreement
        {
            get { return OriginalContract != null; }
        }

        public bool IsAgreementOrSubContract
        {
            get { return IsAgreement || IsSubContract; }
        }

        /// <summary>
        /// возвращает признак является ли КП тем же самым что и для основного договора или открпленным
        /// </summary>
        public bool HasTheSameSchedule(Schedule schedule)
        {
            return (OriginalContract != null) && (OriginalContract.Schedulecontracts.Any(o => o.Schedule == schedule));
        }

        public bool HasTheSameSchedules()
        {
            return Schedulecontracts.Any() && Schedulecontracts.All(sc => HasTheSameSchedule(sc.Schedule));
        }

        /// <summary>
        /// Получает коллекцию генеральных договоров для данного договора
        /// </summary>
        public IEnumerable<Contractdoc> Generals
        {
            get { return Generalcontracthierarchies.Select(x => x.GeneralContractdoc); }
        }

        /// <summary>
        /// генеральный договора
        /// вообще-то для каждого субподрядного - один генеральный
        /// </summary>
        public Contractdoc General
        {
            get { return Generals.FirstOrDefault(); }
        }

        /// <summary>
        /// Получает коллекцию субподрядных договоров для данного договора
        /// </summary>
        public IList<Contractdoc> SubContracts
        {
            get { return Contracthierarchies.Select(x => x.SubContractdoc).ToList(); }
        }

        /// <summary>
        /// Получает признак того, что договор имеет субподрядные договора
        /// </summary>
        public bool HasSubcontracts
        {
            get
            {
                Contract.Requires(SubContracts != null);
                return SubContracts.Any();
            }
        }

        /// <summary>
        /// Получает признак того, что договор имеет допсоглашения
        /// </summary>
        public bool HasAgreements
        {
            get
            {
                Contract.Requires(Agreements != null);
                return Agreements.Any();
            }
        }

        /// <summary>
        /// Получает признак, что данный договор является генеральным
        /// </summary>
        public bool IsGeneral
        {
            get
            {
                Contract.Requires(Generals != null);
                return !Generals.Any();
            }
        }

        /// <summary>
        /// проверяем закрыты ли все этапы
        /// </summary>
        public bool StagesClosed
        {
            get
            {
                return Stages.Any() && Stages.Select(s => s.Stagecondition == StageCondition.Closed).Aggregate(true, (p, next) => p && next);
            }
        }

        /// <summary>
        /// Получает признак, что данный договор является субподрядным
        /// </summary>
        public bool IsSubContract
        {
            get { return !IsGeneral; }
        }


        /// <summary>
        /// Получает сумму к перечислению по платёжным документам договора
        /// </summary>
        public decimal TransferedFunds
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.TransferFounds");

                var path = this.GetBackwardPath(x => x.OriginalContract, x => true);
                return path.SelectMany(x => x.Contractpayments).Sum(x => x.Paymentdocument.Paymentsum);


            }
        }


        /// <summary>
        /// Получает перечисленные средства по актам
        /// </summary>
        public decimal ActTransferedFunds
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0,
                                "Contractdoc.ActTransferFounds");
                return Acts.Sum(x => x.Sumfortransfer.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Получает остатки средств по авансам
        /// </summary>
        public decimal PrepaymentRests
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.PrepaymentRests");
                return TransferedFunds - Prepaymented;
            }
        }

        /// <summary>
        /// Получает сумму использованных средств аванса
        /// </summary>
        public decimal Prepaymented
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.Prepaymented");
                return Acts.Sum(x => x.Credited);                
            }
        }

        /// <summary>
        /// Получает строковое представление цены договора в стандартном виде "целая часть прописью"."разменная монета - числом"
        /// </summary>
        public string PriceAsString
        {
            get
            {
                Contract.Requires(Currency != null, "Свойтво Currency должно быть задано для объекта типа Contract");
                if (!Price.HasValue) return String.Empty;
                return Currency.MoneyInWords(Price.Value, false, true, true);
            }
        }


        /// <summary>
        /// Возвращает цену договора с учетом доп. соглашений к нему
        /// </summary>
        public decimal PriceWithAgreements
        {
            get
            {
                if (IsAgreement)
                {
                    return 0;
                }

                if (!HasAgreements)
                {
                    return ContractMoney.Factor.National.WithNdsValue;
                }

                return AllAgreements.Last().ContractMoney.Factor.National.WithNdsValue;
            }
        }




        /// <summary>
        /// Возвращает сумму освоенных по договору средств (на весь период договора)
        /// </summary>
        public decimal FundsDisbursed
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.FoundDisbursed");
                if (Startat.HasValue && Endsat.HasValue)
                    return FundsResolver.GetFundsDisbursed(this, Startat.Value, Endsat.Value);
                return 0.0M;
            }
        }

        /// <summary>
        /// Возвращает сумму остаточных по договру средств в национальной валюте (на весь период договора)
        /// </summary>
        public decimal FundsLeft
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.FoundLeft");
                if (Startat.HasValue && Endsat.HasValue)
                    return FundsResolver.GetFundsLeft(this, Startat.Value, Endsat.Value);
                return 0.0M;
            }
        }



        /// <summary>
        /// Получает или устанавливает признак охраноспособности объекта
        /// </summary>
        public bool Protectability
        {
            get { return Isprotectability.GetValueOrDefault(); }
            set
            {
                if (value == Isprotectability) return;
                Isprotectability = value;
                SendPropertyChanged("Protecability");
            }
        }

        /// <summary>
        /// Получает все этапы календарного плана по договору
        /// </summary>
        public IEnumerable<Stage> Stages
        {
            get { return Schedulecontracts.OrderBy(s => s.Appnum).Select(x => x.Schedule).SelectMany(x => x.Stages); }
        }

        /// <summary>
        /// Получает все акты по договору
        /// </summary>
        public IEnumerable<Act> Acts
        {
            get { return Stages.Where(x => x.Act != null).Select(y => y.Act).Distinct(); }
        }



        /// <summary>
        /// Получает доп. соглашения всех уровней по договору
        /// </summary>
        public IEnumerable<Contractdoc> AllAgreements
        {
            get
            {
                var result = new List<Contractdoc>();

                FindAgreements(this, result);

                return result.AsReadOnly();
            }
        }

        /// <summary>
        /// Получает доп. соглашения к данному договору
        /// </summary>
        public EntitySet<Contractdoc> Agreements
        {
            get { return Contractdocs; }
        }

        /// <summary>
        /// Получает генеральный договор для соглашения или субподрядного договора.
        /// </summary>
        public IEnumerable<Contractdoc> OriginalGenerals
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Contractdoc>>() != null);


                Contractdoc contractdoc = this;
                // Сначала производится переход на оригинальный договор по иерархии доп. соглашений
                if (IsAgreement)
                {
                    while (contractdoc.OriginalContract != null)
                    {
                        contractdoc = contractdoc.OriginalContract;
                    }
                }

                var result = new List<Contractdoc>();

                // Если оригинальный договор не является генеральным, то получить его генеральные договора);
                if (contractdoc.IsSubContract)
                {
                    result.AddRange(contractdoc.Generals);
                }
                else
                {
                    if (contractdoc != this)
                        result.Add(contractdoc);
                }
                return result.AsReadOnly();
            }
        }

        /// <summary>
        /// Получает сумму авансов, распередлённую по годам
        /// </summary>
        public decimal TotalPrepayments
        {
            get
            {
                Contract.Requires(Prepayments != null);
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Contractdoc.TotalPrepayments");
                return Prepayments.Sum(x => x.Sum);
            }
        }

        public PrepaymentCalcType PrepaymentPrecentCalcType
        {
            get
            {
                if (!Prepaymentprecenttype.HasValue) return PrepaymentCalcType.NotProvided;
                if (Prepaymentprecenttype.Value == 0) return PrepaymentCalcType.ProvidiedWithFromPure;
                if (Prepaymentprecenttype.Value == 1) return PrepaymentCalcType.ProvidedWithFromNds;
                if (Prepaymentprecenttype.Value == 2) return PrepaymentCalcType.ProvidedWithSum;
                return PrepaymentCalcType.NotProvided;
            }
            set
            {
                switch (value)
                {
                    case PrepaymentCalcType.NotProvided:
                        Prepaymentprecenttype = null;
                        break;
                    case PrepaymentCalcType.ProvidiedWithFromPure:
                        Prepaymentprecenttype = 0;
                        break;
                    case PrepaymentCalcType.ProvidedWithFromNds:
                        Prepaymentprecenttype = 1;
                        break;
                    case PrepaymentCalcType.ProvidedWithSum:
                        Prepaymentprecenttype = 2;
                        break;
                }
                SendPropertyChanged("PrepaymentPrecentCalcType");
            }


        }

        /// <summary>
        /// Получает объект с инофрмацией о деньгах договора
        /// </summary>
        public MoneyModel ContractMoney
        {
            get { return new ContractMoneyModel(this); }
        }

        public PrepaymentMoneyModel PrepaymentMoney
        {
            get { return new PrepaymentMoneyModel(this); }
        }

        /// <summary>
        /// Получает актуальную версию договора
        /// </summary>
        public Contractdoc Actual
        {
            get
            {
                Contract.Ensures(Contract.Result<Contractdoc>() != null);
                return AllAgreements.LastOrDefault() ?? this;
            }
        }

        /// <summary>
        /// Получает календарный план договора
        /// </summary>
        public Schedule DefaultSchedule
        {
            get
            {
                if (Schedulecontracts.Count > 0)
                    return Schedulecontracts[0].Schedule;
                return null;
            }
        }

        public bool IsDeleted
        {
            get { return Deleted.GetValueOrDefault(); }
            set { Deleted = value; }
        }

        /// <summary>
        /// Получает форматированный номер договора:
        /// Полный вариант: Внутренний номер / номер, присвоенный заказчиком
        /// Либо один из них (который присутствует)
        /// </summary>
        public string Num
        {
            get
            {
                if (!IsAgreement)
                    return Internalnum;
                return string.Format("{0} (ДС № {1})", Internalnum, Agreementnum);
            }
        }


        public bool HasSchedule
        {
            get
            {
                return Schedulecontracts != null && Schedulecontracts.Any() &&
                       Schedulecontracts.FirstOrDefault().Schedule != null;
            }
        }

        /// <summary>
        /// Возвращает Полное название договора в виде:
        /// Договор № /Внутренний номер/ "/Тема/",
        /// Утвержден /Дата утверждения/,
        /// Заказчик - /Заказчик/,
        /// Функциональный заказчик - /Функциональный заказчик/
        /// </summary>
        public string FullName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(FullSortName);

                if (Contractor != null)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("Заказчик - ");
                    sb.Append(Contractor.Name);
                }

                if (Functionalcustomercontracts != null && Functionalcustomercontracts.Count > 0)
                {
                    sb.AppendLine();
                    sb.Append("Функциональный заказчик - ");
                    sb.Append(Functionalcustomercontracts.Aggregate(string.Empty, (fc, next) => fc +
                                                                                                (next.Functionalcustomer.Name == null
                                                                                                     ? string.
                                                                                                           Empty
                                                                                                     : next.Functionalcustomer.Name +
                                                                                                       "\n")));
                }

                return sb.ToString();
            }
        }

        public IContractor Contractor { get; protected set; }

        public string FullInternalnum
        {
            get
            {
                if (!IsAgreement)
                    return Internalnum;
                else
                {
                    if (Agreementnum.HasValue)
                        return Internalnum + "(" + Agreementnum + ")";
                    else
                        return Internalnum;
                }
            }
        }
        public string FullContractorName
        {
            get
            {
                var sb = new StringBuilder();

                if (Contractor != null)
                {
                    sb.Append("Заказчик - ");
                    sb.Append(Contractor.Name);
                }

                if (Functionalcustomercontracts != null && Functionalcustomercontracts.Count > 0)
                {
                    sb.Append(",");
                    sb.AppendLine();
                    sb.Append("Функциональный заказчик - ");
                    sb.Append(Functionalcustomercontracts.Aggregate(string.Empty, (fc, next) => fc +
                                                                                                (next.Functionalcustomer.Name ?? string.
                                                                                                                                     Empty)));
                }

                return sb.ToString();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public string Customer
        {
            get
            {
                if (Contractor == null) return string.Empty;
                if (Contractor.Contractortype == null) return string.Empty;

                //Если генеральный договор с Газпромом - список функциональных заказчиков
                if (IsGeneral && Contractor.Contractortype.WellKnownType == WellKnownContractorTypes.Gazprom)
                    return (Functionalcustomercontracts == null
                                ? string.Empty
                                : Functionalcustomercontracts.Count <= 0
                                      ? string.Empty
                                      : Functionalcustomercontracts.Aggregate(string.Empty, (fc, next) => fc +
                                                                                                          (next.
                                                                                                               Functionalcustomer
                                                                                                               .Name ==
                                                                                                           null
                                                                                                               ? string
                                                                                                                     .
                                                                                                                     Empty
                                                                                                               : next
                                                                                                                     .
                                                                                                                     Functionalcustomer
                                                                                                                     .
                                                                                                                     Name +
                                                                                                                 "\n")));

                //иначе - заказчик - это контрагент
                return ContractorName;
            }
        }

        public string FullSortNumber
        {
            get
            {
                String s;
                if (IsAgreement)
                {
                    s = "Д.C. " + (Agreementnum == null ? string.Empty : "№ " + Agreementnum) +
                           " от " + (!Approvedat.HasValue ? "        " : Approvedat.Value.ToString("dd.MM.yyyy")) + " " +
                           "к договору " +
                           (OriginalContract.Internalnum == null ? string.Empty : "№ " + OriginalContract.Internalnum) + " от " +
                           (!OriginalContract.Approvedat.HasValue ? "        " : OriginalContract.Approvedat.Value.ToString("dd.MM.yyyy") + " ");
                }
                else
                {
                    s = "Договор " + (Internalnum == null ? string.Empty : "№ " + Internalnum) + " от " +
                           (!Approvedat.HasValue ? "        " : Approvedat.Value.ToString("dd.MM.yyyy") + " ");
                }

                return s.Replace("\n", " ");
            }
        }

        /// <summary>
        /// Возвращает Полное название договора в виде:
        /// для ген договоров:   Договор № /Внутренний номер/  от /Дата утверждения/  "/Тема/" 
        /// для доп соглашений:  Доп. Соглашение № /Номер/ от /Дата утверждения/ к договору № /№ ген договора/  «/Тема ген. договора/»   
        /// </summary>
        public string FullSortName
        {
            get
            {
                String s;
                if (IsAgreement)
                {
                    s = "Д.C. " + (Agreementnum == null ? string.Empty : "№ " + Agreementnum) +
                           " от " + (!Approvedat.HasValue ? "        " : Approvedat.Value.ToString("dd.MM.yyyy")) + " " +
                           "к договору " +
                           (OriginalContract.Internalnum == null ? string.Empty : "№ " + OriginalContract.Internalnum) + " от " +
                           (!OriginalContract.Approvedat.HasValue ? "        " : OriginalContract.Approvedat.Value.ToString("dd.MM.yyyy") + " ") +
                           (OriginalContract.Subject == null ? string.Empty : "\"" + OriginalContract.Subject + "\"");
                }
                else
                {
                    s = "Договор " + (Internalnum == null ? string.Empty : "№ " + Internalnum) + " от " +
                           (!Approvedat.HasValue ? "        " : Approvedat.Value.ToString("dd.MM.yyyy") + " ") +
                           (Subject == null ? string.Empty : "\"" + Subject + "\"");
                }

                return s.Replace("\n", " ");
            }
        }

        /// <summary>
        /// Возвращает название договора в виде: № /Номер/ от /Дата утверждения/
        /// </summary>
        public string ShortName
        {
            get
            {
                return
                    (Internalnum == null ? string.Empty : "№ " + Internalnum) +
                    (!Approvedat.HasValue ? string.Empty : " от " + Approvedat.Value.ToString("dd.MM.yyyy"));
            }
        }

        #region IUnderResponsibleMembers

        public void SendResponsiblesBindingListChanged()
        {
            SendPropertyChanged("ResponsiblesBindingList");
            SendPropertyChanged("DisposalPersons");
        }

        public void RemoveResponsiblesForDisposal(Disposal disposal)
        {
            for (int i = Responsibles.Count - 1; i >= 0; i--)
            {
                if (Responsibles[i].Disposal != null && Responsibles[i].Disposal.Id == disposal.Id)
                    Responsibles.RemoveAt(i);
            }
        }
        /// <summary>
        /// Возвращает имена зам. директора и руководителя напрвления
        /// </summary>
        public string DirectorsAndChiefs(bool includeordersuperviser)
        {

            IList<Employee> all = new List<Employee>();
            if (DirectorEmployee != null) all.Add(DirectorEmployee);
            if (ManagerEmployee != null) all.Add(ManagerEmployee);

            if (Chiefs != null)
                foreach (Responsible e in Chiefs)
                {
                    if (e.Employee != null)
                        all.Add(e.Employee);
                }

            if (includeordersuperviser && OrderSuperviserEmployee != null) all.Add(OrderSuperviserEmployee);

            var dist = all.Where(a => a.Id != EntityBase.ReservedUndifinedOid).Distinct();
            if (dist.Count() > 1)
                return dist.Aggregate("\n", (e, next) => e + next.ToString() + "\n");
            else if (dist.Count() == 1)
                return dist.ElementAt(0).ToString();
            else return string.Empty;


        }

        public string Directors
        {
            get
            {
                IList<Employee> all = new List<Employee>();
                if (DirectorEmployee != null) all.Add(DirectorEmployee);
                if (ManagerEmployee != null) all.Add(ManagerEmployee);


                var dist = all.Where(a => a.Id != EntityBase.ReservedUndifinedOid).Distinct();
                if (dist.Count() > 1)
                    return dist.Aggregate("\n", (e, next) => e + next.ToString() + "\n");
                else if (dist.Count() == 1)
                    return dist.ElementAt(0).ToString();
                else return string.Empty;
            }
        }

        private BindingList<Responsible> _responsiblesbindinglist;
        public IBindingList ResponsiblesBindingList
        {
            get
            {
                if (_responsiblesbindinglist == null)
                    _responsiblesbindinglist = new BindingList<Responsible>(Responsibles.Where(x => x.Stage == null&&x.Disposal != null&&Disposal != null &&x.Disposal.Id == Disposal.Id).ToList());

                return _responsiblesbindinglist;
            }
        }

        public void RefreshRespBindingList()
        {
            _responsiblesbindinglist = null;
        }

        public string GetResponsibleNameForReports()
        {
            return (Disposal != null) ?
                Disposal.GetResponsibleNamesForReports(this) : String.Empty;
        }



        /// <summary>
        /// руководитель договора (промгаз)
        /// </summary>
        public Responsible Chief
        {
            get
            {
                if (Disposal != null)
                    return Disposal.GetChief(this)??(OriginalContract!=null?OriginalContract.Chief:null);
                else
                    return null;
            }
        }

        public Employee ChiefEmployee
        {
            get { return (Chief != null) ? Chief.Employee : null; }
        }

        public IEnumerable<Responsible> Chiefs
        {
            get
            {
                if (Disposal != null)
                    return Disposal.GetChiefs(this)??(OriginalContract!=null?OriginalContract.Chiefs:null);
                else
                    return null;
            }
        }

        /// <summary>
        /// руководитель направления - ответственный
        /// </summary>
        public Responsible Manager
        {
            get
            {
                if (Disposal != null)
                    return Disposal.GetManager(this)??(OriginalContract!=null?OriginalContract.Manager:null);
                else
                    return null;

            }
        }
        /// <summary>
        /// руководитель направления - служащий
        /// </summary>
        public Employee ManagerEmployee
        {
            get { return (Manager != null) ? Manager.Employee : null; }
        }
        /// <summary>
        /// замдир - ответственный
        /// </summary>
        public Responsible Director
        {
            get
            {
                if (Disposal != null)
                    return
                        Disposal.GetDirector(this)??(OriginalContract!=null?OriginalContract.Director:null);
                else
                    return null;
            }
        }

        /// <summary>
        /// замдир - служащий
        /// </summary>
        public Employee DirectorEmployee
        {
            get { return (Director != null) ? Director.Employee : null; }
        }

        /// <summary>
        /// ответственный по договорам
        /// </summary>
        public Responsible Curator
        {
            get
            {
                if (Disposal != null)
                    return Disposal.GetCurator(this)??(OriginalContract!=null?OriginalContract.Curator:null);
                else
                    return null;
            }
        }

        public Employee CuratorEmployee
        {
            get { return (Curator != null) ? Curator.Employee : null; }
        }


        /// <summary>
        /// ответственный по договорам
        /// </summary>
        public Responsible OrderSuperviser
        {
            get
            {
                if (Disposal != null)
                    return
                        Disposal.GetOrderSuperviser(this)??(OriginalContract!=null?OriginalContract.OrderSuperviser:null);
                else
                    return null;
            }
        }


        public Employee OrderSuperviserEmployee
        {
            get
            {
                return (OrderSuperviser != null) ? OrderSuperviser.Employee : null;
            }
        }

        public bool IsManagerVisible
        {
            get { return !(ManagerEmployee != null && ChiefEmployee != null) || ManagerEmployee.Id != ChiefEmployee.Id; }
        }

        /// <summary>
        /// распоряжение по данному договору
        /// если у ДС нет своего собственного распоряжения - берется у старшего договора
        /// </summary>

        
        public Disposal Disposal
        {
            get
            {
                var r = Responsibles.FirstOrDefault(p => (p.Contractdoc.Id == this.Id && p.Stage == null));
                if (r != null && r.Disposal != null)
                {
                    return r.Disposal;
                }

                r = Responsibles.FirstOrDefault(p => (p.Contractdoc.Id == this.Id));
                if (r != null && r.Disposal != null)
                {
                    return r.Disposal;
                }


                if (OriginalContract != null) return OriginalContract.Disposal;
                return null;
            }
        }

        public bool HasOwnDisposal
        {
            get { return (Responsibles.Any(p => (p.Contractdoc.Id == this.Id && p.Stage == null)) || Responsibles.Any(p => (p.Contractdoc.Id == this.Id))); }
        }

        public string DisposalPersons
        {
            get
            {
                var sb = new StringBuilder();
                
                if (ChiefEmployee != null)
                    sb.Append(string.Format("{0}: {1}", "Руководитель", ChiefEmployee));

                if (IsManagerVisible && ManagerEmployee != null)
                {
                   sb.Append("; ");
                   sb.Append(string.Format("{0}: {1}", "рук. напр.", ManagerEmployee));
                }

                if (DirectorEmployee != null)
                {
                  sb.Append("; ");
                  sb.Append(string.Format("{0}: {1}", "зам. дир.", DirectorEmployee));
                }

                if (OrderSuperviserEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "отв. по договорам", OrderSuperviserEmployee));
                }
                
                if (CuratorEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "куратор", CuratorEmployee));
                }
                return sb.ToString();

            }
        }

        /// <summary>
        /// отдел, за которым договор закреплен по распоряжению
        /// </summary>
        public Department DisposalDepartment
        {
            get { return (Chief != null) ? Chief.Employee.Department : null; }
        }

        #endregion
        /// <summary>
        /// Возвращает название контрагента (исполнителя) или пустую строку, если тот отсутствует
        /// </summary>
        public string ContractorName
        {
            get { return Contractor == null ? string.Empty : Contractor.Name ?? string.Empty; }
        }

        /// <summary>
        /// Возвращает тип контрагента (исполнителя) (Газпром, Дочерние организации, ...)
        /// </summary>
        public string ContractorTypeName
        {
            get
            {
                return Contractor == null
                           ? string.Empty
                           : Contractor.Contractortype == null
                                 ? string.Empty
                                 : Contractor.Contractortype.Name ?? string.Empty;
            }
        }

        /// <summary>
        /// Возвращает список функциональных заказчиков с строковом виде
        /// </summary>
        public string FunctionalCustomers
        {
            get
            {
                return
                    Functionalcustomercontracts == null
                        ? string.Empty
                        : Functionalcustomercontracts.Count <= 0
                              ? string.Empty
                              : Functionalcustomercontracts.Aggregate(String.Empty,
                                                                      (fcc, next) =>
                                                                      fcc +
                                                                      (next.Functionalcustomer.Name ?? string.Empty) + "\n");
            }
        }

        /// <summary>
        /// Возвращает тип договора (НИОКР, Газификация, ...)
        /// </summary>
        public string ContractTypeName
        {
            get { return Contracttype == null ? string.Empty : Contracttype.Name ?? string.Empty; }
        }

        /// <summary>
        /// Получает строку с состояние договора (подписан/ не подписан)
        /// </summary>
        public string ContractStateName
        {
            get
            {
                return
                    Contractstate == null ? string.Empty : Contractstate.Name ?? "";
            }
        }

        public string ApprovalContractConditionComment
        {
            get
            {
                if (Contractstate.State != WellKnownContractStates.Signed || (Approvedat.HasValue && Approvalprocesses.Count(p => p.Enteringdate > Approvedat) > 0))
                {

                    Approvalprocess a = Approvalprocesses.Where(p => p.ToLocation != null).OrderBy(p => p.Enterstateat).FirstOrDefault();

                    if (a != null)
                    {
                        string res = string.Empty;
                        
                        res = Approvalprocesses.Aggregate(res,
                                                          (p, next) => p + next.Enterstateat.ToShortDateString() + " " +
                                                                       (!next.ToLocation.ReservedAsUndefined ? next.ToLocation.ToString() + ":" : String.Empty) +
                                                                       ((next.Description != null && next.Description.Trim() != "") ? next.Description : (next.Approvalstate != null ? next.Approvalstate.ToString() : "")) +
                                                                       (next.Missivedate.HasValue ? " (" +
                                                                                    (next.Missivetype.Return(x => !x.ReservedAsUndefined ? x.ToString().ToLower() + " " : String.Empty, String.Empty) +
                                                                                      next.Missiveid.Return(x => next.Missiveid.Trim(), "письмо") +
                                                                                      (next.Missivedate.HasValue ? " от " + next.Missivedate.Value.ToShortDateString() : String.Empty) + ")") : String.Empty) +
                                                                       "\n");

                        return res;
                    }

                }
                return string.Empty;
            }
        }

        public string ContractConditionComment
        {
            get
            {
                string res;
                if (Contractstate.State == WellKnownContractStates.Signed)
                {
                    res = ContractStateName;
                    if (Approvedat.HasValue)
                        res = res + " " + Approvedat.Value.ToShortDateString();
                    res = res + "\n";

                }
                else res = string.Empty;

                res = res + ApprovalContractConditionComment;
                
                return res;

            }


        }

        #region IClonablePersistent Members

        public object CloneRecursively(object owner, object source)
        {
            var newContract = new Contractdoc
                                  {
                                      Appliedat = Appliedat,
                                      Contracttype = Contracttype,
                                      Startat = Startat,
                                      Endsat = Endsat,
                                      Delta = Delta,
                                      Deltacomment = Deltacomment,
                                      Price = Price,
                                      //Approvedat = Appliedat,
                                      Contractornum = Contractornum,
                                      Internalnum = Internalnum,
                                      Subject = Subject,
                                      Currency = Currency,
                                      Currencymeasure = Currencymeasure,
                                      Nds = Nds,
                                      Ndsalgorithm = Ndsalgorithm,
                                      Prepaymentndsalgorithm = Prepaymentndsalgorithm,
                                      Person = Person,
                                      Prepaymentpercent = Prepaymentpercent,
                                      Prepaymentsum = Prepaymentsum,
                                      Protectability = Protectability,
                                      FundsResolver = FundsResolver,
                                      ConditionResolver = ConditionResolver,
                                      Currencyrate = Currencyrate,
                                      Ratedate = Ratedate,
                                      Description = Description,
                                      Acttype =  Acttype
                                  };


            // копируем проблемы и функциональных заказчиков
            foreach (Contracttrouble problem in Contracttroubles)
            {
                newContract.Contracttroubles.Add(new Contracttrouble { Contractdocid = Id, Trouble = problem.Trouble });
            }

            foreach (Functionalcustomercontract fc in Functionalcustomercontracts)
            {
                newContract.Functionalcustomercontracts.Add(new Functionalcustomercontract
                                                                {
                                                                    Contractdocid = Id,
                                                                    Functionalcustomerid = fc.Functionalcustomerid
                                                                });
            }

            foreach (var contractorcontractdoc in Contractorcontractdocs)
            {
                newContract.Contractorcontractdocs.Add(new Contractorcontractdoc() { Contractor = contractorcontractdoc.Contractor});
            }


            foreach (Schedulecontract schedulecontract in Schedulecontracts)
            {
                newContract.Schedulecontracts.Add(
                    (Schedulecontract)
                    schedulecontract.CloneRecursively(newContract, schedulecontract.Schedule));
            }


            return newContract;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            var newContract = new Contractdoc
                                  {
                                      Appliedat = Appliedat,
                                      Contracttype = Contracttype,
                                      Startat = Startat,
                                      Endsat = Endsat,
                                      Delta = Delta,
                                      Deltacomment = Deltacomment,
                                      Price = Price,
                                      //Approvedat = Appliedat,
                                      Contractornum = Contractornum,
                                      Internalnum = Internalnum,
                                      Contractor = Contractor,
                                      Subject = Subject,
                                      Currency = Currency,
                                      Currencymeasure = Currencymeasure,
                                      Nds = Nds,
                                      Ndsalgorithm = Ndsalgorithm,
                                      Prepaymentndsalgorithm = Prepaymentndsalgorithm,
                                      Person = Person,
                                      Prepaymentpercent = Prepaymentpercent,
                                      Prepaymentsum = Prepaymentsum,
                                      Protectability = Protectability,
                                      FundsResolver = FundsResolver,
                                      ConditionResolver = ConditionResolver,
                                      Currencyrate = Currencyrate,
                                      Ratedate = Ratedate,
                                      Description = Description
                                  };

            foreach (Schedulecontract schedulecontract in Schedulecontracts)
            {
                newContract.Schedulecontracts.Add((Schedulecontract)schedulecontract.Clone());
            }
            return newContract;
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get
            {
                return String.Empty;
                //return this.Validate();
            }
        }

        /// <summary>
        /// Получает описание текущей ошибки заданного свойства
        /// </summary>
        /// <param name="columnName">Имя свойства</param>
        /// <returns>Описание ошибки или null в случае корректности свойства</returns>
        public string this[string columnName]
        {
            get
            {
                //return string.Empty;
                return _errorHandlers.HandleError(this, columnName);
            }
        }

        #endregion

        #region IDateRange Members

        /// <summary>
        /// Получает и устанавливает даты начала работ и окончания работ по договору в виде объекта DateRange
        /// </summary>
        public DateRange Range
        {
            get
            {
                Contract.Assert(Startat.HasValue);
                Contract.Assert(Endsat.HasValue);
                return new DateRange { Start = Startat.Value, End = Endsat.Value };
            }
            set
            {
                Startat = value.Start;
                Endsat = value.End;
            }
        }

        #endregion

        #region INull Members

        bool INull.IsNull
        {
            get { return false; }
        }

        #endregion

        #region IPrice Members

        /// <summary>
        /// Получает или устанавливает цену договора
        /// </summary>
        public decimal PriceValue
        {
            get { return Price.HasValue ? Price.Value : 0; }
            set { Price = value; }
        }

        #endregion

        #region ISupportMoneyModel Members

        MoneyModel ISupportMoneyModel.this[string moneySubject]
        {
            get
            {
                if (ContractMoneyModelSubj == moneySubject)
                    return ContractMoney;
                return PrepaymentMoneyModelSubj == moneySubject ? PrepaymentMoney : null;
            }
        }

        #endregion

        /// <summary>
        /// Возвращает цену договора на заданный период времени
        /// ПОКА ЭТО СУММА ЦЕН ЭТАПОВ, КОТОРЫЕ ЛИБО ПОЛНОСТЬЮ ВКЛЮЧЕНЫ В ПЕРИОД, ЛИБО ЗАКАНЧИВАЮТСЯ В ЗАДАННОМ ПЕРИОДЕ
        /// </summary>
        /// <param name="start">Дата начала периода</param>
        /// <param name="end">Дата конца периода</param> 
        /// <param name="measureValue">В каких единицах необходимо получить значение (если в тыс. - measureValue = 1000) по умолчанию - в единицах</param>
        /// <returns>Нормализованная цена договора на заданный период (в рублях, в заданных единицах)</returns>
        public decimal GetPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {

                res = Stages.Where(item => item.IsLeaf() &&
                                   (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                   item.GetConditionForPeriod(start, end) == StageCondition.Overdue)).
                    Aggregate(res, (current, item) => current + item.StageMoneyModel.Factor.National.WithNdsValue);
            }

            return res / measureValue;
        }

        public decimal GetFactPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {

                res = Stages.Where(item => item.IsLeaf() && item.Act != null && 
                                   item.Act.Issigned.HasValue && item.Act.Issigned.Value &&
                                   (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                   item.GetConditionForPeriod(start, end) == StageCondition.Overdue)).
                    Aggregate(res, (current, item) => current + item.StageMoneyModel.Factor.National.WithNdsValue);
            }

            return res / measureValue;
        }

        public decimal GetApprovalStatePriceForPeriodWithNDS(DateTime start, DateTime end, WellKnownApprovalstates approvalstate, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {

                res = Stages.Where(item => item.IsLeaf() && 
                                   (item.Approvalstate != null && (WellKnownApprovalstates)item.Approvalstate.Id == approvalstate) && 
                                   (!item.Statedate.HasValue || (item.Statedate.HasValue && item.Statedate.Value.Between(start, end))) &&
                                   (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                   item.GetConditionForPeriod(start, end) == StageCondition.Overdue)).
                    Aggregate(res, (current, item) => current + item.StageMoneyModel.Factor.National.WithNdsValue);
            }

            return res / measureValue;
        }



        /// <summary>
        /// Возвращает стоимость по этапам планирующимся к выполнению соисполнителями
        /// </summary>
        /// <param name="start">начало периода</param>
        /// <param name="end">окончание периода</param>
        /// <param name="measureValue">единица измерения, руб</param>
        /// <returns>сумма этапов, выполненных соисполнителями</returns>
        public decimal GetCoworkersPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {
                var ss = Stages.Where(item => item.IsLeaf() &&
                                      (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                      item.GetConditionForPeriod(start, end) == StageCondition.Overdue));


                // в цикле выбираются только подписанные договора с соисполнителями
                IEnumerable<Stage> sss;

                foreach (Stage s in ss)
                {
                    if (s.SubContractsStages != null)
                    {
                        sss = s.SubContractsStages.Where(item => item.ContractObject.Contractstate.IsSigned &&
                                                                 item.IsLeaf() &&
                                                                 (item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.HaveToEnd ||
                                                                  item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.Overdue));

                        res = sss.Aggregate(res,
                                            (current, item) =>
                                            current + item.StageMoneyModel.Factor.National.WithNdsValue);
                    }
                }

            }

            return res / measureValue;
        }


        public decimal GetFactCoworkersPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {
                var ss = Stages.Where(item => item.IsLeaf() && item.Act != null &&
                                      item.Act.Issigned.HasValue && item.Act.Issigned.Value &&
                                      (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                      item.GetConditionForPeriod(start, end) == StageCondition.Overdue));


                // в цикле выбираются только подписанные договора с соисполнителями
                IEnumerable<Stage> sss;

                foreach (Stage s in ss)
                {
                    if (s.SubContractsStages != null)
                    {
                        sss = s.SubContractsStages.Where(item => item.ContractObject.Contractstate.IsSigned &&
                                                                 item.IsLeaf() &&
                                                                 (item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.HaveToEnd ||
                                                                  item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.Overdue));

                        res = sss.Aggregate(res,
                                            (current, item) =>
                                            current + item.StageMoneyModel.Factor.National.WithNdsValue);
                    }
                }

            }

            return res / measureValue;
        }


        public decimal GetApprovalStateCoworkersPriceForPeriodWithNDS(DateTime start, DateTime end, WellKnownApprovalstates approvalstate, int measureValue = 1)
        {
            decimal res = 0;
            //Если у договора есть календарный план с этапами - можем расчитать цену за период
            //Расчет ведем только по листьям

            //Расчитывает нормализованную цену. Т.е. в рублях и единицах
            if (Stages != null && Stages.Any())
            {
                var ss = Stages.Where(item => item.IsLeaf() &&
                                              (item.Approvalstate != null && (WellKnownApprovalstates)item.Approvalstate.Id == approvalstate) &&
                                              (!item.Statedate.HasValue ||
                                               (item.Statedate.HasValue && item.Statedate.Value.Between(start, end))) &&
                                              (item.GetConditionForPeriod(start, end) == StageCondition.HaveToEnd ||
                                               item.GetConditionForPeriod(start, end) == StageCondition.Overdue));


                // в цикле выбираются только подписанные договора с соисполнителями
                IEnumerable<Stage> sss;

                foreach (Stage s in ss)
                {
                    if (s.SubContractsStages != null)
                    {
                        sss = s.SubContractsStages.Where(item => item.ContractObject.Contractstate.IsSigned &&
                                                                 item.IsLeaf() &&
                                                                 (item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.HaveToEnd ||
                                                                  item.GetConditionForPeriod(start, end) ==
                                                                  StageCondition.Overdue));

                        res = sss.Aggregate(res,
                                            (current, item) =>
                                            current + item.StageMoneyModel.Factor.National.WithNdsValue);
                    }
                }

            }

            return res / measureValue;
        }
        /// <summary>
        /// Rule: Сумма аванса не может превышать сумму договора
        /// </summary>
        /// <param name="value">Сумма аванса</param>
        partial void OnPrepaymentsumChanging(decimal? value)
        {
            CheckPrepayment(value);
        }

        private void CheckPrepayment(decimal? value)
        {
            if (value.HasValue && Price.HasValue)
            {
                if (value > Price)
                    throw new ArgumentOutOfRangeException("Сумма аванса не может превышать сумму договора.");
            }
            if (value < 0) throw new ArgumentOutOfRangeException("Сумма аванса не может быть отрицательной.");
        }



        public ContractCondition ConditionOnDate(DateTime startDate, DateTime endDate)
        {
            Contract.Requires(ConditionResolver != null, "StateResolver не может быть null.");
            return ConditionResolver.GetContractCondition(this, startDate, endDate);
        }


        private static void FindAgreements(Contractdoc contractdoc, List<Contractdoc> result)
        {
            if (contractdoc == null) return;
            foreach (Contractdoc item in contractdoc.Contractdocs)
            {
                result.Add(item);

                FindAgreements(item, result);
            }
        }

        public void AddSubcontract(Contractdoc subcontract)
        {
            Contract.Requires(subcontract != null);
            var newEntity = new Contracthierarchy
                                {
                                    Subcontractdocid = subcontract.Id,
                                    SubContractdoc = subcontract,
                                    Generalcontractdocid = Id
                                };
            Contracthierarchies.Add(newEntity);
        }
        public decimal? Agreementreferencecount
        {
            get { return Agreements.Count(); }
        }

        public decimal? Generalreferencecount
        {
            get { return Generals.Count(); }
        }

        /// <summary>
        /// Добавляет доп. соглашение к договору
        /// </summary>
        /// <param name="agreement">Доп. соглашение</param>
        public void AddAgreement(Contractdoc agreement)
        {
            Contract.Requires(agreement != null);
            Contract.Requires(agreement.OriginalContract == null,
                              "Добавляемое соглашение не может иметь ссылок на генеральные договора.");
            Contract.Requires(agreement.AllAgreements.Count() == 0,
                              "Добавляемое соглашение не может иметь доп. соглашений");
            Contract.Requires(AllAgreements.Count() == 0, "Договор может иметь только одно соглашение.");
            Contract.Requires(agreement != this, "Рекурсивное добавление соглашений не допускается.");
            Agreements.Add(agreement);
        }

        partial void OnDeletedChanged()
        {
            SendPropertyChanged("Condition");
        }

        partial void OnCreated()
        {
            Contractstateid = ReservedUndifinedOid;

            // Отслеживание изменение в списке платёжных документов для перевычисления данных по авансу
            Contractpayments.ListChanged += new ListChangedEventHandler(Contractpayments_ListChanged);
            _paymentDocumentPropertyChanged = new PropertyChangedEventHandler(Paymentdocument_PropertyChanged);

            Contractor = new ContractorAggregator(this);

        }

        private PropertyChangedEventHandler _paymentDocumentPropertyChanged;

        void Contractpayments_ListChanged(object sender, ListChangedEventArgs e)
        {
            SendPropertyChanged("PrepaymentRests");
            if (e.ListChangedType == ListChangedType.ItemAdded && Contractpayments.Count() > e.NewIndex && e.NewIndex > -1)
                Contractpayments[e.NewIndex].Paymentdocument.PropertyChanged += _paymentDocumentPropertyChanged;
            if (e.ListChangedType == ListChangedType.ItemDeleted && Contractpayments.Count() > e.OldIndex && e.OldIndex > -1)
                Contractpayments[e.OldIndex].Paymentdocument.PropertyChanged -= _paymentDocumentPropertyChanged;
        }

        void Paymentdocument_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SendPropertyChanged("PrepaymentRests");
        }


        public static void AddInternalnum(string note, Contractdoc contractdoc, StringBuilder sb)
        {
            string num = string.IsNullOrEmpty(contractdoc.Num) ? " №(N/A)" : " №" + contractdoc.Num;
            sb.Append(note + num);
        }

        public static void AddApproved(Contractdoc contractdoc, StringBuilder sb)
        {
            if (contractdoc.Approvedat.HasValue)
                sb.Append(" от " + contractdoc.Approvedat.Value.ToShortDateString());
        }

        public override string ToString()
        {
            var contractdoc = this;
            var sb = new StringBuilder();


            if (contractdoc.IsGeneral && !contractdoc.IsAgreement)
            {
                sb.Append("Д. ");
                AddInternalnum(string.Empty, contractdoc, sb);
                AddApproved(contractdoc, sb);
                return sb.ToString();
            }

            var originalGenerals = contractdoc.OriginalGenerals.ToList();
            Contract.Assert(originalGenerals != null && originalGenerals.Any());
            var firstOriginal = originalGenerals.First();

            if (contractdoc.IsAgreement)
            {
                sb.Append("Д.С. " + contractdoc.Agreementnum);
                AddApproved(contractdoc, sb);
                AddInternalnum(" к ", firstOriginal, sb);              
            }

            if (contractdoc.IsSubContract)
            {
                if (contractdoc.IsAgreement) sb.Append(" к ");
                AddInternalnum("С.Д.", contractdoc, sb);
                AddInternalnum("к ", firstOriginal, sb);
                AddApproved(contractdoc, sb);
                return sb.ToString();
            }

            return sb.ToString();
        }

        #region Nested type: ContractstateErrorHandler

        private class ContractstateErrorHandler : IDataErrorHandler<Contractdoc>
        {
            #region IDataErrorHandler<Contractdoc> Members

            public string GetError(Contractdoc source, string propertyName, ref bool handled)
            {
                if (propertyName == Resource.Contractdoc_this_Contractstate ||
                    propertyName == Resource.Contractdoc_this_Appliedat)
                {
                    return HandleContractState(source, out handled);
                }
                return string.Empty;
            }

            #endregion

            private static string HandleContractState(Contractdoc source, out bool handled)
            {
                if (source.Contractstate != null)
                {
                    if (source.Contractstate.IsSigned && !source.Approvedat.HasValue)
                    {
                        handled = true;
                        return Resource.Contractdoc_this_Contractstate_mastbe_signed_if_approved_error;
                    }
                }
                else
                {
                    //if (source.Approvedat.HasValue)
                    //{
                    handled = true;
                    return Resource.Contractdoc_this_no_contractstate_error;
                    //}
                }
                handled = false;
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: CurrencyDataErrorHandler

        private class CurrencyDataErrorHandler : IDataErrorHandler<Contractdoc>
        {
            #region IDataErrorHandler<Contractdoc> Members

            public string GetError(Contractdoc source, string propertyName, ref bool handled)
            {
                if (propertyName == Resource.Contractdoc_this_Price ||
                    propertyName == Resource.Contractdoc_this_Currency ||
                    propertyName == Resource.Contractdoc_this_Currencyrate)
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractdoc source, out bool handled)
            {
                handled = false;
                if (source.Price.HasValue)
                {
                    if (source.Currency != null)
                    {
                        if (source.Currency.IsForeign)
                        {
                            if (!source.Currencyrate.HasValue)
                            {
                                handled = true;
                                return Resource.Contractdoc_this_foreign_currency_rate_error;
                            }
                        }
                    }
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: NdsDataErrorHandler

        private class NdsDataErrorHandler : IDataErrorHandler<Contractdoc>
        {
            #region IDataErrorHandler<Contractdoc> Members

            public string GetError(Contractdoc source, string propertyName, ref bool handled)
            {
                if (propertyName == Resource.Contractdoc_this_Ndsalgorithm ||
                    propertyName == Resource.Contractdoc_this_Nds)
                {
                    return HandleNdsError(source, out handled);
                }
                return string.Empty;
            }

            #endregion

            private static string HandleNdsError(Contractdoc source, out bool handled)
            {
                if (source.Ndsalgorithm != null)
                {
                    if (source.Ndsalgorithm.NdsType != TypeOfNds.NoNds)
                    {
                        if (source.Nds == null)
                        {
                            handled = true;
                            return Resource.Contractdoc_this_no_nds_algorithm_error;
                        }
                    }
                }
                handled = false;
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: PrepaymentDataErrorHandler

        private class PrepaymentDataErrorHandler : IDataErrorHandler<Contractdoc>
        {
            #region IDataErrorHandler<Contractdoc> Members

            public string GetError(Contractdoc source, string propertyName, ref bool handled)
            {
                if (propertyName == Resource.Contractdoc_this_Prepaymentpercent)
                {
                    return HandlePrepaymentPercentError(source, out handled);
                }

                if (propertyName == Resource.Contractdoc_this_Prepaymentsum)
                {
                    return HandlePrepaymentSumError(source, out handled);
                }
                return string.Empty;
            }

            #endregion

            private static string HandlePrepaymentPercentError(Contractdoc source, out bool handled)
            {
                if (source.Prepaymentpercent.HasValue)
                {
                    if (!source.Prepaymentpercent.Value.Between(Percent.Min, Percent.Max))
                    {
                        handled = true;
                        return
                            Resource.
                                PrepaymentDataErrorHandler_HandlePrepaymentError_PrepaymentPercentMustNotExceed0or100;
                    }
                }
                handled = false;
                return string.Empty;
            }

            private static string HandlePrepaymentSumError(Contractdoc source, out bool handled)
            {
                if (source.Price.HasValue && source.Prepaymentsum.HasValue)
                {
                    if (source.Prepaymentsum > source.Price)
                    {
                        handled = true;
                        return Resource.Contractdoc_this_prepayment_sum_must_not_exceed_total_error;
                    }
                }


                handled = false;
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: StartDateErrorHandler

        private class StartDateErrorHandler : IDataErrorHandler<Contractdoc>
        {
            #region IDataErrorHandler<Contractdoc> Members

            public string GetError(Contractdoc source, string propertyName, ref bool handled)
            {
                if (propertyName == Resource.Contractdoc_this_Startat ||
                    propertyName == Resource.Contractdoc_this_Endsat)
                {
                    return HandleDatesError(source, out handled);
                }
                return string.Empty;
            }

            #endregion

            private static string HandleDatesError(Contractdoc source, out bool handled)
            {
                handled = false;
                if (source.Startat > source.Endsat)
                {
                    handled = true;
                    return Resource.Contractdoc_this_endsat_less_startat_error;
                }
                return string.Empty;
            }
        }

        #endregion



        internal void InvalidatePaymentRests()
        {
            SendPropertyChanged("PrepaymentRests");
        }

        /// <summary>
        /// Получает или устанавливает контекстный договор. Используется для 
        /// задач привязки в средствах редактирования данных 
        /// </summary>
        public Contractdoc ContextContract { get; set; }

        public bool IsLastVersion
        {
            get { return !OriginalGenerals.Any(); }
        }
        /// <summary>
        /// Проверят, является ли данный договор собподрядным для договора, переданного в параметре
        /// </summary>
        /// <param name="general"></param>
        /// <returns></returns>
        public bool TestSubcontract(Contractdoc general)
        {
            return Generalcontracthierarchies.Select(x => x.SubContractdoc).Contains(general);
        }
        /// <summary>
        /// Получает или устанавливает признак субподрядного договора по отношению к ContextContract
        /// </summary>
        public bool IsSelected
        {
            get
            {
                Contract.Requires(ContextContract != null);
                return TestSubcontract(ContextContract);
            }
            set
            {
                var res = TestSubcontract(ContextContract);
                if (value == res) return;

                if (value)
                    ContextContract.Contracthierarchies.Add(new Contracthierarchy() { SubContractdoc = this });
                else
                {
                    ContextContract.Contracthierarchies.Remove(
                        ContextContract.Contracthierarchies.Single(x => x.SubContractdoc == this));
                }
            }
        }

        public Currency CurrencyOrDefault
        {
            get
            {
                if (Currency != null)
                {
                    return Currency;
                }
                return Currency.National;
            }
        }

        public bool IsNational
        {
            get { return CurrencyOrDefault.IsNational; }
        }

        public string GetColumnTitle(string mainpart, bool usemeasure)
        {
            var s = new StringBuilder();
            s.Append(mainpart);
            s.Append(", ");

            if (!usemeasure)
            {
                s.Append(CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
            }
            else
            {
                if (Currencymeasure != null)
                {

                    s.AppendFormat(Currencymeasure.CurrencyMeasureFormat,
                                   CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
                }
                else
                {

                    s.AppendFormat(Currencymeasure.FormValue(1).CurrencyMeasureFormat,
                                   CurrencyOrDefault.CI.NumberFormat.CurrencySymbol);
                }
            }
            return s.ToString();
        }


        public Ndsalgorithm Ndsalgorithm
        {
            get { return Ndsalgorithm_Ndsalgorithmid; }
            set
            {
                Ndsalgorithm_Ndsalgorithmid = value;
                SendPropertyChanged("Ndsalgorithm");
            }
        }

        //partial void OnDeltaChanged()
        //{
        //    if (Delta.HasValue)
        //        Endsat = default(DateTime?);
        //}


        public Ndsalgorithm Prepaymentndsalgorithm
        {
            get { return Ndsalgorithm_Prepaymentndsalgorithmid; }
            set
            {
                Ndsalgorithm_Prepaymentndsalgorithmid = value;
                SendPropertyChanged("Prepaymentndsalgorithm");
            }
        }

        public EntitySet<Contractdoc> Contractdocs
        {
            get { return Contractdocs_Origincontractid; }
        }

        public EntitySet<Contracthierarchy> Contracthierarchies
        {
            get
            {
                if (Contracthierarchies_Generalcontractdocid.Count > 0 || OriginalContract == null)
                    return Contracthierarchies_Generalcontractdocid;
                else
                    return OriginalContract.Contracthierarchies;
            }
        }

        public EntitySet<Contracthierarchy> Generalcontracthierarchies
        {
            get { return Contracthierarchies_Subcontractdocid; }
        }

        public bool IsDeltaCalculated
        {
            get { return Delta.HasValue; }

        }

        /// <summary>
        /// Получает сумму договора по всем этапам календарных планов
        /// </summary>
        public decimal StagesTotalPrice
        {
            get { return Stages.Sum(x => x.StageMoneyModel.PriceWithNdsValue); }
        }


        partial void OnDeltaChanged()
        {

            CalcNewEndsat();
            SendPropertyChanged("IsFixedEndsat");
            SendPropertyChanged("IsDeltaCalculated");
        }
        /// <summary>
        /// Если договор с открытой датой, то пересчитать дату окончания договора
        /// </summary>
        private void CalcNewEndsat()
        {
            if (IsDeltaCalculated)
                Endsat = this.GetCalculatedEndsat();
        }

        /// <summary>
        /// При изменении стартовой даты этапа пересчитать дату окончания
        /// </summary>
        partial void OnStartatChanged()
        {
            CalcNewEndsat();
            SendPropertyChanged("Delta");
        }

        /// <summary>
        /// Возвращает истина, если дата окончания этапа фиксирована
        /// </summary>
        public bool IsFixedEndsat
        {
            get { return !IsDeltaCalculated; }
        }

       
        /// <summary>
        /// Обновляет статистику по договору (DisbursedCache, LeftCache)
        /// </summary>
        public void UpdateFundsStatistics()
        {
            if (Contractpricecache == null) Contractpricecache = new Contractpricecache();
            Contractpricecache.DisbursedCache = Acts.Sum(x => x.ActMoney.Factor.National.PriceWithNdsValue);
            Contractpricecache.LeftCache = PriceMoneyModel.Factor.National.PriceWithNdsValue - Contractpricecache.DisbursedCache;
            Contractpricecache.StagesTotalPriceCache =
                Stages.Where(x => x.ParentStage == null).Sum(x => x.StageMoneyModel.Factor.National.PriceWithNdsValue);

            Contractpricecache.DisbursedCoworkersCache =
                SubContracts.SelectMany(x => x.Acts).Sum(x => x.ActMoney.Factor.National.PriceWithNdsValue);
            Contractpricecache.LeftCoworkersCache = SubContracts.Sum(x => x.PriceMoneyModel.Factor.National.PriceWithNdsValue) -
                                 Contractpricecache.DisbursedCoworkersCache;
        }

        public MoneyModel PriceMoneyModel
        {
            get
            {
                return new ContractMoneyModel(this);
            }
        }

        /// <summary>
        /// Получает сумму выполненных работ собственными силами
        /// </summary>
        public decimal? DisbursedByOwnPower
        {
            get
            {
                if (Contractpricecache == null) return default(decimal?);
                return Contractpricecache.DisbursedCache - Contractpricecache.DisbursedCoworkersCache;
            }
        }

        /// <summary>
        /// Получает сумму остатка работ, которые надо выполнить собственными силами
        /// </summary>
        public decimal? LeftByOwnPower
        {
            get
            {
                if (Contractpricecache == null) return default(decimal?);
                return Contractpricecache.LeftCache - Contractpricecache.LeftCoworkersCache;
            }
        }
        
        /// <summary>
        /// возвращает договор в группу которого входит текущий договор
        /// для субподрядов - первый оригинальный генерального
        /// для дс - первый оригинальный
        /// для оригинальных - себя самого
        /// </summary>
        public Contractdoc MainContract
        {
            get
            {
                var maincontract = this;
                foreach (var gen in Generals )
                {
                    maincontract = gen.MainContract;
                }

                while (maincontract.OriginalContract != null)
                {
                    maincontract = maincontract.OriginalContract;
                }


                return maincontract;

            }
        }


        public long? Maincontractid
        {
            get
            {
                return MainContract != null ? MainContract.Id : 0;
            }
        }


        public Currencymeasure Measure
        {
            get { return Currencymeasure; }
        }

        public void InvalidateCurrencyRate()
        {
           
        }
    }

    /// <summary>
    /// Получает Null-объект договора
    /// </summary>
    public class NullContractdoc : Contractdoc, INull
    {
        private const string Na = "Нет данных";

        public static readonly NullContractdoc Instance = new NullContractdoc
                                                              {
                                                                  Internalnum = Na,
                                                                  Contractornum = Na,
                                                                  Subject = Na,
                                                                  Contractor = NullContractorProxy.Instance
                                                              };

        #region INull Members

        bool INull.IsNull
        {
            get { return true; }
        }

        #endregion

        public override string ToString()
        {
            return "Не задан";
        }


    }
}
