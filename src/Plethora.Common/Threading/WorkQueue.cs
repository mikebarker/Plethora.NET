using System;
using System.Collections.Generic;
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
            private readonly object[] args;

            private object result;
            private Exception exception;
            #endregion

            #region Constructors

            public WorkItem(Delegate method, object[] args)
            {
                //Validation
                if (method == null)
                    throw new ArgumentNullException("method");

                this.method = method;
                this.args = args;
            }
            #endregion

            #region Public Members

            public void Execute()
            {
                try
                {
                    object invokeResult = method.DynamicInvoke(args);
                    Success(invokeResult);
                }
                catch (Exception ex)
                {
                    Fail(ex);
                }
            }

            public object GetResult()
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

            private void Success(object invokeResult)
            {
                this.result = invokeResult;
                waitHandle.Set();
            }

            private void Fail(Exception ex)
            {
                if (ex == null)
                    throw new ArgumentNullException("ex");


                this.exception = ex;
                waitHandle.Set();
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
                get { return waitHandle.WaitOne(0); }
            }

            /// <summary>
            /// Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </summary>
            /// <returns>
            /// A <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
            /// </returns>
            public WaitHandle AsyncWaitHandle
            {
                get { return waitHandle; }
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
                get { return true; }
            }
            #endregion
        }

        #region Fields

        private readonly List<Thread> threads;
        private readonly Queue<WorkItem> workQueue = new Queue<WorkItem>();
        private readonly ManualResetEvent workWaitHandle = new ManualResetEvent(false);
        private bool exitDoLoop = false;
        #endregion

        #region Constructors

        public WorkQueue()
            : this(Environment.ProcessorCount)
        {
        }

        public WorkQueue(int threadCount)
        {
            //Validation
            if (threadCount <= 0)
                throw new ArgumentOutOfRangeException("threadCount", threadCount,
                    ResourceProvider.ArgMustBeGreaterThanZero("threadCount"));


            this.threads = new List<Thread>(threadCount);

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(DoLoop);
                thread.IsBackground = true;
                thread.Name = "WorkQueue_" + i.ToString(CultureInfo.InvariantCulture);
                thread.Start();

                this.threads.Add(thread);
            }
        }
        #endregion

        #region Implementation of IDisposable

        private bool disposed = false;

        ~WorkQueue()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releasing unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                disposed = true;
            }
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
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            if (disposed)
                throw new InvalidOperationException(ResourceProvider.AlreadyDisposed());

            var workItem = new WorkItem(method, args);
            lock(workQueue)
            {
                workQueue.Enqueue(workItem);
                workWaitHandle.Set();
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
        public object EndInvoke(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            if (!(result is WorkItem))
                throw new ArgumentException(ResourceProvider.ArgInvalid("result"), "result");

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
        public object Invoke(Delegate method, object[] args)
        {
            var asyncResult = BeginInvoke(method, args);
            return EndInvoke(asyncResult);
        }

        public void Abort()
        {
            this.exitDoLoop = true;
            lock (this.workQueue)
            {
                this.workQueue.Clear();
                workWaitHandle.Set();
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
            //Semi-inifinite loop
            while (!exitDoLoop)
            {
                WorkItem workItem = null;
                bool workComplete;
                lock (workQueue)
                {
                    if (workQueue.Count == 0)
                    {
                        workComplete = true;
                    }
                    else
                    {
                        workItem = workQueue.Dequeue();
                        workComplete = (workQueue.Count == 0);
                    }

                    if (workComplete)
                    {
                        workWaitHandle.Reset();
                    }
                }

                if (workItem != null)
                {
                    //Execute the workItem
                    try { workItem.Execute(); }
                    catch(Exception ex) { }
                }

                if (workComplete)
                {
                    //Wait for work to arrive.
                    workWaitHandle.WaitOne();
                }
            }
        }
        #endregion
    }
}
