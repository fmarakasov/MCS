using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.Common
{
    public interface IHierarchical
    {
        int Level { get; }
        object Parent { get; set; }
    }
}
