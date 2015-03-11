using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public class ContractCardViewModel : ContractdocBaseViewModel
    {
        public ContractCardViewModel(IContractRepository repository, ViewModelBase owner = null) : base(repository, owner)
        {

        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return string.IsNullOrEmpty(ContractObject.Error);
        }
       

    }
}
