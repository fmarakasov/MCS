using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    partial class Ntpview : IObjectId, INamed, IDataErrorInfo, IEditableObject, IComparable, ICloneable
    {
        private EditableObject<Ntpview> _editable;
        private readonly DataErrorHandlers<Ntpview> _errorHandlers = new DataErrorHandlers<Ntpview>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };


        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Ntpview>(this);
        }

        public override string ToString()
        {
            return Name;
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

        private class NameDataErrorHandler : IDataErrorHandler<Ntpview>
        {
            #region IDataErrorHandler<Ntpview> Members

            public string GetError(Ntpview source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Ntpview source, out bool handled)
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

        public int CompareTo(object obj)
        {
            return Id.CompareTo((obj as Ntpview).Id);
        }

        public object Clone()
        {
            return new Ntpview
                       {
                           Name = this.Name
                       };
        }
    }
}
