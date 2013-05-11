using System;
using System.Runtime.Serialization;
using Plethora.Logging;

namespace Plethora.Threading
{
    /// <summary>
    /// Exception thrown during asynchronous execution.
    /// </summary>
    [Serializable]
    public class AsyncException : IsLoggedException
    {
        #region Constructors

        public AsyncException(Exception innerException)
            : base("An error occurred during asynchronous execution.", innerException)
        {
        }

        public AsyncException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AsyncException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
