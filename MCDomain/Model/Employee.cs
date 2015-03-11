using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.ComponentModel;
using CommonBase;

namespace MCDomain.Model
{
    partial class Employee : IObjectId, ICloneable, IDataErrorInfo, IEditableObject, IPerson //, IHierarchical
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        public override string ToString()
        {

            if (this.IsValidPersonName()&&this.Id != -1)
                return this.GetShortFullNameRev();
            return "<Нет данных>";
        }



        private EditableObject<Employee> _editable;
        private readonly DataErrorHandlers<Employee> _errorHandlers = new DataErrorHandlers<Employee>
                                                                       {
                                                                           new FamilyDataErrorHandler(),
                                                                           new RoleDataErrorHandler()
                                                                       };

        partial void OnCreated()
        {
            _editable = new EditableObject<Employee>(this);
        }

        public object Clone()
        {
            return null;
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

        /// <summary>
        /// Фамилия
        /// </summary>
        public String Family
        {
            get { return Familyname; }
            set { Familyname = value; }
        }

        private class FamilyDataErrorHandler : IDataErrorHandler<Employee>
        {
            #region IDataErrorHandler<Employee> Members

            public string GetError(Employee source, string propertyName, ref bool handled)
            {
                if (propertyName == "Family")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Employee source, out bool handled)
            {
                handled = false;
                if (String.IsNullOrEmpty(source.Family))
                {
                    handled = true;
                    return "Фамилия не может быть пустой!";
                }
                return string.Empty;
            }
        }

        #endregion

        private class RoleDataErrorHandler : IDataErrorHandler<Employee>
        {
            #region IDataErrorHandler<Employee> Members

            public string GetError(Employee source, string propertyName, ref bool handled)
            {
                if (propertyName == "Role")
                {
                    return HandleCurrencyError(source, out handled);
                }

                return string.Empty;
            }

            #endregion

            private static string HandleCurrencyError(Employee source, out bool handled)
            {
                handled = false;
                //if (source.Role == null)
                //{
                //    handled = true;
                //    return "Роль сотрудника не может быть пустой!";
                //}
                return string.Empty;
            }
        }

        

        public void BeginEdit()
        {
            _editable.BeginEdit();
        }

        public void CancelEdit()
        {
            _editable.CancelEdit();
        }

        public void EndEdit()
        {
            _editable.EndEdit();
        }

        public int Level
        {
            get
            {
                int level = 0;
                //Employee Parent = Employee1;
                //while (Parent != null)
                //{
                //    level++;
                //    Parent = Parent.Employee1;
                //}

                return level;
            }
        }

        public  Department Department
        {
            get { return Department_Departmentid;  }
            set
            {
                Department_Departmentid = value;
                SendPropertyChanged("Department");
            }
        }



        //public object Parent
        //{
        //    get { return Employee1; }
        //    set
        //    {
        //        Employee1 = value as Employee;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("Parent"));
        //            PropertyChanged(this, new PropertyChangedEventArgs("Level"));
        //        }
        //    }
        //}
    }
}
