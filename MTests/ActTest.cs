using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for ActTest
    /// </summary>
    [TestClass]
    public class ActTest
    {
        public ActTest()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
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
        public void ActToStringTest()
        {
            Act act = new Act();
            Assert.AreEqual("Акт №  от 1 января 0001 г.", act.ToString());
        }

        [TestMethod]
        public void ActToStringTest01()
        {
            Act act = new Act();
            act.Num = "01-01-01";
            Assert.AreEqual("Акт № 01-01-01 от 1 января 0001 г.", act.ToString());
        }

        [TestMethod]
        public void ActToStringTest02()
        {
            Act act = new Act();
            act.Num = "01-01-01";
            act.Signdate = DateTime.Parse("01.01.2010");
            Assert.AreEqual("Акт № 01-01-01 от 1 января 2010 г.", act.ToString());
        }

        [TestMethod]
        public void ActNullContractdocTest()
        {
            Act act = new Act();
            Assert.IsInstanceOfType(act.ContractObject, typeof (NullContractdoc));
        }

        [TestMethod]
        public void CreditedTest()
        {
            var contract = ContractdocTest.CreateContractAndActs();
            var act = contract.Acts.First();

            // Убедимся, что работаем с нужным нам актом
            Assert.AreEqual(100, act.Totalsum);
            Assert.AreEqual(30, act.Credited);

            // Выполнение
            act.Credited = 50;
            Assert.AreEqual(50, act.Credited);

            act.Totalsum = null;
            act.Sumfortransfer = null;
            Assert.AreEqual(0, act.Credited);
        }

       
        [TestMethod]
        public void CreditedChangedShouldInfluenceRests()
        {
            var contract = ContractdocTest.CreateContractAndActs();
            var act = contract.Acts.First();
            contract.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(contract_PropertyChanged);
            act.Credited = 10;
            Assert.AreEqual(1, _hits);

        }

        private int _hits;
        void contract_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PrepaymentRests")
                ++_hits;
        }
    }
}
