using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    public interface IPropertiesCollection
    {
        IDictionary<string, object> GetProperties();
    }
}
