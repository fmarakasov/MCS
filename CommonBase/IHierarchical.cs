using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    public interface IHierarchical
    {
        int Level { get; }
        object Parent { get; set; }
    }
}
