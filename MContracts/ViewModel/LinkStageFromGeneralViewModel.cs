using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using UIShared.Commands;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    
    public class LinkStageFromGeneralViewModel: WorkspaceViewModel
    {

        private RelayCommand linkStagesCommand = null;

        public ICommand LinkStagesCommand
        {
            get
            {
                if (linkStagesCommand == null)
                    linkStagesCommand = new RelayCommand(LinkStages, x => CanLinkStages);
                return linkStagesCommand;
            }
        }

        private List<Subgeneralhierarchi> addedStages = new List<Subgeneralhierarchi>();
        private List<Subgeneralhierarchi> deletedStages = new List<Subgeneralhierarchi>();
        
        private void LinkStages(object o)
        {
            //addedStages.Clear();
            //deletedStages.Clear();
            Error = String.Empty;
            
            if (Stages == null) return;

            if (GeneralStage != null)
            {
                foreach (var selectedStage in Stages)
                {
                        //Repository.TryGetContext().Subgeneralhierarchis.Where(
                        //    x => x.Subcontractdocstageid == selectedStage.Stage.Id&&x.Generalcontractdocstageid==GeneralStage.Id).Count();

                    if (selectedStage.IsSelected && GeneralStage.Subgeneralhierarchis.FirstOrDefault(x => x.Subcontractdocstageid == selectedStage.Stage.Id) == null)
                    {

                        Subgeneralhierarchi subgeneralhierarchi = new Subgeneralhierarchi();
                        subgeneralhierarchi.Substage = selectedStage.Stage;
                        subgeneralhierarchi.Generalstage = GeneralStage;
                        GeneralStage.Subgeneralhierarchis.Add(subgeneralhierarchi);
                    }
                    else
                    {
                        if (!selectedStage.IsSelected && GeneralStage.Subgeneralhierarchis.FirstOrDefault(x => x.Subcontractdocstageid == selectedStage.Stage.Id) != null)
                        {
                            var hrchs = GeneralStage.Subgeneralhierarchis.Where(
                                                                x => x.Subcontractdocstageid == selectedStage.Stage.Id);
                            for (int i = hrchs.Count() - 1; i == 0; i--)
                                GeneralStage.Subgeneralhierarchis.RemoveAt(i);
                        }
                        
                    }
                }
                GeneralStage.RefreshSubstages();
            }
            else
            {
                //foreach (var selectedStage in Stages)
                //{

                    
                //    var hrchs =
                //        Repository.TryGetContext().Subgeneralhierarchis.Where(
                //            x => x.Subcontractdocstageid == selectedStage.Stage.Id);

                //    if (selectedStage.IsSelected)
                //    {
                //        deletedStages.AddRange(hrchs);
                //    }
                //}
            }


            if (this.Error != String.Empty)
            {
                return;
            }

            //foreach (var subgeneralhierarchi in deletedStages)
            //{
            //    var sghi =
            //        Repository.TryGetContext().Subgeneralhierarchis.Single(
            //            x =>
            //            x.Generalcontractdocstageid == subgeneralhierarchi.Generalcontractdocstageid &&
            //            x.Subcontractdocstageid == subgeneralhierarchi.Subcontractdocstageid);
            //    Repository.TryGetContext().Subgeneralhierarchis.DeleteOnSubmit(sghi);
            //}
            //foreach (var subgeneralhierarchi in addedStages)
            //{
            //    Repository.TryGetContext().Subgeneralhierarchis.InsertOnSubmit(subgeneralhierarchi);
            //}

        }

        private bool CanLinkStages
        {
            get
            {
                return true;
            }
        }

        public Stage GeneralStage
        {
            get; set;
        }

        public Contractdoc Contract { get; set; }

        public IEnumerable<Contractdoc> Subcontracts
        {
            get
            {
                return Contract.SubContracts.Where(x => x.Stages.Count() > 0 && x.Stages.Any(s => s.GeneralStage == null||s.GeneralStage == GeneralStage));
            }
        }

        private Contractdoc subContract;
        public Contractdoc SubContract
        {
            get { return subContract; }
            set
            { 
                subContract = value;
                stages = null;

                OnPropertyChanged(()=>SubcontractSchedulecontracts);
                OnPropertyChanged(()=>Selectedschedulecontract);
                OnPropertyChanged(()=>Stages);

            }
        }

        public IList<Schedulecontract> SubcontractSchedulecontracts
        {
            get { return (SubContract != null) ? SubContract.Schedulecontracts : null; }
        }

        private Schedulecontract _selectedschedulecontract;
        public Schedulecontract Selectedschedulecontract
        {
            get
            {
                if (_selectedschedulecontract == null)
                {
                    _selectedschedulecontract = (SubcontractSchedulecontracts != null)
                                                    ? SubcontractSchedulecontracts.FirstOrDefault()
                                                    : null;

                }
                return _selectedschedulecontract;
            }
            set 
            { 
                _selectedschedulecontract = value;
                stages = null;
                OnPropertyChanged(()=>Selectedschedulecontract);
                OnPropertyChanged(() => Stages);
            }
        }

        //public bool HasExit
        //{
        //    get { return Error == String.Empty; }
        //}

        private List<SelectedStage> stages = null;
        public IEnumerable<SelectedStage> Stages
        {
            get
            {
                if (Selectedschedulecontract != null && Selectedschedulecontract.Schedule.Stages.Count > 0)
                {
                    if (stages == null)
                    {
                        stages = new List<SelectedStage>();
                        foreach (var stage in Selectedschedulecontract.Schedule.Stages)
                        {
                            if (stage.GeneralStage == null||stage.GeneralStage == GeneralStage)
                            {
                                stages.Add(new SelectedStage(stage));
                            }
                        }
                        
                        return stages;
                    }
                }
                return stages;
            }
        }

        public bool CheckContaintsInSubgeneralhierarchis(Stage stage)
        {
            var sghi = Repository.TryGetContext().Subgeneralhierarchis.FirstOrDefault(x => x.Subcontractdocstageid == stage.Id && x.Generalcontractdocstageid == GeneralStage.Id);
            if (sghi != null)
                return GeneralStage.Subgeneralhierarchis.Contains(sghi);
            return false;
        }

        public bool HasAddedInSubgeneralhierarchi(Stage stage)
        {
            return stage.Subgeneralhierarchis.Count == 0 || stage.Subgeneralhierarchis.Where(x => x.Generalcontractdocstageid == GeneralStage.Id).Count() > 0;
        }

        public new String Error { get; set; }

        public LinkStageFromGeneralViewModel(IContractRepository repository)
            : base(repository)
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
