using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    public partial class Responsible: IUnderResponsibility, IDataErrorInfo
    {
        #region IDataErrorInfo

        private readonly DataErrorHandlers<Responsible> _errorHandlers = new DataErrorHandlers<Responsible>
                                                                       {
                                                                           new Responsible.FillingErrorHandler()
                                                                       };

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        public string Error
        {
            get { return this.Validate(); }
        }

        private class FillingErrorHandler : IDataErrorHandler<Responsible>
        {

            public string GetError(Responsible source, string propertyName, ref bool handled)
            {
                if (propertyName == "Employee"||propertyName=="Employeeid")
                {
                    return HandleEmployeeFilled(source, out handled);
                }

                if (propertyName == "Role"||propertyName=="Roleid")
                {
                    return HandleRoleFilled(source, out handled);
                }

 
                return string.Empty;
            }

            private string HandleEmployeeFilled(Responsible source, out bool handled)
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

            private string HandleRoleFilled(Responsible source, out bool handled)
            {
                handled = false;

                if (source.Role == null)
                {
                    handled = true;
                    return "Выберите роль";
                }

                return string.Empty;
            }

        }
        #endregion

        #region IUnderResponsibleMembers

        public void SendResponsiblesBindingListChanged()
        {
            SendPropertyChanged("ResponsiblesBindingList");
            SendPropertyChanged("DisposalPersons");
        }

        public void RemoveResponsiblesForDisposal(Disposal disposal)
        {
            
        }

        /// <summary>
        /// все виды ответственных заданы
        /// </summary>
        /// <param name="underResponsibility"></param>
        /// <returns></returns>
        public static bool IsComplete(IUnderResponsibility underResponsibility)
        {
            return (underResponsibility != null&&
                     ((underResponsibility.Responsibles.FirstOrDefault(p => p.Role != null && p.Role.Id == (long)WellKnownRoles.ContractChief) != null)&&
                      (underResponsibility.Responsibles.FirstOrDefault(p => p.Role != null && p.Role.Id == (long)WellKnownRoles.Director) != null) &&
                       (underResponsibility.Responsibles.FirstOrDefault(p => p.Role != null && p.Role.Id == (long)WellKnownRoles.Manager) != null) &&
                       (underResponsibility.Responsibles.FirstOrDefault(p => p.Role != null && p.Role.Id == (long)WellKnownRoles.DepartmentResponsible) != null) &&
                       (underResponsibility.Responsibles.FirstOrDefault(p => p.Role != null && p.Role.Id == (long)WellKnownRoles.Curator) != null)));

        }

        private IBindingList _responsiblebindinglist;
        public IBindingList ResponsiblesBindingList
        {
            get 
            {
                if (_responsiblebindinglist == null)
                    _responsiblebindinglist = Disposal.Responsibles.GetNewBindingList();

                return _responsiblebindinglist;
            }
        }

        public void RefreshRespBindingList()
        {
            _responsiblebindinglist = null;
        }

        public string GetResponsibleNameForReports()
        {
            
            return (Disposal != null) ? (Stage != null) ?
                Disposal.GetResponsibleNamesForReports(Stage) : 
                Disposal.GetResponsibleNamesForReports(Contractdoc) : null;
        }



        /// <summary>
        /// руководитель договора (промгаз)
        /// </summary>
        public Responsible Chief
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Disposal.GetChief(Stage) : Disposal.GetChief(Contractdoc) : null;
            }
        }

        public IEnumerable<Responsible> Chiefs
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Disposal.GetChiefs(Stage) : Disposal.GetChiefs(Contractdoc) : null;
            }
        }

        /// <summary>
        /// руководитель направления - ответственный
        /// </summary>
        public Responsible Manager
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Disposal.GetManager(Stage) : Disposal.GetManager(Contractdoc) : null;
            }
        }

        /// <summary>
        /// замдир - ответственный
        /// </summary>
        public Responsible Director
        {
            get
            {
                 return (Disposal != null) ? (Stage != null) ? Disposal.GetDirector(Stage) : Disposal.GetDirector(Contractdoc) : null;
            }
        }


        /// <summary>
        /// ответственный по договорам
        /// </summary>
        public Responsible Curator
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Disposal.GetCurator(Stage) : Disposal.GetCurator(Contractdoc) : null;
            }
        }


        /// <summary>
        /// ответственный по договорам
        /// </summary>
        public Responsible OrderSuperviser
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Disposal.GetOrderSuperviser(Stage) : Disposal.GetOrderSuperviser(Contractdoc) : null;
            }
        }




        /// <summary>
        /// отдел, за которым договор закреплен по распоряжению
        /// </summary>
        public Department DisposalDepartment
        {
            get { return (Chief != null) ? Chief.Employee.Department : null; }
        }

        public System.Data.Linq.EntitySet<Responsible> Responsibles
        {
            get { return Disposal.Responsibles;  }
        }

        public string DirectorsAndChiefs(bool includeordersuperviser)
        {
            return (Disposal != null) ? (Stage != null) ? Stage.DirectorsAndChiefs(includeordersuperviser) : Contractdoc.DirectorsAndChiefs(includeordersuperviser) : null; 
        }

        public Employee ChiefEmployee
        {
            get { return Chief != null ? Chief.Employee : null; }
        }

        public Employee ManagerEmployee
        {
            get { return Manager != null ? Manager.Employee : null; }
        }

        public Employee DirectorEmployee
        {
            get { return Director != null ? Director.Employee : null; }
        }

        public Employee CuratorEmployee
        {
            get { return Curator != null ? Curator.Employee : null; }

        }

        public Employee OrderSuperviserEmployee
        {
            get { return OrderSuperviser != null ? OrderSuperviser.Employee : null; }
        }

        public bool IsManagerVisible
        {
            get { return !(ManagerEmployee != null && ChiefEmployee != null) || ManagerEmployee.Id != ChiefEmployee.Id; }
        }

        public string DisposalPersons
        {
            get
            {
                var sb = new StringBuilder();

                if (ChiefEmployee != null)
                    sb.Append(string.Format("{0}: {1}", "Руководитель", ChiefEmployee));

                if (IsManagerVisible && ManagerEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "рук. напр.", ManagerEmployee));
                }

                if (DirectorEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "зам. дир.", DirectorEmployee));
                }

                if (OrderSuperviserEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "отв. по договорам", OrderSuperviserEmployee));
                }

                if (CuratorEmployee != null)
                {
                    sb.Append("; ");
                    sb.Append(string.Format("{0}: {1}", "куратор", CuratorEmployee));
                }
                return sb.ToString();

            }
        }

        #endregion

        partial void OnCreated()
        {

        }


        public string Directors
        {
            get
            {
                return (Disposal != null) ? (Stage != null) ? Stage.Directors : Contractdoc.Directors : null; 
            }
        }
    }
}
