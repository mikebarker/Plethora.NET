using System.Collections.Generic;

namespace Plethora.Collections.Trees
{
    /// <summary>
    /// Interface defining an <see cref="IEnumerator{T}"/> which is limited in the
    /// keys which are returned during enumeration.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of the underlying data store.</typeparam>
    /// <typeparam name="TValue">The type of the values of the underlying data store.</typeparam>
    public interface IKeyLimitedEnumerator<TKey, TValue> : IEnumerator<TValue>
    {
        /// <summary>
        /// Lower limit to be placed on the key of the enumerator.
        /// </summary>
        TKey Min { get; set; }

        /// <summary>
        /// Upper limit to be placed on the key of the enumerator.
        /// </summary>
        TKey Max { get; set; }
    }
}
