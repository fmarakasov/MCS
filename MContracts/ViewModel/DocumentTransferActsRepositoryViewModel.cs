using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;
using McUIBase.Factories;
using McUIBase.ViewModel;
using MCDomain.Model;
using CommonBase;

namespace MContracts.ViewModel
{
    public class Documenttransactdto
    {
        public long? Id { get; set; }
        public long? Num { get; set; }
        public string Caption { get; set; }
        public DateTime? ActSignDate { get; set; }
        public string Acttype { get; set; }
        public string DocumentName { get; set; }
        public long Documentid { get; set; }
        public long? PagesCount { get; set; }
        public long? Actid { get; set; }
        public long? Contractdocid { get; set; }
        public TrandocType DocumentType { get; set; }
    }

    public class DocumentTransferActsRepositoryViewModel : WorkspaceViewModel, IContractdocRefHolder, IContractCaption
    {
        public DocumentTransferActsRepositoryViewModel(IContractRepository repository) : base(repository)
        {
            IsUnchangable = true;
        }

        private ObservableCollection<Documenttransactdto> _documenttransacts;

        public ObservableCollection<Documenttransactdto> Documenttransacts
        {
            get
            {
                if (_documenttransacts == null)
                {
                    using (var repository = RepositoryFactory.CreateContractRepository())
                    {
                        _documenttransacts = new ObservableCollection<Documenttransactdto>
                            (repository.Contracttrandocs.Select(
                                x =>
                                new Documenttransactdto()
                                    {
                                        ActSignDate = x.Transferact.Return(t => t.Signdate, null),
                                        Acttype = x.Transferact.Return(t=>t.Transferacttype.Name, "Не задан"),
                                        Caption = x.ToString(),
                                        Actid = x.Actid,
                                        Contractdocid = x.Contractdocid,
                                        DocumentName = x.Document.ToString(),
                                        Documentid = x.Document.Id,
                                        Num = x.Transferact.Return(t=>t.Num, null),
                                        PagesCount = x.Pagescount,
                                        DocumentType = x.DocType
                                    }));
                    }
                }
                return _documenttransacts;
            }
        }

        private Documenttransactdto _selected;

        public Documenttransactdto Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value) return;
                _selected = value;
                OnPropertyChanged(()=>Selected);
            }
        }

        protected override void Save()
        {
           
        }

        protected override bool CanSave()
        {
            return false;
        }

        public override bool IsClosable
        {
            get { return true; }
        }
        public long? Actid { get { return Selected.Return(x => x.Actid, default(long?)); } }
        
        #region IContractRefHolder, IContractCaption
        
        public long? ContractdocId { get { return Selected.Return(x => x.Contractdocid, default(long?)); } }
        public long? Maincontractid { get {throw new NotImplementedException();} }

        public string ContractCaption { get { return Selected.Return(x => x.Caption, "Договор"); } }
        
        #endregion
    }
}
