using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Paymentdocument: IDataErrorInfo
    {
        public override string ToString()
        {
            return this.Num + " от " + this.Paymentdate.ToString();
        }
        
        private readonly DataErrorHandlers<Paymentdocument> _errorHandlers = new DataErrorHandlers<Paymentdocument>
                                                                    {
                                                                        new NumDataErrorHandler(),
                                                                        new SumDataErrorHandler()
                                                                    };


        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #region Nested type: NumDataErrorHandler

        private class NumDataErrorHandler : IDataErrorHandler<Paymentdocument>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Paymentdocument source, string propertyName, ref bool handled)
            {
                if (propertyName == "Num")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Paymentdocument source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Num))
                {
                    handled = true;
                    return "Номер платежного документа не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

        #region Nested type: SumDataErrorHandler

        private class SumDataErrorHandler : IDataErrorHandler<Paymentdocument>
        {
            #region IDataErrorHandler<Acttype> Members

            public string GetError(Paymentdocument source, string propertyName, ref bool handled)
            {
                if (propertyName == "Paymentsum")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Paymentdocument source, out bool handled)
            {
                handled = false;
                if (source.Paymentsum == 0)
                {
                    handled = true;
                    return "Сумма выплаты не может быть нулевой!";
                }
                return string.Empty;
            }
        }

        #endregion
    }
}
