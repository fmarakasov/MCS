using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    partial class Document : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Document> _editable;
        private readonly DataErrorHandlers<Document> _errorHandlers = new DataErrorHandlers<Document>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            return false;
        }

        partial void OnCreated()
        {
            _editable = new EditableObject<Document>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Document()
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

        private class NameDataErrorHandler : IDataErrorHandler<Document>
        {
            #region IDataErrorHandler<Document> Members

            public string GetError(Document source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Document source, out bool handled)
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
