using System.ComponentModel;
using System.Data.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using MContracts.ViewModel.Helpers;
using MediatorLib;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    public class OrderRepositoryViewModel : WorkspaceViewModel
    {
        private RelayCommand _deleteOrderCommand;
        private RelayCommand _editOrderCommand;
        private RelayCommand _newOrderCommand;
        private IBindingList _orders;

        public OrderRepositoryViewModel(IContractRepository repository) : base(repository)
        {
            SelectedResponsibleassignmentorder = null;
            ViewMediator.Register(this);
        }

        public IBindingList Orders
        {
            get
            {
                if (_orders == null)
                {
                    _orders = new BindingList<Responsibleassignmentorder>();
                    foreach (Responsibleassignmentorder r in Repository.Responsibleassignmentorders)
                    {
                        _orders.Add(r);
                    }
                }
                return _orders;
            }
        }

        public Responsibleassignmentorder SelectedResponsibleassignmentorder { get; set; }

        public override bool IsClosable
        {
            get { return true; }
        }

        public override string DisplayName
        {
            get { return "Приказы"; }
        }

        public bool CanCreateOrder
        {
            get { return Orders.Count == 0; }
        }

        public RelayCommand NewOrderCommand
        {
            get
            {
                return _newOrderCommand ??
                       (_newOrderCommand = new RelayCommand(x => CreateOrder(), x => CanCreateOrder));
            }
        }

        public bool CanEditOrder
        {
            get { return (SelectedResponsibleassignmentorder != null); }
        }

        public RelayCommand EditOrderCommand
        {
            get { return _editOrderCommand ?? (_editOrderCommand = new RelayCommand(x => EditOrder(), x => CanEditOrder)); }
        }

        public bool CanDeleteOrder
        {
            get { return (SelectedResponsibleassignmentorder != null); }
        }

        public RelayCommand DeleteOrderCommand
        {
            get
            {
                return _deleteOrderCommand ??
                       (_deleteOrderCommand = new RelayCommand(x => DeleteOrder(), x => CanDeleteOrder));
            }
        }

        public void SendPropertyChanged(string propertyName)
        {
        }

        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof (CatalogType))]
        public void CatalogChanged(CatalogType c)
        {
            if (c == CatalogType.Employee)
            {
                Repository.Refresh(RefreshMode.OverwriteCurrentValues, Repository.Employees);
            }
        }

        [MediatorMessageSink(RequestRepository.ORDERS_CHANGED, ParameterType = typeof (object))]
        public void ReloadOrders(object o)
        {
            _orders = null;
            SendPropertyChanged("Orders");
        }

        protected override void Save()
        {
        }

        protected override bool CanSave()
        {
            return false;
        }

        public void InitViewModel(AddOrderWindow dlg)
        {
            dlg.ViewModel.Order = new Responsibleassignmentorder();
        }


        public void CreateOrder()
        {
            var dlg = new AddOrderWindow(Repository);
            InitViewModel(dlg);
            if (dlg.ShowDialog() == true)
            {
                dlg.SaveOrder();
                Orders.Add(dlg.ViewModel.Order);
                Repository.InsertResponsibleassignmentorder(dlg.ViewModel.Order);
                Repository.SubmitChanges();
                SendPropertyChanged("Orders");
            }

        }

        public void EditOrder()
        {
            var dlg = new AddOrderWindow(Repository);

            InitViewModel(dlg);
            dlg.ViewModel.Order = SelectedResponsibleassignmentorder;
            if (dlg.ShowDialog() == true)
            {
                dlg.SaveOrder();
                Repository.DebugPrintRepository();
                Repository.SubmitChanges();
                SendPropertyChanged("Orders");
            }
        }

        public void DeleteOrder()
        {
            Repository.DeleteResponsibleassignmentorder(SelectedResponsibleassignmentorder);
            Orders.Remove(SelectedResponsibleassignmentorder);
            Repository.SubmitChanges();
            SendPropertyChanged("Orders");
        }
    }
}