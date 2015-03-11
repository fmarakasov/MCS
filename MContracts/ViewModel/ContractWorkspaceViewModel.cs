using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Базовый класс с поддержкой доступа к объекту договора. Используется в производных классах
    /// ContractViewModel, ActsViewModel, ShceduleViewModel
    /// </summary>
    public abstract class ContractWorkspaceViewModel : WorkspaceViewModel, IContractdocRefHolder, IContractCaption
    {
        public override string DisplayName
        {
            get
            {
                return ContractCaption;
            }
            protected set
            {
                base.DisplayName = value;
            }
        }
        protected ContractWorkspaceViewModel(IContractRepository repository) : base(repository)
        {

        }
        
        public Contractdoc ContractObject
        {
            get
            {
                return WrappedDomainObject as Contractdoc;
            }
            set
            {
                WrappedDomainObject = value;
                OnPropertyChanged(() => ContractObject);
            }
        }

        protected override void Save()
        {
            ContractObject.Do(x => x.UpdateFundsStatistics());
        }

        public long? ContractdocId { get { return ContractObject.Return(x => x.Id, default(long?)); } }
        public long? Maincontractid {
            get { return ContractObject.Return(x => x.Maincontractid, default(long?)); }
        }

        public string ContractCaption { get { return ContractObject.Return(x => x.ToString(), "Договор"); } }
    }
}
