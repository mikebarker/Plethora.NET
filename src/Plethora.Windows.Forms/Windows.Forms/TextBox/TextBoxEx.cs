using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Drawing;
using Plethora.Windows.Forms.Styles;

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

        #region Constructors

        public TextBoxEx()
        {
        }
        #endregion

        #region Properties

        #region SelectAllOnEnter Property

        private static readonly object SelectAllOnEnterChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="SelectAllOnEnter"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler SelectAllOnEnterChanged
        {
            add { base.Events.AddHandler(SelectAllOnEnterChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(SelectAllOnEnterChanged_EventKey, value); }
        }

        private const bool SELECTALLONENTER_DEFAULT = true;

        private bool selectAllOnEnter = SELECTALLONENTER_DEFAULT;

        /// <summary>
        /// Gets and sets a flag indicating whether the <see cref="TextBoxBase.SelectAll"/> method is called
        /// when the user enters the text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(SELECTALLONENTER_DEFAULT)]
        [Description("If true the textbox's entire text will be selected when the TextBox is entered.")]
        public bool SelectAllOnEnter
        {
            get { return this.selectAllOnEnter; }
            set
            {
                if (this.selectAllOnEnter == value)
                    return;

                this.selectAllOnEnter = value;
                this.OnSelectAllOnEnterChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnSelectAllOnEnterChanged(EventArgs e)
        {
            var handler = base.Events[SelectAllOnEnterChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

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

        #region Styliser Property

        private static readonly object StyliserChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="Styliser"/> changes.
        /// </summary>
        /// <seealso cref="Styliser"/>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler StyliserChanged
        {
            add { base.Events.AddHandler(StyliserChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(StyliserChanged_EventKey, value); }
        }

        private TextBoxStyliser styliser = null;

        /// <summary>
        /// Gets and sets the styliser which governs the style of this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("The styliser which governs the style of this text box.")]
        public virtual TextBoxStyliser Styliser
        {
            get { return this.StyliserInternal; }
            set { this.StyliserInternal = value; }
        }

        /// <remarks>
        /// Implemented this way to allow the inheritted classes to override the
        /// return type of <see cref="Styliser"/>.
        /// </remarks>
        protected virtual TextBoxStyliser StyliserInternal
        {
            get { return styliser; }
            set
            {
                if (value == styliser)
                    return;

                if (styliser != null)
                    styliser.DeregisterTextBox(this);

                styliser = value;

                if (styliser != null)
                    styliser.RegisterTextBox(this);

                OnStyliserChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="StyliserChanged"/> event.
        /// </summary>
        protected virtual void OnStyliserChanged()
        {
            EventHandler handlers = (EventHandler)base.Events[StyliserChanged_EventKey];
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Non-Public Methods

        protected override void OnEnter(EventArgs e)
        {
            if (SelectAllOnEnter)
            {
                this.SelectAll();
            }

            base.OnEnter(e);
        }

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

        /// <summary>
        /// Disable stylising on this text box.
        /// </summary>
        protected void DisableStylising()
        {
            if (this.Styliser != null)
                this.Styliser.DeregisterTextBox(this);
        }

        /// <summary>
        /// Enable stylising on this text box.
        /// </summary>
        protected void EnableStylising()
        {
            if (this.Styliser != null)
                this.Styliser.RegisterTextBox(this);
        }
        #endregion
    }
}
