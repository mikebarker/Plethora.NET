using System;
using System.Collections.Generic;

namespace Plethora
{
    public struct Range<T>
    {
        #region Fields

        private readonly T min;
        private readonly T max;
        private readonly IComparer<T> comparer;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="min">The minimum value to be represented by the range.</param>
        /// <param name="max">The maximum value to be represented by the range.</param>
        public Range(T min, T max)
            : this(min, max, Comparer<T>.Default)
        {
            if (Comparer<T>.Default.Compare(min, max) == 0)
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThanEqualTo("min", "max"));

            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Initialises a new instance of <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="min">The minimum value to be represented by the range.</param>
        /// <param name="max">The maximum value to be represented by the range.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> to be used to compare items in the range.</param>
        public Range(T min, T max, IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (comparer.Compare(min, max) > 0)  // min > max
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThanEqualTo("min", "max"));

            this.min = min;
            this.max = max;
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
        /// Gets the maximum value represented by the <see cref="Range{T}"/>.
        /// </summary>
        public T Max
        {
            get { return max; }
        }

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> used to compare items over the range.
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return comparer; }
        }
        #endregion

        #region Public Methods

        public bool IsValueInRange(T value)
        {
            return
                (comparer.Compare(min, value) <= 0) &&
                (comparer.Compare(value, max) <= 0);
        }
        #endregion
    }
}
