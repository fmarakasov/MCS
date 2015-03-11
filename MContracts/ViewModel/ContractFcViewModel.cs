using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MediatorLib;
using MContracts.ViewModel.Helpers;
using MContracts.Classes;
using McUIBased.Commands;

namespace MContracts.ViewModel
{
    public class ContractFcViewModel : ContractdocBaseViewModel
    {
        private RelayCommand<Functionalcustomer> _addFunctionalCustomerCommand;
        private IBindingList _contractFunctionalCustomersBindingList;
        private ObservableCollection<Functionalcustomer> _functionalCustomers;
        private RelayCommand<Functionalcustomercontract> _removeFunctionalCustomerCommand;

        public ContractFcViewModel(IContractRepository repository) : base(repository)
        {
        }

        private McDataContext Context
        {
            get
            {
                var ctx = Repository as IContextProvider<McDataContext>;
                return (ctx == null) ? (null) : ctx.Context;
            }
        }

        public override string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Получает команду добавления функционального заказчика в список
        /// </summary>
        public ICommand AddFunctionalCustomerCommand
        {
            get
            {
                return _addFunctionalCustomerCommand ??
                       (_addFunctionalCustomerCommand = new RelayCommand<Functionalcustomer>(AddFunctionalCustomer));
            }
        }

        /// <summary>
        /// Получает команду удаления функционального заказчика из списка
        /// </summary>
        public ICommand RemoveFunctionalCustomerCommand
        {
            get
            {
                if (_removeFunctionalCustomerCommand == null)
                    _removeFunctionalCustomerCommand =
                        new RelayCommand<Functionalcustomercontract>(RemoveFunctionalCustomer,
                                                                     x =>
                                                                     CanRemoveFunctionalCustomer);
                return _removeFunctionalCustomerCommand;
            }
        }

        /// <summary>
        /// Получает коллекцию функциональных заказчиков договора
        /// </summary>
        public IBindingList ContractFunctionalCustomersBindingList
        {
            get
            {
                if (_contractFunctionalCustomersBindingList == null)
                {
                    _contractFunctionalCustomersBindingList =
                        ContractObject.Functionalcustomercontracts.GetNewBindingList();
                }
                return _contractFunctionalCustomersBindingList;
            }
        }

        /// <summary>
        /// Получает признак возможности добавления функционального заказчика
        /// </summary>
        public bool CanAddFunctionalCustomer
        {
            get { return true; }
        }

        /// <summary>
        /// Получает признак возможности удаления функционального заказчика
        /// </summary>
        public bool CanRemoveFunctionalCustomer
        {
            get { return true; }
        }

        /// <summary>
        /// Получает доступ к коллекции функциональных заказчиков (только верхнего уровня)
        /// </summary>
        public ObservableCollection<Functionalcustomer> FunctionalCustomers
        {
            get
            {
                if (_functionalCustomers == null)
                    _functionalCustomers =
                        new ObservableCollection<Functionalcustomer>(
                            Repository.Functionalcustomers.Where(x => x.Parent == null));
                return _functionalCustomers;
            }
        }

        
        private void AddFunctionalCustomer(Functionalcustomer functionalCustomer)
        {
            Contract.Requires(ContractObject != null);
            if (functionalCustomer == null) return;
            if (!ContractObject.Functionalcustomercontracts.Any(x => x.Functionalcustomer == functionalCustomer))
            {

                var newFc = new Functionalcustomercontract();
                newFc.Contractdocid = ContractObject.Id;
                newFc.Functionalcustomerid = functionalCustomer.Id;                                         
                newFc.Customer = functionalCustomer;                                
                ContractFunctionalCustomersBindingList.Add(newFc);
                
            }
        }

        private void RemoveFunctionalCustomer(Functionalcustomercontract functionalCustomerContract)
        {
            Contract.Requires(ContractObject != null);

            if (functionalCustomerContract == null) return;
            if (ContractFunctionalCustomersBindingList.Contains(functionalCustomerContract))
            {                
                ContractFunctionalCustomersBindingList.Remove(functionalCustomerContract);                
            }
        }

        protected override void Save()
        {
            //Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return true;
        }

        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof(CatalogType))]
        public void CatalogChanged(CatalogType c)
        {
            if (c == CatalogType.Functionalcustomer)
            {
                _functionalCustomers = null;
                Repository.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, Repository.Functionalcustomers);
                OnPropertyChanged("FunctionalCustomers");
            }
        }
    }
}