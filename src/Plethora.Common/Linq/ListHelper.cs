using System;
using System.Collections.Generic;
using Plethora.Collections;

namespace Plethora.Linq
{
    public static class ListHelper
    {
        #region BinarySearch

        public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value)
        {
            return BinarySearch(source, valueSelector, value, 0, source.Count, Comparer<TValue>.Default);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value, IComparer<TValue> comparer)
        {
            return BinarySearch(source, valueSelector, value, 0, source.Count, comparer);
        }

        public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value, int index, int count, IComparer<TValue> comparer)
        {
            int minIndex = index;
            int maxIndex = (index + count) - 1;
            while(minIndex <= maxIndex)
            {
                int midIndex = minIndex + ((maxIndex - minIndex) >> 1);
                TSource midElement = source[midIndex];
                TValue midValue = valueSelector(midElement);

                int result = comparer.Compare(midValue, value);
                if (result == 0)
                {
                    return midIndex;
                }
                else if (result < 0)
                {
                    minIndex = midIndex + 1;
                }
                else
                {
                    maxIndex = midIndex - 1;
                }
            }
            return ~minIndex;
        }
        #endregion

        #region SubList

        public static IEnumerable<TSource> SubList<TSource>(this IList<TSource> source, int index, int count)
        {
            return new ListIndexItterator<TSource>(source, index, count);
        }
        #endregion
    }
}
