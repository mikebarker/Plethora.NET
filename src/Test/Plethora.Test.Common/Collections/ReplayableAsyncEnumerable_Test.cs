using System;
using System.Collections.Generic;
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
        bool sourceDisposed = false;
        var enumerable = new ReplayableAsyncEnumerable<int>(
            GetValues(() => sourceDisposed = true));
        var enumerator = enumerable.GetAsyncEnumerator();

        Assert.IsTrue(await enumerator.MoveNextAsync());
        await enumerator.DisposeAsync();
        Assert.IsFalse(sourceDisposed);

        await enumerable.DisposeAsync();

        Assert.IsTrue(sourceDisposed);

        var replayEnumerator = enumerable.GetAsyncEnumerator();
        await Assert.ThrowsAsync<ObjectDisposedException>(
            () => replayEnumerator.MoveNextAsync().AsTask());
    }

    private static async IAsyncEnumerable<int> GetPendingValue(
        TaskCompletionSource sourceStarted,
        Task releaseSource)
    {
        sourceStarted.SetResult();
        await releaseSource.ConfigureAwait(false);
        yield return 1;
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
