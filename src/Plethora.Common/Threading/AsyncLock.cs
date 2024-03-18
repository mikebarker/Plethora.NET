using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Threading
{
    /// <summary>
    /// Represents a lock that limits the number of threads that can access a resource or pool of resources concurrently.
    /// </summary>
    /// <remarks>
    /// <para>
    ///   The lock may be held over an awaited async call.
    /// </para>
    /// <para>
    ///   Utilises the using and <see cref="IDisposable"/> pattern to control the lifetime of locks.
    /// </para>
    /// <para>
    ///   Initialise the <see cref="LockRegister.DefaultInstance"/> prior to creating a <see cref="AsyncLock"/>
    ///   in order to participate in long-wait detection.
    /// </para>
    /// </remarks>
    /// <example>
    /// Locks should be acquired, tested as follows:
    /// <code><![CDATA[
    /// class MyClass
    /// {
    ///     private readonly AsyncLock asyncLock = new AsyncLock();
    /// 
    ///     public async Task MethodAsync()
    ///     {
    ///         using (var lockResult = this.asyncLock.TryLockAsync(TimeSpan.FromSeconds(2)).ConfigureAwait(false))
    ///         {
    ///             if (lockResult.IsLockAcquired)
    ///             {
    ///                 // ... Some asynchronous code ...
    ///             }
    ///         }
    ///     }
    /// 
    ///     public void Method()
    ///     {
    ///         using (var lockObject = this.asyncLock.Lock())
    ///         {
    ///             // ... Some synchronous code ...
    ///         }
    ///     }
    /// 
    ///     // ... SNIP ...
    /// }
    /// ]]></code>
    /// </example>
    public class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim semaphore = new(1, 1);
        private readonly string name;
        private readonly LockRegister? register;
        private readonly CancellationTokenSource disposedCancellationTokenSource = new();

        /// <summary>
        /// Initialise a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        /// <param name="memberName">The caller member name.</param>
        /// <param name="sourceFilePath">The caller source file path.</param>
        /// <param name="sourceLineNumber">The caller source file number.</param>
        /// <remarks>
        /// If the <see cref="LockRegister.DefaultInstance"/> has been set this lock will participate in long-wait
        /// detection using the default instance; otherwise it will not.
        /// </remarks>
        public AsyncLock(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            : this(GetDefaultLockName(memberName, sourceFilePath, sourceLineNumber), LockRegister.DefaultInstance)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        /// <param name="register">
        /// The <see cref="LockRegister"/> with which this instance will participate in long-wait detection.
        /// Specifying null will prevent this instance from participating in long-wait detection.
        /// </param>
        /// <param name="memberName">The caller member name.</param>
        /// <param name="sourceFilePath">The caller source file path.</param>
        /// <param name="sourceLineNumber">The caller source file number.</param>
        public AsyncLock(
            LockRegister? register,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            : this(GetDefaultLockName(memberName, sourceFilePath, sourceLineNumber), register)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="AsyncLock"/>.</param>
        /// <remarks>
        /// If the <see cref="LockRegister.DefaultInstance"/> has been set this lock will participate in long-wait
        /// detection using the default instance; otherwise it will not.
        /// </remarks>
        public AsyncLock(
            string name)
            : this(name, LockRegister.DefaultInstance)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="AsyncLock"/>.</param>
        /// <param name="register">
        /// Optional. The <see cref="LockRegister"/> with which this instance will participate in long-wait detection.
        /// Specifying null will prevent this instance from participating in long-wait detection.
        /// </param>
        public AsyncLock(
            string name,
            LockRegister? register)
        {
            ArgumentNullException.ThrowIfNull(name);

            this.name = name;
            this.register = register;
        }

        #region Implementation of IDisposable

        private bool disposed;

        ~AsyncLock()
        {
            this.Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            this.disposedCancellationTokenSource.Cancel();

            if (disposing)
            {
                // Dispose the semaphore after cancelling the disposedCancellationTokenSource,
                // otherwise a deadlock results.
                this.semaphore.Dispose();
            }

            this.disposed = true;
        }

        #endregion

        /// <summary>
        /// Blocks the current thread until it can enter the lock, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>
        /// A <see cref="LockObject"/> represents the lock acquired.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="AsyncLock"/> instance has been disposed.
        /// -or-
        /// The <see cref="CancellationTokenSource"/> that created <paramref name="cancellationToken"/> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was canceled.
        /// </exception>
        public LockObject? Lock(
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            ObjectDisposedException.ThrowIf(this.disposed, this);

            var callerData = new CallerData(memberName, sourceFilePath, sourceLineNumber);

            LockResult result = this.InternalTryLock(
                Timeout.InfiniteTimeSpan,
                cancellationToken,
                callerData);

            return result.ExtractLockObject();
        }

        /// <summary>
        /// Blocks the current thread until it can enter the lock, using a <see cref="TimeSpan"/> that specifies
        /// the timeout, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> that represents the time to wait, a <see cref="TimeSpan"/> that
        /// represents -1 milliseconds (e.g. <see cref="Timeout.InfiniteTimeSpan"/>) to wait indefinitely,
        /// or a <see cref="TimeSpan"/> that represents 0 milliseconds (e.g. <see cref="TimeSpan.Zero"/>)
        /// to test the wait handle and return immediately.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>
        /// A <see cref="LockResult"/> where <see cref="LockResult.IsLockAcquired"/> is true if the lock was
        /// acquired; otherwise false. <see cref="LockResult.LockObject"/> represents the lock acquired.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> represents a negative number other than -1.
        /// -or-
        /// <paramref name="timeout"/> represents a millisecond value greater than <see cref="Int32.MaxValue"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="AsyncLock"/> instance has been disposed.
        /// -or-
        /// The <see cref="CancellationTokenSource"/> that created <paramref name="cancellationToken"/> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was canceled.
        /// </exception>
        public LockResult TryLock(
            TimeSpan timeout,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            ObjectDisposedException.ThrowIf(this.disposed, this);

            var callerData = new CallerData(memberName, sourceFilePath, sourceLineNumber);

            return this.InternalTryLock(
                timeout,
                cancellationToken,
                callerData);
        }

        /// <summary>
        /// Asynchronously waits to enter the lock, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>
        /// A <see cref="Task"/> that will complete with a <see cref="LockObject"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> represents a negative number other than -1.
        /// -or-
        /// <paramref name="timeout"/> represents a millisecond value greater than <see cref="Int32.MaxValue"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="AsyncLock"/> instance has been disposed.
        /// -or-
        /// The <see cref="CancellationTokenSource"/> that created <paramref name="cancellationToken"/> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was canceled.
        /// </exception>
        public async Task<LockObject?> LockAsync(
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            ObjectDisposedException.ThrowIf(this.disposed, this);

            var callerData = new CallerData(memberName, sourceFilePath, sourceLineNumber);

            LockResult result = await this.InternalTryLockAsync(
                Timeout.InfiniteTimeSpan,
                cancellationToken,
                callerData).ConfigureAwait(false);

            return result.ExtractLockObject();
        }

        /// <summary>
        /// Asynchronously waits to enter the lock, using a <see cref="TimeSpan"/> to measure the
        /// time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="timeout">
        /// A <see cref="TimeSpan"/> that represents the time to wait, a <see cref="TimeSpan"/> that
        /// represents -1 milliseconds (e.g. <see cref="Timeout.InfiniteTimeSpan"/>) to wait indefinitely,
        /// or a <see cref="TimeSpan"/> that represents 0 milliseconds (e.g. <see cref="TimeSpan.Zero"/>)
        /// to test the wait handle and return immediately.
        /// </param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> token to observe.</param>
        /// <returns>
        /// A <see cref="Task"/> that will complete with a <see cref="LockResult"/> where
        /// <see cref="LockResult.IsLockAcquired"/> has a value of true if the lock was acquired;
        /// otherwise false. <see cref="LockResult.LockObject"/> represents the lock acquired.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timeout"/> represents a negative number other than -1.
        /// -or-
        /// <paramref name="timeout"/> represents a millisecond value greater than <see cref="Int32.MaxValue"/>.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The <see cref="AsyncLock"/> instance has been disposed.
        /// -or-
        /// The <see cref="CancellationTokenSource"/> that created <paramref name="cancellationToken"/> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// <paramref name="cancellationToken"/> was canceled.
        /// </exception>
        public Task<LockResult> TryLockAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken = default,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (this.disposed)
                throw new ObjectDisposedException(nameof(AsyncLock));

            var callerData = new CallerData(memberName, sourceFilePath, sourceLineNumber);

            return this.InternalTryLockAsync(
                timeout,
                cancellationToken,
                callerData);
        }

        private LockResult InternalTryLock(
            TimeSpan timeout,
            CancellationToken cancellationToken,
            CallerData callerData)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var disposedToken = this.disposedCancellationTokenSource.Token;
            disposedToken.ThrowIfCancellationRequested();

            bool isLockAcquired;

            using (var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disposedToken))
            using (this.RegisterAwait(callerData))
            {
                isLockAcquired = this.semaphore.Wait(timeout, combinedTokenSource.Token);
            }

            return this.CreateLockResult(isLockAcquired, callerData);
        }

        private async Task<LockResult> InternalTryLockAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken,
            CallerData callerData)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var disposedToken = this.disposedCancellationTokenSource.Token;
            disposedToken.ThrowIfCancellationRequested();

            bool isLockAcquired;

            using (var combinedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, disposedToken))
            using (this.RegisterAwait(callerData))
            {
                isLockAcquired = await this.semaphore.WaitAsync(timeout, combinedTokenSource.Token).ConfigureAwait(false);
            }

            return this.CreateLockResult(isLockAcquired, callerData);
        }

        private LockResult CreateLockResult(
            bool isLockAcquired,
            CallerData callerData)
        {
            if (isLockAcquired)
            {
                var acquiredRegisterDisposable = this.RegisterAcquired(callerData);

                var lockResult = LockResult.Acquired(() => { this.ReleaseLock(); acquiredRegisterDisposable?.Dispose(); });
                return lockResult;
            }
            else
            {
                var lockResult = LockResult.NotAcquired();
                return lockResult;
            }
        }

        private void ReleaseLock()
        {
            if (this.disposed)
                return;

            this.semaphore.Release();
        }

        private static string GetDefaultLockName(string memberName, string sourceFilePath, int sourceLineNumber)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            return $"{fileName}__{sourceLineNumber:00}";
        }

        private IDisposable? RegisterAwait(
            CallerData callerData)
        {
            if (this.register == null)
            {
                return null;
            }

            return this.register.RegisterAwaitingLock(this, callerData.MemberName, callerData.SourceFilePath, callerData.SourceLineNumber);
        }

        private IDisposable? RegisterAcquired(
            CallerData callerData)
        {
            if (this.register == null)
            {
                return null;
            }

            return this.register.RegisterAcquiredLock(this, callerData.MemberName, callerData.SourceFilePath, callerData.SourceLineNumber);
        }

        private readonly struct CallerData(string memberName, string sourceFilePath, int sourceLineNumber)
        {
            public readonly string MemberName = memberName;
            public readonly string SourceFilePath = sourceFilePath;
            public readonly int SourceLineNumber = sourceLineNumber;
        }
    }
}
