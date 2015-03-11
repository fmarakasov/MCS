using System;
using System.Collections.Generic;
using MCDomain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for CurrencyServiceTest and is intended
    ///to contain all CurrencyServiceTest Unit Tests
    ///</summary>
    [TestClass]
    public class CurrencyServiceTest
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
        ///A test for GetCurrencyRateOnDate
        ///</summary>
        [TestMethod]
        public void GetCurrencyRateOnDateTest()
        {
            var target = new CurrencyService(); 
            DateTime dateTime = DateTime.Today;
            string currencyCode = "USD";
            decimal actual;
            actual = target.GetCurrencyRateOnDate(dateTime, currencyCode);
            Assert.IsTrue(actual > 0);
        }

        /// <summary>
        ///A test for GetCurrencyRateOnDate
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof (KeyNotFoundException))]
        public void GetCurrencyRateOnDateTestFailedOnNoCurrency()
        {
            var target = new CurrencyService(); 
            DateTime dateTime = DateTime.Today;
            string currencyCode = "--";
            decimal actual;
            actual = target.GetCurrencyRateOnDate(dateTime, currencyCode);
            Assert.IsTrue(actual > 0);
        }
    }
}