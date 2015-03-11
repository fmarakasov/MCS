using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts;
using MContracts.Classes;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class YearTemplanReportTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestStageConditions()
        {

            IContractRepository repo = LinqContractRepositoryTest.CreateLinqContractRepository();
            var st = repo.AllStages.FirstOrDefault(s => s.Id == 10597);

            DateTime sdt, fdt;
            DateTime.TryParse("01.10.2011", out sdt);
            DateTime.TryParse("31.12.2011", out fdt);

            if (st != null)
            {
                var state = st.GetConditionForPeriod(sdt, fdt);
                Assert.AreNotEqual(state, StageCondition.Closed);

                var price = st.PriceOnPeriodWithNDS(sdt, fdt);
                Assert.AreNotEqual(price, 0);

                price = st.SubContractsStages.Sum(s=>s.PriceOnPeriodWithNDS(sdt, fdt));
                Assert.AreNotEqual(price, 0);
            }
        }
   

        [TestMethod]
        [DeploymentItem("Templates\\1_YearPlanReport.xlt", "Templates")]
        public void YearTemplanReportCreateTest()
        {

            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider("UD", "sys", "XE");

            
            var mvm = new MainWindowViewModel();
            
                DateTime sdt, fdt;
            DateTime.TryParse("01.01.2011", out sdt);
            DateTime.TryParse("31.12.2011", out fdt);

            mvm.SelectedFilterStartDate = sdt;
            mvm.SelectedFilterEndDate = fdt;
            mvm.Workspaces.Add(
                new ContractRepositoryViewBasedViewModel(LinqContractRepositoryTest.CreateLinqContractRepository()));
            mvm.SetActiveWorkspace(mvm.Workspaces[0]);

            ReportCreatorHelper.CreateYearTemplanReport(mvm);
            


        }
    }
    
}
