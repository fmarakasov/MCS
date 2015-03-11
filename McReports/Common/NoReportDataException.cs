using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McReports.Common
{
    
    public class NoReportDataException: ApplicationException
    {
        private static string DefaultMessage =
            "\nОтчет не будет сформирован, поскольку ни один из договоров, выбранных в фильтре, не содержит параметров, необходимых для формирования отчета.";
        
        public NoReportDataException(string message) : base(message) { }

        public NoReportDataException() : base(DefaultMessage) { }
    }
}
