using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonBase
{
    public abstract class BinarySpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        protected BinarySpecification(Specification<T> left, Specification<T> right)
        {            
            this._left = left;
            this._right = right;
        }
        protected Specification<T> Left { get { return _left; } }
        protected Specification<T> Right { get { return _right; } }
    }

    public class AndSpecification<T> : BinarySpecification<T>
    {
        public AndSpecification(Specification<T> left, Specification<T> right) : base(left, right){ }

        public override bool IsSatisfied(T context)
        {
            return Left.IsSatisfied(context) && Right.IsSatisfied(context);
        }
    }

    public class OrSpecification<T> : BinarySpecification<T>
    {
        public OrSpecification(Specification<T> left, Specification<T> right) : base(left, right) { }

        public override bool IsSatisfied(T context)
        {
            return Left.IsSatisfied(context) || Right.IsSatisfied(context);
        }
    }

    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _spec;
        public NotSpecification(Specification<T> spec)
        {
            _spec = spec;
        }
        public override bool IsSatisfied(T context)
        {
            return !_spec.IsSatisfied(context);
        }
    }
}
