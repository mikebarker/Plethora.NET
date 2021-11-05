using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.ComponentModel;

namespace Plethora.Test.ComponentModel
{
#pragma warning disable CS0618 // Type or member is obsolete

    [TestClass]
    public class AsynchronousHelper_Test
    {
        private SynchronizeInvokeEx synchronizeInvoke = new SynchronizeInvokeEx();

        [TestCleanup]
        public void TearDown()
        {
            synchronizeInvoke.Dispose();
        }

        #region GetValue

        [TestMethod]
        public void GetValue()
        {
            // Action
            var result = AsynchronousHelper.GetValue(synchronizeInvoke, si => si.Identifier);

            // Assert
            Assert.AreEqual(synchronizeInvoke.Identifier, result);
        }

        #endregion

        #region Execute

        [TestMethod]
        public void Execute()
        {
            // Arrange
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            ManualResetEventSlim actionEvent = new ManualResetEventSlim();
            Action action = delegate
            {
                threadIdAction = Thread.CurrentThread.ManagedThreadId;
                actionEvent.Set();
            };

            // Action
            AsynchronousHelper.Execute(synchronizeInvoke, action);
            actionEvent.Wait();

            // Assert
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        [TestMethod]
        public void Execute_TimeOut()
        {
            // Arrange
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            Action action = delegate
            {
                threadIdAction = Thread.CurrentThread.ManagedThreadId;
                Thread.Sleep(100);
            };

            // Action
            bool result = AsynchronousHelper.Execute(synchronizeInvoke, action, TimeSpan.FromMilliseconds(10));

            // Assert
            Assert.IsFalse(result);
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        #endregion

        #region AsyncExecute

        [TestMethod]
        public void AsyncExecute()
        {
            // Arrange
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;
            
            int threadIdAction = 0;
            Action action = delegate
                                {
                                    threadIdAction = Thread.CurrentThread.ManagedThreadId;
                                };

            // Action
            IAsyncResult result = AsynchronousHelper.AsyncExecute(synchronizeInvoke, action);
            result.AsyncWaitHandle.WaitOne();

            // Assert
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        [TestMethod]
        public void AsyncExecute_OnComplete()
        {
            // Arrange
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            Action action = delegate
                                {
                                    threadIdAction = Thread.CurrentThread.ManagedThreadId;
                                };

            int threadIdOnComplete = 0;
            ManualResetEventSlim onCompleteWaitHandle = new ManualResetEventSlim();
            Action onComplete = delegate
                                {
                                    threadIdOnComplete = Thread.CurrentThread.ManagedThreadId;
                                    onCompleteWaitHandle.Set();
                                };

            // Action
            IAsyncResult result = AsynchronousHelper.AsyncExecute(synchronizeInvoke, action, onComplete);
            result.AsyncWaitHandle.WaitOne();
            onCompleteWaitHandle.Wait();

            // Assert
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(0, threadIdOnComplete);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
            Assert.AreNotEqual(threadIdInvoke, threadIdOnComplete);
        }

        [TestMethod]
        public void AsyncExecute_OnException()
        {
            // Arrange
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            Exception exceptionThrown = new Exception();
            Action action = delegate
                                {
                                    threadIdAction = Thread.CurrentThread.ManagedThreadId;
                                    throw exceptionThrown;
                                };

            int threadIdOnException = 0;
            Exception exceptionCaught = null;
            Action<Exception> onException = delegate(Exception ex)
                                {
                                    threadIdOnException = Thread.CurrentThread.ManagedThreadId;
                                    exceptionCaught = ex;
                                };

            int threadIdOnComplete = 0;
            Action onComplete = delegate
                                {
                                    threadIdOnComplete = Thread.CurrentThread.ManagedThreadId;
                                };

            // Action
            IAsyncResult result = AsynchronousHelper.AsyncExecute(synchronizeInvoke, action, onComplete, onException);
            result.AsyncWaitHandle.WaitOne();

            // Assert
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(0, threadIdOnException);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
            Assert.AreNotEqual(threadIdAction, threadIdOnException);
            Assert.AreEqual(0, threadIdOnComplete); // onComplete not run
            Assert.AreEqual(exceptionThrown, exceptionCaught);
        }

        #endregion


        #region InnerClass

        private class SynchronizeInvokeEx : SynchronizeInvoke
        {
            private static readonly Random rnd = new Random();
            private readonly int identifier = rnd.Next();

            public int Identifier
            {
                get { return identifier; }
            }
        }
        #endregion
    }
#pragma warning restore CS0618 // Type or member is obsolete
}