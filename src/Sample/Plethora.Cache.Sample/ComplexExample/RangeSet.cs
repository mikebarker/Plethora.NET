using System.Collections.Generic;

using Plethora.Collections.Sets;

namespace Plethora.Cache.Sample.ComplexExample
{
    class RangeSet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        private readonly Range<T> range;

        public RangeSet(T min, T max)
            : this(new Range<T>(min, max, Comparer<T>.Default))
        {
        }

        public RangeSet(Range<T> range)
        {
            this.range = range;
        }

        public T Min
        {
            get { return this.range.Min; }
        }

        public T Max
        {
            get { return this.range.Max; }
        }

        public override bool Contains(T element)
        {
            return
                this.range.IsInRange(element);
        }

        public override bool? IsEmpty
        {
            get { return RangeIsEmpty(this.range); }
        }

        private static bool RangeIsEmpty(Range<T> range)
        {
            //A range is only ever empty when min equals max and neither are included.
            return
                (range.Comparer.Compare(range.Min, range.Max) == 0) &&
                (!range.MinInclusive) &&
                (!range.MaxInclusive);
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            RangeSet<T> otherRangeSet = other as RangeSet<T>;
            if (otherRangeSet != null)
            {
                var remainders = RangeHelper.Subtract(this.range, otherRangeSet.range);
                if (remainders.Count == 0)
                {
                    return EmptySet<T>.Instance;
                }
                else if (remainders.Count == 1)
                {
                    if (this.range.Equals(remainders[0]))
                    {
                        return this;
                    }

                    return new RangeSet<T>(remainders[0]);
                }
                else
                {
                    //TODO: multi range return
                }
            }

            return base.Subtract(other);
        }
    }
}
