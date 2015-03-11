using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
 
    

    partial class Currency : INamed, IObjectId, IDataErrorInfo, ICloneable, IEditableObject
    {
        private readonly DataErrorHandlers<Currency> _errorHandlers = new DataErrorHandlers<Currency>
                                                                      {
                                                                          new NameDataErrorHandler(),
                                                                          new CultureDataErrorHandler()
                                                                      };

        public bool IsWellKnownId()
        {
            return false;
        }

        
        /// <summary>
        /// Получает национальную валюту
        /// </summary>
        public static Currency National
        {
            get
            {
                if (_nationalCulture == null)
                {
                    _nationalCulture = new Currency(){Culture = NationalCulture};
                }
                return _nationalCulture;
            }
        }
        
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Определяет текущую культуру
        /// </summary>
        public static readonly string NationalCulture = "ru-ru";

        /// <summary>
        /// Представляет 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FormatMoney(decimal value)
        {
            return value.ToString("C", CI);   
        }
        /// <summary>
        /// 
        /// </summary>
        public string CultureOrDefault
        {
            get
            {
                return string.IsNullOrEmpty(Culture) ? NationalCulture : Culture;
            }
        }

        /// <summary>
        /// Определяет описывает ли объект иностранную валюту
        /// </summary>
        public bool IsForeign
        {
            get { return Culture != NationalCulture; }
        }

        

        /// <summary>
        /// Возвращает строковое придставление денег
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="shortCurrency">Определяет требуется ли вывод целой части как строки или как числа</param>
        /// <param name="shortSmall">Определяет требуется ли вывод разменной монеты как строки или как числа</param>
        /// <param name="digitSmall">Определяет нужно ли сокращение</param>
        /// <returns>Строковое представление денег</returns>
        public string MoneyInWords(decimal value, bool shortCurrency, bool shortSmall, bool digitSmall)
        {
            var provider = new CustomizableCurrencyToStringProvider(shortCurrency, shortSmall, digitSmall);
            provider.SetCurrency(WordCase.CaseI,Currencyi);
            provider.SetCurrency(WordCase.CaseR, Currencyr);
            provider.SetCurrency(WordCase.CaseM, Currencym);

            provider.SetSmallCurrency(WordCase.CaseI, Smalli);
            provider.SetSmallCurrency(WordCase.CaseR, Smallr);
            provider.SetSmallCurrency(WordCase.CaseM, Smallm);

            provider.LowSmallName = Lowsmallname;
            provider.HighSmallName = Highsmallname;

            string s = provider.MoneyToString(new Money(value));
            var sb = new StringBuilder(s);
            if (sb.Length > 0)
                sb[0] = Char.ToUpper(sb[0]);
            return sb.ToString();

        }
        partial void OnCultureChanged()
        {
            _ci = null;
            SendPropertyChanged("CI");
        }

        /// <summary>
        /// Определяет является ли данная валюта национальной
        /// </summary>
        public bool IsNational { get { return !IsForeign; } }

        /// <summary>
        /// Получает объект культуры для текущего значения Culture 
        /// </summary>
        public System.Globalization.CultureInfo CI
        {
            get
            {
                if (_ci == null)
                _ci =  new System.Globalization.CultureInfo(CultureOrDefault);
                return _ci;
            }
        }

        public string Error
        {
            get
            {
                return DataErrorInfoValidator.Validate(this);
            }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        public object Clone()
        {
            return new Currency
            {
                Name = this.Name,
                Code = this.Code,
                Culture = this.Culture,
                Lowsmallname = this.Lowsmallname,
                Highsmallname = this.Highsmallname,
                Currencyi = this.Currencyi,
                Currencym = this.Currencym,
                Currencyr = this.Currencyr,
                Smalli = this.Smalli,
                Smallr = this.Smallr,
                Smallm = this.Smallm
            };
        }

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Currency>
        {
            #region IDataErrorHandler<Currency> Members

            public string GetError(Currency source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Currency source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Name))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: CultureDataErrorHandler

        private class CultureDataErrorHandler : IDataErrorHandler<Currency>
        {
            #region IDataErrorHandler<Currency> Members

            public string GetError(Currency source, string propertyName, ref bool handled)
            {
                if (propertyName == "Culture")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Currency source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Culture))
                {
                    handled = true;
                    return "Культура не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region IEditableObject members

        private Currency backup = null;
        private bool inTxn = false;
        private CultureInfo _ci;
        private static Currency _nationalCulture;

        public void BeginEdit()
        {
            if (!inTxn)
            {
                backup = Clone() as Currency;
                inTxn = true;
            }

        }

        public void CancelEdit()
        {
            if (inTxn)
            {
                Name = backup.Name;
                Code = backup.Code;
                Culture = backup.Culture;
                Lowsmallname = backup.Lowsmallname;
                Highsmallname = backup.Highsmallname;
                Currencyi = backup.Currencyi;
                Currencym = backup.Currencym;
                Currencyr = backup.Currencyr;
                Smalli = backup.Smalli;
                Smallr = backup.Smallr;
                Smallm = backup.Smallm;
                inTxn = false;
            }
        }

        public void EndEdit()
        {
            if (inTxn)
            {
                backup = new Currency();
                inTxn = false;
            }

        }

        #endregion

        
        public static Currency FromCulture(string culture)
        {
            return new Currency() {Culture = culture};
        }

        //private static IDictionary<long, Currency> _currencyInstances = new Dictionary<long, Currency>();

        //internal static object FromCurrencyId(long currencyid)
        //{
        //    if (!_currencyInstances.ContainsKey(currencyid))
        //    {
        //        Ndsalgorithm.
        //        _currencyInstances.Add(currencyid, currency);
        //    }
        //    return _currencyInstances[currencyid];
        //}
    }
}