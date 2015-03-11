using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for ScheduleTest and is intended
    ///to contain all ScheduleTest Unit Tests
    ///</summary>
    [TestClass]
    public class ScheduleTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion

        private Schedule CreateTestSchedule(DateTime start)
        {
            var target = new Schedule();

            target.Stages.Add(new Stage
                                  {
                                      Range = new DateRange {Start = start, End = start.AddDays(10)},
                                      ConditionResolver = new StubStageConditionResolver(StageCondition.Active)
                                  });
            target.Stages.Add(new Stage
                                  {
                                      Range = new DateRange {Start = start.AddDays(10), End = start.AddDays(30)},
                                      ConditionResolver = new StubStageConditionResolver(StageCondition.Pending)
                                  });
            target.Stages.Add(new Stage
                                  {
                                      Range = new DateRange {Start = start, End = start.AddDays(20)},
                                      ConditionResolver = new StubStageConditionResolver(StageCondition.Closed)
                                  });
            target.Stages.Add(new Stage
                                  {
                                      Range = new DateRange {Start = start.AddDays(30), End = start.AddDays(40)},
                                      ConditionResolver = new StubStageConditionResolver(StageCondition.Overdue)
                                  });
            target.Stages.Add(new Stage
                                  {
                                      Range = new DateRange {Start = start.AddDays(50), End = start.AddDays(70)},
                                      ConditionResolver = new StubStageConditionResolver(StageCondition.Pending)
                                  });

            return target;
        }


        [TestMethod]
        public void GetLeafsTest()
        {
            var schedule = new Schedule();
            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1"
                                    });
            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.1",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1")
                                    });

            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.2",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1")
                                    });

            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.3",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1")
                                    });

            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.1.1",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                    });

            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.1.2",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                    });
            schedule.Stages.Add(new Stage
                                    {
                                        Num = "1.1.3",
                                        ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                    });

            var res = new List<Stage>
                          {
                              new Stage
                                  {
                                      Num = "1.2",
                                      ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1")
                                  },
                              new Stage
                                  {
                                      Num = "1.3",
                                      ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1")
                                  },
                              new Stage
                                  {
                                      Num = "1.1.1",
                                      ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                  },
                              new Stage
                                  {
                                      Num = "1.1.2",
                                      ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                  },
                              new Stage
                                  {
                                      Num = "1.1.3",
                                      ParentStage = schedule.Stages.FirstOrDefault(x => x.Num == "1.1")
                                  }
                          };

            var actual = schedule.GetLeafs(new Stage
                                               {
                                                   Num = "1"
                                               }).ToList();
            Assert.AreEqual(actual, res);
        }

        /// <summary>
        ///Проверка корректности выборки этапов по диапазону дат
        ///</summary>
        [TestMethod]
        public void GetStagesTest()
        {
            DateTime start = DateTime.Today;
            Schedule target = CreateTestSchedule(start);

            DateTime fromDate = start.AddDays(10);
            DateTime toDate = start.AddDays(20);
            IEnumerable<Stage> actual;
            actual = target.SelectStages(fromDate, toDate, x => x != DateRangesIntersectionResult.NotInRange);
            Assert.AreEqual(3, actual.Count());
        }

        /// <summary>
        ///A test for IsOverdue
        ///</summary>
        [TestMethod]
        public void IsOverdueTest()
        {
            Schedule target = CreateTestSchedule(DateTime.Today);
            bool actual;
            actual = target.IsOverdue;
            Assert.IsTrue(actual);
        }
        
        
        [TestMethod]
        public void SelectStagesTest()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var contract = repository.GetContractdoc(1);
                var actual = contract.DefaultSchedule.SelectStages(DateTime.Parse("05.07.2010"), DateTime.Parse("27.12.2010"),
                                                      x => x == DateRangesIntersectionResult.EndsInRange);
                Assert.AreEqual(2, actual.Count());
            }

        }

        [TestMethod]
        public void CreateScheduleInNewContract()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var context = repository.Context;
                Contractdoc doc = repository.NewContractdoc();
                var SC = new Schedulecontract()
                             {
                                 Schedule = new Schedule(){Currencymeasure = context.Currencymeasures.First(), Worktype = context.Worktypes.First()},
                                 Appnum = 3,                                 
                             };
                doc.Schedulecontracts.Add(SC);
                SC.Schedule.Stages.Add(new Stage()
                                           {
                                               Startsat = DateTime.Today,
                                               Endsat = DateTime.Today.AddDays(1),
                                               Subject = "dfsdfsdfsdfsdf",
                                               Ndsalgorithm = context.Ndsalgorithms.FirstOrDefault(),
                                               Price = 100,
                                               Num = "1",
                                               Nds = context.Nds.First()
                                           });

                context.Contractdocs.InsertOnSubmit(doc);
                context.SubmitChanges();
                context.Contractdocs.DeleteOnSubmit(doc);
                Assert.AreNotEqual(doc.Id, 0);
                context.SubmitChanges();
            }
        }
    }
}