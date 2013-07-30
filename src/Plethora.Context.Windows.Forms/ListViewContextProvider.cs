using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public class ListViewContextProvider : ControlContextProvider<ListView>
    {
        #region Fields

        private int prevSelectedItemsCount = 0;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        public ListViewContextProvider(ListView listView, params Func<ListView, IEnumerable<ContextInfo>>[] getContextCallbacks)
            : base(listView, getContextCallbacks)
        {
            listView.SelectedIndexChanged += listView_SelectedIndexChanged;
        }
        #endregion

        #region ControlContextProvider Overrides

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var control = this.Control;
                if (control != null)
                    control.SelectedIndexChanged -= listView_SelectedIndexChanged;
            }
            base.Dispose(disposing);
        }

        protected override void OnEnterContext(object sender, EventArgs e)
        {
            var control = this.Control;
            if ((control != null) && (control.SelectedItems.Count != 0))
                base.OnEnterContext(sender, e);
        }
        #endregion

        #region Private Methods

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;

            if ((listView.SelectedItems.Count == 0) && (prevSelectedItemsCount != 0))
                this.OnLeaveContext();
            else if ((listView.SelectedItems.Count != 0) && (prevSelectedItemsCount == 0))
                this.OnEnterContext();
            else
                this.OnContextChanged();

            prevSelectedItemsCount = listView.SelectedItems.Count;
        }
        #endregion
    }
}
