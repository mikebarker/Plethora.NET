using System;

namespace Plethora.fqi
{
    /// <summary>
    /// Extension class for predicates
    /// </summary>
    public static class PredicateExtensions
    {
        public static Func<T, bool> And<T>(this Func<T, bool> predicate0, Func<T, bool> predicate1)
        {
            return t => predicate0(t) && predicate1(t);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> predicate0, Func<T, bool> predicate1)
        {
            return t => predicate0(t) || predicate1(t);
        }

        public static Func<T, bool> XOr<T>(this Func<T, bool> predicate0, Func<T, bool> predicate1)
        {
            return t => predicate0(t) ^ predicate1(t);
        }

        public static Func<T, bool> Not<T>(this Func<T, bool> predicate)
        {
            return t => !predicate(t);
        }
    }
}
