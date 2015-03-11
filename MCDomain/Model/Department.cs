using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{

    public  enum WellKnownDepartments
    {
        [Description("Не определён")]
        Undefined = -1,
        [Description("Планово-экономический отдел")]
        ContractDepartment = 0
    }

    partial class Department : IObjectId, INamed, ICloneable, IDataErrorInfo,IHierarchical
    {

        public bool IsWellKnownId()
        {
            var en = Enum.GetValues(typeof(WellKnownDepartments));
            foreach (var ch in en)
            {
                if ((WellKnownDepartments)Id == (WellKnownDepartments)ch) return true;
            }
            return false;
        }


        /// <summary>
        /// Получает известный отдел
        /// </summary>
        public WellKnownDepartments WellKnownType
        {
            get
            {
                if (IsWellKnownId())
                    return (WellKnownDepartments)Id;
                return WellKnownDepartments.Undefined;
            }
        }

        /// <summary>
        /// Получает или устанавливает руководителя отдела
        /// </summary>
        public Employee Manager
        {
            get
            {
                if (Employee_Managerid != null)
                    return Employee_Managerid;
                else
                {
                    return  (Parent != null)?(Parent as Department).Manager: null;
                }
            }
            set
            {
                if (value == Employee_Managerid) return;
                SendPropertyChanging("Manager");
                Employee_Managerid = value;
                SendPropertyChanged("Manager");
            }
        }
        /// <summary>
        /// Получает или устанавливает зам. директора
        /// </summary>
        public Employee Director
        {
            get { return Employee_Directedbyid; }
            set
            {
                if (value == Employee_Directedbyid) return;
                Employee_Directedbyid = value;
                SendPropertyChanged("Director");
            }
        }



         public override string ToString()
        {
            return Name;
        }

        private readonly DataErrorHandlers<Department> _errorHandlers = new DataErrorHandlers<Department>
                                                                       {
                                                                           new NameDataErrorHandler()
                                                                       };

        public object Clone()
        {
            return new Department()
            {
                Name = this.Name,
                Manager = this.Manager,
                Director = this.Director,
                Parent = this.Parent
            };
        }

        public string Error
        {
            get { return this.Validate(); }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        #region Nested type: FamilyDataErrorHandler

        private class NameDataErrorHandler : IDataErrorHandler<Department>
        {
            #region IDataErrorHandler<Employee> Members

            public string GetError(Department source, string propertyName, ref bool handled)
            {
                if (propertyName == "Name")
                {
                    return HandleNameError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleNameError(Department source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Name))
                {
                    handled = true;
                    return "Наименование не может быть пустым!";
                }
                return string.Empty;
            }
        }

        #endregion

    

        public int Level
        {
            get
            {
                int level = 0;
                Department pd = Department_Parentid;
                while (pd != null)
                {
                    level++;
                    pd = (Parent as Department).Department_Parentid;
                }

                return level;
            }
        }

        public object Parent
        {
            get { return Department_Parentid; }
            set
            {
                Department_Parentid = value as Department;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Parent"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                }
            }
        }

        partial void OnCreated()
        {
            
        }
        
        public IEnumerable<Responsiblefororder> RealResponsiblefororders
        {
            get
            {
                var d = this;
                while (d != null)
                {
                  if (d.Responsiblefororders.Count > 0) return d.Responsiblefororders;
                  d = d.Parent as Department;
                }
                return Responsiblefororders;
            }
        }

        public  EntitySet<Department> Departments
        {
            get { return Departments_Parentid; }
        }

        public IList<Employee> Employees
        {
            get { return Employees_Departmentid.OrderBy(s => s.Familyname).ToList(); }
        }

        public IEnumerable<Employee> RealEmployees
        {
            get
            {

                var emps = new List<Employee>();
                emps.AddRange(this.Employees);

                if (this.Departments.Count == 0)
                {
                    if (Parent != null)
                        emps.AddRange((Parent as Department).RealEmployees);


                }
                else
                {

                    foreach (var d in Departments)
                    {
                        emps.AddRange(d.Employees);
                    }


                }

                return emps.Distinct();
    
            }

        }



    }
    
}
