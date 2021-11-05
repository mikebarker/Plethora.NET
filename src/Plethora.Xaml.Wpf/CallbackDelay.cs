using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Plethora.Xaml.Wpf
{
    /// <inheritdoc />
    public class CallbackDelay<TEventArgs> : CallbackDelayBase<TEventArgs, DispatcherTimer>
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelay{TEventArgs}"/> class, specifying
        /// a callback which recevie the most recent arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with the arguments recevied during the most recent call to <see cref="Handler"/>.
        /// </remarks>
        public CallbackDelay([NotNull] EventHandler<TEventArgs> callback, TimeSpan delay)
            : base(callback, delay)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelay{TEventArgs}"/> class, specifying
        /// a callback which recevie all arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with all arguments recevied during the calls to <see cref="Handler"/>.
        /// </remarks>
        public CallbackDelay([NotNull] MultiEventHandler<TEventArgs> callback, TimeSpan delay)
            : base(callback, delay)
        {
        }

        protected override bool IsInvokeRequired(DispatcherTimer timer) => !timer.Dispatcher.CheckAccess();

        protected override Task InvokeAsync(DispatcherTimer timer, Action action)
        {
            DispatcherOperation operation = timer.Dispatcher.BeginInvoke(action);
            return operation.Task;
        }

        protected override void SetInterval(DispatcherTimer timer, TimeSpan interval) => timer.Interval = interval;

        protected override void Start(DispatcherTimer timer) => timer.Start();

        protected override void Stop(DispatcherTimer timer) => timer.Stop();

        protected override void SubscribeToTick(DispatcherTimer timer, EventHandler handler) => timer.Tick += handler;
    }
}
