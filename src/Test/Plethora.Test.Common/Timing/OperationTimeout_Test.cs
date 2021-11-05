using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Timing;

namespace Plethora.Test.Timing
{
    [TestClass]
    public class OperationTimeout_Test
    {
        [TestMethod]
        public void RemainingTimeMilliseconds_ctor_Fail_NegativeTimeout()
        {
            try
            {
                var timeout = new OperationTimeout(TimeSpan.FromMilliseconds(-7));

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        [Ignore("Test relies on unreliable timings.")]
        public void RemainingTimeMilliseconds_Remaining()
        {
            //setup
            var timeout = new OperationTimeout(TimeSpan.FromMilliseconds(500));

            //exec
            const int sleeptimeMs = 20;
            Thread.Sleep(sleeptimeMs);

            //test
            var remainingMs = timeout.Remaining.TotalMilliseconds;

            bool isRemainingApproxEqual = (465 <= remainingMs) && (remainingMs <= 480); //Allow for error, due to timing for Thread.Sleep
            Assert.IsTrue(isRemainingApproxEqual, $"Expected remaining time to be approx 480, but was {remainingMs}");
        }

        [TestMethod]
        public void RemainingTimeMilliseconds_Remaining_Elapsed()
        {
            //setup
            var timeout = new OperationTimeout(TimeSpan.Zero);

            //exec
            Thread.Sleep(1);

            //test
            Assert.AreEqual(TimeSpan.Zero, timeout.Remaining);
        }

        [TestMethod]
        public void RemainingTimeMilliseconds_Remaining_Infinite()
        {
            //setup
            var timeout = new OperationTimeout(Timeout.InfiniteTimeSpan);

            //exec
            const int sleeptimeMs = 20;
            Thread.Sleep(sleeptimeMs);

            //test
            TimeSpan remaining = timeout.Remaining;

            Assert.AreEqual(Timeout.InfiniteTimeSpan, remaining);
        }

        [TestMethod]
        public void RemainingTimeMilliseconds_HasElapsed_False()
        {
            //setup
            var timeout = new OperationTimeout(TimeSpan.FromMilliseconds(500));

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsFalse(timeout.HasElapsed);
        }

        [TestMethod]
        public void RemainingTimeMilliseconds_HasElapsed_True()
        {
            //setup
            var timeout = new OperationTimeout(TimeSpan.Zero);

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsTrue(timeout.HasElapsed);
        }

        [TestMethod]
        public void RemainingTimeMilliseconds_HasElapsed_Infinite()
        {
            //setup
            var timeout = new OperationTimeout(Timeout.InfiniteTimeSpan);

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsFalse(timeout.HasElapsed);
        }

    }
}
