using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MContracts.ViewModel;

namespace MTests
{
    [TestClass]
    public class ContractApprovalViewModelTest
    {
        private void TestSkeleton(Action<IContractRepository, ContractApprovalViewModel> action)
        {
            using (var repository = LinqContractRepositoryTest.CreateLinqContractRepository())
            {
                repository.Context.ExecuteCommand("DELETE FROM ApprovalProcess");
                try
                {
                    var vm = new ContractApprovalViewModel(repository, null);
                    vm.ContractObject = repository.GetContractdoc(1);
                    action(repository, vm);
                }
                finally
                {
                    repository.Context.ExecuteCommand("DELETE FROM ApprovalProcess");
                }
            }
        }

        [TestMethod]
        public void TestAddApproval()
        {
            TestSkeleton((x, y) =>
                             {
                                 y.AddApprovalEntryCommand.Execute(null);
                                 Assert.AreEqual(1, y.ContractObject.Approvalprocesses.Count);
                             });

        }

        [TestMethod]
        public void TestRemoveApproval()
        {
            TestSkeleton((x, y) =>
            {
                y.AddApprovalEntryCommand.Execute(null);
                Assert.AreEqual(1, y.ContractObject.Approvalprocesses.Count);
                y.SelectedApproval = y.ContractObject.Approvalprocesses[0];
                y.RemoveApprovalEntryCommand.Execute(null);
                Assert.AreEqual(0, y.ContractObject.Approvalprocesses.Count);
            });

        }
        [TestMethod]
        public void TestRemoveApprovalAfterEdit()
        {
            TestSkeleton((x, y) =>
            {
                y.AddApprovalEntryCommand.Execute(null);
                y.SelectedApproval = y.ContractObject.Approvalprocesses[0];
                y.SelectedApproval.FromLocation = x.TryGetContext().Locations.Single(z => Math.Abs(z.Id - 2.0) < 0.001);
                y.RemoveApprovalEntryCommand.Execute(null);
                Assert.AreEqual(0, y.ContractObject.Approvalprocesses.Count);
            });

        }

        [TestMethod]
        public void TestSubmitApprovalAfterEdit()
        {
            TestSkeleton((x, y) =>
            {
                y.AddApprovalEntryCommand.Execute(null);
                y.SelectedApproval = y.ContractObject.Approvalprocesses[0];
                y.SelectedApproval.FromLocation = x.TryGetContext().Locations.Single(z => Math.Abs(z.Id - 2.0) < 0.001);
                x.SubmitChanges();
            });
        }
       
        

    }
}
