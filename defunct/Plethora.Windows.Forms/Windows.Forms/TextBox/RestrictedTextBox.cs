using System;
using System.ComponentModel;
using System.Media;
using System.Windows.Forms;
using Plethora.Win32;

namespace Plethora.Windows.Forms.Base
{
    /// <summary>
    /// Base class for a text box which restricts user input.
    /// </summary>
    /// <remarks>
    /// When inheriting from this class the <see cref="ValidateText"/> method
    /// should be overridden with a suitable method.
    /// </remarks>
    [ToolboxItem(false)]
    public class RestrictedTextBox : TextBoxEx
    {
        #region Constants

        private const Keys KeysSelectAll = (Keys)0x01;
        private const Keys KeysCopy = (Keys)0x03;
        private const Keys KeysPaste = (Keys)0x16;
        private const Keys KeysCut = (Keys)0x18;
        private const Keys KeysUndo = (Keys)0x1A;
        #endregion

        #region Events

        /// <summary>
        /// Raised when an operation would result in invalid text.
        /// </summary>
        [Category(ControlAttributes.Category.Behavior)]
        [Description("Raised when an operation would result in invalid text.")]
        public event HandledEventHandler ValidateTextFailed;
        #endregion

        #region Control Overrides

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows Message to process.</param>
        protected override void WndProc(ref Message m)
        {
            bool messageHandled = false;
            if (m.Msg == Win32Msg.WM_PASTE)
            {
                messageHandled = !ProvideValidationFeedback(ValidatePaste());
            }
            else if (m.Msg == Win32Msg.WM_CUT)
            {
                messageHandled = !ProvideValidationFeedback(ValidateCut());
            }

            if (!messageHandled)
            {
                base.WndProc(ref m);
            }
        }

        /// <summary>
        /// Processes a keyboard message.
        /// </summary>
        /// <param name="m">
        /// A Message, passed by reference, that represents the window message to
        /// process.
        /// </param>
        /// <returns>
        /// 'true' if the message was processed by the method and no further
        /// processing is required; otherwise, 'false'.
        /// </returns>
        protected override bool ProcessKeyMessage(ref Message m)
        {
            //NOTE: This method contains the variable 'keyUnModified'. To get
            //      convention Keys text as used in KeyPress events, use:
            //            Keys keys = keyUnModified | ModifierKeys;

            bool keyMessageHandled = false;

            if (m.Msg == Win32Msg.WM_KEYDOWN)
            {
                Keys keyUnModified = (Keys)m.WParam;

                if (keyUnModified == Keys.Delete)
                    keyMessageHandled = !ProvideValidationFeedback(ValidateDelete());
            }
            else if (m.Msg == Win32Msg.WM_CHAR)
            {
                Keys keyUnModified = (Keys)m.WParam;

                switch (keyUnModified)
                {
                    case Keys.Back:
                        keyMessageHandled = !ProvideValidationFeedback(ValidateBackspace());
                        break;

                    case KeysSelectAll:
                    case KeysUndo:
                    case KeysCut:
                    case KeysCopy:
                    case KeysPaste:
                        //Allow the base class to handle the key press.
                        keyMessageHandled = false;
                        break;

                    default:
                        char character = (char)m.WParam;
                        string newText = ReplaceSelectedText(character.ToString());
                        keyMessageHandled = !ProvideValidationFeedback(ValidateText(newText, true));
                        break;
                }
            }

            if (keyMessageHandled)
                return true;
            else
                return base.ProcessKeyMessage(ref m);
        }
        #endregion

        #region TextBox Overrides

        /// <summary>
        /// Gets or sets the current text in the text box.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (!ValidateText(value, false))
                    throw new ArgumentException(string.Format("'{0}' was invalid.", "Text"), "Text");

                base.Text = value;
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Validates a string to test if it is a valid text for the Text
        /// property of this control.
        /// </summary>
        /// <param name="value">
        /// The string to be validated.
        /// </param>
        /// <returns>
        /// 'true' the string is a valid text for the Text property; else,
        /// 'false'.
        /// </returns>
        public bool IsValidText(string value)
        {
            return ValidateText(value, false);
        }
        #endregion

        #region Protected Methods

        #region Validation Methods

        /// <summary>
        /// Validates the text which results from a potential delete key press.
        /// </summary>
        /// <returns>
        /// 'true' if the delete key press result in valid text; else 'false'.
        /// </returns>
        protected bool ValidateDelete()
        {
            if (this.ReadOnly)
                return false;


            string text = this.Text;
            int selStart = this.SelectionStart;
            int selLength = this.SelectionLength;

            if (selStart == text.Length)
                return true;

            string newText = null;
            if (selLength == 0)
            {
                Keys modifiers = ModifierKeys;

                if (modifiers == Keys.Control)
                {
                    //CTRL + DEL: Delete all text after the cursor upto the beginning of
                    //            the next whitespace.
                    int indexNextWord = StringHelper.IndexOfWord(text, selStart + 1);

                    if (indexNextWord < 0)
                        newText = text.Substring(0, selStart);
                    else
                        newText = text.Substring(0, selStart) + text.Substring(indexNextWord);
                }
                else
                {
                    if (selStart < text.Length - 1)
                    {
                        //DEL: Delete the next character after the cursor
                        newText = text.Substring(0, selStart) +
                                  text.Substring(selStart + 1);
                    }
                    else if (selStart == text.Length - 1)
                    {
                        //DEL: Delete the next character after the cursor (last character)
                        newText = text.Substring(0, selStart);
                    }
                }
            }
            else
            {
                //DEL: Delete the all selected characters
                newText = ReplaceSelectedText(string.Empty);
            }

            if (newText != null)
                return ValidateText(newText, true);

            return true;
        }

