using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections
{
    /// <summary>
    /// Presents a hierarchy of objects.
    /// </summary>
    public abstract class Hierarchy<T>
    {
        #region Fields

        public readonly T[] items;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="Hierarchy{T}"/> class.
        /// </summary>
        protected Hierarchy(params T[] items)
        {
            //Validation
            if (items == null)
                throw new ArgumentNullException("items");

            this.items = items;
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        /// <remarks>
        /// Lower indexed items give values in preference over lower indexed items.
        /// </remarks>
        protected TValue GetValue<TValue>(Func<T, TValue> valueSelector)
            where TValue : class
        {
            return GetValue(valueSelector, false);
        }

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        protected TValue GetValue<TValue>(Func<T, TValue> valueSelector, bool highIndexPriority)
            where TValue : class
        {
            var itteration = GetItteration(highIndexPriority);
            return GetValue(itteration, valueSelector);
        }

        /// <summary>
        /// Get the first non-default value from the hierarchy.
        /// </summary>
        /// <remarks>
        /// Lower indexed items give values in preference over lower indexed items.
        /// </remarks>
        protected TValue GetValue<TValue>(Func<T, TValue> valueSelector, TValue @default)
            where TValue : struct
        {
            return GetValue(valueSelector, @default, false);
        }

        /// <summary>
        /// Get the first non-default value from the hierarchy.
        /// </summary>
        protected TValue GetValue<TValue>(Func<T, TValue> valueSelector, TValue @default, bool highIndexPriority)
            where TValue : struct
        {
            var itteration = GetItteration(highIndexPriority);

            Func<T, TValue?> nullableValueSelector = delegate(T t)
                {
                    TValue value = valueSelector(t);
                    return @default.Equals(value)
                        ? (TValue?)null
                        : (TValue?)value;
                };

            TValue? rtnValue = GetValue(itteration, nullableValueSelector);
            return (rtnValue.HasValue)
                ? rtnValue.Value
                : @default;
        }

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        /// <remarks>
        /// Lower indexed items give values in preference over lower indexed items.
        /// </remarks>
        protected TValue? GetValue<TValue>(Func<T, TValue?> valueSelector)
            where TValue : struct
        {
            return GetValue(valueSelector, false);
        }

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        protected TValue? GetValue<TValue>(Func<T, TValue?> valueSelector, bool highIndexPriority)
            where TValue : struct
        {
            var itteration = GetItteration(highIndexPriority);
            return GetValue(itteration, valueSelector);
        }
        #endregion

        #region Private Methods

        private IEnumerable<T> ReverseItems()
        {
            for (int i = this.items.Length - 1; i >= 0; i--)
            {
                yield return this.items[i];
            }
        }

        private IEnumerable<T> GetItteration(bool highIndexPriority)
        {
            var itterator =  (highIndexPriority)
                ? ReverseItems()
                : items;
            return itterator.Where(item => item != null);
        }

        private static TValue GetValue<TValue>(IEnumerable<T> enumerable, Func<T, TValue> valueSelector)
            where TValue : class
        {
            foreach (var item in enumerable)
            {
                TValue value = valueSelector(item);
                if (value != null)
                    return value;
            }
            return null;
        }

        private static TValue? GetValue<TValue>(IEnumerable<T> enumerable, Func<T, TValue?> valueSelector)
            where TValue : struct
        {
            foreach (var item in enumerable)
            {
                TValue? value = valueSelector(item);
                if (value != null)
                    return value;
            }
            return null;
        }
        #endregion
    }
}
