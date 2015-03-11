using System;
using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for DateRangeTest and is intended
    ///to contain all DateRangeTest Unit Tests
    ///</summary>
    [TestClass]
    public class DateRangeTest
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

        /// <summary>
        ///A test for CheckIntersect
        ///</summary>
        [TestMethod]
        public void CheckIntersectTest()
        {
            DateTime nowDate = DateTime.Today;
            CheckIntersect(nowDate, nowDate.AddDays(7), nowDate.AddDays(-7), nowDate.AddDays(-1),
                           DateRangesIntersectionResult.NotInRange);
            CheckIntersect(nowDate, nowDate.AddDays(7), nowDate.AddDays(1), nowDate.AddDays(10),
                           DateRangesIntersectionResult.StartsInRane);
            CheckIntersect(nowDate, nowDate.AddDays(7), nowDate.AddDays(-1), nowDate.AddDays(5),
                           DateRangesIntersectionResult.EndsInRange);
            CheckIntersect(nowDate, nowDate.AddDays(7), nowDate.AddDays(1), nowDate.AddDays(5),
                           DateRangesIntersectionResult.InRange);
        }


        internal static void CheckIntersect(DateTime start, DateTime end, DateTime start1, DateTime end1,
                                            DateRangesIntersectionResult expected)
        {
            var target = new DateRange {Start = start, End = end};
            var other = new DateRange {Start = start1, End = end1};
            DateRangesIntersectionResult actual;
            actual = target.CheckIntersect(other);
            Assert.AreEqual(expected, actual);
        }
    }
}