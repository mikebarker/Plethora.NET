using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public abstract class ControlContextProvider<T> : CachedContextProvider
        where T : Control
    {
        #region Fields

        private readonly WeakReference reference;
        private readonly Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        protected ControlContextProvider(T control, params Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));


            this.reference = new WeakReference(control);
            this.getContextCallbacks = getContextCallbacks;

            control.Enter += this.control_Enter;
            control.Leave += this.control_Leave;
        }
        #endregion

        #region Properties

        protected T Control
        {
            get { return (T)this.reference.Target; }
        }

        #endregion

        #region Overrides of ContextProvider

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var control = this.Control;
                if (control != null)
                {
                    control.Enter -= this.control_Enter;
                    control.Leave -= this.control_Leave;
                }
            }

            base.Dispose(disposing);
        }

        protected override IEnumerable<ContextInfo> GetContexts()
        {
            if (this.getContextCallbacks == null)
                return null;

            var control = this.Control;
            if (control == null)
                return null;

            IEnumerable<ContextInfo> contexts = null;
            foreach (var getContextCallback in this.getContextCallbacks)
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

        private void control_Enter(object sender, EventArgs e)
        {
            this.OnEnterContext();
        }

        private void control_Leave(object sender, EventArgs e)
        {
            Control source = (Control)sender;
            if (!ReferenceEquals(source, this.Control))
                source.Leave -= this.control_Leave;

            Form parentForm = source.FindForm();
            Control activeControl = parentForm.ActiveControl;

            if (this.IsActivityControl(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.Control))
                    activeControl.Leave += this.control_Leave;
            }
            else
            {
                this.OnLeaveContext();
            }
        }

        private bool IsActivityControl(Control control)
        {
            while (control != null)
            {
                if (ActivityItemRegister.Instance.IsActivityItem(control))
                    return true;

                control = control.Parent;
            }

            return false;
        }

        #endregion
    }
}
