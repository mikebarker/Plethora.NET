using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Threading;

namespace Plethora.Test.Threading;

[TestClass]
public class WorkQueue_Test
{
    [TestMethod]
    public void Abort_CompletesQueuedWorkAsCancelled()
    {
        using var queue = new WorkQueue(1);
        using var runningWorkStarted = new ManualResetEventSlim();
        using var releaseRunningWork = new ManualResetEventSlim();
        using var runningWorkFinished = new ManualResetEventSlim();
        int queuedWorkExecutionCount = 0;

        _ = queue.BeginInvoke(
            new Action(() =>
            {
                runningWorkStarted.Set();
                try
                {
                    releaseRunningWork.Wait();
                }
                finally
                {
                    runningWorkFinished.Set();
                }
            }),
            null);

        Assert.IsTrue(runningWorkStarted.Wait(TimeSpan.FromSeconds(5)));
        var queuedWork = queue.BeginInvoke(
            new Action(() => Interlocked.Increment(ref queuedWorkExecutionCount)),
            null);

        try
        {
            queue.Abort();

            Assert.IsTrue(queuedWork.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(5)));
            var exception = Assert.Throws<AsyncException>(() => queue.EndInvoke(queuedWork));
            Assert.IsInstanceOfType<OperationCanceledException>(exception.InnerException);
            Assert.AreEqual(0, queuedWorkExecutionCount);
        }
        finally
        {
            releaseRunningWork.Set();
            Assert.IsTrue(runningWorkFinished.Wait(TimeSpan.FromSeconds(5)));
        }
    }

    [TestMethod]
    public void BeginInvoke_AfterAbort_ThrowsInvalidOperationException()
    {
        using var queue = new WorkQueue(1);
        queue.Abort();

        Assert.Throws<InvalidOperationException>(
            () => queue.BeginInvoke(new Action(() => { }), null));
    }
}
