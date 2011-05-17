using System;

namespace Plethora
{
    /// <summary>
    /// Strongly-typed generic wrapper for the <see cref="WeakReference"/> class.
    /// </summary>
    /// <typeparam name="T">The data type of the object tracked by this weak reference.</typeparam>
    public class WeakReference<T> : IEquatable<WeakReference<T>>
        where T : class
    {
        #region Fields

        private readonly WeakReference innerWeakReference;
        private readonly int hashCode; // The GetHashCode method must return a constant value.
        #endregion

        #region Constructors

        public WeakReference(T item)
            : this(item, false)
        {
        }

        public WeakReference(T item, bool trackResurrection)
        {
            //Validation
            if (ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            this.innerWeakReference = new WeakReference(item, trackResurrection);
            this.hashCode = item.GetHashCode();
        }
        #endregion

        #region Properties

        public T Target
        {
            get { return (T)this.innerWeakReference.Target; }
            set
            {
                //Validation
                if (ReferenceEquals(value, null))
                    throw new ArgumentNullException("value");

                this.innerWeakReference.Target = value;
            }
        }

        public bool IsAlive
        {
            get { return this.innerWeakReference.IsAlive; }
        }

        public bool TrackResurrection
        {
            get { return this.innerWeakReference.TrackResurrection; }
        }
        #endregion

        #region Implementation of IEquatable<WeakReference<T>>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(WeakReference<T> other)
        {
            if (other == null)
                return false;

            T thisTarget = this.Target;
            T otherTarget = other.Target;

            return Equals(thisTarget, otherTarget);
        }
        #endregion

        #region Object Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is WeakReference<T>))
                return false;

            var other = (WeakReference<T>)obj;
            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.hashCode;
        }

        public override string ToString()
        {
            var target = this.Target;

            string targetString = (target == null)
                ? "null"
                : target.ToString();

            return "WeakReference[" + targetString + "]";
        }
        #endregion

        #region Operators

        /// <summary>
        /// Explicit cast operator.
        /// </summary>
        /// <param name="item">The <see cref="WeakReference{T}"/> to be cast to <see cref="T"/>.</param>
        /// <returns>
        /// The reference item; or null if the item has been garbage-collected.
        /// </returns>
        public static explicit operator T(WeakReference<T> item)
        {
            return item.Target;
        }
        #endregion
    }
}
