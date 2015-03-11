using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class StubContractStateData : IContractStateData
    {
        public long Id { get; set; }

        public DateTime? Appliedat { get; set; }

        public DateTime? Approvedat { get; set; }

        public DateTime? Startat { get; set; }

        public DateTime? Endsat { get; set; }

        public bool IsLastVersion { get; set; }

        public DateTime? Brokeat { get; set; }

        public DateTime? Outofcontrolat { get; set; }

        public DateTime? Reallyfinishedat { get; set; }

        public string Internalnum { get; set; }

        public decimal? Agreementreferencecount
        {
            get { return 0; }
        }

        public decimal? Generalreferencecount
        {
            get { return 0; }
        }
    }

    [TestClass]
    public class ContractConditionTests
    {
        private readonly DateTime _startDate = new DateTime(2011, 1, 1);
        private readonly DateTime _endDate = new DateTime(2011, 12, 31);

        private void CheckCondition(IContractStateData obj, params ContractCondition[] expectedConditions)
        {
            foreach (var contractCondition in expectedConditions)
            {
                Assert.AreEqual(contractCondition,
                    DefaultContractConditionReolver.Instance.GetContractCondition(obj, _startDate, _endDate) & contractCondition);               
            }    
        }

        [TestMethod]
        public void UndefinedActualStateTest()
        {
            var obj = new StubContractStateData();
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.Actual);

        }
        [TestMethod]
        public void BrokenStateTest()
        {
            var obj = new StubContractStateData(){Brokeat = new DateTime(2011, 10, 10)};
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.Broken);

        }

        [TestMethod]
        public void BrokenPreviouslyStateTest()
        {
            var obj = new StubContractStateData() { Brokeat = new DateTime(2010, 10, 10) };
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.BrokenPreviously);

        }

        [TestMethod]
        public void ClosedStateTest()
        {
            var obj = new StubContractStateData() { Reallyfinishedat = new DateTime(2011, 10, 10) };
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.Closed);

        }

        [TestMethod]
        public void ClosedPrevStateTest()
        {
            var obj = new StubContractStateData() { Reallyfinishedat = new DateTime(2010, 10, 10) };
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.СlosedPreviously);
        }

        [TestMethod]
        public void OutOfControlPrevTest()
        {
            var obj = new StubContractStateData() { Outofcontrolat = new DateTime(2010, 10, 10) };
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.OutOfControlPreviously);

        }

        [TestMethod]
        public void OutOfControlTest()
        {
            var obj = new StubContractStateData() { Outofcontrolat = new DateTime(2011, 10, 10) };
            CheckCondition(obj, ContractCondition.Undefined, ContractCondition.OutOfControl);
        }

        [TestMethod]
        public void SeemsToBeOverdueTestTrue()
        {
            var obj = new StubContractStateData() { Endsat = new DateTime(2011,12,31)};
            Assert.IsTrue(Contractrepositoryview.SeemsToBeOverdue(obj, new DateTime(2012, 1, 1)));
        }

        [TestMethod]
        public void SeemsToBeOverdueTestFalse()
        {
            var obj = new StubContractStateData() { Endsat = new DateTime(2011, 12, 31) };
            Assert.IsFalse(Contractrepositoryview.SeemsToBeOverdue(obj, new DateTime(2011, 10, 10)));
        }

        [TestMethod]
        public void SeemsToBeOverdueTestFalseFinished()
        {
            var obj = new StubContractStateData() { Endsat = new DateTime(2011, 12, 31), Outofcontrolat = new DateTime(2011,12,10)};
            Assert.IsFalse(Contractrepositoryview.SeemsToBeOverdue(obj, new DateTime(2012, 1, 1)));
        }
      

    }
}
