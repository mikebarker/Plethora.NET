using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the intersection of two underlying sets.
    /// </summary>
    internal sealed class IntersectionSet<T> : BaseSetImpl<T>, ISet<T>
    {
        #region Fields

        private readonly ISet<T> a;
        private readonly ISet<T> b;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="IntersectionSet{T}"/> class.
        /// </summary>
        public IntersectionSet(ISet<T> a, ISet<T> b)
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
                a.Contains(element) &&
                b.Contains(element);
        }

        #endregion
    }
}
