using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;

namespace MContracts.ViewModel
{
    class ContractorsDesignerViewModel : ContractorsViewModelBase
    {
        public Contractor ContractorObject { get; set; }

       
        public ContractorsDesignerViewModel(IContractRepository repository):base(repository, null)
        {
            
        }


        public string Caption
        {
            get { return ContractorObject.Return(x => x.ToString(), "Новый " + ContractorsViewModel.GetCaption(ContractObject)); }
        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return true;
        }

        new public Contractortype SelectedContractortype
        {
            get { return ContractorObject.With(x => x.Contractortype); }
            set
            {
                ContractorObject.Do(x => x.Contractortype = value);
                OnPropertyChanged(()=>SelectedContractortype);
            }
        }
    }
}
