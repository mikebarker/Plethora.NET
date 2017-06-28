using System;
using System.ComponentModel;
using System.Threading;

namespace Plethora.ComponentModel
{
    public static class AsyncTaskHelper
    {
        #region GetValue

        /// <summary>
        /// Extention method used to retrieve a value from a <see cref="ISynchronizeInvoke"/>
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
            if (syncInvoke == null)
                throw new ArgumentNullException(nameof(syncInvoke));

            if (getter == null)
                throw new ArgumentNullException(nameof(getter));


            if (syncInvoke.InvokeRequired)
                return (TResult)syncInvoke.Invoke(getter, new object[] { syncInvoke });
            else
                return getter(syncInvoke);
        }
        #endregion

        #region Exec

        /// <summary>
        /// Extention method used to marshall the execution of an action to 
        /// the thread required by an <see cref="ISynchronizeInvoke"/>.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="task">The <see cref="Action{TSync}"/> to be executed.</param>
        /// <example>
        ///  <code>
        ///    textBox1.Exec(() => textBox1.Text = "hello");
        ///  </code>
        /// </example>
        public static void Exec(this ISynchronizeInvoke syncInvoke, Action task)
        {
            //Validation
            if (syncInvoke == null)
                throw new ArgumentNullException(nameof(syncInvoke));

            if (task == null)
                throw new ArgumentNullException(nameof(task));


            if (syncInvoke.InvokeRequired)
            {
                syncInvoke.BeginInvoke(task, new object[0]);
            }
            else
            {
                task();
            }
        }

        /// <summary>
        /// Extention method used to marshall the execution of an action to 
        /// the thread required by an <see cref="ISynchronizeInvoke"/>.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="task">The <see cref="Action{TSync}"/> to be executed.</param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait for the task to complete.
        /// Specify <see cref="Timeout.Infinite"/> to wai indefinitely.
        /// </param>
        /// <returns>
        /// true if the task completed within the required timeout; otherwise false.
        /// </returns>
        /// <remarks>
        /// If no thread marshalling is required, the task is executed on the calling thread
        /// and <paramref name="millisecondsTimeout"/> is ignorred.
        /// </remarks>
        /// <example>
        ///  <code>
        ///    textBox1.Exec(() => textBox1.Text = "hello", 10);
        ///  </code>
        /// </example>
        public static bool Exec(this ISynchronizeInvoke syncInvoke, Action task, int millisecondsTimeout)
        {
            //Validation
            if (syncInvoke == null)
                throw new ArgumentNullException(nameof(syncInvoke));

            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if ((millisecondsTimeout < 0) && (millisecondsTimeout != Timeout.Infinite))
                throw new ArgumentOutOfRangeException(
                    nameof(millisecondsTimeout),
                    millisecondsTimeout,
                    ResourceProvider.ArgTimeout(nameof(millisecondsTimeout)));


            if (syncInvoke.InvokeRequired)
            {
                var result = syncInvoke.BeginInvoke(task, new object[0]);
                return result.AsyncWaitHandle.WaitOne(millisecondsTimeout);
            }
            else
            {
                task();
                return true;
            }
        }
        #endregion

        #region AsyncTask

        /// <summary>
        /// Executes a task asyncronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing async task.
        /// </returns>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// The task <paramref name="action"/> will be executed on a thread-pool thread.
        /// </remarks>
        public static IAsyncResult AsyncTask(
            this ISynchronizeInvoke syncInvoke,
            Action action)
        {
            Action onComplete = () => { };
            Action<Exception> onException = ex => { };

            return AsyncTask(syncInvoke, action, onComplete, onException);
        }

        /// <summary>
        /// Executes a task asyncronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing async task.
        /// </returns>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// The task <paramref name="action"/> will be executed on a thread-pool thread.
        /// <paramref name="onComplete"/> will be marshalled be to
        /// the thread of the <see cref="syncInvoke"/>
        /// </remarks>
        public static IAsyncResult AsyncTask(
            this ISynchronizeInvoke syncInvoke,
            Action action,
            Action onComplete)
        {
            Action<Exception> onException = ex => { };

            return AsyncTask(syncInvoke, action, onComplete, onException);
        }

        /// <summary>
        /// Executes a task asyncronously.
        /// </summary>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <param name="onException">The <see cref="Action{Exception}"/> executed if <paramref name="action"/> fails.</param>
        /// <returns>
        /// An <see cref="IAsyncResult"/> of the executing async task.
        /// </returns>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// The task <paramref name="action"/> will be executed on a thread-pool thread.
        /// <paramref name="onComplete"/> and <paramref name="onException"/> will be marshalled be to
        /// the thread of the <see cref="syncInvoke"/>
        /// </remarks>
        public static IAsyncResult AsyncTask(
            this ISynchronizeInvoke syncInvoke,
            Action action,
            Action onComplete,
            Action<Exception> onException)
        {
            //Validation
            if (syncInvoke == null)
                throw new ArgumentNullException(nameof(syncInvoke));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (onComplete == null)
                throw new ArgumentNullException(nameof(onComplete));

            if (onException == null)
                throw new ArgumentNullException(nameof(onException));


            Action wrappedAction = delegate
                {
                    try
                    {
                        action();
                    }
                    catch(Exception ex)
                    {
                        syncInvoke.Invoke(onException, new object[] { ex });
                    }
                };


            AsyncCallback callback = delegate
                {
                    syncInvoke.Invoke(onComplete, new object[0]);
                };

            return wrappedAction.BeginInvoke(callback, null);
        }
        #endregion
    }
}
