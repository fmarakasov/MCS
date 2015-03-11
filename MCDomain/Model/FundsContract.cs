using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    [ContractClassFor(typeof(IFundsResolver))]
    abstract class FundsContract : IFundsResolver
    {        

        public decimal GetFundsDisbursed(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            Contract.Requires(contract != null);
            Contract.Requires(toDate >= fromDate);
            Contract.Ensures(Contract.Result<decimal>()>=0.0M);
            return default(decimal);
        }

        public decimal GetFundsLeft(Contractdoc contract, DateTime fromDate, DateTime toDate)
        {
            Contract.Requires(contract != null);
            Contract.Requires(toDate >= fromDate);
            Contract.Ensures(Contract.Result<decimal>() >= 0.0M);
            return default(decimal);
        }
    }
}
