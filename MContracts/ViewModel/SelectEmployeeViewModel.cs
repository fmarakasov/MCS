using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    public class SelectEmployeeViewModel: WorkspaceViewModel
    {

        private ObservableCollection<Department> _departments;
        private Department _department;
        private Employee _employee;
        private RelayCommand _okbuttoncommand;


        public RelayCommand OkButtonCommand
        {
            get { return _okbuttoncommand ?? (_okbuttoncommand = new RelayCommand(OkButtonPressed, CanOkPress)); }
        }

        private bool CanOkPress(object obj)
        {
            return SelectedEmployee != null;
        }

        private void OkButtonPressed(object obj)
        {
            
        }

       

        /// <summary>
        /// Получает доступ к коллекции отделов (только верхнего уровня)
        /// </summary>
        public ObservableCollection<Department> Departments
        {
            get
            {
                if (_departments == null)
                    _departments =
                        new ObservableCollection<Department>(
                            Repository.Departments.Where(x => x.Parent == null));
                return _departments;
            }
        }

        public Department SelectedDepartment
        {
            get { return _department;  }
            set
            {
                if (_department == value) return;
                _department = value;
                OnPropertyChanged("SelectedDepartment");
                if (_department != null)
                    SelectedEmployee = (_department.Employees.Count > 0) ? _department.Employees[0] : null;
                else
                    SelectedEmployee = null;
                
            }
        }

        public Employee SelectedEmployee
        {
            get { return _employee;  }
            set
            {
                if (_employee == value) return;
                _employee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public SelectEmployeeViewModel(IContractRepository repository): base(repository)
        {

        }

        public override bool IsClosable
        {
            get { return true; }
        }

        protected override void Save()
        {
        }


        protected override bool CanSave()
        {
            return true;
        }

    }
}
