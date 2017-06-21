using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Plethora.Windows.Forms;

namespace Plethora.ComponentModel
{
    [System.ComponentModel.DesignerCategory("Code")]
    public class ControlDropDownHelper : Component
    {
        #region Fields

        private readonly Control control;
        private Func<Control, Point> dropDownLocationProvider;
        private Control dropDownControl;
        private DropDownForm dropDownForm;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="ControlDropDownHelper"/> class.
        /// </summary>
        public ControlDropDownHelper(Control control)
            : this(control, BottomLeftLocation)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="ControlDropDownHelper"/> class.
        /// </summary>
        public ControlDropDownHelper(Control control, Func<Control, Point> dropDownLocationProvider)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (dropDownLocationProvider == null)
                throw new ArgumentNullException(nameof(dropDownLocationProvider));


            this.control = control;
            this.dropDownLocationProvider = dropDownLocationProvider;

            SetOwner();
        }
        #endregion

        #region Properties

        public Control Control
        {
            get { return this.control; }
        }

        public DropDownForm DropDownForm
        {
            get { return this.dropDownForm; }
        }

        #region DropDownLocationProvider

        private static readonly object DropDownLocationProviderChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="DropDownLocationProvider"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler DropDownLocationProviderChanged
        {
            add { base.Events.AddHandler(DropDownLocationProviderChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(DropDownLocationProviderChanged_EventKey, value); }
        }

        /// <summary>
        /// Gets and sets the function used to retrieve the drop down's location from the control.
        /// </summary>
        [Browsable(false)]
        public Func<Control, Point> DropDownLocationProvider
        {
            get
            {
                return this.dropDownLocationProvider;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value == this.dropDownLocationProvider)
                    return;

                this.dropDownLocationProvider = value;
                this.OnDropDownLocationProviderChanged(EventArgs.Empty);
            }
        }


        protected virtual void OnDropDownLocationProviderChanged(EventArgs e)
        {
            var handler = base.Events[DropDownLocationProviderChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this.Control, e);
        }
        #endregion

        #region DropDownControl Property

        private static readonly object DropDownControlChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="DropDownControl"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Action)]
        public event EventHandler DropDownControlChanged
        {
            add { base.Events.AddHandler(DropDownControlChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(DropDownControlChanged_EventKey, value); }
        }

        /// <summary>
        /// Gets and sets the control to be shown in the drop down.
        /// </summary>
        [Browsable(false)]
        public Control DropDownControl
        {
            get
            {
                return this.dropDownControl;
            }
            set
            {
                if (value == this.dropDownControl)
                    return;

                this.dropDownControl = value;

                if (this.dropDownForm != null)
                {
                    this.dropDownForm.Close();
                    this.dropDownForm.Dispose();
                    this.dropDownForm = null;
                }

                if (value != null)
                {
                    this.dropDownForm = new DropDownForm(value);
                    this.SetOwner();
                }

                this.OnDropDownControlChanged(EventArgs.Empty);
            }
        }


        protected virtual void OnDropDownControlChanged(EventArgs e)
        {
            var handler = base.Events[DropDownControlChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this.Control, e);
        }
        #endregion
        #endregion

        #region Public Methods

        private bool inShowDropDown = false;

        public void ShowDropDown()
        {
            if (inShowDropDown)
                return;

            if (this.dropDownForm == null)
                return;

            inShowDropDown = true;
            try
            {
                SetOwner();
                SetDropDownLocation();
                this.dropDownForm.Show();

                //Force the activation of the drop drop form; before re-activating the main form
                Application.DoEvents();

                var owner = this.Control.FindForm();
                if (owner != null)
                {
                    if (owner.MdiParent == null)
                        owner.Activate();
                    else
                        owner.MdiParent.Activate();
                }

                //Force the activation of the main form; before releasing inShowDropDown.
                Application.DoEvents();
            }
            finally
            {
                inShowDropDown = false;
            }

        }

        public void HideDropDown()
        {
            if (this.dropDownForm == null)
                return;

            this.dropDownForm.Hide();
        }
        #endregion

        #region NonPublic Methods

        private void SetDropDownLocation()
        {
            if (this.dropDownForm == null)
                return;

            var location = dropDownLocationProvider(this.Control);
            if (location != Point.Empty)
                this.dropDownForm.Location = location;
        }

        private void SetOwner()
        {
            if (this.dropDownForm == null)
                return;

            var prevOwner = this.dropDownForm.Owner;
            var owner = this.Control.FindForm();

            if (prevOwner == owner)
                return;

            //Unwire events from the previous owner
            if (prevOwner != null)
            {
                prevOwner.Activated -= owner_Activated;
                prevOwner.Deactivate -= owner_Deactivated;
                prevOwner.Move -= owner_Move;
            }

            this.dropDownForm.Owner = owner;

            //Wire events to the new owner
            if (owner != null)
            {
                owner.Activated += owner_Activated;
                owner.Deactivate += owner_Deactivated;
                owner.Move += owner_Move;
            }
        }

        void owner_Activated(object sender, EventArgs e)
        {
            var activeForm = Form.ActiveForm;
            if ((activeForm != null) &&
                (activeForm != this.dropDownForm.Owner) &&
                (activeForm.ActiveControl == this.Control))
            {
                ShowDropDown();
                dropDownForm.BringToFront();
            }
        }

        void owner_Deactivated(object sender, EventArgs e)
        {
            if (inShowDropDown)
                return;

            var activeForm = Form.ActiveForm;
            if ((activeForm != this.dropDownForm) && (activeForm != this.dropDownForm.Owner))
            {
                HideDropDown();
            }
        }

        void owner_Move(object sender, EventArgs e)
        {
            SetDropDownLocation();
        }

        private static Point BottomLeftLocation(Control control)
        {
            if (control == null)
                return Point.Empty;

            var parentForm = control.FindForm();
            if (parentForm == null)
                return Point.Empty;

            var location = new Point(control.Left, control.Bottom);
            return parentForm.PointToScreen(location);
        }
        #endregion
    }
}
