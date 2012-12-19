using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the union of two underlying sets.
    /// </summary>
    internal sealed class UnionSet<T> : BaseSetImpl<T>, ISet<T>
    {
        #region Fields

        private readonly ISet<T> a;
        private readonly ISet<T> b;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="UnionSet{T}"/> class.
        /// </summary>
        public UnionSet(ISet<T> a, ISet<T> b)
        {
            //Validation
            if (a == null)
                throw new ArgumentNullException("a");

            if (b == null)
                throw new ArgumentNullException("b");


            this.a = a;
            this.b = b;
        }

        #endregion

        #region Overrides of BaseSetImpl<T>

        public override bool Contains(T element)
        {
            return
                a.Contains(element) ||
                b.Contains(element);
        }

        #endregion
    }
}
