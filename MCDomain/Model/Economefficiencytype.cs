using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{

    public enum  WellKnownEconomefficiencytypes
    {
      [Description ("Не определен")]
      Undefined = -1,
      [Description("Ресурсный")]
      Resource = 10000,
      [Description("Экономический")]
      Economic = 10001,
      [Description("Управленческий")]
      Managenent = 10002,
      [Description("Поисковый")]
      Research = 10020
    }

    partial class Economefficiencytype : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject, IComparable
    {
        private readonly DataErrorHandlers<Economefficiencytype> _errorHandlers = new DataErrorHandlers<Economefficiencytype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };



        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownEconomefficiencytypes));
            foreach (var ch in en)
            {
                if ((WellKnownEconomefficiencytypes)Id == (WellKnownEconomefficiencytypes)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип экономической эффективности
        /// </summary>
        public WellKnownEconomefficiencytypes WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownEconomefficiencytypes)Id;
                return WellKnownEconomefficiencytypes.Undefined;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Economefficiencytype
                       {
                           Name = this.Name
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

        private class NameDataErrorHandler : IDataErrorHandler<Economefficiencytype>
        {
            #region IDataErrorHandler<Economefficiencytype> Members

            public string GetError(Economefficiencytype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Economefficiencytype source, out bool handled)
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

        private Economefficiencytype backup = null;
        private bool inTxn = false;
        
        public void BeginEdit()
        {
            if (!inTxn)
            {
                backup = Clone() as Economefficiencytype;
                inTxn = true;
            }

        }

        public void CancelEdit()
        {
            if (inTxn)
            {
                this.Name = backup.Name;
                inTxn = false;
            }
        }

        public void EndEdit()
        {
            if (inTxn)
            {
                backup = new Economefficiencytype();
                inTxn = false;
            }

        }

        #endregion

        public int CompareTo(object obj)
        {
            return Name.CompareTo((obj as Economefficiencytype).Name);
        }
    }
}
