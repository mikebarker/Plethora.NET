using System;

namespace Plethora.Collections.Sets
{
    public sealed class EmptySet<T> : BaseSetImpl<T>, ISetCore<T>
    {
        public static readonly EmptySet<T> Instance = new EmptySet<T>();

        #region Constructors

        private EmptySet()
        {
        }

        #endregion

        #region Implentation Of ISetCore<T>

        public override bool Contains(T element)
        {
            return false;
        }

        public override bool? IsEmpty
        {
            get { return true; }
        }

        #endregion
        
        #region Overrides of BaseSetImpl<T>

        public override ISetCore<T> Union(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            return other;
        }

        public override ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            return this;
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            return this;
        }

        public override ISetCore<T> Inverse()
        {
            return CompleteSet<T>.Instance;
        }

        protected override bool IsNativeUnion
        {
            get { return true; }
        }

        protected override bool IsNativeIntersect 
        {
            get { return true; }
        }

        #endregion

    }
}
