using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    public class ContractArgs :EventArgs
    {
        public IContractStateData ContractState { get; private set; }
        
        public ContractArgs(IContractStateData contractState)
        {
            ContractState = contractState;
        }
    }
}
