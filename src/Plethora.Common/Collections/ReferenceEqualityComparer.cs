using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;

namespace Plethora.Collections
{
    /// <summary>
    /// Implementation of <see cref="IEqualityComparer{T}"/> which compares objects for reference equality.
    /// </summary>
    /// <typeparam name="T">
    /// The type of objects to compare.
    /// </typeparam>
    public sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer
    {
        private static ReferenceEqualityComparer<T> defaultComparer;

        /// <summary>
        /// Returns a default equality comparer for the type specified by the generic argument.
        /// </summary>
        /// <returns>
        /// The default instance of the <see cref="ReferenceEqualityComparer{T}"/> class for type <typeparamref name="T"/>.
        /// </returns>
        [NotNull]
        public static ReferenceEqualityComparer<T> Default
        {
            get
            {
                if (defaultComparer == null)
                {
                    Interlocked.CompareExchange(ref defaultComparer, new ReferenceEqualityComparer<T>(), null);
                }
                return defaultComparer;
            }
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
        /// <returns>true if the specified objects are equal; otherwise, false.</returns>
        [Pure]
        public bool Equals([CanBeNull] T x, [CanBeNull] T y)
        {
            return object.ReferenceEquals(x, y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
        [Pure]
        public int GetHashCode([NotNull] T obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }

        [Pure]
        bool IEqualityComparer.Equals([CanBeNull] object x, [CanBeNull] object y)
        {
            if ((x != null) && (!(x is T)))
                throw new ArgumentException(ResourceProvider.InvalidCast());

            if ((y != null) && (!(y is T)))
                throw new ArgumentException(ResourceProvider.InvalidCast());

            return this.Equals((T)x, (T)y);
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> for which a hash code is to be returned.</param>
        /// <returns>A hash code for the specified object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
        [Pure]
        int IEqualityComparer.GetHashCode([NotNull] object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (!(obj is T))
                throw new ArgumentException(ResourceProvider.InvalidCast());

            return this.GetHashCode((T)obj);
        }
    }
}
