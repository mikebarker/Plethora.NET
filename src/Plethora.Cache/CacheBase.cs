using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Plethora.Linq;
using Plethora.Threading;
using Plethora.Timing;

namespace Plethora.Cache
{
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

            public Request(TArgument argument)
            {
                if (argument == null)
                    throw new ArgumentNullException("argument");

                this.completeWaitHandle = new ManualResetEvent(false);
                this.argument = argument;
            }
            #endregion

            #region Properties

            public TArgument Argument
            {
                get { return argument; }
            }

            public WaitHandle CompleteWaitHandle
            {
                get { return completeWaitHandle; }
            }

            public bool IsComplete
            {
                get { return this.completeWaitHandle.WaitOne(0); }
            }

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

            internal void Fail(Exception ex)
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
            private readonly TArgument originatingArgument;
            #endregion

            #region Constructor

            public GetDataAsyncResult(CacheBase<TData, TArgument> parent, IEnumerable<Request> requests, TArgument originatingArgument)
            {
                this.parent = parent;
                this.requests = requests;
                this.originatingArgument = originatingArgument;
            }
            #endregion

            #region Properties

            public CacheBase<TData, TArgument> Parent
            {
                get { return parent; }
            }

            public IEnumerable<Request> Requests
            {
                get { return requests; }
            }

            public TArgument OriginatingArgument
            {
                get { return originatingArgument; }
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
                    if (aggregateHandle == null)
                    {
                        lock (aggregateHandleLock)
                        {
                            if (aggregateHandle == null)
                            {
                                aggregateHandle = new AggregateWaitHandle(requests
                                    .Select(r => r.CompleteWaitHandle)
                                    .ToArray());
                            }
                        }
                    }

                    return aggregateHandle;
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
                get { return requests; }
            }

            public OperationTimeout Timeout
            {
                get { return timeout; }
            }
            #endregion
        }


        #region Fields

        private readonly ReaderWriterLockSlim requestsLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly List<Request> submittedRequests = new List<Request>();

        private readonly ReaderWriterLockSlim dataLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly List<TData> data = new List<TData>();
        #endregion

        protected IEnumerable<TData> GetData(TArgument argument, int millisecondsTimeout)
        {
            var asyncResult = BeginGetData(argument, millisecondsTimeout);
            return EndGetData(asyncResult);
        }

        protected IAsyncResult BeginGetData(TArgument argument, int millisecondsTimeout)
        {
            OperationTimeout timeout = new OperationTimeout(millisecondsTimeout);

            List<Request> submitted;
            List<Request> required;

            requestsLock.EnterUpgradeableReadLock();
            try
            {
                GetRequests(argument, out submitted, out required);

                if (required.Count != 0)
                {
                    requestsLock.EnterWriteLock();
                    try
                    {
                        submittedRequests.AddRange(required);
                    }
                    finally
                    {
                        requestsLock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                requestsLock.ExitUpgradeableReadLock();
            }

            //Load the data in another thread.
            if (required.Count != 0)
            {
                ThreadPool.QueueUserWorkItem(CacheDataForRequests, new CacheDataParameters(required, timeout));
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

            var asyncResult = new GetDataAsyncResult(this, requests, argument);
            return asyncResult;
        }

        protected IEnumerable<TData> EndGetData(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

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
                    .Take(WaitHandleHelper.MaxWaitHandles)
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
            dataLock.EnterReadLock();
            try
            {
                requiredData = FilterDataSetByArgument(
                    this.data,
                    dataAsyncResult.OriginatingArgument.Singularity());
            }
            finally
            {
                dataLock.ExitReadLock();
            }

            //Use the .CacheResult() linq method to ensure complex filtering is only evaluated once.
            return requiredData.CacheResult();
        }


        private void CacheDataForRequests(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var parameters = obj as CacheDataParameters;
            if (parameters == null)
                throw new ArgumentException(
                    ResourceProvider.ArgMustBeOfType("obj", typeof(CacheDataParameters)), "obj");

            CacheDataForRequests(parameters.Requests, parameters.Timeout);
        }

        private void CacheDataForRequests(IEnumerable<Request> requests, OperationTimeout timeout)
        {
            //Load the data for the 'required' requests.
            List<TData> loadedData;
            try
            {
                loadedData = GetDataForRequests(requests, timeout).ToList();
                timeout.ThrowIfElapsed();
            }
            catch (Exception ex)
            {
                //Mark all requests as failed.
                requests.ForEach(r => r.Fail(ex));

                //If the exception is recoverable, then remove the requests from
                // the submitted requests to allow future calls to attempt to
                // reload the data.
                if (IsExceptionRecoverable(ex))
                {
                    requestsLock.EnterWriteLock();
                    try
                    {
                        foreach (var request in requests)
                        {
                            submittedRequests.Remove(request);
                        }
                    }
                    finally
                    {
                        requestsLock.ExitWriteLock();
                    }
                }

                throw;
            }

            //Add the loaded data to the cache
            dataLock.EnterWriteLock();
            try
            {
                int size = this.data.Count + loadedData.Count;
                if (this.data.Capacity < size)
                    this.data.Capacity = size;
                this.data.AddRange(loadedData);
            }
            finally
            {
                dataLock.ExitWriteLock();
            }

            //Mark the requests as successful
            foreach (var request in requests)
            {
                request.Success();
            }
        }

        private IEnumerable<TData> GetDataForRequests(IEnumerable<Request> requests, OperationTimeout timeout)
        {
            //Call JoinForLoad to allow the multi calls to the source to be collapsed into
            // fewer calls.
            var loadArgs = JoinForLoad(requests.Select(request => request.Argument));

            //TODO: Parrallel-ise the calls to the source. (Unless there is only 1 load Arg)
            IEnumerable<TData> returnData = null;
            foreach (var loadArg in loadArgs)
            {
                var dataForArg = GetDataFromSource(loadArg, timeout.RemainingThrowIfElapsed);
                timeout.ThrowIfElapsed();

                if (dataForArg != null)
                {
                    if (returnData == null)
                        returnData = dataForArg;
                    else
                        returnData = returnData.Concat(dataForArg);
                }
            }

            //More data may be returned than was required. Filter the data to enure
            // only the data requested is returned.
            IEnumerable<TData> dataforRequests = FilterDataSetByArgument(returnData, requests.Select(request => request.Argument));
            timeout.ThrowIfElapsed();

            return dataforRequests;
        }

        protected abstract IEnumerable<TData> GetDataFromSource(TArgument argument, int millisecondsTimeout);


        protected virtual IEnumerable<TArgument> JoinForLoad(IEnumerable<TArgument> arguments)
        {
            return arguments;
        }

        protected virtual bool IsExceptionRecoverable(Exception ex)
        {
            if (ex is TimeoutException)
                return true;

            return false;
        }

        private void GetRequests(
            TArgument newArgument,
            out List<Request> submitted,
            out List<Request> required)
        {
            submitted = new List<Request>();

            List<TArgument> argumentList = new List<TArgument> {newArgument};

            //UpgradableReadLock already taken on requestsLock
            foreach (var submittedRequest in this.submittedRequests)
            {
                //Get the estimated capacity
                int count = argumentList.Count;
                int capacity = count + (count >> 2); // add 25% to the capcacity

                var nextArgumentList = new List<TArgument>(capacity);

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
    }
}
