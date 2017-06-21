using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.fqi
{
    public static class EnumerableExtensions
    {
        private static readonly IEnumerable<string> emptyIndices = new string[0];

        #region Public Methods

        public static IIndexedEnumerable<T> AsIndexedEnumerable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is IIndexedEnumerable<T>)
                return (IIndexedEnumerable<T>)enumerable;

            return new IndexedEnumerableWrapper<T>(enumerable);
        }
        #endregion

        private class IndexedEnumerableWrapper<T> : IIndexedEnumerable<T>
        {
            #region Fields

            private readonly IEnumerable<T> enumerable;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="IndexedEnumerableWrapper{T}"/> class.
            /// </summary>
            public IndexedEnumerableWrapper(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
            }
            #endregion

            #region Implementation of IEnumerable

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                return this.enumerable.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion

            #region Implementation of IIndexedEnumerable<T>

            /// <summary>
            /// Gets the list of indexed members, in order by which elements are sorted.
            /// </summary>
            public IEnumerable<string> IndexedMembers
            {
                get { return EnumerableExtensions.emptyIndices; }
            }

            /// <summary>
            /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
            /// by a minimum and maximum range.
            /// </summary>
            public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr)
            {
                return this.AsEnumerable().Where(t => expr.Execute(t)).AsIndexedEnumerable();
            }

            /// <summary>
            /// Get an enumerator filtered by the specified predicate.
            /// </summary>
            /// <param name="predicate">The filtering predicate to be applied.</param>
            /// <param name="ranges">The keys' ranges to restrict by.</param>
            /// <returns>
            /// The required enumerator.
            /// </returns>
            /// <remarks>
            ///  <para>
            ///   The <paramref name="ranges"/> pertains to the members returned
            ///   by <see cref="IIndexedEnumerable{T}.IndexedMembers"/>.
            ///  </para>
            ///  <para>
            ///   The parameter <paramref name="ranges"/> is a helper array which identifies
            ///   the minimum and maximum values of the available keys.
            ///  </para>
            /// </remarks>
            public IEnumerator<T> GetIndexedEnumerator(Func<T, bool> predicate, IDictionary<string, ILateRange> ranges)
            {
                return this.AsEnumerable().Where(predicate).GetEnumerator();
            }

            /// <summary>
            /// Gets a flag indicating whether the <see cref="IIndexedEnumerable{T}"/>
            /// will support the usage of out-of-order indexing.
            /// </summary>
            public bool SupportsOutOfOrderIndexing
            {
                get { return false; }
            }
            #endregion
        }
    }
}
