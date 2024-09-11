using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public sealed class InclusiveSet<T> : BaseSetImpl<T>
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
            ArgumentNullException.ThrowIfNull(includedElements);


            this.includedElements = new(includedElements);
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
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to intersect well-known sets.
            if (this.TryWellKnownUnion(other, out var result))
                return result;


            //Short-cut method to union two inclusive sets.
            // There is no advantage to be gain in setting IsNativeUnion to true, as
            // this method is only useful if both objects are InclusiveSets. If this
            // is the case, nothing is to be gained by making the call commutative.
            // An infinite loop can result from calling base.Union if IsNativeUnion
            // is set.
            if (other is InclusiveSet<T> otherInclusive)
            {
                var newElements = this.includedElements
                    .Concat(otherInclusive.includedElements);

                HashSet<T> newHashSet = new(newElements);
                if (newHashSet.Count == 0)
                    return EmptySet<T>.Instance;

                if (newHashSet.Count == this.includedElements.Count)
                    return this;

                return new InclusiveSet<T>(newHashSet);
            }
            else if (other is ExclusiveSet<T> otherExclusive)
            {
                var newElements = otherExclusive.excludedElements
                    .Except(this.includedElements);

                HashSet<T> newHashSet = new(newElements);
                if (newHashSet.Count == 0)
                    return CompleteSet<T>.Instance;

                if (newHashSet.Count == otherExclusive.excludedElements.Count)
                    return otherExclusive;

                return new ExclusiveSet<T>(newHashSet);
            }

            return base.Union(other);
        }

        public override ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to intersect well-known sets.
            if (this.TryWellKnownIntersect(other, out var result))
                return result;


            var newElements = this.includedElements
                .Where(element => other.Contains(element));

            HashSet<T> newHashSet = new(newElements);
            if (newHashSet.Count == 0)
                return EmptySet<T>.Instance;

            if (newHashSet.Count == this.includedElements.Count)
                return this;

            return new InclusiveSet<T>(newHashSet);
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to intersect well-known sets.
            if (this.TryWellKnownSubtract(other, out var result))
                return result;


            var newElements = this.includedElements
                .Where(element => !other.Contains(element));

            HashSet<T> newHashSet = new(newElements);
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
