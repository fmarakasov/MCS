using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Efficienceparametertype: ICloneable, IDataErrorInfo, IEditableObject
    {
        private readonly DataErrorHandlers<Efficienceparametertype> _errorHandlers = new DataErrorHandlers<Efficienceparametertype>
                                                                       {
                                                                           new TypeDataErrorHandler(),
                                                                           new ParameterDataErrorHandler()
                                                                       };

        public override string ToString()
        {
            return this.Economefficiencytype.ToString() + " - " + this.Economefficiencyparameter.ToString();
        }

        public object Clone()
        {
            return new Efficienceparametertype
                       {
                           Economefficiencyparameter = this.Economefficiencyparameter,
                           Economefficiencytype = this.Economefficiencytype
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

        #region Nested type: ParameterDataErrorHandler

        private class ParameterDataErrorHandler : IDataErrorHandler<Efficienceparametertype>
        {

            public string GetError(Efficienceparametertype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Economefficiencyparameter")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Efficienceparametertype source, out bool handled)
            {
                handled = false;
                if (source.Economefficiencyparameter == null)
                {
                    handled = true;
                    return "Параметр не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: TypeDataErrorHandler

        private class TypeDataErrorHandler : IDataErrorHandler<Efficienceparametertype>
        {

            public string GetError(Efficienceparametertype source, string propertyName, ref bool handled)
            {
                if (propertyName == "Economefficiencytype")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Efficienceparametertype source, out bool handled)
            {
                handled = false;
                if (source.Economefficiencytype == null)
                {
                    handled = true;
                    return "Тип не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region IEditableObject members

        private Efficienceparametertype backup = null;
        private bool inTxn = false;

        public void BeginEdit()
        {
            if (!inTxn)
            {
                backup = Clone() as Efficienceparametertype;
                inTxn = true;
            }

        }

        public void CancelEdit()
        {
            if (inTxn)
            {
                this.Economefficiencyparameter = backup.Economefficiencyparameter;
                this.Economefficiencytype = backup.Economefficiencytype;
                inTxn = false;
            }
        }

        public void EndEdit()
        {
            if (inTxn)
            {
                backup = new Efficienceparametertype();
                inTxn = false;
            }

        }

        #endregion
    }
}
