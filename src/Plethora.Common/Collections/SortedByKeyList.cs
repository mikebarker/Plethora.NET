﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Plethora.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [Serializable]
    public class SortedByKeyList<TKey, T> : IList<T>
    {
        #region Fields

        private readonly Func<T, TKey> getKeyFunc;
        private readonly List<T> innerList;
        private readonly IComparer<TKey> comparer;
        private readonly DuplicatesPolicy duplicatesPolicy;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="SortedByKeyList{TKey, T}"/>, with a duplicate policy 
        /// of <see cref="Plethora.Collections.DuplicatesPolicy.Error"/> and using the
        /// default comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key for an element.</param>
        public SortedByKeyList(Func<T, TKey> getKeyFunc)
            : this(getKeyFunc, DuplicatesPolicy.Allow, Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SortedByKeyList{TKey, T}"/>, with a duplicate policy 
        /// of <see cref="Plethora.Collections.DuplicatesPolicy.Error"/>.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key for an element.</param>
        /// <param name="comparer">The comparer used to sort the list by key.</param>
        public SortedByKeyList(Func<T, TKey> getKeyFunc, IComparer<TKey> comparer)
            : this(getKeyFunc, DuplicatesPolicy.Allow, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SortedByKeyList{TKey, T}"/> using the
        /// default comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key for an element.</param>
        /// <param name="duplicatesPolicy">The policy to be followed when adding duplicate elements to the list.</param>
        public SortedByKeyList(Func<T, TKey> getKeyFunc, DuplicatesPolicy duplicatesPolicy)
            : this(getKeyFunc, duplicatesPolicy, Comparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SortedByKeyList{TKey, T}"/>.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key for an element.</param>
        /// <param name="duplicatesPolicy">The policy to be followed when adding duplicate elements to the list.</param>
        /// <param name="comparer">The comparer used to sort the list by key.</param>
        public SortedByKeyList(Func<T, TKey> getKeyFunc, DuplicatesPolicy duplicatesPolicy, IComparer<TKey> comparer)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(getKeyFunc);

            if ((duplicatesPolicy != DuplicatesPolicy.Allow) &&
                (duplicatesPolicy != DuplicatesPolicy.Ignore) &&
                (duplicatesPolicy != DuplicatesPolicy.Replace) &&
                (duplicatesPolicy != DuplicatesPolicy.Error))
            {
                throw new ArgumentOutOfRangeException(nameof(duplicatesPolicy), duplicatesPolicy,
                    ResourceProvider.ArgMustBeOneOf(nameof(duplicatesPolicy), "Allow", "Ignore", "Replace", "Error"));
            }

            ArgumentNullException.ThrowIfNull(comparer);


            this.getKeyFunc = getKeyFunc;
            this.duplicatesPolicy = duplicatesPolicy;
            this.comparer = comparer;
            this.innerList = new();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SortedByKeyList{TKey, T}"/>.
        /// </summary>
        /// <param name="getKeyFunc">The function which gets the key for an element.</param>
        /// <param name="enumerable">The collection whose elements are to be copied to the sorted list.</param>
        /// <param name="duplicatesPolicy">The policy to be followed when adding duplicate elements to the list.</param>
        /// <param name="comparer">The comparer used to sort the list the key.</param>
        public SortedByKeyList(Func<T, TKey> getKeyFunc, IEnumerable<T> enumerable, DuplicatesPolicy duplicatesPolicy, IComparer<TKey> comparer)
            : this(getKeyFunc, duplicatesPolicy, comparer)
        {
            this.AddRange(enumerable);
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
            return this.innerList.GetEnumerator();
        }
        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <returns>
        /// The index of the item after it was added; or -1 if a duplicate was attempt was
        /// made to insert a duplicate.
        /// </returns>
        public int Add(T item)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(item);


            TKey key = this.getKeyFunc(item);
            int index = this.BinarySearch(key);

            if (index >= 0)
            {
                if (this.duplicatesPolicy == DuplicatesPolicy.Error)
                {
                    throw new InvalidOperationException(ResourceProvider.ArgAddingDuplicate());
                }
                else if (this.duplicatesPolicy == DuplicatesPolicy.Ignore)
                {
                    return -1;
                }
                else if (this.duplicatesPolicy == DuplicatesPolicy.Replace)
                {
                    this.innerList[index] = item;
                    return index;
                }
            }
            else
            {
                index = ~index;
            }

            this.innerList.Insert(index, item);
            return index;
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.innerList.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="ICollection{T}"/>.</param>
        bool ICollection<T>.Contains(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            TKey key = this.getKeyFunc(item);
            return this.Contains(key);
        }

        /// <summary>
        /// Determines whether the <see cref="ICollection{T}"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if an item with the <paramref name="key"/> is found in the <see cref="ICollection{T}"/>; otherwise, false.
        /// </returns>
        /// <param name="key">The key of the object to locate in the <see cref="ICollection{T}"/>.</param>
        public bool Contains(TKey key)
        {
            ArgumentNullException.ThrowIfNull(key);


            int index = this.BinarySearch(key);
            return (index >= 0);
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
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="array"/> is multidimensional.
        ///     -or-
        ///     <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
        ///     -or-
        ///     The number of elements in the source <see cref="ICollection{T}"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.
        ///     -or-
        ///     Type <typeparamref name="T"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection{T}"/>;
        /// otherwise, false. This method also returns false if <paramref name="item"/> is not found in the
        /// original <see cref="ICollection{T}"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(T item)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(item);


            TKey key = this.getKeyFunc(item);

            int firstIndexOfKey = this.IndexOf(key);
            if (firstIndexOfKey < 0)
                return false;

            int lastIndexOfKey = (this.IsUnique)
                ? firstIndexOfKey
                : this.LastIndexOf(key);  //Can not return -1, since IndexOf found the key

            //Find the first instance of the item in the list and remove it
            for (int i = firstIndexOfKey; i <= lastIndexOfKey; i++)
            {
                if (item.Equals(this.innerList[i]))
                {
                    this.innerList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if the item identified by <paramref name="key"/> was successfully removed from the
        /// <see cref="ICollection{T}"/>; otherwise, false. This method also returns false if no
        /// item with a matching key is not found in the <see cref="ICollection{T}"/>.
        /// </returns>
        /// <param name="key">The key of the object to remove from the <see cref="ICollection{T}"/>.</param>
        public bool Remove(TKey key)
        {
            int index = this.IndexOf(key);
            if (index < 0)
                return false;

            this.innerList.RemoveAt(index);
            return true;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get { return this.innerList.Count; }
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

        #region Implementation of IList<T>

        /// <summary>
        /// Determines the index of a specific item in the <see cref="IList{T}"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">
        /// The object to locate in the <see cref="IList{T}"/>.
        /// </param>
        int IList<T>.IndexOf(T item)
        {
            TKey key = this.getKeyFunc(item);
            return this.IndexOf(key, 0, this.innerList.Count);
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the <see cref="IList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="IList{T}"/>.</exception>
        public void RemoveAt(int index)
        {
            this.innerList.RemoveAt(index);
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="IList{T}"/>.</exception>
        public T this[int index]
        {
            get { return this.innerList[index]; }
        }

        T IList<T>.this[int index]
        {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region Emulation of List<T>

        public void AddRange(IEnumerable<T> enumerable)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(enumerable);

            foreach (T item in enumerable)
            {
                this.Add(item);
            }
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return this.innerList.AsReadOnly();
        }

        public int BinarySearch(TKey key)
        {
            return this.BinarySearch(0, this.innerList.Count, key);
        }

        public int BinarySearch(int index, TKey key)
        {
            int count = this.innerList.Count - index;
            return this.BinarySearch(index, count, key);
        }

        public int BinarySearch(int index, int count, TKey searchKey)
        {
            //Validation
            ArgumentOutOfRangeException.ThrowIfLessThan(index, 0);
            ArgumentOutOfRangeException.ThrowIfLessThan(count, 0);

            if ((this.innerList.Count - index) < count)
                throw new ArgumentException(ResourceProvider.ArgInvalidOffsetLength(nameof(index), nameof(count)));
            
            ArgumentNullException.ThrowIfNull(searchKey);


            int low = index;
            int high = (index + count) - 1;

            while(low <= high)
            {
                int mid = (low + ((high - low) >> 1));

                T midItem = this.innerList[mid];
                TKey midKey = this.getKeyFunc(midItem);

                int result = this.comparer.Compare(midKey, searchKey);

                if (result == 0)
                    return mid;
                else if (result < 0)
                    low = mid + 1;
                else // (result > 0)
                    high = mid - 1;
            }

            return ~low;
        }


        public int Capacity
        {
            get { return this.innerList.Capacity; }
            set { this.innerList.Capacity = value; }
        }

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return this.innerList.ConvertAll(converter);
        }

        public void CopyTo(T[] array)
        {
            this.innerList.CopyTo(array);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            this.innerList.CopyTo(index, array, arrayIndex, count);
        }

        public bool Exists(Predicate<T> match)
        {
            return this.innerList.Exists(match);
        }

        public T? Find(Predicate<T> match)
        {
            return this.innerList.Find(match);
        }

        public List<T> FindAll(Predicate<T> match)
        {
            return this.innerList.FindAll(match);
        }

        public int FindIndex(Predicate<T> match)
        {
            return this.innerList.FindIndex(match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return this.innerList.FindIndex(startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return this.innerList.FindIndex(startIndex, count, match);
        }

        public T? FindLast(Predicate<T> match)
        {
            return this.innerList.FindLast(match);
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return this.innerList.FindLastIndex(match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return this.innerList.FindLastIndex(startIndex, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return this.innerList.FindLastIndex(startIndex, count, match);
        }

        public void ForEach(Action<T> action)
        {
            this.innerList.ForEach(action);
        }

        public List<T> GetRange(int index, int count)
        {
            return this.innerList.GetRange(index, count);
        }

        public int IndexOf(TKey key)
        {
            return this.IndexOf(key, 0, this.innerList.Count);
        }

        public int IndexOf(TKey key, int index)
        {
            int count = this.innerList.Count - index;
            return this.IndexOf(key, index, count);
        }

        /// <returns>
        /// If >= 0 the return value contains the index of the first item with the given key;
        /// if negative the return value is the bit-wise compliment of the point in the list
        /// where the value would be inserted.
        /// </returns>
        /// <see cref="BinarySearch(TKey)"/>
        public int IndexOf(TKey key, int index, int count)
        {
            int indexOf = this.BinarySearch(index, count, key);
            if (indexOf < 0)
                return indexOf;

            if (this.IsUnique)
                return indexOf;

            //List not necessarily unique. Find first matching item using linear search
            int nextIndexOf = indexOf - 1;
            while ((nextIndexOf >= index) && (this.comparer.Compare(key, this.getKeyFunc(this[nextIndexOf])) == 0))
            {
                indexOf = nextIndexOf;
                nextIndexOf--;
            }

            return indexOf;
        }

        public int LastIndexOf(TKey key)
        {
            return this.LastIndexOf(key, 0, this.innerList.Count);
        }

        public int LastIndexOf(TKey key, int index)
        {
            int count = this.innerList.Count - index;
            return this.LastIndexOf(key, index, count);
        }

        public int LastIndexOf(TKey key, int index, int count)
        {
            int indexOf = this.BinarySearch(index, count, key);
            if (indexOf < 0)
                return indexOf;

            if (this.IsUnique)
                return indexOf;

            //List not necessarily unique. Find last matching item using linear search
            int nextIndexOf = indexOf + 1;
            int maxIndex = index + count - 1;
            while ((nextIndexOf <= maxIndex) && (this.comparer.Compare(key, this.getKeyFunc(this[nextIndexOf])) == 0))
            {
                indexOf = nextIndexOf;
                nextIndexOf++;
            }

            return indexOf;
        }

        public int RemoveAll(Predicate<T> match)
        {
            return this.innerList.RemoveAll(match);
        }

        public void RemoveRange(int index, int count)
        {
            this.innerList.RemoveRange(index, count);
        }

        public T[] ToArray()
        {
            return this.innerList.ToArray();
        }

        public void TrimExcess()
        {
            this.innerList.TrimExcess();
        }

        public bool TrueForAll(Predicate<T> match)
        {
            return this.innerList.TrueForAll(match);
        }
        #endregion

        #region Public Methods

        public TKey GetKey(T item)
        {
            return this.getKeyFunc(item);
        }

        public bool IsUnique
        {
            get
            {
                return
                    (this.duplicatesPolicy == DuplicatesPolicy.Error) ||
                    (this.duplicatesPolicy == DuplicatesPolicy.Ignore) ||
                    (this.duplicatesPolicy == DuplicatesPolicy.Replace);
            }
        }

        public DuplicatesPolicy DuplicatesPolicy
        {
            get { return this.duplicatesPolicy; }
        }

        public IComparer<TKey> Comparer
        {
            get { return this.comparer; }
        }
        #endregion
    }
}
