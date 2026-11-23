using System;
using System.Collections.Generic;
using System.Linq;
using Plethora.Collections;

namespace Plethora.Linq;

public static class ListHelper
{
    #region BinarySearch

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element
    /// and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the default comparer.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    public static int BinarySearch<TSource>(this IList<TSource> source, TSource value)
    {
        return BinarySearch(source, x => x, value);
    }

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element using
    /// the specified comparer and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the comparer.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    public static int BinarySearch<TSource>(this IList<TSource> source, TSource value, IComparer<TSource> comparer)
    {
        return BinarySearch(source, x => x, value, comparer);
    }

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element using
    /// the specified comparer and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the comparer.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <param name="index">The zero-based starting index of the range to search.</param>
    /// <param name="count">The length of the range to search.</param>
    /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The range specified by <paramref name="index"/> and <paramref name="count"/> extends beyond the end of <paramref name="source"/>.</exception>
    public static int BinarySearch<TSource>(this IList<TSource> source, TSource value, int index, int count, IComparer<TSource> comparer)
    {
        return BinarySearch(source, x => x, value, index, count, comparer);
    }

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element
    /// and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <typeparam name="TValue">The type of the values selected from the source elements.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the default comparer.</param>
    /// <param name="valueSelector">A function that selects the value to compare from each element.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value)
    {
        return BinarySearch(source, valueSelector, value, 0, source.Count, Comparer<TValue>.Default);
    }

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element using
    /// the specified comparer and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <typeparam name="TValue">The type of the values selected from the source elements.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the comparer.</param>
    /// <param name="valueSelector">A function that selects the value to compare from each element.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value, IComparer<TValue> comparer)
    {
        return BinarySearch(source, valueSelector, value, 0, source.Count, comparer);
    }

    /// <summary>
    /// Searches a range of elements in the sorted <see cref="IList{T}"/> for an element using
    /// the specified comparer and returns the zero-based index of the element.
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the source list.</typeparam>
    /// <typeparam name="TValue">The type of the values selected from the source elements.</typeparam>
    /// <param name="source">The list to search. The specified range must be sorted according to the comparer.</param>
    /// <param name="valueSelector">A function that selects the value to compare from each element.</param>
    /// <param name="value">The object to locate. The value can be null for reference types.</param>
    /// <param name="index">The zero-based starting index of the range to search.</param>
    /// <param name="count">The length of the range to search.</param>
    /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing elements.</param>
    /// <returns>
    /// The zero-based index of item in the sorted <see cref="IList{T}"/>, if item is found;
    /// otherwise, a negative number that is the bitwise complement of the index of the next
    /// element that is larger than item or, if there is no larger element, the bitwise complement
    /// of <see cref="IList{T}.Count"/>.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> or <paramref name="count"/> is less than 0.</exception>
    /// <exception cref="ArgumentException">The range specified by <paramref name="index"/> and <paramref name="count"/> extends beyond the end of <paramref name="source"/>.</exception>
    public static int BinarySearch<TSource, TValue>(this IList<TSource> source, Func<TSource, TValue> valueSelector, TValue value, int index, int count, IComparer<TValue> comparer)
    {
        //Validation
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0);

        if ((source.Count - index) < count)
            throw new ArgumentException(ResourceProvider.ArgInvalidOffsetLength(nameof(index), nameof(count)));


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

    public static SubList<TSource> SubList<TSource>(this IList<TSource> source, int index)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(source);


        var count = source.Count - index;
        return SubList(source, index, count);
    }

    public static SubList<TSource> SubList<TSource>(this IList<TSource> source, int index, int count)
    {
        return new SubList<TSource>(source, index, count);
    }

    #endregion

    #region IndexOfType

    public static int IndexOfType<TSource, TSearchType>(this IList<TSource> source)
    {
        Type searchType = typeof (TSearchType);
        return IndexOfType(source, searchType);
    }

    public static int IndexOfType<TSource>(this IList<TSource> source, Type searchType)
    {
        for (int i = 0; i < source.Count; i++)
        {
            if (searchType.IsInstanceOfType(source[i]))
                return i;
        }

        return -1;
    }

    #endregion
}
