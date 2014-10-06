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
    ///     WpfCallbackDelay callbackDelay = new WpfCallbackDelay(ContextManager_ContextChanged, 10);
    ///     ContextManager.DefaultInstance.ContextChanged += callbackDelay.Handler;
    /// </example>
    public class WpfCallbackDelay
    {
        private readonly object lockObj = new object();
        private object originalSender;
        private EventArgs originalArgs;

        private readonly EventHandler callback;
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public WpfCallbackDelay(EventHandler callback, int delayMilliSeconds)
        {
            this.callback = callback;
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, delayMilliSeconds);
        }

        /// <summary>
        /// The callback <see cref="EventHandler"/> which can be registered with an event.
        /// </summary>
        public void Handler(object sender, EventArgs e)
        {
            if (timer.Dispatcher.CheckAccess())
            {
                lock(lockObj)
                {
                    originalSender = sender;
                    originalArgs = e;
                    timer.Start();
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
                            timer.Start();
                        }
                    };

                timer.Dispatcher.Invoke(startTimer);
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            object _sender;
            EventArgs _args;

            lock (lockObj)
            {
                timer.Stop();
                _sender = originalSender;
                _args = originalArgs;
            }

            callback(_sender, _args);
        }
    }
}
