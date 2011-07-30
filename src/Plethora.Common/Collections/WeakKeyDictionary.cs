using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections
{
    /// <summary>
    /// An implementation of <see cref="IDictionary{TKey,TValue}"/> where the
    /// key is held as a weak reference, allowing it to be garbage collected.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary's key.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary's value.</typeparam>
    public class WeakKeyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private class WeakKey : WeakReference<TKey>
        {
            #region Fields

            private readonly int hashCode;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialise a new instance of the <see cref="WeakKey"/> class.
            /// </summary>
            public WeakKey(TKey target, IEqualityComparer<TKey> comparer)
                : base(target)
            {
                //Capture the hash code to prevent it changing when the
                // target is garbage collected.
                this.hashCode = comparer.GetHashCode(target);
            }
            #endregion

            #region Overrides of Object

            public override int GetHashCode()
            {
                return hashCode;
            }
            #endregion
        }

        private class WeakKeyComparer : IEqualityComparer<WeakKey>
        {
            #region Static Instance

            private static WeakKeyComparer defaultInstance;
            public static WeakKeyComparer Default
            {
                get
                {
                    //Thread safety comment: At worst this will produce multiple
                    // instance of the default WeakKeyComparer if called by several
                    // threads during initialisation. This is not an issue for this class,
                    // and is not worth locking to prevent.
                    if (defaultInstance == null)
                    {
                        defaultInstance = new WeakKeyComparer(EqualityComparer<TKey>.Default);
                    }
                    return defaultInstance;
                }
            }
            #endregion

            #region Fields

            private readonly IEqualityComparer<TKey> innerComparer;
            #endregion

            #region Constructors

            public WeakKeyComparer(IEqualityComparer<TKey> comparer)
            {
                //Validation
                if (comparer == null)
                    throw new ArgumentNullException("comparer");

                this.innerComparer = comparer;
            }
            #endregion

            #region Implementation of IEqualityComparer<WeakKey>

            public bool Equals(WeakKey weakKeyX, WeakKey weakKeyY)
            {
                var keyX = weakKeyX.Target;
                var keyY = weakKeyY.Target;

                //Dead targets are not considered equal
                if ((keyX == null) || (keyY == null))
                    return false;

                return this.innerComparer.Equals(keyX, keyY);
            }

            public int GetHashCode(WeakKey weakKey)
            {
                return weakKey.GetHashCode();
            }
            #endregion

            #region Properties

            public IEqualityComparer<TKey> InnerComparer
            {
                get { return this.innerComparer; }
            }
            #endregion
        }

        #region Fields

        private readonly WeakKeyComparer weakKeyComparer;
        private readonly Dictionary<WeakKey, TValue> innerDictionary;
        #endregion


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary{TKey,TValue}"/>
        /// class that is empty, has the default initial capacity, and uses the default
        /// equality comparer for the key type
        /// </summary>
        public WeakKeyDictionary()
            : this(0, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary{TKey,TValue}"/>
        /// class that is empty, has the specified initial capacity, and uses the
        /// default equality comparer for the key type.
        /// </summary>
        public WeakKeyDictionary(int capacity)
            : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary{TKey,TValue}"/>
        /// class that is empty, has the default initial capacity, and uses the
        /// specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public WeakKeyDictionary(IEqualityComparer<TKey> equalityComparer)
            : this(0, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakKeyDictionary{TKey,TValue}"/>
        /// class that is empty, has the specified initial capacity, and uses the
        /// specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public WeakKeyDictionary(int capacity, IEqualityComparer<TKey> keyComparer)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", capacity, "Capacity may not be less than zero.");

            this.weakKeyComparer = (keyComparer == null)
                ? WeakKeyComparer.Default
                : new WeakKeyComparer(keyComparer);

            this.innerDictionary = new Dictionary<WeakKey, TValue>(
                capacity, this.weakKeyComparer);
        }
        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable<KeyValuePair<TKey,TValue>>

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
        /// </returns>
        /// <remarks>
        /// This only itterates over pairs where the key is alive. The itterator snapshots
        /// the key's target and returns a strong reference to the target (preventing garbage 
        /// collection).
        /// </remarks>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDictionary
                .Select(pair => new KeyValuePair<TKey, TValue>(pair.Key.Target, pair.Value))
                .Where(pair => pair.Key != null)
                .GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        void ICollection<KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            var weakPair = new KeyValuePair<WeakKey, TValue>(GetWeakKey(item.Key), item.Value);
            return ((ICollection<KeyValuePair<WeakKey, TValue>>)this.innerDictionary).Contains(weakPair);
        }

        void ICollection<KeyValuePair<TKey,TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public int Count
        {
            get { return this.Keys.Count; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey,TValue>>)this.innerDictionary).IsReadOnly; }
        }

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var weakKey = GetWeakKey(key);
            return this.innerDictionary.ContainsKey(weakKey);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var weakKey = GetWeakKey(key);

            this.innerDictionary.Add(weakKey, value);
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var weakKey = GetWeakKey(key);
            return this.innerDictionary.Remove(weakKey);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            var weakKey = GetWeakKey(key);
            return this.innerDictionary.TryGetValue(weakKey, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                var weakKey = GetWeakKey(key);
                return this.innerDictionary[weakKey];
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                var weakKey = GetWeakKey(key);
                this.innerDictionary[weakKey] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get { return this.Select(pair => pair.Key).ToList(); }
        }

        public ICollection<TValue> Values
        {
            get { return this.Select(pair => pair.Value).ToList(); }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a fast estimate of the value of <see cref="Count"/>.
        /// </summary>
        /// <remarks>
        /// This will always return a high estimate (or correct value) of the count.
        /// </remarks>
        public int EstimateCount
        {
            get { return this.innerDictionary.Count; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Removes garbage-collected entries from the dictionary.
        /// </summary>
        /// <returns>
        /// true if 'dead' entries were found and space was reclaimed; otherwise false.
        /// </returns>
        public bool TrimExcess()
        {
            var deadKeyList = innerDictionary
                .Where(pair => !pair.Key.IsAlive)
                .Select(pair => pair.Key)
                .ToList();

            if (deadKeyList.Count == 0)
                return false;

            foreach (var deadKey in deadKeyList)
            {
                innerDictionary.Remove(deadKey);
            }

            return true;
        }
        #endregion

        private WeakKey GetWeakKey(TKey key)
        {
            return new WeakKey(key, this.weakKeyComparer.InnerComparer);
        }
    }

}
