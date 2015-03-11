using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    partial class Worktype : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private readonly DataErrorHandlers<Worktype> _errorHandlers = new DataErrorHandlers<Worktype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };
        

        public bool IsWellKnownId()
        {
            return false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(Shortname))
                stringBuilder.Append(string.Format("{0} \u2014 ", Shortname));
            stringBuilder.Append(Name);
            return stringBuilder.ToString();
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Worktype>(this);
        }

        public object Clone()
        {
            return new Worktype
                       {
                           Name = this.Name,
                           Shortname = this.Shortname
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

        private class NameDataErrorHandler : IDataErrorHandler<Worktype>
        {
            #region IDataErrorHandler<Worktype> Members

            public string GetError(Worktype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Worktype source, out bool handled)
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

        private EditableObject<Worktype> _editable;

        #region IEditableObject members

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

        #endregion
    }
}
