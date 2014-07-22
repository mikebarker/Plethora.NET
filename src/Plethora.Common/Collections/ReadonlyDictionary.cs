using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Plethora.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializable, IDeserializationCallback
    {
        private readonly Dictionary<TKey, TValue> innerDictionary;

        #region Constructors

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> dictionary)
            : this(dictionary, EqualityComparer<TKey>.Default)
        {
        }

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, IEqualityComparer<TKey> comparer)
        {
            var collection = enumerable as ICollection<KeyValuePair<TKey, TValue>>;
            this.innerDictionary = (collection == null)
                ? new Dictionary<TKey, TValue>(comparer)
                : new Dictionary<TKey, TValue>(collection.Count, comparer);

            foreach (var keyValuePair in enumerable)
            {
                TKey key = keyValuePair.Key;
                TValue value = keyValuePair.Value;

                this.innerDictionary.Add(key, value);
            }
        }

        #endregion

        #region Implementation of IEnumerable<KeyValuePair<TKey, TValue>>

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey, TValue>>

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }


        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.innerDictionary).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.innerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Implementation of IDictionary<TKey, TValue>

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw new InvalidOperationException(ResourceProvider.CollectionReadonly()); }
        }


        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return innerDictionary[key]; }
        }

        public ICollection<TKey> Keys
        {
            get { return innerDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return innerDictionary.Values; }
        }

        #endregion

        #region Implementation of ISerializable, IDeserializationCallback

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            innerDictionary.GetObjectData(info, context);
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            innerDictionary.OnDeserialization(sender);
        }

        #endregion
    }

    public static class ReadOnlyDictionaryHelper
    {
        public static ReadOnlyDictionary<TKey, TValue> AsReadonly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
