using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCDomain.DataAccess
{
    public class ContextEventArgs<T>: EventArgs where T : class
    {
        public T Context { get; private set; }
        public ContextEventArgs(T context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            Context = context;
        } 
    }
}
