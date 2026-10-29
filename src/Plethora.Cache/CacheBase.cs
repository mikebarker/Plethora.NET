using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Plethora.Linq;
using Plethora.Threading;

namespace Plethora.Cache;

//TODO: Optimise: Use better classes than lists for the requests and data
public abstract class CacheBase<TData, TArgument>
    where TArgument : notnull, IArgument<TData, TArgument>
{
    private class Request
    {
        #region Fields

        private readonly TArgument argument;
        private readonly TaskCompletionSource<object?> taskCompletionSource;

        #endregion

        #region Constructors

        public Request(TArgument argument)
        {
            ArgumentNullException.ThrowIfNull(argument);

            this.taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
            this.argument = argument;
        }
        #endregion

        #region Properties

        public TArgument Argument
        {
            get { return this.argument; }
        }

        public Task Task
        {
            get { return this.taskCompletionSource.Task; }
        }

        #endregion

        #region Methods

        internal void Success()
        {
            this.taskCompletionSource.SetResult(null);
        }

        internal void Fail(Exception ex)
        {
            this.taskCompletionSource.SetException(ex);
        }
        #endregion
    }


    #region Fields

    private readonly AsyncLock requestsLock = new();
    private List<Request> submittedRequests = new();

    private readonly ReaderWriterLockSlim dataLock = new(LockRecursionPolicy.NoRecursion);
    private readonly List<TData> data = new();

    // Readers must hold either requestsLock or dataLock; writers must hold both locks.
    private long generation;

    #endregion

    #region Entry Methods

    /// <summary>
    /// Retrieves data from the cache or if data is not available within the cache it is
    /// retrieved from the source and cached.
    /// </summary>
    /// <param name="arguments">The <typeparamref name="TArgument"/>s representing the data required.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> token to observe.</param>
    /// <returns>
    /// The data requested, as described by <paramref name="arguments"/>.
    /// </returns>
    protected async Task<IEnumerable<TData>> GetDataAsync(IEnumerable<TArgument> arguments, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (TryGetNonEnumeratedCount(arguments, out var count) && (count == 0))
        {
            return Enumerable.Empty<TData>();
        }

        arguments = UniqueArgs(arguments);

        List<Request> submitted;
        List<Request> required;
        long generation;

        using (await this.requestsLock.LockAsync(cancellationToken).ConfigureAwait(false))
        {
            generation = this.generation;
            (submitted, required) = this.GetRequests(arguments);

            if (required.Count != 0)
            {
                this.submittedRequests.AddRange(required);
            }
        }

        //Load the data in another thread.
        if (required.Count != 0)
        {
            // TODO: Exceptions are unobserved.
            _ = this.CacheDataForRequestsAsync(required, generation, CancellationToken.None);
        }

        IEnumerable<Request> requests;
        if ((submitted.Count != 0) && (required.Count != 0))
            requests = submitted.Union(required);
        else if (required.Count != 0)
            requests = required;
        else if (submitted.Count != 0)
            requests = submitted;
        else
            requests = Enumerable.Empty<Request>(); // Can occur if the arguments are empty.

        foreach (var request in requests)
        {
            await request.Task.WaitAsync(cancellationToken).ConfigureAwait(false);
        }

        cancellationToken.ThrowIfCancellationRequested();

        // The data should now have been cached filter the cache to find the requested data.
        this.dataLock.EnterReadLock();
        try
        {
            if (generation != this.generation)
            {
                // The generation has changed since the data was loaded.
                // This means that the data loaded may not be valid for the current generation, and therefore should not be returned.
                return Enumerable.Empty<TData>();
            }

            return FilterDataSetByArgument(
                this.data,
                arguments).ToArray();
        }
        finally
        {
            this.dataLock.ExitReadLock();
        }
    }

    protected async Task DropDataAsync(IEnumerable<TArgument> arguments)
    {
        using (await this.requestsLock.LockAsync().ConfigureAwait(false))
        {
            List<Request> remainingRequests;
            List<Request> nextRemainingRequests = this.submittedRequests;

            //Remove the requests first then the data.
            foreach (TArgument dropArgument in arguments)
            {
                remainingRequests = nextRemainingRequests;

                int count = remainingRequests.Count;
                int capacity = count + (count >> 2); // Add 25%
                nextRemainingRequests = new List<Request>(capacity);

                //Drop (dividing where necesasary) any requests which pertain to the
                // drop arguments
                foreach (Request originalRequest in remainingRequests)
                {
                    if (originalRequest.Argument.IsOverlapped(dropArgument, out var remainingArgs))
                    {
                        //The request must complete before it can "divided".
                        // It must also complete before the dataLock is taken for
                        // data removal, otherwise a race condition develops where the 
                        // retrieved data for the request may only be inserted after
                        // the delete has occurred. This would result in data existing
                        // in the cache without a corresponding request.
                        // Therefore the await can not be called inside the
                        // "if (remainingArgs != null)" block
                        Exception? originalRequestException;
                        try
                        {
                            await originalRequest.Task.ConfigureAwait(false);
                            originalRequestException = null;
                        }
                        catch (Exception ex)
                        {
                            // Do not copy recoverable failures into remainder requests, allow those regions to be reloaded.
                            if (this.IsExceptionRecoverable(ex))
                            {
                                continue;
                            }

                            originalRequestException = ex;
                        }

                        //Create remainder requests from the remainder args, duplicating the state
                        // of the orginal argument
                        if (remainingArgs != null)
                        {
                            foreach (TArgument remainingArg in remainingArgs)
                            {
                                Request remainingRequest = new(remainingArg);

                                //Test if the original request had been successful, or failed
                                if (originalRequestException is null)
                                    remainingRequest.Success();
                                else
                                    remainingRequest.Fail(originalRequestException);

                                nextRemainingRequests.Add(remainingRequest);
                            }
                        }
                    }
                    else
                    {
                        nextRemainingRequests.Add(originalRequest);
                    }
                }
            }

            this.submittedRequests = nextRemainingRequests;

            //All WaitOne() calls on the requests must be completed before locking the data
            // as this could otherwise result in a deadlock (where this thread holds the
            // write-lock on data, and is waiting for the request to complete... and another
            // thread is waiting for the write lock on data before marking the request as
            // complete).
            this.dataLock.EnterWriteLock();
            try
            {
                foreach (TArgument dropArgument in arguments)
                {
                    //Drop the data
                    this.data.RemoveAll(dropArgument.IsDataIncluded);
                }
            }
            finally
            {
                this.dataLock.ExitWriteLock();
            }
        }
    }

    protected void Clear()
    {
        using (this.requestsLock.Lock())
        {
            this.dataLock.EnterWriteLock();
            try
            {
                this.generation++;
                this.data.Clear();
            }
            finally
            {
                this.dataLock.ExitWriteLock();
            }

            this.submittedRequests.Clear();
        }
    }

    #endregion

    #region Virtual and Abstract Methods

    /// <summary>
    /// Fetches the required data from the data source.
    /// </summary>
    /// <param name="arguments">The <typeparamref name="TArgument"/>s describing the data required.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests.
    /// </param>
    /// <returns>
    /// An <see cref="IEnumerable{TData}"/> containing the data required from the source, as described
    /// by <paramref name="arguments"/>.
    /// </returns>
    /// <remarks>
    ///  <para>
    ///   This method may return more data than that requested, or the exact data only. Further internal
    ///   filtering is conducted on the data returned before committing it to the cache.
    ///  </para>
    ///  <para>
    ///   The <see cref="IEnumerable{TData}"/> returned from this method may return a data stream, and
    ///   does not need to be concretised at the point of return. Each element of the enumeration will
    ///   be queried once only. i.e. A forward-only cursor maybe used to retrieve data.
    ///  </para>
    /// </remarks>
    protected abstract Task<IEnumerable<TData>> GetDataFromSourceAsync(IEnumerable<TArgument> arguments, CancellationToken cancellationToken);

    /// <summary>
    /// Tests whether an exception indicates that a subsiquent matching query may succeed.
    /// </summary>
    /// <param name="ex">The exeption to be tested.</param>
    /// <returns>
    /// True if the exception is recoverable; else false.
    /// </returns>
    /// <remarks>
    /// The default implementation will return true for <see cref="TimeoutException"/> as the source
    /// may be more responsive in future calls (e.g. unresponsive due to load, where load is expected
    /// to decrease).
    /// </remarks>
    protected virtual bool IsExceptionRecoverable(Exception ex)
    {
        if (ex is TimeoutException)
            return true;

        return false;
    }
    #endregion

    #region Private Methods

    private async Task CacheDataForRequestsAsync(IEnumerable<Request> requests, long generation, CancellationToken cancellationToken)
    {
        // Load the data for the 'required' requests.
        IReadOnlyCollection<TData> loadedData;
        try
        {
            var data = await this.GetDataForRequestsAsync(requests, cancellationToken).ConfigureAwait(false);
            loadedData = data.ToReadOnlyCollectionIfRequired();

            // Final test on the timeout. Once this try block is exited, all
            // data should committed, and the requests marked successful.
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (Exception ex)
        {
            // Mark all requests as failed.
            requests.ForEach(r => r.Fail(ex));

            // If the exception is recoverable, then remove the requests from
            // the submitted requests to allow future calls to attempt to
            // reload the data.
            if (this.IsExceptionRecoverable(ex))
            {
                using (await this.requestsLock.LockAsync().ConfigureAwait(false))
                {
                    foreach (var request in requests)
                    {
                        this.submittedRequests.Remove(request);
                    }
                }
            }

            throw;
        }

        // Add the loaded data to the cache
        this.dataLock.EnterWriteLock();
        try
        {
            if (generation == this.generation)
            {
                int size = this.data.Count + loadedData.Count;
                if (this.data.Capacity < size)
                    this.data.Capacity = size;
                this.data.AddRange(loadedData);
            }
        }
        finally
        {
            this.dataLock.ExitWriteLock();
        }

        // Mark the requests as successful
        // In the event of discarded requests (when generation changes) complete discarded requests too, so their callers can retry in the new generation.
        foreach (var request in requests)
        {
            request.Success();
        }
    }



    private async Task<IEnumerable<TData>> GetDataForRequestsAsync(
        IEnumerable<Request> requests,
        CancellationToken cancellationToken)
    {
        IEnumerable<TData> returnedData = await this.GetDataFromSourceAsync(
                requests.Select(request => request.Argument),
                cancellationToken)
            .ConfigureAwait(false);

        cancellationToken.ThrowIfCancellationRequested();

        //Ensure that each element of the data is retrieved once only.
        returnedData = returnedData.CacheResult();

        //More data may be returned than was required. Filter the data to ensure
        // only the data requested is returned.
        IEnumerable<TData> dataForRequests = FilterDataSetByArgument(
            returnedData,
            requests.Select(request => request.Argument));

        cancellationToken.ThrowIfCancellationRequested();

        return dataForRequests;
    }

    private (List<Request> submitted, List<Request> required) GetRequests(
        IEnumerable<TArgument> newArguments)
    {
        IEnumerable<TArgument> argumentList = newArguments;

        //Attempt to estimate the capacity required
        int nextCapacity = 4; // Default
        if (TryGetNonEnumeratedCount(argumentList, out var count))
        {
            if (count == 0)
            {
                return (new(), new());
            }

            nextCapacity = count + (count >> 2); // add 25% to the capacity
        }

        // requestsLock is already held.
        List<Request> submitted = new();
        foreach (var submittedRequest in this.submittedRequests)
        {
            var nextArgumentList = new List<TArgument>(nextCapacity);

            foreach (var argument in argumentList)
            {
                if (argument.IsOverlapped(submittedRequest.Argument, out var notSubmitted))
                {
                    submitted.Add(submittedRequest);

                    if ((notSubmitted != null) && (notSubmitted.Any()))
                        nextArgumentList.AddRange(notSubmitted);
                }
                else
                {
                    nextArgumentList.Add(argument);
                }
            }

            count = nextArgumentList.Count;
            nextCapacity = count + (count >> 2); // add 25% to the capacity

            argumentList = nextArgumentList;
        }

        List<Request> required = argumentList.Select(arg => new Request(arg)).ToList();
        return (submitted, required);
    }


    private IEnumerable<TArgument> UniqueArgs(IEnumerable<TArgument> arguments)
    {
        List<TArgument> uniqueArgs = new();
        while (true)
        {
            List<TArgument> nextArgs = new();
            foreach (var argument in arguments)
            {
                bool isArgumentHandled = false;
                foreach (var uniqueArg in uniqueArgs)
                {
                    if (argument.IsOverlapped(uniqueArg, out var notInUnique))
                    {
                        if (notInUnique is not null)
                        {
                            nextArgs.AddRange(notInUnique);
                        }

                        isArgumentHandled = true;
                        break;
                    }
                }

                if (!isArgumentHandled)
                {
                    uniqueArgs.Add(argument);
                }
            }

            if (nextArgs.Count == 0)
                break;

            arguments = nextArgs;
        }

        return uniqueArgs;
    }

    private static IEnumerable<TData> FilterDataSetByArgument(
        IEnumerable<TData> dataSet,
        IEnumerable<TArgument> filterArguments)
    {
        return dataSet
            .Where(dataItem => filterArguments.Any(filter => filter.IsDataIncluded(dataItem)));
    }

    private static bool TryGetNonEnumeratedCount(IEnumerable<TArgument> enumerable, out int count)
    {
        // LINQ's Enumerable.TryGetNonEnumeratedCount doesn't consider readonly collections.
        if (enumerable is IReadOnlyCollection<TArgument> readOnlyCollection)
        {
            count = readOnlyCollection.Count;
            return true;
        }

        return enumerable.TryGetNonEnumeratedCount(out count);
    }

    #endregion
}
