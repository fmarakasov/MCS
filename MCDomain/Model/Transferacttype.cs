using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    /// <summary>
    /// Хорошо известные идентификаторы типов актов передачи
    /// </summary>
    public enum WellKnownTransferActtypes:long
    {
        /// <summary>
        /// Акт передачи договоров
        /// </summary>
        TransferContract = 1,
        /// <summary>
        /// Акт передачи актов
        /// </summary>
        TransferAct = 2
    }

    partial class Transferacttype : IObjectId, INamed, ICloneable, IDataErrorInfo, IEditableObject
    {
        private EditableObject<Transferacttype> _editable;
        private readonly DataErrorHandlers<Transferacttype> _errorHandlers = new DataErrorHandlers<Transferacttype>
                                                                       {
                                                                           new NameDataErrorHandler(),
                                                                       };

        public bool IsWellKnownId()
        {
            return Id.Between((long)WellKnownTransferActtypes.TransferAct, (long)WellKnownTransferActtypes.TransferContract);
        }


        partial void OnCreated()
        {
            _editable = new EditableObject<Transferacttype>(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public object Clone()
        {
            return new Transferacttype()
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

        private class NameDataErrorHandler : IDataErrorHandler<Transferacttype>
        {
            #region IDataErrorHandler<Transferacttype> Members

            public string GetError(Transferacttype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Transferacttype source, out bool handled)
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
