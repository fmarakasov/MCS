using System;
using System.Collections.Generic;
using MCDomain.DataAccess;
using MCDomain.Model;

namespace MContracts.Classes
{
    class ActDocumentSetHolder : IDocumentSetHolder
    {
        public bool DocumentSetCreated { get; private set; }
        public void CreateDocumentSet()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDocumentSetEntry> GetDocumentSet()
        {
            throw new NotImplementedException();
        }

        public void DeleteDocumentSet()
        {
            throw new NotImplementedException();
        }

        public void BindToAct(Transferact act)
        {
            throw new NotImplementedException();
        }

        public Transferacttype Transferacttype { get; private set; }
        public Transferact Transferact { get; private set; }
    }
}