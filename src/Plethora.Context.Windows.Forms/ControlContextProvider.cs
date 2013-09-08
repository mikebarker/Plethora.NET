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

        private readonly WeakReference reference;
        private readonly Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        protected ControlContextProvider(T control, params Func<T, IEnumerable<ContextInfo>>[] getContextCallbacks)
            : base()
        {
            if (control == null)
                throw new ArgumentNullException("control");


            this.reference = new WeakReference(control);
            this.getContextCallbacks = getContextCallbacks;

            control.Enter += control_Enter;
            control.Leave += control_Leave;
        }
        #endregion

        #region Properties

        protected T Control
        {
            get { return (T)reference.Target; }
        }

        public ActivityItemRegister ActivityItemRegister { get; set; }

        #endregion

        #region Overrides of ContextProvider

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var control = this.Control;
                if (control != null)
                {
                    control.Enter -= control_Enter;
                    control.Leave -= control_Leave;
                }
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

        private void control_Enter(object sender, EventArgs e)
        {
            this.OnEnterContext();
        }

        private void control_Leave(object sender, EventArgs e)
        {
            Control source = (Control)sender;
            if (!ReferenceEquals(source, this.Control))
                source.Leave -= control_Leave;

            Form parentForm = source.FindForm();
            Control activeControl = parentForm.ActiveControl;

            if (IsActivityControl(activeControl))
            {
                if (!ReferenceEquals(activeControl, this.Control))
                    activeControl.Leave += control_Leave;
            }
            else
            {
                this.OnLeaveContext();
            }
        }

        private bool IsActivityControl(Control control)
        {
            var itemRegister = this.ActivityItemRegister;
            if (itemRegister == null)
                return false;

            while (control != null)
            {
                if (itemRegister.IsActivityItem(control))
                    return true;

                control = control.Parent;
            }

            return false;
        }

        #endregion
    }
}
