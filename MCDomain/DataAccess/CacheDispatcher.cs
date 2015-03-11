using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MCDomain.DataAccess
{
    class CacheDispatcher : IDisposable
    {
        public IContractRepository Repository { get; private set; }

        public CacheDispatcher(IContractRepository repository)
        {
            Contract.Requires(repository != null);
            Repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateContractdocStatistics(long contractId)
        {
            var contract = Repository.GetContractdoc(contractId);
            Contract.Assert(contract != null);
        }

        public void Dispose()
        {
            Repository.Dispose();
        }
    }
}
