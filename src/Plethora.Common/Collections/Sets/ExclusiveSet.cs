using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public sealed class ExclusiveSet<T> : BaseSetImpl<T>, ISet<T>
    {
        #region Fields

        private readonly HashSet<T> excludedElements;

        #endregion

        #region Constructors

        public ExclusiveSet(params T[] excludedElements)
            : this((IEnumerable<T>)excludedElements)
        {
        }

        public ExclusiveSet(IEnumerable<T> excludedElements)
        {
            //Validation
            if (excludedElements == null)
                throw new ArgumentNullException("excludedElements");


            this.excludedElements = new HashSet<T>(excludedElements);
        }

        #endregion

        #region Implentation Of ISet<T>

        public override bool Contains(T element)
        {
            return !this.excludedElements.Contains(element);
        }
        
        #endregion
        
        #region Overrides of BaseSetImpl<T>

        public override ISet<T> Union(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            var newElements = this.excludedElements
                .Where(element => !other.Contains(element));

            return new ExclusiveSet<T>(newElements);
        }

        public override ISet<T> Intersect(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            //Short-cut method to union two inclusive sets.
            // There is no advantage to be gain in setting IsNativeIntersect to true, as
            // this method is only useful if both objects are ExclusiveSets. If this
            // is the case, nothing is to be gained by making the call commutative.
            // An infinite loop can result from calling base.Intersect if IsNativeUnion
            // is set.
            var otherExclusive = other as ExclusiveSet<T>;
            if (otherExclusive != null)
            {
                var newElements = this.excludedElements
                    .Concat(otherExclusive.excludedElements);

                return new ExclusiveSet<T>(newElements);
            }

            return base.Intersect(other);
        }

        protected override bool IsNativeUnion
        {
            get { return true; }
        }

        #endregion

    }
}
