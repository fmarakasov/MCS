using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    abstract public class Specification<T>
    {
        public abstract bool IsSatisfied(T context);

        public Specification<T> And(Specification<T> spec)
        {
            return new AndSpecification<T>(this, spec);    
        }

        public Specification<T> Or(Specification<T> spec)
        {
            return new OrSpecification<T>(this, spec);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
    
