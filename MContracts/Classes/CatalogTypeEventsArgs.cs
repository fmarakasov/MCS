using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.Classes
{
    public class CatalogTypeEventsArgs: EventArgs
    {
        public CatalogType CatalogType { get; set; }

        public CatalogTypeEventsArgs(CatalogType type) : base()
        {
            CatalogType = type;
        }
    }
}
