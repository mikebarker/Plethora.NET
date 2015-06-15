using System;
using System.Windows.Threading;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// Class used to introduce delay into a wpf callback.
    /// </summary>
    /// <remarks>
    /// This can be used to prevent multiple event triggers from firing,
    /// and instead bundling them all into one.
    /// </remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    ///     WpfCallbackDelay<EventArgs> callbackDelay = new WpfCallbackDelay<EventArgs>(ContextManager_ContextChanged, 10);
    ///     ContextManager.DefaultInstance.ContextChanged += callbackDelay.Handler;
    /// ]]>
    /// </code>
    /// </example>
    public class WpfCallbackDelay<TEventArgs>
        where TEventArgs : EventArgs
    {
        private readonly object lockObj = new object();
        private object originalSender;
        private TEventArgs originalArgs;

        private readonly EventHandler<TEventArgs> callback;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public CallbackDelay(EventHandler<TEventArgs> callback, int delayMilliSeconds)
        {
            this.callback = callback;
            this.timer = new DispatcherTimer();
            this.timer.Tick += timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, delayMilliSeconds);
        }

        /// <summary>
        /// The callback <see cref="EventHandler"/> which can be registered with an event.
        /// </summary>
        public void Handler(object sender, TEventArgs e)
        {
            if (this.timer.Dispatcher.CheckAccess())
            {
                lock(lockObj)
                {
                    originalSender = sender;
                    originalArgs = e;
                    this.timer.Start();
                }
            }
            else
            {
                Action startTimer = delegate
                    {
                        lock (lockObj)
                        {
                            originalSender = sender;
                            originalArgs = e;
                            this.timer.Start();
                        }
                    };

                this.timer.Dispatcher.Invoke(startTimer);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            object _sender;
            TEventArgs _args;

            lock (lockObj)
            {
                this.timer.Stop();
                _sender = originalSender;
                _args = originalArgs;
            }

            callback(_sender, _args);
        }
    }
}
