using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using MCDomain.DataAccess;
using MCDomain.Model;
using CommonBase;

namespace MContracts.Classes
{
    /// <summary>
    /// Реализация управления описью документов к договору
    /// </summary>
    public class ContractDocumentSetHolder : RepositoryHolder, IDocumentSetHolder
    {
        public Contractdoc ContractObject { get; private set; }

        public ContractDocumentSetHolder(IContractRepository repository, Contractdoc contractdoc) : base(repository)
        {
            Contract.Requires(contractdoc != null);
            ContractObject = contractdoc;
        }

        public bool DocumentSetCreated
        {
            get
            {
                Contract.Assert(ContractObject!=null);
                return ContractObject.Contracttranactdocs.Any();
            }
        }

        public IEnumerable<IDocumentSetEntry> GetDocumentSet()
        {
            Contract.Assert(DocumentSetCreated);
            Contract.Assert(ContractObject != null);
            return ContractObject.Contracttranactdocs;
        }

        public void CreateDocumentSet()
        {
            Contract.Assert(!DocumentSetCreated);
            Contract.Assert(ContractObject != null);
            foreach (var newDocument in 
                Repository.Documents.Where(x=>x.Id!=EntityBase.ReservedUndifinedOid).OrderBy(x=>x.Name).Select(document => new Contracttranactdoc(){Document = document}))
            {
                ContractObject.Contracttranactdocs.Add(newDocument);
            }
        }

        public void DeleteDocumentSet()
        {
            Contract.Assert(ContractObject != null);
            ContractObject.Contracttranactdocs.Clear();
        }

        public void BindToAct(Transferact act)
        {
            Contract.Requires(act != null);
            Contract.Assert(DocumentSetCreated);
            Contract.Assert(ContractObject != null);
            ContractObject.Contracttranactdocs.Apply(x => x.Transferact = act);

        }

        public Transferacttype Transferacttype
        {
            get { return Repository.Transferacttypes.Single(x => x.Id == (long) WellKnownTransferActtypes.TransferContract); }
        }

        public Transferact Transferact
        {
            get
            {
                return ContractObject.Contracttranactdocs.FirstOrDefault().Return(x => x.Transferact, null);
            }
        }

        #region Члены IDocumentSetHolder


        #endregion
    }
}