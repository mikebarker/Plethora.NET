using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Windows.Forms
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class Popup : DropDownForm
    {
        #region Constructors

        protected Popup(Control control)
            : base(control)
        {
            //Validation
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            InitializeComponent();

            //Default values
            this.AcceptOnDeactivate = false;
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
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "Popup";
            this.ResumeLayout(false);

        }

        #endregion

        #region Form Overrides

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            if (AcceptOnDeactivate)
                this.Accept();
            else
                this.Cancel();
        }
        #endregion

        #region Properties

        public bool AcceptOnDeactivate
        {
            get;
            set;
        }
        #endregion

        #region Public Methods

        protected override void OnAccepted(EventArgs e)
        {
             base.OnAccepted(e);
            this.Close();
        }

        protected override void OnCancelled(EventArgs e)
        {
             base.OnCancelled(e);
            this.Close();
        }
        #endregion

        #region Static Methods

        #region TextBox

        /// <summary>
        /// Displays a text box in a popup.
        /// </summary>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="size">The size if the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        public static void TextBox(
            Point location,
            Size size,
            Action<string> onAcceptCallback)
        {
            TextBox(location, size, onAcceptCallback, string.Empty, null);
        }

        /// <summary>
        /// Displays a text box in a popup.
        /// </summary>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="size">The size if the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        /// <param name="text">The initial text to be displayed in the pop up.</param>
        /// <param name="autoCompleteValues">A set of auto completion values.</param>
        public static void TextBox(
            Point location,
            Size size,
            Action<string> onAcceptCallback,
            string text,
            string[] autoCompleteValues)
        {
            var textBox = new TextBox();
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Size = size;
            textBox.Text = text;

            if (autoCompleteValues != null)
            {
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox.AutoCompleteCustomSource.AddRange(autoCompleteValues);
            }

            TextBox(textBox, location, () => onAcceptCallback(textBox.Text));
        }

        /// <summary>
        /// Displays a text box in a popup.
        /// </summary>
        /// <param name="textBox">The text box to be displayed.</param>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        public static void TextBox(
            TextBox textBox,
            Point location,
            Action onAcceptCallback)
        {
            Action<Popup> onPopupCreated = popup =>
                {
                    textBox.KeyDown += delegate(object sender, KeyEventArgs e)
                        {
                            switch (e.KeyCode)
                            {
                                case Keys.Enter:
                                    popup.Accept();
                                    break;
                            }
                        };
                };


            ShowPopup(textBox, location, onPopupCreated, onAcceptCallback, null);
        }
        #endregion

        #region Calendar

        /// <summary>
        /// Displays a calendar in a popup.
        /// </summary>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="size">The size if the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        public static void Calendar(
            Point location,
            Size size,
            Action<DateTime> onAcceptCallback)
        {
            Calendar(location, size, onAcceptCallback, DateTime.Today);
        }

        /// <summary>
        /// Displays a calendar in a popup.
        /// </summary>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="size">The size if the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        /// <param name="date">The initial date selected in the calendar.</param>
        public static void Calendar(
            Point location,
            Size size,
            Action<DateTime> onAcceptCallback,
            DateTime date)
        {
            var calendar = new MonthCalendar();
            calendar.MaxSelectionCount = 1;
            calendar.Size = size;
            calendar.SetDate(date);

            Calendar(calendar, location, () => onAcceptCallback(calendar.SelectionStart));
        }

        /// <summary>
        /// Displays a calendar in a popup.
        /// </summary>
        /// <param name="calendar">The calendar control to be displayed.</param>
        /// <param name="location">The location of the pop up.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        public static void Calendar(
            MonthCalendar calendar,
            Point location,
            Action onAcceptCallback)
        {
            Action<Popup> onPopupCreated = popup =>
                {
                    calendar.DateSelected += delegate { popup.Accept(); };
                };

            var control = WrapWithBorder(calendar);
            ShowPopup(control, location, onPopupCreated, onAcceptCallback, null);
        }
        #endregion


        /// <summary>
        /// Displays a control in a popup.
        /// </summary>
        /// <param name="control">The control to be displayed in the popup.</param>
        /// <param name="popuplocation">The location of the pop up.</param>
        /// <param name="onPopupCreatedCallback">Callback called when the popup is created.</param>
        /// <param name="onAcceptCallback">The action to be taken when the value of the pop up is accepted.</param>
        /// <param name="onCancelCallback">The action to be taken when the value of the pop up is cancelled.</param>
        public static void ShowPopup(
            Control control,
            Point popuplocation,
            Action<Popup> onPopupCreatedCallback,
            Action onAcceptCallback,
            Action onCancelCallback)
        {
            //Validation
            if (control == null)
                throw new ArgumentNullException(nameof(control));


            var popup = new Popup(control);
            popup.Location = popuplocation;

            if (onAcceptCallback != null)
                popup.Accepted += delegate { onAcceptCallback(); };

            if (onCancelCallback != null)
                popup.Cancelled += delegate { onCancelCallback(); };

            if (onPopupCreatedCallback != null)
                onPopupCreatedCallback(popup);

            popup.Show();
        }

        public static Control WrapWithBorder(Control control)
        {
            Panel panel = new Panel();
            panel.Size = control.Size + new Size(2, 2);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Controls.Add(control);
            control.SizeChanged += delegate { panel.Size = control.Size + new Size(2, 2); };

            return panel;
        }

        #endregion
    }
}