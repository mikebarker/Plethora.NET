using System;
using System.Collections.Generic;

namespace Plethora.Collections
{
    /// <summary>
    /// A collection which is unique per item.
    /// </summary>
    /// <typeparam name="T">The type of the collection.</typeparam>
    /// <remarks>
    /// This class should only be used if the data type required must
    /// inherit from the <see cref="KeyedCollection{TKey, T}"/> class or
    /// implement the <see cref="IKeyedCollection{TKey,T}"/> interface.
    /// Otherwise, consider using the standard .NET <see cref="HashSet{T}"/> class.
    /// </remarks>
    /// <seealso cref="KeyedCollection{TKey,T}"/>
    /// <seealso cref="HashSet{T}"/>
    public class AutoKeyedCollection<T> : KeyedCollection<T, T>
        where T : notnull
    {
        #region Fields

        private readonly static Func<T, T> autoKeySelector = t => t;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="AutoKeyedCollection{T}"/> class.
        /// </summary>
        public AutoKeyedCollection()
            : base(autoKeySelector)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AutoKeyedCollection{T}"/> class.
        /// </summary>
        public AutoKeyedCollection(IEqualityComparer<T> comparer)
            : base(autoKeySelector, comparer)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AutoKeyedCollection{T}"/> class.
        /// </summary>
        public AutoKeyedCollection(IEnumerable<T> enumerable)
            : base(autoKeySelector, enumerable)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AutoKeyedCollection{T}"/> class.
        /// </summary>
        public AutoKeyedCollection(IEnumerable<T> enumerable, IEqualityComparer<T> comparer)
            : base(autoKeySelector, enumerable, comparer)
        {
        }
        #endregion
    }

    public static class AutoKeyedCollectionHelper
    {
        public static AutoKeyedCollection<T> ToKeyedCollection<T>(this IEnumerable<T> enumerable)
            where T : notnull
        {
            return new AutoKeyedCollection<T>(enumerable);
        }
    }
}
