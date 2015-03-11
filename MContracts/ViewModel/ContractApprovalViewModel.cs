using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.Controls;
using MediatorLib;
using MContracts.ViewModel.Helpers;
using MContracts.Classes;
using System.Collections.ObjectModel;
using McUIBased.Commands;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    
    public class ContractApprovalViewModel : ContractdocBaseViewModel
    {
        private ICommand _addApprovalEntryCommand;
        private ICommand _removeApprovalEntryCommand;
        
        [ApplicationCommand("Добавить запись о процессе согласования", "/MContracts;component/Resources/act_add.png")]
        public ICommand AddApprovalEntryCommand
        {
            get {
                return _addApprovalEntryCommand ??
                       (_addApprovalEntryCommand =
                        new RelayCommand(x => AddAprovalEntry(), x => CanAdd()));
            }
        }

        [ApplicationCommand("Удалить запись о процессе согласования", "/MContracts;component/Resources/act_delete.png", 
            AppCommandType.Confirm, "Выбранная вами запись о ходе согласования будет удалёна. Продолжить?")]
        public ICommand RemoveApprovalEntryCommand
        {
            get {
                return _removeApprovalEntryCommand ??
                       (_removeApprovalEntryCommand =
                        new RelayCommand(x => RemoveAprovalEntry(), x => CanRemove()));
            }
        }

        public Approvalprocess SelectedApproval { get; set; }


        private ObservableCollection<Location> _locations;
        public ObservableCollection<Location> Locations
        {
            get
            {
                return _locations ?? (_locations = new ObservableCollection<Location>(
                                                       Repository.Locations));
            }
        }


        private ObservableCollection<Missivetype> _missivetypes;
        public ObservableCollection<Missivetype> Missivetypes
        {
            get
            {
                if (_missivetypes == null)
                    _missivetypes =
                        new ObservableCollection<Missivetype>(
                            Repository.MissiveTypes);
                return _missivetypes;
            }
        }

        private ObservableCollection<Approvalgoal> _approvalgoals;
        public IEnumerable<Approvalgoal> Approvalgoals
        {
            get
            {
                if (_approvalgoals == null)
                    _approvalgoals =
                        new ObservableCollection<Approvalgoal>(
                            Repository.ApprovalGoals);
                return _approvalgoals;
            }
        }

        private ObservableCollection<Approvalstate> _approvalstates;
        public IEnumerable<Approvalstate> ApprovalStates
        {
            get
            {
                if (_approvalstates == null)
                    _approvalstates =
                        new ObservableCollection<Approvalstate>(
                            Repository.ApprovalStates);
                return _approvalstates;
            }
        }

        [MediatorMessageSink(RequestRepository.CATALOG_CHANGED, ParameterType = typeof(CatalogType))]
        public void CatalogChanged(CatalogType c)
        {
                if (c == CatalogType.Location)
                {

                    try
                    {
                        _locations = null;
                        OnPropertyChanged("Locations");
                    }
                    catch
                    {
                        if (SelectedApproval != null)
                        {
                            // ошибка возникает когда редактируется то, что в комбобоксе выбрано активным элементом
                            SelectedApproval.ToLocation = Locations.GetReservedUndefined();
                            SelectedApproval.FromLocation = Locations.GetReservedUndefined();
                        }

                    }
                }
                else if (c == CatalogType.MissiveType)
                {
                    try
                    {
                        _missivetypes = null;
                        OnPropertyChanged("Missivetypes");
                    }
                    catch
                    {
                        if (SelectedApproval != null)
                        {
                            SelectedApproval.Missivetype = Missivetypes.GetReservedUndefined();
                        }
                    }
                }
                else if (c == CatalogType.ApprovalGoal)
                {
                    try
                    {
                        _approvalgoals = null;
                        OnPropertyChanged("Approvalgoals");
                    }
                    catch
                    {
                        if (SelectedApproval != null)
                        {
                            SelectedApproval.Approvalgoal = Approvalgoals.GetReservedUndefined();
                        }
                    }
                }
                else if (c == CatalogType.ApprovalState)
                {

                    try
                    {
                        _approvalstates = null;
                        OnPropertyChanged("ApprovalStates");
                    }
                    catch
                    {
                        if (SelectedApproval != null)
                        {
                            SelectedApproval.Approvalstate = ApprovalStates.GetReservedUndefined();
                        }
                    }
                }
            
        }

        private bool CanRemove()
        {
            // удалять можем только если аппрувал является последним
            return (SelectedApproval != null)&&(Approvals.IndexOf(SelectedApproval) == Approvals.Count - 1);
        }

        private bool CanAdd()
        {
            return true;
        }

        private void AddAprovalEntry()
        {
            Approvalprocess approval = Repository.TryGetContext().NewApprovalProcess(ContractObject);
            //ContractObject.Approvalprocesses.Add(approval);
            //try
            //{
            //   
            Approvals.Add(approval);
            //}
            //catch
            //{
            //    // делать так нельзя, я знаю, но это опять та же петрушка, что и с этапами
            //}
            
            OnPropertyChanged("Approvals");
        }


        void ApprovalPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        private void RemoveAprovalEntry()
        {

 

            if (Approvals.Contains(SelectedApproval))
            {
                if (SelectedApproval.Contractdoc != null)
                {
                    Repository.DeleteApproval(SelectedApproval);
                    Repository.TryGetContext().Refresh(RefreshMode.KeepChanges, SelectedApproval);
                    
                    
                }

                Approvals.Remove(SelectedApproval);
             
            }


                 
            //ContractObject.Approvalprocess.Remove(SelectedApproval);
            OnPropertyChanged("Approvals");
            //Repository.DebugPrintRepository();
            //Approvals.Remove(SelectedApproval);
            // Repository.TryGetContext().Approvalprocess.DeleteOnSubmit(SelectedApproval);                      
            //Repository.TryGetContext().Refresh(RefreshMode.OverwriteCurrentValues, SelectedApproval);
            
            
        }
        
        private IBindingList _approvals;

        public IBindingList Approvals
        {
            get
            {
                if (_approvals == null)
                {
                    _approvals = ContractObject.Approvalprocesses.GetNewBindingList();
                    _approvals.ListChanged += new ListChangedEventHandler(_approvals_ListChanged);
                    foreach (Approvalprocess a in _approvals)
                    {
                        a.PropertyChanged += new PropertyChangedEventHandler(ApprovalPropertyChanged);
                    }

                }
                return _approvals;
            }
        }

        void _approvals_ListChanged(object sender, ListChangedEventArgs e)
        {
           /* if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                if (e.PropertyDescriptor.Name == "NextStateTimespan" || e.PropertyDescriptor.Name == "PrevStateTimespan")
                    return;
            }

            RecalcTimeSpans(); */
        }

        private void RecalcTimeSpans()
        {
            
            foreach (Approvalprocess approval in Approvals)
            {
                approval.InvalidateTimeSpans();
            }
        }

        

        public ContractApprovalViewModel(IContractRepository repository, ViewModelBase owner) : base(repository, owner)
        {
            ViewMediator.Register(this);    
        }

        protected override void Save()
        {
            //Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return true; 
        }

        
    }
}
