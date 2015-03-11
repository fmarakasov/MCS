using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Responsibleassignmentorder: IObjectId, IDataErrorInfo
    {
        private readonly DataErrorHandlers<Responsibleassignmentorder> _errorHandlers = new DataErrorHandlers<Responsibleassignmentorder>
                                                                       {
                                                                           new NumberDataErrorHandler(),
                                                                       };


        public bool IsWellKnownId()
        {
            return false;
        }


        #region IDataErrorInfo Members

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #region Nested type: NameDataErrorHandler

        private class NumberDataErrorHandler: IDataErrorHandler<Responsibleassignmentorder>
        {

            public string GetError(Responsibleassignmentorder source, string propertyName, ref bool handled)
            {
                if (propertyName == "Ordernum")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(Responsibleassignmentorder source, out bool handled)
            {
                handled = false;
                if (string.IsNullOrEmpty(source.Ordernum))
                {
                    handled = true;
                    return "Номер приказа не может быть пустым!";
                }

                
                handled = true;
                return string.Empty;
            }
        }
        #endregion
        #endregion


        partial void OnCreated()
        {
            Orderdate = DateTime.Today;
        }
    }
}
