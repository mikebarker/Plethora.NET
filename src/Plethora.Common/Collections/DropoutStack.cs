using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections
{
    /// <summary>
    /// Represents a variable size last-in-first-out (LIFO) collection of instances of the same specified type, where
    /// items will drop out of the stack when the maximum size is reached.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    public class DropoutStack<T> : IEnumerable<T>, IReadOnlyCollection<T>
    {
        private class Enumerator : IEnumerator<T>
        {
            private const int InitialIndex = -1;

            private readonly DropoutStack<T> stack;
            private int index;

            public Enumerator(DropoutStack<T> stack)
            {
                this.index = InitialIndex;
                this.stack = stack;
            }

            public T Current => this.stack.items[this.index];

            object IEnumerator.Current => this.Current;

            void IDisposable.Dispose()
            {
                this.Reset();
            }

            public bool MoveNext()
            {
                if (this.index == stack.LastIndex)
                    return false;

                if (this.index == InitialIndex)
                {
                    if (this.stack.Count == 0)
                        return false;

                    this.index = stack.firstIndex;
                    return true;
                }

                this.index--;
                if (this.index < 0)
                    this.index += stack.Capacity;

                return true;
            }

            public void Reset()
            {
                this.index = InitialIndex;
            }
        }

        private readonly T[] items;
        private int firstIndex;
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropoutStack{T}"/> class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The maximum number of elements that the <see cref="DropoutStack{T}"/> can contain.</param>
        public DropoutStack(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, ResourceProvider.ArgMustBeGreaterThanZero(nameof(capacity)));

            this.items = new T[capacity];
            this.firstIndex = -1;
            this.count = 0;
        }

        /// <summary>
        /// The maximum number of elements that the <see cref="DropoutStack{T}"/> can contain.
        /// </summary>
        public int Capacity => this.items.Length;

        /// <summary>
        /// The number of elements that the <see cref="DropoutStack{T}"/> contains.
        /// </summary>
        public int Count => this.count;

        #region Implementation of IEnumerable<T>

        /// <summary>
        /// Returns an enumerator for the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <returns>
        /// An enumerator for the <see cref="DropoutStack{T}"/>.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Removes all objects from the <see cref="DropoutStack{T}"/>.
        /// </summary>
        public void Clear()
        {
            if (this.Count > 0)
            {
                int index = this.firstIndex;
                do
                {
                    this.items[index--] = default;
                    if (index < 0)
                        index += this.Capacity;
                }
                while (index != this.LastIndex);
            }

            this.firstIndex = -1;
            this.count = 0;
        }

        /// <summary>
        /// Inserts an object at the top of the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <param name="item">The object to push onto the <see cref="DropoutStack{T}"/>. The value can be null for reference types.</param>
        public void Push(T item)
        {
            this.firstIndex = (this.firstIndex + 1) % this.Capacity;
            this.items[this.firstIndex] = item;
            if (this.count < Capacity)
                this.count++;
        }

        /// <summary>
        /// Removes and returns the object at the top of the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <returns>
        /// The object removed from the top of the <see cref="DropoutStack{T}"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The <see cref="DropoutStack{T}"/> is empty.</exception>
        public T Pop()
        {
            if (!TryPop(out var result))
                throw new InvalidOperationException();

            return result;
        }

        /// <summary>
        /// Returns the object at the top of the <see cref="DropoutStack{T}"/> without removing it.
        /// </summary>
        /// <returns>
        /// The object at the top of the <see cref="DropoutStack{T}"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">The <see cref="DropoutStack{T}"/> is empty.</exception>
        public T Peek()
        {
            if (!TryPeek(out var result))
                throw new InvalidOperationException();

            return result;
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the <see cref="DropoutStack{T}"/>, and if
        /// one is present, copies it to the result parameter, and removes it from the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <param name="result">
        /// If present, the object at the top of the <see cref="DropoutStack{T}"/>; otherwise, the default value of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// true if there is an object at the top of the <see cref="DropoutStack{T}"/>; false if the <see cref="DropoutStack{T}"/> is empty.
        /// </returns>
        public bool TryPop(out T result)
        {
            if (this.count == 0)
            {
                result = default;
                return false;
            }

            result = this.items[this.firstIndex];
            this.items[this.firstIndex] = default; // Release the item if a reference.

            this.count--;
            this.firstIndex--;
            if (this.firstIndex < 0)
                this.firstIndex += this.Capacity;

            return true;
        }

        /// <summary>
        /// Returns a value that indicates whether there is an object at the top of the <see cref="DropoutStack{T}"/>, and if
        /// one is present, copies it to the result parameter. The object is not removed from the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <param name="result">
        /// If present, the object at the top of the <see cref="DropoutStack{T}"/>; otherwise, the default value of <typeparamref name="T"/>.
        /// </param>
        /// <returns>
        /// true if there is an object at the top of the <see cref="DropoutStack{T}"/>; false if the <see cref="DropoutStack{T}"/> is empty.
        /// </returns>
        public bool TryPeek(out T result)
        {
            if (this.count == 0)
            {
                result = default;
                return false;
            }

            result = this.items[this.firstIndex];
            return true;
        }

        /// <summary>
        /// Determines whether an element is in the <see cref="DropoutStack{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="DropoutStack{T}"/>. The value can be null for reference types.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="DropoutStack{T}"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            foreach (var value in this.items)
            {
                if (Equals(item, value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the <see cref="DropoutStack{T}"/> to an existing one-dimensional <see cref="Array"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="DropoutStack{T}"/>.
        /// The Array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            foreach (var item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        /// <summary>
        /// Copies the <see cref="DropoutStack{T}"/> to a new array.
        /// </summary>
        /// <returns>
        /// A new array containing copies of the elements of the <see cref="DropoutStack{T}"/>.
        /// </returns>
        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            CopyTo(array, 0);
            return array;
        }

        private int LastIndex
        {
            get
            {
                var lastIndex = (this.firstIndex + 1) - this.Count;
                if (lastIndex < 0)
                    lastIndex += this.Capacity;

                return lastIndex;
            }
        }
    }
}
