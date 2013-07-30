using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public class TextBoxContextProvider : ControlContextProvider<TextBox>
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxContextProvider"/> class.
        /// </summary>
        public TextBoxContextProvider(TextBox textBox, params Func<TextBox, IEnumerable<ContextInfo>>[] getContext)
            : base(textBox, getContext)
        {
            textBox.TextChanged += textBox_TextChanged;
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Control.TextChanged -= textBox_TextChanged;
            }
            base.Dispose(disposing);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.OnContextChanged();
        }
    }
}
