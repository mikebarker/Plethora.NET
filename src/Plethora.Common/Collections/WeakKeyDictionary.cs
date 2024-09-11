using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    {
        private class WeakKey : IEquatable<WeakKey>
        {
            #region Fields

            private readonly WeakReference<TKey> weakReference;
            private readonly int hashCode;
            private readonly IEqualityComparer<TKey> comparer;

            #endregion

            #region Constructors

            /// <summary>
            /// Initialise a new instance of the <see cref="WeakKey"/> class.
            /// </summary>
            public WeakKey(TKey target, IEqualityComparer<TKey> comparer)
            {
                //validation
                ArgumentNullException.ThrowIfNull(target);
                ArgumentNullException.ThrowIfNull(comparer);


                this.weakReference = new(target);

                //Capture the hash code to prevent it changing if the
                // target is garbage collected.
                this.hashCode = comparer.GetHashCode(target);
                this.comparer = comparer;
            }

            #endregion

            #region Implementation of IEquatable<WeakKeyDictionary<TKey,TValue>.WeakKey>

            public bool Equals(WeakKey? other)
            {
                if (other is null)
                    return false;

                if (ReferenceEquals(this, other))
                    return true;

                if (this.hashCode != other.hashCode)
                    return false;

                this.weakReference.TryGetTarget(out var thisTarget);
                other.weakReference.TryGetTarget(out var otherTarget);

                return this.comparer.Equals(thisTarget, otherTarget);
            }

            #endregion

            #region 

            public bool TryGetTarget([MaybeNullWhen(false)] out TKey target)
            {
                return this.weakReference.TryGetTarget(out target);
            }

            #endregion

            #region Object Overrides

            public override bool Equals(object? obj)
            {
                if (obj is WeakKey other)
                {
                    return this.Equals(other);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return this.hashCode;
            }

            public override string ToString()
            {
                if (this.weakReference.TryGetTarget(out var target))
                {
                    return $"WeakKey [{target}]";
                }
                else
                {
                    return "WeakKey [<dead>]";
                }
            }

            #endregion
        }


        private class WeakKeyComparer : IEqualityComparer<WeakKey>
        {
            #region Static Instance

            private static WeakKeyComparer? defaultInstance;
            public static WeakKeyComparer Default
            {
                get
                {
                    //Thread safety comment: At worst this will produce multiple
                    // instance of the default WeakKeyComparer if called by several
                    // threads during initialisation. This is not an issue for this class,
                    // and is not worth locking to prevent.
                    defaultInstance ??= new(EqualityComparer<TKey>.Default);
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
                ArgumentNullException.ThrowIfNull(comparer);

                this.innerComparer = comparer;
            }
            #endregion

            #region Implementation of IEqualityComparer<WeakKey>

            public bool Equals(WeakKey? weakKeyX, WeakKey? weakKeyY)
            {
                if ((weakKeyX is null) && (weakKeyY is null))
                    return true;

                if ((weakKeyX is null) || (weakKeyY is null))
                    return false;

                return weakKeyX.Equals(weakKeyY);
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
        public WeakKeyDictionary(int capacity, IEqualityComparer<TKey>? keyComparer)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 0);

            this.weakKeyComparer = (keyComparer is null)
                ? WeakKeyComparer.Default
                : new WeakKeyComparer(keyComparer);

            this.innerDictionary = new(capacity, this.weakKeyComparer);
        }
        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
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
        /// This only iterates over pairs where the key is alive. The iterator snapshots
        /// the key's target and returns a strong reference to the target (preventing garbage 
        /// collection).
        /// </remarks>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.innerDictionary
                .Select(pair =>
                {
                    pair.Key.TryGetTarget(out var target);
                    return new { Key = target, pair.Value };
                })
                .Where(x => x.Key is not null)
                .Select(x => new KeyValuePair<TKey, TValue>(x.Key!, x.Value))
                .GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        void ICollection<KeyValuePair<TKey,TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            KeyValuePair<WeakKey, TValue> weakPair = new(this.GetWeakKey(item.Key), item.Value);
            return ((ICollection<KeyValuePair<WeakKey, TValue>>)this.innerDictionary).Contains(weakPair);
        }

        void ICollection<KeyValuePair<TKey,TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        public int Count
        {
            get { return this.Keys.Count; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<WeakKey, TValue>>)this.innerDictionary).IsReadOnly; }
        }

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var weakKey = this.GetWeakKey(key);
            return this.innerDictionary.ContainsKey(weakKey);
        }

        public void Add(TKey key, TValue value)
        {
            ArgumentNullException.ThrowIfNull(key);

            var weakKey = this.GetWeakKey(key);

            this.innerDictionary.Add(weakKey, value);
        }

        public bool Remove(TKey key)
        {
            ArgumentNullException.ThrowIfNull(key);

            var weakKey = this.GetWeakKey(key);
            return this.innerDictionary.Remove(weakKey);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            ArgumentNullException.ThrowIfNull(key);

            var weakKey = this.GetWeakKey(key);
            return this.innerDictionary.TryGetValue(weakKey, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                ArgumentNullException.ThrowIfNull(key);

                var weakKey = this.GetWeakKey(key);
                return this.innerDictionary[weakKey];
            }
            set
            {
                ArgumentNullException.ThrowIfNull(key);

                var weakKey = this.GetWeakKey(key);
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
        /// This will always return an estimate greater then or equal to the exact count.
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
            var deadKeyList = this.innerDictionary
                .Where(pair => !pair.Key.TryGetTarget(out var target))
                .Select(pair => pair.Key)
                .ToList();

            if (deadKeyList.Count == 0)
                return false;

            foreach (var deadKey in deadKeyList)
            {
                this.innerDictionary.Remove(deadKey);
            }

            return true;
        }
        #endregion

        #region PrivateMethods

        private WeakKey GetWeakKey(TKey key)
        {
            return new WeakKey(key, this.weakKeyComparer.InnerComparer);
        }
        #endregion


/*
        private class AutoCleanup
        {
            #region Fields

            private const int LOW_ACTIVITY_TIMER = 5 * 60 * 1000; // 5 min
            private const int HIGH_ACTIVITY_TIMER = 2 * 1000;     // 2 sec

            private readonly Timer cleanupTimer;
            private readonly WeakKeyDictionary<TKey, TValue> dictionary;
            private int inCleanUp = 0;
            #endregion

            #region Constructors

            public AutoCleanup(WeakKeyDictionary<TKey, TValue> dictionary)
            {
                //validation
                ArgumentNullException.ThrowIfNull(dictionary);

                this.dictionary = dictionary;
                this.cleanupTimer = new Timer(Cleanup, null, LOW_ACTIVITY_TIMER, LOW_ACTIVITY_TIMER);
            }
            #endregion

            #region Private Methods

            private void Cleanup(object state)
            {
                if (Interlocked.CompareExchange(ref inCleanUp, 1, 0) != 0)
                    return;

                try
                {
                    bool anyCleanup;
                    lock(dictionary)
                    {
                        anyCleanup = dictionary.TrimExcess();
                    }

                    if (anyCleanup)
                        cleanupTimer.Change(HIGH_ACTIVITY_TIMER, HIGH_ACTIVITY_TIMER);
                    else
                        cleanupTimer.Change(LOW_ACTIVITY_TIMER, LOW_ACTIVITY_TIMER);
                }
                finally
                {
                    inCleanUp = 0;
                }
            }
            #endregion
        }
*/
    }

}
