using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{    
    partial class Prepayment : IObjectId, ISupportMoneyModel
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        private Contractdoc _currentContractdoc;
        public Contractdoc CurrenctContract
        {
            get { return _currentContractdoc; }
            set
            {
                if (_currentContractdoc == value) return;
                _currentContractdoc = value;
                SendPropertyChanged("CurrenctContract");
                InvalidateMoneyProperties();
            }

        }

        private void InvalidateMoneyProperties()
        {
            SendPropertyChanged("AutoPercent");
            SendPropertyChanged("PriceWithNds");
            SendPropertyChanged("PriceNds");
            SendPropertyChanged("PricePure");
            SendPropertyChanged("PrepaymentMoneyModel");
            SendPropertyChanged("Rest");
            CheckPrepaymentsSum();
        }

        
        public decimal PriceWithNds
        {
            get
            {
                return CalcSum((x,y)=>x+y);
            }
        }

        private decimal CalcSum(Func<decimal, decimal, decimal> func)
        {
            if (!IsNdsalgorithmSpecified()) return 0.0M;
            return func(GetContractObj().Prepaymentndsalgorithm.GetNds(Sum, GetContractObj().Nds.Fraction),
                        GetContractObj().Prepaymentndsalgorithm.GetPure(Sum, GetContractObj().Nds.Fraction));
        }

        private bool IsNdsalgorithmSpecified()
        {
            return GetContractObj() != null && GetContractObj().Prepaymentndsalgorithm != null && GetContractObj().Nds != null;
        }

        public decimal PriceNds
        {
            get
            {
                return CalcSum((x, y) => x);
            }
        }

        public decimal PricePure
        {
            get
            {
                return CalcSum((x, y) => y);
            }
        }
        
        
        public float AutoPercent
        {
            get
            {
                Contractdoc contractObj = GetContractObj();

                if ((contractObj != null) && (contractObj.Price.HasValue))
                    return Percent.GetPercent(Sum, contractObj.ContractMoney.Factor.PriceWithNdsValue);
                return 0.0F;
            }
        }

        private Contractdoc GetContractObj()
        {
            return Contractdoc ?? CurrenctContract;
        }

        partial void OnSumChanging(decimal value)
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("value",@"Сумма аванса не может быть отрицательной.");                        
        }

        /// <summary>
        /// Rule: сумма авансов по годам не может превышать сумму аванса
        /// </summary>
        partial void OnIdChanged()
        {
            InvalidateAutoProperties();
        }

        private void CheckPrepaymentsSum()
        {
            //if (GetContractObj() != null)
            //{
            //    if (GetContractObj().Prepaymentsum.HasValue)
            //    {
            //        if (GetContractObj().TotalPrepayments > GetContractObj().Prepaymentsum.Value)
            //            throw new ArgumentOutOfRangeException("Сумма авансов по годам превышает стоимость авансов!");
            //    }
            //}
        }

        partial void OnYearChanging(int value)
        {
            if (value < 1960) throw new ArgumentOutOfRangeException("value",@"Год авансирования не может быть меньше 1960");
            if (GetContractObj() != null && GetContractObj().Startat.HasValue && GetContractObj().Endsat.HasValue)
            {
                CheckYearRange(value);
                if (GetContractObj().Prepayments.Any(x=>x.Year == value)) throw new ArgumentException("Для указанного года уже задан аванс.");
            }
        }

        private void CheckYearRange(int value)
        {
            if (!value.Between(GetContractObj().Startat.Value.Year, GetContractObj().Endsat.Value.Year))
                throw new ArgumentOutOfRangeException("value",
                                                      string.Format(
                                                          "Год авансирования должен быть между {0} и {1}",
                                                          GetContractObj().Startat.Value.Year,
                                                          GetContractObj().Endsat.Value.Year));
        }

        partial void OnSumChanged()
        {
            InvalidateAutoProperties();
            
        }

        partial void OnContractdocidChanged()
        {
            InvalidateAutoProperties();
        }

        /// <summary>
        /// Объявляет процент аванса изменённым
        /// </summary>
        public void InvalidateAutoProperties()
        {
            InvalidateMoneyProperties();
            
        }

        public MoneyModel this[string moneySubject]
        {
            get { return null; }
        }

        /// <summary>
        /// Получает остаток авансовых средств по этому году
        /// </summary>
        public decimal Rest
        {
            get
            {
                Contract.Ensures(Contract.Result<decimal>() >= 0, "Остаток по авансу не может быть отрицательным.");
                // Получить этапы календарного плана с группировкой по годам
                var groupedStages = YearGroupedStages;
                try
                {
                    // Получить этапы на год аванса
                    var thisYearStages = groupedStages.Single(x => x.Key == Year);
                    
                    // Получить остаток по авансу на текущий год
                    return Sum  - thisYearStages.Select(x => x.Act).Distinct().Sum(x => x.Credited);
                    
                }
                // Аванс на текущий год не указан
                catch (InvalidOperationException)
                {
                    return 0;
                }

            }
        }

        private IEnumerable<IGrouping<int, Stage>> YearGroupedStages
        {
            get { return Contractdoc.Stages.GroupBy(x=>x.Endsat.GetValueOrDefault().Year); }
        }
    }

    public class NullPrepayment:Prepayment, INull
    {
        private static NullPrepayment _instance;

        public static NullPrepayment Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NullPrepayment() {Sum = 0, Percentvalue = 0};

                }
                return _instance;
            }
        }

        public bool IsNull
        {
            get { return true; }
        }
    }

}
