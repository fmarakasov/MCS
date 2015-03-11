using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Model
{
    public partial class Closedstagerelation
    {
        public Stage ClosedStage
        {
            get { return Stage_Closedstageid; }
            set
            {
                Stage_Closedstageid = value;
                SendPropertyChanged("Stage");
            }
        }
    }
}
