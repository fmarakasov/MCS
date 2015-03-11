using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Funccustomerperson: ICloneable, IDataErrorInfo, IEditableObject
    {
        public override string ToString()
        {
            return Functionalcustomer.ToString() + " - " + Person.ToString();
        }

        private EditableObject<Funccustomerperson> _editable;
        private readonly DataErrorHandlers<Funccustomerperson> _errorHandlers = new DataErrorHandlers<Funccustomerperson>
                                                                                       {
                                                                                           new FunctionalcustomerDataErrorHandler()
                                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Funccustomerperson>(this);
        }

        public object Clone()
        {
            return new Funccustomerperson()
            {
                Functionalcustomer = this.Functionalcustomer,
                Person = this.Person
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

        private class FunctionalcustomerDataErrorHandler : IDataErrorHandler<Funccustomerperson>
        {
            #region IDataErrorHandler<Funccustomerperson> Members

            public string GetError(Funccustomerperson source, string propertyName, ref bool handled)
            {
                if (propertyName == "Functionalcustomer")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Funccustomerperson source, out bool handled)
            {
                handled = false;
                if (source.Functionalcustomer == null)
                {
                    handled = true;
                    return "Функциональный заказчик не может быть пустым!";
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
