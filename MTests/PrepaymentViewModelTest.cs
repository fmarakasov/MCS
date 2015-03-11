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
    public class PrepaymentViewModelTest
    {
        /// <summary>
        /// Тест правила, что НДС аванса не может быть изменён, если договор не облагается налогом и должен быть выставлен как необлагающияся налогом
        /// </summary>
        [WorkItem(439), TestMethod]
        public void TestNdsAlgorithmRule()
        {
            var repository = new StubContractRepository();
            PrepaymentViewModel model = new PrepaymentViewModel(repository);
            var contract = repository.Contracts[0];            
            model.ContractObject = contract;

            contract.Ndsalgorithm = repository.Ndsalgorithms.Single(x => x.NdsType == TypeOfNds.NoNds);
            Assert.IsFalse(model.NdsAlgorithmEnabled);
            Assert.AreSame(contract.Ndsalgorithm, contract.Prepaymentndsalgorithm);

            contract.Ndsalgorithm = repository.Ndsalgorithms.Single(x => x.NdsType == TypeOfNds.ExcludeNds);
            Assert.IsTrue(model.NdsAlgorithmEnabled);
            Assert.AreNotSame(contract.Ndsalgorithm, contract.Prepaymentndsalgorithm);

        }

        [WorkItem(455), TestMethod]
        public void PrepaymentMoneyInfoTest()
        {
            var repository = new StubContractRepository();
            PrepaymentViewModel model = new PrepaymentViewModel(repository);
            var contract = repository.Contracts[0];
            contract.Ndsalgorithm = new Ndsalgorithm();
            contract.Prepaymentndsalgorithm = new Ndsalgorithm();
            model.ContractObject = contract;
            var actual = model.PrepaymentMoneyInfo;
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Algorithm);
            Assert.AreSame(actual.Algorithm, contract.Prepaymentndsalgorithm);
            Assert.IsNotNull(actual.Nds);
            Assert.IsNotNull(actual.Currency);
            Assert.IsNotNull(actual.Measure);
            Assert.AreEqual(actual.Price, contract.Prepaymentsum);            
        }
       
    }
}
