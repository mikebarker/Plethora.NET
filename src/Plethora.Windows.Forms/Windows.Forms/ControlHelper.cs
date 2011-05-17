using System.Windows.Forms;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// Helper class for working with controls.
    /// </summary>
    public static class ControlHelper
    {
        #region Public Static Methods

        /// <summary>
        /// Tests if this control or any of its ancestors are in design mode.
        /// </summary>
        /// <param name="value">The control to be tested.</param>
        /// <returns>
        /// 'true' if the control or one of its ancestors is in design mode; else
        /// 'false'.
        /// </returns>
        public static bool IsDesignMode(this Control value)
        {
            Control ctrl = value;

            while (ctrl != null)
            {
                if ((ctrl.Site != null) && (ctrl.Site.DesignMode))
                    return true;

                ctrl = ctrl.Parent;
            }

            return false;
        }
        #endregion
    }
}
