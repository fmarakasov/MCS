using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for CurrencyTest and is intended
    ///to contain all CurrencyTest Unit Tests
    ///</summary>
    [TestClass]
    public class CurrencyTest
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
        ///A test for MoneyInWords
        ///</summary>
        [TestMethod]
        public void MoneyInWordsZeroPriceTest()
        {
            TestMoneyInWords(0, false, false, false, "Ноль тугриков ноль тригов");
        }
        /// <summary>
        ///A test for MoneyInWords
        ///</summary>
        [TestMethod]
        public void MoneyInWordsTest()
        {
            string expectedString =
                "Сто двадцать три миллиона четыреста пятьдесят шесть тысяч семьсот восемьдесят девять тугриков девяносто девять тригов";
            Currency target = CreateTestCurrency();

            decimal value = 123456789.99M;
            bool shortCurrency = false;
            bool shortSmall = false; 
            bool digitSmall = false;


            TestMoneyInWords(value, shortCurrency, shortSmall, digitSmall, expectedString); 
        }
        private static void TestMoneyInWords(decimal value, bool shortCurrency, bool shortSmall, bool digitSmall, string expected)
        {
            Currency target = CreateTestCurrency();
            var actual = target.MoneyInWords(value, shortCurrency, shortSmall, digitSmall);
            Assert.AreEqual(expected, actual);
        }

        public static Currency CreateTestCurrency()
        {
            var target = new Currency(); 

            target.Currencyi = "тугрик";
            target.Currencyr = "тугрика";
            target.Currencym = "тугриков";

            target.Smalli = "триг";
            target.Smallr = "трига";
            target.Smallm = "тригов";

            target.Lowsmallname = "т.";
            target.Highsmallname = "тр.";

            return target;
        }
    }
}