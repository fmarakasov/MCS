using MCDomain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for MoneyToStringProviderBaseTest and is intended
    ///to contain all MoneyToStringProviderBaseTest Unit Tests
    ///</summary>
    [TestClass]
    public class MoneyToStringProviderBaseTest
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

        internal virtual MoneyToStringProviderBase CreateMoneyToStringProviderBase()
        {
      
            MoneyToStringProviderBase target = new DollarToStringProvider(true, true, true);
            return target;
        }

        /// <summary>
        ///A test for MoneyToString
        ///</summary>
        [TestMethod]
        public void MoneyToStringDollTest()
        {
            MoneyToStringProviderBase target = CreateMoneyToStringProviderBase();
               
            var m = new Money(123456789.99M); 
            string expected =
                "сто двадцать три миллиона четыреста пятьдесят шесть тысяч семьсот восемьдесят девять дол. 99 ц.";
            string actual;
            actual = target.MoneyToString(m);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MoneyToString
        ///</summary>
        [TestMethod]
        public void MoneyToStringCustomTest()
        {
            var target = new CustomizableCurrencyToStringProvider(false, false, true);
            target.SetCurrency(WordCase.CaseI, "тугрик");
            target.SetCurrency(WordCase.CaseR, "тугрика");
            target.SetCurrency(WordCase.CaseM, "тугриков");

            target.SetSmallCurrency(WordCase.CaseI, "триг");
            target.SetSmallCurrency(WordCase.CaseR, "трига");
            target.SetSmallCurrency(WordCase.CaseM, "тригов");

            var m = new Money(123456789.99M);
            string expected =
                "сто двадцать три миллиона четыреста пятьдесят шесть тысяч семьсот восемьдесят девять тугриков 99 тригов";
            string actual;
            actual = target.MoneyToString(m);
            Assert.AreEqual(expected, actual);
        }
    }
}