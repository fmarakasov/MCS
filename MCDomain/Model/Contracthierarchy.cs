using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public partial class Contracthierarchy
    {
        public Contractdoc GeneralContractdoc
        {
            get { return Contractdoc_Generalcontractdocid; }
            set
            {
                Contractdoc_Generalcontractdocid = value;
                SendPropertyChanged("GeneralContracdoc");
            }
        }


        public Contractdoc SubContractdoc
        {
            get { return Contractdoc_Subcontractdocid; }
            set
            {
                Contractdoc_Subcontractdocid = value;
                SendPropertyChanged("SubContractdoc");
            }
        }


    }
}
