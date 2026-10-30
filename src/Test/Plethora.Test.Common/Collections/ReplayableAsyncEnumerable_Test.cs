using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections;

[TestClass]
public class ReplayableAsyncEnumerable_Test
{
    [TestMethod]
    public async Task EnumerateToEnd_DisposesSourceAndAllowsReplay()
    {
        bool sourceDisposed = false;
        var enumerable = new ReplayableAsyncEnumerable<int>(
            GetValues(() => sourceDisposed = true));

        List<int> firstEnumeration = new();
        await foreach (int value in enumerable)
        {
            firstEnumeration.Add(value);
        }

        Assert.IsTrue(sourceDisposed);
        CollectionAssert.AreEqual(new[] { 1, 2 }, firstEnumeration);

        List<int> replay = new();
        await foreach (int value in enumerable)
        {
            replay.Add(value);
        }

        CollectionAssert.AreEqual(firstEnumeration, replay);
        await enumerable.DisposeAsync();
    }

    [TestMethod]
    public async Task Current_AfterEnd_ThrowsInvalidOperationException()
    {
        // Arrange
        await using var enumerable = new ReplayableAsyncEnumerable<int>(GetValues(() => { }));
        await using var enumerator = enumerable.GetAsyncEnumerator();

        bool result;
        do
        {
            result = await enumerator.MoveNextAsync();
        } while (result);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => { var current = enumerator.Current; });
    }

    [TestMethod]
    public async Task MoveNextAsync_AfterEnd_ContinuesReturningFalse()
    {
        // Arrange
        await using var enumerable = new ReplayableAsyncEnumerable<int>(GetValues(() => { }));
        await using var enumerator = enumerable.GetAsyncEnumerator();

        // Act & Assert
        Assert.IsTrue(await enumerator.MoveNextAsync());
        Assert.IsTrue(await enumerator.MoveNextAsync());
        Assert.IsFalse(await enumerator.MoveNextAsync());

        Assert.IsFalse(await enumerator.MoveNextAsync());
        Assert.IsFalse(await enumerator.MoveNextAsync());
    }

    [TestMethod]
    public async Task MoveNextAsync_CancelWhileSourcePending_CancelsOnlyThatReader()
    {
        // Arrange
        var sourceStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var enumerable = new ReplayableAsyncEnumerable<int>(
            GetPendingValue(sourceStarted, releaseSource.Task));
        using var cancellation = new CancellationTokenSource();
        await using var cancelledEnumerator = enumerable.GetAsyncEnumerator(cancellation.Token);
        await using var otherEnumerator = enumerable.GetAsyncEnumerator();

        // Action
        Task<bool> cancelledMove = cancelledEnumerator.MoveNextAsync().AsTask();
        Task<bool> otherMove = otherEnumerator.MoveNextAsync().AsTask();
        try
        {
            await sourceStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            cancellation.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => cancelledMove.WaitAsync(TimeSpan.FromSeconds(2)));
            Assert.IsFalse(otherMove.IsCompleted);
        }
        finally
        {
            releaseSource.TrySetResult();
        }

        // Assert
        Assert.IsTrue(await otherMove.WaitAsync(TimeSpan.FromSeconds(5)));
        Assert.AreEqual(1, otherEnumerator.Current);
    }

    [TestMethod]
    public async Task DisposeAsync_BeforeEnd_DisposesSource()
    {
        // Arrange
        bool sourceDisposed = false;
        var enumerable = new ReplayableAsyncEnumerable<int>(
            GetValues(() => sourceDisposed = true));
        var enumerator = enumerable.GetAsyncEnumerator();

        Assert.IsTrue(await enumerator.MoveNextAsync());

        await enumerator.DisposeAsync();
        Assert.IsFalse(sourceDisposed);

        await enumerable.DisposeAsync();
        Assert.IsTrue(sourceDisposed);
        Assert.Throws<ObjectDisposedException>(() => enumerable.GetAsyncEnumerator());
    }

    [TestMethod]
    public async Task DisposeAsync_WhileSourcePending_CancelsSourceAndAllWaitingReaders()
    {
        var sourceStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        int disposeCount = 0;
        var enumerable = new ReplayableAsyncEnumerable<int>(
            GetPendingValue(sourceStarted, releaseSource.Task, () => Interlocked.Increment(ref disposeCount)));
        await using var first = enumerable.GetAsyncEnumerator();
        await using var second = enumerable.GetAsyncEnumerator();

        Task<bool> firstMove = first.MoveNextAsync().AsTask();
        Task<bool> secondMove = second.MoveNextAsync().AsTask();
        try
        {
            await sourceStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));

            Task firstDispose = enumerable.DisposeAsync().AsTask();
            Task secondDispose = enumerable.DisposeAsync().AsTask();
            await Task.WhenAll(firstDispose, secondDispose).WaitAsync(TimeSpan.FromSeconds(5));

            await Assert.ThrowsAsync<OperationCanceledException>(() => firstMove);
            await Assert.ThrowsAsync<OperationCanceledException>(() => secondMove);
            Assert.AreEqual(1, disposeCount);
            await enumerable.DisposeAsync();
            Assert.AreEqual(1, disposeCount);
        }
        finally
        {
            releaseSource.TrySetResult();
            await enumerable.DisposeAsync();
        }
    }

    [TestMethod]
    public async Task DisposeAsync_SourceIgnoresCancellation_WaitsBeforeDisposingSource()
    {
        var sourceStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        bool sourceDisposed = false;
        var enumerable = new ReplayableAsyncEnumerable<int>(
            GetPendingValue(sourceStarted, releaseSource.Task, () => sourceDisposed = true, observeCancellation: false));
        await using var enumerator = enumerable.GetAsyncEnumerator();

        Task<bool> move = enumerator.MoveNextAsync().AsTask();
        Task disposal = null;
        try
        {
            await sourceStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            disposal = enumerable.DisposeAsync().AsTask();
            Task secondDisposal = enumerable.DisposeAsync().AsTask();

            Assert.IsFalse(disposal.IsCompleted);
            Assert.IsFalse(secondDisposal.IsCompleted);
            Assert.IsFalse(sourceDisposed);
            disposal = Task.WhenAll(disposal, secondDisposal);
        }
        finally
        {
            releaseSource.TrySetResult();
            await (disposal ?? enumerable.DisposeAsync().AsTask()).WaitAsync(TimeSpan.FromSeconds(5));
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => move);
        Assert.IsTrue(sourceDisposed);
    }

    [TestMethod]
    public async Task MoveNextAsync_TerminalCleanupThrows_AllReadersObserveSameFailure()
    {
        var failure = new InvalidOperationException("Terminal cleanup failed.");
        var disposalStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseDisposal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var enumerable = new ReplayableAsyncEnumerable<int>(
            new TerminalCleanupFailingSource(disposalStarted, releaseDisposal.Task, failure));
        await using var first = enumerable.GetAsyncEnumerator();
        await using var second = enumerable.GetAsyncEnumerator();

        Task<bool> firstMove = first.MoveNextAsync().AsTask();
        try
        {
            await disposalStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            Task<bool> secondMove = second.MoveNextAsync().AsTask();
            Assert.IsFalse(secondMove.IsCompleted, "Readers should wait until terminal cleanup completes.");
            releaseDisposal.SetResult();

            var firstFailure = await Assert.ThrowsAsync<InvalidOperationException>(
                () => firstMove.WaitAsync(TimeSpan.FromSeconds(5)));
            Assert.AreSame(failure, firstFailure);

            var secondFailure = await Assert.ThrowsAsync<InvalidOperationException>(
                () => secondMove.WaitAsync(TimeSpan.FromSeconds(5)));
            Assert.AreSame(failure, secondFailure);

            await using var replay = enumerable.GetAsyncEnumerator();
            var replayFailure = await Assert.ThrowsAsync<InvalidOperationException>(
                () => replay.MoveNextAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5)));
            Assert.AreSame(failure, replayFailure);
        }
        finally
        {
            releaseDisposal.TrySetResult();
            try
            {
                await enumerable.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreSame(failure, ex);
            }
        }
    }

    [TestMethod]
    public async Task DisposeAsync_SourceFails_PreservesFailure()
    {
        var failure = new InvalidOperationException("Source failed.");
        var enumerable = new ReplayableAsyncEnumerable<int>(GetFailure(failure));
        await using var enumerator = enumerable.GetAsyncEnumerator();

        var moveFailure = await Assert.ThrowsAsync<InvalidOperationException>(
            () => enumerator.MoveNextAsync().AsTask());
        var disposeFailure = await Assert.ThrowsAsync<InvalidOperationException>(
            () => enumerable.DisposeAsync().AsTask());

        Assert.AreSame(failure, moveFailure);
        Assert.AreSame(failure, disposeFailure);
    }

    [TestMethod]
    public async Task MoveNextAsync_SourceAndCleanupThrow_PreservesBothFailuresForReplayAndDisposal()
    {
        var sourceFailure = new InvalidOperationException("Source failed.");
        var cleanupFailure = new InvalidOperationException("Cleanup failed.");
        var source = new SourceAndCleanupFailingSource(sourceFailure, cleanupFailure);
        var enumerable = new ReplayableAsyncEnumerable<int>(source);
        await using var first = enumerable.GetAsyncEnumerator();

        var moveFailure = await Assert.ThrowsAsync<AggregateException>(
            () => first.MoveNextAsync().AsTask());
        Assert.AreEqual(2, moveFailure.InnerExceptions.Count);
        Assert.AreSame(sourceFailure, moveFailure.InnerExceptions[0]);
        Assert.AreSame(cleanupFailure, moveFailure.InnerExceptions[1]);

        await using var replay = enumerable.GetAsyncEnumerator();
        var replayFailure = await Assert.ThrowsAsync<AggregateException>(
            () => replay.MoveNextAsync().AsTask());
        Assert.AreSame(moveFailure, replayFailure);

        var disposeFailure = await Assert.ThrowsAsync<AggregateException>(
            () => enumerable.DisposeAsync().AsTask());
        Assert.AreSame(moveFailure, disposeFailure);
        Assert.AreEqual(1, source.DisposeCount);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task DisposeAsync_SourceCurrentAndCleanupThrow_PreservesBothFailures(bool sourceCancelled)
    {
        Exception sourceFailure = sourceCancelled
            ? new OperationCanceledException(new CancellationToken(canceled: true))
            : new InvalidOperationException("Source Current failed.");
        var cleanupFailure = new InvalidOperationException("Cleanup failed.");
        var source = new SourceAndCleanupFailingSource(sourceFailure, cleanupFailure, failOnCurrent: true);
        var enumerable = new ReplayableAsyncEnumerable<int>(source);
        await using var enumerator = enumerable.GetAsyncEnumerator();

        var moveFailure = await Assert.ThrowsAsync<Exception>(
            () => enumerator.MoveNextAsync().AsTask());
        Assert.AreSame(sourceFailure, moveFailure);

        var disposeFailure = await Assert.ThrowsAsync<AggregateException>(
            () => enumerable.DisposeAsync().AsTask());
        Assert.AreEqual(2, disposeFailure.InnerExceptions.Count);
        Assert.AreSame(sourceFailure, disposeFailure.InnerExceptions[0]);
        Assert.AreSame(cleanupFailure, disposeFailure.InnerExceptions[1]);
        Assert.AreEqual(1, source.DisposeCount);
    }

    [TestMethod]
    public async Task DisposeAsync_SourceCancelledIndependently_PreservesCancellation()
    {
        var failure = new OperationCanceledException(new CancellationToken(canceled: true));
        var enumerable = new ReplayableAsyncEnumerable<int>(GetFailure(failure));
        await using var enumerator = enumerable.GetAsyncEnumerator();

        var moveFailure = await Assert.ThrowsAsync<OperationCanceledException>(
            () => enumerator.MoveNextAsync().AsTask());
        await enumerable.DisposeAsync();

        Assert.AreSame(failure, moveFailure);
    }

    [TestMethod]
    public async Task DisposeAsync_SourceCleanupThrowsCancellation_PreservesCleanupFailure()
    {
        var sourceStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var enumerable = new ReplayableAsyncEnumerable<int>(
            new CancellationFailingDisposeSource(sourceStarted, releaseSource.Task));

        await using var enumerator = enumerable.GetAsyncEnumerator();
        Task<bool> move = enumerator.MoveNextAsync().AsTask();
        try
        {
            await sourceStarted.Task.WaitAsync(TimeSpan.FromSeconds(5));
            var disposeFailure = await Assert.ThrowsAsync<AggregateException>(
                () => enumerable.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5)));
            Assert.AreEqual(2, disposeFailure.InnerExceptions.Count);
            Assert.IsInstanceOfType<OperationCanceledException>(disposeFailure.InnerExceptions[0]);
            Assert.IsInstanceOfType<OperationCanceledException>(disposeFailure.InnerExceptions[1]);
            Assert.AreEqual("Cleanup failed.", disposeFailure.InnerExceptions[1].Message);

            var moveFailure = await Assert.ThrowsAsync<Exception>(
                () => move.WaitAsync(TimeSpan.FromSeconds(5)));
            if (moveFailure is AggregateException)
                Assert.AreSame(disposeFailure, moveFailure);
            else
                Assert.IsInstanceOfType<OperationCanceledException>(moveFailure);
        }
        finally
        {
            releaseSource.TrySetResult();
        }
    }

    private sealed class SourceAndCleanupFailingSource(
        Exception sourceFailure,
        Exception cleanupFailure,
        bool failOnCurrent = false) : IAsyncEnumerable<int>, IAsyncEnumerator<int>
    {
        public int DisposeCount { get; private set; }

        public int Current => throw sourceFailure;

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return failOnCurrent
                ? ValueTask.FromResult(true)
                : ValueTask.FromException<bool>(sourceFailure);
        }

        public ValueTask DisposeAsync()
        {
            this.DisposeCount++;
            return ValueTask.FromException(cleanupFailure);
        }
    }

    private sealed class TerminalCleanupFailingSource(
        TaskCompletionSource disposalStarted,
        Task releaseDisposal,
        Exception failure) : IAsyncEnumerable<int>, IAsyncEnumerator<int>
    {
        public int Current => throw new InvalidOperationException();

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return this;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(false);
        }

        public async ValueTask DisposeAsync()
        {
            disposalStarted.SetResult();
            await releaseDisposal.ConfigureAwait(false);
            throw failure;
        }
    }

    private sealed class CancellationFailingDisposeSource(
        TaskCompletionSource sourceStarted,
        Task releaseSource) : IAsyncEnumerable<int>, IAsyncEnumerator<int>
    {
        private CancellationToken cancellationToken;

        public int Current => 1;

        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            this.cancellationToken = cancellationToken;
            return this;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            sourceStarted.SetResult();
            await releaseSource.WaitAsync(this.cancellationToken);
            return true;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.FromException(
                new OperationCanceledException("Cleanup failed.", this.cancellationToken));
        }
    }

    private static async IAsyncEnumerable<int> GetPendingValue(
        TaskCompletionSource sourceStarted,
        Task releaseSource,
        Action onDispose = null,
        bool observeCancellation = true,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        try
        {
            sourceStarted.SetResult();
            await releaseSource.WaitAsync(observeCancellation ? cancellationToken : default).ConfigureAwait(false);
            yield return 1;
        }
        finally
        {
            onDispose?.Invoke();
        }
    }

    private static async IAsyncEnumerable<int> GetFailure(Exception failure)
    {
        yield return await Task.FromException<int>(failure);
    }

    private static async IAsyncEnumerable<int> GetValues(Action onDispose)
    {
        try
        {
            yield return 1;
            await Task.Yield();
            yield return 2;
        }
        finally
        {
            onDispose();
        }
    }
}
