using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel;
using MContracts.ViewModel.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class StubContractHolderModel : WorkspaceViewModel, IContractdocHolder
    {
        private Contractdoc _contractdoc;

        public StubContractHolderModel(IContractRepository repository, Contractdoc contractdoc) : base(repository)
        {
            _contractdoc = contractdoc;
        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return false;
        }

        public override bool IsClosable
        {
            get {return false; }
        }

        public Contractdoc SelectedContract
        {
            get { return _contractdoc; }
        }
    }

    /// <summary>
    /// Summary description for MainViewModelTest
    /// </summary>
    [TestClass]
    public class MainViewModelTest
    {
        public MainViewModelTest()
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
        public void TestAddNewContractCommand()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var vm = new MainWindowViewModel();
                vm.NewContractCommand.Execute(null);

            }
        }
        private void TestContractCreateCommands(Func<MainWindowViewModel, ICommand> commandResolver)
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var vm = new MainWindowViewModel();
                var contractHolderVm = new StubContractHolderModel(repository, new Contractdoc() { Subject = "The test general contract" });
                Assert.IsFalse(commandResolver(vm).CanExecute(null));
                vm.Workspaces.Add(contractHolderVm);
                contractHolderVm.IsActive = true;
                Assert.IsTrue(commandResolver(vm).CanExecute(null));
                commandResolver(vm).Execute(null);
            }
        }

        [TestMethod]
        public void TestAddAgreementContractCommand()
        {
            TestContractCreateCommands(x => x.NewAgreementCommand);
        }
        [TestMethod]
        public void TestAddSubgeneralContractCommand()
        {
            TestContractCreateCommands(x => x.NewSubgeneralCommand);
        }
    }
}
