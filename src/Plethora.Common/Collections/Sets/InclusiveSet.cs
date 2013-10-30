using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public sealed class InclusiveSet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        #region Fields

        internal readonly HashSet<T> includedElements;

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

        private InclusiveSet(HashSet<T> includedElements)
        {
            this.includedElements = includedElements;
        }

        #endregion

        #region Implentation Of ISetCore<T>

        public override bool Contains(T element)
        {
            return this.includedElements.Contains(element);
        }

        public override bool? IsEmpty
        {
            get { return (this.includedElements.Count == 0); }
        }

        #endregion
        
        #region Overrides of BaseSetImpl<T>

        public override ISetCore<T> Union(ISetCore<T> other)
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

                HashSet<T> newHashSet = new HashSet<T>(newElements);
                if (newHashSet.Count == 0)
                    return EmptySet<T>.Instance;

                if (newHashSet.Count == this.includedElements.Count)
                    return this;

                return new InclusiveSet<T>(newHashSet);
            }

            return base.Union(other);
        }

        public override ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            var newElements = this.includedElements
                .Where(element => other.Contains(element));

            HashSet<T> newHashSet = new HashSet<T>(newElements);
            if (newHashSet.Count == 0)
                return EmptySet<T>.Instance;

            if (newHashSet.Count == this.includedElements.Count)
                return this;

            return new InclusiveSet<T>(newHashSet);
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");

            var newElements = this.includedElements
                .Where(element => !other.Contains(element));

            HashSet<T> newHashSet = new HashSet<T>(newElements);
            if (newHashSet.Count == 0)
                return EmptySet<T>.Instance;

            if (newHashSet.Count == this.includedElements.Count)
                return this;

            return new InclusiveSet<T>(newHashSet);
        }

        public override ISetCore<T> Inverse()
        {
            return new ExclusiveSet<T>(this.includedElements);
        }

        protected override bool IsNativeIntersect
        {
            get { return true; }
        }

        #endregion

        public IEnumerable<T> IncludedElements
        {
            get { return this.includedElements; }
        }
    }
}
