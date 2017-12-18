using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Collections
{
    public static class MergeJoin
    {
        #region FindMergeSet

        #region FindMergeSet (on IEnumerable)

        public static List<MergeItem<T, T>> FindMergeSet<T>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet)
        {
            return FindMergeSet(
                leftSet,
                rightSet,
                item => item,
                Comparer<T>.Default,
                item => item,
                EqualityComparer<T>.Default);
        }

        public static List<MergeItem<TKey, T>> FindMergeSet<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector)
        {
            return FindMergeSet(
                leftSet,
                rightSet,
                keySelector,
                Comparer<TKey>.Default,
                item => item,
                EqualityComparer<T>.Default);
        }

        public static List<MergeItem<TKey, T>> FindMergeSet<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer)
        {
            return FindMergeSet(
                leftSet,
                rightSet,
                keySelector,
                keyComparer,
                item => item,
                EqualityComparer<T>.Default);
        }

        public static List<MergeItem<TKey, TValue>> FindMergeSet<T, TKey, TValue>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<T, TValue> valueSelector,
            IEqualityComparer<TValue> valueEqualityComparer)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));


            //Order the sets by key
            var leftSetOrdered = leftSet.OrderBy(keySelector, keyComparer);
            var rightSetOrdered = rightSet.OrderBy(keySelector, keyComparer);

            return FindMergeSetPreOrdered(
                leftSetOrdered,
                rightSetOrdered,
                keySelector, keyComparer,
                valueSelector, valueEqualityComparer);
        }
        #endregion

        #region FindMergeSet (on IEnumerable<KeyValuePair>)

        public static List<MergeItem<TKey, TValue>> FindMergeSet<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet)
        {
            return FindMergeSet(
                leftSet,
                rightSet,
                Comparer<TKey>.Default,
                EqualityComparer<TValue>.Default);
        }

        public static List<MergeItem<TKey, TValue>> FindMergeSet<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueEqualityComparer)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));


            //Order the sets by key
            var leftSetOrdered = leftSet.OrderBy(pair => pair.Key, keyComparer);
            var rightSetOrdered = rightSet.OrderBy(pair => pair.Key, keyComparer);

            return FindMergeSetPreOrdered(
                leftSetOrdered,
                rightSetOrdered,
                keyComparer,
                valueEqualityComparer);
        }
        #endregion

        #region FindMergeSetPreOrdered (on IEnumerable)

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the elements, using the default comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<T, T>> FindMergeSetPreOrdered<T>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet)
        {
            return FindMergeSetPreOrdered(
                leftSet,
                rightSet,
                item => item,
                Comparer<T>.Default,
                item => item,
                EqualityComparer<T>.Default);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the return value from <paramref name="keySelector"/>, using
        ///   the default <typeparam name="TKey"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<TKey, T>> FindMergeSetPreOrdered<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector)
        {
            return FindMergeSetPreOrdered(
                leftSet,
                rightSet,
                keySelector,
                Comparer<TKey>.Default,
                item => item,
                EqualityComparer<T>.Default);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the return value from <paramref name="keySelector"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<TKey, T>> FindMergeSetPreOrdered<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer)
        {
            return FindMergeSetPreOrdered(
                leftSet,
                rightSet,
                keySelector,
                keyComparer,
                item => item,
                EqualityComparer<T>.Default);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the return value from <paramref name="keySelector"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<TKey, TValue>> FindMergeSetPreOrdered<T, TKey, TValue>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<T, TValue> valueSelector,
            IEqualityComparer<TValue> valueEqualityComparer)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            if (valueEqualityComparer == null)
                throw new ArgumentNullException(nameof(valueEqualityComparer));


            //Select the key-value pairs
            var leftKeyValueSet = leftSet.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), valueSelector(item)));
            var rightKeyValueSet = rightSet.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), valueSelector(item)));

            return FindMergeSetPreOrdered(
                leftKeyValueSet,
                rightKeyValueSet,
                keyComparer,
                valueEqualityComparer);
        }
        #endregion

        #region FindMergeSetPreOrdered (on IEnumerable<KeyValuePair>)

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the <see cref="KeyValuePair{TKey,TValue}.Key"/>, using
        ///   the default comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<TKey, TValue>> FindMergeSetPreOrdered<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet)
        {
            return FindMergeSetPreOrdered(
                leftSet,
                rightSet,
                Comparer<TKey>.Default,
                EqualityComparer<TValue>.Default);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the <see cref="KeyValuePair{TKey,TValue}.Key"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        public static List<MergeItem<TKey, TValue>> FindMergeSetPreOrdered<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueEqualityComparer)
        {
            var mergeSet = new List<MergeItem<TKey, TValue>>();

            InternalMergePreOrdered(
                leftSet,
                rightSet,
                keyComparer,
                valueEqualityComparer,

                (key, value) => mergeSet.Add(MergeItem.Match(key, value)),
                (key, leftValue, rightValue) => mergeSet.Add(MergeItem.Different(key, leftValue, rightValue)),
                (key, leftValue) => mergeSet.Add(MergeItem.LeftOnly(key, leftValue)),
                (key, rightValue) => mergeSet.Add(MergeItem.RightOnly(key, rightValue)));

            return mergeSet;
        }
        #endregion

        #endregion

        #region Merge

        #region Merge (on IEnumerable)

        public static void Merge<T>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Action<T> onMatch,
            Action<T, T> onDifferent,
            Action<T> onLeftOnly,
            Action<T> onRightOnly)
        {
            Merge(
                leftSet,
                rightSet,
                item => item,
                Comparer<T>.Default,
                item => item,
                EqualityComparer<T>.Default,

                (key, value) => onMatch(value),
                (key, lValue, rValue) => onDifferent(lValue, rValue),
                (key, lValue) => onLeftOnly(lValue),
                (key, rValue) => onRightOnly(rValue));
        }

        public static void Merge<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            Action<TKey, T> onMatch,
            Action<TKey, T, T> onDifferent,
            Action<TKey, T> onLeftOnly,
            Action<TKey, T> onRightOnly)
        {
            Merge(
                leftSet,
                rightSet,
                keySelector,
                Comparer<TKey>.Default,
                item => item,
                EqualityComparer<T>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        public static void Merge<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Action<TKey, T> onMatch,
            Action<TKey, T, T> onDifferent,
            Action<TKey, T> onLeftOnly,
            Action<TKey, T> onRightOnly)
        {
            Merge(
                leftSet,
                rightSet,
                keySelector,
                keyComparer,
                item => item,
                EqualityComparer<T>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        public static void Merge<T, TKey, TValue>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<T, TValue> valueSelector,
            IEqualityComparer<TValue> valueEqualityComparer,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));


            //Order the sets by key
            var leftSetOrdered = leftSet.OrderBy(keySelector, keyComparer);
            var rightSetOrdered = rightSet.OrderBy(keySelector, keyComparer);

            MergePreOrdered(
                leftSetOrdered,
                rightSetOrdered,
                keySelector, keyComparer,
                valueSelector, valueEqualityComparer,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }
        #endregion

        #region Merge (on IEnumerable<KeyValuePair>)

        public static void Merge<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            Merge(
                leftSet,
                rightSet,
                Comparer<TKey>.Default,
                EqualityComparer<TValue>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        public static void Merge<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueEqualityComparer,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));


            //Order the sets by key
            var leftSetOrdered = leftSet.OrderBy(pair => pair.Key, keyComparer);
            var rightSetOrdered = rightSet.OrderBy(pair => pair.Key, keyComparer);

            MergePreOrdered(
                leftSetOrdered,
                rightSetOrdered,
                keyComparer,
                valueEqualityComparer,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }
        #endregion

        #region MergePreOrdered (on IEnumerable)

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the elements, using the default comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<T>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Action<T> onMatch,
            Action<T, T> onDifferent,
            Action<T> onLeftOnly,
            Action<T> onRightOnly)
        {
            MergePreOrdered(
                leftSet,
                rightSet,
                item => item,
                Comparer<T>.Default,
                item => item,
                EqualityComparer<T>.Default,

                (key, value) => onMatch(value),
                (key, lValue, rValue) => onDifferent(lValue, rValue),
                (key, lValue) => onLeftOnly(lValue),
                (key, rValue) => onRightOnly(rValue));
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the value from <paramref name="keySelector"/>, using
        ///   the default <typeparam name="TKey"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            Action<TKey, T> onMatch,
            Action<TKey, T, T> onDifferent,
            Action<TKey, T> onLeftOnly,
            Action<TKey, T> onRightOnly)
        {
            MergePreOrdered(
                leftSet,
                rightSet,
                keySelector,
                Comparer<TKey>.Default,
                item => item,
                EqualityComparer<T>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the value from <paramref name="keySelector"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<T, TKey>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Action<TKey, T> onMatch,
            Action<TKey, T, T> onDifferent,
            Action<TKey, T> onLeftOnly,
            Action<TKey, T> onRightOnly)
        {
            MergePreOrdered(
                leftSet,
                rightSet,
                keySelector,
                keyComparer,
                item => item,
                EqualityComparer<T>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the value from <paramref name="keySelector"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<T, TKey, TValue>(
            IEnumerable<T> leftSet,
            IEnumerable<T> rightSet,
            Func<T, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<T, TValue> valueSelector,
            IEqualityComparer<TValue> valueEqualityComparer,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            if (valueSelector == null)
                throw new ArgumentNullException(nameof(valueSelector));

            if (valueEqualityComparer == null)
                throw new ArgumentNullException(nameof(valueEqualityComparer));


            //Select the key-value pairs
            var leftKeyValueSet = leftSet.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), valueSelector(item)));
            var rightKeyValueSet = rightSet.Select(item => new KeyValuePair<TKey, TValue>(keySelector(item), valueSelector(item)));

            MergePreOrdered(
                leftKeyValueSet,
                rightKeyValueSet,
                keyComparer,
                valueEqualityComparer,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }
        #endregion

        #region MergePreOrdered (on IEnumerable<KeyValuePair>)

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the <see cref="KeyValuePair{TKey,TValue}.Key"/>, using
        ///   the default comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            MergePreOrdered(
                leftSet,
                rightSet,
                Comparer<TKey>.Default,
                EqualityComparer<TValue>.Default,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the <see cref="KeyValuePair{TKey,TValue}.Key"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will render inaccurate results.
        ///  </para>
        /// </remarks>
        public static void MergePreOrdered<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueEqualityComparer,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            InternalMergePreOrdered(
                leftSet,
                rightSet,
                keyComparer,
                valueEqualityComparer,

                onMatch, onDifferent, onLeftOnly, onRightOnly);
        }
        #endregion

        #endregion

        #region Private Methods

        /// <remarks>
        ///  <para>
        ///   Assumes that <paramref name="leftSet"/> and  <paramref name="rightSet"/> have 
        ///   been pre-sorted by the <see cref="KeyValuePair{TKey,TValue}.Key"/>, using
        ///   the <paramref name="keyComparer"/> comparer.
        ///  </para>
        ///  <para>
        ///   If this assumption is not met, the method will return inaccurate results.
        ///  </para>
        /// </remarks>
        private static void InternalMergePreOrdered<TKey, TValue>(
            IEnumerable<KeyValuePair<TKey, TValue>> leftSet,
            IEnumerable<KeyValuePair<TKey, TValue>> rightSet,
            IComparer<TKey> keyComparer,
            IEqualityComparer<TValue> valueEqualityComparer,
            Action<TKey, TValue> onMatch,
            Action<TKey, TValue, TValue> onDifferent,
            Action<TKey, TValue> onLeftOnly,
            Action<TKey, TValue> onRightOnly)
        {
            //Validation
            if (leftSet == null)
                throw new ArgumentNullException(nameof(leftSet));

            if (rightSet == null)
                throw new ArgumentNullException(nameof(rightSet));

            if (keyComparer == null)
                throw new ArgumentNullException(nameof(keyComparer));

            if (valueEqualityComparer == null)
                throw new ArgumentNullException(nameof(valueEqualityComparer));


            //Get the enumerators
            using (var leftEnumerator = leftSet.GetEnumerator())
            using (var rightEnumerator = rightSet.GetEnumerator())
            {
                //Move to first elements
                bool leftIsFinished = !leftEnumerator.MoveNext();
                bool rightIsFinished = !rightEnumerator.MoveNext();

                //Loop through the ordered collections
                while (!leftIsFinished && !rightIsFinished)
                {
                    var leftPair = leftEnumerator.Current;
                    var rightPair = rightEnumerator.Current;

                    TKey leftKey = leftPair.Key;
                    TKey rightKey = rightPair.Key;

                    int result = keyComparer.Compare(leftKey, rightKey);
                    if (result == 0) // leftKey == rightKey
                    {
                        #region In Both

                        TValue leftValue = leftPair.Value;
                        TValue rightValue = rightPair.Value;

                        if (valueEqualityComparer.Equals(leftValue, rightValue))
                        {
                            //Values match
                            if (!ReferenceEquals(onMatch, null))
                                onMatch(leftKey, leftValue);
                        }
                        else
                        {
                            //Values do not match
                            if (!ReferenceEquals(onDifferent, null))
                                onDifferent(leftKey, leftValue, rightValue);
                        }


                        // increment both
                        leftIsFinished = !leftEnumerator.MoveNext();
                        rightIsFinished = !rightEnumerator.MoveNext();

                        #endregion
                    }
                    else if (result < 0) // leftKey < rightKey   (in the left only)
                    {
                        #region Left Only

                        if (!ReferenceEquals(onLeftOnly, null))
                        {
                            TValue leftValue = leftPair.Value;
                            onLeftOnly(leftKey, leftValue);
                        }

                        // increment left
                        leftIsFinished = !leftEnumerator.MoveNext();

                        #endregion
                    }
                    else if (result > 0) // leftKey > rightKey   (in the right only)
                    {
                        #region Right Only

                        if (!ReferenceEquals(onRightOnly, null))
                        {
                            TValue rightValue = rightPair.Value;
                            onRightOnly(rightKey, rightValue);
                        }

                        // increment right
                        rightIsFinished = !rightEnumerator.MoveNext();

                        #endregion
                    }
                }

                while (!leftIsFinished)
                {
                    if (!ReferenceEquals(onLeftOnly, null))
                    {
                        var leftPair = leftEnumerator.Current;
                        onLeftOnly(leftPair.Key, leftPair.Value);
                    }

                    leftIsFinished = !leftEnumerator.MoveNext();
                }

                while (!rightIsFinished)
                {
                    if (!ReferenceEquals(onRightOnly, null))
                    {
                        var rightPair = rightEnumerator.Current;
                        onRightOnly(rightPair.Key, rightPair.Value);
                    }

                    rightIsFinished = !rightEnumerator.MoveNext();
                }
            }
        }

        #endregion
    }

    public enum MergeType
    {
        #region Members

        /// <summary>
        /// The item is found in the left collection only.
        /// </summary>
        LeftOnly,

        /// <summary>
        /// The item is found in the right collection only.
        /// </summary>
        RightOnly,

        /// <summary>
        /// The item is found in both collections, but its value differs.
        /// </summary>
        Different,

        /// <summary>
        /// The item is identical between the collections.
        /// </summary>
        Match,
        #endregion
    }

    public static class MergeItem
    {
        #region Static Methods

        public static MergeItem<TKey, TValue> LeftOnly<TKey, TValue>(TKey key, TValue leftValue)
        {
            return new MergeItem<TKey, TValue>(MergeType.LeftOnly, key, leftValue, default(TValue));
        }

        public static MergeItem<TKey, TValue> RightOnly<TKey, TValue>(TKey key, TValue rightValue)
        {
            return new MergeItem<TKey, TValue>(MergeType.RightOnly, key, default(TValue), rightValue);
        }

        public static MergeItem<TKey, TValue> Match<TKey, TValue>(TKey key, TValue value)
        {
            return new MergeItem<TKey, TValue>(MergeType.Match, key, value, value);
        }

        public static MergeItem<TKey, TValue> Different<TKey, TValue>(TKey key, TValue leftValue, TValue rightValue)
        {
            return new MergeItem<TKey, TValue>(MergeType.Different, key, leftValue, rightValue);
        }

        #endregion
    }

    public struct MergeItem<TKey, TValue>
    {
        #region Fields

        public readonly MergeType MergeType;
        public readonly TKey Key;
        public readonly TValue LeftValue;
        public readonly TValue RightValue;
        #endregion

        #region Constructors

        internal MergeItem(MergeType mergeType, TKey key, TValue leftValue, TValue rightValue)
        {
            this.MergeType = mergeType;
            this.Key = key;
            this.LeftValue = leftValue;
            this.RightValue = rightValue;
        }

        #endregion
    }

}
