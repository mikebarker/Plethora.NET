using System.Diagnostics.Contracts;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// An abstract set representation.
    /// </summary>
    /// <remarks>
    /// Where <see cref="System.Collections.Generic.ISet{T}"/> relies on a collection of discrete elements,
    /// implementations of this interface may be abstract.
    /// </remarks>
    public interface ISetCore
    {
        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is included; else false.</returns>
        [Pure]
        bool Contains(object element);

        /// <summary>
        /// Gets a flag indicating whether this instance represents an empty set.
        /// </summary>
        /// <returns>
        /// True if the set is definitely empty; false if the set is not definitely empty; otherwise null
        /// if it can not be determined whether the set is empty.
        /// </returns>
        /// <remarks>
        /// This property is utilised to perform optimisation where possible. Utilisations
        /// should not rely on this property returning a non-null value.
        /// </remarks>
        bool? IsEmpty { get; }

        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        [Pure]
        ISetCore Union(ISetCore other);

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        [Pure]
        ISetCore Intersect(ISetCore other);

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        [Pure]
        ISetCore Subtract(ISetCore other);

        /// <summary>
        /// Returns the inverse set of this set.
        /// </summary>
        [Pure]
        ISetCore Inverse();
    }

    /// <summary>
    /// An abstract set representation.
    /// </summary>
    /// <remarks>
    /// Where <see cref="System.Collections.Generic.ISet{T}"/> relies on a collection of discrete elements,
    /// implementations of this interface may be abstract.
    /// </remarks>
    /// <typeparam name="T">The data types of the elements in the set.</typeparam>
    public interface ISetCore<T> : ISetCore
    {
        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is included; else false.</returns>
        [Pure]
        bool Contains(T element);

        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        [Pure]
        ISetCore<T> Union(ISetCore<T> other);

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        [Pure]
        ISetCore<T> Intersect(ISetCore<T> other);

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        [Pure]
        ISetCore<T> Subtract(ISetCore<T> other);

        /// <summary>
        /// Returns the inverse set of this set.
        /// </summary>
        [Pure]
        new ISetCore<T> Inverse();
    }
}
