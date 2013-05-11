using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections
{
    public static class SortedByKeyListHelper
    {
        public static IEnumerable<T> GetByKey<TKey, T>(this SortedByKeyList<TKey, T> list, TKey key)
        {
            return GetByRange(list, new Range<TKey>(key, key, true, true));
        }

        public static IEnumerable<T> GetByRange<TKey, T>(this SortedByKeyList<TKey, T> list, Range<TKey> range)
        {
            if (list.Comparer.Compare(range.Min, range.Max) > 0) // min > max
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThan("range.Min", "range.Max"), "range");


            int startIndex;
            if (range.MinInclusive)
            {
                var firstIndex = list.IndexOf(range.Min);
                if (firstIndex < 0)
                    firstIndex = ~firstIndex;

                startIndex = firstIndex;
            }
            else
            {
                var lastIndex = list.LastIndexOf(range.Min);
                if (lastIndex < 0)
                    startIndex = ~lastIndex;
                else
                    startIndex = lastIndex + 1;


                if (startIndex >= list.Count)
                    return Enumerable.Empty<T>();
            }


            int endIndex;
            if (range.MaxInclusive)
            {
                var lastIndex = list.LastIndexOf(range.Max);
                if (lastIndex < 0)
                    lastIndex = ~lastIndex - 1;

                endIndex = lastIndex;
            }
            else
            {
                var firstIndex = list.IndexOf(range.Max);
                if (firstIndex < 0)
                    endIndex = ~firstIndex - 1;
                else
                    endIndex = firstIndex - 1;


                if (endIndex < 0)
                    return Enumerable.Empty<T>();
            }

            var count = endIndex - startIndex + 1;
            if (count == 0)
                return Enumerable.Empty<T>();

            return new ListIndexItterator<T>(list, startIndex, count);
        }
    }
}
