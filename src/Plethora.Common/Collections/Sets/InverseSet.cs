using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the intersection of two underlying sets.
    /// </summary>
    internal sealed class InverseSet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        #region Fields

        private readonly ISetCore<T> a;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="InverseSet{T}"/> class.
        /// </summary>
        public InverseSet(ISetCore<T> a)
        {
            //Validation
            if (a == null)
                throw new ArgumentNullException(nameof(a));


            this.a = a;
        }

        #endregion

        #region Overrides of BaseSetImpl<T>

        public override bool Contains(T element)
        {
            return !this.a.Contains(element);
        }

        public override ISetCore<T> Inverse()
        {
            return this.a;
        }

        public override bool? IsEmpty
        {
            get
            {
                if (this.a.IsEmpty == true)
                    return false;

                if (this.a.IsEmpty == false)
                    return true;

                return null;
            }
        }

        #endregion
    }
}
