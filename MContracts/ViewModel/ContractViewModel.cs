using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using MContracts.Classes;
using MContracts.Classes.Converters;
using MContracts.Properties;
using MContracts.ViewModel.Helpers;
using MediatorLib;
using System;
using CommonBase;
using McUIBase.Factories;
using UIShared.ViewModel;

namespace MContracts.ViewModel
{
    /// <summary>
    /// Модель представления рабочей области работы с договором. Агрегирует модели для ввода данных по всем аспектам договора
    /// </summary>
    public class ContractViewModel : CompositeWorkspaceViewModel, IContractdocRefHolder, ICacheble, IContractCaption
    {
        private const string VM_FC = "VM_FC";
        private const string VM_NTP = "VM_NTP";
        private const string VM_ACTS = "VM_ACTS";
        private const string VM_SCHEDULE = "VM_SCHEDULE";
        private const string VM_GENERALS = "VM_GENERALS";
        private const string VM_AGREEMENTS = "VM_AGREEMENTS";
        private const string VM_SUBGENERALS = "VM_SUBGENERALS";
        private const string VM_CONTRACTCARD = "VM_CONTRACTCARD";
        private const string VM_RESULTS = "VM_RESULTS";
        private const string VM_PAYMENTDOCUMENTS = "VM_PAYMENTDOCUMENTS";
        private const string VM_APPROVALS = "VM_APPROVALS";
        private const string VM_DOCUMENTIMAGES = "VM_DOCUMENTIMAGES";
        private const string VM_CONTRACTORS = "VM_CONTRACTORS";
        private const string VM_DOCUMENTSET = "VM_DOCUMENTSET";


       
        /// <summary>
        /// Создаёт экземпляр модели представления
        /// </summary>
        /// <param name="repository">Репозитарий, используемый для получения объектов модели</param>
        public ContractViewModel(IContractRepository repository) : base(repository)
        {
            InitChildViewModels(repository);
            ViewMediator.Register(this);
        }

        [MediatorMessageSink(RequestRepository.REQUEST_GLOBAL_PROPERTIES_CHANGED, ParameterType = typeof(PropertyChangedEventArgs))]
        public void GlobalPropertiesChanged(PropertyChangedEventArgs e)
        {
            if (IsValidPropertyName(e.PropertyName))
                OnPropertyChanged(e.PropertyName);
        }

        public Visibility LeftPanelVisibility
        {
            get
            {
                return
                    (GeneralsViewModel.ListVisibility.ToBoolean() && Settings.Default.LeftPanelVisibility).
                        ToVisibility();                     
            }
        }

        public void LoadFrom(IEnumerable<Contractdoc> source)
        {

            SubgeneralsViewModel.LoadFrom(source);
            OnPropertyChanged(()=>RightPanelVisibility);
        }


        public Visibility RightPanelVisibility
        {
            get
            {
                return
                    SubgeneralsViewModel.ListVisibility.Or(AgreementsViewModel.ListVisibility).And(
                        Settings.Default.RightPanelVisibility.ToVisibility());
            }
        }

        /// <summary>
        /// Получает или устанавливает объект договора
        /// </summary>
        public Contractdoc Contractdoc
        {
            get { return WrappedDomainObject as Contractdoc; }
        }

        /// <summary>
        /// Получает модель представления карточки договора
        /// </summary>
        public ContractdocCardViewModel ContractCardCardViewModel
        {
            get { return Childs[VM_CONTRACTCARD] as ContractdocCardViewModel; }
        }

        /// <summary>
        /// Получает модель представления карточки договора
        /// </summary>
        public DocumentImageViewModel ContractDocumentImageViewModel
        {
            get { return Childs[VM_DOCUMENTIMAGES] as DocumentImageViewModel; }
        }
        /// <summary>
        /// Получает модель представления списка генеральных договоров
        /// </summary>
        public ContractsListViewModel GeneralsViewModel
        {
            get { return Childs[VM_GENERALS] as ContractsListViewModel; }
        }

        /// <summary>
        /// Получает модель представления списка доп соглашений 
        /// </summary>
        public ContractsListViewModel AgreementsViewModel
        {
            get { return Childs[VM_AGREEMENTS] as ContractsListViewModel; }
        }

        /// <summary>
        /// Получает модель представления списка субподрядных договоров
        /// </summary>
        public ContractsListViewModel SubgeneralsViewModel
        {
            get { return Childs[VM_SUBGENERALS] as ContractsListViewModel; }
        }

        public ContractApprovalViewModel ApprovalsViewModel
        {
            get
            {
                return Childs[VM_APPROVALS] as ContractApprovalViewModel; 
            }
        }

        public ContractorsViewModel ContractorsViewModel
        {
            get
            {
                return Childs[VM_CONTRACTORS] as ContractorsViewModel;
            }
        }

