using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections.Transformations
{
    /// <summary>
    /// Represents a strongly typed list of objects which projects a modified view of a
    /// source list.
    /// </summary>
    /// <typeparam name="T">The type of objects to in the list.</typeparam>
    public abstract class TransformedList<T> : TransformedCollection<T>, IReadOnlyList<T>, IList, IList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransformedList{T}"/> class with its
        /// source.
        /// </summary>
        /// <param name="source">
        /// The underlying source, for which this instance presents a view of the contained data.
        /// </param>
        public TransformedList(
            IEnumerable<T> source)
            : base(source)
        {
        }

        #region Implementaiton of IReadOnlyList<T>

        /// <summary>
        /// Gets the element at the specified index in the read-only list.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the read-only list.</returns>
        public abstract T this[int index] { get; }

        #endregion

        #region Implementaiton of IList

        /// <inheritdoc/>
        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => ((ICollection<T>)this).IsReadOnly;

        /// <inheritdoc/>
        int IList.Add(object value)
        {
            if (value is T item)
            {
                this.Add(item);
                return this.IndexOf(item);
            }

            throw new ArgumentException("Invalid type", nameof(value));
        }

        /// <inheritdoc/>
        bool IList.Contains(object value)
        {
            if (value is T item)
            {
                return this.Contains(item);
            }

            return false;
        }

        /// <inheritdoc/>
        int IList.IndexOf(object value)
        {
            if (value is T item)
            {
                return this.IndexOf(item);
            }

            throw new ArgumentException("Invalid type", nameof(value));
        }

        /// <inheritdoc/>
        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        void IList.Remove(object value)
        {
            if (value is T item)
            {
                this.Remove(item);
                return;
            }

            throw new ArgumentException("Invalid type", nameof(value));
        }

        /// <inheritdoc/>
        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        #endregion

        #region Implementation of IList<T>

        /// <inheritdoc/>
        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        public abstract int IndexOf(T item);

        /// <exception cref="NotSupportedException">Thrown always.</exception>
        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes the <see cref="TransformedList{T}"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            var item = this[index];
            this.Remove(item);
        }

        #endregion
    }
}
