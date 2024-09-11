using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the intersection of two underlying sets.
    /// </summary>
    internal sealed class IntersectionSet<T> : BaseSetImpl<T>
    {
        #region Fields

        private readonly ISetCore<T> a;
        private readonly ISetCore<T> b;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="IntersectionSet{T}"/> class.
        /// </summary>
        public IntersectionSet(ISetCore<T> a, ISetCore<T> b)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);


            this.a = a;
            this.b = b;
        }

        #endregion

        #region Overrides of BaseSetImpl<T>

        public override bool Contains(T element)
        {
            return this.a.Contains(element) && this.b.Contains(element);
        }

        public override bool? IsEmpty
        {
            get
            {
                if ((this.a.IsEmpty == true) ||
                    (this.b.IsEmpty == true))
                {
                    return true;
                }

                return null;
            }
        }

        #endregion
    }
}
