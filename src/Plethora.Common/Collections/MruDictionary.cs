using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Plethora.Collections
{
    public static class MruDictionary
    {
        public const int DefaultMaxEntries = 1024;
        internal const double DefaultWatermarkPercent = 0.75;
    }


    /// <summary>
    /// An implementation of <see cref="IDictionary{TKey,TValue}"/> which tracks the number of times each item is accessed.
    /// When <see cref="Count"/> exceeds <see cref="MaxEntries"/> the least used entries are dropped from the dictionary until the <see cref="Count"/> reaches <see cref="Watermark"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <remarks>
    /// Accessing the entries via LINQ will cause all entries itterated through to be marked as accessed. It is recommended that this method of access be avoided.
    /// </remarks>
    public class MruDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, MruEntry<TValue>> innerDictionary;
        private int maxEntries;
        private int? watermark;

        /// <summary>
        /// Initializes a new instance of the <see cref="MruDictionary{TKey,TValue}"/> class.
        /// </summary>
        /// <param name="maxEntries">The maximum number of elements this dictionary will store.</param>
        /// <param name="watermark">The number of enteries this dictionary will reduce to once the <see cref="MaxEntries"/> is exceeded.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> implementation to use when comparing keys, or null to use the default <see cref="EqualityComparer{TKey}"/> for the type of the key.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxEntries"/> is less than two.
        /// OR
        /// <paramref name="watermark"/> is not null and is greater than or equal to <paramref name="maxEntries"/> or less than 1.
        /// </exception>
        public MruDictionary(
            int maxEntries = MruDictionary.DefaultMaxEntries,
            int? watermark = null,
            IEqualityComparer<TKey> comparer = null)
        {
            if (maxEntries < 2)
                throw new ArgumentOutOfRangeException(nameof(maxEntries), maxEntries, ResourceProvider.ArgMustBeGreaterThan(nameof(maxEntries), 2));

            if (watermark != null)
            {
                if ((watermark < 1) || (watermark >= maxEntries))
                    throw new ArgumentOutOfRangeException(nameof(watermark));
            }

            this.maxEntries = maxEntries;
            this.watermark = watermark;
            this.innerDictionary = new Dictionary<TKey, MruEntry<TValue>>(comparer);
        }

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.innerDictionary
                .Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, this.GetValue(pair.Value)))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey, TValue>>

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
        {
            this.Add(pair.Key, pair.Value);
        }

        public void Clear()
        {
            this.innerDictionary.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            TValue value;
            if (!this.TryGetValue(pair.Key, out value))
                return false;

            return EqualityComparer<TValue>.Default.Equals(value, pair.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Rank != 1)
                throw new ArgumentException(ResourceProvider.ArgArrayMultiDimensionNotSupported());

            if ((array.Length - arrayIndex) < this.Count)
                throw new ArgumentException(ResourceProvider.ArgInvalidOffsetLength(nameof(arrayIndex), nameof(this.Count)));


            int i = 0;
            foreach (var pair in this.innerDictionary)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(pair.Key, this.GetValue(pair.Value));
                i++;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
        {
            TValue value;
            if (!this.TryGetValue(pair.Key, out value))
                return false;

            if (!EqualityComparer<TValue>.Default.Equals(value, pair.Value))
                return false;

            return this.Remove(pair.Key);
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, MruEntry<TValue>>>)this.innerDictionary).IsReadOnly; }
        }

        #endregion

        #region Implementation of IDictionary<TKey, TValue>

        public bool ContainsKey([NotNull] TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return this.innerDictionary.ContainsKey(key);
        }

        public void Add([NotNull] TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            this.ReclaimLeastUsedEnties(1);
            this.innerDictionary.Add(key, new MruEntry<TValue>(value));
        }

        public bool Remove([NotNull] TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return this.innerDictionary.Remove(key);
        }

        public bool TryGetValue([NotNull] TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            MruEntry<TValue> entry;
            if (!this.innerDictionary.TryGetValue(key, out entry))
            {
                value = default(TValue);
                return false;
            }

            value = this.GetValue(entry);
            return true;
        }

        public TValue this[[NotNull] TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                MruEntry<TValue> entry = this.innerDictionary[key];
                return this.GetValue(entry);
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                MruEntry<TValue> entry;
                if (this.innerDictionary.TryGetValue(key, out entry))
                {
                    entry.Value = value;
                }
                else
                {
                    this.innerDictionary[key] = new MruEntry<TValue>(value);
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get 
            {
                return this.innerDictionary.Values
                    .Select(entry => this.GetValue(entry))
                    .ToList()
                    .AsReadOnly();
            }
        }

        #endregion

        /// <summary>
        /// Sets the <see cref="MaxEntries"/> and <see cref="Watermark"/> of this <see cref="MruDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="maxEntries">The maximum number of elements this dictionary will store.</param>
        /// <param name="watermark">The number of enteries this dictionary will reduce to when <paramref name="maxEntries"/> will be exceeded.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxEntries"/> is less than two.
        /// OR
        /// <paramref name="watermark"/> is not null and is greater than or equal to <paramref name="maxEntries"/> or less than 1.
        /// </exception>
        public void SetMaxEntriesAndWatermark(int maxEntries, int? watermark)
        {
            if (maxEntries < 2)
                throw new ArgumentOutOfRangeException(nameof(maxEntries), maxEntries, ResourceProvider.ArgMustBeGreaterThan(nameof(maxEntries), 2));

            if (watermark != null)
            {
                if ((watermark < 1) || (watermark >= maxEntries))
                    throw new ArgumentOutOfRangeException(nameof(watermark));
            }

            this.maxEntries = maxEntries;
            this.watermark = watermark;

            this.ReclaimLeastUsedEnties(0);
        }

        /// <summary>
        /// The maximum number of enteries this dictionary can contain.
        /// </summary>
        /// <remarks>
        /// When <see cref="Count"/> exceeds <see cref="MaxEntries"/> the least used entries are dropped from the dictionary until the <see cref="Count"/> reaches <see cref="Watermark"/>.
        /// </remarks>
        public int MaxEntries
        {
            get { return this.maxEntries; }
        }

        /// <summary>
        /// The number of enteries this dictionary will reduce to once the <see cref="MaxEntries"/> is exceeded.
        /// </summary>
        /// <remarks>
        /// When <see cref="Count"/> exceeds <see cref="MaxEntries"/> the least used entries are dropped from the dictionary until the <see cref="Count"/> reaches <see cref="Watermark"/>.
        /// </remarks>
        public int? Watermark
        {
            get { return this.watermark; }
        }

        private void ReclaimLeastUsedEnties(int additionalEntries)
        {
            if ((this.Count + additionalEntries) <= this.maxEntries)
                return;

            int watermarkCount = (this.Watermark != null)
                ? this.Watermark.Value
                : Math.Max(1, (int)Math.Floor(this.maxEntries * MruDictionary.DefaultWatermarkPercent));

            int reclaimCount = ((this.Count + additionalEntries) - watermarkCount);
            if (reclaimCount <= 0)
                return;

            IEnumerable<TKey> reclaimKeys = this.innerDictionary
                .OrderBy(pair => pair.Value.AccessCount)
                .Select(pair => pair.Key)
                .Take(reclaimCount)
                .ToList();

            foreach (TKey reclaimKey in reclaimKeys)
            {
                this.innerDictionary.Remove(reclaimKey);
            }
        }

        private TValue GetValue(MruEntry<TValue> entry)
        {
            TValue value = entry.Value;
            if (entry.AccessCount == uint.MaxValue)
            {
                this.ReduceAccessCounts();
            }
            return value;
        }

        private void ReduceAccessCounts()
        {
            foreach (MruEntry<TValue> entry in this.innerDictionary.Values)
            {
                entry.AccessCount >>= 1; //Divide by 2
            }
        }
    }

    internal sealed class MruEntry<TValue>
    {
        private uint accessCount;
        private TValue value;

        public MruEntry(TValue value)
        {
            this.accessCount = 0;
            this.value = value;
        }

        public TValue Value
        {
            get
            {
                unchecked
                {
                    this.accessCount++;
                }
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public uint AccessCount
        {
            get { return this.accessCount; }
            internal set { this.accessCount = value; }
        }
    }
}
