using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Plethora.Collections
{
    /// <summary>
    /// A read-only wrapper for an instance of <see cref="IKeyedCollection{TKey, T}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keyed element of the collection</typeparam>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <seealso cref="KeyedCollection{TKey, T}"/>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class ReadOnlyKeyedCollection<TKey, T> : IKeyedCollection<TKey, T>
    {
        #region Fields

        private readonly IKeyedCollection<TKey, T> innerKeyedCollection;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="ReadOnlyKeyedCollection{TKey,T}"/> class.
        /// </summary>
        public ReadOnlyKeyedCollection(IKeyedCollection<TKey, T> keyedCollection)
        {
            //Validation
            if (keyedCollection == null)
                throw new ArgumentNullException("keyedCollection");


            this.innerKeyedCollection = keyedCollection;
        }
        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Implementation of IEnumerable<T>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.innerKeyedCollection.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadonly());
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        void ICollection<T>.Clear()
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadonly());
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="ICollection{T}"/>;
        /// otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}"/>.</param>
        public bool Contains(T item)
        {
            return this.innerKeyedCollection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied
        /// from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerKeyedCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection{T}"/>;
        /// otherwise, false. 
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadonly());
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get { return this.innerKeyedCollection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="ICollection{T}"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get { return true; }
        }
        #endregion

        #region Implementation of IKeyedCollection<TKey,T>

        bool IKeyedCollection<TKey, T>.Upsert(T item)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadonly());
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if an item with the key <paramref name="key"/> is found in the
        /// <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        public bool ContainsKey(TKey key)
        {
            return this.innerKeyedCollection.ContainsKey(key);
        }

        bool IKeyedCollection<TKey, T>.RemoveKey(TKey key)
        {
            throw new NotSupportedException(ResourceProvider.CollectionReadonly());
        }

        public bool TryGetValue(TKey key, out T item)
        {
            return this.innerKeyedCollection.TryGetValue(key, out item);
        }

        public T this[TKey key]
        {
            get { return this.innerKeyedCollection[key]; }
        }

        public IEnumerable<TKey> Keys
        {
            get { return this.innerKeyedCollection.Keys; }
        }

        public IDictionary<TKey, T> AsReadOnlyDictionary()
        {
            return this.innerKeyedCollection.AsReadOnlyDictionary();
        }
        #endregion
    }
}