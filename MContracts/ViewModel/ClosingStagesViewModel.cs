using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using UIShared.Commands;
using MContracts.ViewModel.Helpers;
using McUIBase.ViewModel;
using McUIBase.Factories;

namespace MContracts.ViewModel
{
    public enum SelectedAction
    {
        NoAction,
        NewAct,
        ExistingAct,
        NoAct
    }
    
    public class ClosingStagesViewModel: WorkspaceViewModel
    {
        private SelectedAction selectedAction;
        public SelectedAction SelectedAction
        {
            get { return selectedAction; }
            set
            {
                selectedAction = value;
                OnPropertyChanged("HasExit");
            }
        }

        private IContractRepository backupRepository = RepositoryFactory.CreateContractRepository();
        
        private RelayCommand detachCommand = null;

        public ICommand DetachCommand
        {
            get
            {
                if (detachCommand == null)
                    detachCommand = new RelayCommand(Detach, x => CanDetach(null));
                return detachCommand;
            }
        }

        private RelayCommand attachExistingActCommand = null;

        public ICommand AttachExistingActCommand
        {
            get
            {
                if (attachExistingActCommand == null)
                    attachExistingActCommand = new RelayCommand(AttachExistingAct, x => CanAttachExistingAct(null));
                return attachExistingActCommand;
            }
        }

        private RelayCommand attachNewActCommand = null;

        public ICommand AttachNewActCommand
        {
            get
            {
                if (attachNewActCommand == null)
                    attachNewActCommand = new RelayCommand(AttachNewAct, x => CanAttachNewAct(null));
                return attachNewActCommand;
            }
        }

        private void Detach(object o)
        {
            foreach (var closedStage in ClosedStages)
            {
                closedStage.Act = null;
            }
        }

        private bool CanDetach(object o)
        {
            return true;
        }

        private void AttachExistingAct(object o)
        {
            foreach (var closedStage in ClosedStages)
            {
                closedStage.Act = ExistingAct;
            }
        }

        private bool CanAttachExistingAct(object o)
        {
            return ExistingAct != null;
        }

        private void AttachNewAct(object o)
        {
            
            foreach (var closedStage in ClosedStages)
            {
                backupRepository.TryGetContext().Acts.InsertOnSubmit(newAct);
                backupRepository.SubmitChanges();
                closedStage.Act = Repository.TryGetContext().Acts.Single(x=>x.Region == newAct.Region 
                    && x.Enterpriseauthority == newAct.Enterpriseauthority 
                    && x.Num == newAct.Num 
                    && x.Signdate == newAct.Signdate
                    && x.Nds == newAct.Nds
                    && x.Status == newAct.Status);
            }
        }

        private bool CanAttachNewAct(object o)
        {
            return String.IsNullOrEmpty(NewAct.Error);
        }
        
        public IBindingList Stages
        {
            get; set;
        }

        public bool HasExit
        {
            get
            {
                switch (SelectedAction)
                {
                    case SelectedAction.NoAct:
                        return true;
                    case SelectedAction.ExistingAct:
                        return ExistingAct != null;
                    case SelectedAction.NewAct:
                        return String.IsNullOrEmpty(NewAct.Error);
                    case SelectedAction.NoAction:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public IBindingList Acts
        {
            get
            {
                IBindingList result = new BindingList<Act>();
                IEnumerable<Schedule> q = Contract.Schedulecontracts.Select(x => x.Schedule);
                foreach (Schedule schedule in q)
                {
                    foreach (var stage in schedule.Stages)
                    {
                        var act = Repository.TryGetContext().Acts.Where(x => x.Stages.Contains(stage)).FirstOrDefault();
                        if (!result.Contains(act))
                            result.Add(act);
                    }
                }

                return result;
            }
        }

        public List<Stage> ClosedStages = new List<Stage>();

        private Act existingAct;
        public Act ExistingAct
        {
            get { return existingAct; }
            set
            {
                existingAct = value; 
                OnPropertyChanged("HasExit");
            }
        }

        private Act newAct;
        public Act NewAct
        {
            get
            {
                if (newAct == null)
                {
                    newAct = new Act();
                    newAct.PropertyChanged += new PropertyChangedEventHandler(newAct_PropertyChanged);
                }
                return newAct;
            }
        }

        void newAct_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("HasExit");
        }

        public Contractdoc Contract { get; set; }

        public IEnumerable<Nds> Ndses
        {
            get
            {
                return backupRepository.Nds;
            }
        }

        public IEnumerable<Enterpriseauthority> Enterpriseauthorities
        {
            get
            {
                return backupRepository.Enterpriseauthorities;
            }
        }

        public IEnumerable<Region> Regions
        {
            get
            {
                return backupRepository.Regions;
            }
        }

        public IEnumerable<Acttype> Acttypes
        {
            get
            {
                return backupRepository.Acttypes;
            }
        }
        
        public ClosingStagesViewModel(IContractRepository repository) : base(repository)
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

        protected override void OnDispose()
        {
            if (backupRepository != null)
                backupRepository.Dispose();
        }


    }
}
