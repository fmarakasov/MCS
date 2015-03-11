using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MCDomain.Common;
using System.Diagnostics.Contracts;
using CommonBase;

namespace MCDomain.Model
{
    partial class Disposal : IObjectId, IDataErrorInfo
    {

        #region IDataErrorInfo Members

            private readonly DataErrorHandlers<Disposal> _errorHandlers = new DataErrorHandlers<Disposal>
                                                                           {
                                                                               new NumberDataErrorHandler(),
                                                                           };


            public string this[string columnName]
            {
                get { return _errorHandlers.HandleError(this, columnName); }
            }

            public string Error
            {
                get { return this.Validate(); }
            }

            #region Nested type: NameDataErrorHandler

            private class NumberDataErrorHandler : IDataErrorHandler<Disposal>
            {

                public string GetError(Disposal source, string propertyName, ref bool handled)
                {
                    if (propertyName == "Num")
                    {
                        return HandleNumError(source, out handled);
                    }

                    HandleResponsibles(source, out handled);

                    return string.Empty;
                }

                private static string HandleNumError(Disposal source, out bool handled)
                {
                    //handled = false;
                    //if (string.IsNullOrEmpty(source.Num))
                    //{
                    //    handled = true;
                    //    return "Номер распоряжения не может быть пустым!";
                    //}


                    handled = true;
                    return string.Empty;
                }


                private static string HandleResponsibles(Disposal source, out bool handled)
                {
                    handled = false;
                    foreach (var r in source.Responsibles)
                    {
                        if (r.Error != String.Empty)
                        {
                            handled = true;
                            return r.Error;
                        }
                    }

                    handled = true;
                    return string.Empty;
                }

            }
            #endregion
        #endregion

            public bool IsWellKnownId()
            {
                return false;
            }


            partial void OnCreated()
            {
                this.Approveddate = DateTime.Today;
            }

            public override string ToString()
            {

                return  Num.Return(x => (Num.Trim() != string.Empty ? "№ " + Num + (Approveddate.HasValue?" от ":string.Empty):string.Empty), string.Empty) + 
                        (Approveddate.HasValue?(Approveddate.Value.ToString("dd.MM.yyyy")):string.Empty);
            }


            public IEnumerable<Contractdoc> Contractdocs
            {
                get { return Responsibles.Where(p=>p.Contractdoc!=null).Select(p => p.Contractdoc).Distinct(); }
            }
            
            public IEnumerable<Stage> Stages
            {
                get { return Responsibles.Where(p => p.Stage != null).Select(p => p.Stage).Distinct(); }
            }

            public string ContractdocNums
            {
                get
                {
                    if (Contractdocs != null)
                        return Contractdocs.Aggregate(string.Empty, (r, next) => r + next.Num + ";");
                    else return null;
                }
            }



            public string GetResponsibleNamesForReports(IUnderResponsibility o)
            {
                Contract.Assert(o != null, "Объект должен быть задан");

                var sb = new StringBuilder();
                if (o.DirectorEmployee != null) sb.AppendLine(o.DirectorEmployee.ToString());
                if (o.ManagerEmployee != null && o.ManagerEmployee != o.DirectorEmployee) sb.AppendLine(o.ManagerEmployee.ToString());
                if (o.ChiefEmployee != null && o.ChiefEmployee != o.ManagerEmployee && o.ChiefEmployee != o.DirectorEmployee)
                    sb.AppendLine(o.ChiefEmployee.ToString());
                if (o.OrderSuperviserEmployee != null && o.OrderSuperviserEmployee != o.ChiefEmployee && o.OrderSuperviserEmployee != o.ManagerEmployee && o.OrderSuperviserEmployee != o.DirectorEmployee)
                    sb.AppendLine(o.OrderSuperviserEmployee.ToString());

                return sb.ToString();
            }

            private Responsible GetRoleForContractdoc(Contractdoc contractdoc, WellKnownRoles role)
            {
                Contract.Assert(contractdoc != null, "Договор должен быть задан");
                return Responsibles.FirstOrDefault(p => (p.Role != null && p.Role.Id == (long)role && p.Contractdoc != null && p.Contractdoc.Id == contractdoc.Id && p.Stage == null));
            }
            
            



            private  Responsible GetRoleForStage(Stage stage, WellKnownRoles role)
            {
                Contract.Assert(stage != null, "Этап должен быть задан");
                // проверяем есть ли собственная такая роль

                var r = Responsibles.FirstOrDefault(p => (p.Role != null && p.Role.Id == (long)role && (p.Stage != null && p.Stage.Id == stage.Id)))??
                        GetRoleForContractdoc(stage.ContractObject, role);

                return r;

            }

            public Responsible GetChief(Contractdoc contractdoc)
            {
                return GetRoleForContractdoc(contractdoc, WellKnownRoles.ContractChief);
            }
            
            public Responsible GetChief(Stage stage)
            {
                return GetRoleForStage(stage, WellKnownRoles.ContractChief);
            }

            public IEnumerable<Responsible> GetChiefs(Contractdoc contractdoc)
            {
                return Responsibles.Where(p => p.Role.Id == (long)WellKnownRoles.ContractChief&&p.Contractdoc!=null&&p.Contractdoc.Id == contractdoc.Id&&p.Stage == null);
            }

            public IEnumerable<Responsible>  GetChiefs(Stage stage)
            {
                return Responsibles.Where(p => p.Role.Id == (long)WellKnownRoles.ContractChief && (p.Stage != null&&p.Stage.Id == stage.Id));
            }

            public Responsible GetManager(Contractdoc contractdoc)
            {
                return GetRoleForContractdoc(contractdoc, WellKnownRoles.Manager);
            }

            public Responsible GetManager(Stage stage)
            {
                return GetRoleForStage(stage, WellKnownRoles.Manager);
            }

            public Responsible GetDirector(Contractdoc contractdoc)
            {
                return GetRoleForContractdoc(contractdoc, WellKnownRoles.Director);
            }

            public Responsible GetDirector(Stage stage)
            {
                return GetRoleForStage(stage, WellKnownRoles.Director);
            }

            public Responsible GetCurator(Contractdoc contractdoc)
            {
                return GetRoleForContractdoc(contractdoc, WellKnownRoles.Curator);
            }

            public Responsible GetCurator(Stage stage)
            {
                return GetRoleForStage(stage, WellKnownRoles.Curator);
            }

            public  Responsible GetOrderSuperviser(Contractdoc contractdoc)
            {
                return GetRoleForContractdoc(contractdoc, WellKnownRoles.DepartmentResponsible);
            }

            public Responsible GetOrderSuperviser(Stage stage)
            {
                return GetRoleForStage(stage, WellKnownRoles.DepartmentResponsible);
            }



     }
}
