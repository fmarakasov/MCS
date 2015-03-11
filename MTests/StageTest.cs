using System;
using System.Linq;
using System.Security.Principal;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for StageTest
    /// </summary>
    [TestClass]
    public class StageTest
    {
        public StageTest()
        {

        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBeUndefined()
        {
            Stage newState = new Stage();
            var today = DateTime.Now;
            newState.TimeResolver = new StubTimeResolver(today);
            Assert.AreEqual(StageCondition.Undefined, newState.Stagecondition);

        }

        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBePending()
        {
            Stage newState = new Stage();
            var today = DateTime.Now;
            newState.TimeResolver = new StubTimeResolver(today);
            newState.Startsat = today.AddDays(1);
            newState.Endsat = today.AddDays(2);
            Assert.AreEqual(StageCondition.Pending, newState.Stagecondition);

        }
        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBeActive()
        {
            Stage newState = new Stage();
            var today = DateTime.Now;
            newState.TimeResolver = new StubTimeResolver(today);
            newState.Startsat = today.AddDays(-1);
            newState.Endsat = today.AddDays(1);
            Assert.AreEqual(StageCondition.Active, newState.Stagecondition);
        }
        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBeOverdue()
        {
            Stage newState = new Stage();
            var today = DateTime.Now;
            newState.TimeResolver = new StubTimeResolver(today);
            newState.Startsat = today.AddDays(-2);
            newState.Endsat = today.AddDays(-1);
            Assert.AreEqual(StageCondition.Overdue, newState.Stagecondition);
        }

        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBeClosed()
        {
            Stage newState = new Stage();
            var today = DateTime.Now;
            newState.TimeResolver = new StubTimeResolver(today);
            newState.Startsat = today.AddDays(-2);
            newState.Endsat = today.AddDays(-1);
            newState.Act = new Act();
            Assert.AreEqual(StageCondition.Closed, newState.Stagecondition);
        }
        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldBeClosedByParent()
        {
            Stage parentStage;
            Stage newStage = GetNewStageWithParent(out parentStage);
            parentStage.Act = new Act();
            Assert.AreEqual(StageCondition.Closed, newStage.Stagecondition);
        }

        private Stage GetNewStageWithParent(out Stage parentStage)
        {
            Stage newStage = new Stage();
            parentStage = new Stage();
            newStage.ParentStage = parentStage;
            var today = DateTime.Now;
            parentStage.TimeResolver = new StubTimeResolver(today);
            parentStage.Startsat = today.AddDays(-2);
            parentStage.Endsat = today.AddDays(-1);
            return newStage;
        }

        [TestMethod, WorkItem(474)]
        public void StageConditionTestShouldntBeClosedByParent()
        {
            Stage parentStage;
            Stage newStage = GetNewStageWithParent(out parentStage);            
            Assert.IsTrue(newStage.Stagecondition != StageCondition.Closed);
        }
       

        [TestMethod]
        public void CloseStageWithActTest()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var context = repository.Context;
                var stage = context.Stages.First();
                var act = new Act()
                              {
                                  Nds = context.Nds.FirstOrDefault(),
                                  Acttype = context.Acttypes.FirstOrDefault(),
                                  Enterpriseauthority = context.Enterpriseauthorities.FirstOrDefault(),
                                  Region = context.Regions.FirstOrDefault()
                              };
                stage.Act = act;
                stage.Actid = null;
                context.SubmitChanges();
                context.Acts.DeleteOnSubmit(act);
                context.SubmitChanges();
            }

        }
        

        [TestMethod]
        public void NtpsubviewResultTest()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var context = repository.Context;

                Stageresult result = context.Stageresults.GetNewBindingList()[0] as Stageresult;

                Ntpsubview subview = context.Ntpsubviews.GetNewBindingList()[1] as Ntpsubview;
                Ntpsubview subview2 = context.Ntpsubviews.GetNewBindingList()[0] as Ntpsubview;
                Ntpsubview subview3 = context.Ntpsubviews.GetNewBindingList()[2] as Ntpsubview;

                result.Ntpsubview = subview;
                result.Ntpsubview = subview2;
                result.Ntpsubview = subview3;
                LinqTestUtilities.AssertChangeset(context, 0, 4, 0);

                //context.SubmitChanges();
            }
        }

        private int _hitCount = 0;
        [TestMethod]
        public void StageConditionShouldChangedAfterDates()
        {
            var stage = new Stage();
            var condition = stage.Stagecondition;
            stage.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(stage_PropertyChanged);
            stage.Startsat = DateTime.Today;
            stage.Endsat = DateTime.Today;
            Assert.AreEqual(2, _hitCount);
        }

        void stage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Stagecondition")
                ++_hitCount;
        }

    }
}
