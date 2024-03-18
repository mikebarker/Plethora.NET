using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Plethora.Threading
{
    /// <summary>
    /// Work queue which can be used to schedule work onto a managed thread pool.
    /// </summary>
    /// <remarks>
    /// Each <see cref="WorkQueue"/> object executes on its own separate thread.
    /// </remarks>
    public class WorkQueue : IDisposable
    {
        private class WorkItem : IAsyncResult
        {
            #region Fields

            private readonly EventWaitHandle waitHandle = new ManualResetEvent(false);
            private readonly Delegate method;
            private readonly object?[]? args;

            private object? result;
            private Exception? exception;
            #endregion

            #region Constructors

            public WorkItem(Delegate method, object?[]? args)
            {
                //Validation
                ArgumentNullException.ThrowIfNull(method);

                this.method = method;
                this.args = args;
            }
            #endregion

            #region Public Members

            public void Execute()
            {
                try
                {
                    object? invokeResult = this.method.DynamicInvoke(this.args);
                    this.Success(invokeResult);
                }
                catch (Exception ex)
                {
                    this.Fail(ex);
                }
            }

            public object? GetResult()
            {
                this.waitHandle.WaitOne();
                if (this.exception == null)
                {
                    return this.result;
                }
                else
                {
                    throw new AsyncException(this.exception);
                }
            }
            #endregion

            #region Private Methods

            private void Success(object? invokeResult)
            {
                this.result = invokeResult;
                this.waitHandle.Set();
            }

            private void Fail(Exception ex)
            {
                ArgumentNullException.ThrowIfNull(ex);


                this.exception = ex;
                this.waitHandle.Set();
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
                get { return this.waitHandle.WaitOne(0); }
            }

            /// <summary>
            /// Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </summary>
            /// <returns>
            /// A <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </returns>
            public WaitHandle AsyncWaitHandle
            {
                get { return this.waitHandle; }
            }

            /// <summary>
            /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
            /// </summary>
            /// <returns>
            /// A user-defined object that qualifies or contains information about an asynchronous operation.
            /// </returns>
            object? IAsyncResult.AsyncState
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
                get { return true; }
            }
            #endregion
        }

        #region Fields

        private readonly List<Thread> threads;
        private readonly Queue<WorkItem> workQueue = new();
        private readonly ManualResetEventSlim workWaitHandle = new(false);
        private bool exitDoLoop = false;
        private int availableThreadCount;

        #endregion

        #region Constructors

        public WorkQueue()
            : this(Environment.ProcessorCount, true)
        {
        }

        public WorkQueue(int threadCount)
            : this(threadCount, true)
        {
        }

        public WorkQueue(int threadCount, bool isBackground)
        {
            //Validation
            if (threadCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(threadCount), threadCount,
                    ResourceProvider.ArgMustBeGreaterThanZero(nameof(threadCount)));


            this.availableThreadCount = threadCount;
            this.threads = new List<Thread>(threadCount);
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new(this.DoLoop)
                {
                    IsBackground = isBackground,
                    Name = "WorkQueue_" + i.ToString(CultureInfo.InvariantCulture)
                };
                thread.Start();

                this.threads.Add(thread);
            }
        }
        #endregion

        #region Implementation of IDisposable

        private bool disposed = false;

        ~WorkQueue()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Releasing unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// If true releases managed and unmanaged resources; otherwise release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if(!this.disposed)
            {
                if(disposing)
                {
                    // Dispose managed resources.
                    this.Abort();
                }

                // Release unmanaged resources.


                // Note disposing has been done.
                this.disposed = true;
            }
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Raised when a thread becomes available for additional work.
        /// </summary>
        /// <remarks>
        /// This event will be called on a <see cref="ThreadPool"/> thread. Logic contained in this event handler should therefore be kept short.
        /// 
        /// Due to race conditions all worker threads may have been consumed by the time work is queued.
        /// Similarly the <see cref="AvailableThreadCount"/> may show zero after this event has triggered.
        /// </remarks>
        /// <seealso cref="AvailableThreadCount"/>
        public event EventHandler? ThreadAvailable;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the number of threads which are not currently working.
        /// </summary>
        /// <seealso cref="ThreadAvailable"/>
        public int AvailableThreadCount
        {
            get { return this.availableThreadCount; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Asynchronously executes the delegate on the thread that created this object.
        /// </summary>
        /// <returns>
        /// An <see cref="IAsyncResult"/> interface that represents the asynchronous
        /// operation started by calling this method.
        /// </returns>
        /// <param name="method">
        /// A <see cref="Delegate"/> to a method that takes parameters of the same number
        /// and type that are contained in <paramref name="args"/>.
        /// </param>
        /// <param name="args">
        /// An array of type <see cref="Object"/> to pass as arguments to the given method.
        /// This can be null if no arguments are needed.
        /// </param>
        public IAsyncResult BeginInvoke(Delegate method, object?[]? args)
        {
            if (this.disposed)
                throw new InvalidOperationException(ResourceProvider.AlreadyDisposed());

            var workItem = new WorkItem(method, args);
            lock(this.workQueue)
            {
                this.workQueue.Enqueue(workItem);
                this.workWaitHandle.Set();
            }
            return workItem;
        }

        /// <summary>
        /// Waits until the process started by calling <see cref="BeginInvoke"/> completes,
        /// and then returns the value generated by the process.
        /// </summary>
        /// <returns>
        /// An <see cref="Object"/> that represents the return value generated by the
        /// asynchronous operation.
        /// </returns>
        /// <param name="result">
        /// An <see cref="IAsyncResult"/> interface that represents the asynchronous operation
        /// started by calling <see cref="BeginInvoke"/>. 
        /// </param>
        public object? EndInvoke(IAsyncResult result)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result is not WorkItem)
                throw new ArgumentException(ResourceProvider.ArgInvalid(nameof(result)), nameof(result));

            var workItem = (WorkItem)result;
            return workItem.GetResult();
        }

        /// <summary>
        /// Synchronously executes the delegate on the thread that created this object
        /// and marshals the call to the creating thread.
        /// </summary>
        /// <returns>
        /// An <see cref="Object"/> that represents the return value from the delegate
        /// being invoked, or null if the delegate has no return value.
        /// </returns>
        /// <param name="method">
        /// A <see cref="Delegate"/> that contains a method to call, in the context of
        /// the thread for the control.
        /// </param>
        /// <param name="args">
        /// An array of type <see cref="Object"/> that represents the arguments to pass
        /// to the given method. This can be null if no arguments are needed.
        /// </param>
        public object? Invoke(Delegate method, object?[]? args)
        {
            var asyncResult = this.BeginInvoke(method, args);
            return this.EndInvoke(asyncResult);
        }

        /// <summary>
        /// Removes all waiting work from the queue, and signals worker threads to end after they complete their current work.
        /// </summary>
        public void Abort()
        {
            this.exitDoLoop = true;
            lock (this.workQueue)
            {
                this.workQueue.Clear();
                this.workWaitHandle.Set();
            }
        }

        #endregion

        #region Protected Methods

        protected bool IsWorkerThread()
        {
            bool result = this.threads.Contains(Thread.CurrentThread);
            return result;
        }

        #endregion

        #region Private Methods

        private void DoLoop()
        {
            bool isThisThreadAvailable = true;

            //Semi-infinite loop
            while (!this.exitDoLoop)
            {
                WorkItem? workItem = null;
                bool workComplete;
                lock (this.workQueue)
                {
                    if (this.workQueue.Count == 0)
                    {
                        workComplete = true;
                    }
                    else
                    {
                        workItem = this.workQueue.Dequeue();
                        workComplete = (this.workQueue.Count == 0);
                    }

                    if (workComplete)
                    {
                        this.workWaitHandle.Reset();
                    }
                }

                if (workItem != null)
                {
                    //Ensure the decrement only happens once per thread
                    if (isThisThreadAvailable)
                    {
                        Interlocked.Decrement(ref this.availableThreadCount);
                        isThisThreadAvailable = false;
                    }

                    //Execute the workItem
                    workItem.Execute();
                }

                if (workComplete)
                {
                    if (!isThisThreadAvailable)
                    {
                        Interlocked.Increment(ref this.availableThreadCount);
                        isThisThreadAvailable = true;

                        EventHandler? handler = this.ThreadAvailable;
                        if (handler != null)
                            ThreadPool.QueueUserWorkItem((o) => handler(this, EventArgs.Empty));
                    }

                    //Wait for work to arrive.
                    this.workWaitHandle.Wait();
                }
            }
        }

        #endregion
    }
}
