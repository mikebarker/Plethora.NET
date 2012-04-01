﻿using System;
using System.Threading;
using NUnit.Framework;

namespace Plethora.Timing.Test
{
    [TestFixture]
    public class OperationTimeout_Test
    {
        [Test]
        public void RemainingTimeMilliseconds_ctor_Fail_NegativeTimeout()
        {
            try
            {
                var timeout = new OperationTimeout(-7);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void RemainingTimeMilliseconds_Remaining()
        {
            //setup
            const int timeoutMs = 500;
            var timeout = new OperationTimeout(timeoutMs);

            //exec
            const int sleeptimeMs = 20;
            Thread.Sleep(sleeptimeMs);

            //test
            int remaining = timeout.Remaining;

            const double THRESHHOLD = 0.01; //Allow for one percent error, due to timing for Thread.Sleep
            int lLimit = (int) Math.Floor((timeoutMs - sleeptimeMs)*(1 - THRESHHOLD));
            int uLimit = (int) Math.Ceiling((timeoutMs - sleeptimeMs)*(1 + THRESHHOLD));

            bool isRemainingApproxEqual = (lLimit <= remaining) && (remaining <= uLimit);
            Assert.True(isRemainingApproxEqual);
        }

        [Test]
        public void RemainingTimeMilliseconds_Remaining_Elapsed()
        {
            //setup
            var timeout = new OperationTimeout(0);

            //exec
            Thread.Sleep(1);

            //test
            Assert.AreEqual(0, timeout.Remaining);
        }

        [Test]
        public void RemainingTimeMilliseconds_Remaining_Infinite()
        {
            //setup
            var timeout = new OperationTimeout(Timeout.Infinite);

            //exec
            const int sleeptimeMs = 20;
            Thread.Sleep(sleeptimeMs);

            //test
            int remaining = timeout.Remaining;

            Assert.AreEqual(Timeout.Infinite, remaining);
        }

        [Test]
        public void RemainingTimeMilliseconds_HasElapsed_False()
        {
            //setup
            const int timeoutMs = 500;
            var timeout = new OperationTimeout(timeoutMs);

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsFalse(timeout.HasElapsed);
        }

        [Test]
        public void RemainingTimeMilliseconds_HasElapsed_True()
        {
            //setup
            var timeout = new OperationTimeout(0);

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsTrue(timeout.HasElapsed);
        }

        [Test]
        public void RemainingTimeMilliseconds_HasElapsed_Infinite()
        {
            //setup
            var timeout = new OperationTimeout(Timeout.Infinite);

            //exec
            Thread.Sleep(1);

            //test
            Assert.IsFalse(timeout.HasElapsed);
        }

    }
}