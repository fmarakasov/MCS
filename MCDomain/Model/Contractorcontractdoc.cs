using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonBase;

namespace MCDomain.Model
{
    partial class Contractorcontractdoc
    {
        public override string ToString()
        {
            return Contractor.With(x=>x.ToString());
        }
    }
}
