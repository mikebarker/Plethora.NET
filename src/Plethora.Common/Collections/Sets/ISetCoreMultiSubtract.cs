using JetBrains.Annotations;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Extends <see cref="ISetCore"/> to allow for subtract method to return multiple sets.
    /// </summary>
    public interface ISetCoreMultiSubtract
    {
        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        [Pure, NotNull]
        IReadOnlyCollection<ISetCore> Subtract([NotNull] ISetCore other);
    }

    /// <summary>
    /// Extends <see cref="ISetCore{T}"/> to allow for subtract method to return multiple sets.
    /// </summary>
    public interface ISetCoreMultiSubtract<T> : ISetCoreMultiSubtract
    {
        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        [Pure, NotNull]
        IReadOnlyCollection<ISetCore<T>> Subtract([NotNull] ISetCore<T> other);
    }

    public static class SetCoreMultiSubtractHelper
    {
        public static IReadOnlyCollection<ISetCore> SubtractMulti(this ISetCore x, ISetCore y)
        {
            if (x is ISetCoreMultiSubtract xMulti)
            {
                return xMulti.Subtract(y);
            }

            var result = new[] { x.Subtract(y) };
            return result;
        }

        public static IReadOnlyCollection<ISetCore<T>> SubtractMulti<T>(this ISetCore<T> x, ISetCore<T> y)
        {
            if (x is ISetCoreMultiSubtract<T> xMulti)
            {
                return xMulti.Subtract(y);
            }

            var result = new[] { x.Subtract(y) };
            return result;
        }
    }
}
