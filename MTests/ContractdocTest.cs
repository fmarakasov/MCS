using System;
using System.Collections.Generic;
using System.Linq;
using MCDomain;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for ContractdocTest and is intended
    ///to contain all ContractdocTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractdocTest
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
        /// Тест дат договора.
        /// Предполагается, что должны выполняться следующие утверждения:
        ///    1) Д(подписания) LE Д(Применения) LE Д(Начала) L (Окончание)
        ///    2) Если договор находится в состоянии Подписан, то дата подписания должна быть задана
        ///    
        /// </summary>
        [TestMethod]
        public void TestDataErrorInfoDates()
        {
            var target = new Contractdoc(); 

            DateTime today = DateTime.Today;
            Assert.Inconclusive("Не завершён");
            
        }


        /// <summary>
        /// Тест свойства AllAgreement при условии, что у договора нет ДС
        /// </summary>
        [TestMethod]
        public void TestAgreements00()
        {
            var contractdoc = new Contractdoc();
            Assert.AreEqual(0, contractdoc.AllAgreements.Count());
        }

        /// <summary>
        /// Тест свойства AllAgreement при условии, что у договора есть 1 ДС
        /// </summary>
        [TestMethod]
        public void TestAgreements01()
        {
            var contractdoc = new Contractdoc();
            var agreement1 = new Contractdoc();

            contractdoc.Contractdocs.Add(agreement1);
            IEnumerable<Contractdoc> agreements = contractdoc.AllAgreements;
            Assert.AreEqual(1, agreements.Count());
            Assert.IsTrue(agreements.Contains(agreement1));
            Assert.AreEqual(0, agreement1.AllAgreements.Count());
        }

        /// <summary>
        /// Тест свойства AllAgreement при условии, что у договора есть 2 ДС
        /// </summary>        
        [TestMethod]
        public void TestAgreements02()
        {
            var contractdoc = new Contractdoc();
            var agreement1 = new Contractdoc();
            var agreement2 = new Contractdoc();

            contractdoc.Contractdocs.Add(agreement1);
            agreement1.Contractdocs.Add(agreement2);

            Assert.AreEqual(2, contractdoc.AllAgreements.Count());
            Assert.AreEqual(1, agreement1.AllAgreements.Count());
            Assert.AreEqual(0, agreement2.AllAgreements.Count());
        }

        /// <summary>
        /// Тест свойства AllAgreement при условии, что у договора есть ДС к которому есть ещё ДС
        /// </summary>        
        [TestMethod]
        public void TestAgreements002()
        {
            var contractdoc = new Contractdoc();
            var agreement1 = new Contractdoc();
            var agreement2 = new Contractdoc();

            contractdoc.Contractdocs.Add(agreement1);
            contractdoc.Contractdocs.Add(agreement2);

            Assert.AreEqual(2, contractdoc.AllAgreements.Count());
            Assert.AreEqual(0, agreement1.AllAgreements.Count());
            Assert.AreEqual(0, agreement2.AllAgreements.Count());
        }

        /// <summary>
        /// Тест свойства IsSubContract
        /// </summary>        
        [TestMethod]
        public void TestIsSubcontract()
        {
            var contractdoc = new Contractdoc();
            var subcontract = new Contractdoc();
            Assert.IsFalse(contractdoc.IsSubContract);
            Assert.IsFalse(subcontract.IsSubContract);
            contractdoc.AddSubcontract(subcontract);            
            Assert.IsTrue(subcontract.IsSubContract);
            Assert.IsFalse(contractdoc.IsSubContract);
        }


        /// <summary>
        ///Проверка IDataErrorInfo при неверном задании даты начала и окончания договора
        ///</summary>
        [TestMethod]
        public void DataErrorInfoInvalidStartatTest()
        {
            DateTime startsat = DateTime.Today;
            var target = new Contractdoc { Startat = startsat, Endsat = startsat.AddDays(-1) };
            string columnName = "Startat";
            string result = target[columnName];
            Assert.IsFalse(string.IsNullOrEmpty(result), "Ожидалось сообщение об ошибке");
        }

        /// <summary>
        ///Проверка IDataErrorInfo при неверном задании даты подписания
        ///</summary>
        [TestMethod]
        public void DataErrorInfoInvalidApprovedat()
        {
            DateTime startsat = DateTime.Today;
            var target = new Contractdoc { Startat = startsat, Endsat = startsat.AddDays(7), Approvedat = startsat.AddDays(-1) };
            string columnName = "Approvedat";
            string result = target[columnName];
            Assert.IsFalse(string.IsNullOrEmpty(result), "Ожидалось сообщение об ошибке");
        }


        /// <summary>
        ///A test for PriceAsString
        ///</summary>
        [TestMethod]
        public void PriceAsStringTest()
        {
            var target = new Contractdoc();
            target.Currency = CurrencyTest.CreateTestCurrency();
            target.Price = 123.45M;
            string expected = "сто двадцать три тугрика 45 т.";
            string actual;
            actual = target.PriceAsString;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod]
        public void ContractCloneTest()
        {
            //Contract target = new Contract(); // TODO: Initialize to an appropriate value
            //object expected = target; // TODO: Initialize to an appropriate value
            //object actual;
            //actual = target.Clone();            
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PriceNds
        ///</summary>
        [TestMethod]
        public void PriceNdsTest()
        {
            var target = new Contractdoc(); 
            var nds = new Nds { Percents = 20 };
            var alg = new Ndsalgorithm { Id = (int)TypeOfNds.ExcludeNds };
            target.Nds = nds;
            target.Ndsalgorithm = alg;
            target.Price = 100;
            Assert.AreEqual(20, target.ContractMoney.NdsValue);
        }
        [TestMethod]
        public void ActualShouldReturnThisTest()
        {
            var target = new Contractdoc();
            Assert.AreSame(target, target.Actual);
        }

        [TestMethod]
        public void ActualShouldReturnAgrrementTest()
        {
            var target = new Contractdoc();
            var agreement = new Contractdoc();
            target.AddAgreement(agreement);
            Assert.AreSame(agreement, target.Actual);
        }

        [TestMethod]
        public void ActualShouldReturnThisAgreementTest()
        {
            var target = new Contractdoc();
            var agreement = new Contractdoc();
            target.AddAgreement(agreement);
            Assert.AreSame(agreement, agreement.Actual);
        }

        [TestMethod]
        public void ActualShouldReturnLastAgreementTest()
        {
            var target = new Contractdoc();
            var agreement = new Contractdoc();
            var agreement2 = new Contractdoc();
            target.AddAgreement(agreement);
            agreement.AddAgreement(agreement2);
            Assert.AreSame(agreement2, target.Actual);
        }   
 
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddAgreementShouldRiseContractNotNullTest()
        {
            var target = new Contractdoc();
            target.AddAgreement(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void AddAgreementShouldRiseContractRecursiveTest()
        {
            var target = new Contractdoc();
            target.AddAgreement(target);
        }

        [TestMethod]        
        public void TestActsShouldReturnEmptyCollection()
        {
            var target = new Contractdoc();
            var result = target.Acts;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TestActsShouldReturnNonEmpty()
        {
            var target = CreateContractAndActs();

            var result = target.Acts;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        //[TestMethod]
        //public void TestPrepaymentRestsShouldNotRelayOnPreviuseValue()
        //{
        //    var target = CreateContractAndActs();
        //    var act = target.Acts.First();
            
        //    var old_credited = act.Credited;
        //    var old_total = act.Totalsum;
        //    var old_sum_to_trans = act.Sumfortransfer;
        //    var old_Rests = target.PrepaymentRests;



        //}

        [TestMethod]
        public void TestActsShouldReturnEmptyWhenStagesPresents()
        {
            var target = CreateContractAndActs();
            foreach (var s in target.Stages) s.Act = null;
            var result = target.Acts;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void TestActTransferedFounds()
        {
            var contract = CreateContractAndActs();
            Assert.AreEqual(420, contract.ActTransferedFunds);
        }

        [TestMethod]
        public void TestTransferedFounds()
        {
            var contract = CreateContractAndActs();
            Assert.AreEqual(350, contract.TransferedFunds);
        }

        [TestMethod]
        public void TestPrepaymented()
        {
            var contract = CreateContractAndActs();
            contract.FundsResolver = new StubFundsResolver(700, 0);
            Assert.AreEqual(280, contract.Prepaymented);
        }

        [TestMethod]
        public void TestPrepaymentRests()
        {
            var contract = CreateContractAndActs();
            Assert.AreEqual(70, contract.PrepaymentRests);
        }

        [TestMethod]
        public void TestFundsDisbursed()
        {
            var contract = CreateContractAndActs();
            Assert.AreEqual(700, contract.FundsDisbursed);
        }

        [TestMethod]
        public void TestPrepaymentsListChangedShouldInfluenceRests()
        {
            var contract = CreateContractAndActs();
            contract.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(contract_PropertyChanged);
            contract.Contractpayments.Add(new Contractpayment(){Paymentdocument = new Paymentdocument(){Paymentsum = 100}});
            Assert.AreEqual(1, _hits);
            contract.Contractpayments[0].Paymentdocument.Paymentsum = 200;
            Assert.AreEqual(2, _hits);
        }

        private int _hits;
        void contract_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PrepaymentRests")
                ++_hits;
        }

        public static Contractdoc CreateContractAndActs()
        {
            var nds = new Nds() { Id = 1, Percents = 0.18 };
            var measure = new Currencymeasure() { Id = 1, Factor = 1 };
            var currency = new Currency() { Id = 1, Culture = "Ru-Ru" };
            var ndsAlg = new Ndsalgorithm() { Id = 1 };

            var target = new Contractdoc()
                             {
                                 Startat = DateTime.Parse("1.1.2000"),
                                 Endsat = DateTime.Parse("31.12.2002"),
                                 Currency = currency,
                                 Currencymeasure = measure,
                                 Nds = nds,
                                 Ndsalgorithm = ndsAlg,
                                 Prepaymentndsalgorithm = ndsAlg
                             };
            
            target.Prepayments.Add(new Prepayment() { Year = 2000, Sum = 100 });
            target.Prepayments.Add(new Prepayment() { Year = 2001, Sum = 200 });
            target.Prepayments.Add(new Prepayment() { Year = 2002, Sum = 300 });

            target.Schedulecontracts.Add(new Schedulecontract());
            target.Schedulecontracts.Add(new Schedulecontract());

            target.Schedulecontracts[0].Schedule = new Schedule();
            target.Schedulecontracts[1].Schedule = new Schedule();

            
            target.Schedulecontracts[0].Schedule.Currencymeasure = measure;
            
            target.Schedulecontracts[0].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2000"), Nds = nds, Ndsalgorithm = ndsAlg });
            target.Schedulecontracts[0].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2000"), Nds = nds, Ndsalgorithm = ndsAlg });
            target.Schedulecontracts[0].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2001"), Nds = nds, Ndsalgorithm = ndsAlg });
            target.Schedulecontracts[1].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2001"), Nds = nds, Ndsalgorithm = ndsAlg });
            target.Schedulecontracts[1].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2001"), Nds = nds, Ndsalgorithm = ndsAlg });
            target.Schedulecontracts[1].Schedule.Stages.Add(new Stage() { Endsat = DateTime.Parse("1.2.2002"), Nds = nds, Ndsalgorithm = ndsAlg });

            Act act1 = new Act() { Sumfortransfer = 70, Totalsum = 100  };
            Act act2 = new Act() { Sumfortransfer = 150, Totalsum = 200 };
            Act act3 = new Act() { Sumfortransfer = 200, Totalsum = 400 }; 

            target.Schedulecontracts[0].Schedule.Stages[0].Act = act1;
            target.Schedulecontracts[0].Schedule.Stages[1].Act = act1;
            target.Schedulecontracts[0].Schedule.Stages[2].Act = act2;

            target.Schedulecontracts[1].Schedule.Stages[0].Act = act2;
            target.Schedulecontracts[1].Schedule.Stages[1].Act = act2;
            target.Schedulecontracts[1].Schedule.Stages[2].Act = act3;

            target.Contractpayments.Add(new Contractpayment()
                                            {Paymentdocument = new Paymentdocument() {Paymentsum = 200}});
            target.Contractpayments.Add(new Contractpayment()
                                            {Paymentdocument = new Paymentdocument() {Paymentsum = 150}});
            return target;
        }
    }
}