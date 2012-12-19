using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public sealed class InclusiveSet<T> : BaseSetImpl<T>, ISet<T>
    {
        #region Fields

        private readonly HashSet<T> includedElements;

        #endregion

        #region Constructors

        public InclusiveSet(params T[] includedElements)
            : this((IEnumerable<T>)includedElements)
        {
        }

        public InclusiveSet(IEnumerable<T> includedElements)
        {
            //Validation
            if (includedElements == null)
                throw new ArgumentNullException("includedElements");


            this.includedElements = new HashSet<T>(includedElements);
        }

        #endregion

        #region Implentation Of ISet<T>

        public override bool Contains(T element)
        {
            return this.includedElements.Contains(element);
        }
        
        #endregion
        
        #region Overrides of BaseSetImpl<T>

        public override ISet<T> Union(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            //Short-cut method to union two inclusive sets.
            // There is no advantage to be gain in setting IsNativeUnion to true, as
            // this method is only useful if both objects are InclusiveSets. If this
            // is the case, nothing is to be gained by making the call commutative.
            // An infinite loop can result from calling base.Union if IsNativeUnion
            // is set.
            var otherInclusive = other as InclusiveSet<T>;
            if (otherInclusive != null)
            {
                var newElements = this.includedElements
                    .Concat(otherInclusive.includedElements);

                return new InclusiveSet<T>(newElements);
            }

            return base.Union(other);
        }

        public override ISet<T> Intersect(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            var newElements = this.includedElements
                .Where(element => other.Contains(element));

            return new InclusiveSet<T>(newElements);
        }

        public override ISet<T> Subtract(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            var newElements = this.includedElements
                .Where(element => !other.Contains(element));

            return new InclusiveSet<T>(newElements);
        }

        protected override bool IsNativeIntersect
        {
            get { return true; }
        }

        protected override bool IsNativeSubtract
        {
            get { return true; }
        }

        #endregion

    }
}
