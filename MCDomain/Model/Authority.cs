using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Authority : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        private EditableObject<Authority> _editable;
        private readonly DataErrorHandlers<Authority> _errorHandlers = new DataErrorHandlers<Authority>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Authority>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Authority()
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

        private class NameDataErrorHandler : IDataErrorHandler<Authority>
        {
            #region IDataErrorHandler<Authority> Members

            public string GetError(Authority source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Authority source, out bool handled)
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
