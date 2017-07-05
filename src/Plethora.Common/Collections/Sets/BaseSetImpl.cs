using System;

using JetBrains.Annotations;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// A base implementation of the <see cref="ISetCore{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the set.</typeparam>
    /// <remarks>
    ///  <para>
    ///   An inheriting class can provide optimised versions of the <see cref="Union"/>, <see cref="Intersect"/>
    ///   and <see cref="Subtract"/> methods. Because the <see cref="Union"/> and <see cref="Intersect"/> methods
    ///   are commutative, if either set indicates that it has an optimised version of the method, that override
    ///   is used. This is indicated by returning true in the <see cref="IsNativeUnion"/> and <see cref="IsNativeIntersect"/>
    ///   properties, respectively.
    ///  </para>
    ///  <para>
    ///   NOTE: If an inheritting class overrides either of the <see cref="IsNativeUnion"/> or <see cref="IsNativeIntersect"/>
    ///   properties to return true, it must not call the base methods in <see cref="Union"/> or <see cref="Intersect"/>
    ///   methods as this can result in a recursive loop.
    ///  </para>
    /// </remarks>
    public abstract class BaseSetImpl<T> : ISetCore<T>
    {
        #region Implementation of ISetCore<T>

        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is represented; else false.</returns>
        public abstract bool Contains(T element);

        public abstract bool? IsEmpty { get; }

        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        public virtual ISetCore<T> Union(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));


            //Union is commutative
            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsNativeUnion))
                return otherImpl.Union(this);

            //Attempt any well-known optimisations
            ISetCore<T> result;
            if (this.TryWellKnownUnion(other, out result))
                return result;

            return new UnionSet<T>(this, other);
        }

        [ContractAnnotation("=> true, result: notnull; => false, result: null")]
        protected bool TryWellKnownUnion([NotNull] ISetCore<T> other, [CanBeNull] out ISetCore<T> result)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));


            bool thisIsEmpty = (this.IsEmpty == true);

            var otherImpl = other as BaseSetImpl<T>;
            bool otherIsEmpty = ((otherImpl != null) && (otherImpl.IsEmpty == true));

            if (thisIsEmpty && otherIsEmpty)
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            if (thisIsEmpty)
            {
                result = other;
                return true;
            }

            if (otherIsEmpty)
            {
                result = this;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        public virtual ISetCore<T> Intersect(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));


            //Intersect is commutative
            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsNativeIntersect))
                return otherImpl.Intersect(this);

            //Attempt any well-known optimisations
            ISetCore<T> result;
            if (this.TryWellKnownIntersect(other, out result))
                return result;

            return new IntersectionSet<T>(this, other);
        }

        [ContractAnnotation("=> true, result: notnull; => false, result: null")]
        protected bool TryWellKnownIntersect([NotNull] ISetCore<T> other, [CanBeNull] out ISetCore<T> result)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (this.IsEmpty == true)
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsEmpty == true))
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        public virtual ISetCore<T> Subtract(ISetCore<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            //Attempt any well-known optimisations
            ISetCore<T> result;
            if (this.TryWellKnownSubtract(other, out result))
                return result;

            return new SubtractionSet<T>(this, other);
        }

        [ContractAnnotation("=> true, result: notnull; => false, result: null")]
        protected bool TryWellKnownSubtract([NotNull] ISetCore<T> other, [CanBeNull] out ISetCore<T> result)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other is CompleteSet<T>)
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            if (this.IsEmpty == true)
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsEmpty == true))
            {
                result = this;
                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Returns the inverse set of this set.
        /// </summary>
        public virtual ISetCore<T> Inverse()
        {
            return new InverseSet<T>(this);
        }

        #endregion

        /// <summary>
        /// Gets a flag indicating whether the implementation natively supports set union.
        /// </summary>
        /// <remarks>
        /// Due to the commutative nature of the <see cref="Union"/> operator, the operation (A union B)
        /// can be optimised to (B union A) if B natively supports the union operator.
        /// </remarks>
        protected virtual bool IsNativeUnion
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a flag indicating whether the implementation natively supports set intersection.
        /// </summary>
        /// <remarks>
        /// Due to the commutative nature of the <see cref="Intersect"/> operator, the operation (A intersect B)
        /// can be optimised to (B intersect A) if B natively supports the intersect operator.
        /// </remarks>
        protected virtual bool IsNativeIntersect
        {
            get { return false; }
        }
    }
}
