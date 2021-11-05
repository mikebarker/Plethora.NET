using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IEnumerable<TSource> SubList<TSource>(this IList<TSource> source, int index)
        {
            //Validation
            if (source == null)
                throw new ArgumentNullException(nameof(source));


            var count = source.Count - index;
            return SubList(source, index, count);
        }

        public static IEnumerable<TSource> SubList<TSource>(this IList<TSource> source, int index, int count)
        {
            return new ListIndexIterator<TSource>(source, index, count);
        }


        public static IEnumerable<TSource> SubListOrEmpty<TSource>(this IList<TSource> source, int index)
        {
            //Validation
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            
            var count = source.Count - index;
            return SubListOrEmpty(source, index, count);
        }

        public static IEnumerable<TSource> SubListOrEmpty<TSource>(this IList<TSource> source, int index, int count)
        {
            //Validation
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(index)));


            if (index > source.Count - 1)
                return Enumerable.Empty<TSource>();

            var maxCount = source.Count - index;
            count = Math.Min(count, maxCount);

            return new ListIndexIterator<TSource>(source, index, count);
        }
        #endregion

        public static int IndexOfType<TSource>(this IList<TSource> source, Type type)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (type.IsInstanceOfType(source[i]))
                    return i;
            }

            return -1;
        }
    }
}
