using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCDomain.DataAccess;

namespace MCDomain.Common
{
    public interface IClonableRecursive
    {
        object CloneRecursively(object owner, object source);
    }
}
