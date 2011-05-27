using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Drawing;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox with additional triple click functionality.
    /// </summary>
    [ToolboxBitmapEx(typeof(Properties.Resources), "TextBox")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class TextBoxEx : TextBox
    {
        #region Fields

        private static readonly long doubleClickTicks = TimeSpan.TicksPerMillisecond * SystemInformation.DoubleClickTime;

        private long prevDblClickTimeStamp = DateTime.MinValue.Ticks;
        #endregion

        #region Events

        #region MouseTripleClick Event

        private static readonly object MouseTripleClick_EventKey = new object();

        /// <summary>
        /// Raised when the control is triple-clicked.
        /// </summary>
        [Category(ControlAttributes.Category.Action)]
        [Description("Raised when the control is triple-clicked.")]
        public event MouseEventHandler MouseTripleClick
        {
            add { base.Events.AddHandler(MouseTripleClick_EventKey, value); }
            remove { base.Events.RemoveHandler(MouseTripleClick_EventKey, value); }
        }

        protected virtual void OnMouseTripleClick(MouseEventArgs e)
        {
            var handler = base.Events[MouseTripleClick_EventKey] as MouseEventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region TripleClick Event

        private static readonly object TripleClick_EventKey = new object();

        /// <summary>
        /// Raised when the control is triple-clicked.
        /// </summary>
        [Category(ControlAttributes.Category.Action)]
        [Description("Raised when the control is triple-clicked.")]
        public event EventHandler TripleClick
        {
            add { base.Events.AddHandler(TripleClick_EventKey, value); }
            remove { base.Events.RemoveHandler(TripleClick_EventKey, value); }
        }

        protected virtual void OnTripleClick(EventArgs e)
        {
            var handler = base.Events[TripleClick_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            if (this.SelectAllOnTripleClick)
                this.SelectAll();
        }

        #endregion
        #endregion

        #region Properties

        #region SelectAllOnTripleClick Property

        private static readonly object SelectAllOnTripleClickChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="SelectAllOnTripleClick"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler SelectAllOnTripleClickChanged
        {
            add { base.Events.AddHandler(SelectAllOnTripleClickChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(SelectAllOnTripleClickChanged_EventKey, value); }
        }

        private const bool SELECTALLONTRIPLECLICK_DEFAULT = true;

        private bool selectAllOnTripleClick = SELECTALLONTRIPLECLICK_DEFAULT;

        /// <summary>
        /// Gets and sets a flag indicating whether the <see cref="TextBoxBase.SelectAll"/> method is called
        /// when the user Triple clicks.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(SELECTALLONTRIPLECLICK_DEFAULT)]
        [Description("If true the textbox's entire text will be selected on Triple click.")]
        public bool SelectAllOnTripleClick
        {
            get { return this.selectAllOnTripleClick; }
            set
            {
                if (this.selectAllOnTripleClick == value)
                    return;

                this.selectAllOnTripleClick = value;
                this.OnSelectAllOnTripleClickChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnSelectAllOnTripleClickChanged(EventArgs e)
        {
            var handler = base.Events[SelectAllOnTripleClickChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion
        #endregion

        #region Methods

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if ((DateTime.Now.Ticks - prevDblClickTimeStamp) < doubleClickTicks)
            {
                MouseEventArgs mouseEventArgs = new MouseEventArgs(e.Button, 3, e.X, e.Y, e.Delta);
                this.OnMouseTripleClick(mouseEventArgs);
                this.OnTripleClick(EventArgs.Empty);
            }
            else
            {
                base.OnMouseClick(e);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            prevDblClickTimeStamp = DateTime.Now.Ticks;

            base.OnMouseDoubleClick(e);
        }
        #endregion
    }
}
