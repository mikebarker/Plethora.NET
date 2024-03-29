﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Plethora.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [Serializable]
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, ISerializable, IDeserializationCallback
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> innerDictionary;

        #region Constructors

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
            : this(enumerable, ExtractKeyComparer(enumerable) ?? EqualityComparer<TKey>.Default)
        {
        }

        public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary)
            : this(dictionary, dictionary.Comparer)
        {
        }

        public ReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, IEqualityComparer<TKey> comparer)
        {
            this.innerDictionary = (enumerable is not ICollection<KeyValuePair<TKey, TValue>> collection)
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
            return this.innerDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey, TValue>>

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
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

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Implementation of IDictionary<TKey, TValue>

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw new InvalidOperationException(ResourceProvider.CollectionReadOnly()); }
        }


        public bool ContainsKey(TKey key)
        {
            return this.innerDictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.innerDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return this.innerDictionary[key]; }
        }

        public ICollection<TKey> Keys
        {
            get { return this.innerDictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return this.innerDictionary.Values; }
        }

        #endregion

        #region Implementation of IReadOnlyDictionary<TKey, TValue>

        bool IReadOnlyDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return this.ContainsKey(key);
        }

        bool IReadOnlyDictionary<TKey, TValue>.TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            return this.TryGetValue(key, out value);
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return this.Keys; }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return this.Values; }
        }

        #endregion

        #region Implementation of ISerializable, IDeserializationCallback

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to the serialization ctor
#endif
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.innerDictionary.GetObjectData(info, context);
        }

        void IDeserializationCallback.OnDeserialization(object? sender)
        {
            this.innerDictionary.OnDeserialization(sender);
        }

        #endregion

        #region Private Static Members

        private static IEqualityComparer<TKey>? ExtractKeyComparer(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            if (enumerable is not Dictionary<TKey, TValue> dictionary)
                return null;

            return dictionary.Comparer;
        }

        #endregion
    }

    public static class ReadOnlyDictionaryHelper
    {
        public static ReadOnlyDictionary<TKey, TValue> AsReadonly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
