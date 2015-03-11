using System.Linq;
using MCDomain.DataAccess;
using MContracts.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    [TestClass]
    public class ActReportTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DeploymentItem("Templates\\act.dot", "Templates")]
        public void ActReportCreateTest()
        {
            using (var ctx = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var act = ctx.TryGetContext().Acts.First();
                var contract = act.Stages.First().ContractObject;
                ReportCreatorHelper.CreateActReport(act, contract,
                                                    ReportCreatorHelper.ActReportTemplate.GazpromNiokr);
            }

        }
    }
}
