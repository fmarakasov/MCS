using System.Collections.ObjectModel;
using MCDomain.DataAccess;
using MCDomain.Model;
using System.Linq;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    public abstract class ContractorsViewModelBase : ContractdocBaseViewModel
    {
        private ObservableCollection<Contractortype> _contractortypes;
        private Contractortype _selectedContractortype;

        protected ContractorsViewModelBase(IContractRepository repository, ViewModelBase owner) : base(repository, owner)
        {
        }

        protected virtual void OnContractortypesCreated()
        {

        }

        private ObservableCollection<Education> _educations;
        /// <summary>
        /// Получает коллекцию видов образования
        /// </summary>
        public ObservableCollection<Education> Educations
        {
            get
            {
                if (_educations == null)
                {
                    _educations =
                        new ObservableCollection<Education>(Repository.TryGetContext().Educations.OrderBy(x=>x.Id));
                    OnEducationsCreated();

                }
                return _educations;
            }
        }

        protected virtual void OnEducationsCreated()
        {

        }


        /// <summary>
        /// Получает коллекцию типов контрагентов
        /// </summary>
        public ObservableCollection<Contractortype> Contractortypes
        {
            get
            {
                if (_contractortypes == null)
                {
                    _contractortypes =
                        new ObservableCollection<Contractortype>(Repository.TryGetContext().Contractortypes.OrderBy(x => x.Id));
                    OnContractortypesCreated();
                    
                }
                return _contractortypes;
            }
        }

        /// <summary>
        /// Получает выбранный тип контрагента
        /// </summary>
        public Contractortype SelectedContractortype
        {
            get { return _selectedContractortype; }
            set
            {
                _selectedContractortype = value;
                OnPropertyChanged(() => SelectedContractortype);
                OnSelectedContractortypeChanged();
            }
        }

        protected virtual void OnSelectedContractortypeChanged()
        {
     
        }
    }
}