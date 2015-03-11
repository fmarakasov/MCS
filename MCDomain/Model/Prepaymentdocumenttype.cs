using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Определяет известные типы платёжных документов
    /// </summary>
    public enum WellKnownPaymentTypes
    {
        /// <summary>
        /// Не определён
        /// </summary>
        [Description("Не определён")]            
        Undefined = -1
        
    }

    partial class Prepaymentdocumenttype : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Prepaymentdocumenttype> _editable;
        private readonly DataErrorHandlers<Prepaymentdocumenttype> _errorHandlers = new DataErrorHandlers<Prepaymentdocumenttype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownPaymentTypes));
            foreach (var ch in en)
            {
                if ((WellKnownPaymentTypes)Id == (WellKnownPaymentTypes)ch) return true;
            }
            return false;
        }

        /// <summary>
        /// Получает известный тип контрагента
        /// </summary>
        public WellKnownPaymentTypes WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownPaymentTypes)Id;
                return WellKnownPaymentTypes.Undefined;
            }
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Prepaymentdocumenttype>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Prepaymentdocumenttype()
            {
                Name = this.Name
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

        private class NameDataErrorHandler : IDataErrorHandler<Prepaymentdocumenttype>
        {
            #region IDataErrorHandler<Prepaymentdocumenttype> Members

            public string GetError(Prepaymentdocumenttype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Prepaymentdocumenttype source, out bool handled)
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
    }
}
