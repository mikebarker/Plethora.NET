using System.Collections;
using System.Collections.Generic;

namespace Plethora.Linq
{
    public static partial class EnumerableHelper
    {
        /// <summary>
        /// Wrapper class for presenting an <see cref="IEnumerator{T}"/> as an
        /// <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects to enumerate.</typeparam>
        private sealed class EnumeratorWrapper<T> : IEnumerable<T>
        {
            #region Fields

            private readonly IEnumerator<T> enumerator;
            #endregion

            #region Constructors

            /// <summary>
            /// Initialises a new instance of the <see cref="EnumeratorWrapper{T}"/> class.
            /// </summary>
            /// <param name="enumerator">
            /// The enumerator to be wrapped.
            /// </param>
            internal EnumeratorWrapper(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }
            #endregion

            #region Implementation of IEnumerable

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<T> GetEnumerator()
            {
                return enumerator;
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }
    }
}
