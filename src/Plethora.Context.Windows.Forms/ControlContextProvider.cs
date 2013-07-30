using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public abstract class ControlContextProvider<T> : ContextProvider
        where T : Control
    {
        #region Fields

        private readonly WeakReference<T> reference;
        private readonly Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        protected ControlContextProvider(T control, params Func<T, ContextInfo>[] getContextCallbacks)
            : this(control, getContextCallbacks.Select(AsEnumerable).ToArray())
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        protected ControlContextProvider(T control, params Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks)
            : base()
        {
            if (control == null)
                throw new ArgumentNullException("control");


            this.reference = new WeakReference<T>(control);
            this.getContextCallbacks = getContextCallbacks;

            control.GotFocus += control_GotFocus;
            control.LostFocus += control_LostFocus;
        }
        #endregion

        #region Properties

        protected T Control
        {
            get { return reference.Target; }
        }
        #endregion

        #region Overrides of ContextProvider

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var control = reference.Target;
                control.GotFocus -= control_GotFocus;
                control.LostFocus -= control_LostFocus;
            }

            base.Dispose(disposing);
        }

        public override IEnumerable<ContextInfo> GetContexts()
        {
            if (getContextCallbacks == null)
                return null;

            var control = this.Control;
            if (control == null)
                return null;

            IEnumerable<ContextInfo> contexts = null;
            foreach (var getContextCallback in getContextCallbacks)
            {
                if (contexts == null)
                    contexts = getContextCallback(control);
                else
                    contexts = contexts.Concat(getContextCallback(control));
            }

            return contexts;
        }

        #endregion

        #region Private Methods

        private void control_LostFocus(object sender, EventArgs e)
        {
            this.OnLeaveContext();
        }

        private void control_GotFocus(object sender, EventArgs e)
        {
            this.OnEnterContext();
        }

        private static Func<T, IEnumerable<ContextInfo>> AsEnumerable(Func<T, ContextInfo> func)
        {
            return t => Enumerable.Repeat(func(t), 1);
        }
        #endregion
    }
}
