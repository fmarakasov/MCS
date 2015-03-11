using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Model;

namespace MContracts.ViewModel
{
    public class TransferActSelector
    {
        public Transferacttype PrefferedActType { get; private set; }
        public Transferact Act { get; set; }

        public TransferActSelector(Transferacttype prefferedActType)
        {
            PrefferedActType = prefferedActType;
        }
    }
}
