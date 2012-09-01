using System.Collections.Generic;

namespace Plethora.Collections
{
    public static class SortedByKeyListHelper
    {
        public static IEnumerable<T> GetByRange<TKey, T>(this SortedByKeyList<TKey, T> list, Range<TKey> range)
        {
            int startIndex = list.BinarySearch(range.Min);
            if (startIndex < 0)
            {
                startIndex = ~startIndex;
            }
            else if (range.MinInclusive)
            {
                //Find the first index which is equal to the min key
                while (true)
                {
                    int nextIndex = startIndex - 1;
                    T item = list[nextIndex];
                    TKey key = list.GetKey(item);
                    if (list.Comparer.Compare(range.Min, key) == 0)
                        startIndex = nextIndex;
                    else
                        break;
                }
            }
            else
            {
                //Find the first index which is not equal to the min key
                while (true)
                {
                    startIndex++;
                    T item = list[startIndex];
                    TKey key = list.GetKey(item);
                    if (list.Comparer.Compare(range.Min, key) != 0)
                        break;
                }
            }

            int endIndex = list.BinarySearch(range.Max);
            if (endIndex < 0)
            {
                endIndex = (~endIndex) - 1;
            }
            else if (range.MaxInclusive)
            {
                //Find the last index which is equal to the max key
                while (true)
                {
                    int nextIndex = endIndex + 1;
                    T item = list[nextIndex];
                    TKey key = list.GetKey(item);
                    if (list.Comparer.Compare(key, range.Max) == 0)
                        endIndex = nextIndex;
                    else
                        break;
                }
            }
            else
            {
                //Find the last index which is not equal to the max key
                while (true)
                {
                    endIndex--;
                    T item = list[endIndex];
                    TKey key = list.GetKey(item);
                    if (list.Comparer.Compare(key, range.Max) != 0)
                        break;
                }
            }

            return new ListIndexItterator<T>(list, startIndex, (endIndex - startIndex + 1));
        }
    }
}
