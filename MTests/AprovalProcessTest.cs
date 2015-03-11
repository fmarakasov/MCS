using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MCDomain.Model;
namespace MTests
{
    [TestClass]
    public class AprovalProcessTest
    {
        [TestMethod]
        public void TestTimeSpanShoulReturnZero()
        {
            Contractdoc contract = new Contractdoc();
            Approvalprocess aprovalEntry = new Approvalprocess();
            contract.Approvalprocesses.Add(aprovalEntry);
            Assert.AreEqual(new TimeSpan(0), aprovalEntry.PrevStateTimespan);
            Assert.AreEqual(new TimeSpan(0), aprovalEntry.NextStateTimespan);
        }

        [TestMethod]
        public void TestTimeSpanShoulReturnOneDay()
        {
            Contractdoc contract = new Contractdoc();
            
            DateTime today = DateTime.Today;

            Approvalprocess aprovalEntry = new Approvalprocess() {Enterstateat = today};
            contract.Approvalprocesses.Add(aprovalEntry);

            Approvalprocess lastAprovalEntry = new Approvalprocess() {Enterstateat = today.AddDays(1)};
            contract.Approvalprocesses.Add(lastAprovalEntry);

            Assert.AreEqual(new TimeSpan(0), aprovalEntry.PrevStateTimespan);
            Assert.AreEqual(new TimeSpan(1, 0, 0, 0), lastAprovalEntry.PrevStateTimespan);

            Assert.AreEqual(new TimeSpan(1, 0, 0, 0), aprovalEntry.NextStateTimespan);
            Assert.AreEqual(new TimeSpan(0), lastAprovalEntry.NextStateTimespan);
        }
    }
}
