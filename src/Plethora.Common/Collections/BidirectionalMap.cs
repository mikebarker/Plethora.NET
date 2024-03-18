using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plethora.Collections
{
    /// <summary>
    /// Represents a map of pairs with semantics for forward and reverse lookups.
    /// </summary>
    /// <typeparam name="T1">The type of the forward key of the pair in the map.</typeparam>
    /// <typeparam name="T2">The type of the reverse key of the pair in the map.</typeparam>
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    [Serializable]
    public class BidirectionalMap<T1, T2> : IEnumerable, IDictionary<T1, T2>, IReadOnlyDictionary<T1,T2>
        where T1 : notnull
        where T2 : notnull
    {
        private readonly Dictionary<T1, T2> forwardMap = new();
        private readonly Dictionary<T2, T1> reverseMap = new();

        #region Implementation of IEnumerable

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of IEnumerable<KeyValuePair<T1, T2>>

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            return this.forwardMap.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<T1, T2>>

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<T1, T2>>.IsReadOnly => false;

        /// <inheritdoc/>
        void ICollection<KeyValuePair<T1, T2>>.Add(KeyValuePair<T1, T2> pair)
        {
            T1 t1 = pair.Key;
            T2 t2 = pair.Value;

            this.Add(t1, t2);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<T1, T2>>.Contains(KeyValuePair<T1, T2> pair)
        {
            T1 t1 = pair.Key;
            T2 t2 = pair.Value;

            var result = this.ValidateExistingItems(t1, t2);
            return (result == ValidateExistingItemsResult.MatchingItems);
        }

        /// <inheritdoc/>
        void ICollection<KeyValuePair<T1, T2>>.CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<T1, T2>>)this.forwardMap).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<T1, T2>>.Remove(KeyValuePair<T1, T2> pair)
        {
            T1 t1 = pair.Key;
            T2 t2 = pair.Value;

            var result = ValidateExistingItems(t1, t2);
            switch (result)
            {
                case ValidateExistingItemsResult.NonExistent:
                    return false;

                case ValidateExistingItemsResult.MismatchedItems:
                    throw new ArgumentException(ResourceProvider.KeysDoNotMatch(this.GetType()));
            }

            this.forwardMap.Remove(t1);
            this.reverseMap.Remove(t2);
            return true;
        }

        #endregion

        #region Implementation of IDictionary<T1, T2>

        /// <inheritdoc/>
        ICollection<T1> IDictionary<T1, T2>.Keys => this.forwardMap.Keys;

        /// <inheritdoc/>
        ICollection<T2> IDictionary<T1, T2>.Values => this.forwardMap.Values;

        /// <inheritdoc/>
        T2 IDictionary<T1, T2>.this[T1 t1]
        {
            get => this.Lookup(t1);
            set => throw new NotSupportedException();
        }

        /// <inheritdoc/>
        bool IDictionary<T1, T2>.ContainsKey(T1 t1)
        {
            return this.Contains(t1);
        }

        #endregion

        #region IReadOnlyDictionary<T1, T2>

        /// <inheritdoc/>
        IEnumerable<T1> IReadOnlyDictionary<T1, T2>.Keys => this.forwardMap.Keys;

        /// <inheritdoc/>
        IEnumerable<T2> IReadOnlyDictionary<T1, T2>.Values => this.forwardMap.Values;

        /// <inheritdoc/>
        bool IReadOnlyDictionary<T1, T2>.ContainsKey(T1 t1)
        {
            return this.Contains(t1);
        }

        #endregion

        /// <summary>
        /// Gets the item with the forward key.
        /// </summary>
        /// <param name="t1">The forward key of the element to get.</param>
        /// <returns>The element with the specified forward key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="t1"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">The forward key is not found.</exception>
        /// <remarks>
        /// Utilise the <see cref="Lookup(T1)"/> method if the generic parameter types cause this to be an ambiguous method.
        /// </remarks>
        public T2 this[T1 t1]
        {
            get => this.Lookup(t1);
        }

        /// <summary>
        /// Gets the item with the reverse key.
        /// </summary>
        /// <param name="t2">The reverse key of the element to get.</param>
        /// <returns>The element with the specified reverse key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="t2"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">The reverse key is not found.</exception>
        /// <remarks>
        /// Utilise the <see cref="LookupReverse(T2)"/> method if the generic parameter types cause this to be an ambiguous method.
        /// </remarks>
        public T1 this[T2 t2]
        {
            get => this.LookupReverse(t2);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        public int Count => this.forwardMap.Count;

        /// <summary>
        /// Adds the specified pair to the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        /// <param name="t1">The forward key.</param>
        /// <param name="t2">The reverse key.</param>
        /// <exception cref="ArgumentNullException">The forward or reverse key is null.</exception>
        /// <exception cref="ArgumentException">A mismatched pair already exists in the <see cref="BidirectionalMap{T1, T2}"/>.</exception>
        public void Add(T1 t1, T2 t2)
        {
            if (t1 == null)
                throw new ArgumentNullException(nameof(t1));

            if (t2 == null)
                throw new ArgumentNullException(nameof(t2));


            var result = this.ValidateExistingItems(t1, t2);
            switch (result)
            {
                case ValidateExistingItemsResult.MatchingItems:
                    return;

                case ValidateExistingItemsResult.MismatchedItems:
                    throw new ArgumentException(ResourceProvider.ElementWithSameKeyExists(this.GetType()));
            }

            this.forwardMap.Add(t1, t2);
            this.reverseMap.Add(t2, t1);
        }

        /// <summary>
        /// Removes all items from the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        public void Clear()
        {
            this.forwardMap.Clear();
            this.reverseMap.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalMap{T1, T2}"/> contains the specified pair.
        /// </summary>
        /// <param name="t1">The forward key to locate in the <see cref="BidirectionalMap{T1, T2}"/>.</param>
        /// <param name="t2">The reverse key to locate in the <see cref="BidirectionalMap{T1, T2}"/>.</param>
        /// <returns>
        /// true if the <see cref="BidirectionalMap{T1, T2}"/> contains the specified pair; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The forward or reverse key is null.</exception>
        public bool Contains(T1 t1, T2 t2)
        {
            if (t1 == null)
                throw new ArgumentNullException(nameof(t1));

            if (t2 == null)
                throw new ArgumentNullException(nameof(t2));


            var result = this.ValidateExistingItems(t1, t2);
            return (result == ValidateExistingItemsResult.MatchingItems);
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalMap{T1, T2}"/> contains the specified forward key.
        /// </summary>
        /// <param name="t1">The forward key to locate in the <see cref="BidirectionalMap{T1, T2}"/>.</param>
        /// <returns>
        /// true if the <see cref="BidirectionalMap{T1, T2}"/> contains the specified forward key; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The forward key is null.</exception>
        public bool Contains(T1 t1)
        {
            return this.forwardMap.ContainsKey(t1);
        }

        /// <summary>
        /// Determines whether the <see cref="BidirectionalMap{T1, T2}"/> contains the specified reverse key.
        /// </summary>
        /// <param name="t2">The reverse key to locate in the <see cref="BidirectionalMap{T1, T2}"/>.</param>
        /// <returns>
        /// true if the <see cref="BidirectionalMap{T1, T2}"/> contains the specified reverse key; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The reverse key is null.</exception>
        public bool ContainsReverse(T2 t2)
        {
            return this.reverseMap.ContainsKey(t2);
        }

        /// <summary>
        /// Gets the item with the forward key.
        /// </summary>
        /// <param name="t1">The forward key of the element to get.</param>
        /// <returns>The element with the specified forward key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="t1"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">The forward key is not found.</exception>
        public T2 Lookup(T1 t1)
        {
            return this.forwardMap[t1];
        }

        /// <summary>
        /// Gets the item with the reverse key.
        /// </summary>
        /// <param name="t2">The reverse key of the element to get.</param>
        /// <returns>The element with the specified reverse key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="t2"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">The reverse key is not found.</exception>
        public T1 LookupReverse(T2 t2)
        {
            return this.reverseMap[t2];
        }

        /// <summary>
        /// Removes the specified pair from the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        /// <param name="t1">The forward key of the pair to remove.</param>
        /// <param name="t2">The reverse key of the pair to remove.</param>
        /// <returns>
        /// true if the pair is successfully found and removed; otherwise, false. This method
        /// returns false if the pair is not found in the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The forward or reverse key is null.</exception>
        /// <exception cref="ArgumentException">A mismatched pair already exists in the <see cref="BidirectionalMap{T1, T2}"/>.</exception>
        public bool Remove(T1 t1, T2 t2)
        {
            if (t1 == null)
                throw new ArgumentNullException(nameof(t1));

            if (t2 == null)
                throw new ArgumentNullException(nameof(t2));


            var result = this.ValidateExistingItems(t1, t2);
            switch (result)
            {
                case ValidateExistingItemsResult.NonExistent:
                    return false;

                case ValidateExistingItemsResult.MismatchedItems:
                    throw new ArgumentException(ResourceProvider.KeysDoNotMatch(this.GetType()));
            }

            this.forwardMap.Remove(t1);
            this.reverseMap.Remove(t2);
            return true;
        }

        /// <summary>
        /// Removes the pair with the specified forward key from the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        /// <param name="t1">The forward key of the pair to remove.</param>
        /// <returns>
        /// true if the pair is successfully found and removed; otherwise, false. This method
        /// returns false if the forward key is not found in the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The forward key is null.</exception>
        public bool Remove(T1 t1)
        {
            ArgumentNullException.ThrowIfNull(nameof(t1));

            if (!this.forwardMap.TryGetValue(t1, out T2? existingT2))
                return false;

            this.forwardMap.Remove(t1);
            this.reverseMap.Remove(existingT2);
            return true;
        }

        /// <summary>
        /// Removes the pair with the specified reverse key from the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </summary>
        /// <param name="t1">The reverse key of the pair to remove.</param>
        /// <returns>
        /// true if the pair is successfully found and removed; otherwise, false. This method
        /// returns false if the reverse key is not found in the <see cref="BidirectionalMap{T1, T2}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The reverse key is null.</exception>
        public bool RemoveReverse(T2 t2)
        {
            ArgumentNullException.ThrowIfNull(nameof(t2));

            if (!this.reverseMap.TryGetValue(t2, out T1? existingT1))
                return false;

            this.forwardMap.Remove(existingT1);
            this.reverseMap.Remove(t2);
            return true;
        }

        /// <summary>
        /// Gets a readonly dictionary representing the reverse lookup semantics.
        /// </summary>
        /// <returns>
        /// A readonly dictionary representing the reverse lookup semantics.
        /// </returns>
        public IReadOnlyDictionary<T2, T1> ReverseDictionary()
        {
            return this.reverseMap;
        }

        /// <summary>
        /// Gets the value associated with the specified forward key.
        /// </summary>
        /// <param name="t1">The forward key of the value to get.</param>
        /// <param name="t2">
        /// When this method returns, contains the value associated with the specified forward key, if the forward key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the <see cref="BidirectionalMap{T1, T2}"/> contains an element with the specified forward key; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The specified forward key is null.</exception>
        public bool TryGetValue(T1 t1, [MaybeNullWhen(false)] out T2 t2)
        {
            return this.forwardMap.TryGetValue(t1, out t2);
        }

        /// <summary>
        /// Gets the value associated with the specified reverse key.
        /// </summary>
        /// <param name="t1">The reverse key of the value to get.</param>
        /// <param name="t2">
        /// When this method returns, contains the value associated with the specified reverse key, if the reverse key is found;
        /// otherwise, the default value for the type of the value parameter.
        /// This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        /// true if the <see cref="BidirectionalMap{T1, T2}"/> contains an element with the specified reverse key; otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">The specified reverse key is null.</exception>
        public bool TryGetValueReverse(T2 t2, [MaybeNullWhen(false)] out T1 t1)
        {
            return this.reverseMap.TryGetValue(t2, out t1);
        }

        /// <summary>
        /// The result of validating if a pair exist in the map.
        /// </summary>
        private enum ValidateExistingItemsResult
        {
            /// <summary>
            /// The specified pair is not found in the map.
            /// </summary>
            NonExistent,

            /// <summary>
            /// The specified pair mismatch to keys that exist in the map.
            /// </summary>
            MismatchedItems,

            /// <summary>
            /// The specified pair is found in the map.
            /// </summary>
            MatchingItems,
        }

        private ValidateExistingItemsResult ValidateExistingItems(T1 t1, T2 t2)
        {
            if (this.forwardMap.TryGetValue(t1, out T2? existingT2))
            {
                if (object.Equals(t2, existingT2))
                {
                    return ValidateExistingItemsResult.MatchingItems;
                }
                else
                {
                    return ValidateExistingItemsResult.MismatchedItems;
                }
            }
            else
            {
                if (this.reverseMap.ContainsKey(t2))
                {
                    return ValidateExistingItemsResult.MismatchedItems;
                }

                return ValidateExistingItemsResult.NonExistent;
            }
        }
    }
}
