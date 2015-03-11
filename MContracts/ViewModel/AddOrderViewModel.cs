using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Classes;
using CommonBase;
using CommonBase;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    public class ResponsibleForOrderEntity: IDataErrorInfo
    {

        #region Nested type: ErrorHandlers

        private class DepartmentErrorHandler : IDataErrorHandler<ResponsibleForOrderEntity>
        {

            public string GetError(ResponsibleForOrderEntity source, string propertyName, ref bool handled)
            {
                if (propertyName == "Department")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(ResponsibleForOrderEntity source, out bool handled)
            {
                handled = false;
                if (source.Department == null)
                {
                    handled = true;
                    return "Выберите отдел";
                }


                handled = true;
                return string.Empty;
            }
        }

        private class EmployeeErrorHandler : IDataErrorHandler<ResponsibleForOrderEntity>
        {

            public string GetError(ResponsibleForOrderEntity source, string propertyName, ref bool handled)
            {
                if (propertyName == "Employee")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            private static string HandleCurrencyError(ResponsibleForOrderEntity source, out bool handled)
            {
                handled = false;
                if (source.Employee == null)
                {
                    handled = true;
                    return "Выберите сотрудника";
                }


                handled = true;
                return string.Empty;
            }
        }
        #endregion

        private readonly DataErrorHandlers<ResponsibleForOrderEntity> _errorHandlers = new DataErrorHandlers<ResponsibleForOrderEntity>
                                                                       {
                                                                           new DepartmentErrorHandler(),
                                                                           new EmployeeErrorHandler()
                                                                       };
        public double Id { get; set; }

        public Department Department { get; set; }
        public Employee Employee { get; set;  }
        public Contracttype Contracttype { get; set; }


        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        public string Error
        {
            get { return this.Validate(); }
        }
    }

    public class AddOrderViewModel : WorkspaceViewModel
    {

        public RelayCommand OkPressedAction
        {
            get { return _okpressedaction ?? (_okpressedaction = new RelayCommand(PressOK, CanPressOk)); }
        }

        private bool CanPressOk(object obj)
        {
            return (Order.Validate() == string.Empty)&&(ResponsibleForOrderEntities.Cast<ResponsibleForOrderEntity>().Select(p => p.Validate()).Where(s => s != string.Empty).Count() == 0);
        }

        private void PressOK(object obj)
        {

        }

        public AddOrderViewModel(IContractRepository repository) : base(repository)
        {
            
        }

        public override bool IsClosable
        {
            get { return true; }
        }

        protected override void Save()
        {
            // удаляем из респонсиблов каждый удаленный из DTO коллекции
            ResponsibleForOrderEntity re = null;
            Responsiblefororder r = null;
            for (int i = Order.Responsiblefororders.Count - 1; i >= 0; i--)
            {
                r = Order.Responsiblefororders[i];
                re = ResponsibleForOrderEntities.Where<ResponsibleForOrderEntity>(p => p.Id == r.Id).FirstOrDefault() as ResponsibleForOrderEntity;
                if (re == null) Order.Responsiblefororders.Remove(r);
            }

            // добавляем или обновляем каждый добавленный
            Responsiblefororder ru = null;
            foreach (ResponsibleForOrderEntity reu in ResponsibleForOrderEntities)
            {
                ru = Order.Responsiblefororders.FirstOrDefault(p => (p.Id == reu.Id && reu.Id != 0));
                if (ru == null)
                {
                    ru = new Responsiblefororder() { Responsibleassignmentorder =  Order, Employee = reu.Employee, Department =  reu.Department, Contracttype = reu.Contracttype};
                    Order.Responsiblefororders.Add(ru);
                }
                else
                {
                    ru.Employee = reu.Employee;
                    ru.Department = reu.Department;
                    ru.Contracttype = reu.Contracttype;
                }
            }    
        }

        protected override bool CanSave()
        {
            return true;
        }

        private Responsibleassignmentorder _order;
        /// <summary>
        /// редактируемый приказ
        /// </summary>
        public Responsibleassignmentorder Order
        {
            get { return _order; }
            set
            {
                if (_order == value) return;
                _order = value;
                OnPropertyChanged("Order");
            }
        }

        private IBindingList _responsiblefororderentities;
        private ResponsibleForOrderEntity _selectedresponsiblefororder;

        /// <summary>
        /// набор ответственных по приказу (DTO)
        /// </summary>
        public IBindingList ResponsibleForOrderEntities
        {
            get
            {
                if (_responsiblefororderentities == null)
                {
                    _responsiblefororderentities = new BindingList<ResponsibleForOrderEntity>();
                    if (_order != null)
                    {
                        foreach (Responsiblefororder r in _order.Responsiblefororders)
                        {
                            var rr = new ResponsibleForOrderEntity { Id = r.Id, Department = r.Department, Employee = r.Employee, Contracttype = r.Contracttype};
                            _responsiblefororderentities.Add(rr);
                        }
                    }
                    OnPropertyChanged("ResponsibleForOrderEntities");
                }
                return _responsiblefororderentities;
            }
        }

        /// <summary>
        /// выбранный ответственный
        /// </summary>
        public ResponsibleForOrderEntity SelectedResponsibleForOrder
        {
            get { return _selectedresponsiblefororder; }
            set
            {
                if (_selectedresponsiblefororder == value) return;
                _selectedresponsiblefororder = value;
                OnPropertyChanged("SelectedResponsibleForOrder");
                OnPropertyChanged("SelectedDepartment");
                OnPropertyChanged("DepartmentEmployees");
            }

        }

        private RelayCommand _createResponsibleForOrderCommand;
        /// <summary>
        /// добавить ответственного по приказу
        /// </summary>
        public RelayCommand CreateResponsibleForOrderCommand
        {
            get
            {
                return _createResponsibleForOrderCommand ??
                          (_createResponsibleForOrderCommand = new RelayCommand(CreateResponsibleForOrder, x => true));
            }
        }

        public void CreateResponsibleForOrder(object o)
        {
       
            var r = new ResponsibleForOrderEntity() {Contracttype = Contracttypes.GetReservedUndefined()};
            ResponsibleForOrderEntities.Add(r);
            SelectedResponsibleForOrder = r;
            OnPropertyChanged("ResponsibleForOrderEntities");
        }

        
        private RelayCommand _deleteResponsibleForOrderCommand;
        private RelayCommand _okpressedaction;

        /// <summary>
        /// Удалить ответственного по приказу
        /// </summary>
        public RelayCommand DeleteResponsibleForOrderCommand
        {
            get
            {
                return _deleteResponsibleForOrderCommand ?? (_deleteResponsibleForOrderCommand = new RelayCommand(DeleteResponsibleForOrder, x => CanDeleteResponsibleForOrder));
            }

        }

        public void DeleteResponsibleForOrder(object o)
        {
            ResponsibleForOrderEntities.Remove(SelectedResponsibleForOrder);
            SelectedResponsibleForOrder = null;
        }

        public bool CanDeleteResponsibleForOrder
        {
            get
            {
                return (SelectedResponsibleForOrder != null);
            }
        }


        /// <summary>
        /// отделы, по которым еще не определены ответственные
        /// </summary>
        public IEnumerable<Department> Departments
        {
            get 
            {
                return Repository.Departments.Where(d => (ResponsibleForOrderEntities.Where<ResponsibleForOrderEntity>(p => p.Department == d&&p.Contracttype.Id == EntityBase.ReservedUndifinedOid).Count() == 0) || d == SelectedDepartment);
            }
        }

        public IEnumerable<Contracttype> Contracttypes
        {
            get { return Repository.Contracttypes.Where(c => (ResponsibleForOrderEntities.Where<ResponsibleForOrderEntity>(p => p.Contracttype == c&&p.Department == SelectedDepartment).Count() == 0) || c == SelectedContracttype);}
        }

        /// <summary>
        /// выбранный департамент
        /// </summary>
        public Department SelectedDepartment
        {
            get
            {
                return SelectedResponsibleForOrder != null ? SelectedResponsibleForOrder.Department : null;
            }
        }
        
        public Contracttype SelectedContracttype
        {
            get
            {
                return SelectedResponsibleForOrder != null?SelectedResponsibleForOrder.Contracttype : null;
            }
        }
        
        /// <summary>
        /// сотрудники выбранного департамента
        /// </summary>
        public IEnumerable<Employee> DepartmentEmployees
        {
            get {
                
                   return SelectedDepartment != null ? SelectedDepartment.RealEmployees : null; 
                }
        }
        
        public void SendPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }
    }
}
