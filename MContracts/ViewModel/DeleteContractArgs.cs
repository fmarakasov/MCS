using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    public class DeleteContractArgs : CancelEventArgs
    {
        public IContractStateData ContractState { get; private set; }
        public DeleteContractArgs(IContractStateData contractStateData)
        {
            Contract.Requires(contractStateData != null);
            ContractState = contractStateData;
        }

    }
}
