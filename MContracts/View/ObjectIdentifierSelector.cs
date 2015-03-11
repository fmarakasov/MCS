using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MContracts.View
{
    public class ObjectIdentifierSelector
    {
        public long? Id { get; set; }
        public object Item { get; private set; }
        public ObjectIdentifierSelector(object item)
        {
            Item = item;
        }


    }
}
