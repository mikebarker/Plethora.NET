using System;

namespace Plethora.Logging.Exceptions
{
    /// <summary>
    /// Wrapper class, which indicates that the inner exception has been logged.
    /// </summary>
    /// <seealso cref="IsLoggedException"/>
    [Serializable]
    public class LoggedException : IsLoggedException
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggedException"/> class.
        /// </summary>
        public LoggedException(Exception innerException)
            : base(innerException.Message, true, innerException)
        {
        }
        #endregion
    }
}
