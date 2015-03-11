using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Position : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Position> _editable;
        private readonly DataErrorHandlers<Position> _errorHandlers = new DataErrorHandlers<Position>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Position>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Position()
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

        private class NameDataErrorHandler : IDataErrorHandler<Position>
        {
            #region IDataErrorHandler<Position> Members

            public string GetError(Position source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Position source, out bool handled)
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
