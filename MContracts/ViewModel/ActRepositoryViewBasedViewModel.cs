using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.ViewModel.Helpers;
using McUIBase.Factories;
using McUIBase.ViewModel;
using MediatorLib;
using UIShared.Commands;
using UOW;

namespace MContracts.ViewModel
{
    public class ActRepositoryViewBasedViewModel : WorkspaceViewModel, IContractdocRefHolder, IContractCaption
    {
 
        /// <summary>
        /// Модель представления для панели команд
        /// </summary>
        public class ActsRepositoryCommandsAggregatorViewModel : BasicCommandsAggregatorViewModel<Actrepositoryview> 
        {
            /// <summary>
            /// Получает модель представления репозитария актов
            /// </summary>
            public ActRepositoryViewBasedViewModel ViewModel { get; private set; }

            /// <summary>
            /// Создаёт экзмемпляр модели представления
            /// </summary>
            /// <param name="viewModel"></param>
            public ActsRepositoryCommandsAggregatorViewModel(ActRepositoryViewBasedViewModel viewModel)
            {
                Contract.Requires(viewModel!=null);
                ViewModel = viewModel;
            }

            [ApplicationCommand("Сделать подписанными","",AppCommandType.Silent, "", SeparatorType.Before)]
            public ICommand SignSelectedActs
            {
                get { return new RelayCommand(x=>SetSelectedActsSigned(true), (x)=>HasSelectedActs); }
            }

            protected bool HasSelectedActs
            {
                get { return Selected != null; }
    
            }

            private void SetSelectedActsSigned(bool signOrUnsign)
            {
                
            }
        }

        private readonly ActsRepositoryCommandsAggregatorViewModel _commandsAggregator;
        private ObservableCollection<Actrepositoryview> _acts;

        /// <summary>
        /// Получает или устанавливает событие для изменения заданного акта
        /// </summary>
        public event EventHandler<EventParameterArgs<long>> RequestUpdateAct;
        
        /// <summary>
        /// Получает или устанавливает событие для создания нового акта. Созданный акт возвращается через 
        /// параметр события
        /// </summary>
        public event EventHandler<EventParameterArgs<Act>> RequestNewAct;

        public void OnRequestNewAct(EventParameterArgs<Act> e)
        {
            var handler = RequestNewAct;
            if (handler != null) handler(this, e);
        }

        public void OnRequestUpdateAct(EventParameterArgs<long> e)
        {
            var handler = RequestUpdateAct;
            if (handler != null) handler(this, e);
        }

        public ActRepositoryViewBasedViewModel(IContractRepository repository) : base(repository)
        {
            var uof = repository.UnitOfWork;

            IsUnchangable = true;
            ViewMediator.Register(this);

            _commandsAggregator = new ActsRepositoryCommandsAggregatorViewModel(this)
                {
                    //LoadEntities = () => Repository.Actsrepositoryview,
                    LoadEntities = () => uof.Repository<Actrepositoryview>().ToList(),
                    CreateEntity = () => CreateNewAct(),
                    UpdateEntity = x => UpdateAct(x),
                    DeleteEntity = x => DeleteAct(x)
                };
        }

        private IList<Nds> _nds;
        private IList<Currencymeasure> _measures;
        private IList<Currency> _currencies;
        private IList<Ndsalgorithm> _ndsalgorithms; 

        //[MediatorMessageSink(RequestRepository.REFRESH_ACTS)]
        //public void OnActUpdatedMessage(Act act)
        //{
        //    Contract.Requires(act != null);
        //    var oldObj = Acts.SingleOrDefault(x => x.Id == act.Id);
            
        //    using (var repository = RepositoryFactory.CreateContractRepository())
        //    {
        //        var updated = repository.Actsrepositoryview.Single(x => x.Id == act.Id);
        //        if (oldObj == null)
        //        {
        //            if (updated != null)
        //            {
        //                Acts.Add(updated);
        //            }
        //        }
        //        else if (updated != null)
        //        {
        //            var pos = Acts.IndexOf(oldObj);
        //            Acts[pos] = updated;
        //        }
                
                    
        //    }
        //}

        public override string DisplayName
        {
            get
            {
                return "Реестр актов";
            }
            protected set
            {
                base.DisplayName = value;
            }
        }
        public ObservableCollection<Actrepositoryview> Acts
        {
            get { return _acts ?? (_acts = new ObservableCollection<Actrepositoryview>(FetchActs())); }
        }

        private IEnumerable<Actrepositoryview> FetchActs()
        {
            //TODO : нужно уточнить запрос, что бы не грузить все акты системы за раз
            var result = Repository.UnitOfWork.Repository<Actrepositoryview>().ToList();
            result.Apply(CreateActMoneyModel);
            return result;
        }

        private void CreateActMoneyModel(Actrepositoryview x)
        {
            x.TransferSumMoney = GetPriceInfoMoneyModel(x, x.Sumfortransfer);
            x.ActMoney = GetPriceInfoMoneyModel(x, x.Totalsum);
        }

        private MoneyModel GetPriceInfoMoneyModel(Actrepositoryview actrepositoryview, decimal? price)
        {
            var currency = Currencies.Single(x => x.Id == actrepositoryview.Currencyid);
            var nds = NDS.Single(x => x.Id == actrepositoryview.Ndsid);
            var ndsalgorithm = Ndsalgorithms.Single(x => x.Id == actrepositoryview.Ndsalgorithmid);
            var measure = Measures.Single(x => x.Id == actrepositoryview.Currencymeasureid);
            return new MoneyModel(ndsalgorithm, nds, currency, measure, price, actrepositoryview.Currencyrate).National.Factor;
        }

        private void DeleteAct(Actrepositoryview act)
        {
            Contract.Requires(act != null);
            Repository.UnitOfWork.Repository<Act>().Delete(x=>x.Id == act.Id);

        }

        private void UpdateAct(Actrepositoryview act)
        {
            var args = new EventParameterArgs<long>(act.Id);
            RequestUpdateAct(this, args);
        }

        private Actrepositoryview CreateNewAct()
        {
            var args = new EventParameterArgs<Act>(null);
            RequestNewAct(this, args);
            return args.Parameter != null ?
                Repository.UnitOfWork.Repository<Actrepositoryview>().Single(x => x.Id == args.Parameter.Id) : null;
        }

        protected override void Save()
        {
            
        }

        protected override bool CanSave()
        {
            return true;
        }

        public override bool IsClosable
        {
            get { return true; }
        }



        public ActsRepositoryCommandsAggregatorViewModel CommandsAggregator
        {
            get
            {
                return _commandsAggregator;
            }
        }
        public Actrepositoryview CurrentAct { get; set; }
        public long? ContractdocId
        {
            get { return CurrentAct.Return(x => (long?)x.Contractdocid, default(long?)); }
        }

        public long? Maincontractid
        {
            get { return CurrentAct.Return(x => (long?) x.Maincontractid, default(long?)); }
        }

        public string ContractCaption { get { return "Работа с реестром актов"; } }

        public IList<Nds> NDS
        {
            get { return _nds??(_nds = Repository.Nds.ToList()); }
        }

        public IList<Currencymeasure> Measures
        {
            get { return _measures??(_measures = Repository.Currencymeasures.ToList()); }
        }

        public IList<Currency> Currencies
        {
            get { return _currencies?? (_currencies = Repository.Currencies.ToList()); }
        }

        public IList<Ndsalgorithm> Ndsalgorithms
        {
            get { return _ndsalgorithms??(_ndsalgorithms = Repository.Ndsalgorithms.ToList()); }
        }
    }
}
