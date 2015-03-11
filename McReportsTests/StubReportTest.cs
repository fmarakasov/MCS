using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using McUIBase.Factories;
using McReports.ViewModel;
using MCDomain.DataAccess;

namespace McReportsTests
{
    
    [TestClass]
    public class StubReportTest
    {
        [TestMethod]
        public void StubReportTest01()
        {
            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider();
            var report = new StubReports.StubReport(RepositoryFactory.CreateContractRepository());
            report.TemplateProvider = new StubTemplateProvider();
            report.SetReport();            
        }
    }
}
