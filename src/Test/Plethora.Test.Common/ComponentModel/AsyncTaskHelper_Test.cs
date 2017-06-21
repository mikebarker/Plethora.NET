using System;
using System.Threading;
using NUnit.Framework;
using Plethora.ComponentModel;

namespace Plethora.Test.ComponentModel
{
    [TestFixture]
    public class AsyncTaskHelper_Test
    {
        private SynchronizeInvokeEx synchronizeInvoke;

        [SetUp]
        public void SetUp()
        {
            synchronizeInvoke = new SynchronizeInvokeEx();
        }

        [TearDown]
        public void TearDown()
        {
            synchronizeInvoke.Dispose();
        }

        #region GetValue

        [Test]
        public void GetValue()
        {
            //exec
            var result = AsyncTaskHelper.GetValue(synchronizeInvoke, si => si.Identifier);

            //test
            Assert.AreEqual(synchronizeInvoke.Identifier, result);
        }

        #endregion

        #region Exec

        [Test]
        public void Exec()
        {
            //setup
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            ManualResetEventSlim actionEvent = new ManualResetEventSlim();
            Action action = delegate
            {
                threadIdAction = Thread.CurrentThread.ManagedThreadId;
                actionEvent.Set();
            };

            //exec
            AsyncTaskHelper.Exec(synchronizeInvoke, action);
            actionEvent.Wait();

            //test
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        [Test]
        public void Exec_TimeOut()
        {
            //setup
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;

            int threadIdAction = 0;
            Action action = delegate
            {
                threadIdAction = Thread.CurrentThread.ManagedThreadId;
                Thread.Sleep(100);
            };

            //exec
            bool result = AsyncTaskHelper.Exec(synchronizeInvoke, action, 10);

            //test
            Assert.IsFalse(result);
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        #endregion

        #region AsyncTask

        [Test]
        public void AsyncTask()
        {
            //setup
            int threadIdInvoke = Thread.CurrentThread.ManagedThreadId;
            
            int threadIdAction = 0;
            Action action = delegate
                                {
                                    threadIdAction = Thread.CurrentThread.ManagedThreadId;
                                };

            //exec
            IAsyncResult result = AsyncTaskHelper.AsyncTask(synchronizeInvoke, action);
            result.AsyncWaitHandle.WaitOne();

            //test
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
        }

        [Test]
        public void AsyncTask_OnComplete()
        {
            //setup
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

            //exec
            IAsyncResult result = AsyncTaskHelper.AsyncTask(synchronizeInvoke, action, onComplete);
            result.AsyncWaitHandle.WaitOne();
            onCompleteWaitHandle.Wait();

            //test
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(0, threadIdOnComplete);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
            Assert.AreNotEqual(threadIdInvoke, threadIdOnComplete);
        }

        [Test]
        public void AsyncTask_OnException()
        {
            //setup
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
            ManualResetEventSlim onCompleteWaitHandle = new ManualResetEventSlim();
            Action onComplete = delegate
                                {
                                    threadIdOnComplete = Thread.CurrentThread.ManagedThreadId;
                                    onCompleteWaitHandle.Set();
                                };

            //exec
            IAsyncResult result = AsyncTaskHelper.AsyncTask(synchronizeInvoke, action, onComplete, onException);
            result.AsyncWaitHandle.WaitOne();
            onCompleteWaitHandle.Wait();

            //test
            Assert.AreNotEqual(0, threadIdAction);
            Assert.AreNotEqual(0, threadIdOnComplete);
            Assert.AreNotEqual(0, threadIdOnException);
            Assert.AreNotEqual(threadIdInvoke, threadIdAction); //Confirm the action was completed off-thread.
            Assert.AreNotEqual(threadIdAction, threadIdOnComplete);
            Assert.AreNotEqual(threadIdAction, threadIdOnException);
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
}
