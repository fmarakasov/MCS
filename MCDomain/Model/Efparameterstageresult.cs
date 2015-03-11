using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using MCDomain.DataAccess;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Efparameterstageresult: IDataErrorInfo, ICloneable, IClonableRecursive
    {
        public override string ToString()
        {
            if (this.Economefficiencyparameter != null)
                return this.Economefficiencyparameter.ToString() + " - " + this.Value.ToString();
            else
                return this.Value.ToString();
        }

        private readonly DataErrorHandlers<Efparameterstageresult> _errorHandlers = new DataErrorHandlers<Efparameterstageresult>
                                                                       {
                                                                           new ParameterDataErrorHandler(),
                                                                           new ValueDataErrorHandler()
                                                                       };

        #region ICloneable Members

        public object Clone()
        {
            return new Efparameterstageresult()
            {
                Economefficiencyparameter = this.Economefficiencyparameter,
                Value = this.Value
            };
        }

        public object CloneRecursively(object owner, object source)
        {
            Efparameterstageresult er = new Efparameterstageresult()
            {
                Economefficiencyparameter  = this.Economefficiencyparameter,
                Value = this.Value
            };
            er.Stageresult = (Stageresult)owner;
            return er;
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #endregion

        #region Nested type: ParameterDataErrorHandler

        private class ParameterDataErrorHandler : IDataErrorHandler<Efparameterstageresult>
        {

            public string GetError(Efparameterstageresult source, string propertyName, ref bool handled)
            {
                if (propertyName == "Economefficiencyparameter")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Efparameterstageresult source, out bool handled)
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

        #region Nested type: ValueDataErrorHandler

        private class ValueDataErrorHandler : IDataErrorHandler<Efparameterstageresult>
        {

            public string GetError(Efparameterstageresult source, string propertyName, ref bool handled)
            {
                if (propertyName == "Value")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Efparameterstageresult source, out bool handled)
            {
                handled = false;
                if (source.Value == null)
                {
                    handled = true;
                    return "Значение не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion


        partial void OnCreated()
        {
            Economefficiencyparameterid = EntityBase.ReservedUndifinedOid;
        }

    }
}
