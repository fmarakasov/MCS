using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    partial class Contracttrouble
    {
        public override string ToString()
        {
            if (Trouble != null)
            {
                return Trouble.ToString();
            }
            return string.Empty;
        }
    }
}
