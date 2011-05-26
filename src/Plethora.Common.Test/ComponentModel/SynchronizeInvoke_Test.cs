using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using Plethora.Test.UtilityClasses;
using Plethora.Threading;

namespace Plethora.ComponentModel.Test
{
    [TestFixture]
    public class SynchronizeInvoke_Test
    {
        private SynchronizeInvoke synchronizeInvoke;

        [SetUp]
        public void SetUp()
        {
            this.synchronizeInvoke = new SynchronizeInvoke();
        }

        #region BeginInvoke

        [Test]
        public void BeginInvoke()
        {
            //exec
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<int>(GetNextInt),
                null);

            //test
            Assert.IsNotNull(asyncResult);
        }

        [Test]
        public void BeginInvoke_Arguments()
        {
            //setup
            var o = new object();

            //exec
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<object, object>(ReturnInstance),
                new[] {o});

            //test
            Assert.IsNotNull(asyncResult);
        }

        [Test]
        public void BeginInvoke_Fail_NullMethod()
        {
            try
            {
                //exec
                synchronizeInvoke.BeginInvoke(null, null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        #endregion

        #region EndInvoke

        [Test]
        public void EndInvoke()
        {
            //setup
            var o = new object();
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<object, object>(ReturnInstance),
                new[] {o});

            //exec
            object result = synchronizeInvoke.EndInvoke(asyncResult);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual(o, result);
        }

        [Test]
        public void EndInvoke_OnException()
        {
            //setup
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Action(ThrowException),
                null);

            //exec
            Exception exception = null;
            try
            {
                synchronizeInvoke.EndInvoke(asyncResult);
                Assert.Fail();
            }
            catch (AsyncException ex)
            {
                exception = ex;
            }

            //test
            Assert.IsNotNull(exception);
            Assert.IsNotNull(exception.InnerException);
        }

        [Test]
        public void EndInvoke_Fail_Null()
        {
            try
            {
                //exec
                synchronizeInvoke.EndInvoke(null);
                Assert.Fail();
            }
            catch(ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void EndInvoke_Fail_InvalidAsyncResult()
        {
            try
            {
                //exec
                synchronizeInvoke.EndInvoke(new EmptyAsyncResult());
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        [Test]
        public void MultipleExecutions()
        {
            //setup
            const int ITTERATIONS = 64;
            var stopwatch = new Stopwatch();

            var asyncResultList = new List<IAsyncResult>(ITTERATIONS);

            //exec
            stopwatch.Reset();
            stopwatch.Start();
            for (int j = 0; j < ITTERATIONS; j++)
            {
                var asyncResult = synchronizeInvoke.BeginInvoke(new Func<int>(GetNextIntWait), null);
                asyncResultList.Add(asyncResult);
            }
            stopwatch.Stop();
            long time = stopwatch.ElapsedMilliseconds;

            Assert.IsTrue(time < 100);

            var waitHandles = asyncResultList
                .Select(async => async.AsyncWaitHandle)
                .ToArray();
            WaitHandle.WaitAll(waitHandles);

            //Ensure the invokes executed in order
            int prevResult = -1;
            foreach (var asyncResult in asyncResultList)
            {
                int result = (int)synchronizeInvoke.EndInvoke(asyncResult);

                //Special case for first result
                if (prevResult == -1)
                    Assert.IsTrue(result > prevResult);
                else
                    Assert.AreEqual(prevResult + 1, result);

                prevResult = result;
            }
        }



        #region Private Methods

        private static int i = 0;

        private int GetNextInt()
        {
            return i++;
        }

        private int GetNextIntWait()
        {
            var result = GetNextInt();
            Thread.Sleep(100);
            return result;
        }

        private object ReturnInstance(object obj)
        {
            return obj;
        }

        private void ThrowException()
        {
            throw new Exception("Exception");
        }
        #endregion
    }
}
