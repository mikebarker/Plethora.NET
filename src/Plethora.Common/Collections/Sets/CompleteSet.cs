using System;

namespace Plethora.Collections.Sets
{
    public sealed class CompleteSet<T> : BaseSetImpl<T>
    {
        public static readonly CompleteSet<T> Instance = new();

        #region Constructors

        private CompleteSet()
        {
        }

        #endregion

        #region Implentation Of ISetCore<T>

        public override bool Contains(T element)
        {
            return true;
        }

        public override bool? IsEmpty
        {
            get { return false; }
        }

        #endregion
        
        #region Overrides of BaseSetImpl<T>

        public override ISetCore<T> Union(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            return this;
        }

        public override ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            return other;
        }

        public override ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            return other.Inverse();
        }

        public override ISetCore<T> Inverse()
        {
            return EmptySet<T>.Instance;
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
