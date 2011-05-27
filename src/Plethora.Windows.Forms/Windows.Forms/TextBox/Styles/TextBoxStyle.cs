//------------------------------------------------------------------------------
//     Copyright 2007 The ChocolateBox Project.
//     All rights reserved.
//
//     Refer to the Licence.txt distributed with this file for licencing terms.
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// Style to be applied to text boxes.
    /// </summary>
    public class TextBoxStyle : Component, IDisposable, ICloneable
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxStyle"/> class.
        /// </summary>
        public TextBoxStyle()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxStyle"/> class,
        /// with fore and background colour, and font.
        /// </summary>
        /// <param name="foreColor">
        /// The foreground color for this style.
        /// </param>
        /// <param name="backColor">
        /// The background color for this style.
        /// </param>
        /// <param name="font">
        /// The Font for this style.
        /// </param>
        public TextBoxStyle(Color foreColor, Color backColor, Font font)
        {
            this.foreColor = foreColor;
            this.backColor = backColor;
            this.font = font;
        }
        #endregion

        #region IDisposable Implementation

        private bool disposed = false;

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~TextBoxStyle()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of managed and unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        ///  <para>
        ///   If disposing equals true, the method has been called directly
        ///   or indirectly by a user's code. Managed and unmanaged resources
        ///   can be disposed.
        ///  </para>
        ///  <para>
        ///   If disposing equals false, the method has been called by the
        ///   runtime from inside the finalizer and one should not reference
        ///   other objects. Only unmanaged resources can be disposed.
        ///  </para>
        /// </param>
        protected override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // Dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    this.font.Dispose();
                }

                // Dispose unmanaged resources.


                // Note disposing has been done.
                disposed = true;
            }
        }
        #endregion

        #region Properties

        #region ForeColor Property

        /// <summary>
        /// Raised when <see cref="ForeColor"/> changes.
        /// </summary>
        /// <seealso cref="ForeColor"/>
        public event EventHandler ForeColorChanged;

        private Color foreColor = Color.Empty;

        /// <summary>
        /// Gets and sets the foreground colour for this style.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")]
        [Description("The foreground colour for this style.")]
        public Color ForeColor
        {
            get { return foreColor; }
            set
            {
                if (value == foreColor)
                    return;

                foreColor = value;
                OnForeColorChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="ForeColorChanged"/> event.
        /// </summary>
        protected void OnForeColorChanged()
        {
            EventHandler handlers = this.ForeColorChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region BackColor Property

        /// <summary>
        /// Raised when <see cref="BackColor"/> changes.
        /// </summary>
        /// <seealso cref="BackColor"/>
        public event EventHandler BackColorChanged;

        private Color backColor = Color.Empty;

        /// <summary>
        /// Gets and sets the foreground colour for this style.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Empty")]
        [Description("The foreground colour for this style.")]
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                if (value == backColor)
                    return;

                backColor = value;
                OnBackColorChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="BackColorChanged"/> event.
        /// </summary>
        protected void OnBackColorChanged()
        {
            EventHandler handlers = this.BackColorChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region Font Property

        /// <summary>
        /// Raised when <see cref="Font"/> changes.
        /// </summary>
        /// <seealso cref="Font"/>
        public event EventHandler FontChanged;

        private Font font = null;

        /// <summary>
        /// Gets and sets the foreground colour for this style.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The foreground colour for this style.")]
        public Font Font
        {
            get { return font; }
            set
            {
                if (value == font)
                    return;

                font = value;
                OnFontChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="FontChanged"/> event.
        /// </summary>
        protected void OnFontChanged()
        {
            EventHandler handlers = this.FontChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion
        #endregion

        #region Public Methods

        /// <summary>
        /// Applies this stlye to the text box.
        /// </summary>
        /// <param name="textBox">
        /// The text box to which the style must be applied.
        /// </param>
        public void ApplyStyle(TextBox textBox)
        {
            if (this.ForeColor != Color.Empty)
                textBox.ForeColor = this.ForeColor;

            if (this.BackColor != Color.Empty)
                textBox.BackColor = this.BackColor;

            if (this.Font != null)
                textBox.Font = this.Font;
        }
        #endregion

        #region Static Methods

        /// <summary>
        ///  Applies the list of styles to the text box.
        /// </summary>
        /// <param name="textBox">
        ///  The text box to which the styles must be applied.
        /// </param>
        /// <param name="styles">
        ///  The list of styles to be applied to the text box.
        /// </param>
        /// <remarks>
        ///  The styles are applied in decresing preference order through the list.
        /// </remarks>
        public static void ApplyStyles(TextBox textBox, params TextBoxStyle[] styles)
        {
            bool isForeColorApplied = false;
            bool isBackColorApplied = false;
            bool isFontApplied = false;

            for (int i = 0; i < styles.Length; i++)
            {
                TextBoxStyle style = styles[i];

                if (style != null)
                {
                    if ((!isForeColorApplied) && (style.ForeColor != Color.Empty))
                    {
                        isForeColorApplied = true;
                        textBox.ForeColor = style.ForeColor;
                    }
                    if ((!isBackColorApplied) && (style.BackColor != Color.Empty))
                    {
                        isBackColorApplied = true;
                        textBox.BackColor = style.BackColor;
                    }
                    if ((!isFontApplied) && (style.Font != null))
                    {
                        isFontApplied = true;
                        textBox.Font = style.Font;
                    }

                    if (isForeColorApplied && isBackColorApplied && isFontApplied)
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a style using the text boxes current settings.
        /// </summary>
        /// <param name="textBox">
        /// The text box from which to base the style.
        /// </param>
        /// <returns>
        /// A <see cref="TextBoxStyle"/> based on the current settings of the
        /// text box provided.
        /// </returns>
        public static TextBoxStyle CreateStyle(TextBox textBox)
        {
            return new TextBoxStyle(textBox.ForeColor, textBox.BackColor, textBox.Font);
        }
        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new TextBoxStyle(this.ForeColor, this.BackColor, this.Font);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion
    }
}
