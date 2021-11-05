using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.ComponentModel;
using Plethora.Test.UtilityClasses;
using Plethora.Threading;

namespace Plethora.Test.ComponentModel
{
    [TestClass]
    public class SynchronizeInvoke_Test
    {
        private readonly SynchronizeInvoke synchronizeInvoke = new SynchronizeInvoke();

        #region BeginInvoke

        [TestMethod]
        public void BeginInvoke()
        {
            // Action
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<int>(GetNextInt),
                null);

            // Assert
            Assert.IsNotNull(asyncResult);
        }

        [TestMethod]
        public void BeginInvoke_Arguments()
        {
            // Arrange
            var o = new object();

            // Action
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<object, object>(ReturnInstance),
                new[] {o});

            // Assert
            Assert.IsNotNull(asyncResult);
        }

        [TestMethod]
        public void BeginInvoke_Fail_NullMethod()
        {
            try
            {
                // Action
                synchronizeInvoke.BeginInvoke(null, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        #endregion

        #region EndInvoke

        [TestMethod]
        public void EndInvoke()
        {
            // Arrange
            var o = new object();
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Func<object, object>(ReturnInstance),
                new[] {o});

            // Action
            object result = synchronizeInvoke.EndInvoke(asyncResult);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(o, result);
        }

        [TestMethod]
        public void EndInvoke_OnException()
        {
            // Arrange
            var asyncResult = synchronizeInvoke.BeginInvoke(
                new Action(ThrowException),
                null);

            // Action
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

            // Assert
            Assert.IsNotNull(exception);
            Assert.IsNotNull(exception.InnerException);
        }

        [TestMethod]
        public void EndInvoke_Fail_Null()
        {
            try
            {
                // Action
                synchronizeInvoke.EndInvoke(null);
                Assert.Fail();
            }
            catch(ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void EndInvoke_Fail_InvalidAsyncResult()
        {
            try
            {
                // Action
                synchronizeInvoke.EndInvoke(new EmptyAsyncResult());
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }
        #endregion

        [TestMethod]
        public void Invoke()
        {
            // Action
            object result = synchronizeInvoke.Invoke(new Func<int>(GetNextInt), null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is int);
        }

        [TestMethod]
        [Ignore("Test takes too long to run, and relies on unreliable timings.")]
        public void MultipleExecutions()
        {
            // Arrange
            const int ITTERATIONS = 16;
            var stopwatch = new Stopwatch();

            var asyncResultList = new List<IAsyncResult>(ITTERATIONS);

            // Action
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
