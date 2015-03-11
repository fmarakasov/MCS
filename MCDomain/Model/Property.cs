using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Property : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Property> _editable;
        private readonly DataErrorHandlers<Property> _errorHandlers = new DataErrorHandlers<Property>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            return false;
        }


        partial void OnCreated()
        {
            _editable = new EditableObject<Property>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Property()
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

        private class NameDataErrorHandler : IDataErrorHandler<Property>
        {
            #region IDataErrorHandler<Property> Members

            public string GetError(Property source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Property source, out bool handled)
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
