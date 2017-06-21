using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public class ListViewContextProvider : ControlContextProvider<ListView>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        public ListViewContextProvider(ListView listView, params Func<ListView, IEnumerable<ContextInfo>>[] getContextCallbacks)
            : base(listView, getContextCallbacks)
        {
            listView.SelectedIndexChanged += this.listView_SelectedIndexChanged;
        }
        #endregion

        #region ControlContextProvider Overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var control = this.Control;
                if (control != null)
                    control.SelectedIndexChanged -= this.listView_SelectedIndexChanged;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }
        #endregion
    }
}
