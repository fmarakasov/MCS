using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for MoneyViewModelTest
    /// </summary>
    [TestClass]
    public class MoneyViewModelTest
    {
        public MoneyViewModelTest()
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

        [TestMethod]
        public void CanCalculatePricesTest()
        {
            MoneyModel moneyViewModel = new MoneyModel(null, null, null, null, null);
            Assert.IsFalse(moneyViewModel.CanCalculatePrices);
            moneyViewModel = new MoneyModel(new Ndsalgorithm(), null, null, null, null);
            Assert.IsFalse(moneyViewModel.CanCalculatePrices);
            moneyViewModel = new MoneyModel(new Ndsalgorithm(), new Nds(), null, null, null);
            Assert.IsFalse(moneyViewModel.CanCalculatePrices);
            moneyViewModel = new MoneyModel(new Ndsalgorithm(), new Nds(), new Currency(), null, null);
            Assert.IsFalse(moneyViewModel.CanCalculatePrices);
            moneyViewModel = new MoneyModel(new Ndsalgorithm(), new Nds(), new Currency(), new Currencymeasure(), null);
            Assert.IsFalse(moneyViewModel.CanCalculatePrices);
            moneyViewModel = new MoneyModel(new Ndsalgorithm(), new Nds(), new Currency(), new Currencymeasure(), 100);
            Assert.IsTrue(moneyViewModel.CanCalculatePrices);            
        }

        private MoneyModel CreateTestMoneyViewModel(int factor, Currency currency = null, decimal? price = 100)
        {
            if (currency == null)
                currency = new Currency() {Culture = "RU-RU"}; 
            return new MoneyModel(new Ndsalgorithm() { Id = (int)TypeOfNds.IncludeNds },
                new Nds() { Percents = 20 }, currency, new Currencymeasure() { Factor = factor }, price);
            
        }

        private void AssertPrices(Func<string> func, string expected)
        {
            Assert.AreEqual(expected, func());
        }

        [TestMethod, WorkItem(455)]
        public void PriceWithNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(1);
            AssertPrices(()=>moneyViewModel.PriceWithNds, Currency.National.FormatMoney(100));
        }


        [TestMethod, WorkItem(455)]
        public void PriceNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(1);
            AssertPrices(() => moneyViewModel.PriceNds, Currency.National.FormatMoney(16.67M));
        }

        [TestMethod, WorkItem(455)]
        public void PricePureTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(1);
            AssertPrices(() => moneyViewModel.PricePure, Currency.National.FormatMoney(83.33M));
        }

        [TestMethod, WorkItem(455)]
        public void PriceNormalizedPureTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10);
            AssertPrices(() => moneyViewModel.Factor.PricePure, Currency.National.FormatMoney(833.33M));
        }

        [TestMethod, WorkItem(455)]
        public void PriceNormalizedNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10);
            AssertPrices(() => moneyViewModel.Factor.PriceNds, Currency.National.FormatMoney(166.67M));
        }

        [TestMethod, WorkItem(455)]
        public void PriceNormalizedPriceWithNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10);
            AssertPrices(() => moneyViewModel.Factor.PriceWithNds, Currency.National.FormatMoney(1000));
        }

        [TestMethod, WorkItem(455)]
        public void PriceAsNationalPriceWithNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10, new Currency() {Culture = "En-us"});
            AssertPrices(() => moneyViewModel.AsNational(10).PriceWithNds, Currency.National.FormatMoney(1000));
        }

        [TestMethod, WorkItem(455)]
        public void PriceAsNationalWhenNationalPriceWithNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10, new Currency() { Culture = "Ru-ru" });
            AssertPrices(() => moneyViewModel.AsNational(10).PriceWithNds, Currency.National.FormatMoney(1000));
        }
        [TestMethod, WorkItem(455)]
        public void PriceAsNationalWhenNationalAndFactorPriceWithNdsTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10, new Currency() { Culture = "Ru-ru" });
            AssertPrices(() => moneyViewModel.Factor.AsNational(10).PriceWithNds, Currency.National.FormatMoney(10000));
        }

        [TestMethod, WorkItem(455)]
        public void PriceAsNationalWhenPriceNotSpecifiedTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10, new Currency() { Culture = "Ru-ru" }, default(decimal?));
            AssertPrices(() => moneyViewModel.Factor.AsNational(10).PriceWithNds, string.Empty);
        }

        [TestMethod, WorkItem(455)]
        public void PriceAsNationalWhenRateNotSpecifiedTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(10, new Currency() { Culture = "Ru-ru" });
            AssertPrices(() => moneyViewModel.Factor.AsNational(default(decimal?)).PriceWithNds, string.Empty);
        }

        [TestMethod, WorkItem(455)]
        public void PrefixTest()
        {
            MoneyModel moneyViewModel = CreateTestMoneyViewModel(100);
            moneyViewModel.NdsPrefix = "НДС: ";
            moneyViewModel.PurePrefix = "Цена без НДС: ";
            moneyViewModel.WithNdsPrefix = "Цена с НДС: ";
            Assert.AreEqual("Цена с НДС: 100,00р.", moneyViewModel.PriceWithNds);
            Assert.AreEqual("НДС: 16,67р.", moneyViewModel.PriceNds);
            Assert.AreEqual("Цена без НДС: 83,33р.", moneyViewModel.PricePure);

        }
        [TestMethod, WorkItem(455)]
        public void PrefixNoInfoMessageTest()
        {
            MoneyModel moneyViewModel = new MoneyModel(null, null, null, null, null);
            moneyViewModel.NoInfoMessage = "-";
            moneyViewModel.WithNdsPrefix = "Цена с НДС: ";
            Assert.AreEqual(moneyViewModel.WithNdsPrefix + moneyViewModel.NoInfoMessage, moneyViewModel.PriceWithNds);            
        }
        [TestMethod, WorkItem(514)]
        public void DefaultMoneyViewModelTest()
        {
            DefaultMoneyModel moneyViewModel = new DefaultMoneyModel(null, null, null, null, null, null);
            Assert.AreEqual("Цена с НДС: ", moneyViewModel.WithNdsPrefix);
            Assert.AreEqual("НДС: ", moneyViewModel.NdsPrefix);
            Assert.AreEqual("Цена без НДС: ", moneyViewModel.PurePrefix);
        }



    }
}
