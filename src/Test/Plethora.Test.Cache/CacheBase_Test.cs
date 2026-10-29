using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plethora.Cache;

namespace Plethora.Test.Cache
{
    [TestClass]
    public class CacheBase_Test
    {
        [TestMethod]
        public async Task GetData_CacheEmpty_DataRequestedFromSource()
        {
            // Arrange
            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => Task.FromResult(GetDataOrNull(id)));

            Cache cache = new Cache(dataSourceMock.Object);

            // Action
            var result = await cache.GetDataAsync(3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().Id);
            Assert.AreEqual("three", result.First().Value);

            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);
        }

        [TestMethod]
        public async Task GetData_CacheMiss_DataRequestedFromSource()
        {
            // Arrange
            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => Task.FromResult(GetDataOrNull(id)));

            Cache cache = new Cache(dataSourceMock.Object);

            await cache.GetDataAsync(1).ConfigureAwait(false);
            await cache.GetDataAsync(2).ConfigureAwait(false);
            await cache.GetDataAsync(4).ConfigureAwait(false);
            dataSourceMock.Invocations.Clear();

            // Action
            var result = await cache.GetDataAsync(3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().Id);
            Assert.AreEqual("three", result.First().Value);

            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);
        }

        [TestMethod]
        public async Task GetData_CacheHit_DataRequestedFromSource()
        {
            // Arrange
            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => Task.FromResult(GetDataOrNull(id)));

            Cache cache = new Cache(dataSourceMock.Object);

            await cache.GetDataAsync(3).ConfigureAwait(false);
            dataSourceMock.Invocations.Clear();

            // Action
            var result = await cache.GetDataAsync(3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().Id);
            Assert.AreEqual("three", result.First().Value);

            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Never);
        }

        [TestMethod]
        public async Task GetData_ConcurrentRequests_DataRequestedFromSource()
        {
            // Arrange
            TaskCompletionSource returnDataTcs = new();

            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(async id =>
                {
                    await returnDataTcs.Task;
                    return GetDataOrNull(id);
                });

            Cache cache = new Cache(dataSourceMock.Object);

            // Action
            List<Task> tasks = new();
            for (int i = 0; i < 2; i++)
            {
                var task = cache.GetDataAsync(3);
                tasks.Add(task);
            }

            foreach (var task in tasks)
            {
                Assert.IsFalse(task.IsCompleted);
            }

            returnDataTcs.SetResult();

            foreach (var task in tasks)
            {
                await task;
            }

            // Assert
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);
        }

        [TestMethod]
        public async Task GetData_Exception_Rethrown()
        {
            // Arrange
            var getDataException = new CacheException();

            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => throw getDataException);

            Cache cache = new Cache(dataSourceMock.Object);

            // Action
            CacheException caughtException = null;
            try
            {
                await cache.GetDataAsync(3).ConfigureAwait(false);

                Assert.Fail();
            }
            catch (CacheException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.AreSame(getDataException, caughtException);
        }

        [TestMethod]
        public async Task GetData_ExceptionCacheHit_Rethrown()
        {
            // Arrange
            var getDataException = new CacheException();

            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => throw getDataException);

            Cache cache = new Cache(dataSourceMock.Object);

            // Ensure an exception is cached
            try { await cache.GetDataAsync(3).ConfigureAwait(false); }
            catch { }

            // Action
            CacheException caughtException = null;
            try
            {
                await cache.GetDataAsync(3).ConfigureAwait(false);

                Assert.Fail();
            }
            catch (CacheException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.AreSame(getDataException, caughtException);
        }

        [TestMethod]
        public async Task DropDataAsync()
        {
            // Arrange
            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => Task.FromResult(GetDataOrNull(id)));

            Cache cache = new Cache(dataSourceMock.Object);

            await cache.GetDataAsync(3).ConfigureAwait(false);
            dataSourceMock.Invocations.Clear();

            // Action
            await cache.DropDataAsync(3).ConfigureAwait(false);

            var result = await cache.GetDataAsync(3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().Id);
            Assert.AreEqual("three", result.First().Value);

            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);
        }

        [TestMethod]
        [DataRow(false)]
        [DataRow(true)]
        public async Task DropData_InFlightRequest_CompletesOnAnotherThreadAndAllowsReload(bool sourceFails)
        {
            var sourceResult = new TaskCompletionSource<Data>(TaskCreationOptions.RunContinuationsAsynchronously);
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .SetupSequence(m => m.GetDataAsync(3))
                .Returns(sourceResult.Task)
                .ReturnsAsync(GetDataOrNull(3));
            Cache cache = new(dataSourceMock.Object);

            var loading = cache.GetDataAsync(3);
            var dropStarted = new TaskCompletionSource<Task>(TaskCreationOptions.RunContinuationsAsynchronously);
            Thread dropThread = new(() => dropStarted.SetResult(cache.DropDataAsync(3)))
            {
                IsBackground = true
            };
            dropThread.Start();
            var dropping = await dropStarted.Task.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Assert.IsFalse(dropping.IsCompleted);

            if (sourceFails)
            {
                var exception = new TimeoutException();
                sourceResult.SetException(exception);
                var caughtException = await Assert.ThrowsAsync<TimeoutException>(
                    () => loading.WaitAsync(TimeSpan.FromSeconds(5))).ConfigureAwait(false);
                Assert.AreSame(exception, caughtException);
            }
            else
            {
                sourceResult.SetResult(GetDataOrNull(3));
                await loading.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            }

            await dropping.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            var result = await cache.GetDataAsync(3).WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            Assert.AreEqual(3, result.Single().Id);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Exactly(2));
        }

        [TestMethod]
        public async Task GetData_DuringDrop_WaitsAsynchronouslyAndReloadsDroppedData()
        {
            var sourceResult = new TaskCompletionSource<Data>(TaskCreationOptions.RunContinuationsAsynchronously);
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .SetupSequence(m => m.GetDataAsync(3))
                .Returns(sourceResult.Task)
                .ReturnsAsync(GetDataOrNull(3));
            Cache cache = new(dataSourceMock.Object);

            var loading = cache.GetDataAsync(3);
            var dropping = cache.DropDataAsync(3);
            var reloading = cache.GetDataAsync(3);

            Assert.IsFalse(dropping.IsCompleted);
            Assert.IsFalse(reloading.IsCompleted);

            sourceResult.SetResult(GetDataOrNull(3));
            await Task.WhenAll(loading, dropping, reloading)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            Assert.AreEqual(3, (await reloading.ConfigureAwait(false)).Single().Id);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Exactly(2));
        }

        [TestMethod]
        public async Task DropData_RequestContinuationClearsCache_DoesNotBlockCompletion()
        {
            var sourceResult = new TaskCompletionSource<Data>(TaskCreationOptions.RunContinuationsAsynchronously);
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .SetupSequence(m => m.GetDataAsync(3))
                .Returns(sourceResult.Task)
                .ReturnsAsync(GetDataOrNull(3));
            Cache cache = new(dataSourceMock.Object);

            var loading = cache.GetDataAsync(3);
            var clearing = loading.ContinueWith(
                _ => cache.ClearCache(),
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
            var dropping = cache.DropDataAsync(3);

            sourceResult.SetResult(GetDataOrNull(3));
            await Task.WhenAll(loading, dropping, clearing)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            var result = await cache.GetDataAsync(3).WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Assert.AreEqual(3, result.Single().Id);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Clear()
        {
            // Arrange
            Mock<IDataSource> dataSourceMock = new Mock<IDataSource>();
            dataSourceMock
                .Setup(m => m.GetDataAsync(It.IsAny<long>()))
                .Returns<long>(id => Task.FromResult(GetDataOrNull(id)));

            Cache cache = new Cache(dataSourceMock.Object);

            await cache.GetDataAsync(3).ConfigureAwait(false);
            dataSourceMock.Invocations.Clear();

            // Action
            cache.ClearCache();

            var result = await cache.GetDataAsync(3).ConfigureAwait(false);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, result.First().Id);
            Assert.AreEqual("three", result.First().Value);

            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);
        }

        [TestMethod]
        public async Task Clear_InFlightRequest_DoesNotRepopulateCacheWithStaleData()
        {
            var sourceResult = new TaskCompletionSource<Data>(TaskCreationOptions.RunContinuationsAsynchronously);
            var staleData = new Data(3, "stale");
            var freshData = new Data(3, "fresh");
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .SetupSequence(m => m.GetDataAsync(3))
                .Returns(sourceResult.Task)
                .ReturnsAsync(freshData);
            Cache cache = new(dataSourceMock.Object);

            var loading = cache.GetDataAsync(3);
            Assert.IsFalse(loading.IsCompleted);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);

            cache.ClearCache();
            var freshResult = await cache.GetDataAsync(3)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Assert.AreSame(freshData, freshResult.Single());

            sourceResult.SetResult(staleData);
            var originalResult = await loading.WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Assert.AreSame(freshData, originalResult.Single());

            var result = (await cache.GetDataAsync(3)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false)).ToArray();

            Assert.AreEqual(1, result.Length, "The pre-clear load must not reinsert stale data.");
            Assert.AreSame(freshData, result[0]);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Clear_InFlightRequest_ReloadsDataForWaitingCallers()
        {
            var sourceResult = new TaskCompletionSource<Data>(TaskCreationOptions.RunContinuationsAsynchronously);
            var freshData = new Data(3, "fresh");
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .SetupSequence(m => m.GetDataAsync(3))
                .Returns(sourceResult.Task)
                .ReturnsAsync(freshData);
            Cache cache = new(dataSourceMock.Object);

            var loading = cache.GetDataAsync(3);
            var joining = cache.GetDataAsync(3);
            Assert.IsFalse(loading.IsCompleted);
            Assert.IsFalse(joining.IsCompleted);
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Once);

            cache.ClearCache();
            sourceResult.SetResult(new Data(3, "stale"));

            var results = await Task.WhenAll(loading, joining)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            foreach (var result in results)
            {
                Assert.AreSame(freshData, result.Single());
            }
            var cachedResult = await cache.GetDataAsync(3)
                .WaitAsync(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            Assert.AreSame(freshData, cachedResult.Single());
            dataSourceMock.Verify(m => m.GetDataAsync(3), Times.Exactly(2));
        }

        [TestMethod]
        public async Task Clear_PreviouslyReturnedData_RemainsAvailable()
        {
            Mock<IDataSource> dataSourceMock = new();
            dataSourceMock
                .Setup(m => m.GetDataAsync(3))
                .ReturnsAsync(GetDataOrNull(3));
            Cache cache = new(dataSourceMock.Object);

            var result = await cache.GetDataAsync(3).ConfigureAwait(false);
            cache.ClearCache();

            Assert.AreEqual(3, result.Single().Id);
        }

        #region Private Members

        class CacheException: Exception
        {
        }

        private static readonly Dictionary<long, string> knownPairs = new()
        {
            { 0, "zero" },
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
            { 4, "four" },
            { 5, "five" },
        };

        private static Data GetDataOrNull(long id)
        {
            if (knownPairs.TryGetValue(id, out string value))
                return new Data(id, value);
            else
                return null;
        }

        public interface IDataSource
        {
            Task<Data> GetDataAsync(long id);
        }

        class Cache : CacheBase<Data, IdArg>
        {
            private readonly IDataSource dataSource;

            public Cache(IDataSource dataSource)
            {
                this.dataSource = dataSource;
            }

            public async Task<IEnumerable<Data>> GetDataAsync(params long[] ids)
            {
                var args = ids.Select(id => new IdArg(id));
                var pairs = await base.GetDataAsync(args).ConfigureAwait(false);
                return pairs;
            }

            public async Task DropDataAsync(params long[] ids)
            {
                var args = ids.Select(id => new IdArg(id));
                await base.DropDataAsync(args).ConfigureAwait(false);
            }


            public void ClearCache()
            {
                base.Clear();
            }

            #region Overrides of CacheBase<Pair,IdArg>

            protected override async Task<IEnumerable<Data>> GetDataFromSourceAsync(
                IEnumerable<IdArg> arguments,
                CancellationToken cancellationToken)
            {
                List<Data> results = new();
                foreach (var arg in arguments)
                {
                    var data = await this.dataSource.GetDataAsync(arg.Id).ConfigureAwait(false);
                    if (data != null)
                    {
                        results.Add(data);
                    }
                }
                return results;
            }

            #endregion
        }

        public class Data
        {
            public Data(long id, string value)
            {
                this.Id = id;
                this.Value = value;
            }

            public long Id { get; }
            public string Value { get; }
        }

        class IdArg : IArgument<Data, IdArg>
        {
            public IdArg(long id)
            {
                this.Id = id;
            }

            public long Id { get; }

            #region Implementation of IArgument<Pair,IdArg>

            bool IArgument<Data, IdArg>.IsOverlapped(IdArg B, out IEnumerable<IdArg> notInB)
            {
                notInB = null; //null (could be empty enumeration) in the case when true is returned (Ids match),
                               //and ignorred in the case where false is returned.

                return (B.Id == this.Id);
            }

            bool IArgument<Data, IdArg>.IsDataIncluded(Data data)
            {
                return (data.Id == this.Id);
            }
            #endregion
        }

        #endregion
    }
}
