using System;
using System.Collections.Generic;
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
