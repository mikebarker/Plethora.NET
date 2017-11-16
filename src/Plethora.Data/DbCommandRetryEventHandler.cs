using System;
using System.ComponentModel;
using System.Data;

namespace Plethora.Data
{
    /// <summary>
    /// Represents the method which will handle the retry event when executing a <see cref="IDbCommand"/>.
    /// </summary>
    /// <param name="sender">The origin of the event.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void DbCommandRetryEventHandler(object sender, DbCommandRetryEventArgs e);

    /// <summary>
    /// Provides the data for a retry event when executing a <see cref="IDbCommand"/>.
    /// </summary>
    [Serializable]
    public class DbCommandRetryEventArgs : CancelEventArgs
    {
        private readonly IDbCommand command;
        private readonly Exception exception;

        /// <summary>
        /// Initialises a new instance of the <see cref="DbCommandRetryEventArgs"/> class.
        /// </summary>
        /// <param name="command">The <see cref="IDbCommand"/> which is to be retried.</param>
        /// <param name="exception">The <see cref="Exception"/> which has caused the retry.</param>
        public DbCommandRetryEventArgs(IDbCommand command, Exception exception)
            : base(false)
        {
            this.command = command;
            this.exception = exception;
        }

        /// <summary>
        /// The <see cref="IDbCommand"/> which is to be retried.
        /// </summary>
        public IDbCommand Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// The <see cref="Exception"/> which has caused the retry.
        /// </summary>
        public Exception Exception
        {
            get { return this.exception; }
        }
    }
}