using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CommonBase
{
    public interface IClonableRecursive
    {
        object CloneRecursively(object owner, object source);
    }
}
