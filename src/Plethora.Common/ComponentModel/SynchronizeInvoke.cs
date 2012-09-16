using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Plethora.Threading;

namespace Plethora.ComponentModel
{
    /// <summary>
    /// Implementation of <see cref="ISynchronizeInvoke"/>, which does not require
    /// a UI thread and message queue.
    /// </summary>
    /// <remarks>
    /// Each <see cref="SynchronizeInvoke"/> object executes on its own separate thread.
    /// </remarks>
    public class SynchronizeInvoke : ISynchronizeInvoke, IDisposable
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

        private static int syncInvokeCount = 0;

        private readonly int syncInvokeId;
        private Thread asyncThread;
        private readonly Queue<WorkItem> workQueue = new Queue<WorkItem>();
        private readonly ManualResetEvent workWaitHandle = new ManualResetEvent(false);
        private bool exitDoLoop = false;
        #endregion

        #region Constructors

        public SynchronizeInvoke()
        {
            this.syncInvokeId = syncInvokeCount++;
        }
        #endregion

        #region Implementation of IDisposable

        private bool disposed = false;

        ~SynchronizeInvoke()
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

        #region Implementation of ISynchronizeInvoke

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
                if (asyncThread == null)
                    EnsureAsyncThreadRunning();

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

        /// <summary>
        /// Gets a value indicating whether the caller must call <see cref="Invoke"/> when
        /// calling an object that implements this interface.
        /// </summary>
        /// <returns>
        /// true if the caller must call <see cref="Invoke"/>; otherwise, false.
        /// </returns>
        public bool InvokeRequired
        {
            get { return true; }
        }
        #endregion

        #region Public Methods

        public void Abort()
        {
            this.exitDoLoop = true;
            lock (this.workQueue)
            {
                this.workQueue.Clear();
            }
            this.asyncThread = null; //Release the handle to the thread.
        }
        #endregion

        #region Protected Properties

        protected Thread AsyncThread
        {
            get { return asyncThread; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures the async thread is running.
        /// </summary>
        /// <remarks>
        ///  <para>
        ///   Creates and starts the async thread if it does not already exist. Allows
        ///   the thread to only be started when it is first required.
        ///  </para>
        ///  <para>
        ///   Must be called from within <c>lock(this.workQueue)</c>.
        ///  </para>
        /// </remarks>
        private void EnsureAsyncThreadRunning()
        {
            this.asyncThread = new Thread(DoLoop);
            this.asyncThread.IsBackground = true;
            this.asyncThread.Name = string.Format("SynchInvoke_{0}", syncInvokeId);
            this.asyncThread.Start();
        }

        private void DoLoop()
        {
            //Inifinite loop
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
                    // Timeout prevents any possibility of the class dead-locking and ensures
                    // that the workQueue is re-tested periodically.
                    workWaitHandle.WaitOne(5000);
                }
            }
        }
        #endregion
    }
}
