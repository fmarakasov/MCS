using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Transferacttypedocument : ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Transferacttypedocument> _editable;
        private readonly DataErrorHandlers<Transferacttypedocument> _errorHandlers = new DataErrorHandlers<Transferacttypedocument>
                                                                                           {
                                                                                               new TransferacttypeDataErrorHandler(),
                                                                                               new DocumentDataErrorHandler()
                                                                                           };

        partial void OnCreated()
        {
            _editable = new EditableObject<Transferacttypedocument>(this);
        }

        public override string ToString()
        {
            return this.Document.ToString() + " - " + this.Transferacttype.ToString();
        }

        public object Clone()
        {
            return new Transferacttypedocument()
            {
                Document = this.Document,
                Transferacttype = this.Transferacttype
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

        #region Nested type: DocumentDataErrorHandler

        private class DocumentDataErrorHandler : IDataErrorHandler<Transferacttypedocument>
        {
            #region IDataErrorHandler<Transferacttypedocument> Members

            public string GetError(Transferacttypedocument source, string propertyName, ref bool handled)
            {
                if (propertyName == "Document")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Transferacttypedocument source, out bool handled)
            {
                handled = false;
                if (source.Document == null)
                {
                    handled = true;
                    return "Ссылка на документ не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: TransferacttypeDataErrorHandler

        private class TransferacttypeDataErrorHandler : IDataErrorHandler<Transferacttypedocument>
        {
            #region IDataErrorHandler<Transferacttypedocument> Members

            public string GetError(Transferacttypedocument source, string propertyName, ref bool handled)
            {
                if (propertyName == "Transferacttype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Transferacttypedocument source, out bool handled)
            {
                handled = false;
                if (source.Transferacttype == null)
                {
                    handled = true;
                    return "Ссылка на тип акта не может быть пустой!";
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
