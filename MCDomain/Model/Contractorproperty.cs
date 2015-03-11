using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Contractorpropertiy : ICloneable, IDataErrorInfo, IEditableObject
    {
        public override string ToString()
        {
            return this.Contractor.ToString() + " - " + this.Property.ToString() + " - " + Value.ToString();
        }

        private EditableObject<Contractorpropertiy> _editable;
        private readonly DataErrorHandlers<Contractorpropertiy> _errorHandlers = new DataErrorHandlers<Contractorpropertiy>
                                                                       {
                                                                           new ContractorDataErrorHandler(),
                                                                           new PropertyDataErrorHandler(),
                                                                           new ValueDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Contractorpropertiy>(this);
        }

        public object Clone()
        {
            return new Contractorpropertiy()
            {
                Contractor = this.Contractor,
                Property = this.Property,
                Value = this.Value
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

        #region Nested type: ContractorDataErrorHandler

        private class ContractorDataErrorHandler : IDataErrorHandler<Contractorpropertiy>
        {
            #region IDataErrorHandler<Contractorpropertiy> Members

            public string GetError(Contractorpropertiy source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractor")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractorpropertiy source, out bool handled)
            {
                handled = false;
                if (source.Contractor == null)
                {
                    handled = true;
                    return "Контрагент не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: PropertyDataErrorHandler

        private class PropertyDataErrorHandler : IDataErrorHandler<Contractorpropertiy>
        {
            #region IDataErrorHandler<Contractorpropertiy> Members

            public string GetError(Contractorpropertiy source, string propertyName, ref bool handled)
            {
                if (propertyName == "Property")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractorpropertiy source, out bool handled)
            {
                handled = false;
                if (source.Property == null)
                {
                    handled = true;
                    return "Свойство не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: ValueDataErrorHandler

        private class ValueDataErrorHandler : IDataErrorHandler<Contractorpropertiy>
        {
            #region IDataErrorHandler<Contractorpropertiy> Members

            public string GetError(Contractorpropertiy source, string propertyName, ref bool handled)
            {
                if (propertyName == "Value")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Contractorpropertiy source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Value))
                {
                    handled = true;
                    return "Значение свойства не может быть пустым!";
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
