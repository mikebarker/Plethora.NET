using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Plethora
{
    public struct Range<T> : IEquatable<Range<T>>
    {
        #region Fields

        private readonly T min;
        private readonly bool minInclusive;
        private readonly T max;
        private readonly bool maxInclusive;
        private readonly IComparer<T> comparer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max)
            : this(min, max, true, true, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max, IComparer<T> comparer)
            : this(min, max, true, true, comparer)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max, bool minInclusive, bool maxInclusive)
            : this(min, max, minInclusive,maxInclusive, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max, bool minInclusive, bool maxInclusive, IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (comparer.Compare(min, max) > 0) // min > max
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThanEqualTo("range.Min", "range.Max"));


            this.min = min;
            this.minInclusive = minInclusive;
            this.max = max;
            this.maxInclusive = maxInclusive;
            this.comparer = comparer;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the minimum value represented by the <see cref="Range{T}"/>.
        /// </summary>
        public T Min
        {
            get { return min; }
        }

        /// <summary>
        /// Gets a flag indicating whether the minimum value is included in the range.
        /// </summary>
        public bool MinInclusive
        {
            get { return minInclusive; }
        }

        /// <summary>
        /// Gets the maximum value represented by the <see cref="Range{T}"/>.
        /// </summary>
        public T Max
        {
            get { return max; }
        }

        /// <summary>
        /// Gets a flag indicating whether the maximum value is included in the range.
        /// </summary>
        public bool MaxInclusive
        {
            get { return maxInclusive; }
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> used to compare elements within the range.
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return comparer; }
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Tests if an item falls within the range denoted by this <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="item">The element to be tested.</param>
        /// <returns>
        /// 'true' if the <paramref name="item"/> false within the range; else 'false'.
        /// </returns>
        [Pure]
        public bool IsInRange(T item)
        {
            int result;

            result = this.comparer.Compare(min, item);
            if (result > 0) // min > item
                return false;
            else if ((result == 0) && (!minInclusive))
                return false;


            result = this.comparer.Compare(item, max);
            if (result > 0) // item > max
                return false;
            else if ((result == 0) && (!maxInclusive))
                return false;

            return true;
        }

        #endregion

        #region Implementation of IEquatable<Range<T>>

        public bool Equals(Range<T> other)
        {
            return
                EqualityComparer<T>.Default.Equals(min, other.min) &&
                EqualityComparer<T>.Default.Equals(max, other.max) &&
                minInclusive == other.minInclusive &&
                maxInclusive == other.maxInclusive;
        }

        #endregion

        #region Overrides of Object
        
        public override bool Equals(object obj)
        {
            if (!(obj is Range<T>))
                return false;

            return Equals((Range<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.min, this.max, this.minInclusive, this.maxInclusive);
        }

        #endregion
    }

    public static class RangeHelper
    {
        public static IList<Range<T>> Subtract<T>(this Range<T> rangeA, Range<T> rangeB)
        {
            List<Range<T>> list = new List<Range<T>>(2);

            var comparer = rangeA.Comparer;

            // rangeB falls entirely before rangeA, nothing to subtract
            if (comparer.Compare(rangeA.Min, rangeB.Max) > 0)
            {
                list.Add(rangeA);
                return list;
            }
            if ((comparer.Compare(rangeA.Min, rangeB.Max) == 0) &&
                (!rangeA.MinInclusive || !rangeB.MaxInclusive))
            {
                list.Add(rangeA);
                return list;
            }


            // rangeB falls entirely after rangeA, nothing to subtract
            if (comparer.Compare(rangeA.Max, rangeB.Min) < 0)
            {
                list.Add(rangeA);
                return list;
            }
            if ((comparer.Compare(rangeA.Max, rangeB.Min) == 0) &&
                (!rangeA.MaxInclusive || !rangeB.MinInclusive))
            {
                list.Add(rangeA);
                return list;
            }


            // Account for rangeA.Min to rangeB.Min
            int minCompare = comparer.Compare(rangeA.Min, rangeB.Min);
            if (minCompare < 0)  // rangeA.Min < rangeB.Min
            {
                var remainder = new Range<T>(rangeA.Min, rangeA.Min, rangeA.MinInclusive, !rangeB.MinInclusive, comparer);
                list.Add(remainder);
            }
            else if ((minCompare == 0) && (rangeA.MinInclusive) && (!rangeB.MinInclusive))
            {
                var remainder = new Range<T>(rangeA.Min, rangeA.Min, true, true, comparer);
                list.Add(remainder);
            }

            // Account for rangeB.Max to rangeA.Max
            int maxCompare = comparer.Compare(rangeA.Max, rangeB.Max);
            if (maxCompare > 0)  // rangeA.Max > rangeB.Max
            {
                var remainder = new Range<T>(rangeB.Max, rangeA.Max, !rangeB.MaxInclusive, rangeA.MaxInclusive, comparer);
                list.Add(remainder);
            }
            else if ((maxCompare == 0) && (rangeA.MaxInclusive) && (!rangeB.MaxInclusive))
            {
                var remainder = new Range<T>(rangeA.Max, rangeA.Max, true, true, comparer);
                list.Add(remainder);
            }

            return list;
        }
    }
}
