﻿using System;
using System.Diagnostics.CodeAnalysis;

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
    ///   NOTE: If an inheriting class overrides either of the <see cref="IsNativeUnion"/> or <see cref="IsNativeIntersect"/>
    ///   properties to return true, it must not call the base methods in <see cref="Union"/> or <see cref="Intersect"/>
    ///   methods as this can result in a recursive loop.
    ///  </para>
    /// </remarks>
    public abstract class BaseSetImpl<T> : ISetCore<T>, ISetCore
    {
        #region Implementation of ISetCore

        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is represented; else false.</returns>
        bool ISetCore.Contains(object element)
        {
            if (element is not T elementT)
                return false;

            return this.Contains(elementT);
        }

        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        ISetCore ISetCore.Union(ISetCore other)
        {
            //Validation
            if (other is not ISetCore<T> otherT)
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(other), typeof(ISetCore<T>)), nameof(other));

            return this.Union(otherT);
        }

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        ISetCore ISetCore.Intersect(ISetCore other)
        {
            //Validation
            if (other is not ISetCore<T> otherT)
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(other), typeof(ISetCore<T>)), nameof(other));

            return this.Intersect(otherT);

        }

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        ISetCore ISetCore.Subtract(ISetCore other)
        {
            //Validation
            if (other is not ISetCore<T> otherT)
                throw new ArgumentException(ResourceProvider.ArgMustBeOfType(nameof(other), typeof(ISetCore<T>)), nameof(other));

            return this.Subtract(otherT);
        }

        /// <summary>
        /// Returns the inverse set of this set.
        /// </summary>
        ISetCore ISetCore.Inverse()
        {
            return this.Inverse();
        }

        #endregion

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
            ArgumentNullException.ThrowIfNull(other);


            //Union is commutative
            if ((other is BaseSetImpl<T> otherImpl) && (otherImpl.IsNativeUnion))
                return otherImpl.Union(this);

            //Attempt any well-known optimisations
            if (this.TryWellKnownUnion(other, out var result))
                return result;

            return new UnionSet<T>(this, other);
        }

        protected bool TryWellKnownUnion(ISetCore<T> other, [MaybeNullWhen(false)] out ISetCore<T> result)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);


            bool thisIsEmpty = (this.IsEmpty == true);

            bool otherIsEmpty = ((other is BaseSetImpl<T> otherImpl) && (otherImpl.IsEmpty == true));

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
            ArgumentNullException.ThrowIfNull(other);


            //Intersect is commutative
            if ((other is BaseSetImpl<T> otherImpl) && (otherImpl.IsNativeIntersect))
                return otherImpl.Intersect(this);

            //Attempt any well-known optimisations
            if (this.TryWellKnownIntersect(other, out var result))
                return result;

            return new IntersectionSet<T>(this, other);
        }

        protected bool TryWellKnownIntersect(ISetCore<T> other, [MaybeNullWhen(false)] out ISetCore<T> result)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);

            if (this.IsEmpty == true)
            {
                result = EmptySet<T>.Instance;
                return true;
            }

            if ((other is BaseSetImpl<T> otherImpl) && (otherImpl.IsEmpty == true))
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
            ArgumentNullException.ThrowIfNull(other);

            //Attempt any well-known optimisations
            if (this.TryWellKnownSubtract(other, out var result))
                return result;

            return new SubtractionSet<T>(this, other);
        }

        protected bool TryWellKnownSubtract(ISetCore<T> other, [MaybeNullWhen(false)] out ISetCore<T> result)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(other);

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

            if ((other is BaseSetImpl<T> otherImpl) && (otherImpl.IsEmpty == true))
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