        /// <summary>
        /// Validates the text which results from a potential backspace key press.
        /// </summary>
        /// <returns>
        /// 'true' if the backspace key press result in valid text; else 'false'.
        /// </returns>
        protected bool ValidateBackspace()
        {
            if (this.ReadOnly)
                return false;

            string text = this.Text;
            int selStart = this.SelectionStart;
            int selLength = this.SelectionLength;

            if ((selStart == 0) && (selLength == 0))
                return true;

            string newText = null;
            if (selLength == 0)
            {
                Keys modifiers = ModifierKeys;

                if (modifiers == Keys.Control)
                {
                    //CTRL + BACKSPACE: Delete all text before the cursor upto the beginning of
                    //                  the next whitespace.
                    int indexPrevWord = StringHelper.LastIndexOfWord(text, selStart - 1);

                    if (indexPrevWord < 0)
                        newText = text.Substring(selStart);
                    else
                        newText = text.Substring(0, indexPrevWord) + text.Substring(selStart);
                }
                else
                {
                    if (selStart > 0)
                    {
                        //BACKSPACE: Delete the character before the cursor
                        newText = text.Substring(0, selStart - 1) +
                                  text.Substring(selStart);
                    }
                }
            }
            else
            {
                //BACKSPACE: Delete the all selected characters
                newText = ReplaceSelectedText(string.Empty);
            }

            if (newText != null)
                return ValidateText(newText, true);

            return false;
        }

        /// <summary>
        /// Validates the text which results from a potential cut operation.
        /// </summary>
        /// <returns>
        /// 'true' if the cut operation result in valid text; else 'false'.
        /// </returns>
        protected bool ValidateCut()
        {
            string postCutText = ReplaceSelectedText(string.Empty);

            return ValidateText(postCutText, true);
        }

        /// <summary>
        /// Validates the text which results from a potential paste operation.
        /// </summary>
        /// <returns>
        /// 'true' if the paste operation result in valid text; else 'false'.
        /// </returns>
        protected bool ValidatePaste()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                string postPasteText = ReplaceSelectedText(clipboardText);

                return ValidateText(postPasteText, true);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validates a string to test if it is a valid text for the Text
        /// property of this control.
        /// </summary>
        /// <param name="value">
        /// The string to be validated.
        /// </param>
        /// <param name="partial">
        /// 'true' if the string is still being entered by the user; else,
        /// 'false'.
        /// </param>
        /// <returns>
        /// 'true' if the string is a valid text for the Text property; else,
        /// 'false'.
        /// </returns>
        protected virtual bool ValidateText(string value, bool partial)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// Provides feedback on the result of a ValidateText method.
        /// </summary>
        /// <param name="validationResult">
        /// The result from the <see cref="ValidateText"/> method.
        /// </param>
        /// <returns>
        /// 'validationResult'.
        /// </returns>
        /// <remarks>
        /// If 'validationResult' is false, the <see cref="ValidateTextFailed"/>
        /// event is raised. If the <see cref="ValidateTextFailed"/> event is not
        /// handled, the windows beep is played.
        /// </remarks>
        /// <example>
        ///  <code>
        ///   bool result = ProvideValidationFeedback(ValidateText(text));
        ///  </code>
        /// </example>
        protected virtual bool ProvideValidationFeedback(bool validationResult)
        {
            if (!validationResult)
            {
                bool handled = OnValidateTextFailed();

                if (!handled)
                    SystemSounds.Beep.Play();
            }

            return validationResult;
        }

        /// <summary>
        /// Raises the <see cref="ValidateTextFailed"/> event.
        /// </summary>
        /// <returns>
        /// 'true' if the event was handled; else 'false'.
        /// </returns>
        protected bool OnValidateTextFailed()
        {
            HandledEventHandler handlers = this.ValidateTextFailed;
            HandledEventArgs e = new HandledEventArgs();

            if (handlers != null)
                handlers(this, e);

            return e.Handled;
        }

        /// <summary>
        /// Returns the full text of this box with the selected text replaced with
        /// new text.
        /// </summary>
        /// <param name="replacementText">
        /// The string to replace the selected text with.
        /// </param>
        /// <returns>
        /// The full text after the selected text has been replaced.
        /// </returns>
        protected string ReplaceSelectedText(string replacementText)
        {
            string text = this.Text;
            int selStart = this.SelectionStart;
            int selLength = this.SelectionLength;

            return string.Concat(
              text.Substring(0, selStart),
              replacementText,
              text.Substring(selStart + selLength));
        }

        /// <summary>
        /// Force the text of the textbox, by-passing validation of the text.
        /// </summary>
        /// <param name="text">
        /// The new Text value for the text box.
        /// </param>
        protected void ForceText(string text)
        {
            base.Text = text;
        }
        #endregion
    }
}
