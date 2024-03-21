using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Plethora.Collections
{
    /// <summary>
    /// A list in which the elements are sorted according to the comparer provided.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the elements in the list.
    /// </typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [Serializable]
    public class AutoSortedList<T> : IList<T>
    {
        #region Fields

        private readonly List<T> innerList;
        private readonly IComparer<T> comparer;
        private readonly DuplicatesPolicy duplicatesPolicy;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="AutoSortedList{T}"/>, with a duplicate policy 
        /// of <see cref="Plethora.Collections.DuplicatesPolicy.Error"/> and using the
        /// default comparer for <typeparamref name="T"/>.
        /// </summary>
        public AutoSortedList()
            : this(DuplicatesPolicy.Error, Comparer<T>.Default)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoSortedList{T}"/>, with a duplicate policy 
        /// of <see cref="Plethora.Collections.DuplicatesPolicy.Error"/>.
        /// </summary>
        /// <param name="comparer">The comparer used to sort the list.</param>
        public AutoSortedList(IComparer<T> comparer)
            : this(DuplicatesPolicy.Error, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoSortedList{T}"/>.
        /// </summary>
        /// <param name="duplicatesPolicy">The policy to be followed when adding duplicate elements to the list.</param>
        /// <param name="comparer">The comparer used to sort the list.</param>
        public AutoSortedList(DuplicatesPolicy duplicatesPolicy, IComparer<T> comparer)
        {
            //Validation
            if ((duplicatesPolicy != DuplicatesPolicy.Allow) &&
                (duplicatesPolicy != DuplicatesPolicy.Ignore) &&
                (duplicatesPolicy != DuplicatesPolicy.Replace) &&
                (duplicatesPolicy != DuplicatesPolicy.Error))
            {
                throw new ArgumentOutOfRangeException(nameof(duplicatesPolicy), duplicatesPolicy,
                    ResourceProvider.ArgMustBeOneOf(nameof(duplicatesPolicy), "Allow", "Ignore", "Replace", "Error"));
            }

            ArgumentNullException.ThrowIfNull(comparer);


            this.duplicatesPolicy = duplicatesPolicy;
            this.comparer = comparer;
            this.innerList = new();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AutoSortedList{T}"/>.
        /// </summary>
        /// <param name="enumerable">The collection whose elements are to be copied to the sorted list.</param>
        /// <param name="duplicatesPolicy">The policy to be followed when adding duplicate elements to the list.</param>
        /// <param name="comparer">The comparer used to sort the list.</param>
        public AutoSortedList(IEnumerable<T> enumerable, DuplicatesPolicy duplicatesPolicy, IComparer<T> comparer)
            : this(duplicatesPolicy, comparer)
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
            if (item == null)
                throw new ArgumentNullException(nameof(item));


            int index = this.BinarySearch(item);

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
        public bool Contains(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            int index = this.BinarySearch(item);
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
            ArgumentNullException.ThrowIfNull(item);

            int index = this.BinarySearch(item);
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
        public int IndexOf(T item)
        {
            return this.IndexOf(item, 0, this.innerList.Count);
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

        public int BinarySearch(T item)
        {
            return this.BinarySearch(0, this.innerList.Count, item);
        }

        public int BinarySearch(int index, T item)
        {
            int count = this.innerList.Count - index;
            return this.BinarySearch(index, count, item);
        }

        public int BinarySearch(int index, int count, T item)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(item);


            int result = this.innerList.BinarySearch(index, count, item, this.comparer);
            return result;
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

        public int IndexOf(T item, int index)
        {
            int count = this.innerList.Count - index;
            return this.IndexOf(item, 0, count);
        }

        public int IndexOf(T item, int index, int count)
        {
            int indexOf = this.BinarySearch(index, count, item);
            if (indexOf < 0)
                return indexOf;

            if (this.IsUnique)
                return indexOf;

            //List not necessarily unique. Find first matching item using linear search
            int nextIndexOf = indexOf - 1;
            while ((nextIndexOf >= index) && (this.comparer.Compare(item, this[nextIndexOf]) == 0))
            {
                indexOf = nextIndexOf;
                nextIndexOf--;
            }

            return indexOf;
        }

        public int LastIndexOf(T item)
        {
            return this.LastIndexOf(item, 0, this.innerList.Count);
        }

        public int LastIndexOf(T item, int index)
        {
            int count = this.innerList.Count - index;
            return this.LastIndexOf(item, index, count);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            int indexOf = this.BinarySearch(index, count, item);
            if (indexOf < 0)
                return indexOf;

            if (this.IsUnique)
                return indexOf;

            //List not necessarily unique. Find last matching item using linear search
            int nextIndexOf = indexOf + 1;
            int maxIndex = index + count;
            while ((nextIndexOf <= maxIndex) && (this.comparer.Compare(item, this[nextIndexOf]) == 0))
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

        public IComparer<T> Comparer
        {
            get { return this.comparer; }
        }
        #endregion
    }
}
