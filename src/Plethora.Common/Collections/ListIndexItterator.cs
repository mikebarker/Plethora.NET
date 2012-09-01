using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections
{
    public class ListIndexItterator<T> : IEnumerator<T>, IEnumerable<T>
    {
        #region Fields

        private readonly IList<T> list;
        private readonly int startIndex;
        private readonly int endIndex;
        private int currentIndex;
        private T current;
            
        #endregion

        #region Constructors

        public ListIndexItterator(IList<T> list, int startIndex, int count)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", startIndex,
                    ResourceProvider.ArgMustBeGreaterThanEqualToZero("startIndex"));

            if ((count < 0) || ((startIndex + count) > list.Count))
                throw new ArgumentOutOfRangeException("count",
                    ResourceProvider.ArgMustBeBetween("count", "0", "list.Length - startIndex"));


            this.list = list;
            this.startIndex = startIndex;
            this.endIndex = startIndex + count - 1;

            Reset();
        }
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Reset();
        }

        #endregion

        #region Implementation of IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            if (currentIndex == -1)
                currentIndex = startIndex;
            else
                currentIndex++;

            if ((currentIndex > endIndex) || (currentIndex > list.Count))
            {
                current = default(T);
                return false;
            }

            current = list[currentIndex];
            return true;
        }

        public void Reset()
        {
            currentIndex = -1;
            current = default(T);
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
    }
}
