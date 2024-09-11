using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections
{
    /// <summary>
    /// A <see cref="ICollection{T}"/> containing weak references to items.
    /// </summary>
    /// <typeparam name="T">The type of the objects to be contained.</typeparam>
    /// <remarks>
    /// null reference may not be stored in the collection.
    /// </remarks>
    public class WeakCollection<T> : ICollection<T>
        where T : class
    {
        private class Enumerator : IEnumerator<T>
        {
            #region Fields

            private readonly WeakCollection<T> weakCollection;
            private T? current;
            private int index;

            #endregion

            #region Constructors

            public Enumerator(WeakCollection<T> weakCollection)
            {
                ArgumentNullException.ThrowIfNull(weakCollection);


                this.weakCollection = weakCollection;
                this.Reset();
            }
            #endregion

            #region Implementation of IDisposable

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or
            /// resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.Reset();
            }
            #endregion

            #region Implementation of IEnumerator<T>

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <returns>
            /// The element in the collection at the current position of the enumerator.
            /// </returns>
            public T Current
            {
                get
                {
                    if (this.current is null)
                        throw new InvalidOperationException(
                            "Current item is before the start or after the end of the enumeration.");

                    return this.current;
                }
            }

            #endregion

            #region Implementation of IEnumerator

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element;
            /// false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                this.index++;

                while (true)
                {
                    if (this.index >= this.weakCollection.innerList.Count)
                    {
                        this.current = null;
                        this.index = -1;
                        return false;
                    }

                    this.weakCollection.innerList[this.index].TryGetTarget(out var target);
                    this.current = target;
                    if (this.current is not null)
                    {
                        return true;
                    }
                    else
                    {
                        this.weakCollection.innerList.RemoveAt(this.index);
                    }
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first
            /// element in the collection.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                this.current = null;
                this.index = -1;
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="InvalidOperationException">
            /// The enumerator is positioned before the first element of the collection or after the last element.
            /// </exception>
            object IEnumerator.Current
            {
                get { return this.Current; }
            }
            #endregion
        }

        #region Fields

        private readonly List<WeakReference<T>> innerList;
        #endregion

        #region Constructors

        public WeakCollection()
        {
            this.innerList = new();
        }

        public WeakCollection(IEnumerable<T> collection)
            : this()
        {
            //Validation
            ArgumentNullException.ThrowIfNull(collection);

            foreach (T item in collection)
            {
                this.innerList.Add(new WeakReference<T>(item));
            }
        }

        public WeakCollection(int capacity)
        {
            this.innerList = new(capacity);
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
            return new Enumerator(this);
        }
        #endregion

        #region Implementation of ICollection<T>

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <seealso cref="Add(T, bool)"/>
        public void Add(T item)
        {
            this.Add(item, true);
        }

        /// <summary>
        /// Adds an item to the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ICollection{T}"/>.</param>
        /// <param name="minimiseMemoryUsage">true to minimise the memory usage; else false.
        /// true reduces memory usage, but decreases write performance.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// The <see cref="ICollection{T}"/> is read-only.
        /// </exception>
        /// <remarks>
        /// If writing a large number of items first call the <see cref="TrimExcess"/> method,
        /// and then add each item with <paramref name="minimiseMemoryUsage"/> set to false.
        /// </remarks>
        public void Add(T item, bool minimiseMemoryUsage)
        {
            ArgumentNullException.ThrowIfNull(item);

            WeakReference<T> weakReference = new(item);

            if (minimiseMemoryUsage)
            {
                //Try to reuse memory if it is available
                for (int i = 0; i < this.innerList.Count; i++)
                {
                    if (!this.innerList[i].TryGetTarget(out _))
                    {
                        this.innerList[i] = weakReference;  
                        return;
                    }
                }
            }

            this.innerList.Add(weakReference);
        }

        /// <summary>
        /// Removes all items from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// The <see cref="ICollection{T}"/> is read-only.
        /// </exception>
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
        /// <remarks>
        /// Will return false if the item was stored in the collection but has been garbage collected.
        /// </remarks>
        public bool Contains(T item)
        {
            ArgumentNullException.ThrowIfNull(item);


            foreach (var weakReference in this.innerList)
            {
                weakReference.TryGetTarget(out var target);
                if (item.Equals(target))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements
        /// copied from <see cref="ICollection{T}"/>. The <see cref="Array"/> must have
        /// zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="array"/> is multidimensional.
        ///                     -or-
        ///     <paramref name="arrayIndex"/> is equal to or greater than the length
        ///     of <paramref name="array"/>.
        ///                     -or-
        ///     The number of elements in the source <see cref="ICollection{T}"/> is greater than
        ///     the available space from <paramref name="arrayIndex"/> to the end of the
        ///     destination <paramref name="array"/>.
        ///                     -or-
        ///     Type <typeparamref name="T"/> cannot be cast automatically to the type of the
        ///     destination <paramref name="array"/>.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var collection = this.ToCollection();
            
            collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="ICollection{T}"/>;
        /// otherwise, false.
        /// This method also returns false if <paramref name="item"/> is not found in the
        /// original <see cref="ICollection{T}"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="ICollection{T}"/>.</param>
        /// <exception cref="NotSupportedException">The <see cref="ICollection{T}"/> is read-only.</exception>
        public bool Remove(T item)
        {
            ArgumentNullException.ThrowIfNull(item);


            for (int i = 0; i < this.innerList.Count; i++)
            {
                this.innerList[i].TryGetTarget(out var target);
                if (item.Equals(target))
                {
                    this.innerList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the number of alive elements contained in the <see cref="ICollection{T}"/>.
        /// </summary>
        /// <returns>
        /// The number of alive elements contained in the <see cref="ICollection{T}"/>.
        /// </returns>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (var weakReference in this.innerList)
                {
                    if (weakReference.TryGetTarget(out _))
                        count++;
                }

                return count;
            }
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

        #region Properties

        /// <summary>
        /// Returns a high estimate of the number of alive items contained in the collection.
        /// </summary>
        public int EstimateCount
        {
            get { return this.innerList.Count; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a shallow-copy of the collection. All references are held as strong-references,
        /// preventing garbage-collection.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection{T}"/> containing strong-references to the alive items in this
        /// <see cref="WeakCollection{T}"/>.
        /// </returns>
        public ICollection<T> ToCollection()
        {
            List<T> list = new(this.EstimateCount);
            foreach (var weakReference in this.innerList)
            {
                if (weakReference.TryGetTarget(out var target))
                    list.Add(target);
            }

            return list;
        }

        /// <summary>
        /// Sets the capacity to the actual number of alive elements contained in this collection.
        /// </summary>
        public void TrimExcess()
        {
            for (int i = this.innerList.Count - 1; i >= 0; i--)
            {
                if (!this.innerList[i].TryGetTarget(out _))
                {
                    this.innerList.RemoveAt(i);
                }
            }

            this.innerList.TrimExcess();
        }
        #endregion
    }
}
