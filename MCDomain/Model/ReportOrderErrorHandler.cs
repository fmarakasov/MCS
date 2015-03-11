using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    class ReportOrderErrorHandler:IDataErrorHandler<IReportOrderProvider>
    {
        public string GetError(IReportOrderProvider source, string propertyName, ref bool handled)
        {
            try
            {
                if (propertyName == "Reportorder")
                {
                    handled = true;
                    ValueRangeChecker.CheckReportOrderValue(source.Reportorder);
                }
            }
            catch (ArgumentOutOfRangeException e)
            {                
                return e.Message;                
            }
            return string.Empty;
        }
    }
}
