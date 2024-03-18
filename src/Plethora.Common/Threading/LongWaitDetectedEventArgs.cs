using System;
using System.Collections.Generic;

namespace Plethora.Threading
{
    /// <summary>
    /// Provides data for a long-wait detected event
    /// </summary>
    public class LongWaitDetectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initialise a new instance of the <see cref="LongWaitDetectedEventArgs"/> class.
        /// </summary>
        /// <param name="longWaitingLockContext">The <see cref="LockContext"/> for which the long-wait was detected.</param>
        /// <param name="applicationLockContext">The list of all <see cref="LockContext"/> instances for the applciation.</param>
        public LongWaitDetectedEventArgs(
            LockContext longWaitingLockContext,
            IReadOnlyCollection<LockContext> applicationLockContexts)
        {
            if (longWaitingLockContext == null)
                throw new ArgumentNullException(nameof(longWaitingLockContext));

            if (applicationLockContexts == null)
                throw new ArgumentNullException(nameof(applicationLockContexts));

            this.LongWaitingLockContext = longWaitingLockContext;
            this.ApplicationLockContexts = applicationLockContexts;
        }

        /// <summary>
        /// Gets the <see cref="LockContext"/> for which the long-wait was detected 
        /// </summary>
        public LockContext LongWaitingLockContext { get; }

        /// <summary>
        /// Gets the list of all <see cref="LockContext"/> instances for the applciation
        /// </summary>
        public IReadOnlyCollection<LockContext> ApplicationLockContexts { get; }
    }
}
