using System.Collections.Generic;

namespace Plethora.Collections
{
    /// <summary>
    /// A collection which is accessible by a unique key per item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keyed element of the collection</typeparam>
    /// <typeparam name="T">The type of the collection.</typeparam>
    public interface IKeyedCollection<TKey, T> : ICollection<T>
    {
        /// <summary>
        /// Performs an insert if the item does not exist (by key) in the collection;
        /// else replaces the item in the list.
        /// </summary>
        /// <param name="item">The element to be inserted or updated.</param>
        /// <returns>
        /// true if <paramref name="item"/> was added; false if it was updated.
        /// </returns>
        bool Upsert(T item);

        bool ContainsKey(TKey key);
        bool RemoveKey(TKey key);
        bool TryGetValue(TKey key, out T item);
        T this[TKey key] { get; }

        IEnumerable<TKey> Keys { get; }

        IDictionary<TKey, T> AsReadOnlyDictionary();
    }
}