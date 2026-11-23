using System;
using System.Collections;
using System.Collections.Generic;

namespace Plethora.Collections;

public sealed class SubList<T> : IEnumerable<T>, IReadOnlyCollection<T>, ICollection<T>, IReadOnlyList<T>, IList<T>
{
    public struct Enumerator : IEnumerator<T>
    {
        #region Fields

        private readonly SubList<T> subList;
        private int currentIndex;
        private T? current;

        #endregion

        #region Constructors

        public Enumerator(SubList<T> subList)
        {
            ArgumentNullException.ThrowIfNull(subList);


            this.subList = subList;
        }
        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.Reset();
        }

        #endregion

        #region Implementation of IEnumerator

        readonly object? IEnumerator.Current => this.Current;

        public bool MoveNext()
        {
            if ((this.currentIndex < this.subList.Count) &&
                (this.currentIndex != -1))
            {
                this.current = this.subList[this.currentIndex];
                this.currentIndex++;
                return true;
            }

            this.current = default;
            this.currentIndex = -1;
            return false;
        }

        public void Reset()
        {
            this.currentIndex = 0;
            this.current = default;
        }

        #endregion

        #region Implementation of IEnumerator<T>

        public readonly T Current
        {
            get
            {
                if (this.currentIndex <= 0)
                    throw new InvalidOperationException(ResourceProvider.EnumeratorOpCantHappen());

                return this.current!;
            }
        }

        #endregion
    }

    #region Fields

    private readonly IList<T> list;
    private readonly int startIndex;
    private readonly int count;

    #endregion

    #region Constructors

    public SubList(IList<T> list, int startIndex, int count)
    {
        ArgumentNullException.ThrowIfNull(list);

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
        this.count = count;
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
        return new Enumerator(this);
    }

    #endregion

    #region Implementation of IReadOnlyCollection<T>

    public int Count
    {
        get { return this.count; }
    }

    #endregion

    #region Implementation of ICollection<T>

    void ICollection<T>.Add(T item)
    {
        throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    void ICollection<T>.Clear()
    {
        throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    public bool Contains(T item)
    {
        for (int i = 0; i < this.count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(this.list[this.startIndex + i], item))
                return true;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        if (this.list is T[] sourceArray)
        {
            Array.Copy(sourceArray,
                this.startIndex,
                array,
                arrayIndex,
                this.count);
        }
        else if (this.list is List<T> l)
        {
            l.CopyTo(
                this.startIndex,
                array,
                arrayIndex,
                this.count);
        }
        else
        {
            for (int i = 0; i < this.count; i++)
            {
                array[arrayIndex++] = this.list[this.startIndex + i];
            }
        }
    }

    bool ICollection<T>.IsReadOnly
    {
        get { return true; }
    }

    bool ICollection<T>.Remove(T item)
    {
        throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    #endregion

    #region Implemenation of IReadOnlyList<T>

    public T this[int index]
    {
        get
        {
            if ((index < 0) || (index >= this.count))
                throw new ArgumentOutOfRangeException(nameof(index),
                    ResourceProvider.ArgMustBeBetween(nameof(index), "0", "Count - 1"));

            return this.list[this.startIndex + index];
        }
    }

    #endregion

    #region Implemenation of IList<T>

    T IList<T>.this[int index]
    {
        get => this[index];
        set => throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    public int IndexOf(T item)
    {
        for (int i = 0; i < this.count; i++)
        {
            if (EqualityComparer<T>.Default.Equals(this.list[this.startIndex + i], item))
                return i;
        }

        return -1;
    }

    void IList<T>.Insert(int index, T item)
    {
        throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    void IList<T>.RemoveAt(int index)
    {
        throw new InvalidOperationException(ResourceProvider.CollectionReadOnly());
    }

    #endregion
}