using System;
using System.Linq;
using System.Windows;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    ///This is a test class for ContractViewModelTest and is intended
    ///to contain all ContractViewModelTest Unit Tests
    ///</summary>
    [TestClass]
    public class ContractViewModelTest
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
        ///A test for DisplayName
        ///</summary>
        [TestMethod]
        [DeploymentItem("MContracts.exe")]
        public void DisplayNameTest()
        {
            var contract = new Contractdoc();
            var viewModel = new ContractViewModel(new StubContractRepository());
            Assert.AreEqual("Новый договор", viewModel.DisplayName);
            viewModel.WrappedDomainObject = contract;
            Assert.AreEqual("Договор №(N/A)", viewModel.DisplayName);
            contract.Internalnum = "100";
            Assert.AreEqual("Договор №100", viewModel.DisplayName);
            DateTime today = DateTime.Today;
            contract.Approvedat = today;
            Assert.AreEqual("Договор №100 от " + today.ToShortDateString(), viewModel.DisplayName);
            var state = new Contractstate {Name = "Не подписан"};
            contract.Contractstate = state;
            Assert.AreEqual("Договор №100 от " + today.ToShortDateString(), viewModel.DisplayName);
        }

        

        
    }
}