using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Plethora
{
    public readonly struct Range<T> : IEquatable<Range<T>>
        where T : notnull
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
            : this(min, true, max, true, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max, IComparer<T> comparer)
            : this(min, true, max, true, comparer)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, bool minInclusive, T max, bool maxInclusive)
            : this(min, minInclusive, max, maxInclusive, Comparer<T>.Default)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, bool minInclusive, T max, bool maxInclusive, IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);

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
            get { return this.min; }
        }

        /// <summary>
        /// Gets a flag indicating whether the minimum value is included in the range.
        /// </summary>
        public bool MinInclusive
        {
            get { return this.minInclusive; }
        }

        /// <summary>
        /// Gets the maximum value represented by the <see cref="Range{T}"/>.
        /// </summary>
        public T Max
        {
            get { return this.max; }
        }

        /// <summary>
        /// Gets a flag indicating whether the maximum value is included in the range.
        /// </summary>
        public bool MaxInclusive
        {
            get { return this.maxInclusive; }
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> used to compare elements within the range.
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return this.comparer; }
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

            result = this.comparer.Compare(this.min, item);
            if (result > 0) // min > item
                return false;
            else if ((result == 0) && (!this.minInclusive))
                return false;


            result = this.comparer.Compare(item, this.max);
            if (result > 0) // item > max
                return false;
            else if ((result == 0) && (!this.maxInclusive))
                return false;

            return true;
        }

        #endregion

        #region Implementation of IEquatable<Range<T>>

        public bool Equals(Range<T> other)
        {
            return
                this.minInclusive == other.minInclusive &&
                this.maxInclusive == other.maxInclusive &&
                EqualityComparer<T>.Default.Equals(this.min, other.min) &&
                EqualityComparer<T>.Default.Equals(this.max, other.max);
        }

        #endregion

        #region Overrides of Object
        
        public override bool Equals(object? obj)
        {
            if (obj is not Range<T>)
                return false;

            return this.Equals((Range<T>)obj);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(this.min, this.max, this.minInclusive, this.maxInclusive);
        }

        #endregion
    }

    public static class RangeHelper
    {
        public static IReadOnlyCollection<Range<T>> Subtract<T>(this Range<T> rangeA, Range<T> rangeB)
            where T : notnull
        {
            List<Range<T>> list = new(2);

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
                Range<T> remainder = new(rangeA.Min, rangeA.MinInclusive, rangeB.Min, !rangeB.MinInclusive, comparer);
                list.Add(remainder);
            }
            else if ((minCompare == 0) && (rangeA.MinInclusive) && (!rangeB.MinInclusive))
            {
                Range<T> remainder = new(rangeA.Min, true, rangeA.Min, true, comparer);
                list.Add(remainder);
            }

            // Account for rangeB.Max to rangeA.Max
            int maxCompare = comparer.Compare(rangeA.Max, rangeB.Max);
            if (maxCompare > 0)  // rangeA.Max > rangeB.Max
            {
                Range<T> remainder = new(rangeB.Max, !rangeB.MaxInclusive, rangeA.Max, rangeA.MaxInclusive, comparer);
                list.Add(remainder);
            }
            else if ((maxCompare == 0) && (rangeA.MaxInclusive) && (!rangeB.MaxInclusive))
            {
                Range<T> remainder = new(rangeA.Max, true, rangeA.Max, true, comparer);
                list.Add(remainder);
            }

            return list;
        }
    }
}
