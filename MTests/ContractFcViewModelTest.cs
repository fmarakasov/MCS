using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    /// <summary>
    /// Summary description for ContractFcViewModelTest
    /// </summary>
    [TestClass]
    public class ContractFcViewModelTest
    {
        public ContractFcViewModelTest()
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
        public void TestAddContractFunctionalCustomer()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var ctx = repository.Context;

                var vm = new ContractFcViewModel(repository);

                vm.ContractObject = repository.NewContractdoc();
                ctx.Contractdocs.InsertOnSubmit(vm.ContractObject);

                Functionalcustomer fc = repository.Functionalcustomers.First();

                vm.AddFunctionalCustomerCommand.Execute(fc);
                Assert.AreEqual(1, vm.ContractFunctionalCustomersBindingList.Count);
                
                ctx.SubmitChanges();

                ctx.Contractdocs.DeleteOnSubmit(vm.ContractObject);
                
                ctx.SubmitChanges();
            }
        }

        [TestMethod]
        public void TestRemoveContractFunctionalCustomer()
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var ctx = repository.Context;
              
                ctx.Log = Console.Out;      

                var vm = new ContractFcViewModel(repository);

                vm.ContractObject = repository.NewContractdoc();
                ctx.Contractdocs.InsertOnSubmit(vm.ContractObject);
                

                Functionalcustomer fc = repository.Functionalcustomers.First();

                vm.AddFunctionalCustomerCommand.Execute(fc);
                Assert.AreEqual(1, vm.ContractFunctionalCustomersBindingList.Count);

                vm.RemoveFunctionalCustomerCommand.Execute(vm.ContractFunctionalCustomersBindingList[0]);
                //Assert.AreEqual(0, vm.ContractFunctionalCustomersBindingList.Count);
                //LinqTestUtilities.PrintChangeSet(ctx);


                ctx.SubmitChanges();

                ctx.Contractdocs.DeleteOnSubmit(vm.ContractObject);

                ctx.SubmitChanges();
            }
        }


        [TestMethod]
        public void TestAddRemoveContractFunctionalCustomer()
        {
            // Этот тест пытается воспроизвести ошибку, связанную с циклом добавления/удаления функциональных заказчиков в договор. 
            // Для этого осуществялется попытка добавления ФЗ, за которой следует попытка удаления и снова добавления. 
            // На последней операции возникает исключение, которое требуется устранить
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                var ctx = repository.Context;
                

                var vm = new ContractFcViewModel(repository);

                vm.ContractObject = repository.NewContractdoc();
                ctx.Contractdocs.InsertOnSubmit(vm.ContractObject);

                Functionalcustomer fc = repository.Functionalcustomers.First();

                vm.AddFunctionalCustomerCommand.Execute(fc);
                Assert.AreEqual(1, vm.ContractFunctionalCustomersBindingList.Count);
                
                vm.RemoveFunctionalCustomerCommand.Execute(vm.ContractFunctionalCustomersBindingList[0]);
                Assert.AreEqual(0, vm.ContractFunctionalCustomersBindingList.Count);
                
                vm.AddFunctionalCustomerCommand.Execute(fc);

                ctx.Log = Console.Out;
                ctx.SubmitChanges();

                //ctx.Contractdocs.DeleteOnSubmit(vm.ContractObject);

                //ctx.SubmitChanges();
            }
        }
    }
}
