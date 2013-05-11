using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Plethora.ComponentModel;
using Plethora.Format;
using Plethora.Windows.Forms.Base;

namespace Plethora.Windows.Forms
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class DateTimeTextBox : DateTimeTextBoxBase
    {
    }
}

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Intermediate class required to "fool" the designers to construct the <see cref="DateTimeTextBox"/>.
    /// </summary>
    [Browsable(false)]
    [ToolboxItem(false)]
    public class DateTimeTextBoxBase : ComparableTextBox<DateTime>
    {
        #region Fields

        private readonly ControlDropDownHelper dropDownHelper;
        private MonthCalendarEx calendar;
        private Panel panel;

        private bool isSelfUpdating = false;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="DateTimeTextBoxBase"/> class.
        /// </summary>
        public DateTimeTextBoxBase()
        {
            this.FormatParser = DateTimeFormatParser.Default;

            InitializeComponent();

            this.dropDownHelper = new ControlDropDownHelper(this);
            this.DropDownControl = panel;

            this.Value = DateTime.Today;
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
            this.calendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calendar_DateChanged);
            this.calendar.SizeChanged += new System.EventHandler(this.calendar_SizeChanged);
            // 
            // panel
            // 
            this.panel.BorderStyle = BorderStyle.FixedSingle;
            this.panel.Controls.AddRange(new Control[] { this.calendar });
            this.panel.Size = calendar.Size + new Size(2, 2);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Overrides of ComparableTextBox<DateTime>

        /// <summary>
        /// Validate partial values of <see cref="DateTime"/>, allowing for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The <see cref="DateTime"/> value to be validated.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid, partial value of <see cref="DateTime"/>; else 'false'.
        /// </returns>
        /// <example>
        /// Consider the case where <see cref="ComparableTextBox{T}.MinValue"/> is 25. The user wants to
        /// type the number 347. As the user types 3 this function must return true.
        /// </example>
        protected override bool ValidateValuePartial(DateTime validateValue)
        {
            return true;
        }

        /// <summary>
        /// Gets the minimum value for the type <see cref="DateTime"/>.
        /// </summary>
        protected override DateTime MinOfT
        {
            get { return DateTime.MinValue; }
        }

        /// <summary>
        /// Gets the maximum value for the type <see cref="DateTime"/>.
        /// </summary>
        protected override DateTime MaxOfT
        {
            get { return DateTime.MaxValue; }
        }

        #endregion

        #region Public Properties

        #region DropDownForm Property

        public void AcceptDropDown()
        {
            this.dropDownHelper.DropDownForm.Accept();
        }

        public void CancelDropDown()
        {
            this.dropDownHelper.DropDownForm.Cancel();
        }

        public event EventHandler DropDownAccepted
        {
            add { this.dropDownHelper.DropDownForm.Accepted += value; }
            remove { this.dropDownHelper.DropDownForm.Accepted -= value; }
        }

        public event EventHandler DropDownCancelled
        {
            add { this.dropDownHelper.DropDownForm.Cancelled += value; }
            remove { this.dropDownHelper.DropDownForm.Cancelled -= value; }
        }
        #endregion

        #region DropDownControl Property

        /// <summary>
        /// Raised when the value of <see cref="DropDownControl"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler DropDownControlChanged
        {
            add { this.dropDownHelper.DropDownControlChanged += value; }
            remove { this.dropDownHelper.DropDownControlChanged -= value; }
        }

        /// <summary>
        /// Gets and sets the control to be shown in the drop down.
        /// </summary>
        [Browsable(false)]
        public Control DropDownControl
        {
            get { return this.dropDownHelper.DropDownControl; }
            set { this.dropDownHelper.DropDownControl = value; }
        }
        #endregion
        #endregion

        #region Non-Public Members

        protected void HideDropDown()
        {
            this.dropDownHelper.HideDropDown();
        }

        protected void ShowDropDown()
        {
            this.dropDownHelper.ShowDropDown();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            ShowDropDown();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (Form.ActiveForm != this.dropDownHelper.DropDownForm)
            {
                HideDropDown();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var activeForm = Form.ActiveForm;
                if ((activeForm !=null) && (activeForm.ActiveControl == this))
                {
                    ShowDropDown();
                }
            }

            base.OnMouseClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                //Keys that neither show nor hide the drop-down
                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    break;

                case Keys.Escape:
                    HideDropDown();
                    break;

                default:
                    ShowDropDown();
                    break;
            }
        }

        protected override void OnValueChanged()
        {
            if (!isSelfUpdating)
            {
                this.calendar.SelectionStart = this.Value;
            }

            base.OnValueChanged();
        }

        void calendar_DateDoubleClick(object sender, EventArgs e)
        {
            HideDropDown();
        }

        void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            this.isSelfUpdating = true;
            this.Value = calendar.SelectionStart.Date;
            this.isSelfUpdating = false;
        }

        void calendar_SizeChanged(object sender, EventArgs e)
        {
            this.panel.Size = this.calendar.Size + new Size(2, 2);
        }
        #endregion
    }
}
