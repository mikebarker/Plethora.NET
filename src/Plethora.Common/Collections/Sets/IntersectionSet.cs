﻿using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// Represents the intersection of two underlying sets.
    /// </summary>
    internal sealed class IntersectionSet<T> : BaseSetImpl<T>, ISetCore<T>
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

        public override bool? IsEmpty
        {
            get
            {
                if ((a.IsEmpty == true) ||
                    (b.IsEmpty == true))
                {
                    return true;
                }

                return null;
            }
        }

        #endregion
    }
}