        /// <summary>
        /// Получает заголовок рабочей области
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return Contractdoc == null ? Resources.NewContractString : GetDisplayName();
            }

            protected set { base.DisplayName = value; }
        }


        /// <summary>
        /// Получает модель представления функциональных заказчиков
        /// </summary>
        public ContractFcViewModel FcViewModel
        {
            get { return Childs[VM_FC] as ContractFcViewModel; }
        }

        /// <summary>
        /// Получает модель представления актов 
        /// </summary>
        public ActsViewModel ActsViewModel
        {
            get { return Childs[VM_ACTS] as ActsViewModel; }
        }

        /// <summary>
        /// Получает модель представления результатов этапов 
        /// </summary>
        public StageResultsViewModel StageResultsViewModel
        {
            get { return Childs[VM_RESULTS] as StageResultsViewModel; }
        }

        /// <summary>
        /// Получает модель представления КП
        /// </summary>
        public ScheduleViewModel ScheduleViewModel
        {
            get { return Childs[VM_SCHEDULE] as ScheduleViewModel; }
        }


        /// <summary>
        /// Получает модель представления НТП
        /// </summary>
        public NtpProblemViewModel NtpProblemViewModel
        {
            get { return Childs[VM_NTP] as NtpProblemViewModel; }
        }

        /// <summary>
        /// Получает модель представления платёжных документов
        /// </summary>
        public PaymentDocumentsBaseViewModel PaymentDocumentsViewModel
        {
            get { return Childs[VM_PAYMENTDOCUMENTS] as PaymentDocumentsBaseViewModel; }
        }
        /// <summary>
        /// Получает модель представления Описи документов
        /// </summary>
        public DocumentSetTransferViewModel DocumentSetTransferModel
        {
            get { return Childs[VM_DOCUMENTSET] as DocumentSetTransferViewModel; }
        }
        private void InitChildViewModels(IContractRepository repository)
        {
            Childs.Clear();
            Childs.Add(VM_CONTRACTCARD, new ContractdocCardViewModel(repository, this)
                                            {
                                                IsMaximized = true
                                            });

            Childs.Add(VM_FC, new ContractFcViewModel(repository));
            Childs.Add(VM_NTP, new NtpProblemViewModel(repository));
            //Childs.Add(VM_ACTS, new ActsViewModel(repository));
            //Childs.Add(VM_SCHEDULE, new ScheduleViewModel(repository));
            Childs.Add(VM_RESULTS, new StageResultsViewModel(repository));
            Childs.Add(VM_PAYMENTDOCUMENTS, new PaymentDocumentsBaseViewModel(repository, this));
            Childs.Add(VM_APPROVALS, new ContractApprovalViewModel(repository, this));
            Childs.Add(VM_DOCUMENTIMAGES, new DocumentImageViewModel(repository, this));
            Childs.Add(VM_CONTRACTORS, new ContractorsViewModel(repository, this));
            Childs.Add(VM_DOCUMENTSET, new DocumentSetTransferViewModel(repository));

            Childs.Add(VM_GENERALS,
                       new ContractsListViewModel
                           {
                               Title = "Генеральные",
                               Tooltip = "Генеральные договора",
                               ImageResourceName = "/MContracts;component/Resources/General.png"
                           });
            Childs.Add(VM_AGREEMENTS,
                       new ContractsListViewModel
                           {
                               Title = "Соглашения",
                               Tooltip = "Дополнительные соглашения",
                               ImageResourceName = "/MContracts;component/Resources/stargreen.ico"
                           });
            Childs.Add(VM_SUBGENERALS,
                       new ContractsListViewModel
                           {
                               Title = "Соисполнители",
                               Tooltip = "Договора с соисполнителями",
                               ImageResourceName = "/MContracts;component/Resources/subcontract.ico"
                           });
        }

        protected override void OnWrappedDomainObjectChanged()
        {
            SetViewModelsContractdoc(WrappedDomainObject as Contractdoc);
            base.OnWrappedDomainObjectChanged();
        }

        private void SetViewModelsContractdoc(Contractdoc value)
        {
            FcViewModel.WrappedDomainObject = value;
            NtpProblemViewModel.WrappedDomainObject = value;
            //ScheduleViewModel.WrappedDomainObject = value;
            ContractCardCardViewModel.ContractObject = value;
            StageResultsViewModel.WrappedDomainObject = value;
            //ActsViewModel.WrappedDomainObject = value;
            ContractDocumentImageViewModel.ContractObject = value;
            ContractorsViewModel.ContractObject = value;
            
            GeneralsViewModel.LoadFrom(value.OriginalGenerals);
            AgreementsViewModel.LoadFrom(value.AllAgreements);
            SubgeneralsViewModel.LoadFrom(value.SubContracts);

            // Платёжные документы проходят все по основной версии договора
            PaymentDocumentsViewModel.WrappedDomainObject = value.MainContract;
            
            ApprovalsViewModel.WrappedDomainObject = value;
            DocumentSetTransferModel.WrappedDomainObject = value;
            DocumentSetTransferModel.DocumentSetHolder = new ContractDocumentSetHolder(DocumentSetTransferModel.Repository, value);

            OnPropertyChanged(()=>RightPanelVisibility);
            OnPropertyChanged(()=>LeftPanelVisibility);

            Invalidate();
        }

        protected override void SaveState()
        {
            //System.Diagnostics.Debug.WriteLine("Модель представления для КП InstanceId={0} деактивирована", ScheduleViewModel.InstanceId);
        }

        protected override void RestoreState()
        {
            //ScheduleViewModel.RefreshWrappedDomainObject();
            //System.Diagnostics.Debug.WriteLine("Модель представления для КП InstanceId={0} активирована", ScheduleViewModel.InstanceId);
        }

        [MediatorMessageSink(RequestRepository.REFRESH_SUBGENERALS, ParameterType = typeof(Contractdoc))]
        private void RefreshSubgeneralsViewModel(Contractdoc value)
        {
            Childs.Remove(VM_SUBGENERALS);
            Childs.Add(VM_SUBGENERALS,
               new ContractsListViewModel
               {
                   Title = "Соисполнители",
                   Tooltip = "Договора с соисполнителями",
                   ImageResourceName = "/MContracts;component/Resources/subcontract.ico",
                   
               });
            OnPropertyChanged(()=>SubgeneralsViewModel);


            SubgeneralsViewModel.LoadFrom(value.SubContracts);
            OnPropertyChanged(()=>RightPanelVisibility);
            SubgeneralsViewModel.SendPropertyChanged("ListVisibility");
            SubgeneralsViewModel.SendPropertyChanged("Contracts");
        }

        [MediatorMessageSink(RequestRepository.REFRESH_GENERALS, ParameterType = typeof(Contractdoc))]
        private void RefreshGeneralsViewModel(Contractdoc value)
        {
           
            OnPropertyChanged(()=>GeneralsViewModel);


            GeneralsViewModel.LoadFrom(value.OriginalGenerals);
            OnPropertyChanged(()=>LeftPanelVisibility);
            GeneralsViewModel.SendPropertyChanged("ListVisibility");
            GeneralsViewModel.SendPropertyChanged("Contracts");
        }


        /// <summary>
        /// Реализация сохранения договора
        /// </summary>
        protected override void Save()
        {
            Contract.Assert(Repository != null);
            Contract.Assert(Contractdoc != null);
            //base.Save();
            
            //Если в репозитории нет контракта, значит добавляется новый контракт
            if (!Repository.Contracts.Contains(Contractdoc))
            {
                Repository.AddContract(Contractdoc);
            }
            Repository.DebugPrintRepository();     
            
            Repository.SubmitChanges();
            UpdateContractdocStatistics();

           
       
        }

        private void UpdateContractdocStatistics()
        {
            using (var ctx = RepositoryFactory.CreateContractRepository())
            {
                var c = ctx.GetContractdoc(Contractdoc.Id);
                c.UpdateFundsStatistics();
                ctx.SubmitChanges();
            }
        }

        /// <summary>
        /// Переопределите для проверки возможности сохранения модели
        /// </summary>
        /// <returns></returns>
        protected override bool CanSave()
        {
            return Childs.Where(viewModelBase => viewModelBase.Value is ContractdocBaseViewModel).
                Aggregate(true, (current, viewModelBase) => current && (viewModelBase.Value as ContractdocBaseViewModel).SaveCommand.CanExecute(null));
        }

        public override string Error
        {
            get
            {
                var error = Childs.Aggregate(String.Empty, (current, viewModelBase) => 
                    current + (viewModelBase.Value as ViewModelBase).Error);

                if (error != String.Empty)
                    return error.Trim('\n');

                return null;
            }
        }

        private string GetDisplayName()
        {
            Contract.Requires(Contractdoc != null);
            return ContractdocConverter.GetContractdocName(Contractdoc);
            //return ContractCardCardViewModel.DisplayName;
        }

        
        public void Invalidate()
        {
            OnPropertyChanged(() => DisplayName);
            OnPropertyChanged(() => ContractCaption);
        }

        public long? ContractdocId
        {
            get
            {
                var obj = WrappedDomainObject as Contractdoc;
                return obj != null ? obj.Id : default(long?);
            }
        }

        public long? Maincontractid { get
            {
                var obj = WrappedDomainObject as Contractdoc;
                return obj != null ? obj.Maincontractid : default(long?);
            } }

        public string ContractCaption
        {
            get { return DisplayName; }
        }

    }
}