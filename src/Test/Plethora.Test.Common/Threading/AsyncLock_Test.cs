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

        [TestMethod]
        public async Task TryLockAsync_LockRegister()
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
            _ = Task.Run(() => asyncLock.TryLockAsync(Timeout.InfiniteTimeSpan));

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
