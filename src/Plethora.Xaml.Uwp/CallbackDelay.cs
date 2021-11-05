using JetBrains.Annotations;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Plethora.Xaml.Uwp
{
    /// <inheritdoc />
    public class CallbackDelay<TEventArgs> : CallbackDelayBase<TEventArgs, DispatcherTimer>
        where TEventArgs : EventArgs
    {
        private readonly CoreDispatcher dispatcher;
        private readonly CoreDispatcherPriority priority;

        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelay{TEventArgs}"/> class, specifying
        /// a callback which recevie the most recent arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <param name="dispatcher">The <see cref="CoreDispatcher"/> on which <paramref name="callback"/> is to be called.</param>
        /// <param name="priority">The priority with which <paramref name="callback"/> is to be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with the arguments recevied during the most recent call to <see cref="Handler"/>.
        /// </remarks>
        public CallbackDelay(
            [NotNull] EventHandler<TEventArgs> callback,
            TimeSpan delay,
            [NotNull] CoreDispatcher dispatcher,
            CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
            : base(callback, delay)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));


            this.dispatcher = dispatcher;
            this.priority = priority;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelay{TEventArgs}"/> class, specifying
        /// a callback which recevie all arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <param name="dispatcher">The <see cref="CoreDispatcher"/> on which <paramref name="callback"/> is to be called.</param>
        /// <param name="priority">The priority with which <paramref name="callback"/> is to be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with all arguments recevied during the calls to <see cref="Handler"/>.
        /// </remarks>
        public CallbackDelay(
            [NotNull] MultiEventHandler<TEventArgs> callback,
            TimeSpan delay,
            [NotNull] CoreDispatcher dispatcher,
            CoreDispatcherPriority priority = CoreDispatcherPriority.Normal)
            : base(callback, delay)
        {
            if (dispatcher == null)
                throw new ArgumentNullException(nameof(dispatcher));


            this.dispatcher = dispatcher;
            this.priority = priority;
        }

        protected override bool IsInvokeRequired(DispatcherTimer timer) => !this.dispatcher.HasThreadAccess;

        protected override Task InvokeAsync(DispatcherTimer timer, Action action)
        {
            var operation = this.dispatcher.RunAsync(priority, () => action());
            return operation.AsTask();
        }

        protected override void SetInterval(DispatcherTimer timer, TimeSpan interval) => timer.Interval = interval;

        protected override void Start(DispatcherTimer timer) => timer.Start();

        protected override void Stop(DispatcherTimer timer) => timer.Stop();

        protected override void SubscribeToTick(DispatcherTimer timer, EventHandler handler)
        {
            timer.Tick += (sender, e) => handler(sender, EventArgs.Empty);
        }
    }
}
