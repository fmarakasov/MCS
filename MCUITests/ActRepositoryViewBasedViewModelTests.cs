using System;
using System.Collections.Generic;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MCUITests
{
    [TestClass]
    public class ActRepositoryViewBasedViewModelTests
    {
        [TestMethod]
        public void TestBasicCommandsAggregatorImplementation()
        {
            var repository = CreateMockRepository();

            repository.Verify(x=>x.DeleteAct(1));
            
            
            var vm = new ActRepositoryViewBasedViewModel(repository.Object);
            vm.RequestNewAct += vm_RequestNewAct;

            Assert.AreEqual(4, vm.CommandsAggregator.Entities.Count);
            vm.CommandsAggregator.New.Execute(null);       
            



        }

        internal static Mock<IContractRepository> CreateMockRepository()
        {
            var repository = new Mock<IContractRepository>();
            repository.Setup(x => x.Actsrepositoryview).Returns(
                () =>
                new List<Actrepositoryview>()
                    {
                        new Actrepositoryview() {Id = 1, Num = "1", Contractdocid = 1},
                        new Actrepositoryview() {Id = 1, Num = "2", Contractdocid = 1},
                        new Actrepositoryview() {Id = 1, Num = "3", Contractdocid = 2},
                        new Actrepositoryview() {Id = 1, Num = "4", Contractdocid = 2}
                    });
            return repository;
        }

        void vm_RequestNewAct(object sender, CommonBase.EventParameterArgs<Act> e)
        {
            e.Parameter = new Act();
        }
    }
}
