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

    public enum  WellKnownCurrencyMeasures
    {
        [Description("Не определено")]
        Undefined = -1,
        [Description("Ед.")]
        Units = 1,
        [Description("Тыс.")]
        Thousands = 2,
        [Description("Млн.")]
        Millions = 3
        
    }

    partial class Currencymeasure : IObjectId, IDataErrorInfo, ICloneable, IEditableObject
    {


        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownCurrencyMeasures));
            foreach (var ch in en)
            {
                if ((WellKnownCurrencyMeasures)Id == (WellKnownCurrencyMeasures)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип единицы измерения
        /// </summary>
        public WellKnownCurrencyMeasures WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownCurrencyMeasures)Id;
                return WellKnownCurrencyMeasures.Undefined;
            }
        }

        public override string ToString()
        {
              return Name;            
        }
        /// <summary>
        /// Возращаяет нормализованную цену, умножая параметр на значение свойства Factor
        /// </summary>
        /// <param name="price">Ненормализованная цена</param>
        /// <returns>Нормализованная цена</returns>
        public double Normalize(double price)
        {
            Contract.Requires(Factor.HasValue);
            return price*Factor.Value;
        }


         
        private EditableObject<Currencymeasure> _editable;
        private readonly DataErrorHandlers<Currencymeasure> _errorHandlers = new DataErrorHandlers<Currencymeasure>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Currencymeasure>(this);
        }

        public object Clone()
        {
            return new Currencymeasure()
            {
                Name = this.Name,
                Factor = this.Factor
            };
        }

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Currencymeasure>
        {
            #region IDataErrorHandler<Currencymeasure> Members

            public string GetError(Currencymeasure source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Currencymeasure source, out bool handled)
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

        public void BeginEdit()
        {
            _editable.BeginEdit();
        }

        public void CancelEdit()
        {
            _editable.CancelEdit();
        }

        public void EndEdit()
        {
            _editable.EndEdit();

        }

        public static Currencymeasure FormValue(long? factor)
        {
            return new Currencymeasure() {Factor = factor};
        }

        public string CurrencyMeasureFormat
        {
            get
            {
                if (Factor == 1) return "{0}";
                else return Name + " {0}"; 
            }
        }

    }

}
