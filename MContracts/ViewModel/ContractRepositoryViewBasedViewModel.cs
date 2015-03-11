using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MCDomain.Model.RepositoryExtensions;
using MContracts.ViewModel.Helpers;
using McReports.Common;
using MediatorLib;
using McUIBase.ViewModel;
using UOW;
using RepositoryFactory = McUIBase.Factories.RepositoryFactory;

namespace MContracts.ViewModel
{
    public class ContractRepositoryViewBasedViewModel : MainViewModelWorkspace, IContractdocRefHolder, IReportSourceProvider, IContractCaption
    {
        
        private ObservableCollection<IContractStateData> _contractrepositoryviews;

        public ContractRepositoryViewBasedViewModel(IContractRepository repository)
            : base(repository)
        {
            BusyState = "Пожалуйста, подождите...";
            ViewMediator.Register(this);
            PropertyChanged += ContractRepositoryViewBasedViewModel_PropertyChanged;
            IsUnchangable = true;
        }

        /// <summary>
        /// Получает текущий выбранный договор
        /// </summary>
        public Contractdoc Current
        {
            get
            {
                if (CurrentContractRepositoryView == null) return null;
                return UnitOfWork.Repository<Contractdoc>().Single(x=>x.Id == CurrentContractRepositoryView.Id);
            }
        }

        private Contractrepositoryview _currentContractRepositoryView;
        public Contractrepositoryview CurrentContractRepositoryView
        {
            get { return _currentContractRepositoryView; }
            set
            {
                if (value == _currentContractRepositoryView) return;
                _currentContractRepositoryView = value;
                OnPropertyChanged(() => CurrentContractRepositoryView);
                OnPropertyChanged(() => ContractCaption);
            }
        }


        public override string DisplayName
        {
            get { return "Реестр договоров"; }
            protected set { base.DisplayName = value; }
        }

        public ObservableCollection<IContractStateData> Contractrepositoryviews
        {
            get
            {
                if (_contractrepositoryviews == null)
                {
                    _contractrepositoryviews =
                        new ObservableCollection<IContractStateData>(FilteredContracts);

                    InitializeContractrepositoryviews();
                }
                return _contractrepositoryviews;
            }
        }

        private void InitializeContractrepositoryviews()
        {
            _contractrepositoryviews.ParallelApply(
                x =>
                {
                    //x.CastTo<Contractrepositoryview>().ContractRepository = Repository;
                    if (MainViewModel != null)
                    {
                        x.CastTo<Contractrepositoryview>().FilterStartDate = FilteredDates.Start;
                        x.CastTo<Contractrepositoryview>().FilterEndDate = FilteredDates.End;
                    }
                });
        }

        [MediatorMessageSink(RequestRepository.REFRESH_ACTS_SCHEDULE, ParameterType = typeof(Act))]
        internal void ActChanged(Act act)
        {
            UpdateContractOnActChanged(act);
        }

        private void UpdateContractOnActChanged(Act act)
        {
            Contract.Assert(act != null);
            if (act.ContractObject != null) ContractChangedHandler(act.ContractObject);
        }

        [MediatorMessageSink(RequestRepository.REQUEST_ACT_DELETED, ParameterType = typeof(Contractdoc))]
        internal void ActDeleted(Contractdoc contractdoc)
        {
            ContractChangedHandler(contractdoc);
        }


        /// <summary>
        /// Получыает или устанавливает делегат для задания функции выборки текущих элементов
        /// </summary>
        public Func<IList> FetchContextFunc;

        /// <summary>
        /// Получает или устанавливает контекстные элементы (как результата применения фильтрации)
        /// </summary>
        public IList ContextItems
        {
            get { return (FetchContextFunc != null) ? FetchContextFunc() : null; }

        }

        /// <summary>
        /// Получает активные договора
        /// </summary>
        public IEnumerable<IContractStateData> ActualContextItems
        {
            get
            {
                return ContextItems.Cast<Contractrepositoryview>().Where(x => x.IsActual);
            }
        }
        public override bool IsClosable
        {
            get { return false; }
        }

        public override bool IsModified
        {
            get { return false; }
        }

        /// <summary>
        /// Получает диапазон дат фильтрации 
        /// </summary>
        private DateRange FilteredDates
        {
            get
            {

                DateTime start, end;
                if (MainViewModel != null)
                {
                    start = MainViewModel.SelectedFilterStartDate;
                    end = MainViewModel.SelectedFilterEndDate;
                }
                else
                {
                    DateTime now = DateTime.Today;
                    start = DateTimeExtensions.GetFirstYearDay(now.Year);
                    end = DateTimeExtensions.GetLastYearDay(now.Year);
                }
                return new DateRange() { Start = start, End = end };
            }
        }

        private IEnumerable<IContractStateData> GetFilteredContracts(IContractRepository repository)
        {
            if (ProjectStartupInfo.FastLoad) return new List<IContractStateData>();
            if (ProjectStartupInfo.Contracts != String.Empty)
            {

                var ids = ProjectStartupInfo.Contracts.Split(';').ToList().Select(s =>
                {
                    long o;
                    return long.TryParse(
                        s, out o) ? o : long.MaxValue;
                });
                return UnitOfWork.Repository<Contractrepositoryview>().FetchActualContractviews(ids);
            }

            var dates = FilteredDates;
            return UnitOfWork.Repository<Contractrepositoryview>().FetchActualContractviews(dates.Start, dates.End);
        }

