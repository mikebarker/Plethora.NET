using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.ComponentModel;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// Base <see cref="Control"/> for a text box which hosts a drop down.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class TextBoxWithDropDown : TextBoxEx
    {
        private readonly ControlDropDownHelper dropDownHelper;

        public TextBoxWithDropDown()
        {
            this.dropDownHelper = new ControlDropDownHelper(this);
        }

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

        #region Overrides

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
                case Keys.Escape:
                    HideDropDown();
                    break;

                case Keys.Home:
                case Keys.End:
                case Keys.PageUp:
                case Keys.PageDown:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    break;

                default:
                    ShowDropDown();
                    break;
            }
        }
        #endregion
    }
}
