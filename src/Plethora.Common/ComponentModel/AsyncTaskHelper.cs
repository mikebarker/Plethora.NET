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
                throw new ArgumentNullException("syncInvoke");

            if (getter == null)
                throw new ArgumentNullException("getter");


            if (syncInvoke.InvokeRequired)
                return (TResult)syncInvoke.Invoke(getter, new object[] { syncInvoke });
            else
                return getter(syncInvoke);
        }
        #endregion

        #region SyncTask and AsyncTask

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// </remarks>
        /// <example>
        ///  <code>
        ///    textBox1.AsyncTask(() => textBox1.Text = "Finished");
        ///  </code>
        /// </example>
        public static void AsyncTask<TSync>(
            this TSync syncInvoke,
            Action action)
            where TSync : ISynchronizeInvoke
        {
            Action onComplete = () => { };
            Action<Exception> onException = ex => { };

            AsyncTask(syncInvoke, action, onComplete, onException, 0);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread, and waits for the
        /// task to complete before returning.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <example>
        ///  <code>
        ///    textBox1.SyncTask(() => textBox1.Text = "Finished");
        ///  </code>
        /// </example>
        public static void SyncTask<TSync>(
            this TSync syncInvoke,
            Action action)
            where TSync : ISynchronizeInvoke
        {
            Action onComplete = () => { };
            Action<Exception> onException = ex => { };

            AsyncTask(syncInvoke, action, onComplete, onException, Timeout.Infinite);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// </remarks>
        public static void AsyncTask<TSync>(
            this TSync syncInvoke,
            Action action,
            Action onComplete)
            where TSync : ISynchronizeInvoke
        {
            Action<Exception> onException = ex => { };

            AsyncTask(syncInvoke, action, onComplete, onException, 0);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread, and waits for the
        /// task to complete before returning.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        public static void SyncTask<TSync>(
            this TSync syncInvoke,
            Action action,
            Action onComplete)
            where TSync : ISynchronizeInvoke
        {
            Action<Exception> onException = ex => { };

            AsyncTask(syncInvoke, action, onComplete, onException, Timeout.Infinite);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <param name="onException">The <see cref="Action{Exception}"/> executed if <paramref name="action"/> fails.</param>
        /// <remarks>
        /// Method returns immediately without waiting for the task to complete.
        /// </remarks>
        public static void AsyncTask<TSync>(
            this TSync syncInvoke,
            Action action,
            Action onComplete,
            Action<Exception> onException)
            where TSync : ISynchronizeInvoke
        {
            AsyncTask(syncInvoke, action, onComplete, onException, 0);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread, and waits for the
        /// task to complete before returning.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <param name="onException">The <see cref="Action{Exception}"/> executed if <paramref name="action"/> fails.</param>
        public static void SyncTask<TSync>(
            this TSync syncInvoke,
            Action action,
            Action onComplete,
            Action<Exception> onException)
            where TSync : ISynchronizeInvoke
        {
            AsyncTask(syncInvoke, action, onComplete, onException, Timeout.Infinite);
        }

        /// <summary>
        /// Executes a task asyncronously on the required synchronized thread.
        /// </summary>
        /// <typeparam name="TSync">The type of the synchronization object.</typeparam>
        /// <param name="syncInvoke">The object for which the called must be synchronised.</param>
        /// <param name="action">The <see cref="Action"/> which must be executed on the synchronized value.</param>
        /// <param name="onComplete">The <see cref="Action"/> executed when <paramref name="action"/> completes successfully.</param>
        /// <param name="onException">The <see cref="Action{Exception}"/> executed if <paramref name="action"/> fails.</param>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait for the task to complete.
        /// May be zero (0) to return immediately, or <see cref="Timeout.Infinite"/> (-1) to wait indefinitely.
        /// </param>
        /// <returns>
        /// true if the task completes within the given timeout; otherwise, false.
        /// </returns>
        /// <remarks>
        /// If called on the synchronized thread the task will be completed synchronously,
        /// and <paramref name="millisecondsTimeout"/> will be ignored.
        /// </remarks>
        public static bool AsyncTask<TSync>(
            this TSync syncInvoke,
            Action action,
            Action onComplete,
            Action<Exception> onException,
            int millisecondsTimeout)
            where TSync : ISynchronizeInvoke
        {
            //Validation
            if (syncInvoke == null)
                throw new ArgumentNullException("syncInvoke");

            if (action == null)
                throw new ArgumentNullException("action");

            if (onComplete == null)
                throw new ArgumentNullException("onComplete");

            if (onException == null)
                throw new ArgumentNullException("onException");

            if (millisecondsTimeout < -1)
                throw new ArgumentOutOfRangeException("millisecondsTimeout", millisecondsTimeout, "Timeout may not be negative; unless Timeout.Infinite (-1).");


            Action task = delegate
                {
                    try
                    {
                        action();
                        try
                        { onComplete(); }
                        catch
                        { /* Do nothing */ }
                    }
                    catch(Exception ex)
                    {
                        onException(ex);
                    }
                };


            if (syncInvoke.InvokeRequired)
            {
                var asyncResult = syncInvoke.BeginInvoke(task, new object[0]);
                return asyncResult.AsyncWaitHandle.WaitOne(millisecondsTimeout);
            }
            else
            {
                task();
                return true;
            }
        }
        #endregion
    }
}
