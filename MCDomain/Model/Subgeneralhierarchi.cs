using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public partial class Subgeneralhierarchi
    {
        public override string ToString()
        {
            return this.Generalcontractdocstageid + " - " + Subcontractdocstageid;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            var sghi = (obj as Subgeneralhierarchi);
            if (sghi != null)
                return this.Subcontractdocstageid == sghi.Subcontractdocstageid &&
                       this.Generalcontractdocstageid == sghi.Generalcontractdocstageid;
            return false;
        }


        public Stage Substage
        {
            get { return Stage_Subcontractdocstageid; }
            set
            {
                Stage_Subcontractdocstageid = value;
                SendPropertyChanged("Substage");
            }
        }

        public Stage Generalstage
        {
            get { return Stage_Generalcontractdocstageid; }
            set 
            {
                Stage_Generalcontractdocstageid = value; 
                SendPropertyChanged("Generalstage");
            }
        }




    }
}
