using System;

namespace Plethora.Collections.Sets
{
    /// <summary>
    /// A base implementation of the <see cref="ISet{T}"/> interface.
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
    public abstract class BaseSetImpl<T> : ISet<T>
    {
        #region Implementation of ISet<T>

        /// <summary>
        /// Gets a flag indicating whether an element is included in the set.
        /// </summary>
        /// <param name="element">The element to be tested.</param>
        /// <returns>True if the element is represented; else false.</returns>
        public abstract bool Contains(T element);


        /// <summary>
        /// Returns a set representing the union of this and another set.
        /// </summary>
        public virtual ISet<T> Union(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            //Union is commutative
            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsNativeUnion))
                return otherImpl.Union(this);


            return new UnionSet<T>(this, other);
        }

        /// <summary>
        /// Returns a set representing the intersection of this and another set.
        /// </summary>
        public virtual ISet<T> Intersect(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            //Intersect is commutative
            var otherImpl = other as BaseSetImpl<T>;
            if ((otherImpl != null) && (otherImpl.IsNativeIntersect))
                return otherImpl.Intersect(this);


            return new IntersectionSet<T>(this, other);
        }

        /// <summary>
        /// Returns a set representing the set difference of this and another set.
        /// </summary>
        public virtual ISet<T> Subtract(ISet<T> other)
        {
            //Validation
            if (other == null)
                throw new ArgumentNullException("other");


            return new SubtractionSet<T>(this, other);
        }

        #endregion

        /// <summary>
        /// Gets a flag indicating whether the implementation natively supports set union.
        /// </summary>
        protected virtual bool IsNativeUnion
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a flag indicating whether the implementation natively supports set intersection.
        /// </summary>
        protected virtual bool IsNativeIntersect
        {
            get { return false; }
        }
    }
}
