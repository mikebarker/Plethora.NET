namespace Plethora.Threading
{
    /// <summary>
    /// Gets the status of a lock request.
    /// </summary>
    public enum LockRequestStatus
    {
        /// <summary>
        /// The caller is currently awaiting to acquire the lock.
        /// </summary>
        Awaiting,

        /// <summary>
        /// The caller has acquired the lock.
        /// </summary>
        Acquired,
    }
}
