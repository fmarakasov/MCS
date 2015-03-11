using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Devart.Data.Linq;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;

namespace MContracts.ViewModel
{

    public class ContractRelationsEditorViewModel : MainViewModelViewModelBase
    {
        private Contractdoc _contextContract;
        public Contractdoc ContextContract
        {
            get
            {
                if (WrappedDomainObject is Contractdoc && _contextContract == null)
                    _contextContract = Repository.GetContractdoc(WrappedDomainObject.CastTo<Contractdoc>().Id);
                return _contextContract;
            }
        }
        public ContractRelationsEditorViewModel(IContractRepository repository):base(repository)
        {
            
        }
        public IEnumerable<IContractStateData> SubcontractCandidates
        {
            get
            {
                if (MainViewModel == null || ContextContract == null) return null;
               
                var actualItems = MainViewModel.ContractRepositoryViewBased.ActualContextItems.Where(
                    x => x.Id == ContextContract.Id);
                var actualContracts = Repository.Contracts.AsParallel().Join(actualItems.AsParallel(), x => x.Id, x => x.Id, (x, y) => x);
                actualContracts.ParallelApply(x => x.ContextContract = ContextContract);
                return actualContracts;
            }
        }

        public ICommand SaveRelationsCommand
        {
            get
            {
                return new RelayCommand(x=>SaveChanges());
            }
        }

        protected override void OnWrappedDomainObjectChanged()
        {
            if (ContextContract !=null)
            {
                DisplayName = string.Format("Редактор связей договора №{0}",WrappedDomainObject.CastTo<IContractStateData>().Internalnum);
            }
        }

        private ObservableCollection<Contractdoc> _agreements;
        public ObservableCollection<Contractdoc> Agreements
        {
            get
            {
                if (_agreements == null && ContextContract !=null)
                {

                    _agreements = new ObservableCollection<Contractdoc>(Repository.TryGetContext().GetDependantContracts(ContextContract.Id));
                }
                return _agreements;
            }
        }

        private IBindingList _subcontracts;
        public IBindingList Subcontracts
        {
            get
            {
                if (_subcontracts == null && ContextContract != null)
                {
                    _subcontracts = ContextContract.CastTo<Contractdoc>().Contracthierarchies.GetNewBindingList();

                }
                return _subcontracts;
            }
        }
        private IList<T> AllChanges<T>(ChangeSet changeSet)
        {
            var updates = changeSet.Updates.OfType<T>().ToList();
            var inserts = changeSet.Inserts.OfType<T>().ToList(); 
            var deletes = changeSet.Deletes.OfType<T>().ToList(); 
            var result = updates.Concat(inserts).Concat(deletes).Distinct().ToList();
            return result;
        }
        protected override void Save()
        {
            var cs = Repository.TryGetContext().GetChangeSet();
            var changed = AllChanges<Contractdoc>(cs);
            var subchanaged = AllChanges<Contracthierarchy>(cs).Select(x => x.SubContractdoc).Distinct();
            var subchanaged1 = AllChanges<Contracthierarchy>(cs).Select(x => x.GeneralContractdoc).Distinct();
            changed = changed.Concat(subchanaged).Concat(subchanaged1).Distinct().ToList();
       
            
            Repository.SubmitChanges();

            foreach (var contractdoc in changed)
            {
                if (contractdoc != null)
                    ViewMediator.NotifyColleagues(RequestRepository.REQUEST_CONTRACT_UPDATED, contractdoc);
            }
        }

        protected override bool CanSave()
        {
            return true;
        }
    }

   
}
