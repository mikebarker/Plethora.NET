using System;
using System.Collections.Generic;

namespace Plethora.Collections
{
    /// <summary>
    /// An instance of <see cref="IComparer{T}"/> which reverses the sort-direction of a comparer.
    /// </summary>
    public class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> comparer;

        public ReverseComparer(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            this.comparer = comparer;
        }

        public int Compare(T? x, T? y)
        {
            //Change the comparison order
            int result = this.comparer.Compare(y, x);

            return result;
        }
    }

    public static class ComparerHelper
    {
        /// <summary>
        /// Creates an instance of <see cref="IComparer{T}"/> which reverses the sort direction
        /// of the comparer for which this is called.
        /// </summary>
        /// <param name="comparer">The comparer for which the sort direction is to be reversed.</param>
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            return new ReverseComparer<T>(comparer);
        }
    }
}
