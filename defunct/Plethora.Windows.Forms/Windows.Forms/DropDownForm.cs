using System;
using System.Windows.Forms;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// <see cref="Form"/> which forms the basis of drop downs and popups.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class DropDownForm : Form
    {
        #region Fields

        private readonly Control control;
        #endregion

        #region Events

        #region Accepted Event

        private static readonly object Accepted_EventKey = new object();

        public event EventHandler Accepted
        {
            add { base.Events.AddHandler(Accepted_EventKey, value); }
            remove { base.Events.RemoveHandler(Accepted_EventKey, value); }
        }

        protected void OnAccepted()
        {
            OnAccepted(EventArgs.Empty);
        }

        protected virtual void OnAccepted(EventArgs e)
        {
            var handler = base.Events[Accepted_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Cancelled Event

        private static readonly object Cancelled_EventKey = new object();

        public event EventHandler Cancelled
        {
            add { base.Events.AddHandler(Cancelled_EventKey, value); }
            remove { base.Events.RemoveHandler(Cancelled_EventKey, value); }
        }

        protected void OnCancelled()
        {
            OnCancelled(EventArgs.Empty);
        }

        protected virtual void OnCancelled(EventArgs e)
        {
            var handler = base.Events[Cancelled_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion
        #endregion

        #region Constructors

        private DropDownForm()
        {
            InitializeComponent();
        }

        public DropDownForm(Control control)
            : this()
        {
            //Validation
            if (control == null)
                throw new ArgumentNullException(nameof(control));


            this.control = control;
            this.control.SizeChanged += control_SizeChanged;
            this.ClientSize = this.control.Size;

            this.SuspendLayout();
            this.Controls.Add(control);
            this.ResumeLayout(true);
        }
        #endregion

        #region Private Methods

        void control_SizeChanged(object sender, EventArgs e)
        {
            this.ClientSize = this.control.Size;
        }
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DropDownForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DropDownForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }

        #endregion

        #region Form Overrides

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Cancel();
                    break;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            BringToFront();
            base.OnShown(e);
        }
        #endregion

        #region Public Methods

        public void Accept()
        {
            this.DialogResult = DialogResult.OK;
            this.OnAccepted();
        }

        public void Cancel()
        {
            this.DialogResult = DialogResult.Cancel;
            this.OnCancelled();
        }
        #endregion
    }
}