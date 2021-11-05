using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Plethora.IO
{
    /// <summary>
    /// TextWriter which directs the output to a textbox.
    /// </summary>
    public class TextBoxWriter : TextWriter
    {
        #region Fields

        private TextBoxBase txt;
        private StringBuilder sb;
        private Encoding encoding;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxWriter"/>
        /// class.
        /// </summary>
        /// <param name="txt">
        /// The control to which data must be routed.
        /// </param>
        public TextBoxWriter(TextBoxBase txt)
            : this(txt, CultureInfo.CurrentCulture)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxWriter"/>
        /// class.
        /// </summary>
        /// <param name="txt">
        /// The control to which data must be routed.
        /// </param>
        /// <param name="formatProvider">
        /// An IFormatProvider object that controls formatting.
        /// </param>
        public TextBoxWriter(TextBoxBase txt, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            //Validation
            if (txt == null)
                throw new ArgumentNullException(nameof(txt));


            this.sb = new StringBuilder();
            this.txt = txt;
            this.txt.HandleCreated += txt_HandleCreated;
        }
        #endregion

        #region TextWriter Overrides

        /// <summary>
        /// Gets the Encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                if (encoding == null)
                    encoding = Encoding.Unicode;

                return encoding;
            }
        }

        /// <summary>
        /// Releases all resources used by the TextWriter object
        /// </summary>
        /// <param name="disposing">
        /// 'true' to release both managed and unmanaged resources; 'false' to
        /// release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            this.sb = null;
            this.txt = null;
            base.Dispose(disposing);
        }

        /// <summary>
        /// Writes a character to the TextBox.
        /// </summary>
        /// <param name="value">
        /// The character to write.
        /// </param>
        public override void Write(char value)
        {
            Append(value.ToString());
        }

        /// <summary>
        /// Writes a character array to the TextBox.
        /// </summary>
        /// <param name="buffer">
        /// The character to write.
        /// </param>
        public override void Write(char[] buffer)
        {
            if (buffer == null)
                return;

            Append(new string(buffer));
        }

        /// <summary>
        /// Write a protion of a character array to the TextBox.
        /// </summary>
        /// <param name="buffer">
        /// The character array to write data from.
        /// </param>
        /// <param name="index">
        /// Starting index in the buffer.
        /// </param>
        /// <param name="count">
        /// The number of characters to write.
        /// </param>
        public override void Write(char[] buffer, int index, int count)
        {
            //Validation
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), ResourceProvider.ArgMustBeGreaterThanZero(nameof(index)));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), ResourceProvider.ArgMustBeGreaterThanZero(nameof(count)));

            if (buffer == null)
                return;

            if ((buffer.Length - index) < count)
                throw new ArgumentException(ResourceProvider.ArgMustBeLessThan(nameof(count), "buffer.Length - index"));

            
            Append(new string(buffer, index, count));
        }

        /// <summary>
        /// Writes a string to the TextBox.
        /// </summary>
        /// <param name="value">
        /// The string to write.
        /// </param>
        public override void Write(string value)
        {
            if (value == null)
                return;

            Append(value);
        }

        #endregion

        #region Event Handlers

        private void txt_HandleCreated(object sender, EventArgs e)
        {
            if (sb != null)
                txt.Text = sb.ToString();
        }
        #endregion

        #region Protected Method

        /// <summary>
        /// Appends the string to the TextBox.
        /// </summary>
        /// <param name="value">
        /// The string to be written to the TextBox.
        /// </param>
        protected virtual void Append(string value)
        {
            if (this.sb == null)
                throw new InvalidOperationException(ResourceProvider.InvalidState());


            sb.Append(value);
            if (txt.IsHandleCreated)
                txt.Text = sb.ToString();
        }
        #endregion
    }
}
