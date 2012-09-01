namespace Plethora
{
    public struct Range<T>
    {
        #region Fields

        private readonly T min;
        private readonly bool minInclusive;
        private readonly T max;
        private readonly bool maxInclusive;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max)
            : this(min, max, true, true)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Range{T}"/> struct.
        /// </summary>
        public Range(T min, T max, bool minInclusive, bool maxInclusive)
        {
            //Can't validate min <= max, because the comparer to be used is unknown.


            this.min = min;
            this.minInclusive = minInclusive;
            this.max = max;
            this.maxInclusive = maxInclusive;
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
        #endregion
    }
}
