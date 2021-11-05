using System.Windows.Forms;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// Style to be applied to a <see cref="TextBox"/>.
    /// </summary>
    public class TextBoxStyle : ControlStyle
    {
        #region Static Members

        /// <summary>
        /// Creates a style using the text boxes current settings.
        /// </summary>
        /// <param name="textBox">
        /// The text box from which to base the style.
        /// </param>
        /// <returns>
        /// A <see cref="ControlStyle"/> based on the current settings of the
        /// text box provided.
        /// </returns>
        public static TextBoxStyle CreateStyle(TextBox textBox)
        {
            TextBoxStyle style = new TextBoxStyle();

            style.ForeColor = textBox.ForeColor;
            style.BackColor = textBox.BackColor;
            style.FontName = textBox.Font.Name;
            style.FontSize = textBox.Font.Size;
            style.FontBold = textBox.Font.Bold;
            style.FontItalic = textBox.Font.Italic;
            style.FontUnderline = textBox.Font.Underline;
            style.FontStrikeout = textBox.Font.Strikeout;

            return style;
        }
        #endregion
    }

}
