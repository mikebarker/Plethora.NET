using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Windows.Forms
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class DateTimeTextBox : TextBoxWithDropDown
    {
        #region Fields

        private Plethora.Windows.Forms.MonthCalendarEx calendar;
        private System.Windows.Forms.Panel panel;

        private bool isSelfUpdating = false;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="DateTimeTextBox"/> class.
        /// </summary>
        public DateTimeTextBox()
        {
            InitializeComponent();

            this.DropDownControl = panel;
        }
        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel = new Panel();
            this.calendar = new Plethora.Windows.Forms.MonthCalendarEx();
            this.SuspendLayout();
            this.panel.SuspendLayout();
            // 
            // calendar
            // 
            this.calendar.Location = new System.Drawing.Point(0, 0);
            this.calendar.MaxSelectionCount = 1;
            this.calendar.Name = "calendar";
            this.calendar.TabIndex = 0;
            this.calendar.DateDoubleClick += new System.EventHandler(this.calendar_DateDoubleClick);
            this.calendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calendar_DateSelected);
            this.calendar.SizeChanged += new System.EventHandler(this.calendar_SizeChanged);
            // 
            // panel
            // 
            this.panel.BorderStyle = BorderStyle.FixedSingle;
            this.panel.Controls.AddRange(new Control[] { this.calendar } );
            this.panel.Size = calendar.Size + new Size(2, 2);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Properties

        #region DateFormat Property

        private static readonly object DateFormatChanged_EventKey = new object();
        private const string DateFormat_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="DateFormat"/> has changed.
        /// </summary>
        public event EventHandler DateFormatChanged
        {
            add { base.Events.AddHandler(DateFormatChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(DateFormatChanged_EventKey, value); }
        }

        private string dateFormat = DateFormat_DefaultValue;

        /// <summary>
        /// Gets and sets the default format to be used to display dates.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(DateFormat_DefaultValue)]
        [Description("The default format to be used to display dates.")]
        public string DateFormat
        {
            get { return dateFormat; }
            set
            {
                if (this.dateFormat == value)
                    return;

                this.dateFormat = value;
                this.OnDateFormatChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnDateFormatChanged(EventArgs e)
        {
            var handler = base.Events[DateFormatChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion
        #endregion

        #region Nonpublic Methods

        void calendar_DateDoubleClick(object sender, EventArgs e)
        {
            HideDropDown();
        }

        void calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            this.isSelfUpdating = true;
            this.Text = calendar.SelectionStart.Date.ToString(this.DateFormat);
            this.isSelfUpdating = false;
        }

        void calendar_SizeChanged(object sender, EventArgs e)
        {
            this.panel.Size = this.calendar.Size + new Size(2, 2);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (!isSelfUpdating)
            {
                DateTime dateTime;
                if (DateTime.TryParse(this.Text, out dateTime))
                {
                    this.calendar.SelectionStart = dateTime;
                }
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            DateTime dateTime;
            if (DateTime.TryParse(this.Text, out dateTime))
            {
                this.Text = dateTime.ToString(DateFormat);
            }

            base.OnValidating(e);
        }
        #endregion
    }
}
