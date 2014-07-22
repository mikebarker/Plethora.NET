using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Plethora.Collections
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class ReadOnlyHashSet<T> : ISet<T>, ISerializable, IDeserializationCallback
    {
        private readonly HashSet<T> innerHashSet;

        #region Constructors

        public ReadOnlyHashSet(IEnumerable<T> enumerable)
            : this(enumerable, EqualityComparer<T>.Default)
        {
        }

        public ReadOnlyHashSet(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
        {
            innerHashSet = new HashSet<T>(enumerable, comparer);
            innerHashSet.TrimExcess();
        }

        #endregion
        
        #region Implementation of IEnumerable<T>

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.innerHashSet.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        public bool Contains(T item)
        {
            return this.innerHashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerHashSet.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        public int Count
        {
            get { return innerHashSet.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Implementation of ISet<T>

        bool ISet<T>.Add(T item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {

            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return innerHashSet.IsSubsetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return innerHashSet.IsProperSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return innerHashSet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return innerHashSet.IsProperSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return innerHashSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return innerHashSet.SetEquals(other);
        }
        
        #endregion

        #region Implementation of ISerializable and IDeserializationCallback

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            innerHashSet.GetObjectData(info, context);
        }

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            innerHashSet.OnDeserialization(sender);
        }

        #endregion

        #region Public Members

        public void CopyTo(T[] array)
        {
            innerHashSet.CopyTo(array);
        }

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            innerHashSet.CopyTo(array, arrayIndex, count);
        }

        public IEqualityComparer<T> Comparer
        {
            get { return innerHashSet.Comparer; }
        }

        #endregion
    }

    public static class ReadOnlyHashSetHelper
    {
        public static ReadOnlyHashSet<T> AsReadonly<T>(this HashSet<T> hashSet)
        {
            return new ReadOnlyHashSet<T>(hashSet);
        }
    }
}
