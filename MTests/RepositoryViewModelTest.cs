using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MCDomain.DataAccess;
using MContracts.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MTests
{
    class StubViewModel : RepositoryViewModel
    {
        public StubViewModel(IContractRepository contractRepository)
            : base(contractRepository)
        {

        }
        public void InvokeAutoSubmit()
        {
            AutoSubmit(()=> { return;});
        }
        protected override void Save()
        {
            throw new NotImplementedException();
        }

        protected override bool CanSave()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    
    public class RepositoryViewModelTest
    {
        [TestMethod]
        [ExpectedException(typeof(SubmitChangesFailed))]
        public void TestAutoSummitShouldFailed()
        {
            StubViewModel vm = new StubViewModel(new StubContractRepositoryUnimplemented());
            vm.InvokeAutoSubmit();
        }
    }
}
