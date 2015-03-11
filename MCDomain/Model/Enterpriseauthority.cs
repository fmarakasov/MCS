using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.Common;
using CommonBase;

namespace MCDomain.Model
{
    partial class Enterpriseauthority: IObjectId
    {

        public bool IsWellKnownId()
        {
            return false;
        }

        public bool? Valid 
        { 
            get { return Isvalid; } 
            set
            {
                if (Isvalid == value) return;
                Isvalid = value;
                SendPropertyChanged("Valid");
            } 
        }

        public override string ToString()
        {
           var sb = new StringBuilder();
           if (Authority != null) sb.Append(Authority);
           if (!string.IsNullOrWhiteSpace(Num)) sb.Append(string.Format(" №{0}", Num));
           if (Employee != null) sb.Append(string.Format(", выдано {0}", Employee));
           if (Validfrom.HasValue) sb.Append(string.Format(", действует с {0:d}", Validfrom.Value));
           if (Validto.HasValue) sb.Append(string.Format(" по {0:d}", Validto.Value));
            sb.Append(string.Format(" ({0})", Isvalid.GetValueOrDefault() ? "Действует" : "Не действует"));
            return sb.ToString();

        }
    }
}
