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
        ///   executed mutiple times, wasting execution resource.
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
            if (source == null)
                throw new ArgumentNullException("source");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count,
                    ResourceProvider.ArgMustBeGreaterThanEqualToZero("count"));

            if (comparison == null)
                throw new ArgumentNullException("comparison");


            //short-cut ICollection
            if (source is ICollection<T>)
                return comparison(((ICollection<T>)source).Count, count);

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
        /// The latter example will only enumerate 'enumerable' upto the eleventh element
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
            if (source == null)
                throw new ArgumentNullException("source");

            if (action == null)
                throw new ArgumentNullException("action");

            
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
            if (source == null)
                throw new ArgumentNullException("source");

            if (action == null)
                throw new ArgumentNullException("action");


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
            return source.Concat(source.SelectMany(item => (item == null)
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
    }
}
