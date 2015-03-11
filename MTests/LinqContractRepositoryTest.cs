using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.Common;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts;
using MContracts.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for LinqContractRepositoryTest
    /// </summary>
    [TestClass]
    public class LinqContractRepositoryTest
    {
        public LinqContractRepositoryTest()
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
        public void LinqRepositoryIsContextProviderTest()
        {
            Assert.IsInstanceOfType(new LinqContractRepository(ContextFactoryService.Instance), typeof(IContextProvider<McDataContext>));
        }

        [TestMethod]
        public void FetchActualContractviewTest()
        {
            using (var ctx = CreateLinqContractRepository().Context )
            {
                DateTime start = DateTimeExtensions.GetFirstYearDay(2011);
                DateTime end = DateTimeExtensions.GetLastYearDay(2011);
                var result = ctx.FetchActualContractviews(start, end);
                Assert.IsFalse(result.Any(x => x.Id.In(99999, 99998, 99997, 99996, 99995, 99994)));

            }
        }


        [TestMethod]
        public void NewContractTest()
        {
            LinqContractRepository contractRepository = CreateLinqContractRepository();
            Contractdoc actual = contractRepository.NewContractdoc();
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Nds);
            Assert.IsTrue(actual.Contractstate != null && actual.Contractstate.IsUnsigned);
        }

        public static LinqContractRepository CreateLinqContractRepository()
        {
            ContextFactoryService.Instance.QueryLoginProvider = new StubQueryLoginProvider("UD","sys","XE");
            return new LinqContractRepository(ContextFactoryService.Instance);
        }

        [TestMethod]
        public void TestLinqConnectProblems()
        {
            using (var ctx = LinqContractRepositoryTest.CreateLinqContractRepository().Context)
            {
                ctx.ExecuteCommand("DELETE FROM Functionalcustomercontract");
                var contract = ctx.Contractdocs.First();
                var funcCustomer = ctx.Functionalcustomers.First();
                var newFuncCustomerContract = new Functionalcustomercontract() {Functionalcustomer = funcCustomer};
                
                contract.Functionalcustomercontracts.Add(newFuncCustomerContract);                                
                contract.Functionalcustomercontracts.Remove(newFuncCustomerContract);                
                
                ctx.SubmitChanges();
            }
        }
    }
}
