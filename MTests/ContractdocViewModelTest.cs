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
    [TestClass]
    public class ContractdocViewModelTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public static void InsertNewContractWrapper(Action<ContractViewModel, LinqContractRepository> assertFunc)
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                ContractViewModel contractViewModel = new ContractViewModel(repository);
                contractViewModel.WrappedDomainObject = repository.NewContractdoc();
                if (assertFunc!=null)
                    assertFunc(contractViewModel, repository);
                contractViewModel.SaveCommand.Execute(null);
                repository.Context.Contractdocs.DeleteOnSubmit(contractViewModel.WrappedDomainObject as Contractdoc);
                repository.Context.SubmitChanges();
            }
        }

        [TestMethod]
        public void InsertNewContractdocTest()
        {
           InsertNewContractWrapper(null);
        }

        [TestMethod]
        public void PrepaymentCalcTypesTest()
        {
            ContractdocCardViewModel vm = new ContractdocCardViewModel(new StubContractRepository(), null);
            var actual = vm.PrepaymentCalcTypes;
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count());
        }        
        
    }
}
