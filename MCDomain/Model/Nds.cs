using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{


    partial  class Nds : IObjectId, ICloneable, IDataErrorInfo, IEditableObject
    {
        private readonly DataErrorHandlers<Nds> _errorHandlers = new DataErrorHandlers<Nds>
                                                                       {
                                                                           new PercentDataErrorHandler(),
                                                                           new YearDataErrorHandler()
                                                                       };


        public bool IsWellKnownId()
        {
            return false;
        }

        /// <summary>
        /// Получает или устанавливает долю НДС. Изменение значения этого свойства изменяет значение Percent
        /// </summary>
        public double Fraction
        {
            [Pure]
            get
            {   
                //Contract.Ensures(Contract.Result<double>().Between(0.0, 1.0));
                Contract.Assert(Percents.Between(0.0, 100.0));
                return Percents / 100;
            }
            [Pure]
            set
            {                
                //Contract.Ensures(Percents.Between(0.0, 100.0));
                Contract.Assert(value.Between(0.0, 1.0));
                SendPropertyChanging("Fraction");
                Percents = value * 100;
                SendPropertyChanged("Fraction");
            }
        }
        partial void OnPercentsChanging(double value)
        {
            SendPropertyChanging("Fraction");
        }

        partial void OnPercentsChanged()
        {
            SendPropertyChanged("Fraction");
        }
        public override string ToString()
        {
            return Fraction.ToString("P");
        }

        public static Nds FromValue(double ndsValue)
        {
            return new Nds() {Percents = ndsValue};
        }

        public object Clone()
        {
            return new Nds
                       {
                           Percents = this.Percents,
                           Year = this.Year
                       };
        }

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

        #region Nested type: PercentDataErrorHandler

        private class PercentDataErrorHandler : IDataErrorHandler<Nds>
        {
            #region IDataErrorHandler<Contractstate> Members

            public string GetError(Nds source, string propertyName, ref bool handled)
            {
                if (propertyName == "Percents")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Nds source, out bool handled)
            {
                handled = false;
                if (source.Percents <= 0)
                {
                    handled = true;
                    return "Процент НДС должен быть положительным числом!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: YearDataErrorHandler

        private class YearDataErrorHandler : IDataErrorHandler<Nds>
        {
            #region IDataErrorHandler<Nds> Members

            public string GetError(Nds source, string propertyName, ref bool handled)
            {
                if (propertyName == "Year")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Nds source, out bool handled)
            {
                handled = false;
                if (source.Year <= 0 || source.Year > DateTime.Now.Year)
                {
                    handled = true;
                    return "Год должен быть положительным числом и не превышать текущего года!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region IEditableObject members

        private Nds backup = null;
        private bool inTxn = false;

        public void BeginEdit()
        {
            if (!inTxn)
            {
                backup = Clone() as Nds;
                inTxn = true;
            }

        }

        public void CancelEdit()
        {
            if (inTxn)
            {
                this.Percents = backup.Percents;
                this.Year = backup.Year;
                inTxn = false;
            }
        }

        public void EndEdit()
        {
            if (inTxn)
            {
                backup = new Nds();
                inTxn = false;
            }

        }

        #endregion
    }
}
