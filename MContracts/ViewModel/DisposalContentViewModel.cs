using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Controls.Dialogs;
using CommonBase;
using UIShared.ViewModel;
using MCDomain.Common;

namespace MContracts.ViewModel
{

    public class StageResponsibleDTO : IDataErrorInfo
    {

        public Disposal Disposal { get; set; }

        private Schedulecontract _schedulecontract;
        public Schedulecontract Schedulecontract
        {
            get
            {
                return _schedulecontract;
            }
            set
            {
                _schedulecontract = value;
            }
        }

        private Stage _stage;
        public Stage Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;
               
            }
        }

        //private IBindingList _responsiblesbindinglist;
        //public IBindingList ResponsiblesBindingList
        //{
        //    get
        //    {
                
        //        if (Stage == null) return null;

        //        if (_responsiblesbindinglist == null)
        //        {
        //            var ds = Stage.Responsibles.Where(r => r.Disposal.Id == Disposal.Id);
        //            _responsiblesbindinglist = new BindingList<Responsible>();
        //            foreach (var responsible in ds)
        //            {
        //                _responsiblesbindinglist.Add(responsible);
        //            }

        //        }
        //        return _responsiblesbindinglist;
        //    }
        //}

        #region IDataErrorInfo

        private readonly DataErrorHandlers<StageResponsibleDTO> _errorHandlers = new DataErrorHandlers<StageResponsibleDTO>
                                                                       {
                                                                           new StageResponsibleDTO.FillingErrorHandler()
                                                                       };

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        public string Error
        {
            get { return this.Validate(); }
        }

        private class FillingErrorHandler : IDataErrorHandler<StageResponsibleDTO>
        {

            public string GetError(StageResponsibleDTO source, string propertyName, ref bool handled)
            {
                if (propertyName == "Schedulecontract")
                {
                    return HandleScheduleFilled(source, out handled);
                }

                if (propertyName == "Stage")
                {
                    return HandleStageFilled(source, out handled);
                }

                return HandleResponsibleFilled(source, out handled);
            }

            private string HandleStageFilled(StageResponsibleDTO source, out bool handled)
            {
                handled = false;
                if (source.Stage == null)
                {
                    handled = true;
                    return "Выберите этап";
                }
                handled = true;
                return string.Empty;

            }

            private string HandleResponsibleFilled(StageResponsibleDTO source, out bool handled)
            {
                handled = false;

                if (source.Stage.ResponsiblesBindingList != null && source.Stage.ResponsiblesBindingList.Cast<Responsible>().Any(e => e.Error != String.Empty))
                {
                    handled = true;
                    return "Ошибка при заполнении ответственных";
                }

                return string.Empty;
            }

            private static string HandleScheduleFilled(StageResponsibleDTO source, out bool handled)
            {
                handled = false;
                if (source.Schedulecontract == null)
                {
                    handled = true;
                    return "Укажите календарный план";
                }


                handled = true;
                return string.Empty;
            }

        }
        #endregion

    }

    public class ContractStageResponsibleDTO : IDataErrorInfo
    {
        public Contractdoc Contractdoc { get; set; }
        public Disposal Disposal { get; set; }
        private BindingList<StageResponsibleDTO> _stagerespdtobindinglist;
        public BindingList<StageResponsibleDTO> StageResponsibleDTOBindingList
        {
            get
            {
                if (_stagerespdtobindinglist == null && Contractdoc != null)
                {
                    _stagerespdtobindinglist = new BindingList<StageResponsibleDTO>();
                    StageResponsibleDTO srd;
                    foreach (var ss in Contractdoc.Schedulecontracts)
                    {

                        foreach (var s in ss.Schedule.Stages)
                        {
                            if (s.Responsibles.Count(r => r.Disposal == Disposal) > 0)
                            {
                                srd = _stagerespdtobindinglist.AddNew();
                                srd.Stage = s;
                                srd.Schedulecontract = ss;
                                srd.Disposal = Disposal;
                            }
                        }
                    }

                }

                return _stagerespdtobindinglist;

                
            }
        }


        #region IDataErrorInfo

        private readonly DataErrorHandlers<ContractStageResponsibleDTO> _errorHandlers = new DataErrorHandlers<ContractStageResponsibleDTO>
                                                                       {
                                                                           new ContractStageResponsibleDTO.FillingErrorHandler()
                                                                       };

        private class FillingErrorHandler : IDataErrorHandler<ContractStageResponsibleDTO>
        {


            public string GetError(ContractStageResponsibleDTO source, string propertyName, ref bool handled)
            {
                if (propertyName == "Contractdoc")
                {
                    return HandleContractdocFilled(source, out handled);
                }

                return string.Empty;
            }



            private string HandleContractdocFilled(ContractStageResponsibleDTO source, out bool handled)
            {
                handled = false;
                if (source.Contractdoc == null)
                {
                    handled = true;
                    return "Укажите договор";
                }
                handled = true;
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get { return _errorHandlers.HandleError(this, columnName); }
        }

        public string Error
        {
            get { return this.Validate(); }
        }

        #endregion
    }


    public class DisposalContentViewModel : ContractdocBaseViewModel
    {

        public DisposalContentViewModel(IContractRepository repository, ViewModelBase owner = null)
            : base(repository, owner)
        {

        }


        private BindingList<ContractStageResponsibleDTO> _contractstageresponsibledtolist;

        public BindingList<ContractStageResponsibleDTO> ContractStageResponsibleDTOList
        {
            get
            {
                if (_contractstageresponsibledtolist == null)
                {
                    _contractstageresponsibledtolist = new BindingList<ContractStageResponsibleDTO>();

                    IList _currentcontractdocs = Repository.Contracts.Where(
                                                        c => (ContractdocNumSubstring != string.Empty && c.Num != null && c.Num.Contains(ContractdocNumSubstring)) ||
                                                             (ContractObject != null && ContractObject.Id == c.Id) ||
                                                             (Disposal != null && Disposal.Contractdocs.Contains(c))).Distinct().ToList();
                    ContractStageResponsibleDTO d;
                    foreach (Contractdoc c in _currentcontractdocs)
                    {
                        d = _contractstageresponsibledtolist.AddNew();
                        d.Contractdoc = c;
                        d.Disposal = Disposal;
                    }
                }

                return _contractstageresponsibledtolist;
            }

        }



        public StageResponsibleDTO _currentstageresponsibledto;
        public StageResponsibleDTO CurrentStageResponsibleDTO
        {
            get { return _currentstageresponsibledto; }
            set
            {
                _currentstageresponsibledto = value;
                OnPropertyChanged("CurrentStageResponsibleDTO");
            }
        }

        #region Members of ContractocBaseViewModel;
        protected override void Save()
        {
            Repository.DebugPrintRepository();
            Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return Disposal.Error == string.Empty;
        }
        #endregion


        private void CopyOriginDisposal()
        {
            _disposal.Num = OriginDisposal.Num;
            _disposal.Approveddate = OriginDisposal.Approveddate;

            IList<Responsible> resps = OriginDisposal.Responsibles.Where(r => r.Stage == null && OriginDisposal.Responsibles.Select(rr => rr.Contractdocid).Contains(ContractObject.OriginalContract.Id)).ToList();
            foreach (var r in resps)
            {
                _disposal.Responsibles.Add(new Responsible() { Contractdoc = ContractObject, Disposal = this._disposal, Employee = r.Employee, Role = r.Role }); 
            }

            resps = OriginDisposal.Responsibles.Where(r => r.Stage != null && OriginDisposal.Responsibles.Select(rr => rr.Contractdocid).Contains(ContractObject.OriginalContract.Id)).Distinct().ToList();


            foreach (var r in resps)
            {
                var tschc = ContractObject.OriginalContract.Schedulecontracts.FirstOrDefault(ss => ss.Schedule.Stages.Any(sss => sss.Id == r.Stage.Id || sss.Num == r.Stage.Num || sss.Subject == r.Stage.Subject));
                if (tschc != null && tschc.Schedule != null)
                {
                    var schc = ContractObject.Schedulecontracts.FirstOrDefault(s => s.Appnum == tschc.Appnum);
                    if (schc != null && schc.Schedule != null)
                    {
                        var sc = schc.Schedule.Stages.FirstOrDefault(
                            ss => ss.Id == r.Stage.Id || ss.Num == r.Stage.Num || ss.Subject == r.Stage.Subject);

                        if (sc != null)
                        {
                            _disposal.Responsibles.Add(new Responsible()
                                                           {
                                                               Contractdoc = ContractObject,
                                                               Disposal = this._disposal,
                                                               Employee = r.Employee,
                                                               Role = r.Role,
                                                               Stage = sc
                                                           });
                        }
                    }
                }
            }
        }

        private Disposal _disposal;
        /// <summary>
        /// на вход поступает распоряжение, второй входной параметр - договор, он тут и так есть
        /// </summary>
        public Disposal Disposal
        {
            get
            {
                if (_disposal == null)
                {
                    _disposal = new Disposal();
                    if (OriginDisposal != null) CopyOriginDisposal();
                }
                return _disposal ;
            }

            set
            {
                _disposal = value;
                OnPropertyChanged("Disposal");
                OnPropertyChanged("Contractdocs");
            }
        }

        private Disposal _origindisposal;
        /// <summary>
        /// на вход может также поступить распоряжение, из которого следует скопировать этапы
        /// </summary>
        public Disposal OriginDisposal
        {
            get { return _origindisposal;  }

            set 
            { 
                _origindisposal = value;
                OnPropertyChanged("OriginDisposal");
            }
        }

        private ContractStageResponsibleDTO _contractstageresponsibledto;
        public ContractStageResponsibleDTO ContractStageResponsibleDto
        {
            get
            {
                if (_contractstageresponsibledto == null && ContractStageResponsibleDTOList != null)
                    _contractstageresponsibledto = ContractStageResponsibleDTOList.FirstOrDefault();
                return _contractstageresponsibledto;
            }
            set
            {

                if (CurrentContractStageResponsibleDTOBindingList != null)
                    (CurrentContractStageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>).ResetBindings();

                _contractstageresponsibledto = value;

                if (CurrentContractStageResponsibleDTOBindingList != null)
                    (CurrentContractStageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>).ResetBindings();
                CurrentStageResponsibleDTO = null;
                OnPropertyChanged("ContractStageResponsibleDto");
                OnPropertyChanged("CurrentContract");
                OnPropertyChanged("ContractStageResponsibleDTOList");
                OnPropertyChanged("CurrentStageResponsibleDTO");
                OnPropertyChanged("CurrentStage");
            }
        }

        public Contractdoc _contractdoc;
        /// <summary>
        /// текущий договор
        /// </summary>
        public Contractdoc CurrentContract
        {
            get
            {
                return ContractStageResponsibleDto != null ? ContractStageResponsibleDto.Contractdoc : null;
            }
        }







        /// <summary>
        /// переданы одни этапы
        /// </summary>
        public BindingList<Stage> AllStages
        {
            get
            {

                IList<Stage> selectedstages = (CurrentStageResponsibleDTO != null && CurrentStageResponsibleDTO.Schedulecontract != null) ?
                                      CurrentStageResponsibleDTO.Schedulecontract.Schedule.Stages.Where
                                      (s => (CurrentStage != null && s.Id == CurrentStage.Id) ||
                                      !_contractstageresponsibledto.StageResponsibleDTOBindingList.Select(ss => ss.Stage).Contains(s)).OrderBy(sss => sss.Num).ToList() : null;

                if (selectedstages != null)
                    selectedstages =
                        selectedstages.Where(
                            s =>
                            (CurrentStage != null && s.Id == CurrentStage.Id) || StageNumSubstring == string.Empty ||
                            StageNumSubstring != string.Empty && s.Num != null && s.Num.Contains(StageNumSubstring)).
                            ToList();
                
                if (CurrentStageResponsibleDTO != null && selectedstages != null && CurrentStageResponsibleDTO.Stage == null)
                {
                    CurrentStageResponsibleDTO.Stage = selectedstages.FirstOrDefault();
                    OnPropertyChanged(()=>CurrentStageResponsibleDTO);
                    OnPropertyChanged(() => CurrentStage);
                }

                var bl = new BindingList<Stage>();
                bl.AddRange(selectedstages);

                return bl;
            }
        }



        /// <summary>
        /// текущий этап
        /// </summary>
        public Stage CurrentStage
        {
            get
            {

                if (CurrentStageResponsibleDTO != null) return CurrentStageResponsibleDTO.Stage;
                else return null;
            }
        }

        public IUnderResponsibility ResponsibilityObject
        {
            get
            {
                IUnderResponsibility r = CurrentStage;
                if (r == null) r = CurrentContract;

                return r;
            }
        }

        /// <summary>
        /// роли
        /// </summary>
        public IList<Role> Roles
        {
            get
            {

                if (LastSelectedResponsible != null && LastSelectedResponsible.Role != null)
                    return
                        Repository.Roles.Where(
                            r =>
                            ((r.Id != (long) WellKnownRoles.Undfined) &&
                             (!ResponsibilityObject.Responsibles.Select(o => o.Role).Distinct().Contains(r))) ||
                            (r.Id == LastSelectedResponsible.Role.Id)).ToList();
                else 
                    return
                        Repository.Roles.Where(
                            r =>
                            ((r.Id != (long)WellKnownRoles.Undfined) &&
                             (!ResponsibilityObject.Responsibles.Select(o => o.Role).Distinct().Contains(r)))).ToList();

            }
        }

        public IList<Employee> Employees
        {
            get
            {

                if (_responsible == null)
                    return Repository.Employees;
                else
                {
                    if (_responsible.Role != null && _responsible.Role.Id == (long)WellKnownRoles.Curator)
                       return
                            Repository.Employees.Where(
                                e => (e.Department != null && e.Department.Id == (long)WellKnownDepartments.ContractDepartment)||e.Id == EntityBase.ReservedUndifinedOid).ToList();
                    //это не работает, потому что нет такой должности зам.дир в справочнике
                    else if (_responsible.Role != null && _responsible.Role.Id == (long)WellKnownRoles.Director)
                    {
                        var dd = Repository.Departments.Where(d => d.Director != null);
                        return Repository.Employees.Where(
                                          e => (dd.Any(d => d.Director.Id == e.Id) || (e.Post != null && e.Post.WellKnownType == WellKnownPosts.Director) || e.Id == _responsible.Employee.Id || e.Id == EntityBase.ReservedUndifinedOid)).Distinct().ToList();
                    }
                    else if (_responsible.Role != null && _responsible.Role.Id == (long)WellKnownRoles.DepartmentResponsible)
                    {
                        if (ResponsibilityObject.Chief != null && ResponsibilityObject.Chief.DisposalDepartment != null)
                        {
                            IList<Employee> es = Repository.Employees.Where(
                                    e => (e.Department != null &&
                                          Repository.Responsiblefororders.Where(
                                              r => ResponsibilityObject.Chief.DisposalDepartment.Id == e.Department.Id).
                                              Select(p => p.Employee).Contains(e)) || (e.Department == null)).ToList();

                            if (es.Count() > 0)
                                return
                                    Repository.Employees.Where(
                                        r => es.Contains(r) || r.Id == EntityBase.ReservedUndifinedOid).ToList();
                            else
                                return Repository.Employees;


                        }
                        else
                            return Repository.Employees;

                    }
                    else
                        return Repository.Employees;
                }
            }
        }




        public Responsible CreateResponsible(IUnderResponsibility resp)
        {


            Responsible responsible = resp.ResponsiblesBindingList.AddNew() as Responsible;
            if (responsible != null)
            {
                var i = resp.ResponsiblesBindingList.IndexOf(responsible);
                responsible.Disposal = Disposal;
                // сначала назначаем руководителя

                if (CurrentStage != null)
                {
                    responsible.Stage = CurrentStage;
                    responsible.Contractdoc = CurrentContract;
                }
                else
                    responsible.Contractdoc = CurrentContract;

                responsible.Role = Roles.FirstOrDefault();
                responsible.Employee = Employees.FirstOrDefault(e => e.Id == EntityBase.ReservedUndifinedOid);


                (resp.ResponsiblesBindingList as BindingList<Responsible>).EndNew(i);
            }

            return responsible;
        }


        public void OnResponsiblePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Employee")
            {
                AutoInitRoles(ResponsibilityObject);
            }
        }


        public void AutoInitRoles(IUnderResponsibility resp)
        {
            if (resp != null)
            {


                if (resp.Chief != null && resp.Chief.Employee != null) //Removed: && resp.Chief.Employee.Departmentid != null
                {
                    Responsible re = null;

                    // руководитель направления
                    if (resp.Manager == null || (!resp.Responsibles.Any(r => (WellKnownRoles)r.Role.Id == WellKnownRoles.Manager)))
                    {
                        re = CreateResponsible(resp);

                        re.Role = Roles.FirstOrDefault(p => p.Id == (long)WellKnownRoles.Manager);
                        if (resp.Chief.DisposalDepartment.Manager != null)
                            re.Employee = resp.Chief.DisposalDepartment.Manager;
                        else
                            re.Employee = Employees.GetReservedUndefined();
                        re.Contractdoc = CurrentContract;
                    }

                    // замдир
                    if (resp.Director == null||(!resp.Responsibles.Any(r =>(WellKnownRoles)r.Role.Id == WellKnownRoles.Director)))
                    {

                        re = CreateResponsible(resp);
                        re.Role = Roles.FirstOrDefault(p => p.Id == (long)WellKnownRoles.Director);
                        if (resp.Chief.DisposalDepartment.Manager != null)
                            re.Employee = resp.Chief.DisposalDepartment.Director;
                        else
                            re.Employee = Employees.GetReservedUndefined();
                        re.Contractdoc = CurrentContract;
                    }

                    // ответственный по договорам от отдела
                    if (resp.OrderSuperviser == null||(!resp.Responsibles.Any(r =>(WellKnownRoles)r.Role.Id == WellKnownRoles.DepartmentResponsible)))
                    {
                        re = CreateResponsible(resp);
                        re.Role = Roles.FirstOrDefault(p => p.Id == (long)WellKnownRoles.DepartmentResponsible);
                        re.Contractdoc = CurrentContract;
                        var ro = resp.Chief.DisposalDepartment.Responsiblefororders.FirstOrDefault();
                        if (ro != null)
                            re.Employee = ro.Employee;

                    }

                    // ответственный от договорного отдел
                    if (resp.Curator == null||(!resp.Responsibles.Any(r =>(WellKnownRoles)r.Role.Id == WellKnownRoles.Curator)))
                    {
                        re = CreateResponsible(resp);

                        re.Role = Roles.FirstOrDefault(p => p.Id == (long)WellKnownRoles.Curator);
                        re.Contractdoc = CurrentContract;
                    }
                }
            }
        }



        private Responsible _responsible;
        public Responsible LastSelectedResponsible
        {
            get { return _responsible; }
            set
            {
                if (_responsible != null) _responsible.PropertyChanged -= OnResponsiblePropertyChanged;
                _responsible = value;
                if (_responsible != null) _responsible.PropertyChanged += OnResponsiblePropertyChanged;
                OnPropertyChanged(() => Employees);
            }
        }

        private void ShowEmployees(object o)
        {
            // показываем диалог со списком сотрудников
            var dlg = new SelectEmployeeWindow(Repository);
            if (LastSelectedResponsible.Employee != null)
            {
                dlg.ViewModel.SelectedDepartment = LastSelectedResponsible.Employee.Department;
                dlg.ViewModel.SelectedEmployee = LastSelectedResponsible.Employee;
            }

            if (dlg.ShowDialog() == true)
            {
                if (LastSelectedResponsible != null)
                {
                    LastSelectedResponsible.Employee = ((SelectEmployeeViewModel)dlg.DataContext).SelectedEmployee;
                }
            }
        }

        public bool CanShowContractEmployees
        {
            get { return (LastSelectedResponsible != null && LastSelectedResponsible.Role != null && LastSelectedResponsible.Role.Id != (long)WellKnownRoles.Curator); }
        }

        private RelayCommand _showemployeescontractcommand;
        public RelayCommand ShowEmployeesContractCommand
        {
            get
            {
                return _showemployeescontractcommand ?? (_showemployeescontractcommand = new RelayCommand(ShowEmployees, x => CanShowContractEmployees));

            }
        }

        public bool CanShowStageEmployees
        {
            get { return (LastSelectedResponsible != null && LastSelectedResponsible.Role != null && LastSelectedResponsible.Role.Id != (long)WellKnownRoles.Curator); }
        }

        private RelayCommand _showemployeesstagecommand;
        public RelayCommand ShowEmployeesStageCommand
        {
            get
            {
                return _showemployeesstagecommand ?? (_showemployeesstagecommand = new RelayCommand(ShowEmployees, x => CanShowStageEmployees));

            }
        }

        private void AddContractResponsible(object o)
        {
            CurrentStageResponsibleDTO = null;
            LastSelectedResponsible = CreateResponsible(CurrentContract);

        }


        public bool CanAddContractResponsible
        {
            get { return !Responsible.IsComplete(CurrentContract); }
        }

        private RelayCommand _addcontractresponsiblecommand;
        public RelayCommand AddContractResponsibleCommand
        {
            get
            {
                return _addcontractresponsiblecommand ?? (_addcontractresponsiblecommand = new RelayCommand(AddContractResponsible, x => CanAddContractResponsible));

            }
        }

        public bool CanDeleteContractResponsible
        {
            get { return (LastSelectedResponsible != null && CurrentContract != null && CurrentContract.Responsibles.IndexOf(LastSelectedResponsible) > -1); }
        }

        private void DeleteContractResponsible(object o)
        {

            LastSelectedResponsible.PropertyChanged -= OnResponsiblePropertyChanged;
            var responsible = LastSelectedResponsible;
            CurrentContract.ResponsiblesBindingList.Remove(responsible);
            CurrentContract.Responsibles.Remove(responsible);
            responsible.Disposal = null;
            OnPropertyChanged(()=>CurrentContract);
        }

        private RelayCommand _deletecontractresponsiblecommand;


        public RelayCommand DeleteContractResponsibleCommand
        {
            get
            {
                return _deletecontractresponsiblecommand ??
                       (_deletecontractresponsiblecommand = new RelayCommand(DeleteContractResponsible, x => CanDeleteContractResponsible));
            }
        }


        private bool CanDeleteAllContractResponsibles
        {
            get { return (CurrentContract != null && CurrentContract.ResponsiblesBindingList.Count > 0); }
        }

        private void DeleteAllContractresponsibles(object o)
        {
            CurrentContract.ResponsiblesBindingList.Clear();
            CurrentContract.Responsibles.Clear();
            OnPropertyChanged(() => CurrentContract);
            OnPropertyChanged(() => ContractStageResponsibleDto);
        }


        private RelayCommand _deleteallContractresponsiblescommand;
        public RelayCommand DeleteAllContractResponsiblesCommand
        {
            get
            {
                return _deleteallContractresponsiblescommand ??
                       (_deleteallContractresponsiblescommand =
                        new RelayCommand(DeleteAllContractresponsibles, x => CanDeleteAllContractResponsibles));
            }
        }



        private bool CanDeleteStageResponsible
        {
            get { return LastSelectedResponsible != null && (CurrentStage != null && CurrentStage.Responsibles.IndexOf(LastSelectedResponsible) > -1) || (CurrentStage == null); }
        }

        private void DeleteStageResponsible(object o)
        {


            LastSelectedResponsible.PropertyChanged -= OnResponsiblePropertyChanged;
            var responsible = LastSelectedResponsible;
            
            CurrentStage.ResponsiblesBindingList.Remove(responsible);
            CurrentStage.Responsibles.Remove(responsible);
            //CurrentStageResponsibleDTO.ResponsiblesBindingList.Remove(responsible);

            responsible.Disposal = null;
            OnPropertyChanged(() => CurrentStageResponsibleDTO);

        }

        private RelayCommand _deleteStageresponsiblecommand;
        public RelayCommand DeleteStageResponsibleCommand
        {
            get
            {
                return _deleteStageresponsiblecommand ??
                       (_deleteStageresponsiblecommand = new RelayCommand(DeleteStageResponsible, x => CanDeleteStageResponsible));
            }
        }

        private bool CanDeleteAllStageResponsibles
        {
            get { return (CurrentStage != null && CurrentStage.ResponsiblesBindingList.Count > 0); }
        }

        private void DeleteAllStageresponsibles(object o)
        {
           CurrentStage.ResponsiblesBindingList.Clear();
           CurrentStage.Responsibles.Clear();
           OnPropertyChanged(() => CurrentStageResponsibleDTO);
        }


        private RelayCommand _deleteallstageresponsiblescommand;
        public RelayCommand DeleteAllStageResponsiblesCommand
        {
            get
            {
                return _deleteallstageresponsiblescommand ??
                       (_deleteallstageresponsiblescommand =
                        new RelayCommand(DeleteAllStageresponsibles, x => CanDeleteAllStageResponsibles));
            }
        }

        private void AddStageResponsible(object o)
        {
            LastSelectedResponsible = CreateResponsible(CurrentStage);
            //CurrentStageResponsibleDTO.ResponsiblesBindingList.Add(LastSelectedResponsible);

        }


        public bool CanAddStageResponsible
        {
            get
            {
                return !Responsible.IsComplete(CurrentStage);
            }
        }

        private RelayCommand _addStageresponsiblecommand;
        public RelayCommand AddStageResponsibleCommand
        {
            get
            {
                return _addStageresponsiblecommand ?? (_addStageresponsiblecommand = new RelayCommand(AddStageResponsible, x => CanAddStageResponsible));

            }
        }


        protected bool CanAddResponsibleStage
        {
            get
            {

                return CurrentContract != null && (CurrentContract.Stages.Where(s => ContractStageResponsibleDto != null && !ContractStageResponsibleDto.StageResponsibleDTOBindingList.Select(r => r.Stage).Contains(s)).Count() > 0); ;
            }
        }

        protected bool CanAddAllResponsibleStages
        {
            get { return CurrentContract != null; }
        }

        public IBindingList CurrentContractStageResponsibleDTOBindingList
        {
            get
            {
                return ContractStageResponsibleDto != null
                           ? ContractStageResponsibleDto.StageResponsibleDTOBindingList
                           : null;
            }
        }

        private void AddResponsibleStage(object obj)
        {

            CurrentStageResponsibleDTO = ContractStageResponsibleDto.StageResponsibleDTOBindingList.AddNew() as StageResponsibleDTO;
            var i = ContractStageResponsibleDto.StageResponsibleDTOBindingList.IndexOf(CurrentStageResponsibleDTO);

            if (CurrentStageResponsibleDTO != null)
            {
                CurrentStageResponsibleDTO.Schedulecontract = CurrentContract.Schedulecontracts.FirstOrDefault(s => s.Schedule.Stages.Count() > 0);
                if (CurrentStageResponsibleDTO.Schedulecontract != null)
                    CurrentStageResponsibleDTO.Stage = AllStages.FirstOrDefault();

            }
            (ContractStageResponsibleDto.StageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>).EndNew(i);
            var stageResponsibleDtos = CurrentContractStageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>;
            if (stageResponsibleDtos != null)
                stageResponsibleDtos.ResetBindings();
            

            OnPropertyChanged("CurrentContractStageResponsibleDTOBindingList");
        }

        private void AddAllResponsibleStages(object obj)
        {

            foreach (var s in CurrentContract.Schedulecontracts.OrderBy(s => s.Appnum))
            {
                foreach (var st in s.Schedule.Stages.Where(st => ContractStageResponsibleDto.StageResponsibleDTOBindingList.All(sr => sr.Stage.Id != st.Id)).OrderBy(sl => sl.Num))
                {
                    CurrentStageResponsibleDTO = ContractStageResponsibleDto.StageResponsibleDTOBindingList.AddNew() as StageResponsibleDTO;
                    var i = ContractStageResponsibleDto.StageResponsibleDTOBindingList.IndexOf(CurrentStageResponsibleDTO);

                    if (CurrentStageResponsibleDTO != null)
                    {
                        CurrentStageResponsibleDTO.Schedulecontract = s;
                        CurrentStageResponsibleDTO.Stage = st;
                    }

                    (ContractStageResponsibleDto.StageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>).EndNew(i);
                }
            }

            var stageResponsibleDtos = CurrentContractStageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>;
            if (stageResponsibleDtos != null)
                stageResponsibleDtos.ResetBindings();
            OnPropertyChanged("CurrentContractStageResponsibleDTOBindingList");
        }

        private RelayCommand _addresponsiblestagecommand;
        public RelayCommand AddResponsibleStageCommand
        {
            get
            {
                return _addresponsiblestagecommand ??
                       (_addresponsiblestagecommand = new RelayCommand(AddResponsibleStage, x => CanAddResponsibleStage));
            }
        }


        private RelayCommand _addallresponsiblestagecommand;
        public RelayCommand  AddAllResponsibleStageCommand
        {
            get
            {
                return _addallresponsiblestagecommand ??
                       (_addallresponsiblestagecommand =
                        new RelayCommand(AddAllResponsibleStages, x => CanAddAllResponsibleStages));
            }
        }

        private void AddResponsibleContractdoc(object o)
        {
            ContractStageResponsibleDto = ContractStageResponsibleDTOList.AddNew();
            ContractStageResponsibleDto.Contractdoc = Contractdocs.FirstOrDefault(c => !Disposal.Contractdocs.Contains(c));
            var i = ContractStageResponsibleDTOList.IndexOf(ContractStageResponsibleDto);
            ContractStageResponsibleDTOList.EndNew(i);
        }

        private bool CanAddResponsibleContracdoc
        {
            get
            {
                //IList<Contractdoc> cs = Disposal.Responsibles.Select(r => r.Contractdoc).ToList(); 
                return Contractdocs.Where(s => Disposal != null && !Disposal.Contractdocs.Contains(s)).Count() > 0;
            }
        }

        private RelayCommand _addresponsiblecontractcommand;
        public RelayCommand AddResponsibleContractdocCommand
        {
            get
            {
                return _addresponsiblecontractcommand ??
                       (_addresponsiblecontractcommand = new RelayCommand(AddResponsibleContractdoc, x => CanAddResponsibleContracdoc));
            }
        }


        protected bool CanDeleteResponsibleContracdoc
        {
            get { return (ContractStageResponsibleDto != null && ((ContractStageResponsibleDto.Contractdoc != null && ContractObject != null && ContractStageResponsibleDto.Contractdoc.Id != ContractObject.Id) || ContractObject == null || ContractStageResponsibleDto.Contractdoc == null)); }
        }

        private void DeleteResponsibleContractdoc(object obj)
        {
            // удаляем всех закрепленных ответственных
            //IList<Responsible> rr = Disposal.Responsibles.Where(p => ContractStageResponsibleDto.Contractdoc != null && p.Contractdoc != null && p.Contractdoc.Id == ContractStageResponsibleDto.Contractdoc.Id).ToList();
            //foreach (var r in rr)
            //{
            //    r.Disposal = null;

            //}

            ContractStageResponsibleDto.Contractdoc.RemoveResponsiblesForDisposal(Disposal);

            ContractStageResponsibleDTOList.Remove(ContractStageResponsibleDto);

        }


        private RelayCommand _deleteresponsiblecontractcommand;
        public RelayCommand DeleteResponsibleContractdocCommand
        {
            get
            {
                return _deleteresponsiblecontractcommand ?? (_deleteresponsiblecontractcommand = new RelayCommand(DeleteResponsibleContractdoc, x => CanDeleteResponsibleContracdoc));
            }
        }


        public void SaveState(object o)
        {
            SaveChanges();
        }

        //private RelayCommand _savecommand;
        //public RelayCommand SaveCommand
        //{
        //    get { return _savecommand ?? (_savecommand = new RelayCommand(SaveState, x => true)); }
        //}


        protected bool CanDeleteResponsibleStage
        {
            get
            {
                return CurrentStageResponsibleDTO != null;
            }
        }

        private void DeleteResponsibleStage(object obj)
        {

            
            
            // удаляем всех закрепленных ответственных
            //IList<Responsible> rr = Disposal.Responsibles.Where(p => CurrentStageResponsibleDTO.Stage != null && p.Stage != null && p.Stage.Id == CurrentStageResponsibleDTO.Stage.Id).ToList();
            //foreach (var r in rr)
            //{
            //    r.Disposal = null;
            //}
            CurrentStageResponsibleDTO.Stage.RemoveResponsiblesForDisposal(Disposal);
            CurrentStageResponsibleDTO.Stage.RefreshRespBindingList();
            (ContractStageResponsibleDto.StageResponsibleDTOBindingList as BindingList<StageResponsibleDTO>).Remove(CurrentStageResponsibleDTO);
        }

        
        private RelayCommand _deleteresponsiblecommand;
        public RelayCommand DeleteResponsibleStageCommand
        {
            get
            {
                return _deleteresponsiblecommand ??
                       (_deleteresponsiblecommand =
                        new RelayCommand(DeleteResponsibleStage, x => CanDeleteResponsibleStage));
            }
        }


        private bool CanCopySelectedStageResponsibles
        {
            get { return (ContractStageResponsibleDto != null && CurrentStage != null && CurrentStage.ResponsiblesBindingList.Count > 1); }
        }

        private void CopySelectedStageResponsibles(object obj)
        {




            foreach (StageResponsibleDTO c in StageContextItems)
            {
                if (c.Stage != null && c.Stage.Id != CurrentStage.Id)
                {

                    c.Stage.RemoveResponsiblesForDisposal(Disposal);
                    c.Stage.RefreshRespBindingList();
                    

                    foreach (Responsible r in CurrentStage.ResponsiblesBindingList)
                    {
                        var rr = new Responsible();
                        c.Stage.Responsibles.Add(rr);
                        rr.Role = r.Role;
                        rr.Employee = r.Employee;
                        rr.Stage = c.Stage;
                        rr.Contractdoc = r.Contractdoc;
                        rr.Disposal = r.Disposal;
                        
                    }
                    c.Stage.SendResponsiblesBindingListChanged();
                }
            }

            OnPropertyChanged("CurrentContractStageResponsibleDTOBindingList");
        }


        private RelayCommand _copyselectedstageresponsiblescommand;
        public RelayCommand CopySelectedStageResponsiblesCommand
        {
            get
            {
                return _copyselectedstageresponsiblescommand ?? (_copyselectedstageresponsiblescommand = new RelayCommand(CopySelectedStageResponsibles, x=>CanCopySelectedStageResponsibles));
            }
        }

        private IList _stagecontextitems; 
        public IList StageContextItems
        {
            get { return _stagecontextitems; }
            set { _stagecontextitems = value; }
        }

        protected bool CanDeleteAllResponsibleStage
        {
            get
            {
                return CurrentContract != null;
            }
        }

        private void DeleteAllResponsibleStage(object obj)
        {
            for (var i = StageContextItems.Count - 1; i >= 0; i--)
            {
                _currentstageresponsibledto = (StageResponsibleDTO)StageContextItems[i];
                DeleteResponsibleStage(StageContextItems[i]);
            }
        }

        private RelayCommand _deleteallresponsiblestagecommand;
        public RelayCommand DeleteAllResponsibleStageCommand
        {
            get
            {
                return _deleteallresponsiblestagecommand ??
                       (_deleteallresponsiblestagecommand =
                        new RelayCommand(DeleteAllResponsibleStage, x => CanDeleteAllResponsibleStage));
            }
        }

        private string _contractdocnumsubstring = String.Empty;
        public string ContractdocNumSubstring
        {
            get { return _contractdocnumsubstring; }
            set
            {
                if (_contractdocnumsubstring == value) return;
                _contractdocnumsubstring = value;
                OnPropertyChanged("ContractdocNumSubstring");
                OnPropertyChanged("Contractdocs");
            }
        }

        private string _stagenumsubstring = string.Empty;
        public string StageNumSubstring
        {
            get { return _stagenumsubstring;  }
            set
            {
                if (_stagenumsubstring == value) return;
                _stagenumsubstring = value;
                OnPropertyChanged(() => StageNumSubstring);
                OnPropertyChanged(() => AllStages);
            }
        }

        public IList<Contractdoc> Contractdocs
        {
            get
            {
                return
                    Repository.Contracts.Where(
                        c =>
                        (ContractdocNumSubstring != string.Empty && c.Num != null && c.Num.Contains(ContractdocNumSubstring) && (c.Disposal == null||c.Disposal.Id == Disposal.Id)) ||
                        (CurrentContract != null && CurrentContract.Id == c.Id) ||
                        (Disposal != null && Disposal.Contractdocs.Contains(c))).Distinct().ToList();


                //return Disposal != null ? ((ContractObject != null) ? 
                //     Disposal.Responsibles.Where(p => (p.Contractdoc != null && p.Contractdoc.Id == ContractObject.Id)).Select(p => p.Contractdoc).Union(Contractdocs).Distinct() 
                //     : Disposal.Responsibles.Where(p => p.Stage == null).Select(p => p.Contractdoc).Union(Contractdocs).Distinct()) : null;
            }
        }

    }
}
