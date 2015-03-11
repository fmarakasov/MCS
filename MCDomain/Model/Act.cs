using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;
using UOW;

namespace MCDomain.Model
{
    public partial class Act : IDataErrorInfo, IAct 
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            _Signdate = DateTime.Today;
        }

        public override string ToString()
        {
            return FullName;

        }



        //private EditableObject<Act> _editable;
        private readonly DataErrorHandlers<Act> _errorHandlers = new DataErrorHandlers<Act>
                                                                     {
                                                                         new ActtypeDataErrorHandler(),
                                                                         new NdsDataErrorHandler(),
                                                                         new RegionDataErrorHandler(),
                                                                         new EnterpriseauthorityDataErrorHandler(),
                                                                         new ActMustHaveDateIfSignedHandler(),
                                                                         new ActCurrencyRateHandler(),
                                                                         new ZeroFundsHandler()
                                                                     };



        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        private MoneyModel CreateMoneyModel(decimal? price)
        {
            var ndsalgorithm = Model.Ndsalgorithm.TypeIdToObject(Ndsalgorithmid);
            return new MoneyModel(ndsalgorithm, Nds, Currency, Currencymeasure, price, Currencyrate);
        }

        /// <summary>
        /// Получает данные об общей сумме по акту
        /// </summary>
        public MoneyModel ActMoney
        {
            get { return CreateMoneyModel(Totalsum); }
        }

        public MoneyModel CreditedMoneyModel
        {
            get { return CreateMoneyModel(Credited); }
        }

        /// <summary>
        /// Получает данные по сумме к перечислению
        /// </summary>
        public MoneyModel TransferSumMoney
        {
            get { return CreateMoneyModel(Sumfortransfer); }
        }

        /// <summary>
        /// Получает номера закрытых этапов в виде строки
        /// </summary>
        public String StagesNumbers
        {
            get
            {
                if (Stages.Count == 0) return String.Empty;
                return Stages.Select(x=>x.Num).Distinct().OrderBy(x => x, new NaturSortComparer<Stage>()).Aggregate(string.Empty,
                                                                                            (result, next) =>
                                                                                            result + (next + "; "));
            }
        }

        /// <summary>
        /// получает закрытые этапы оригинального договора (того, которому принадлежит акт)
        /// </summary>
        public IList<Stage> OriginalStages
        {
            get
            {
                if (ContractdocActAssignedFor != null) return Stages.Where(s => ContractdocActAssignedFor.Stages.Any(ss => ss.Id == s.Id)).ToList();
                return Stages.ToList();
            }
        }
        
        private class ZeroFundsHandler :IDataErrorHandler<Act>
        {

            #region Члены IDataErrorHandler<Act>

            public string GetError(Act source, string propertyName, ref bool handled)
            {
         
                if (propertyName == "Totalsum")
                {
                  
                    if (!source.Totalsum.HasValue || source.Totalsum.Value <= 0)
                    {
                        handled = true;
                        return "Сумма по акту должна быть больше нуля";
                    }
                }
                return string.Empty;
            }

            #endregion
        }
        #region Nested type: ActCurrencyRateHandler

        private class ActCurrencyRateHandler : IDataErrorHandler<Act>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Act source, string propertyName, ref bool handled)
            {
                
                if (propertyName == "Currencyrate")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Act source, out bool handled)
            {
                handled = false;
                if (source.Currencyrate == null && source.Currency != null && source.Currency.IsForeign)
                {
                    handled = true;
                    return "Курс валюты должен быть задан.";
                }
                if (source.Currencyrate.HasValue)
                    if (source.Currencyrate.Value <=0)
                    {
                        handled = true;
                        return "Курс валюты не может быть отрицательным.";
                    }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: ActtypeDataErrorHandler

        private class ActtypeDataErrorHandler : IDataErrorHandler<Act>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Act source, string propertyName, ref bool handled)
            {
                if (propertyName == "Acttype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Act source, out bool handled)
            {
                handled = false;
                if (source.Acttype == null)
                {
                    handled = true;
                    return "Тип акта не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: NdsDataErrorHandler

        private class NdsDataErrorHandler : IDataErrorHandler<Act>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Act source, string propertyName, ref bool handled)
            {
                if (propertyName == "Nds")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Act source, out bool handled)
            {
                handled = false;
                if (source.Nds == null)
                {
                    handled = true;
                    return "НДС не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: RegionDataErrorHandler

        private class RegionDataErrorHandler : IDataErrorHandler<Act>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Act source, string propertyName, ref bool handled)
            {
                if (propertyName == "Region")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Act source, out bool handled)
            {
                handled = false;
                if (source.Region == null)
                {
                    handled = true;
                    return "Регион не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: EnterpriseauthorityDataErrorHandler
        private class ActMustHaveDateIfSignedHandler : IDataErrorHandler<Act>
        {
            public string GetError(Act source, string propertyName, ref bool handled)
            {
                
                if (propertyName == "Signdate" && !source.Signdate.HasValue && source.Issigned.GetValueOrDefault())
                {
                    handled = true;
                    return "Дата подписания должна быть указана для подписанного акта";
                }
                return string.Empty;
            }
        }
        private class EnterpriseauthorityDataErrorHandler : IDataErrorHandler<Act>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Act source, string propertyName, ref bool handled)
            {
                if (propertyName == "Enterpriseauthority")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Act source, out bool handled)
            {
                handled = false;
                if (source.Enterpriseauthority == null)
                {
                    handled = true;
                    return "Основание для Промгаза не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion





        public object Clone()
        {
            Contract.Ensures(Contract.Result<object>() != null);
            return new Act
                       {
                           Num = this.Num,
                           Signdate = this.Signdate,
                           Acttype = this.Acttype,
                           Currencyrate = this.Currencyrate,
                           Nds = this.Nds,
                           Enterpriseauthority = this.Enterpriseauthority,
                           Region = this.Region,
                           Status = this.Status,
                           Totalsum = this.Totalsum,
                           Ratedate = this.Ratedate,
                           Sumfortransfer = this.Sumfortransfer
                       };
        }


        partial void OnTotalsumChanged()
        {
            SendPropertyChanged("Sumfortransfer");
            SendMoneyModelChanged();
        }

        partial void OnNdsidChanged()
        {
            SendMoneyModelChanged();
        }
        partial void OnCurrencyidChanged()
        {
            SendMoneyModelChanged();
        }
        partial void OnCurrencymeasureidChanged()
        {
            SendMoneyModelChanged();
        }
        partial void OnSumfortransferChanged()
        {
            SendMoneyModelChanged();
        } 
      

        public bool IsAssignedForContractdoc(Contractdoc contractdoc)
        {
            var sch = Stages.Select(s => s.Schedule).FirstOrDefault();
            if (sch != null)
            {

                var cdocs = sch.Schedulecontracts.Select(cc => cc.Contractdoc); 
                var c = cdocs.FirstOrDefault(cc => cc.Id == contractdoc.Id && !cc.IsAgreement && !contractdoc.IsAgreement) ??
                        sch.Schedulecontracts.Where(ss => ss.IsOtherScheduleAsInOriginalContractdoc)
                        .Select(cc => cc.Contractdoc)
                        .FirstOrDefault(cc => cc.Id == contractdoc.Id && cc.IsAgreement && contractdoc.IsAgreement);
                return (c != null);
            }
            return false;
        }

        public Contractdoc ContractdocActAssignedFor
        {
            get
            {

                var sch = Stages.Select(s => s.Schedule).FirstOrDefault();
                if (sch != null)
                {
                    var cdocs = sch.Schedulecontracts.Select(cc => cc.Contractdoc);

                    foreach (var cc in cdocs)
                    {
                        if (IsAssignedForContractdoc(cc)) return cc;
                    }

                }
                return null;
            }
        }

        private void SendMoneyModelChanged()
        {
            SendPropertyChanged("ActMoney");
            SendPropertyChanged("CreditedMoneyModel");
            SendPropertyChanged("TransferSumMoney");
        }

        private Contractdoc _conteContractdoc;
        public Contractdoc ContractObject
        {
            get { return _conteContractdoc; }
            set
            {
                if (_conteContractdoc == value) return;
                _conteContractdoc = value;
                SendPropertyChanged("ContractObject");
                SendPropertyChanged("ContextContractdoc");
            }
        }


        /// <summary>
        /// Получает или устанавливает аванс. 
        /// Аванс непостредственно не хранится в акте, 
        /// а вычисляется как разница в полной сумме по акту и сумме к перечислению
        /// </summary>
        public decimal Credited
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0);
                return Prepayment();
            }
            set
            {
                if (value != 0)
                {
 
                    if (value > ContractObject.PrepaymentRests + Credited || value < 0)
                        throw new ArgumentException(
                            "Сумма аванса должна быть не меньше нуля и не превышать остатка по перечисленным средствам.");
                }
                var newSumForStransfer = Totalsum - value;
                if (newSumForStransfer == Sumfortransfer) return;
                Sumfortransfer = Totalsum - value;
                SendPropertyChanged("Credited");
                ContractObject.InvalidatePaymentRests();
            }
        }
        partial void OnIssignedChanged()
        {
            SendPropertyChanged("Signedstring");
        }

        private decimal Prepayment()
        {          
            if (Totalsum.HasValue && Sumfortransfer.HasValue)
            {
                return Totalsum.Value - Sumfortransfer.Value;
            }
            return 0;
        }


        public decimal GetTotalSum()
        {
            List<Stage> tempstages = Stages.ToList();
            foreach (Stage stage in Stages)
            {
                if (stage.GeneralStage != null && stage.Act == stage.GeneralStage.Act)
                {
                    tempstages.Remove(stage);
                }
            }

            return tempstages.Sum(x => x.StageMoneyModel.National.Factor.WithNdsValue);
        }

        public decimal GetTotalNds()
        {
            List<Stage> tempstages = Stages.ToList();
            foreach (Stage stage in Stages)
            {
                if (stage.GeneralStage != null && stage.Act == stage.GeneralStage.Act)
                {
                    tempstages.Remove(stage);
                }
            }

            return tempstages.Sum(x => x.StageMoneyModel.National.Factor.NdsValue);
        }

        public MoneyModel NdsMoney
        {
            get
            {
                return CreateMoneyModel(GetTotalNds());
            }
        }

        public void OnChanged(string p)
        {
            SendPropertyChanged(p);
        }

        partial void OnNdsalgorithmidChanged()
        {
            SendMoneyModelChanged();
        }

        //public void AllChanged()
        //{
        //    foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
        //    {
        //        if (propertyInfo.Name != "ContractObject")
        //            OnChanged(propertyInfo.Name);
        //    }
        //}

        /// <summary>
        /// Возвращает наименование акта в виде: №акта /Номер акта/ от /Дата акта/
        /// </summary>
        public string FullName
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Акт");

                if (!string.IsNullOrEmpty(Num))
                {
                    sb.Append(" №");
                    sb.Append(Num);
                }

                if (Signdate.HasValue)
                {
                    sb.Append(" от ");
                    sb.Append(Signdate.Value.ToShortDateString());
                }

                return sb.ToString();

            }
        }
   


        public string Signedstring
        {
            get
            {
                return (Issigned.GetValueOrDefault()) ? "Подписан" : "Не подписан";
            }
        }

        /// <summary>
        /// тип акта может браться из договора или назначаться отдельно для акта
        /// </summary>
        public Acttype Realacttype
        {
            get
            {
                if (Acttype == null || Acttype.WellKnownType == WellKnownActtypes.Undefined)
                {
                    if (ContractObject == null)
                      ContractObject = Stages.Select(s => s.ContractObject).FirstOrDefault();
                        
                    if (ContractObject != null)
                       return ContractObject.Acttype;
                }
                return Acttype;
            }

            set
            {
                Acttype = value;
                SendPropertyChanged("Acttype");
            }
        }

        #region Члены IAct

        #endregion

        public void InvalidateMoney()
        {
            SendPropertyChanged("TransferSumMoney");
            SendPropertyChanged("ActMoney");
        }
    }
}
