using System;
using System.Diagnostics.Contracts;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    public class MoneyViewModel
    {
        
        private Ndsalgorithm _algorithm;
        private Nds _nds;
        private Currency _currency;
        private double? _price;
        private Currencymeasure _measure;


        struct PriceInfo
        {
            public PriceInfo(double nds, double pure, double withNds)
                : this()
            {
                Nds = nds;
                Pure = pure;
                WithNds = withNds;
            }

            public double Nds { get; private set; }
            public double Pure { get; private set; }
            public double WithNds { get; private set; }
        }

        public string PurePrefix { get; set; }
        public string NdsPrefix { get; set; }
        public string WithNdsPrefix { get; set; }
        public string NoInfoMessage { get; set; }

        protected MoneyViewModel()
        {

        }


        public MoneyViewModel(Ndsalgorithm  algorithm, Nds nds, Currency currency, Currencymeasure measure, double? price)
        {
            Init(algorithm, nds, currency, measure, price);
        }

        private void Init(Ndsalgorithm algorithm, Nds nds, Currency currency, Currencymeasure measure, double? price)
        {
            _algorithm = algorithm;
            _nds = nds;
            _currency = currency;
            _measure = measure;
            _price = price;
        }

        

        /// <summary>
        /// Создаёт экземпляр класса MoneyViewModel на основе данных договора. Цена и алгоритм вычисления НДС вводятся отдельно так как используются и для цены договора, так и и цены аванса
        /// </summary>
        /// <param name="contractdoc">Договор</param>
        /// <param name="price">Цена</param>
        /// <param name="algorithm">Алгоритм вычисления НДС</param>
        /// <returns>Экземпляр MoneyViewModel</returns>
        public static MoneyViewModel FromContractdoc(Contractdoc contractdoc, double? price, Ndsalgorithm algorithm)
        {
            Contract.Requires(contractdoc != null);
            Contract.Ensures(Contract.Result<MoneyViewModel>() != null);

            return new MoneyViewModel(
                algorithm,
                contractdoc.Nds,
                contractdoc.Currency,
                contractdoc.Currencymeasure,
                price);
        }

        private string CalculatePriceInfo(Func<PriceInfo, string> resolverFunc)
        {
            Contract.Requires(resolverFunc != null);

            if (CanCalculatePrices)
            {
                // ReSharper disable PossibleInvalidOperationException
                double nds = Algorithm.GetNds(Price.Value, Nds.Fraction);
                // ReSharper restore PossibleInvalidOperationException
                double pure = Algorithm.GetPure(Price.Value, Nds.Fraction);

                PriceInfo pi = new PriceInfo(nds, pure, pure + nds);
                return resolverFunc(pi);

            }
            return NoInfoMessage;
        }

        /// <summary>
        /// Определяет возможность вычисления информации по валюте
        /// </summary>        
        public bool CanCalculatePrices
        {
            get
            {
                return Price.HasValue && Currency != null && Algorithm != null && Measure != null && Price.Value != double.MinValue;                
            }
        }

        public bool IsForeignOrFactored
        {
            get { return (CanCalculatePrices && Measure.Factor.HasValue && Measure.Factor.Value != 1 && Currency.IsForeign); }
        }

        private double Normalize(double? value)
        {
            if (Measure!=null && Measure.Factor.HasValue && value.HasValue)
                return Measure.Factor.Value*value.Value;
            return double.MinValue;
        }

        private MoneyViewModel Clone()
        {
            return new MoneyViewModel(Algorithm, Nds, Currency, Measure, Price);    
        }

        /// <summary>
        /// Получает нормализованную величину денег
        /// </summary>
        public MoneyViewModel Factor
        {
            get
            {
                return new MoneyViewModel(Algorithm, Nds, Currency, Measure, Normalize(Price));
            }
        }

        public MoneyViewModel AsNational(double? currencyRate)
        {
            Contract.Ensures(Contract.Result<MoneyViewModel>() != null);
            var result = new MoneyViewModel(Algorithm, Nds, Currency.National, Measure, GetRatedPrice(currencyRate));        
            return result;
        }

        public string PriceWithNds
        {
            get { return WithNdsPrefix + CalculatePriceInfo((x) => Currency.FormatMoney(x.WithNds)); }
        }

        public string PriceNds
        {
            get { return NdsPrefix + CalculatePriceInfo((x) => Currency.FormatMoney(x.Nds)); }
        }

        public string PricePure
        {
            get { return PurePrefix + CalculatePriceInfo((x) => Currency.FormatMoney(x.Pure)); }
        }

        public Ndsalgorithm Algorithm
        {
            get { return _algorithm; }
            
        }

        public Nds Nds
        {
            get { return _nds; }
            
        }

        public Currency Currency
        {
            get { return _currency; }
            
        }

        public double? Price
        {
            get { return _price; }
            
        }

        public Currencymeasure Measure
        {
            get { return _measure; }
            
        }

        public double WithNdsValue
        {
            get
            {
                if (CanCalculatePrices)
                    return Algorithm.GetNds(Price.Value, Nds.Fraction) + Algorithm.GetPure(Price.Value, Nds.Fraction);
                return 0.0;
            }
        }

        public double PureValue
        {
            get
            {
                if (CanCalculatePrices)
                    return Algorithm.GetPure(Price.Value, Nds.Fraction);
                return 0.0;
            }
        }

        private double? GetRatedPrice(double? rate)
        {
            if (CanCalculatePrices && rate.HasValue)
            {
                if (Currency.IsNational) return Price;
                return Price * rate.Value;
            }
            return double.MinValue;
        }    
    }
}
