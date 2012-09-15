using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Plethora.Cache.Spacial
{
    public class DimensionDefinition<T> : IComparer<T>
    {
        #region Fields

        private readonly IComparer<T> comparer;
        #endregion

        #region Constructors

        public DimensionDefinition(IComparer<T> comparer)
        {
            //Validation
            if (comparer == null)
                throw new ArgumentNullException("comparer");


            this.comparer = comparer;
        }

        #endregion

        #region Implementation of IComparer<in T>

        public int Compare(T x, T y)
        {
            return this.comparer.Compare(x, y);
        }

        #endregion

        #region Public Methods

        [Pure]
        public bool IsPointInDimension(T t, Range<T> range)
        {
            int result;
            result = this.comparer.Compare(range.Min, t);
            if ((result > 0) ||
                (result == 0) && (!range.MinInclusive))
            {
                return false;
            }

            result = this.comparer.Compare(range.Max, t);
            if ((result < 0) ||
                (result == 0) && (!range.MaxInclusive))
            {
                return false;
            }

            return true;
        }

        [Pure]
        public bool DoDimensionsOverlap(Range<T> rangeA, Range<T> rangeB)
        {
            int result;
            result = this.comparer.Compare(rangeA.Min, rangeB.Max);
            if ((result > 0) ||
                (result == 0) && (!rangeA.MinInclusive | !rangeB.MaxInclusive))
            {
                return false;
            }

            result = this.comparer.Compare(rangeA.Max, rangeB.Min);
            if ((result < 0) ||
                (result == 0) && (!rangeA.MaxInclusive | !rangeB.MinInclusive))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
