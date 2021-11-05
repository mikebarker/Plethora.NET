using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Plethora.Xaml
{
    public delegate void MultiEventHandler<in TEventArgs>(object[] sender, TEventArgs[] e);

    /// <summary>
    /// Class used to introduce delay into a callback.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each time the <see cref="Handler"/> delegate is called the callback-delay is started. After the specified delay
    /// has elapsed the callback is called with the arguments recevied during the most recent call to <see cref="Handler"/>.
    /// If <see cref="Handler"/> is called again before the delay has elapsed the delay is restarted.
    /// </para>
    /// <para>
    /// This can be used to prevent multiple event triggers from firing, and instead bundling them all into one.
    /// </para>
    /// <para>
    /// The class must be created on a thread associated with a <see cref="Dispatcher"/>. The callback will be called
    /// on the dispatcher thread.
    /// </para>
    /// <para>
    /// <b>NOTE:</b> It is important that a reference to the <see cref="CallbackDelayBase{TEventArgs}"/> instance be maintained at
    /// the appropriate level to ensure that it is not pre-maturely garbage collected. Usually this requires the reference
    /// to be stored as a field in the utilising view's code-behind class.
    /// </para>
    /// </remarks>
    /// <example>
    /// A delay can be introduced by re-writing:
    /// <code>
    /// <![CDATA[
    ///     ContextManager.DefaultInstance.ContextChanged += ContextManager_ContextChanged;
    /// ]]>
    /// </code>
    /// as
    /// <code>
    /// <![CDATA[
    ///     CallbackDelay<EventArgs> callbackDelay; // stored as a field to prevent garbage-collection.
    /// 
    ///     ...
    /// 
    ///     this.callbackDelay = new CallbackDelay<EventArgs>(ContextManager_ContextChanged, 10);
    ///     ContextManager.DefaultInstance.ContextChanged += this.callbackDelay.Handler;
    /// ]]>
    /// </code>
    /// </example>
    public abstract class CallbackDelayBase<TEventArgs, TDispatcherTimer>
        where TEventArgs : EventArgs
        where TDispatcherTimer : new()
    {
        private readonly object lockObj = new object();
        private LinkedList<Tuple<object, TEventArgs>> callbackParametersList;

        private readonly EventHandler<TEventArgs> singleCallback;
        private readonly MultiEventHandler<TEventArgs> multiCallback;
        private readonly TDispatcherTimer timer;

        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelayBase{TEventArgs}"/> class, specifying
        /// a callback which recevie the most recent arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with the arguments recevied during the most recent call to <see cref="Handler"/>.
        /// </remarks>
        protected CallbackDelayBase([NotNull] EventHandler<TEventArgs> callback, TimeSpan delay)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (delay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(Plethora.ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(delay)));


            this.singleCallback = callback;
            this.timer = new TDispatcherTimer();
            SubscribeToTick(this.timer, this.timer_Tick);
            SetInterval(this.timer, delay);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CallbackDelayBase{TEventArgs}"/> class, specifying
        /// a callback which recevie all arguments from calls to <see cref="Handler"/>.
        /// </summary>
        /// <param name="callback">The callback to be called after the delay has elapsed.</param>
        /// <param name="delay">The delay, after which <paramref name="callback"/> will be called.</param>
        /// <remarks>
        /// After the specified delay has elapsed the callback is called with all arguments recevied during the calls to <see cref="Handler"/>.
        /// </remarks>
        protected CallbackDelayBase([NotNull] MultiEventHandler<TEventArgs> callback, TimeSpan delay)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (delay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(Plethora.ResourceProvider.ArgMustBeGreaterThanEqualToZero(nameof(delay)));


            this.multiCallback = callback;
            this.timer = new TDispatcherTimer();
            SubscribeToTick(this.timer, this.timer_Tick);
            SetInterval(this.timer, delay);
        }

        /// <summary>
        /// The callback <see cref="EventHandler"/> which can be registered with an event.
        /// </summary>
        public async void Handler(object sender, TEventArgs e)
        {
            if (!IsInvokeRequired(this.timer))
            {
                this.RestartTimer(sender, e);
            }
            else
            {
                var task = InvokeAsync(this.timer, () => this.RestartTimer(sender, e));
                await task.ConfigureAwait(false);
            }
        }

        protected abstract void SubscribeToTick(TDispatcherTimer timer, EventHandler handler);
        protected abstract void SetInterval(TDispatcherTimer timer, TimeSpan interval);
        protected abstract bool IsInvokeRequired(TDispatcherTimer timer);
        protected abstract Task InvokeAsync(TDispatcherTimer timer, Action action);
        protected abstract void Start(TDispatcherTimer timer);
        protected abstract void Stop(TDispatcherTimer timer);

        private void RestartTimer(object sender, TEventArgs e)
        {
            Tuple<object, TEventArgs> callbackParameters = new Tuple<object, TEventArgs>(sender, e);

            lock (this.lockObj)
            {
                if (this.callbackParametersList == null)
                    this.callbackParametersList = new LinkedList<Tuple<object, TEventArgs>>();

                this.callbackParametersList.AddLast(callbackParameters);
                Start(this.timer);
            }
        }

        private void timer_Tick(object s, EventArgs e)
        {
            LinkedList<Tuple<object, TEventArgs>> _callbackParametersList;

            lock (this.lockObj)
            {
                Stop(this.timer);
                _callbackParametersList = this.callbackParametersList;
                this.callbackParametersList = null;
            }

            if (this.singleCallback != null)
            {
                Tuple<object, TEventArgs> callbackParameters = _callbackParametersList.Last.Value;

                object sender = callbackParameters.Item1;
                TEventArgs args = callbackParameters.Item2;

                this.singleCallback(sender, args);
            }

            if (this.multiCallback != null)
            {
                object[] senders = new object[_callbackParametersList.Count];
                TEventArgs[] args = new TEventArgs[_callbackParametersList.Count];

                int index = 0;
                LinkedListNode<Tuple<object, TEventArgs>> node = _callbackParametersList.First;
                while (node != null)
                {
                    senders[index] = node.Value.Item1;
                    args[index] = node.Value.Item2;

                    index++;
                    node = node.Next;
                }

                this.multiCallback(senders, args);
            }
        }
    }
}
