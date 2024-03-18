using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections
{
    /// <summary>
    /// Presents a hierarchy of objects.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   Searches the hierarchy of objects provided to return a required value.
    ///   Of the values provided in the constructor, low-indexed items take
    ///   preference over high-indexed items. Order can be reversed using the
    ///   'highIndexPriority' arguments of the GetValue methods.
    ///  </para>
    ///  <para>
    ///   For properties / methods returning a class, the hierarchy is searched until
    ///   a non-null return value is found.
    ///  </para>
    ///  <para>
    ///   For properties / methods returning a struct, the hierarchy is searched until
    ///   the first value differing from specified default is found.
    ///   Nullables may also be used, in which case the first non-null value is returned.
    ///  </para>
    /// </remarks>
    /// <example>
    /// This class is intended to be used to implement an interface, and for it
    /// to represent a hierarchy of child objects from the same interface. Example:
    ///  <code>
    ///  <![CDATA[
    ///   public interface IStyle
    ///   {
    ///      Color ForeColor { get; }
    ///   }
    ///   
    ///   // Simple implementation of the interface
    ///   public class Style : IStyle
    ///   {
    ///      ...
    ///   }
    /// 
    ///   // Hierarchical implementation of the interface.
    ///   public class StyleHierarchy : Hierarchy<IStyle>, IStyle
    ///   {
    ///      public Color ForeColor
    ///      { 
    ///        //Return the first non-transparent color
    ///        get { return GetValue(style => style.ForeColor, Color.Transparent); }
    ///      }
    ///   }
    ///  ]]>
    ///  </code>
    /// </example>
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
            ArgumentNullException.ThrowIfNull(items);

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
        protected TValue? GetValue<TValue>(Func<T, TValue> valueSelector)
            where TValue : class
        {
            return this.GetValue(valueSelector, false);
        }

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        protected TValue? GetValue<TValue>(Func<T, TValue> valueSelector, bool highIndexPriority)
            where TValue : class
        {
            var iteration = this.GetIteration(highIndexPriority);
            return GetValue(iteration, valueSelector);
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
            return this.GetValue(valueSelector, @default, false);
        }

        /// <summary>
        /// Get the first non-default value from the hierarchy.
        /// </summary>
        protected TValue GetValue<TValue>(Func<T, TValue> valueSelector, TValue @default, bool highIndexPriority)
            where TValue : struct
        {
            var iteration = this.GetIteration(highIndexPriority);

            TValue? nullableValueSelector(T t)
            {
                TValue value = valueSelector(t);
                return @default.Equals(value)
                    ? null
                    : value;
            }

            TValue? rtnValue = GetValue(iteration, nullableValueSelector);
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
            return this.GetValue(valueSelector, false);
        }

        /// <summary>
        /// Get the first non-null value from the hierarchy.
        /// </summary>
        protected TValue? GetValue<TValue>(Func<T, TValue?> valueSelector, bool highIndexPriority)
            where TValue : struct
        {
            var iteration = this.GetIteration(highIndexPriority);
            return GetValue(iteration, valueSelector);
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

        private IEnumerable<T> GetIteration(bool highIndexPriority)
        {
            var iterator =  (highIndexPriority)
                ? this.ReverseItems()
                : this.items;
            return iterator.Where(item => item != null);
        }

        private static TValue? GetValue<TValue>(IEnumerable<T> enumerable, Func<T, TValue> valueSelector)
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
