using System;
using System.Diagnostics.Contracts;

namespace MCDomain.Model
{
    public class MoneyValue
    {
        private readonly decimal _price;

        public MoneyValue(decimal price)
        {
            _price = price;
        }

        public decimal Price
        {
            get { return _price; }
        }

        public string National
        {
            get
            {
                return Currency.National.FormatMoney(_price);
            }
        }

        public decimal PriceOrDefault
        {
            get { return _price; }
        }

        public override string ToString()
        {
            return National;
        }
    }

    public class MoneyModel
    {
        private Ndsalgorithm _algorithm;
        private Currency _currency;
        private Currencymeasure _measure;
        private Nds _nds;
        private decimal? _price;
        private decimal? _rate;

        protected MoneyModel()
        {
        }

        public MoneyModel(Ndsalgorithm algorithm, Nds nds, Currency currency, Currencymeasure measure, decimal? price,
                          decimal? rate = null)
        {
            Init(algorithm, nds, currency, measure, price, rate);
        }


        public string PurePrefix { get; set; }
        public string NdsPrefix { get; set; }
        public string WithNdsPrefix { get; set; }
        public string NoInfoMessage { get; set; }

        /// <summary>
        /// Определяет возможность вычисления информации по валюте
        /// </summary>        
        public bool CanCalculatePrices
        {
            get
            {
                return Price.HasValue && Currency != null && Algorithm != null && Measure != null;
            }
        }

        public bool IsForeignOrFactored
        {
            get { return ((CanCalculatePrices && Measure.Factor.HasValue && Measure.Factor.Value != 1) || Currency.IsForeign); }
        }

        /// <summary>
        /// Получает нормализованную величину денег
        /// </summary>
        public MoneyModel Factor
        {
            get
            {
                //return new MoneyModel(Algorithm, Nds, Currency, Measure, Normalize(Price));
                _price = Normalize(Price);
                return this;
            }
        }

