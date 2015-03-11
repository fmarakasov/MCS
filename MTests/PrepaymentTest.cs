using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for PrepaymentTest
    /// </summary>
    [TestClass]
    public class PrepaymentTest
    {
        public PrepaymentTest()
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

        [TestMethod, WorkItem(478)]
        public void TestSumOfPrepaymentsNotExceedPrepayment()
        {
            Contractdoc contractdoc = new Contractdoc() {Price = 100, Prepaymentsum = 50};
            contractdoc.Prepayments.Add(new Prepayment(){CurrenctContract = contractdoc});
            contractdoc.Prepayments[0].Sum = 25;
            contractdoc.Prepayments.Add(new Prepayment(){CurrenctContract = contractdoc});
            contractdoc.Prepayments[1].Sum = 25;
        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSumOfPrepaymentsExceedPrepayment()
        {
            Contractdoc contractdoc = new Contractdoc() { Price = 100, Prepaymentsum = 50 };
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Sum = 25;
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[1].Sum = 26;
        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPrepaymentExceedPrice()
        {
            Contractdoc contractdoc = new Contractdoc() { Price = 100};
            contractdoc.Prepaymentsum = 101;
        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPrepaymentExceedRange()
        {
            Contractdoc contractdoc = new Contractdoc() { Price = 100 };
            contractdoc.Prepaymentsum = -1;
        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPrepaymentYearExceed1960Range()
        {
            Contractdoc contractdoc = new Contractdoc() { Price = 100, Prepaymentsum = 50 };
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Year = 1900;

        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPrepaymentYearExceedLowRange()
        {
            DateTime today = DateTime.Today;
            Contractdoc contractdoc = new Contractdoc() { Startat=today.AddYears(-2), Endsat = today.AddYears(2)};
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Year = today.Year-3;

        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestPrepaymentYearExceedHighRange()
        {
            DateTime today = DateTime.Today;
            Contractdoc contractdoc = new Contractdoc() { Startat = today.AddYears(-2), Endsat = today.AddYears(2) };
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Year = today.Year + 3;
        }

        [TestMethod, WorkItem(478)]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPrepaymentYearDuplicated()
        {
            DateTime today = DateTime.Today;
            Contractdoc contractdoc = new Contractdoc() { Startat = today.AddYears(-2), Endsat = today.AddYears(2) };
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Year = today.Year;

            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[1].Year = today.Year;
        }

        [TestMethod, WorkItem(478)]        
        public void TestPrepaymentYearNotDuplicated()
        {
            DateTime today = DateTime.Today;
            Contractdoc contractdoc = new Contractdoc() { Startat = today.AddYears(-2), Endsat = today.AddYears(2) };
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[0].Year = today.Year;
            contractdoc.Prepayments.Add(new Prepayment() { CurrenctContract = contractdoc });
            contractdoc.Prepayments[1].Year = today.Year+1;
        }

        [TestMethod]
        public void TestRest()
        {
            var contract = ContractdocTest.CreateContractAndActs();
            Assert.AreEqual(70, contract.Prepayments[0].Rest);
            Assert.AreEqual(150, contract.Prepayments[1].Rest);
            Assert.AreEqual(100, contract.Prepayments[2].Rest);
        }
    }
}
