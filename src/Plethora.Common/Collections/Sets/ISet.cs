namespace Plethora.Collections.Sets
{
    public interface ISet<T>
    {
        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is represented; else false.</returns>
        bool Contains(T element);


        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        ISet<T> Union(ISet<T> other);

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        ISet<T> Intersect(ISet<T> other);

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        ISet<T> Subtract(ISet<T> other);
    }
}
