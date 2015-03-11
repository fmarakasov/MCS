using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.Classes
{
    public enum ActionType
    {
        Edit,
        Add
    }
    
    public class EditWindowShowedEventsArgs: EventArgs
    {
        public CatalogType CatalogType { get; set; }

        public ActionType ActionType { get; set; }

        public EditWindowShowedEventsArgs(CatalogType type1, ActionType type2): base()
        {
            this.CatalogType = type1;
            this.ActionType = type2;
        }
    }
}
