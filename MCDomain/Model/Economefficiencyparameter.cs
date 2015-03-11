using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет известные идентификаторы параметров экономической эффективности
    /// </summary>
    public enum WellKnownEconomicEfficiencyParameters
    {
        /// <summary>
        /// Не определён
        /// </summary>
        [Description("Не определено")]
        Undefined = -1,
        /// <summary>
        /// НТР-У (ККач)
        /// </summary>
        [Description("НТР-У(Ккач)")]
        NTRU_kach = 10002,
        /// <summary>
        /// НТР-У (Крез)
        /// </summary>
        [Description("НТР-У(Крез)")]
        NTRU_res = 10003,
        /// <summary>
        /// ИЭр
        /// </summary>
        [Description("ИЭр")]
        EconEff = 10020,
        /// <summary>
        /// ЭИ в млн.руб.
        /// </summary>
        [Description("ЭИ в млн.руб.")]
        EconEffMln = 10021,
        /// <summary>
        /// Период
        /// </summary>
        [Description("Период")]
        Period = 10022
    }

    partial class Economefficiencyparameter : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject, IComparable
    {
        private readonly DataErrorHandlers<Economefficiencyparameter> _errorHandlers = new DataErrorHandlers<Economefficiencyparameter>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownEconomicEfficiencyParameters));
            return en.Cast<object>().Any(ch => (WellKnownEconomicEfficiencyParameters) Id == (WellKnownEconomicEfficiencyParameters) ch);
        }

        /// <summary>
        /// Получает известный тип параметра экономической эффективности
        /// </summary>
        public WellKnownEconomicEfficiencyParameters WellKnownType
        {
            get
            {
                
                if (IsWellKnownId())
                    return (WellKnownEconomicEfficiencyParameters)Id;
                return WellKnownEconomicEfficiencyParameters.Undefined;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Economefficiencyparameter
            {
                Name = Name
            };
        }

        public string Error
        {
            get
            {
                return this.Validate();
            }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #region Nested type: NameDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Economefficiencyparameter>
        {
            #region IDataErrorHandler<Economefficiencyparameter> Members

            public string GetError(Economefficiencyparameter source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Economefficiencyparameter source, out bool handled)
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

        #region IEditableObject members

        private Economefficiencyparameter _backup;
        private bool _inTxn;

        public void BeginEdit()
        {
            if (_inTxn) return;
            _backup = Clone() as Economefficiencyparameter;
            _inTxn = true;
        }

        public void CancelEdit()
        {
            if (!_inTxn) return;
            Name = _backup.Name;
            _inTxn = false;
        }

        public void EndEdit()
        {
            if (!_inTxn) return;
            _backup = new Economefficiencyparameter();
            _inTxn = false;
        }

        #endregion

        public int CompareTo(object obj)
        {
            var economefficiencyparameter = obj as Economefficiencyparameter;
            Debug.Assert(economefficiencyparameter != null, "economefficiencyparameter != null");
            return String.Compare(Name, economefficiencyparameter.Name, System.StringComparison.Ordinal);
        }
    }
}
