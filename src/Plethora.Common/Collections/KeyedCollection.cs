using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Plethora.Collections
{
    /// <summary>
    /// A collection which is accessible by a unique key per item.
    /// </summary>
    /// <typeparam name="TKey">The type of the keyed element of the collection</typeparam>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <remarks>
    /// This is a more efficient implementation than the
    /// <see cref="System.Collections.ObjectModel.KeyedCollection{TKey,TItem}"/>
    /// class.
    /// </remarks>
    [Serializable]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public class KeyedCollection<TKey, T> : IKeyedCollection<TKey, T>
    {
        private class ReadOnlyDictionaryWrapper : IDictionary<TKey, T>
        {
            #region Fields

            private readonly KeyedCollection<TKey, T> keyedCollection;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialise a new instance of the <see cref="ReadOnlyDictionaryWrapper"/> class.
            /// </summary>
            /// <param name="keyedCollection">
            /// The <see cref="IKeyedCollection{TKey,T}"/> to be wrapped.
            /// </param>
            public ReadOnlyDictionaryWrapper(KeyedCollection<TKey, T> keyedCollection)
            {
                //Validation
                if (keyedCollection == null)
                    throw new ArgumentNullException(nameof(keyedCollection));


                this.keyedCollection = keyedCollection;
            }
            #endregion

            #region Implementation of IEnumerable

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            #endregion

            #region Implementation of IEnumerable<KeyValuePair<TKey,T>>

            public IEnumerator<KeyValuePair<TKey, T>> GetEnumerator()
            {
                return this.keyedCollection.innerDictionary.GetEnumerator();
            }
            #endregion

            #region Implementation of ICollection<KeyValuePair<TKey,T>>

            void ICollection<KeyValuePair<TKey,T>>.Add(KeyValuePair<TKey, T> item)
            {
                throw new NotSupportedException("Dictionary is readonly.");
            }

            void ICollection<KeyValuePair<TKey,T>>.Clear()
            {
                throw new NotSupportedException("Dictionary is readonly.");
            }

            bool ICollection<KeyValuePair<TKey,T>>.Contains(KeyValuePair<TKey, T> pair)
            {
                return this.keyedCollection.ContainsKey(pair.Key);
            }

            void ICollection<KeyValuePair<TKey,T>>.CopyTo(KeyValuePair<TKey, T>[] array, int arrayIndex)
            {
                ((IDictionary<TKey, T>)this.keyedCollection.innerDictionary).CopyTo(array, arrayIndex);
            }

            bool ICollection<KeyValuePair<TKey,T>>.Remove(KeyValuePair<TKey, T> item)
            {
                throw new NotSupportedException("Dictionary is readonly.");
            }

            public int Count
            {
                get { return this.keyedCollection.Count; }
            }

            bool ICollection<KeyValuePair<TKey, T>>.IsReadOnly
            {
                get { return true; }
            }
            #endregion

            #region Implementation of IDictionary<TKey,T>

            public bool ContainsKey(TKey key)
            {
                return this.keyedCollection.ContainsKey(key);
            }

            void IDictionary<TKey, T>.Add(TKey key, T value)
            {
                throw new NotSupportedException("Dictionary is readonly.");
            }

            bool IDictionary<TKey,T>.Remove(TKey key)
            {
                throw new NotSupportedException("Dictionary is readonly.");
            }

            public bool TryGetValue(TKey key, out T value)
            {
                return this.keyedCollection.TryGetValue(key, out value);
            }

            public T this[TKey key]
            {
                get { return this.keyedCollection[key]; }
                set { throw new NotSupportedException("Dictionary is readonly."); }
            }

            ICollection<TKey> IDictionary<TKey, T>.Keys
            {
                get { return this.keyedCollection.innerDictionary.Keys; }
            }

            ICollection<T> IDictionary<TKey, T>.Values
            {
                get { return this.keyedCollection.innerDictionary.Values; }
            }
            #endregion
        }

        #region Fields

        private readonly Dictionary<TKey, T> innerDictionary;
        private readonly Func<T, TKey> keySelector;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="KeyedCollection{TKey,T}"/> class.
        /// </summary>
        public KeyedCollection(Func<T, TKey> keySelector)
            : this(keySelector, new T[0], null)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="KeyedCollection{TKey,T}"/> class.
        /// </summary>
        public KeyedCollection(Func<T, TKey> keySelector, IEqualityComparer<TKey> comparer)
            : this(keySelector, new T[0], comparer)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="KeyedCollection{TKey,T}"/> class.
        /// </summary>
        public KeyedCollection(Func<T, TKey> keySelector, IEnumerable<T> enumerable)
            : this(keySelector, enumerable, null)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="KeyedCollection{TKey,T}"/> class.
        /// </summary>
        public KeyedCollection(Func<T, TKey> keySelector, IEnumerable<T> enumerable, IEqualityComparer<TKey> comparer)
        {
            //Validation
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));


            this.keySelector = keySelector;
            this.innerDictionary = enumerable.ToDictionary(keySelector, comparer);
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
            return this.GetEnumerator();
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
            return this.innerDictionary.Values.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        public void Add(T item)
        {
            var key = this.GetKey(item);
            this.innerDictionary.Add(key, item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.innerDictionary.Clear();
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
            var key = this.GetKey(item);
            return this.ContainsKey(key);
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
            this.innerDictionary.Values.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection{T}"/>;
        /// otherwise, false. 
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(T item)
        {
            var key = this.GetKey(item);
            return this.innerDictionary.Remove(key);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="ICollection{T}"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="ICollection{T}"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region Implementation of IKeyedCollection<TKey,T>

        /// <summary>
        /// Adds the item to the collection, or if an item already exists with a matching key the item is updated.
        /// </summary>
        /// <param name="item">The item to be added or updated.</param>
        /// <returns>
        /// true if <paramref name="item"/> was added to the <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        public bool Upsert(T item)
        {
            var key = this.GetKey(item);
            bool result = this.innerDictionary.ContainsKey(key);
            this.innerDictionary[key] = item;
            return !result;
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
            return this.innerDictionary.ContainsKey(key);
        }

        public bool RemoveKey(TKey key)
        {
            return this.innerDictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out T item)
        {
            return this.innerDictionary.TryGetValue(key, out item);
        }

        public T this[TKey key]
        {
            get { return this.innerDictionary[key]; }
        }

        public IEnumerable<TKey> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public IDictionary<TKey, T> AsReadOnlyDictionary()
        {
            return new ReadOnlyDictionaryWrapper(this);
        }
        #endregion

        #region Public Methods

        public ReadOnlyKeyedCollection<TKey, T> AsReadOnly()
        {
            return new ReadOnlyKeyedCollection<TKey, T>(this);
        }
        #endregion

        #region Private Methods

        private TKey GetKey(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return this.keySelector(item);
        }
        #endregion
    }

    public static class KeyedCollection
    {
        public static KeyedCollection<TKey, T> Create<TKey, T>(Func<T, TKey> keySelector)
        {
            return new KeyedCollection<TKey, T>(keySelector);
        }

        public static KeyedCollection<TKey, T> ToKeyedCollection<TKey, T>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return new KeyedCollection<TKey, T>(keySelector, enumerable);
        }
    }
}