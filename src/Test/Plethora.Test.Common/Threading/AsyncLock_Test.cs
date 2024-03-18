using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test._UtilityClasses;
using Plethora.Threading;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Test.Threading
{
    [TestClass]
    public class AsyncLock_Test
    {
        [TestMethod]
        public async Task TryLockAsync_AcquireLockResult()
        {
            // Arrange
            AsyncLock asyncLock = new AsyncLock();

            // Action
            var lockResult = await asyncLock.TryLockAsync(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(lockResult);
            Assert.IsTrue(lockResult.IsLockAcquired);
            Assert.IsNotNull(lockResult.LockObject);
        }

        [TestMethod]
        public async Task TryLockAsync_ConcurrentLock_Waits()
        {
            // Arrange
            AsyncLock asyncLock = new AsyncLock();

            var lockObject = await asyncLock.LockAsync().ConfigureAwait(false);

            // Action
            var lockResult = await asyncLock.TryLockAsync(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(lockResult);
            Assert.IsFalse(lockResult.IsLockAcquired);
            Assert.IsNull(lockResult.LockObject);

            GC.KeepAlive(lockObject);
        }

        [TestMethod]
        public async Task TryLockAsync_DisposeLockObject_RelesaesLock()
        {
            // Arrange
            AsyncLock asyncLock = new AsyncLock();

            using (var lockObject = await asyncLock.LockAsync().ConfigureAwait(false))
            {
            }

            // Action
            var lockResult = await asyncLock.TryLockAsync(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(lockResult);
            Assert.IsTrue(lockResult.IsLockAcquired);
            Assert.IsNotNull(lockResult.LockObject);
        }

        /* TEST IS NOT COMPLETING *
        [TestMethod]
        public async Task TryLockAsync_LockDisposed_WaitCancelled()
        {
            // Arrange
            AsyncLock asyncLock = new AsyncLock();

            var lockObject = await asyncLock.LockAsync().ConfigureAwait(false);
            var tryLockTask = asyncLock.TryLockAsync(TimeSpan.FromMilliseconds(1_000));

            // Action
            asyncLock.Dispose();

            try
            {
                await tryLockTask.ConfigureAwait(false);

                // Assert
                Assert.Fail("OperationCanceledException was not thrown.");
            }
            catch (OperationCanceledException)
            {
                // If the exception is caught the test has succeeded, there is no further asserts required.
            }

            GC.KeepAlive(lockObject);
        }
        /**/

        [TestMethod]
        public async Task TryLockAsync_LockRegister_Contexts()
        {
            // Arrange
            LockRegister register = new LockRegister(TimeSpan.FromMilliseconds(1));
            AsyncLock asyncLock = new AsyncLock(register);


            // Action - Acquire a lock
            var lockObject = await asyncLock.LockAsync().ConfigureAwait(false);

            // Assert
            var lockContexts = register.GetLockContexts();
            Assert.AreEqual(1, lockContexts.Count);
            Assert.AreEqual(asyncLock, lockContexts.First().Lock);
            Assert.AreEqual(LockRequestStatus.Acquired, lockContexts.First().LockRequestStatus);


            // Action - Await a new lock
            var task = asyncLock.TryLockAsync(Timeout.InfiniteTimeSpan);

            // Assert
            lockContexts = register.GetLockContexts();
            var lockContextsList = lockContexts.OrderBy(c => c.SourceLineNumber).ToList();

            Assert.AreEqual(2, lockContextsList.Count);
            Assert.AreEqual(asyncLock, lockContextsList[0].Lock);
            Assert.AreEqual(LockRequestStatus.Acquired, lockContextsList[0].LockRequestStatus);

            Assert.AreEqual(asyncLock, lockContextsList[1].Lock);
            Assert.AreEqual(LockRequestStatus.Awaiting, lockContextsList[1].LockRequestStatus);


            // Action - Release the original lock
            lockObject.Release();
            var result = await task.ConfigureAwait(false);

            // Assert
            lockContexts = register.GetLockContexts();
            Assert.AreEqual(1, lockContexts.Count);
            Assert.AreEqual(asyncLock, lockContexts.First().Lock);
            Assert.AreEqual(LockRequestStatus.Acquired, lockContexts.First().LockRequestStatus);


            // Action - Release all locks
            ((IDisposable)result).Dispose();

            // Assert
            lockContexts = register.GetLockContexts();
            Assert.AreEqual(0, lockContexts.Count);
        }

        [TestMethod]
        public async Task TryLockAsync_LockRegister_LongWaitDetection()
        {
            // Arrange
            LockRegister register = new LockRegister(TimeSpan.FromMilliseconds(1));
            AsyncLock asyncLock = new AsyncLock(register);

            bool isLongWaitLockDetected = false;
            LongWaitDetectedEventArgs eventArgs = null;
            register.LongWaitDetected += (sender, e) =>
            {
                isLongWaitLockDetected = true;
                eventArgs = e;
            };

            var lockObject = await asyncLock.LockAsync().ConfigureAwait(false);
            var task = asyncLock.TryLockAsync(Timeout.InfiniteTimeSpan);

            // Action
            bool result = Wait.For(() => isLongWaitLockDetected, TimeSpan.FromMilliseconds(100));

            // Assert
            Assert.IsTrue(result);

            Assert.IsNotNull(eventArgs.LongWaitingLockContext);
            Assert.AreEqual(asyncLock, eventArgs.LongWaitingLockContext.Lock);
            Assert.AreEqual(LockRequestStatus.Awaiting, eventArgs.LongWaitingLockContext.LockRequestStatus);
            Assert.AreEqual(GetMemberName(), eventArgs.LongWaitingLockContext.MemberName);
            Assert.AreEqual(GetSourceFilePath(), eventArgs.LongWaitingLockContext.SourceFilePath);

            Assert.AreEqual(1, eventArgs.ApplicationLockContexts.Count);
            Assert.AreEqual(asyncLock, eventArgs.ApplicationLockContexts.First().Lock);
            Assert.AreEqual(LockRequestStatus.Acquired, eventArgs.ApplicationLockContexts.First().LockRequestStatus);
            Assert.AreEqual(GetMemberName(), eventArgs.ApplicationLockContexts.First().MemberName);
            Assert.AreEqual(GetSourceFilePath(), eventArgs.ApplicationLockContexts.First().SourceFilePath);

            Assert.IsTrue(eventArgs.ApplicationLockContexts.First().SourceLineNumber < eventArgs.LongWaitingLockContext.SourceLineNumber);
        }

        private static string GetMemberName(
            [CallerMemberName] string memberName = null)
        {
            return memberName;
        }

        private static string GetSourceFilePath(
            [CallerFilePath] string memberName = null)
        {
            return memberName;
        }
    }
}
