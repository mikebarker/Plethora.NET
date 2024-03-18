using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public sealed class ExclusiveSet<T> : BaseSetImpl<T>
    {
        #region Fields

        internal readonly HashSet<T> excludedElements;

        #endregion

        #region Constructors

        public ExclusiveSet(params T[] excludedElements)
            : this((IEnumerable<T>)excludedElements)
        {
        }

        public ExclusiveSet(IEnumerable<T> excludedElements)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(excludedElements);


            this.excludedElements = new HashSet<T>(excludedElements);
        }

        #endregion

        #region Implentation Of ISet<T>

        public override bool Contains(T element)
        {
            return !this.excludedElements.Contains(element);
        }

        public override bool? IsEmpty
        {
            get { return false; }
        }

        #endregion

        #region Overrides of BaseSetImpl<T>

        public override ISetCore<T> Union(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to union well-known sets.
            if (this.TryWellKnownUnion(other, out var result))
                return result;


            var newElements = this.excludedElements
                .Where(element => !other.Contains(element));

            return new ExclusiveSet<T>(newElements);
        }

        public override ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to intersect well-known sets.
            if (this.TryWellKnownIntersect(other, out var result))
                return result;


            //Short-cut method to intersect two exclusive sets.
            // There is no advantage to be gain in setting IsNativeIntersect to true, as
            // this method is only useful if both objects are ExclusiveSets. If this
            // is the case, nothing is to be gained by making the call commutative.
            // An infinite loop can result from calling base.Intersect if IsNativeUnion
            // is set.
            if (other is ExclusiveSet<T> otherExclusive)
            {
                var newElements = this.excludedElements
                    .Concat(otherExclusive.excludedElements);

                return new ExclusiveSet<T>(newElements);
            }

            return base.Intersect(other);
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            //Short-cut method to subtract well-known sets.
            if (this.TryWellKnownIntersect(other, out var result))
                return result;


            //Short-cut method to subtract an exclusive set.
            if (other is ExclusiveSet<T> otherExclusive)
            {
                var newElements = otherExclusive.excludedElements
                    .Except(this.excludedElements);

                return new InclusiveSet<T>(newElements);
            }

            //Short-cut method to subtract an inclusive set.
            if (other is InclusiveSet<T> otherInclusive)
            {
                var newElements = this.excludedElements
                    .Concat(otherInclusive.includedElements);

                return new ExclusiveSet<T>(newElements);
            }


            return base.Subtract(other);
        }

        public override ISetCore<T> Inverse()
        {
            return new InclusiveSet<T>(this.excludedElements);
        }

        protected override bool IsNativeUnion
        {
            get { return true; }
        }

        #endregion

        public IEnumerable<T> ExcludedElements
        {
            get { return this.excludedElements; }
        }
    }
}
