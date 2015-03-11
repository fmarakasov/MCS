using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.Classes
{
    public class SubContractEventArgs: EventArgs
    {
        private Contractdoc contract;

        public Contractdoc Contract
        {
            get { return contract; }
        }
        
        public SubContractEventArgs(Contractdoc C)
        {
            contract = C;
        }
    }
}
