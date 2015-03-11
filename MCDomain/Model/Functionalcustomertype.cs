using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Functionalcustomertype : ICloneable, IDataErrorInfo, IEditableObject
    {
        public override string ToString()
        {
            return Name;
        }

        private EditableObject<Functionalcustomertype> _editable;
        private readonly DataErrorHandlers<Functionalcustomertype> _errorHandlers = new DataErrorHandlers<Functionalcustomertype>
                                                                                       {
                                                                                            new NameDataErrorHandler()
                                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Functionalcustomertype>(this);
        }

        public object Clone()
        {
            return new Functionalcustomertype()
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

        private class NameDataErrorHandler : IDataErrorHandler<Functionalcustomertype>
        {
            #region IDataErrorHandler<Functionalcustomertype> Members

            public string GetError(Functionalcustomertype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Functionalcustomertype source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Name))
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
