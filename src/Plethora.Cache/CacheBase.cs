using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using JetBrains.Annotations;

using Plethora.Linq;
using Plethora.Threading;
using Plethora.Timing;

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
            private readonly ManualResetEvent completeWaitHandle;
            private Exception exception;
            #endregion
            
            #region Constructors

            public Request([NotNull] TArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException(nameof(argument));

                this.completeWaitHandle = new ManualResetEvent(false);
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
            public WaitHandle CompleteWaitHandle
            {
                get { return this.completeWaitHandle; }
            }

            public bool IsComplete
            {
                get { return this.completeWaitHandle.WaitOne(0); }
            }

            [CanBeNull]
            public Exception Exception
            {
                get { return this.exception; }
            }
            #endregion

            #region Methods

            internal void Success()
            {
                this.completeWaitHandle.Set();
            }

            internal void Fail([NotNull] Exception ex)
            {
                this.exception = ex;
                this.completeWaitHandle.Set();
            }
            #endregion
        }

        private class GetDataAsyncResult : IAsyncResult
        {
            #region Fields

            private readonly object aggregateHandleLock = new object();
            private AggregateWaitHandle aggregateHandle;
            private readonly IEnumerable<Request> requests;
            private readonly CacheBase<TData, TArgument> parent;
            private readonly IEnumerable<TArgument> originatingArguments;
            #endregion

            #region Constructor

            public GetDataAsyncResult(
                CacheBase<TData, TArgument> parent,
                IEnumerable<Request> requests,
                IEnumerable<TArgument> originatingArguments)
            {
                this.parent = parent;
                this.requests = requests;
                this.originatingArguments = originatingArguments;
            }
            #endregion

            #region Properties

            public CacheBase<TData, TArgument> Parent
            {
                get { return this.parent; }
            }

            public IEnumerable<Request> Requests
            {
                get { return this.requests; }
            }

            public IEnumerable<TArgument> OriginatingArguments
            {
                get { return this.originatingArguments; }
            }
            #endregion

            #region Implementation of IAsyncResult

            /// <summary>
            /// Gets a value that indicates whether the asynchronous operation has completed.
            /// </summary>
            /// <returns>
            /// true if the operation is complete; otherwise, false.
            /// </returns>
            public bool IsCompleted
            {
                get { return this.requests.All(r => r.IsComplete); }
            }

            /// <summary>
            /// Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </summary>
            /// <returns>
            /// A <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </returns>
            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    if (this.aggregateHandle == null)
                    {
                        lock (this.aggregateHandleLock)
                        {
                            if (this.aggregateHandle == null)
                            {
                                this.aggregateHandle = new AggregateWaitHandle(this.requests
                                    .Select(r => r.CompleteWaitHandle)
                                    .ToArray());
                            }
                        }
                    }

                    return this.aggregateHandle;
                }
            }

            /// <summary>
            /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
            /// </summary>
            /// <returns>
            /// A user-defined object that qualifies or contains information about an asynchronous operation.
            /// </returns>
            public object AsyncState
            {
                get { return null; }
            }

            /// <summary>
            /// Gets a value that indicates whether the asynchronous operation completed synchronously.
            /// </summary>
            /// <returns>
            /// true if the asynchronous operation completed synchronously; otherwise, false.
            /// </returns>
            public bool CompletedSynchronously
            {
                get { return false; }
            }

            #endregion
        }

        private class CacheDataParameters
        {
            #region Fields

            private readonly IEnumerable<Request> requests;
            private readonly OperationTimeout timeout;
            #endregion

            #region Constructors

            public CacheDataParameters(IEnumerable<Request> requests, OperationTimeout timeout)
            {
                this.requests = requests;
                this.timeout = timeout;
            }
            #endregion

            #region Properties

            public IEnumerable<Request> Requests
            {
                get { return this.requests; }
            }

            public OperationTimeout Timeout
            {
                get { return this.timeout; }
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
        /// <param name="millisecondsTimeout">
        /// The time in milliseconds to wait for the operation to complete, otherwise a
        /// <see cref="TimeoutException"/> is thrown. Specify <see cref="Timeout.Infinite"/> (-1) for
        /// no timeout.
        /// </param>
        /// <returns>
        /// The data requested, as described by <paramref name="arguments"/>.
        /// </returns>
        protected IEnumerable<TData> GetData(IEnumerable<TArgument> arguments, int millisecondsTimeout)
        {
            var asyncResult = this.BeginGetData(arguments, millisecondsTimeout);
            return this.EndGetData(asyncResult);
        }

        /// <summary>
        /// Begin the retrieval of data from the cache or if data is not available within the cache it is
        /// retrieved from the source and cached.
        /// </summary>
        /// <param name="arguments">The <typeparamref name="TArgument"/>s representing the data required.</param>
        /// <param name="millisecondsTimeout">
        /// The time in milliseconds to wait for the operation to complete, otherwise a
        /// <see cref="TimeoutException"/> is thrown. Specify <see cref="Timeout.Infinite"/> (-1) for
        /// no timeout.
        /// </param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> which can be passed to <see cref="EndGetData"/>.
        /// </returns>
        protected IAsyncResult BeginGetData(IEnumerable<TArgument> arguments, int millisecondsTimeout)
        {
            OperationTimeout timeout = new OperationTimeout(millisecondsTimeout);

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
                ThreadPool.QueueUserWorkItem(this.CacheDataForRequests, new CacheDataParameters(required, timeout));
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

            var asyncResult = new GetDataAsyncResult(this, requests, arguments);
            return asyncResult;
        }

        /// <summary>
        /// Ends the retrieval of data from the cache or if data is not available within the cache it is
        /// retrieved from the source and cached.
        /// </summary>
        /// <param name="asyncResult">
        /// A <see cref="IAsyncResult"/> as returned from <see cref="BeginGetData"/>.
        /// </param>
        /// <returns>
        /// The data requested, as described by 'arguments' in <see cref="BeginGetData"/>.
        /// </returns>
        protected IEnumerable<TData> EndGetData(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException(nameof(asyncResult));

            var dataAsyncResult = asyncResult as GetDataAsyncResult;
            if (dataAsyncResult == null)
                throw new ArgumentException( /* TODO: */);

            if (!ReferenceEquals(dataAsyncResult.Parent, this))
                throw new ArgumentException( /* TODO: */);


            //This loop essentially acts as WaitHandle.WaitAll(..) but allows for early
            // exit in the case where a request fails before the rest have completed.
            do
            {
                //Get the first 64 incomplete wait handles.
                // If there are more than 64 there is little we can do to use WaitAny across all
                // without a complex thread structure. Generally the requests should not exceed
                // 64.
                // In the case of more than 64 requests this method may not return as early as
                // possible, but it does allow for some degree of early exit.
                WaitHandle[] first64IncompleteWaitHandles = dataAsyncResult.Requests
                    .Where(r => !r.IsComplete)
                    .Select(r => r.CompleteWaitHandle)
                    .Take(AggregateWaitHandle.MaxWaitHandles)
                    .ToArray();

                //Test if none are incomplete
                if (first64IncompleteWaitHandles.Length == 0)
                    break;

                WaitHandle.WaitAny(first64IncompleteWaitHandles);

                //Test if any requests have failed.
                Exception ex = dataAsyncResult.Requests
                    .Where(r => r.IsComplete && r.Exception != null)
                    .Select(r => r.Exception)
                    .FirstOrDefault();

                if (ex != null)
                {
                    //TODO: A more intellegent exception
                    throw new Exception("", ex);
                }
            } while (true);

            //Ensure all requests are complete (should have been completed in loop above)
            dataAsyncResult.AsyncWaitHandle.WaitOne();

            //The data should now have been cached filter the cache to find the requested data.
            IEnumerable<TData> requiredData;
            this.dataLock.EnterReadLock();
            try
            {
                requiredData = FilterDataSetByArgument(
                    this.data,
                    dataAsyncResult.OriginatingArguments);
            }
            finally
            {
                this.dataLock.ExitReadLock();
            }

            //Use the .CacheResult() linq method to ensure complex filtering is only evaluated once.
            return requiredData.CacheResult();
        }

        protected void DropData(IEnumerable<TArgument> arguments)
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
                            originalRequest.CompleteWaitHandle.WaitOne();

                            //Create remainder requests from the remainder args, duplicating the state
                            // of the orginal argument
                            if (remainingArgs != null)
                            {
                                foreach (TArgument remainingArg in remainingArgs)
                                {
                                    Request remainingRequest = new Request(remainingArg);

                                    //Test if the original request had been successful, or failed
                                    Exception ex = originalRequest.Exception;
                                    if (ex == null)
                                        remainingRequest.Success();
                                    else
                                        remainingRequest.Fail(ex);

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
        #endregion

        #region Virtual and Abstract Methods

        /// <summary>
        /// Fetches the required data from the data source.
        /// </summary>
        /// <param name="arguments">The <typeparamref name="TArgument"/>s describing the data required.</param>
        /// <param name="millisecondsTimeout">
        /// The operation timeout in milliseconds. <see cref="Timeout.Infinite"/> (-1) for no timeout.
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
        protected abstract IEnumerable<TData> GetDataFromSource(IEnumerable<TArgument> arguments, int millisecondsTimeout);

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

        private void CacheDataForRequests(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var parameters = obj as CacheDataParameters;
            if (parameters == null)
                throw new ArgumentException(
                    ResourceProvider.ArgMustBeOfType(nameof(obj), typeof(CacheDataParameters)), nameof(obj));

            this.CacheDataForRequests(parameters.Requests, parameters.Timeout);
        }

        private void CacheDataForRequests(IEnumerable<Request> requests, OperationTimeout timeout)
        {
            //Load the data for the 'required' requests.
            List<TData> loadedData;
            try
            {
                loadedData = this.GetDataForRequests(requests, timeout).ToList();

                //Final test on the timeout. Once this try block is exited, all
                // data should committed, and the requests marked successful.
                timeout.ThrowIfElapsed();
            }
            catch (Exception ex)
            {
                //Mark all requests as failed.
                requests.ForEach(r => r.Fail(ex));

                //If the exception is recoverable, then remove the requests from
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

            //Add the loaded data to the cache
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

            //Mark the requests as successful
            foreach (var request in requests)
            {
                request.Success();
            }
        }

        private IEnumerable<TData> GetDataForRequests(
            IEnumerable<Request> requests,
            OperationTimeout timeout)
        {
            IEnumerable<TData> returnedData = this.GetDataFromSource(
                requests.Select(request => request.Argument),
                timeout.RemainingThrowIfElapsed);
            timeout.ThrowIfElapsed();

            //Ensure that each element of the data is retrieved once only.
            returnedData = returnedData.CacheResult();

            //More data may be returned than was required. Filter the data to enure
            // only the data requested is returned.
            IEnumerable<TData> dataforRequests = FilterDataSetByArgument(
                returnedData,
                requests.Select(request => request.Argument));
            timeout.ThrowIfElapsed();

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