        public MoneyModel National
        {
            get { return AsNational(_rate ?? 1); }
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

        /// <summary>
        /// Получает строковое представление валюты с учётом единицы измерения. Функция возвращает одну и туже строку основываясть 
        /// только на свойствах Currency и Mesure, не принимая во внимание вызов свойств Factor и Nationalize
        /// </summary>
        public string FormatCurrencyMeasure
        {
            get
            {
                if (_currency == null || _measure == null) return string.Empty;
                string currencySymbol = _currency.CI.NumberFormat.CurrencySymbol;
                if (_measure.Factor == 1) return currencySymbol;
                return string.Format("{0} {1}", _measure.Name, currencySymbol);
            }
        }

        public MoneyValue ToMoney
        {
            get { return new MoneyValue(Price.GetValueOrDefault()); }
        }

        public Nds Nds
        {
            get { return _nds; }
        }

        public Currency Currency
        {
            get { return _currency; }
        }

        public decimal? Price
        {
            get { return _price; }
        }

        public Currencymeasure Measure
        {
            get { return _measure; }
        }

        public decimal WithNdsValue
        {
            get
            {
                if (CanCalculatePrices)
                    return Algorithm.GetNds(Price.Value, Nds.Fraction) + Algorithm.GetPure(Price.Value, Nds.Fraction);
                return 0.0M;
            }
        }

        public decimal NdsValue
        {
            get
            {
                if (CanCalculatePrices)
                    return Algorithm.GetNds(Price.Value, Nds.Fraction);
                return 0.0M;
            }
        }

        public decimal PureValue
        {
            get
            {
                if (CanCalculatePrices)
                    return Algorithm.GetPure(Price.Value, Nds.Fraction);
                return 0.0M;
            }
        }


        public decimal PriceWithNdsValue
        {
            get
            {
                if (CanCalculatePrices)
                    return PureValue + Algorithm.GetNds(Price.Value, Nds.Fraction);
                return 0M;
            }
        }

        public MoneyValue PriceWithNdsMoneyValue
        {
            get { return new MoneyValue(PriceWithNdsValue); }
        }

        public MoneyValue PureMoneyValue
        {
            get { return new MoneyValue(PureValue); }
        }

        public MoneyValue NdsMoneyValue
        {
            get { return new MoneyValue(NdsValue); }
        }

        /// <summary>
        /// Получает строковое представление числа в валюте системы
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string AsNationalString(decimal value)
        {
            //return string.Format("{0:C}", value);
            return Currency.National.FormatMoney(value);
        }

        private void Init(Ndsalgorithm algorithm, Nds nds, Currency currency, Currencymeasure measure, decimal? price,
                          decimal? rate)
        {
            _algorithm = algorithm;
            _nds = nds;
            _currency = currency;
            _measure = measure;
            _price = price;
            _rate = rate;
        }

        /// <summary>
        /// Создаёт экземпляр класса MoneyViewModel на основе данных договора. Цена и алгоритм вычисления НДС вводятся отдельно так как используются и для цены договора, так и и цены аванса
        /// </summary>
        /// <param name="contractdoc">Договор</param>
        /// <param name="price">Цена</param>
        /// <param name="algorithm">Алгоритм вычисления НДС</param>
        /// <returns>Экземпляр MoneyViewModel</returns>
        public static MoneyModel FromContractdoc(Contractdoc contractdoc, decimal? price, Ndsalgorithm algorithm)
        {
            Contract.Requires(contractdoc != null);
            Contract.Ensures(Contract.Result<MoneyModel>() != null);

            return new MoneyModel(
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
                decimal nds = Algorithm.GetNds(Price.Value, Nds.Fraction);
                // ReSharper restore PossibleInvalidOperationException
                decimal pure = Algorithm.GetPure(Price.Value, Nds.Fraction);

                var pi = new PriceInfo(nds, pure, pure + nds);
                return resolverFunc(pi);
            }
            return NoInfoMessage;
        }

        private decimal Normalize(decimal? value)
        {
            if (Measure != null && Measure.Factor.HasValue && value.HasValue)
                return Measure.Factor.Value*value.Value;
            return 0M;
        }

        private MoneyModel Clone()
        {
            return new MoneyModel(Algorithm, Nds, Currency, Measure, Price);
        }

        public MoneyModel AsNational(decimal? currencyRate)
        {
            Contract.Ensures(Contract.Result<MoneyModel>() != null);
            //var result = new MoneyModel(Algorithm, Nds, Currency.National, Measure, GetRatedPrice(currencyRate));        
            //return result;
            _currency = Currency.National;
            _price = GetRatedPrice(currencyRate);
            return this;
        }

        private decimal? GetRatedPrice(decimal? rate)
        {
            if (CanCalculatePrices && rate.HasValue)
            {
                if (Currency.IsNational) return Price;
                return Price*rate.Value;
            }
            return 0M;
        }

        public override string ToString()
        {
            if (_currency != null)
                return string.Format(_currency.CI, "{0:C}", Price);
            return string.Format("{0:N2}", Price);
        }

        #region Nested type: PriceInfo

        private struct PriceInfo
        {
            public PriceInfo(decimal nds, decimal pure, decimal withNds)
                : this()
            {
                Nds = nds;
                Pure = pure;
                WithNds = withNds;
            }

            public decimal Nds { get; private set; }
            public decimal Pure { get; private set; }
            public decimal WithNds { get; private set; }
        }

        #endregion


        /// <summary>
        /// Пересчитывает цены из одной валюты в другую 
        /// </summary>
        /// <param name="fromCurrency"></param>
        /// <param name="toCurrency"></param>
        /// <param name="amount"></param>
        /// <param name="currencyRate"></param>
        /// <returns></returns>
        public static decimal CurrencyToCurrency(Model.Currency fromCurrency, Model.Currency toCurrency, decimal amount, decimal? currencyRate)
        {
            Contract.Requires(fromCurrency != null);
            Contract.Requires(toCurrency != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(fromCurrency.Culture));
            Contract.Requires(!string.IsNullOrWhiteSpace(toCurrency.Culture));
            if (fromCurrency.Culture.ToUpper() == toCurrency.Culture.ToUpper()) return amount;
            Contract.Assert(currencyRate.HasValue);
            return currencyRate.Value*amount;
        }
    }
}