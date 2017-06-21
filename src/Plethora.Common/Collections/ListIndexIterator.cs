using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections
{
    public class ListIndexIterator<T> : IEnumerator<T>, IEnumerable<T>, ICollection<T>
    {
        #region Fields

        private readonly IList<T> list;
        private readonly int startIndex;
        private readonly int endIndex;
        private int currentIndex;
        private T current;
            
        #endregion

        #region Constructors

        public ListIndexIterator(IList<T> list, int startIndex, int count)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), startIndex,
                    ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(startIndex)));

            if (startIndex >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex),
                    ResourceProvider.ArgMustBeBetween(nameof(startIndex), "0", "list.Count"));

            if ((count < 0) || ((startIndex + count) > list.Count))
                throw new ArgumentOutOfRangeException(nameof(count),
                    ResourceProvider.ArgMustBeBetween(nameof(count), "0", "list.Count - startIndex"));


            this.list = list;
            this.startIndex = startIndex;
            this.endIndex = startIndex + count - 1;

            this.Reset();
        }
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.Reset();
        }

        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        #endregion

        #region Implementation of IEnumerator

        public bool MoveNext()
        {
            if (this.currentIndex == -1)
                this.currentIndex = this.startIndex;
            else
                this.currentIndex++;

            if ((this.currentIndex > this.endIndex) || (this.currentIndex > this.list.Count))
            {
                this.current = default(T);
                return false;
            }

            this.current = this.list[this.currentIndex];
            return true;
        }

        public void Reset()
        {
            this.currentIndex = -1;
            this.current = default(T);
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        #endregion

        #region Implementation of IEnumerator<T>

        public T Current
        {
            get { return this.current; }
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
            for (int i = this.startIndex; i <= this.endIndex; i++)
            {
                if (EqualityComparer<T>.Default.Equals(this.list[i], item))
                    return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            List<T> l = this.list as List<T>;
            if (l != null)
            {
                l.CopyTo(
                    this.startIndex,
                    array,
                    arrayIndex,
                    this.endIndex - this.startIndex + 1);
            }
            else
            {
                for (int i = this.startIndex; i <= this.endIndex; i++)
                {
                    array[arrayIndex++] = this.list[i];
                }
            }
        }

        public int Count
        {
            get { return this.endIndex - this.startIndex + 1; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException(ResourceProvider.CollectionReadonly());
        }

        #endregion
    }
}
