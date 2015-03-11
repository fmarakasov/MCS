using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using UIShared.Commands;
using McUIBase.ViewModel;

namespace MContracts.ViewModel
{
    
    public class LinkStageFromSubcontractViewModel: WorkspaceViewModel
    {

        private RelayCommand linkStagesCommand = null;

        public ICommand LinkStagesCommand
        {
            get { return linkStagesCommand ?? (linkStagesCommand = new RelayCommand(LinkStages, x => CanLinkStages)); }
        }

        private void LinkStages(object o)
        {
            Error = String.Empty;            
            //addedStages.Clear();
            //deletedStages.Clear();
            
            if (GeneralStage != null)
            {
                foreach (var selectedStage in Stages)
                {
                    //var count =
                     //   Repository.TryGetContext().Subgeneralhierarchis.Count(x => x.Subcontractdocstageid == selectedStage.Stage.Id&&x.Generalcontractdocstageid == GeneralStage.Id);

                    if (selectedStage.IsSelected)
                    {
                            selectedStage.Stage.GeneralStage = GeneralStage;
                    }
                }
            }
            else
            {
                foreach (var selectedStage in Stages)
                {
                    
                    if (selectedStage.IsSelected)
                    {
                        selectedStage.Stage.GeneralStage = null;
                    }
                }
            }


           
        }

        private bool CanLinkStages
        {
            get
            {
                return true;
            }
        }

        public IList<SelectedStage> Stages
        {
            get; set;
        }

        public Contractdoc Contract { get; set; }

        private Schedulecontract _schedulecontract;

        public IList<Schedulecontract> GeneralSchedulecontracts
        {
            get { return Contract.Generals.SelectMany(x => x.Schedulecontracts).ToList(); }
        }

        public Schedulecontract GeneralSchedulecontract
        {
            get
            {
                var firstOrDefault = Contract.Generals.FirstOrDefault();
                if (firstOrDefault != null)
                    return _schedulecontract ?? (_schedulecontract = firstOrDefault.Schedulecontracts[0]);
                return null;
            }
            set
            {
                _schedulecontract = value;
                OnPropertyChanged(()=>GeneralSchedulecontract);
                OnPropertyChanged(()=>GeneralStages);
            }
        
        }

        public IEnumerable<Stage> GeneralStages
        {
            get
            {
                if (GeneralSchedulecontract != null)
                    return GeneralSchedulecontract.Schedule.Stages.OrderBy(o => o.Num);
                return null;
            }
        }

        public void HasAddedInSubgeneralhierarchi()
        {
            Error = String.Empty;

           /*
            if (GeneralStage != null)
            {
                foreach (SelectedStage selectedStage in Stages)
                {
                    if (selectedStage.IsSelected)
                    {
                        if (selectedStage.Stage.Subgeneralhierarchis1.Any(x => x.Generalcontractdocstageid != GeneralStage.Id))
                        {
                            Error += "Этап № " + selectedStage.Stage.Num +
                                     " уже связан с этапом генерального договора № " +
                                     selectedStage.Stage.Subgeneralhierarchis1.FirstOrDefault(
                                         x => x.Generalcontractdocstageid != GeneralStage.Id).Stage.Num + "\n\n";
                        }
                    }
                }
            }
            */ 
        }

        public bool HasExit
        {
            get { return Error == String.Empty; }
        }

        public new String Error { get; set; }



        public Stage GeneralStage { get; set; }

        private List<Subgeneralhierarchi> addedStages = new List<Subgeneralhierarchi>();

        public void TrySetGeneralStage(Stage stage)
        {
            foreach (var subgeneralhierarchi in addedStages)
            {
                subgeneralhierarchi.Generalcontractdocstageid = stage.Id;
                if (subgeneralhierarchi.Substage.Endsat > GeneralStage.Endsat || subgeneralhierarchi.Substage.Startsat < GeneralStage.Startsat)
                {
                    Error += "Этап №" + subgeneralhierarchi.Substage.Num +
                             " не может быть привязан к этапу генерального договора № " +
                             subgeneralhierarchi.Generalstage.Num + ". Проверьте даты начала и конца этапов!" + "\n\n";
                }
            }

            OnPropertyChanged("HasExit");
        }

        public LinkStageFromSubcontractViewModel(IContractRepository repository)
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
