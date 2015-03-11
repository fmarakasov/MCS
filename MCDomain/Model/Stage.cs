#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using MCDomain.DataAccess;
using CommonBase;

#endregion

namespace MCDomain.Model
{
    partial class Stage : IDateRange, IPrice, IObjectId, ICloneable,
        IDataErrorInfo, INull, IUnderResponsibility, ISupportStateApproval, ISupportDeltaEndDate, IClonableRecursive
    {
        private readonly DataErrorHandlers<Stage> _errorHandlers = new DataErrorHandlers<Stage>
                                                                       {
                                                                           new StageSubjectDataErrorHandler(),
                                                                           new NdsAlgotithmDataErrorHandler(),
                                                                           new NdsDataErrorHandler(),
                                                                           new StageStartDataErrorHandler(), 
                                                                           new StageEndDataErrorHandler()
                                                                       };

        private Stage _closedgeneralstage;
        
        private IStageConditionResolver _conditionResolver;
        private EditableObject<Stage> _editable;

        private ITimeResolver _timeResolver;


        public bool IsWellKnownId()
        {
            return false;
        }


        partial void OnEndsatChanged()
        {
            SendConditionChanged();
        }

        partial void OnStartsatChanged()
        {
            CalcNewEndsat();
            SendPropertyChanged("Delta");
            SendConditionChanged();
        }

        private void SendConditionChanged()
        {
            SendPropertyChanged("Stagecondition");
        }

        private IBindingList resultsBindingList;

        public Contractdoc Contractdoc
        {
            get { return Schedule.Schedulecontracts.Select(x => x.Contractdoc).FirstOrDefault() ?? NullContractdoc.Instance; }
        }

        public bool IsReadonly
        {
            get { return Stagecondition == StageCondition.Closed; }
        }

        /// <summary>
        /// Telerik Grid Column не поддерживает Margin, поэтому пришлось отказваться от конвертора и сделать все тут
        /// </summary>
        public string NumWithMargin
        {
            get
            {
                StringBuilder s = new StringBuilder();
                for (int i = 0; i < Level; i++)
                {
                    s.Append(" ");
                }
                s.Append(Num);
                return s.ToString();
            }
            set { Num = value.Trim(); }
        }

        public string NumWithSchedulenum
        {
            get
            {
                var s = new StringBuilder();

                var ss = Schedule.Schedulecontracts.FirstOrDefault();
                if (ss != null && ss.Appnum.HasValue)
                {
                    s.Append(ss.Appnum.Value);
                    s.Append('.');
                }
                s.Append(Num);

                return s.ToString();
            }
        }

        /// <summary>
        ///   Получает или устанавливает объект вычисления текущего времени. Если объект не установлен, то возвращается экземпляр DefaultTimeResolver
        /// </summary>
        public ITimeResolver TimeResolver
        {
            get { return _timeResolver ?? DefaultTimeResolver.Instance; }
            set { _timeResolver = value; }
        }

        /// <summary>
        ///   Получает текущее состояние этапа договора
        /// </summary>
        public StageCondition Stagecondition
        {
            get
            {
                Contract.Requires(TimeResolver != null);

                /*  так раньше было - убрали потому что бывает что ДСка добавляет подэтап к уже закрытому старшему этапу
                    return (ClosedGeneralStage == null)
                           ? GetConditionOnDate(TimeResolver.Now.Date)
                           : ClosedGeneralStage.Stagecondition; */

                return GetConditionOnDate(TimeResolver.Now.Date);
            }
        }

        /// <summary>
        ///   Получает или устанавливает объект для вычисления состояния этапа договора
        /// </summary>
        public IStageConditionResolver ConditionResolver
        {
            get { return _conditionResolver ?? StageConditionResolver.Instance; }
            set { _conditionResolver = value; }
        }

        /// <summary>
        ///   Получает или устанавливает уровень этапа.
        /// </summary>
        public int Level
        {
            get { return ParentStage == null ? 0 : ParentStage.Level + 1; }
        }

        partial void OnParentidChanged()
        {
            SendPropertyChanged("Level");
        }

        public IBindingList ResultsBindingList
        {
            get
            {
                if (resultsBindingList == null)
                    resultsBindingList = Stageresults.GetNewBindingList();

                return resultsBindingList;
            }
        }

        public Stage ParentStage
        {
            get { return Stage_Parentid; }

            set
            {
                Stage_Parentid = value;
                SendPropertyChanged("ParentStage");
                SendPropertyChanged("Level");
            }
        }

        /// <summary>
        ///   старший этап - для ДС
        /// </summary>
        public Stage ClosedGeneralStage
        {
            get
            {
                _closedgeneralstage = null;
                Closedstagerelation cs = Closedstagerelations.FirstOrDefault();
                if (cs != null)
                {
                    _closedgeneralstage = cs.ClosedStage;
                }

                return _closedgeneralstage;
            }
            set
            {
                if (_closedgeneralstage == value) return;

                // завязываем на закрытый этап

                Closedstagerelation cs = Closedstagerelations.FirstOrDefault();
                if (value != null)
                {
                    if (cs == null)
                    {
                        cs = new Closedstagerelation {ClosedStage = value};
                        Closedstagerelations.Add(cs);
                    }
                    else
                    {
                        cs.ClosedStage = value;
                    }
                }
                else
                {
                    if (cs != null) Closedstagerelations.Remove(cs);
                }
                _closedgeneralstage = value;
                SendPropertyChanged("ClosedGeneralStage");
            }
        }

        public IList<Stage> AgreementStages
        {
            get
            {
                if (Fromclosedstagerelations != null)
                    return Fromclosedstagerelations.Select(c => c.Stage_Stageid).ToList();
                else
                    return null;
            }
        }

        
        public bool IsOrphant
        {
            get { return Actid == default(long) && Stages.Any() && Stages.All(x => x.Actid != default(long)); }
        }

        public bool HasGeneralStage
        {
            get
            {
                return ContractObject.IsSubContract && GeneralStage != null;
            }
        }

        /// <summary>
        ///   получает связанный этап генерального договора
        /// </summary>
        public Stage GeneralStage
        {
            get
            {
                if (Generalsubgeneralhierarchies.Count == 0)
                    return null;

                return Generalsubgeneralhierarchies.FirstOrDefault().Stage_Generalcontractdocstageid;
            }

            set
            {
                if (value != null)
                {
                    if (Generalsubgeneralhierarchies.Count() == 0)
                        Generalsubgeneralhierarchies.Add(new Subgeneralhierarchi() { Generalstage = value, Substage = this });
                    else
                        Generalsubgeneralhierarchies.FirstOrDefault().Generalstage = value;
                }
                else
                {
                    Subgeneralhierarchi s =
                        Generalsubgeneralhierarchies.FirstOrDefault(x => x.Generalstage == GeneralStage);
                    if (s != null)
                        Generalsubgeneralhierarchies.Remove(s);
                }
                SendPropertyChanged("GeneralStage");
                SendPropertyChanged("HasGeneralStage");
            }
        }

        public void NotifyGeneralStageChanged()
        {
            SendPropertyChanged("GeneralStage");
            SendPropertyChanged("HasGeneralStage");
        }

        /// <summary>
        ///   получает связанные этапы субподрядных договоров
        /// </summary>
        public List<Stage> SubContractsStages
        {
            get
            {
                return Subgeneralhierarchis.Select(s => s.Substage).ToList();
            }
        }

        public Schedulecontract Schedulecontract
        {
            get
            {
                return
                    Schedule.Schedulecontracts.FirstOrDefault(
                        x => x.Contractdoc == Contractdoc && x.Schedule.Stages.FirstOrDefault(s => s == this) != null);
            }
        }

        public void RefreshSubstages()
        {
            SendPropertyChanged("SubContractsStages");
        }

        public String ContractNum
        {
            get
            {
                if (Schedule.Schedulecontracts.Count > 0)
                    return Schedule.Schedulecontracts.FirstOrDefault().Contractdoc.Internalnum;

                return null;
            }
        }

        /// <summary>
        ///   Получает признак того, что сам этап или какой-либо родительский этап закрыт актом
        /// </summary>
        public bool HasActOrParentHasAct
        {
            get { return Act != null || ParentAct != null; }
        }

        public Act ParentAct
        {
            get
            {
                Stage parent = ParentStage;

                while (parent != null)
                {
                    if (parent.Act != null) return parent.Act;
                    parent = parent.ParentStage;
                }
                return null;
            }
        }


        public Quarters? PlanFinishingQuarter(int Year)
        {
            if (Endsat.HasValue&&Endsat.Value.Year == Year)
                return DateTimeExtensions.GetQuarterByDate(Endsat.Value);
            return null;
        }

        public int? PlanFinishingYear
        {
            get
            {
                if (Endsat.HasValue) return Endsat.Value.Year;
                return null;
            }
        }

        public Quarters? FinishingQuarter(int Year)
        {
           if (HasActOrParentHasAct)
           {
               DateTime? dt = null;
               if (Act != null && Act.Issigned.HasValue && Act.Issigned.Value) dt = Act.Signdate;
               else if (ParentAct != null && ParentAct.Issigned.HasValue && ParentAct.Issigned.Value) dt = ParentAct.Signdate;

               if (dt != null && dt.Value.Year == Year)
                   return DateTimeExtensions.GetQuarterByDate(dt.Value);
           }
           return null;
        }

        public Contractdoc ContractObject
        {
            get { return Contractdoc; }
        }

        public Currency ContractCurrency
        {
            get
            {
                if (ContractObject == null) return null;
                return ContractObject.Currency;
            }
        }

        /// <summary>
        /// возвращает валюту для этапа 
        /// которая соответствует договору - если задана для него
        /// или является российским рублем
        /// </summary>
        public Currency ContractCurrencyOrDefault
        {
            get
            {
                if (ContractCurrency != null)
                {
                    return ContractCurrency;
                }
                else return Currency.National;
            }
        }

        public decimal? ContractCurrencyRate
        {
            get
            {
                if (ContractObject == null) return default(decimal?);
                return ContractObject.Currencyrate;
            }
        }

        public Currencymeasure ScheduleCurrencyMeasure
        {
            get
            {
                if (Schedule == null) return null;
                return Schedule.Currencymeasure;
            }
        }

        public MoneyModel StageMoneyModel
        {
            get
            {
                return new MoneyModel(Ndsalgorithm, Nds, ContractCurrency, ScheduleCurrencyMeasure, Price,
                                             ContractCurrencyRate);
            }
        }

        /// <summary>
        /// свойство, возвращающее цену только для терминальных этапов
        /// нужно, чтобы правильно суммы считать в таблице КП
        /// </summary>
        public decimal TerminalStagePrice
        {
            get
            {
                if (Stages.Count == 0) return Price.HasValue?Price.Value:0;
                else return 0;
            }
        }

        /// <summary>
        /// свойство, возвращающее цену без НДС только для терминальных этапов
        /// нужно, чтобы правильно суммы считать в таблице КП
        /// </summary>
        public decimal TerminalStagePureValue
        {
            get
            {
                if (Stages.Count == 0) return StageMoneyModel.PureValue;
                else return 0;
            }
        }

        /// <summary>
        /// свойство, возвращающее НДС только для терминальных этапов
        /// нужно, чтобы правильно суммы считать в таблице КП
        /// </summary>
        public decimal TerminalStageNdsValue
        {
            get
            {
                if (Stages.Count == 0) return StageMoneyModel.NdsValue;
                else return 0;
            }
        }

        /// <summary>
        /// свойство, возвращающее цену с НДС только для терминальных этапов
        /// нужно, чтобы правильно суммы считать в таблице КП
        /// </summary>
        public decimal TerminalStagePriceWithNdsValue
        {
            get
            {
                if (Stages.Count == 0) return StageMoneyModel.PriceWithNdsValue;
                else return 0;
            }
        }

        public Act ActObject
        {
            get { return Act; }
            set
            {
                Act = value;
                SendPropertyChanged("ActObject");
            }
        }


        /// <summary>
        /// Получает перечисленные средства по акту
        /// </summary>
        public decimal ActTransferedFunds
        {
            get
            {
                decimal sum = 0;
                if (Act != null)
                {
                    sum = Act.Sumfortransfer.GetValueOrDefault();

                    if (!StageMoneyModel.IsForeignOrFactored) return sum;
                    else
                    {
                        sum = sum/(Schedule.Currencymeasure.Factor.HasValue ? Schedule.Currencymeasure.Factor.Value : 1);

                        if (StageMoneyModel.Currency.IsForeign)
                            sum = sum/(ContractObject.Currencyrate.HasValue ? ContractObject.Currencyrate.Value : 1);

                    }
                }
                return sum;
            }
        }



        public decimal SubcontractStagesSum
        {
            get
            {
                decimal sum = 0;
                // сумма в рублях
                if (SubContractsStages != null)
                {
                    sum = SubContractsStages.Sum(s => s.Stages.Count == 0?s.StageMoneyModel.National.Factor.PriceWithNdsValue:0);

                    if (!StageMoneyModel.IsForeignOrFactored) return sum;
                    else
                    {
                        sum = sum/(Schedule.Currencymeasure.Factor.HasValue ? Schedule.Currencymeasure.Factor.Value : 1);

                        if (StageMoneyModel.Currency.IsForeign)
                            sum = sum/(ContractObject.Currencyrate.HasValue ? ContractObject.Currencyrate.Value : 1);

                    }
                }
                return sum;
            }
        }

        public decimal SubcontractActTransferedFunds
        {
            get 
            {
                decimal sum = 0;
                if (SubContractsStages != null)
                {

                    sum = SubContractsStages.Sum(s => s.Stages.Count == 0 ? s.ActTransferedFunds : 0);
                        
                    if (!StageMoneyModel.IsForeignOrFactored) return sum;
                    else
                    {
                        sum = sum / (Schedule.Currencymeasure.Factor.HasValue ? Schedule.Currencymeasure.Factor.Value : 1);

                        if (StageMoneyModel.Currency.IsForeign)
                            sum = sum / (ContractObject.Currencyrate.HasValue ? ContractObject.Currencyrate.Value : 1);

                    }
                }

                return sum;
            }
        }

        /// <summary>
        ///   Возвращает сроки этапа в виде строки
        /// </summary>
        public string RunTime
        {
            get
            {
                return
                    (Startsat.HasValue ? Startsat.Value.ToString("dd.MM.yyyy") : string.Empty) +
                    (Endsat.HasValue ? " - " + Endsat.GetValueOrDefault().ToString("dd.MM.yyyy") : string.Empty);
            }
        }

        /// <summary>
        ///   Получает корневой этап
        /// </summary>
        public Stage RootStage
        {
            get
            {
                Stage res = this;
                while (res.ParentStage != null)
                {
                    res = res.ParentStage;
                }
                return res;
            }
        }

        #region IClonablePersistent Members

        public object CloneRecursively(object owner, object source)
        {
            Stage s;
            
            s = new Stage
                    {
                        Num = Num,
                        Subject = Subject,
                        Startsat = Startsat,
                        Endsat = Endsat,
                        Price = Price,
                        Act = Act,
                        Ndsalgorithm = Ndsalgorithm,
                        Nds = Nds,
                        Code = Code,
                        ConditionResolver = ConditionResolver

                        //Level = Level
                    };

            if (owner is Schedule)
                s.Schedule = (Schedule) owner;
            else if (owner is Stage)
            {
                s.ParentStage = (Stage) owner;
                
                s.Schedule = ((Stage) owner).Schedule;
                
            }

            if (Act != null)
            {
                s.ClosedGeneralStage = Act.Stages.FirstOrDefault(p => p.Num == s.Num);
            }

            Stageresult nsr;
            foreach (Stageresult sr in Stageresults)
            {
                nsr = (Stageresult)sr.CloneRecursively(s, null);
                s.Stageresults.Add(nsr);
            }


            Stage ns;
            foreach (Stage ss in Stages)
            {
                ns = (Stage)ss.CloneRecursively(s, null);
                s.Stages.Add(ns);
            }

            return s;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            Stage s;
            s = new Stage
                    {
                        Num = Num,
                        Subject = Subject,
                        Startsat = Startsat,
                        Endsat = Endsat,
                        Price = Price,
                        Act = Act,
                        Ndsalgorithm = Ndsalgorithm,
                        Nds = Nds,
                        Code = Code,
                        ConditionResolver = ConditionResolver
                    };


            Stageresult nsr;
            foreach (Stageresult sr in Stageresults)
            {
                nsr = (Stageresult) sr.Clone();
                nsr.Stage = s;
                s.Stageresults.Add(nsr);
            }

            Stage ns;
            foreach (Stage ss in Stages)
            {
                ns = (Stage) ss.Clone();
                ns.ParentStage = s;
                s.Stages.Add(ns);
            }


            return s;
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region IDateRange Members

        /// <summary>
        ///   Получает или устанавливает сроки выполнения этапа
        /// </summary>
        public DateRange Range
        {
            get
            {
                var DT = new DateRange();
                if (Startsat.HasValue)
                    DT.Start = Startsat.Value;
                if (Endsat.HasValue)
                    DT.End = Endsat.Value;

                return DT;
            }
            set
            {
                Startsat = value.Start;
                Endsat = value.End;
            }
        }

        #endregion

        #region Implementation of IPrice

        //public double PriceNds
        //{
        //    get
        //    {
        //        if (Ndsalgorithm == null)
        //            throw new NoNullAllowedException(
        //                "Не задано значение свойства Ndsalgorithm. Расчёт НДС не может быть продолжен");
        //        Contract.EndContractBlock();
        //        return Ndsalgorithm.GetNds(Price, this.Nds.Fraction);
        //    }
        //}


        //public double PriceWithNds
        //{
        //    get
        //    {
        //        if (Ndsalgorithm == null)
        //            throw new NoNullAllowedException(
        //                "Не задано значение свойства Ndsalgorithm. Расчёт НДС не может быть продолжен");
        //        return Ndsalgorithm.GetPure(Price, this.Nds.Fraction) + Ndsalgorithm.GetNds(Price, this.Nds.Fraction);
        //    }
        //}

        public decimal PriceValue
        {
            get
            {
                if (Price.HasValue)
                    return Price.Value;
                else
                    return 0;
            }
            set { Price = value; }
        }


        /// <summary>
        ///   Получает фактическое состояние этапа договора на заданную дату
        /// </summary>
        /// <param name = "onDate">Дата</param>
        /// <returns>Состояние этапа договора</returns>
        public StageCondition GetConditionOnDate(DateTime onDate)
        {
            return ConditionResolver.GetStageCondition(this, onDate);
        }

        /// <summary>
        ///   Получает плановое состояние этапа договора на заданную дату
        /// </summary>
        /// <param name = "onDate">Дата</param>
        /// <returns>Состояние этапа договора</returns>
        public StageCondition GetPlanConditionOnDate(DateTime onDate)
        {
            return ConditionResolver.GetPlanStageCondition(this, onDate);
        }

        /// <summary>
        /// </summary>
        /// <param name = "startPeriod"></param>
        /// <param name = "endPeriod"></param>
        /// <returns>Состояние договора на период</returns>
        public StageCondition GetConditionForPeriod(DateTime startPeriod, DateTime endPeriod)
        {

            //Если не заданы дата начала или дата конца этапа - неопределенное состояние
            if (!Startsat.HasValue || !Endsat.HasValue) return StageCondition.Undefined;

            //если на начало периода этап уже закрыт - состояние на период - закрытое
            if (GetConditionOnDate(startPeriod) == StageCondition.Closed) return StageCondition.Closed;

            //Если дата окончания этапа лежит между датой начала и датой конца периода, значит этап должнен быть сдан в текущем периоде
            if (Endsat.Value.Between(startPeriod, endPeriod)||GetConditionOnDate(endPeriod)==StageCondition.Closed)
                return StageCondition.HaveToEnd;

            //Если на начало периода этап ожидает выполнения, а на конец - состояние изменилось или на начало периоди этап в состоянии активен - этап на период считается активным
            if ((GetConditionOnDate(startPeriod) == StageCondition.Pending &&
                    GetConditionOnDate(endPeriod) != StageCondition.Pending) ||
                (GetConditionOnDate(startPeriod) == StageCondition.Active))
                return StageCondition.Active;

            //если на начало период этап просрочен - состояние на период - просрочен 
            if (GetConditionOnDate(startPeriod) == StageCondition.Overdue) return StageCondition.Overdue;

            if (GetConditionOnDate(endPeriod) == StageCondition.Pending) return StageCondition.Pending;

            return StageCondition.Undefined;
        }

        public StageCondition GetPlanConditionForPeriod(DateTime startPeriod, DateTime endPeriod)
        {
            //Если не заданы дата начала или дата конца этапа - неопределенное состояние
            if (!Startsat.HasValue || !Endsat.HasValue) return StageCondition.Undefined;

            //Если дата окончания этапа лежит между датой начала и датой конца периода, значит этап должнен быть сдан в текущем периоде
            if (Endsat.Value.Between(startPeriod, endPeriod))
                return StageCondition.HaveToEnd;

            //Если на начало периода этап ожидает выполнения, а на конец - состояние изменилось или на начало периоди этап в состоянии активен - этап на период считается активным
            if ((GetPlanConditionOnDate(startPeriod) == StageCondition.Pending &&
                 GetPlanConditionOnDate(endPeriod) != StageCondition.Pending) ||
                (GetPlanConditionOnDate(startPeriod) == StageCondition.Active))
                return StageCondition.Active;

            //если на начало период этап просрочен - состояние на период - просрочен 
            if (GetPlanConditionOnDate(startPeriod) == StageCondition.Overdue) return StageCondition.Overdue;

            if (GetPlanConditionOnDate(endPeriod) == StageCondition.Pending) return StageCondition.Pending;

            return StageCondition.Undefined;
        }

        #endregion

        #region INull Members

        bool INull.IsNull
        {
            get { return false; }
        }

        #endregion

        partial void OnCreated()
        {
            _editable = new EditableObject<Stage>(this);
            Ndsalgorithmid = ReservedUndifinedOid;
            Startsat = null;
            Endsat = null;
            SendPropertyChanged("Startsat");
            SendPropertyChanged("Endsat");
        }

        /// <summary>
        ///   Возвращает признак того, что этап не является для других родителем
        /// </summary>
        /// <returns>Признак того, что этап является листом</returns>
        public bool IsLeaf()
        {
            return Schedule.StageIsLeaf(this);
        }


        public IEnumerable<Stage> GetNextChildren()
        {
            return Schedule.GetNextChildren(this);
        }

        /// <summary>
        ///   Возвращает цену этапа, если он должен был закончиться в заданном периоде
        /// </summary>
        /// <param name = "startPeriod">Дата начала периода</param>
        /// <param name = "endPeriod">Дата конца периода</param>
        /// <returns>Цена этапа в заданном периоде</returns>
        public decimal PriceOnPeriodWithNDS(DateTime startPeriod, DateTime endPeriod, int measureValue = 1)
        {
            StageCondition s = GetConditionForPeriod(startPeriod, endPeriod);
            return ((s == StageCondition.HaveToEnd||(s == StageCondition.Closed && GetConditionOnDate(startPeriod) != StageCondition.Closed)) && Price.HasValue)
                        ? StageMoneyModel.Factor.National.WithNdsValue / measureValue
                        : 0;
  
        }

        public decimal PriceOnPeriodForWaitingForSignature(DateTime startPeriod, DateTime endPeriod, int measureValue = 1)
        {
            StageCondition s = GetConditionForPeriod(startPeriod, endPeriod);
            
            return (s != StageCondition.Closed && s != StageCondition.HaveToEnd && Approvalstate != null && Approvalstate.WellKnownType == WellKnownApprovalstates.WaitingForSignature &&
                    Statedate.HasValue && Statedate.Value.Between(startPeriod, endPeriod))
                       ? StageMoneyModel.Factor.National.WithNdsValue/measureValue
                       : 0;

        }

        /// <summary>
        ///   Возвращает цену этапа, если он по плану должен закончиться в заданном периоде
        /// </summary>
        /// <param name = "startPeriod">Дата начала периода</param>
        /// <param name = "endPeriod">Дата конца периода</param>
        /// <returns>Цена этапа в заданном периоде</returns>
        public decimal PlanPriceOnPeriodWithNDS(DateTime startPeriod, DateTime endPeriod, int measureValue = 1)
        {
           StageCondition s = GetPlanConditionForPeriod(startPeriod, endPeriod);
           return (s == StageCondition.HaveToEnd && Price.HasValue) ? StageMoneyModel.Factor.National.WithNdsValue / measureValue : 0;
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

            StageCondition stc = GetConditionForPeriod(start, end);
            if (SubContractsStages != null && (stc == StageCondition.HaveToEnd || stc == StageCondition.Overdue || stc == StageCondition.Closed))
            {
                  //item.ContractObject.Contractstate.IsSigned - убрано, потому что там непонятно

                var sss = SubContractsStages.Where(item =>     item.ContractObject.Contractstate.IsSigned&&
                                                               item.IsLeaf() &&
                                                               (item.GetConditionForPeriod(start, end) ==
                                                               StageCondition.HaveToEnd ||
                                                               item.GetConditionForPeriod(start, end) ==
                                                               StageCondition.Overdue ||
                                                               item.GetConditionForPeriod(start, end) ==
                                                               StageCondition.Closed));

                  res = sss.Aggregate(res,
                                      (current, item) =>
                                       current + item.StageMoneyModel.Factor.National.WithNdsValue);
                    
                

            }

            return res / measureValue;
        }

        /// <summary>
        /// Возвращает стоимость по этапам планирующимся к выполнению соисполнителями, если этапы должны по плану закончиться в заданном периоде
        /// </summary>
        /// <param name="start">начало периода</param>
        /// <param name="end">окончание периода</param>
        /// <param name="measureValue">единица измерения, руб</param>
        /// <returns>сумма этапов, выполненных соисполнителями</returns>
        public decimal GetPlanCoworkersPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;

            if (SubContractsStages != null && (GetConditionForPeriod(start, end)== StageCondition.HaveToEnd || GetConditionForPeriod(start, end) == StageCondition.Overdue))
            {
                var sss = SubContractsStages.Where(item => item.ContractObject.Contractstate.IsSigned &&
                                                             item.IsLeaf() &&
                                                             (item.GetPlanConditionForPeriod(start, end) ==
                                                             StageCondition.HaveToEnd ||
                                                             item.GetPlanConditionForPeriod(start, end) ==
                                                             StageCondition.Overdue));

                res = sss.Aggregate(res,
                                    (current, item) =>
                                     current + item.StageMoneyModel.Factor.National.WithNdsValue);



            }

            return res / measureValue;
        }



        /// <summary>
        /// Возвращает стоимость по просроченным этапам, выполняемым соисполнителями
        /// </summary>
        /// <param name="start">начало периода</param>
        /// <param name="end">окончание периода</param>
        /// <param name="measureValue">единица измерения, руб</param>
        /// <returns>сумма этапов, выполненных соисполнителями</returns>
        public decimal GetOverdueCoworkersPriceForPeriodWithNDS(DateTime start, DateTime end, int measureValue = 1)
        {
            decimal res = 0;

            if (SubContractsStages != null && GetConditionForPeriod(start, end) == StageCondition.Overdue)
            {
                var sss = SubContractsStages.Where(item =>   item.ContractObject.Contractstate.IsSigned &&
                                                             item.IsLeaf() &&
                                                             item.GetConditionForPeriod(start, end) == StageCondition.Overdue);

                res = sss.Aggregate(res,
                                    (current, item) =>
                                     current + item.StageMoneyModel.Factor.National.WithNdsValue);



            }

            return res / measureValue;
        }

        public void OnNdsPriceChanged()
        {
            SendPropertyChanged("PriceNds");
            SendPropertyChanged("PriceWithNds");
            SendPropertyChanged("StageMoneyModel");
        }

        public void OnStageconditionChanged()
        {
            SendPropertyChanged("Stagecondition");
            SendPropertyChanged("IsReadonly");
        }

        /// <summary>
        ///   обход проблем с LinqConnect
        /// </summary>
        public override string ToString()
        {
            return SimpleName;
        }

        public string SimpleName
        {
            get
            {
                return Num + " - " + Subject;
            }
        }
        public void RefreshBindingList()
        {
            resultsBindingList = null;
        }

        public void OnResultsChanged()
        {
            RefreshRespBindingList();
            SendPropertyChanged("ResultsBindingList");
        }

        public void SetLevel()
        {
            //level = 0;
            Stage Parent = this.ParentStage;
            while (Parent != null)
            {
                //level++;
                Parent = Parent.ParentStage;
            }
            //Level = level;
        }

        /// <summary>
        ///   Получает признак того, что сам этап или какой-либо родительский этап закрыт актом на определененную дату
        /// </summary>
        /// <param name = "date">Дата</param>
        /// <returns></returns>
        public bool HasActOrParentHasActOnDate(DateTime date)
        {
            if ((Act != null && Act.Signdate != null && Act.Signdate <= date && Act.Issigned.GetValueOrDefault())) return true;
            
            return (ParentAct != null && ParentAct.Signdate != null && ParentAct.Signdate <= date && ParentAct.Issigned.GetValueOrDefault() && ParentAct.Stages.Count(s => s.Schedule.Id == Schedule.Id) == ParentStage.Stages.Count );
        }

        public Stage FindStage(string stagenum)
        {
            Stage s = Stages.FirstOrDefault(p => p.Num == stagenum);
            if (s == null)
            {
                foreach (Stage ss in Stages)
                {
                    s = ss.FindStage(stagenum);
                }
            }

            return s;
        }

        /// <summary>
        ///   Получает все листья этапа (т.е. для этапов 1, 1.1, 1.2, 1.2.1, 1.2.2, 1.3 результатом будет 1.1, 1.2.1, 1.2.2, 1.3)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Stage> GetLeafs()
        {
            return Schedule.GetLeafs(this);
        }


        public EntitySet<Subgeneralhierarchi> Subgeneralhierarchis
        {
            get { return Subgeneralhierarchis_Generalcontractdocstageid; }
        }

        public EntitySet<Subgeneralhierarchi> Generalsubgeneralhierarchies
        {
            get { return Subgeneralhierarchis_Subcontractdocstageid;  }
        }

        public EntitySet<Stage> Stages
        {
            get { return Stages_Parentid; }
        }

        public EntitySet<Closedstagerelation> Closedstagerelations
        {
            get { return Closedstagerelations_Stageid; }
        }

        public EntitySet<Closedstagerelation> Fromclosedstagerelations
        {
            get { return Closedstagerelations_Closedstageid; }
        }

        private  void CheckChildParentPrice()
        {
            if (Stages.Count() > 0)
            {
                Price = Stages.Sum(x => x.Price);
            }
        }

        private void CheckChildParentDates()
        {
            if (Stages.Count() > 0)
            {
                Startsat = Stages.Min(x => x.Startsat);
                Endsat = Stages.Max(x => x.Endsat);
            }
        }

        public void CheckChildParentProperties()
        {
            CheckChildParentDates();
            CheckChildParentPrice();

            if (ParentStage != null)
              ParentStage.CheckChildParentProperties();
            
        }

        #region IUnderResponsibleMembers

        public void RemoveResponsiblesForDisposal(Disposal disposal)
        {
            for (int i = Responsibles.Count - 1; i >= 0; i--)
            {
                if (Responsibles[i].Disposal != null && Responsibles[i].Disposal.Id == disposal.Id)
                {
                    Responsibles[i].Contractdoc = null;
                    Responsibles.RemoveAt(i);
                }
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

                if (includeordersuperviser&&OrderSuperviserEmployee != null) all.Add(OrderSuperviserEmployee);

                var dist = all.Where(a => a.Id != EntityBase.ReservedUndifinedOid).Distinct();
                if (dist.Count() > 1)
                    return dist.Aggregate("\n", (e, next) => e + next.ToString() + "\n");
                else if (dist.Count() == 1)
                    return dist.ElementAt(0).ToString();
                else return string.Empty;

            
        }

        private IBindingList _responsiblesbindinglist;
        public IBindingList ResponsiblesBindingList
        {
            get
            {
                if (_responsiblesbindinglist == null)
                {
                    _responsiblesbindinglist = new BindingList<Responsible>();
                    foreach (var r in Responsibles)
                    {
                        if ((Disposal != null) && (r.Disposal != null) && (r.Disposal.Id == Disposal.Id))
                        {
                            _responsiblesbindinglist.Add(r);
                        }
                    }
                }
                return _responsiblesbindinglist;
            }
        }

        public void RefreshRespBindingList()
        {
            _responsiblesbindinglist = null;
        }

        public void SendResponsiblesBindingListChanged()
        {
            SendPropertyChanged("ResponsiblesBindingList");
            SendPropertyChanged("DisposalPersons");
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
                    return Disposal.GetChief(this);
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
                    return Disposal.GetChiefs(this);
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
                    return Disposal.GetManager(this);
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
                        Disposal.GetDirector(this);
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
                    return Disposal.GetCurator(this);
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
                        Disposal.GetOrderSuperviser(this);
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
        public Disposal StageDisposal
        {
            get
            {
                var resp = Responsibles.Where(r => r.Stage.Id == this.Id&&r.Disposal != null).FirstOrDefault();
                if (resp != null) return resp.Disposal;
                
                // пытаемся перейти к родительскому этапу и найти его распоряжение
                if (ParentStage != null) return ParentStage.StageDisposal;

                return null;
            }
        }


        public Disposal ClosedStageDisposal
        {
            get 
            {
                
                return ClosedGeneralStage != null ? ClosedGeneralStage.StageDisposal : null;
            }
        }

        public Disposal Disposal
        {
            get
            {
                return StageDisposal ?? ClosedStageDisposal ?? ContractObject.Disposal;
            }
        }



        /// <summary>
        /// отдел, за которым договор закреплен по распоряжению
        /// </summary>
        public Department DisposalDepartment
        {
            get { return (Chief != null) ? Chief.Employee.Department : null; }
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


        #endregion

        #region Nested type: NdsAlgotithmDataErrorHandler

        private class NdsAlgotithmDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Ndsalgorithm")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (source.Ndsalgorithm == null)
                {
                    handled = true;
                    return "Алгоритм НДС не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: NdsDataErrorHandler

        private class NdsDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Nds")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (source.Nds == null)
                {
                    handled = true;
                    return "Ставка НДС не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: StageEndDataErrorHandler

        private class StageEndDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Endsat")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (source.Startsat.HasValue && source.Endsat.HasValue &&  source.Endsat.Value < source.Startsat.Value)
                {
                    handled = true;
                    return "Дата окончания не может быть меньше даты начала этапа!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: StagePriceDataErrorHandler

        private class StagePriceDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Price")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (source.Price <= 0)
                {
                    handled = true;
                    return "Стоимость этапа должна быть положительным числом!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: StageStartDataErrorHandler

        private class StageStartDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Startsat")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (source.Startsat.HasValue && source.Endsat.HasValue && source.Startsat.Value > source.Endsat.Value)
                {
                    handled = true;
                    return "Дата начала не может быть больше даты окончания этапа!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: StageSubjectDataErrorHandler

        private class StageSubjectDataErrorHandler : IDataErrorHandler<Stage>
        {
            #region IDataErrorHandler<Stage> Members

            public string GetError(Stage source, string propertyName, ref bool handled)
            {
                if (propertyName == "Subject")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Stage source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Subject))
                {
                    handled = true;
                    return "Содержание этапа не может быть пустым!";
                }

                if (source.Schedule != null && (from schedulecontract in source.Schedule.Schedulecontracts
                                                from stage in schedulecontract.Schedule.Stages
                                                where (source.Schedule != null) && (stage.Schedule != null)
                                                select stage).Any(
                                                    stage =>
                                                    (source.Schedule.Worktype == stage.Schedule.Worktype) &&
                                                    (source != stage) &&
                                                    (source.ParentStage == stage.ParentStage) &&
                                                    (source.Subject == stage.Subject)))
                {
                    handled = true;
                    return
                        "Содержание этапа должно быть уникальным в рамках версии календарного плана и вида работ!";
                }

                return string.Empty;
            }
        }

        #endregion

        #region Члены ISupportStateApproval

        private const byte Mask = 2;
        public byte TypeMask
        {
            get { return Mask; }
        }

        #endregion

        public bool IsDeltaCalculated
        {
            get { return Delta.HasValue; }

        }

        public DateTime? Startat
        {
            get { return Startsat; }
            set 
            {
                if (Startsat == value) return;
                Startsat = value;
                SendPropertyChanged("Startsat");
            }
        }
        partial void OnDeltaChanged()
        {
            CalcNewEndsat();
            SendPropertyChanged("IsFixedEndsat");
            SendPropertyChanged("IsDeltaCalculated");
        }

        private void CalcNewEndsat()
        {
            if (IsDeltaCalculated)
                Endsat = this.GetCalculatedEndsat();
        }

        
        public bool IsFixedEndsat
        {
            get { return !IsDeltaCalculated; }
        }


        public string Directors
        {
            get
            {
                IList<Employee> all = new List<Employee>();
                if (DirectorEmployee != null) all.Add(DirectorEmployee);
                if (ManagerEmployee != null) all.Add(ManagerEmployee);

                
                var dist = all.Where(a => a.Id != EntityBase.ReservedUndifinedOid).Distinct().ToArray();
                var count = dist.Count();
                return count > 1
                           ? dist.Aggregate("\n", (e, next) => e + next.ToString() + "\n")
                           : (count == 1 ? dist.ElementAt(0).ToString() : string.Empty);
            }
        }
    }

    /// <summary>
    ///   Представляет Null-объект этапа
    /// </summary>
    public class NullStage : Stage, INull
    {
        public static readonly NullStage Instance = new NullStage {Num = "Нет данных"};

        #region INull Members

        bool INull.IsNull
        {
            get { return true; }
        }

        #endregion
    }
}
