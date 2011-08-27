using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Linq
{
    public static class List
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

        private class SubListEnumerator<T> : IEnumerator<T>
        {
            #region Fields

            private readonly IList<T> source;
            private readonly int index;
            private readonly int count;

            private int currentIndex;
            private T current;
            #endregion

            #region Constructors

            public SubListEnumerator(IList<T> source, int index, int count)
            {
                //Validation
                if (source == null)
                    throw new ArgumentNullException("source");

                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", index,
                                                          ResourceProvider.ArgMustBeGreaterThanEqualToZero("index"));

                if (count < 0)
                    throw new ArgumentOutOfRangeException("count", count,
                                                          ResourceProvider.ArgMustBeGreaterThanEqualToZero("count"));

                if (index + count > source.Count)
                    throw new ArgumentException(ResourceProvider.ArgInvaliadOffsetLength("index", "count"));

                this.source = source;
                this.count = count;
                this.index = index;
                Reset();
            }
            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                this.Reset();
            }
            #endregion

            #region Implementation of IEnumerator

            public bool MoveNext()
            {
                int newIndex;
                if (currentIndex == -1)
                    newIndex = index;
                else
                    newIndex = currentIndex + 1;

                if (newIndex >= index + count)
                {
                    current = default(T);
                    return false;
                }

                currentIndex = newIndex;
                current = source[currentIndex];
                return true;
            }

            public void Reset()
            {
                this.currentIndex = -1;
                this.current = default(T);
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            #endregion

            #region Implementation of IEnumerator<T>

            public T Current
            {
                get { return this.current; }
            }

            #endregion
        }

        public static IEnumerable<TSource> SubList<TSource>(this IList<TSource> source, int index, int count)
        {
            return new SubListEnumerator<TSource>(source, index, count).AsEnumerable();
        }
        #endregion
    }
}
