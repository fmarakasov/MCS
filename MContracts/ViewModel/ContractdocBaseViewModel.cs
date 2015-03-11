using System.ComponentModel;
using System.Diagnostics.Contracts;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    abstract public class ContractdocBaseViewModel : RepositoryViewModel
    {
        protected ContractdocBaseViewModel(IContractRepository repository, ViewModelBase owner=null)
            : base(repository, owner)
        {
            ViewMediator.Register(this);
        }

        void ContractObject_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            
        }

        public bool IsMaximized { get; set; }

        /// <summary>
        /// Получает или устанавливает объект договора для модели представления
        /// </summary>
        public Contractdoc ContractObject
        {
            get
            {
                if (WrappedDomainObject != null)
                    Contract.Assert(WrappedDomainObject is Contractdoc);
                return WrappedDomainObject as Contractdoc;
            }
            set
            {
                Contract.Ensures(WrappedDomainObject == value);
                if (WrappedDomainObject == value) return;

                if (ContractObject != null)
                {
                    ContractObject.PropertyChanging -= value_PropertyChanging;
                    ContractObject.PropertyChanged -= value_PropertyChanged;
                }

                WrappedDomainObject = value;

                if (ContractObject != null)
                {
                    ContractObject.PropertyChanging +=
                        value_PropertyChanging;
                    ContractObject.PropertyChanged += value_PropertyChanged;
                }

                OnPropertyChanged(() => ContractObject);
            }
        }

        public Contractdoc GeneralContractObject
        {
            get
            {
                return ContractObject.OriginalContract;
            }
        }

        void value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ERROR_CHANGED, Error);
        }

        void value_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            OnPropertyChanged("IsModified");
            OnPropertyChanged("Error");
        }

    }
}