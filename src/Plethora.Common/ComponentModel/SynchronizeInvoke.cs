using System.ComponentModel;
using Plethora.Threading;

namespace Plethora.ComponentModel
{
    /// <summary>
    /// Implementation of <see cref="ISynchronizeInvoke"/>, which does not require
    /// a UI thread and message queue.
    /// </summary>
    /// <remarks>
    /// Each <see cref="SynchronizeInvoke"/> object executes on its own separate thread.
    /// </remarks>
    public class SynchronizeInvoke : WorkQueue, ISynchronizeInvoke
    {
        #region Constructors

        public SynchronizeInvoke()
            : base(1)
        {
        }
        #endregion

        #region Implementation of ISynchronizeInvoke

        /// <summary>
        /// Gets a value indicating whether the caller must call <see cref="ISynchronizeInvoke.Invoke"/> when
        /// calling an object that implements this interface.
        /// </summary>
        /// <returns>
        /// true if the caller must call <see cref="ISynchronizeInvoke.Invoke"/>; otherwise, false.
        /// </returns>
        public bool InvokeRequired
        {
            get { return !this.IsWorkerThread(); }
        }
        #endregion
    }
}
