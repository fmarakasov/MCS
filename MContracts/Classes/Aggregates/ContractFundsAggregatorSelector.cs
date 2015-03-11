using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Data;

namespace MContracts.Classes.Aggregates
{
    public class ContractFundsAggregatorSelector : EnumerableSelectorAggregateFunction
    {
        protected override string AggregateMethodName
        {
            get { return "AggregateFunds"; }
        }

        protected override Type ExtensionMethodsType
        {
            get { return typeof (ContractFundsAggregator); }
        }
    }
}
