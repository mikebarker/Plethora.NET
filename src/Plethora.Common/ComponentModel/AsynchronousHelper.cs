using System;
using System.ComponentModel;
using System.Threading;

namespace Plethora.ComponentModel
{
    [Obsolete("Prefer using Tasks to gain native support for asynchronous tasks.")]
    public static class AsynchronousHelper
    {
        #region GetValue

        /// <summary>
        /// Extension method used to retrieve a value from a <see cref="ISynchronizeInvoke"/>
        /// object, where the getter must be called on the required thread.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <typeparam name="TResult">The return type of <paramref name="getter"/>.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="getter">The <see cref="Func{T, TResult}"/> used to retrieve the required value.</param>
        /// <returns>
        /// The result of calling getter(syncInvoke).
        /// </returns>
        /// <example>
        ///  <code>
        ///    string text = textBox1.GetValue(txt => txt.Text);
        ///  </code>
        /// </example>
        public static TResult GetValue<TSync, TResult>(this TSync syncInvoke, Func<TSync, TResult> getter)
            where TSync : ISynchronizeInvoke
        {
            //Validation
            ArgumentNullException.ThrowIfNull(syncInvoke);
            ArgumentNullException.ThrowIfNull(getter);


            if (syncInvoke.InvokeRequired)
                return (TResult)syncInvoke.Invoke(getter, [syncInvoke])!;
            else
                return getter(syncInvoke);
        }
        #endregion

        #region Execute

        /// <summary>
        /// Extension method used to marshal the execution of an action to 
        /// the thread required by an <see cref="ISynchronizeInvoke"/>.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        /// <example>
        ///  <code>
        ///    textBox1.Execute(() => textBox1.Text = "hello");
        ///  </code>
        /// </example>
        public static void Execute(this ISynchronizeInvoke syncInvoke, Action action)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(syncInvoke);
            ArgumentNullException.ThrowIfNull(action);


            if (syncInvoke.InvokeRequired)
            {
                syncInvoke.BeginInvoke(action, Array.Empty<object>());
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Extension method used to marshal the execution of an action to 
        /// the thread required by an <see cref="ISynchronizeInvoke"/>.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        /// <param name="timeout">
        /// The <see cref="TimeSpan"/> to wait for the action to complete.
        /// Specify <see cref="Timeout.InfiniteTimeSpan"/> to wai indefinitely.
        /// </param>
        /// <returns>
        /// true if the action completed within the required timeout; otherwise false.
        /// </returns>
        /// <remarks>
        /// If no thread marshalling is required, the action is executed on the calling thread
        /// and <paramref name="timeout"/> is ignored.
        /// </remarks>
        /// <example>
        ///  <code>
        ///    textBox1.Execute(() => textBox1.Text = "hello", 10);
        ///  </code>
        /// </example>
        public static bool Execute(this ISynchronizeInvoke syncInvoke, Action action, TimeSpan timeout)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(syncInvoke);
            ArgumentNullException.ThrowIfNull(action);

            if ((timeout < TimeSpan.Zero) && (timeout != Timeout.InfiniteTimeSpan))
                throw new ArgumentOutOfRangeException(
                    nameof(timeout),
                    timeout,
                    ResourceProvider.ArgTimeout(nameof(timeout)));


            if (syncInvoke.InvokeRequired)
            {
                var result = syncInvoke.BeginInvoke(action, Array.Empty<object>());
                return result.AsyncWaitHandle.WaitOne(timeout);
            }
            else
            {
                action();
                return true;
            }
        }
        #endregion

        #region AsyncTask

        /// <summary>
        /// Executes an <see cref="Action"/> asynchronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing asynchronous action.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   This method returns immediately without waiting for the action to complete.
        ///  </para>
        ///  <para>
        ///   The callback <paramref name="action"/> will be executed on a thread-pool thread.
        ///  </para>
        /// </remarks>
        public static IAsyncResult AsyncExecute(
            this ISynchronizeInvoke syncInvoke,
            Action action)
        {
            return AsyncExecute(syncInvoke, action, null, null);
        }

        /// <summary>
        /// Executes an <see cref="Action"/> asynchronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onCompletedSuccessfully">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing asynchronous action.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   This method returns immediately without waiting for the action to complete.
        ///  </para>
        ///  <para>
        ///   The callback <paramref name="action"/> will be executed on a thread-pool thread.
        ///   <paramref name="onCompletedSuccessfully"/> will be marshalled be to
        ///   the thread of the <see cref="syncInvoke"/>
        ///  </para>
        ///  <para>
        ///   The callback <paramref name="onCompletedSuccessfully"/> will be executed when the <paramref name="action"/> completes, whether
        ///   successfully or unsuccessfully.
        ///  </para>
        /// </remarks>
        public static IAsyncResult AsyncExecute(
            this ISynchronizeInvoke syncInvoke,
            Action action,
            Action? onCompletedSuccessfully)
        {
            return AsyncExecute(syncInvoke, action, onCompletedSuccessfully, null);
        }

        /// <summary>
        /// Executes an <see cref="Action"/> asynchronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onCompletedSuccessfully">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <param name="onException">The <see cref="Action{Exception}"/> executed if <paramref name="action"/> fails.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing asynchronous action.
        /// </returns>
        /// <remarks>
        ///  <para>
        ///   This method returns immediately without waiting for the action to complete.
        ///  </para>
        ///  <para>
        ///   The callback <paramref name="action"/> will be executed on a thread-pool thread.
        ///   <paramref name="onCompletedSuccessfully"/> and <paramref name="onException"/> will be marshalled be to
        ///   the thread of the <see cref="syncInvoke"/>
        ///  </para>
        /// </remarks>
        public static IAsyncResult AsyncExecute(
            this ISynchronizeInvoke syncInvoke,
            Action action,
            Action? onCompletedSuccessfully,
            Action<Exception>? onException)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(syncInvoke);
            ArgumentNullException.ThrowIfNull(action);


            AsyncExecuteResult asyncResult = new();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                Exception? exception = null;

                try
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    // Do not call the callbacks within the context of the try..catch block
                    if (exception is null)
                    {
                        if (onCompletedSuccessfully is not null)
                        {
                            syncInvoke.Invoke(onCompletedSuccessfully, Array.Empty<object>());
                        }
                    }
                    else
                    {
                        if (onException is not null)
                        {
                            syncInvoke.Invoke(onException, [exception]);
                        }
                    }
                }
                finally
                {
                    asyncResult.Complete();
                }
            });

            return asyncResult;
        }

        private class AsyncExecuteResult : IAsyncResult
        {
            private readonly ManualResetEvent waitHandle = new(false);

            object? IAsyncResult.AsyncState => null;

            public WaitHandle AsyncWaitHandle => this.waitHandle;

            public bool CompletedSynchronously => false;

            public bool IsCompleted => this.waitHandle.WaitOne(0);

            internal void Complete()
            {
                this.waitHandle.Set();
            }
        }

        #endregion
    }
}
