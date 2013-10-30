namespace Plethora.Collections.Sets
{
    public interface ISetCore<T>
    {
        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is represented; else false.</returns>
        bool Contains(T element);

        /// <summary>
        /// 
        /// </summary>
        bool? IsEmpty { get; }

        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        ISetCore<T> Union(ISetCore<T> other);

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        ISetCore<T> Intersect(ISetCore<T> other);

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        ISetCore<T> Subtract(ISetCore<T> other);

        /// <summary>
        /// Returns the inverse set of this set.
        /// </summary>
        ISetCore<T> Inverse();
    }
}
