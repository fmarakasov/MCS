using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace MContracts.Classes
{
    public class OpenedCollectionViewGroup
    {
        public CollectionViewGroup group { get; set; }
        public bool IsOpen { get; set; }
    }
}
