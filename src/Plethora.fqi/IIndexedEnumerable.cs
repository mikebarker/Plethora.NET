using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Plethora.fqi
{
    /// <summary>
    /// A multi-indexed <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IMultiIndexedEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the list of <see cref="IIndexedEnumerable{T}"/>s.
        /// </summary>
        IEnumerable<IIndexedEnumerable<T>> IndexedEnumerables { get; }
    }

    /// <summary>
    /// A multi-indexed <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IMultiIndexedCollection<T> : IMultiIndexedEnumerable<T>, ICollection<T>
    {
        /// <summary>
        /// Gets the list of <see cref="IIndexedEnumerable{T}"/>s.
        /// </summary>
        IEnumerable<IIndexedCollection<T>> IndexedCollections { get; }
    }

    /// <summary>
    /// An indexed <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IIndexedEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// Gets the list of indexed members, in order by which elements are sorted.
        /// </summary>
        IEnumerable<string> IndexedMembers { get; }

        /// <summary>
        /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
        /// by a minimum and maximum range.
        /// </summary>
        /// <param name="expr">The expression of the filtering predicate to be applied.</param>
        IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr);


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
        IEnumerator<T> GetIndexedEnumerator(Func<T, bool> predicate,
                                            IDictionary<string, ILateRange> ranges);

        /// <summary>
        /// Gets a flag indicating whether the <see cref="IIndexedEnumerable{T}"/>
        /// will support the usage of out-of-order indexing.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   Out-of-order indexing is used when indexed members are accessed in a different
        ///   order from the definition of the index.
        ///  </para>
        ///  <para>
        ///   <example>
        ///     Consider an index defined as:
        ///     <list>
        ///       <item>Year</item>
        ///       <item>Month</item>
        ///       <item>Day</item>
        ///     </list>
        ///     If a request is made to limit against 'Month', out-of-order indexing must be used
        ///     (as no restriction for year is defined).
        ///   </example>
        ///  </para>
        ///  <para>
        ///   Out-of-order indexing also allows for further index restrictions after an inequality
        ///   operator to be supported.
        ///  </para>
        /// </remarks>
        bool SupportsOutOfOrderIndexing { get; }
    }

    /// <summary>
    /// An <see cref="ICollection{T}"/> with additional 'set' semantics.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface ISetCollection<T> : ICollection<T>
    {
        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="ISetCollection{T}" />,
        /// or updates the existing value if the key already exists.
        /// </summary>
        /// <param name="item">The object to add.</param>
        /// <returns>
        /// true if the item was added; else false.
        /// </returns>
        bool AddOrUpdate(T item);
    }

    /// <summary>
    /// An indexed <see cref="ICollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    public interface IIndexedCollection<T> : IIndexedEnumerable<T>, ISetCollection<T>
    {
    }
}
