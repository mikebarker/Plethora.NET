using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Plethora.Linq
{
    public static partial class EnumerableHelper
    {
        /// <summary>
        /// Provides the functionality for the <see cref="EnumerableHelper.CacheResult{T}"/>
        /// extension method.
        /// </summary>
        private sealed class CacheEnumerable<T> : IEnumerable<T>, IDisposable
        {
            private sealed class CacheEnumerator : IEnumerator<T>
            {
                #region Fields

                private readonly CacheEnumerable<T> owner;

                private int index;
                private bool hasCurrent;
                private T? current;
                #endregion

                #region Constructors

                /// <summary>
                /// Initialise a new instance of the <see cref="CacheEnumerator"/> class.
                /// </summary>
                public CacheEnumerator(CacheEnumerable<T> owner)
                {
                    this.owner = owner;

                    this.Reset();
                }
                #endregion

                #region Implementation of IDisposable

                /// <summary>
                /// Performs application-defined tasks associated with freeing,
                /// releasing, or resetting unmanaged resources.
                /// </summary>
                public void Dispose()
                {
                    this.Reset();
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
                public bool MoveNext()
                {
                    Debug.Assert(this.owner.cachedElements is not null);

                    if (this.index < (this.owner.cachedElements.Count - 1))
                    {
                        this.current = this.owner.cachedElements[++this.index];
                        this.hasCurrent = true;
                        return true;
                    }

                    if (this.owner.sourceEnumerator is null)
                    {
                        this.current = default;
                        this.hasCurrent = false;
                        return false;
                    }

                    if (!this.owner.sourceEnumerator.MoveNext())
                    {
                        //Enumeration finish, release source references
                        this.owner.source = null;
                        this.owner.sourceEnumerator.Dispose();
                        this.owner.sourceEnumerator = null;

                        this.current = default;
                        this.hasCurrent = false;
                        return false;
                    }

                    ++this.index;
                    this.current = this.owner.sourceEnumerator.Current;
                    this.owner.cachedElements.Add(this.current);
                    this.hasCurrent = true;
                    return true;
                }

                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first
                /// element in the collection.
                /// </summary>
                public void Reset()
                {
                    this.index = -1;
                    this.current = default;
                    this.hasCurrent = false;
                }

                /// <summary>
                /// Gets the current element in the collection.
                /// </summary>
                /// <returns>
                /// The current element in the collection.
                /// </returns>
                object? IEnumerator.Current
                {
                    get { return this.Current; }
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
                        if (!this.hasCurrent)
                            throw new InvalidOperationException("Enumeration has not started or already completed.");

                        return this.current!;
                    }
                }
                #endregion
            }

            #region Fields

            private IEnumerable<T>? source;

            private IEnumerator<T>? sourceEnumerator;
            private List<T>? cachedElements;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialise a new instance of the <see cref="CacheEnumerable{T}"/> class.
            /// </summary>
            public CacheEnumerable(IEnumerable<T> source)
            {
                //Validation
                ArgumentNullException.ThrowIfNull(source);

                this.source = source;
            }
            #endregion

            #region Implementation of IEnumerable

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            #endregion

            #region Implementation of IEnumerable<T>

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            public IEnumerator<T> GetEnumerator()
            {
                //Performance short-cut for fully cached results
                if (this.source is null)
                    return this.cachedElements!.GetEnumerator();

                //Performance short-cut for CacheEnumerable types
                if (this.source is CacheEnumerable<T>)
                    return this.source.GetEnumerator();

                //Performance short-cut for already concrete types
                if (this.source is ICollection<T>)
                    return this.source.GetEnumerator();


                if (this.cachedElements is null)
                {
                    this.sourceEnumerator = this.source.GetEnumerator();
                    this.cachedElements = new();
                }

                return new CacheEnumerator(this);
            }
            #endregion

            #region Implementation of IDisposable

            private bool disposed = false;

            ~CacheEnumerable()
            {
                this.Dispose(false);
            }

            /// <summary>
            /// Release managed and unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Release managed and unmanaged resources.
            /// </summary>
            /// <param name="disposing">
            /// If true releases managed and unmanaged resources; otherwise release
            /// only unmanaged resources.
            /// </param>
            private void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called.
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        // Dispose managed resources.
                        this.sourceEnumerator?.Dispose();
                    }

                    // Release unmanaged resources.


                    // Disposing has been done.
                    this.disposed = true;
                }
            }
            #endregion
        }
    }
}
