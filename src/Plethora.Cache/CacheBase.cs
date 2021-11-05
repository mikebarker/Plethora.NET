using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

using Plethora.Linq;

namespace Plethora.Cache
{
    //TODO: Optimise: Use better classes than lists for the requests and data
    public abstract class CacheBase<TData, TArgument>
        where TArgument : IArgument<TData, TArgument>
    {
        private class Request
        {
            #region Fields

            private readonly TArgument argument;
            private readonly TaskCompletionSource<object> taskCompletionSource;

            #endregion
            
            #region Constructors

            public Request([NotNull] TArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException(nameof(argument));

                this.taskCompletionSource = new TaskCompletionSource<object>();
                this.argument = argument;
            }
            #endregion

            #region Properties

            [NotNull]
            public TArgument Argument
            {
                get { return this.argument; }
            }

            [NotNull]
            public Task Task
            {
                get { return this.taskCompletionSource.Task; }
            }

            #endregion

            #region Methods

            internal void Success()
            {
                taskCompletionSource.SetResult(null);
            }

            internal void Fail([NotNull] Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
            #endregion
        }


        #region Fields

        private readonly ReaderWriterLockSlim requestsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private List<Request> submittedRequests = new List<Request>();

        private readonly ReaderWriterLockSlim dataLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly List<TData> data = new List<TData>();
        #endregion

        #region Entry Methods

        /// <summary>
        /// Retrieves data from the cache or if data is not available within the cache it is
        /// retrieved from the source and cached.
        /// </summary>
        /// <param name="arguments">The <typeparamref name="TArgument"/>s representing the data required.</param>
        /// <param name="cancellationToken">
        /// The token to monitor for cancellation requests.
        /// </param>
        /// <returns>
        /// The data requested, as described by <paramref name="arguments"/>.
        /// </returns>
        protected async Task<IEnumerable<TData>> GetDataAsync(IEnumerable<TArgument> arguments, CancellationToken cancellationToken = default)
        {
            List<Request> submitted;
            List<Request> required;

            this.requestsLock.EnterUpgradeableReadLock();
            try
            {
                this.GetRequests(arguments, out submitted, out required);

                if (required.Count != 0)
                {
                    this.requestsLock.EnterWriteLock();
                    try
                    {
                        this.submittedRequests.AddRange(required);
                    }
                    finally
                    {
                        this.requestsLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                this.requestsLock.ExitUpgradeableReadLock();
            }

            //Load the data in another thread.
            if (required.Count != 0)
            {
                _ = this.CacheDataForRequestsAsync(required, cancellationToken);
            }

            IEnumerable<Request> requests;
            if ((submitted.Count != 0) && (required.Count != 0))
                requests = submitted.Union(required);
            else if (required.Count != 0)
                requests = required;
            else if (submitted.Count != 0)
                requests = submitted;
            else
                //This should not occur
                throw new InvalidOperationException("Neither submitted nor required returned any results.");

            foreach (var request in requests)
            {
                await request.Task;
            }

            //The data should now have been cached filter the cache to find the requested data.
            IEnumerable<TData> requiredData;
            this.dataLock.EnterReadLock();
            try
            {
                requiredData = FilterDataSetByArgument(
                    this.data,
                    arguments);
            }
            finally
            {
                this.dataLock.ExitReadLock();
            }

            //Use the .CacheResult() linq method to ensure complex filtering is only evaluated once.
            return requiredData.CacheResult();
        }

        protected async Task DropDataAsync(IEnumerable<TArgument> arguments)
        {
            this.requestsLock.EnterWriteLock();
            try
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
                        IEnumerable<TArgument> remainingArgs;
                        if (originalRequest.Argument.IsOverlapped(dropArgument, out remainingArgs))
                        {
                            //The request must complete before it can "divided".
                            // It must also complete before the dataLock is taken for
                            // data removal, otherwise a race condition develops where the 
                            // retrieved data for the request may only be inserted after
                            // the delete has occurred. Which would result in data existing
                            // in the cache without a corresponding request.
                            // Therefore WaitOne() can not be called inside the
                            // "if (remainingArgs != null)" block
                            Exception originalRequestException = null;
                            try
                            {
                                await originalRequest.Task;
                            }
                            catch (Exception ex)
                            {
                                originalRequestException = ex;
                            }

                            //Create remainder requests from the remainder args, duplicating the state
                            // of the orginal argument
                            if (remainingArgs != null)
                            {
                                foreach (TArgument remainingArg in remainingArgs)
                                {
                                    Request remainingRequest = new Request(remainingArg);

                                    //Test if the original request had been successful, or failed
                                    if (originalRequestException == null)
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
            finally
            {
                this.requestsLock.ExitWriteLock();
            }
        }

        protected void Clear()
        {
            this.requestsLock.EnterWriteLock();
            try
            {
                this.dataLock.EnterWriteLock();
                try
                {
                    this.data.Clear();
                }
                finally
                {
                    this.dataLock.ExitWriteLock();
                }

                this.submittedRequests.Clear();
            }
            finally
            {
                this.requestsLock.ExitWriteLock();
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

        private async Task CacheDataForRequestsAsync(IEnumerable<Request> requests, CancellationToken cancellationToken)
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
                    this.requestsLock.EnterWriteLock();
                    try
                    {
                        foreach (var request in requests)
                        {
                            this.submittedRequests.Remove(request);
                        }
                    }
                    finally
                    {
                        this.requestsLock.ExitWriteLock();
                    }
                }

                throw;
            }

            // Add the loaded data to the cache
            this.dataLock.EnterWriteLock();
            try
            {
                int size = this.data.Count + loadedData.Count;
                if (this.data.Capacity < size)
                    this.data.Capacity = size;
                this.data.AddRange(loadedData);
            }
            finally
            {
                this.dataLock.ExitWriteLock();
            }

            // Mark the requests as successful
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

            //More data may be returned than was required. Filter the data to enure
            // only the data requested is returned.
            IEnumerable<TData> dataforRequests = FilterDataSetByArgument(
                returnedData,
                requests.Select(request => request.Argument));

            cancellationToken.ThrowIfCancellationRequested();

            return dataforRequests;
        }
        
        private void GetRequests(
            IEnumerable<TArgument> newArguments,
            out List<Request> submitted,
            out List<Request> required)
        {
            submitted = new List<Request>();

            IEnumerable<TArgument> argumentList = newArguments;

            //Attempt to estimate the capacity required
            int nextCapacity = 4; // Default
            if (argumentList is ICollection<TArgument>)
            {
                //Get the estimated capacity
                int count = ((ICollection<TArgument>)argumentList).Count;
                nextCapacity = count + (count >> 2); // add 25% to the capcacity
            }

            //UpgradableReadLock already taken on requestsLock
            foreach (var submittedRequest in this.submittedRequests)
            {
                var nextArgumentList = new List<TArgument>(nextCapacity);

                foreach (var argument in argumentList)
                {
                    IEnumerable<TArgument> notSubmitted;
                    if (argument.IsOverlapped(submittedRequest.Argument, out notSubmitted))
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

                int count = nextArgumentList.Count;
                nextCapacity = count + (count >> 2); // add 25% to the capacity

                argumentList = nextArgumentList;
            }

            required = argumentList.Select(arg => new Request(arg)).ToList();
        }


        private static IEnumerable<TData> FilterDataSetByArgument(
            IEnumerable<TData> dataSet,
            IEnumerable<TArgument> filterArguments)
        {
            return dataSet
                .Where(dataItem => filterArguments.Any(filter => filter.IsDataIncluded(dataItem)));
        }

        #endregion
    }
}
