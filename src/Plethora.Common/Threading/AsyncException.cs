using System;
using System.Runtime.Serialization;

namespace Plethora.Threading
{
    /// <summary>
    /// Exception thrown during asynchronous execution.
    /// </summary>
    [Serializable]
    public class AsyncException : Exception
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

#if NET8_0_OR_GREATER
        [Obsolete(DiagnosticId = "SYSLIB0051")] // add this attribute to the serialization ctor
#endif
        protected AsyncException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
