using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using CommonBase;
using MCDomain.DataAccess;
using MCDomain.Model;
using McUIBase.ViewModel;
using UIShared.Commands;

namespace MContracts.ViewModel
{
    public class DocumentSetTransferViewModel : RepositoryViewModel
    {
        public DocumentSetTransferViewModel(IContractRepository repository) : base(repository)
        {
            
        }

        private IDocumentSetHolder _documentSetHolder;

        /// <summary>
        /// Получает объект IDocumentSetHolder, ответственный за работу с описью документов для заданного типа
        /// </summary>
        public IDocumentSetHolder DocumentSetHolder
        {
            get { return _documentSetHolder; }
            set { 
                Contract.Requires(value != null);
                if (_documentSetHolder == value) return;
                _documentSetHolder = value;
                _documentSet = null;
                OnPropertyChanged(()=>DocumentSetHolder);
            }
        }

        private ObservableCollection<IDocumentSetEntry> _documentSet;
        public ObservableCollection<IDocumentSetEntry> DocumentSet
        {
            get
            {
                Contract.Assert(DocumentSetHolder != null);
                if (!DocumentSetHolder.DocumentSetCreated) return null;
                return _documentSet ??
                       (_documentSet = new ObservableCollection<IDocumentSetEntry>(DocumentSetHolder.GetDocumentSet()));
            }
        }
       
        [ApplicationCommand("Создать опись документов", "/MContracts;component/Resources/act_add.png")]
        public ICommand CreateDocumentSetCommand
        {
            get { return new RelayCommand(x => CreateDocumentSet(), x => CanCreateDocumentSet()); }
        }

        [ApplicationCommand("Добавить в акт передачи", "/MContracts;component/Resources/archive_add.png")]
        public ICommand BindToActCommand
        {
            get { return new RelayCommand(x => BindToAct(), x => !CanCreateDocumentSet()); }
        }

        public event EventHandler<EventParameterArgs<TransferActSelector>> SelectTransferActRequest;

        protected virtual void OnSelectTransferActRequest(EventParameterArgs<TransferActSelector> e)
        {
            EventHandler<EventParameterArgs<TransferActSelector>> handler = SelectTransferActRequest;
            if (handler != null) handler(this, e);
        }

        private void BindToAct()
        {
            var args = new EventParameterArgs<TransferActSelector>(new TransferActSelector(DocumentSetHolder.Transferacttype));
            OnSelectTransferActRequest(args);
            if (args.Parameter.Act == null) return;
            DocumentSetHolder.BindToAct(args.Parameter.Act);
            OnPropertyChanged(()=>Transferact);
        }

        public Transferact Transferact
        {
            get { return DocumentSetHolder.Transferact; }
       
        }

        [ApplicationCommand("Удалить опись документов", "/MContracts;component/Resources/act_delete.png", AppCommandType.Confirm, "Продолжить удаление описи документов")]
        public ICommand DeleteDocumentSetCommand
        {
            get { return new RelayCommand(x => DeleteDocumentSet(), x => !CanCreateDocumentSet()); }
        }
        

        
        private void DeleteDocumentSet()
        {
            Contract.Assert(DocumentSetHolder != null);
           DocumentSetHolder.DeleteDocumentSet();
            _documentSet = null;
            OnPropertyChanged(()=>DocumentSet);
        }

        private bool CanCreateDocumentSet()
        {
            Contract.Assert(DocumentSetHolder != null);
            return !DocumentSetHolder.DocumentSetCreated;
        }

        private void CreateDocumentSet()
        {
            Contract.Assert(DocumentSetHolder != null);
            DocumentSetHolder.CreateDocumentSet();
            OnPropertyChanged(()=>DocumentSet);
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
