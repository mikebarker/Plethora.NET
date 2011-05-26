using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Plethora.ExpressionAide;

namespace Plethora.fqi
{
    public class IndexedEnumerable<T> : IIndexedEnumerable<T>
    {
        #region Fields

        private readonly IIndexedEnumerable<T> innerIndexedEnumerable;
        private readonly Func<T, bool> predicate;
        private readonly int indexesConsumed;
        private readonly IDictionary<string, ILateRange> ranges;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="IndexedEnumerable{T}"/> class.
        /// </summary>
        /// <param name="indexedEnumerable">The index enumerator to be filtered.</param>
        /// <param name="expr">The filtering expression to be applied.</param>
        /// <param name="memberRanges">A list of ranges for each member.</param>
        public IndexedEnumerable(IIndexedEnumerable<T> indexedEnumerable, Expression<Func<T, bool>> expr, IDictionary<string, INamedLateRange> memberRanges)
        {
            this.innerIndexedEnumerable = indexedEnumerable;
            this.predicate = t => CachedExecutor.Execute(expr, t);

            //Get restrictive ranges for refined selection
            this.ranges = GetRelevantRanges(memberRanges, indexedEnumerable, this.SupportsOutOfOrderIndexing);
            this.indexesConsumed = (this.SupportsOutOfOrderIndexing) ? 0 : this.ranges.Count;
        }
        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="IEnumerator{T}" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.GetIndexedEnumerator(t => true, new Dictionary<string, ILateRange>());
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Implementation of IIndexedEnumerable<T>

        /// <summary>
        /// Gets the list of indexed members, in order by which elements are sorted.
        /// </summary>
        public IEnumerable<string> IndexedMembers
        {
            get { return this.innerIndexedEnumerable.IndexedMembers.Skip(this.indexesConsumed); }
        }

        /// <summary>
        /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
        /// by a minimum and maximum range.
        /// </summary>
        IIndexedEnumerable<T> IIndexedEnumerable<T>.FilterBy(Expression<Func<T, bool>> expression)
        {
            var memberRanges = ExpressionAnalyser.GetMemberRestrictions(expression);

            return FilterBy(expression, memberRanges);
        }

        /// <summary>
        /// Get an enumerator filtered by the specified predicate.
        /// </summary>
        /// <param name="predicate">The filtering predicate to be applied.</param>
        /// <param name="ranges">The keys' ranges to restrict by.</param>
        /// <returns>
        /// The required enumerator.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   The <paramref name="ranges"/> pertains to the members returned
        ///   by <see cref="IIndexedEnumerable{T}.IndexedMembers"/>.
        ///  </para>
        ///  <para>
        ///   The parameter <paramref name="ranges"/> is a helper array which identifies
        ///   the minimum and maximum values of the available keys.
        ///  </para>
        /// </remarks>
        public IEnumerator<T> GetIndexedEnumerator(Func<T, bool> predicate, IDictionary<string, ILateRange> ranges)
        {
            var totalPredicate = this.predicate.And(predicate);
            var totalRanges = LateRangeHelper.CombineDictionariesAND(this.ranges, ranges);

            return this.innerIndexedEnumerable.GetIndexedEnumerator(totalPredicate, totalRanges);
        }

        /// <summary>
        /// Gets a flag indicating whether the <see cref="IIndexedEnumerable{T}"/>
        /// will support the usage of out-of-order indexing.
        /// </summary>
        public bool SupportsOutOfOrderIndexing
        {
            get { return this.innerIndexedEnumerable.SupportsOutOfOrderIndexing; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a <see cref="IIndexedEnumerable{T}"/> which restricts its returned values
        /// by a mniimum and maximum range.
        /// </summary>
        public IIndexedEnumerable<T> FilterBy(Expression<Func<T, bool>> expr, IDictionary<string, INamedLateRange> memberRanges)
        {
            return new IndexedEnumerable<T>(this, expr, memberRanges);
        }
        #endregion

        #region Private Methods

        private static IDictionary<string, ILateRange> GetRelevantRanges(
            IDictionary<string, INamedLateRange> memberRanges,
            IIndexedEnumerable<T> indexedEnumerable,
            bool outOfOrderIndexing)
        {
            var relevantMemberRanges = new Dictionary<string, ILateRange>(memberRanges.Count);

            foreach (var propertyName in indexedEnumerable.IndexedMembers)
            {
                INamedLateRange namedRange;
                bool result = memberRanges.TryGetValue(propertyName, out namedRange);
                if (result)
                {
                    relevantMemberRanges.Add(namedRange.Name, namedRange);
                }
                else
                {
                    if (!outOfOrderIndexing)
                        break;
                }
            }

            //Set the output return variable
            return relevantMemberRanges;
        }
        #endregion
    }
}
