using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the union of two underlying sets.
    /// </summary>
    internal sealed class UnionSet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        #region Fields

        private readonly ISetCore<T> a;
        private readonly ISetCore<T> b;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="UnionSet{T}"/> class.
        /// </summary>
        public UnionSet(ISetCore<T> a, ISetCore<T> b)
        {
            //Validation
            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (b == null)
                throw new ArgumentNullException(nameof(b));


            this.a = a;
            this.b = b;
        }

        #endregion

        #region Overrides of BaseSetImpl<T>

        public override bool Contains(T element)
        {
            return this.a.Contains(element) || this.b.Contains(element);
        }

        public override bool? IsEmpty
        {
            get
            {
                if ((this.a.IsEmpty == true) &&
                    (this.b.IsEmpty == true))
                {
                    return true;
                }

                if ((this.a.IsEmpty == false) ||
                    (this.b.IsEmpty == false))
                {
                    return false;
                }

                return null;
            }
        }

        #endregion
    }
}
