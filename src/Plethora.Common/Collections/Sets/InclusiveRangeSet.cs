using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections.Sets
{
    public class RangeInclusiveSet<T> : BaseSetImpl<T>, ISetCoreMultiSubtract<T>
    {
        private readonly Range<T> range;

        public RangeInclusiveSet(T min, T max)
            : this(new Range<T>(min, max, Comparer<T>.Default))
        {
        }

        public RangeInclusiveSet(Range<T> range)
        {
            this.range = range;
        }

        public Range<T> Range
        {
            get { return this.range; }
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


        IReadOnlyCollection<ISetCore> ISetCoreMultiSubtract.Subtract(ISetCore other)
        {
            //Validation
            if (!(other is ISetCore<T> otherT))
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(other), typeof(ISetCore<T>)), nameof(other));

            var results = SubtractInternal(otherT, true);
            return results;
        }

        IReadOnlyCollection<ISetCore<T>> ISetCoreMultiSubtract<T>.Subtract(ISetCore<T> other)
        {
            var results = SubtractInternal(other, true);
            return results;
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            var results = SubtractInternal(other, false);
            return results.First();
        }

        private IReadOnlyCollection<ISetCore<T>> SubtractInternal(ISetCore<T> other, bool allowMultipleResults)
        {
            if (other is RangeInclusiveSet<T> otherRangeSet)
            {
                var remainders = RangeHelper.Subtract(this.range, otherRangeSet.range);
                if (remainders.Count == 0)
                {
                    return new[] { EmptySet<T>.Instance };
                }
                else if (remainders.Count == 1)
                {
                    Range<T> remainder = remainders.First();
                    if (this.range.Equals(remainder))
                    {
                        return new[] { this };
                    }

                    return new[] { new RangeInclusiveSet<T>(remainder) };
                }
                else
                {
                    if (allowMultipleResults)
                    {
                        var results = new List<RangeInclusiveSet<T>>();
                        foreach (var remainder in remainders)
                        {
                            results.Add(new RangeInclusiveSet<T>(remainder));
                        }
                        return results;
                    }
                }
            }

            var result = base.Subtract(other);
            return new[] { result };
        }
    }
}
