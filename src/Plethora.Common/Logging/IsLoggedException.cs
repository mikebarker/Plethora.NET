using System;
using System.Runtime.Serialization;

namespace Plethora.Logging
{
    /// <summary>
    /// Exception base class, which indicates whether an exception has been logged.
    /// </summary>
    /// <remarks>
    /// Once an <see cref="IsLoggedException"/> has been logged, using the <see cref="Logger"/>
    /// class, any further attempts to log the exception will be ignorred.
    /// This is useful when putting in place exception logging as exceptions bubble-up
    /// through exception-handling code, as exceptions are logged when they first occur
    /// and do not then clutter up the logs by re-logging the same information.
    /// </remarks>
    [Serializable]
    public class IsLoggedException : Exception
    {
        #region Fields

        private bool? isLogged = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IsLoggedException"/> class.
        /// </summary>
        public IsLoggedException()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IsLoggedException"/> class.
        /// </summary>
        public IsLoggedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IsLoggedException"/> class.
        /// </summary>
        public IsLoggedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IsLoggedException"/> class.
        /// </summary>
        public IsLoggedException(string message, Exception innerException, bool isLogged)
            : base(message, innerException)
        {
            this.IsLogged = isLogged;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="IsLoggedException"/> class.
        /// </summary>
        protected IsLoggedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a flag indicating whether this <see cref="IsLoggedException"/>
        /// has been logged.
        /// </summary>
        public bool IsLogged
        {
            get
            {
                // Return the value if set
                if (this.isLogged.HasValue)
                    return this.isLogged.Value;

                // Allow the property to cascade
                var innerEx = this.InnerException as IsLoggedException;
                if (innerEx != null)
                    return innerEx.IsLogged;

                // Default to false
                return false;
            }
            set { this.isLogged = value; }
        }

        #endregion
    }
}
