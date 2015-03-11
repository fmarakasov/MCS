using McReports.Common;
using McReports.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McReportsTests
{
    class StubTemplateProvider : ITemplateProvider
    {
        public string GetTemplate(string propertyName)
        {
            return string.Empty;
        }
    }
}
