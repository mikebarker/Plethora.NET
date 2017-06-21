using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the set difference between one set and another.
    /// </summary>
    internal sealed class SubtractionSet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        #region Fields

        private readonly ISetCore<T> a;
        private readonly ISetCore<T> b;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="SubtractionSet{T}"/> class.
        /// </summary>
        public SubtractionSet(ISetCore<T> a, ISetCore<T> b)
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
            return this.a.Contains(element) &&
                !this.b.Contains(element);
        }

        public override bool? IsEmpty
        {
            get
            {
                if (this.a.IsEmpty == true)
                {
                    return true;
                }

                if ((this.a.IsEmpty == false) &&
                    (this.b.IsEmpty == true))
                {
                    return false;
                }

                return null;
            }
        }

        #endregion
    }
}
