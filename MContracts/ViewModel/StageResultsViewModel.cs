using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel.Helpers;
using McUIBased.Commands;
using MediatorLib;
using UIShared.Commands;

namespace MContracts.ViewModel
{
    public class StageResultsViewModel : ContractWorkspaceViewModel
    {
        private Schedulecontract _selectedschedule;
        private IBindingList _stages;
        private ICommand _createResultCommand;
        private ICommand _deleteResultCommand;

        private IBindingList _resultsBindingList;
        private Stageresult _selectedResult;

        public StageResultsViewModel(IContractRepository repository)
            : base(repository)
        {
            ViewMediator.Register(this);
        }

        public IBindingList ResultsBindingList
        {
            get
            {
                if (_resultsBindingList == null)
                {
                    if (ContractObject.Schedulecontracts.Any())
                    {
                        _resultsBindingList = new BindingList<Stageresult>();

                        foreach (var sr in SelectedSchedule.Schedule.Stages.SelectMany(s => s.Stageresults))
                        {
                            _resultsBindingList.Add(sr);
                        }
                    }
                }
                return _resultsBindingList;
            }
        }

        public IEnumerable<Schedulecontract> Schedulecontracts
        {
            get { return ContractObject.Schedulecontracts.Where(c => c.Schedule != null); }
        }

        /// <summary>
        ///     выбранный КП
        /// </summary>
        public Schedulecontract SelectedSchedule
        {
            get { return _selectedschedule ?? (_selectedschedule = Schedulecontracts.FirstOrDefault()); }
            set
            {
                _selectedschedule = value;
                _resultsBindingList = null;
                OnPropertyChanged(()=>SelectedSchedule);
                OnPropertyChanged(()=>Stages);
                OnPropertyChanged(()=>ResultsBindingList);
            }
        }

        public Stageresult SelectedResult
        {
            get
            {
                return _selectedResult;
            }
            set
            {
                if (_selectedResult == value) return;
                _selectedResult = value;
                OnPropertyChanged(()=>SelectedResult);
            }
        }

        public IList<Economefficiencytype> Economefficiencytypes
        {
            get { return Repository.Economefficiencytypes; }
        }

        public IList<Ntpview> Ntpviews
        {
            get { return Repository.Ntpviews; }
        }

        public IList<Ntpsubview> Ntpsubviews
        {
            get { return Repository.Ntpsubviews; }
        }

        public IBindingList Stages
        {
            get
            {
                if (_stages == null)
                {
                    if (SelectedSchedule != null)
                    {
                        _stages = SelectedSchedule.Schedule.Stages.GetNewBindingList();
                    }
                }
                return _stages;
            }
        }

        public override string DisplayName
        {
            get { return string.Format("Рез. по {0}", ContractObject); }
        }

        public override string Error
        {
            get
            {
                var sb = new StringBuilder();

                if (ResultsBindingList != null)
                {
                    foreach (Stageresult stageresult in ResultsBindingList)
                    {
                        sb.AppendLine(stageresult.Error);
                    }
                }

                return sb.ToString();
            }
        }

        public override bool IsClosable
        {
            get { return true; }
        }

        //[MediatorMessageSink(RequestRepository.REFRESH_RESULT_SCHEDULE, ParameterType = typeof (Stageresult))]
        //public void AddedResult(Stageresult stageresult)
        //{
        //    _resultsBindingList = null;
        //    OnPropertyChanged("ResultsBindingList");
        //}

        //[MediatorMessageSink(RequestRepository.REFRESH_RESULT_SCHEDULE_STAGE, ParameterType = typeof (Stage))]
        //public void StageChanged(Stage stage)
        //{
        //    _resultsBindingList = null;
        //    OnPropertyChanged("ResultsBindingList");
        //}


        //[MediatorMessageSink(RequestRepository.REFRESH_RESULT_SCHEDULE_GENERAL, ParameterType = typeof (Schedule))]
        //public void ScheduleChanged(Schedule schedule)
        //{
        //    _resultsBindingList = null;
        //    OnPropertyChanged("ResultsBindingList");
        //}

        protected override void Save()
        {
            base.Save();
            Repository.SubmitChanges();
        }

        protected override bool CanSave()
        {
            return ValidateResults();
        }

        public void OnErrorChanged()
        {
            ViewMediator.NotifyColleagues(RequestRepository.REQUEST_ERROR_CHANGED, Error);
        }

        public void CreateParameters()
        {
            var parameters = _selectedResult.Economefficiencytype.Efficienceparametertypes;

            _selectedResult.ParametersBindingList.Clear();

            foreach (var parameter in parameters)
            {
                _selectedResult.ParametersBindingList.Add(new Efparameterstageresult
                    {
                        Economefficiencyparameter = parameter.Economefficiencyparameter
                    });
            }

            OnPropertyChanged("SelectedResult");
        }

        private bool ValidateResults()
        {
            if (ResultsBindingList != null)
            {
                return ResultsBindingList.Cast<object>().All(stageresult =>
                    {
                        var stageresult1 = stageresult as Stageresult;
                        return stageresult1 != null && string.IsNullOrEmpty(stageresult1.Error);
                    });
            }
            return true;
        }

        public void AddResult(Stage stage)
        {
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT, stage);
        }

        public void UpdateResult()
        {
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT, _selectedResult.Stage);
        }


        private void stageresult_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //var stageresult = sender as Stageresult;
            //if (stageresult == null) return;
            //ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT, stageresult.Stage);
            //ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT_SCHEDULE, stageresult);
        }

        #region ResultsCommands

        public ICommand CreateResultCommand
        {
            get
            {
                return _createResultCommand ??
                       (_createResultCommand = new RelayCommand(CreateResult, x => CanCreateResult));
            }
        }

        public ICommand DeleteResultCommand
        {
            get
            {
                return _deleteResultCommand ??
                       (_deleteResultCommand = new RelayCommand(DeleteResult, x => CanDeleteResult));
            }
        }


        public bool CanCreateResult
        {
            get { return ResultsBindingList != null; }
        }

        public bool CanDeleteResult
        {
            get { return _selectedResult != null; }
        }

        private void CreateResult(object o)
        {
            var result = new Stageresult();
            ResultsBindingList.Add(result);
            result.PropertyChanged += stageresult_PropertyChanged;
            OnPropertyChanged("ResultsBindingList");
        }

        private void DeleteResult(object o)
        {
            var stage = _selectedResult.Stage;
            if (stage != null)
            {
                Repository.DeleteStateResult(_selectedResult);
                stage.Stageresults.Remove(_selectedResult);
            }
            _resultsBindingList = null;
            OnPropertyChanged("ResultsBindingList");
            ViewMediator.NotifyColleagues(RequestRepository.REFRESH_RESULT, stage);
            //ResultsBindingList.Remove(selectedResult);
        }

        #endregion

    }
}