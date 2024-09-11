using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plethora
{
    /// <summary>
    /// Class used to create an event with a weak reference to the target.
    /// </summary>
    /// <typeparam name="TEventHandler">A type describing the <see cref="Delegate"/> of the event.</typeparam>
    /// <remark>
    /// This class is intended to be used in a method similar to:
    ///  <example>
    /// <![CDATA[
    ///        private readonly WeakEvent<EventHandler> myWeakEvent = new WeakEvent<EventHandler>();
    /// 
    ///        public event EventHandler MyWeakEvent
    ///        {
    ///            add { myWeakEvent.Add(value); }
    ///            remove { myWeakEvent.Remove(value); }
    ///        }
    /// 
    ///        protected virtual void OnWeakEvent(object sender, EventArgs e)
    ///        {
    ///            foreach (var handler in myWeakEvent.GetInvocationList())
    ///            {
    ///                handler?.Invoke(sender, e);
    ///            }
    ///        }
    /// ]]>
    ///  </example>
    /// </remark>
    public class WeakEvent<TEventHandler>
        where TEventHandler : notnull
    {
        #region Internal Class

        private class HandlerInfo
        {
            internal readonly WeakReference WeakTarget;
            internal readonly MethodInfo Method;
            internal readonly TEventHandler Handler;

            public HandlerInfo(TEventHandler handler, WeakEvent<TEventHandler> parent)
            {
                var @delegate = (Delegate)(object)handler;

                this.WeakTarget = new(@delegate.Target);
                this.Method = @delegate.Method;
                this.Handler = WeakDelegate.CreateWeakDelegate(handler, h => parent.Remove(this));
            }
        }

        #endregion

        #region Field

        private readonly List<HandlerInfo> innerDelegates = new(0);
        private readonly object @lock = new();

        #endregion

        #region Methods

        /// <summary>
        /// Add a delegate to the invocation list.
        /// </summary>
        public void Add(TEventHandler handler)
        {
            HandlerInfo info = new(handler, this);
            lock (this.@lock)
            {
                this.innerDelegates.Add(info);
            }
        }

        /// <summary>
        /// Remove a delegate from the invocation list.
        /// </summary>
        public void Remove(TEventHandler handler)
        {
            var @delegate = (Delegate)(object)handler;

            var target = @delegate.Target;
            var method = @delegate.Method;

            lock (this.@lock)
            {
                for (int i = 0; i < this.innerDelegates.Count; i++)
                {
                    var info = this.innerDelegates[i];

                    if ((info.WeakTarget.Target == target) && (info.Method == method))
                    {
                        this.innerDelegates.RemoveAt(i);
                        return;
                    }
                }
            }
        }

        private void Remove(HandlerInfo info)
        {
            lock (this.@lock)
            {
                this.innerDelegates.Remove(info);
            }
        }

        /// <summary>
        /// Returns the invocation list of the <typeparamref name="TEventHandler"/>.
        /// </summary>
        public TEventHandler[] GetInvocationList()
        {
            lock (this.@lock)
            {
                return this.innerDelegates
                    .Select(info => info.Handler)
                    .ToArray();
            }
        }

        #endregion
    }
}
