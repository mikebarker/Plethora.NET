using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Plethora.ExpressionAide;

namespace Plethora.fqi
{
    public static class IIndexedEnumerableExtension
    {
        public static IIndexedEnumerable<T> Where<T>(
            this IIndexedEnumerable<T> indexedEnumerable,
            Expression<Func<T, bool>> expr)
        {
            return Where(indexedEnumerable, expr, null);
        }

        internal static IIndexedEnumerable<T> Where<T>(
            this IIndexedEnumerable<T> indexedEnumerable,
            Expression<Func<T, bool>> expr,
            IDictionary<string, INamedLateRange> restrictions)
        {
            if (indexedEnumerable is IndexedCollection<T>)
            {
                if (restrictions == null)
                    restrictions = ExpressionAnalyser.GetMemberRestrictions(expr);

                return ((IndexedCollection<T>)indexedEnumerable).FilterBy(expr, restrictions);
            }
            if (indexedEnumerable is IndexedEnumerable<T>)
            {
                if (restrictions == null)
                    restrictions = ExpressionAnalyser.GetMemberRestrictions(expr);

                return ((IndexedEnumerable<T>)indexedEnumerable).FilterBy(expr, restrictions);
            }

            return indexedEnumerable.FilterBy(expr);
        }
    }

    public static class IMultiIndexedEnumerableExtension
    {
        public static IIndexedEnumerable<T> Where<T>(
            this IMultiIndexedEnumerable<T> multiIndexEnumerable,
            Expression<Func<T, bool>> expr)
        {
            int highScore = 0;
            IIndexedEnumerable<T> highScoreIndex = null;

            //Find the most relevant multi-column index to use.
            var restrictions = ExpressionAnalyser.GetMemberRestrictions(expr);
            foreach (var indexedEnumerable in multiIndexEnumerable.IndexedEnumerables)
            {
                int score = ScoreRestrictionsInIndexedMembers(
                    indexedEnumerable.IndexedMembers,
                    restrictions,
                    indexedEnumerable.SupportsOutOfOrderIndexing);

                if (score > highScore)
                {
                    highScore = score;
                    highScoreIndex = indexedEnumerable;
                }
            }

            if (highScoreIndex == null)
            {
                //No suitable index
                // Use the IEnuemrable<T>.Where extension method.
                return multiIndexEnumerable.AsEnumerable().Where(t => CachedExecutor.Execute(expr, t)).AsIndexedEnumerable();
            }

            return highScoreIndex.Where(expr, restrictions);
        }

        /// <remarks>
        ///  <para>
        ///   The score is summed through the list of indexed members until the first non-equality
        ///   restriction is found. (Whereafter further indices are of no use in restricting the
        ///   result set required).
        ///  </para>
        ///  <para>
        ///   An equality restrictions scores 5.
        ///   A range restriction scores 3.
        ///   An unbound range restriction (i.e. greater than or less than only) scores 2.
        ///   An out-of-order index incures a penalty of -1.
        ///  </para>
        /// </remarks>
        private static int ScoreRestrictionsInIndexedMembers(
            IEnumerable<string> indexedMembers,
            IDictionary<string, INamedLateRange> restrictions,
            bool outOfOrderIndexing)
        {
            const int RESTRICTION_EQUALITY = 5;
            const int RESTRICTION_BOUND_RANGE = 3;
            const int RESTRICTION_UNBOUND_RANGE = 2;
            const int OUT_OF_ORDER_PENALTY = -1;

            int outOfOrderPenalty = 0;

            int score = 0;
            foreach (var member in indexedMembers)
            {
                INamedLateRange restriction;
                bool result = restrictions.TryGetValue(member, out restriction);
                if (!result)
                {
                    //Indexed member not restricted
                    if (outOfOrderIndexing)
                        outOfOrderPenalty += OUT_OF_ORDER_PENALTY;
                    else
                        break;
                }
                else
                {
                    //Indexed member is restricted
                    if ((restriction.HasMin && restriction.HasMax) && (restriction.MaxFunc == restriction.MinFunc))
                    {
                        score += RESTRICTION_EQUALITY;
                    }
                    else
                    {
                        if (restriction.HasMin && restriction.HasMax)
                            score += RESTRICTION_BOUND_RANGE;
                        else if (restriction.HasMin)
                            score += RESTRICTION_UNBOUND_RANGE;
                        else if (restriction.HasMax)
                            score += RESTRICTION_UNBOUND_RANGE;

                        if (!outOfOrderIndexing)
                            break;
                    }
                    score += outOfOrderPenalty;
                    outOfOrderPenalty = 0; //Reset the penalty
                }
            }
            return score;
        }
    }
}
