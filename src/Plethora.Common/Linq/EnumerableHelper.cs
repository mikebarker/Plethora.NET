using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Linq
{
    public static partial class EnumerableHelper
    {
        #region CacheResult

        /// <summary>
        /// Provides an instance of <see cref="IEnumerable{T}"/> which will execute
        /// the enumeration only on demand, and only once.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   The reference to <paramref name="source"/> is release once the results
        ///   have been fully cached.
        ///  </para>
        ///  <para>
        ///   Provides an optimised way to concretise the result of a LINQ based query.
        ///   If one concretises using ".ToList()" or ".ToArray()" the entire enumeration
        ///   will execute once, however if part of the enumeration is not required this
        ///   waste execution and memory resource.
        ///   If one chooses not to concretise a LINQ query the entire enumeration may be
        ///   executed multiple times, wasting execution resource.
        ///  </para>
        /// </remarks>
        public static IEnumerable<T> CacheResult<T>(this IEnumerable<T> source)
        {
            return new CacheEnumerable<T>(source);
        }
        #endregion

        #region IsCountXxx

        /// <summary>
        /// Provides a method to test the number of elements in an <see cref="IEnumerable{T}"/>
        /// with early exit semantics.
        /// </summary>
        private static bool IsCount<T>(this IEnumerable<T> source, int count, Func<int, int, bool> comparison)
        {
            //Validity
            ArgumentNullException.ThrowIfNull(source);

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count,
                    ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(count)));

            ArgumentNullException.ThrowIfNull(comparison);


            //short-cut IReadOnlyCollection
            if (source is IReadOnlyCollection<T> readOnlyCollection)
                return comparison(readOnlyCollection.Count, count);

            //short-cut ICollection
            if (source is ICollection<T> collection)
                return comparison(collection.Count, count);

            int num = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (++num > count)
                        return comparison(num, count);
                }
            }
            return comparison(num, count);
        }

        /// <summary>
        /// Provides a method to test the number of elements in an <see cref="IEnumerable{T}"/>
        /// with early exit semantics.
        /// </summary>
        /// <remarks>
        /// This allows a more performant method of testing the size of a possibly large 
        /// enumeration. Instead of
        /// <c>if (enumerable.Count() == 10) { }</c>
        /// use:
        /// <c>if (enumerable.IsCount(10)) { }</c>.
        /// The latter example will only enumerate 'enumerable' up to the eleventh element
        /// before returning. For a large enumerable this is more performant. 
        /// </remarks>
        public static bool IsCount<T>(this IEnumerable<T> source, int count)
        {
            return IsCount(source, count, (n, c) => n == c);
        }

        /// <see cref="IsCount{T}(IEnumerable{T}, int)"/>
        public static bool IsCountLessThan<T>(this IEnumerable<T> source, int count)
        {
            return IsCount(source, count, (n, c) => n < c);
        }

        /// <see cref="IsCount{T}(IEnumerable{T}, int)"/>
        public static bool IsCountLessThanOrEqualTo<T>(this IEnumerable<T> source, int count)
        {
            return IsCount(source, count, (n, c) => n <= c);
        }

        /// <see cref="IsCount{T}(IEnumerable{T}, int)"/>
        public static bool IsCountGreaterThan<T>(this IEnumerable<T> source, int count)
        {
            return IsCount(source, count, (n, c) => n > c);
        }

        /// <see cref="IsCount{T}(IEnumerable{T}, int)"/>
        public static bool IsCountGreaterThanOrEqualTo<T>(this IEnumerable<T> source, int count)
        {
            return IsCount(source, count, (n, c) => n >= c);
        }
        #endregion

        #region ForEach

        /// <summary>
        /// Performs the <paramref name="action"/> on each element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);


            foreach (T t in source)
            {
                action(t);
            }
        }

        /// <summary>
        /// Performs the <paramref name="action"/> on each element in an <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(action);


            int num = 0;
            foreach (T t in source)
            {
                action(t, num++);
            }
        }
        #endregion

        #region Flattern

        /// <summary>
        /// Provides access to each member in a tree of elements, as a single enumerable.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements.</typeparam>
        /// <param name="source">The enumerable source of elements.</param>
        /// <param name="childSelector">The function to access child elements from a parent element.</param>
        /// <returns>
        /// A single enumeration of tree elements.
        /// </returns>
        /// <remarks>
        /// Elements are returned in 'pre-order' traversal style
        /// (ie. parents are returned before their children).
        /// </remarks>
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> childSelector)
        {
            return source.Concat(source.SelectMany(item => (item is null)
                ? Enumerable.Empty<TSource>()
                : childSelector(item).Flatten(childSelector)));
        }

        #endregion

        #region Singularity

        /// <summary>
        /// Returns an <see cref="IEnumerable{TResult}"/> containing a single element.
        /// </summary>
        /// <typeparam name="TResult">The type of the element to be contained.</typeparam>
        /// <param name="element">The element to be contained in the <see cref="IEnumerable{TResult}"/>.</param>
        /// <returns>
        /// An <see cref="IEnumerable{TResult}"/> containing a single element.
        /// </returns>
        public static IEnumerable<TResult> Singularity<TResult>(this TResult element)
        {
            return Enumerable.Repeat(element, 1);
        }
        #endregion

        #region ToCollectionIfRequired<T>

        /// <summary>
        /// Converts the source <see cref="IEnumerable{T}"/> to a <see cref="IReadOnlyCollection{T}"/> if required,
        /// otherwise returns the source directly.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to be converted.</param>
        /// <returns>
        /// The <paramref name="source"/> if it is already an <see cref="IReadOnlyCollection{T}"/>, else
        /// an <see cref="IReadOnlyCollection{T}"/> containing the elements from <paramref name="source"/>.
        /// </returns>
        public static IReadOnlyCollection<T> ToReadOnlyCollectionIfRequired<T>(this IEnumerable<T> source)
        {
            if (source is IReadOnlyCollection<T> sourceCollection)
                return sourceCollection;

            return source.ToList();
        }

        #endregion

        #region ToCollectionIfRequired<T>

        /// <summary>
        /// Converts the source <see cref="IEnumerable{T}"/> to a <see cref="ICollection{T}"/> if required,
        /// otherwise returns the source directly.
        /// </summary>
        /// <typeparam name="T">The type of the collection.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to be converted.</param>
        /// <returns>
        /// The <paramref name="source"/> if it is already an <see cref="ICollection{T}"/>, else
        /// an <see cref="ICollection{T}"/> containing the elements from <paramref name="source"/>.
        /// </returns>
        public static ICollection<T> ToCollectionIfRequired<T>(this IEnumerable<T> source)
        {
            if (source is ICollection<T> sourceCollection)
                return sourceCollection;

            return source.ToList();
        }

        #endregion
        
        #region ToListIfRequired<T>

        /// <summary>
        /// Converts the source <see cref="IEnumerable{T}"/> to a <see cref="IList{T}"/> if required,
        /// otherwise returns the source directly.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to be converted.</param>
        /// <returns>
        /// The <paramref name="source"/> if it is already an <see cref="IList{T}"/>, else
        /// an <see cref="IList{T}"/> containing the elements from <paramref name="source"/>.
        /// </returns>
        public static IList<T> ToListIfRequired<T>(this IEnumerable<T> source)
        {
            if (source is IList<T> sourceList)
                return sourceList;

            return source.ToList();
        }

        #endregion

        #region AsEnumerable

        /// <summary>
        /// Returns a wrapper, presenting an <see cref="IEnumerator{T}"/> as a <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The data type of the elements.</typeparam>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> presenting the <see cref="IEnumerator{T}"/>.
        /// </returns>
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> enumerator)
        {
            return new EnumeratorWrapper<T>(enumerator);
        }
        #endregion
    }
}
