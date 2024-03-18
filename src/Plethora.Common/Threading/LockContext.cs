using System;

namespace Plethora.Threading
{
    /// <summary>
    /// Contains the data for a lock's context.
    /// </summary>
    public class LockContext
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LockContext"/> class.
        /// </summary>
        /// <param name="lock">The lock.</param>
        /// <param name="lockRequestStatus">The status of the lock request.</param>
        /// <param name="memberName">The member name of the origin of the lock.</param>
        /// <param name="sourceFilePath">The source file path of the origin of the lock.</param>
        /// <param name="sourceLineNumber">The source line number of the origin of the lock.</param>
        public LockContext(
            object @lock,
            LockRequestStatus lockRequestStatus,
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            if (@lock == null)
                throw new ArgumentNullException(nameof(@lock));

            this.Lock = @lock;
            this.LockRequestStatus = lockRequestStatus;
            this.MemberName = memberName;
            this.SourceFilePath = sourceFilePath;
            this.SourceLineNumber = sourceLineNumber;
        }

        /// <summary>
        /// Gets the lock.
        /// </summary>
        public object Lock { get; }

        /// <summary>
        /// Gets the status of the lock request.
        /// </summary>
        public LockRequestStatus LockRequestStatus { get; }

        /// <summary>
        /// Gets the member name of the origin of the lock.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the source file path of the origin of the lock.
        /// </summary>
        public string SourceFilePath { get; }

        /// <summary>
        /// Gets the source line number of the origin of the lock.
        /// </summary>
        public int SourceLineNumber { get; }
    }
}