        public IEnumerable<IContractStateData> FilteredContracts
        {
            get
            {
                var repository = RepositoryFactory.CreateContractRepository();
                return GetFilteredContracts(repository);
            }
        }


        public event EventHandler WorkspaceChanged;

        [MediatorMessageSink(RequestRepository.REQUEST_ACTIVE_WORKSPACE_CHANGED,
            ParameterType = typeof(WorkspaceViewModel))]
        public void WorkspaceChangedHandler(WorkspaceViewModel workspace)
        {
            if (WorkspaceChanged != null) WorkspaceChanged(workspace, EventArgs.Empty);
        }

        private void ContractRepositoryViewBasedViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MainViewModel")
            {
                if (MainViewModel != null)
                {
                    MainViewModel.PropertyChanged += MainViewModel_PropertyChanged;
                }
            }
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedFilterstate")
            {
                if (MainViewModel.SelectedFilterEndDate >= MainViewModel.SelectedFilterStartDate)
                {
                    _contractrepositoryviews = null;
                    OnPropertyChanged("Contractrepositoryviews");
                }
            }
        }

        protected override void Save()
        {
        }

        protected override bool CanSave()
        {
            return false;
        }

        [MediatorMessageSink(RequestRepository.REQUEST_CONTRACT_UPDATED, ParameterType = typeof(Contractdoc))]
        public void ContractChangedHandler(Contractdoc contractdoc)
        {
            Contract.Requires(contractdoc != null);

            var obj = Contractrepositoryviews.SingleOrDefault(x => x.Id == contractdoc.Id);
            if (obj != null)
            {
                var relatedobjs =
                    (Contractrepositoryviews.Where(
                        x => x.Maincontractid == obj.Maincontractid)).ToList();

                // Этот договор уже загружен в реестр
                using (var repository = RepositoryFactory.CreateContractRepository())
                {

                    for (int i = relatedobjs.Count - 1; i >= 0; i--)
                    {
                        var index = Contractrepositoryviews.IndexOf(relatedobjs[i]);
                        if (index == -1) continue;

                        var updated =
                            repository.With(x => x.TryGetContext()).With(x => x.Contractrepositoryviews).Return(
                                x => x.Single(y => y.Id == relatedobjs[i].Id), null);

                        Contractrepositoryviews[index] = updated;
                    }
                }
            }
            else
            {
                // Этот договор новый
                var newContract = GetContractRepositoryView(contractdoc.Id);
                if (newContract != null)
                {
                    //newContract.ContractRepository = Repository;
                    Contractrepositoryviews.Add(newContract);
                }
            }

        }

        [MediatorMessageSink(RequestRepository.REQUEST_SAVE_WORKSPACE, ParameterType = typeof(WorkspaceViewModel))]
        public void ActiveWorkspaceSaved(WorkspaceViewModel activeWorkspace)
        {
            var ws = activeWorkspace as ContractViewModel;
            if (ws == null) return;
            Contract.Assert(ws.Contractdoc != null);
            if (EntityBase.IsReservedOrEmpty(ws.Contractdoc.Id)) return;
            ContractChangedHandler(ws.Contractdoc);
        }

        private Contractrepositoryview GetContractRepositoryView(long id)
        {
            return
                UnitOfWork.Repository<Contractrepositoryview>().AsQueryable().SingleOrDefault(
                    x => x.Id == id);
        }

        internal void DeleteContract(IContractStateData contract)
        {
            Contract.Requires(contract != null);

            var deleted = UnitOfWork.DeleteContractdoc(contract.Id);
            UnitOfWork.Commit();

            foreach (var c in deleted)
            {
                var obj = Contractrepositoryviews.SingleOrDefault(x => x.Id == c.Id);
                if (obj != null)
                {
                    Contractrepositoryviews.Remove(obj);
                }
            }
        }

        public event EventHandler RequestExportRegistry;

        public void OnRequestExportRegistry(EventArgs e)
        {
            EventHandler handler = RequestExportRegistry;
            if (handler != null) handler(this, e);
        }

        internal void ExportRegistry()
        {
            OnRequestExportRegistry(EventArgs.Empty);
        }

        public long? ContractdocId
        {
            get { return CurrentContractRepositoryView != null ? CurrentContractRepositoryView.Id : default(long?); }
        }

        public long? Maincontractid
        {
            get { return CurrentContractRepositoryView != null ? (long?)CurrentContractRepositoryView.Maincontractid : default(long?); }
        }

        public IEnumerable<IContractStateData> Source
        {
            get { return ActualContextItems.Cast<IContractStateData>(); }
        }

        public string ContractCaption
        {
            get { return CurrentContractRepositoryView.Return(x => "Д. №" + x.FullInternalnum, "Работа с договором"); }
        }
    }
}
