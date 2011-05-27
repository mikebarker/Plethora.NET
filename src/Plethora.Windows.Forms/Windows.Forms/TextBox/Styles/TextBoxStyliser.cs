using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// A component which can be used to apply styles to a TextBox which
    /// implements <see cref="IValueTextBox"/>
    /// </summary>
    [Designer(typeof(TextBoxStyliser.Designer))]
    public partial class TextBoxStyliser : Component, ICloneable
    {
        #region Static Members

        private static TextBoxStyliser instance;

        /// <summary>
        /// Gets the default instance of <see cref="TextBoxStyliser"/>.
        /// </summary>
        public static TextBoxStyliser Default
        {
            get
            {
                if (instance == null)
                    instance = new TextBoxStyliser();

                return instance;
            }
        }

        #endregion

        #region Fields

        private Dictionary<IValueTextBox, TextBoxStyle> textBoxStyles =
            new Dictionary<IValueTextBox, TextBoxStyle>();
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxStyliser"/> class.
        /// </summary>
        public TextBoxStyliser()
        {
            this.NegativeStyleChanged += new EventHandler(style_Changed);
            this.PositiveStyleChanged += new EventHandler(style_Changed);
            this.ZeroStyleChanged += new EventHandler(style_Changed);
            this.NullStyleChanged += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxStyliser"/> class.
        /// </summary>
        /// <param name="positiveStyle">The style to be applied when the value is positive.</param>
        /// <param name="negativeStyle">The style to be applied when the value is negative.</param>
        /// <param name="zeroStyle">The style to be applied when the value is zero.</param>
        /// <param name="nullStyle">The style to be applied when the value is null.</param>
        public TextBoxStyliser(
            TextBoxStyle positiveStyle,
            TextBoxStyle negativeStyle,
            TextBoxStyle zeroStyle,
            TextBoxStyle nullStyle)
            : this()
        {
            //Validation
            if (positiveStyle == null)
                throw new ArgumentNullException("positiveStyle");

            if (negativeStyle == null)
                throw new ArgumentNullException("negativeStyle");

            if (zeroStyle == null)
                throw new ArgumentNullException("zeroStyle");

            if (nullStyle == null)
                throw new ArgumentNullException("nullStyle");


            this.PositiveStyle = positiveStyle;
            this.NegativeStyle = negativeStyle;
            this.ZeroStyle = zeroStyle;
            this.NullStyle = nullStyle;
        }
        #endregion

        #region Properties

        #region PositiveStyle Property

        /// <summary>
        /// Raised when <see cref="PositiveStyle"/> changes.
        /// </summary>
        /// <seealso cref="PositiveStyle"/>
        public event EventHandler PositiveStyleChanged;

        private TextBoxStyle positiveStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when a text box's value is positive.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The style to be applied when a text box's value is positive.")]
        public TextBoxStyle PositiveStyle
        {
            get { return positiveStyle; }
            set
            {
                if (value == positiveStyle)
                    return;

                if (positiveStyle != null)
                    UnwireStyle(positiveStyle);

                positiveStyle = value;
                OnPositiveStyleChanged();

                if (positiveStyle != null)
                    WireStyle(positiveStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="PositiveStyleChanged"/> event.
        /// </summary>
        protected void OnPositiveStyleChanged()
        {
            EventHandler handlers = this.PositiveStyleChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region NegativeStyle Property

        /// <summary>
        /// Raised when <see cref="NegativeStyle"/> changes.
        /// </summary>
        /// <seealso cref="NegativeStyle"/>
        public event EventHandler NegativeStyleChanged;

        private TextBoxStyle negativeStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is negative.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The style to be applied when the text box's value is negative.")]
        public TextBoxStyle NegativeStyle
        {
            get { return negativeStyle; }
            set
            {
                if (value == negativeStyle)
                    return;

                if (negativeStyle != null)
                    UnwireStyle(negativeStyle);

                negativeStyle = value;
                OnNegativeStyleChanged();

                if (negativeStyle != null)
                    WireStyle(negativeStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="NegativeStyleChanged"/> event.
        /// </summary>
        protected void OnNegativeStyleChanged()
        {
            EventHandler handlers = this.NegativeStyleChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region ZeroStyle Property

        /// <summary>
        /// Raised when <see cref="ZeroStyle"/> changes.
        /// </summary>
        /// <seealso cref="ZeroStyle"/>
        public event EventHandler ZeroStyleChanged;

        private TextBoxStyle zeroStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is zero.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The style to be applied when the text box's value is zero.")]
        public TextBoxStyle ZeroStyle
        {
            get { return zeroStyle; }
            set
            {
                if (value == zeroStyle)
                    return;

                if (zeroStyle != null)
                    UnwireStyle(zeroStyle);

                zeroStyle = value;
                OnZeroStyleChanged();

                if (zeroStyle != null)
                    WireStyle(zeroStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="ZeroStyleChanged"/> event.
        /// </summary>
        protected void OnZeroStyleChanged()
        {
            EventHandler handlers = this.ZeroStyleChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region NullStyle Property

        /// <summary>
        /// Raised when <see cref="NullStyle"/> changes.
        /// </summary>
        /// <seealso cref="NullStyle"/>
        public event EventHandler NullStyleChanged;

        private TextBoxStyle nullStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is null.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The style to be applied when the text box's value is null.")]
        public TextBoxStyle NullStyle
        {
            get { return nullStyle; }
            set
            {
                if (value == nullStyle)
                    return;

                if (nullStyle != null)
                    UnwireStyle(nullStyle);

                nullStyle = value;
                OnNullStyleChanged();

                if (nullStyle != null)
                    WireStyle(nullStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="NullStyleChanged"/> event.
        /// </summary>
        protected void OnNullStyleChanged()
        {
            EventHandler handlers = this.NullStyleChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Register a TextBox with this <see cref="TextBoxStyliser"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="IValueTextBox"/> to be registered
        /// with this instance.
        /// </param>
        public void RegisterTextBox(IValueTextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            if (!(textBox is TextBox))
                throw new ArgumentException("textBox must be a TextBox which implements IValueTextBox.");


            if (!textBoxStyles.ContainsKey(textBox))
            {
                textBoxStyles.Add(textBox, TextBoxStyle.CreateStyle(textBox as TextBox));
                Stylise(textBox);
                WireTextBox(textBox);
            }
        }

        /// <summary>
        /// Deregister a TextBox from this <see cref="TextBoxStyliser"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="IValueTextBox"/> to be deregistered
        /// with this instance.
        /// </param>
        public void DeregisterTextBox(IValueTextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            if (!(textBox is TextBox))
                throw new ArgumentException("textBox must be a TextBox which implements IValueTextBox.");


            if (textBoxStyles.ContainsKey(textBox))
            {
                TextBoxStyle defaultStyle = GetDefaultStyle(textBox);

                UnwireTextBox(textBox);
                textBoxStyles.Remove(textBox);

                TextBoxStyle.ApplyStyles(textBox as TextBox, defaultStyle);
            }
        }
        #endregion

        #region Event Handlers

        void style_Changed(object sender, EventArgs e)
        {
            StyliseAllRegisteredTextBoxes();
        }

        void textBox_ValueChanged(object sender, EventArgs e)
        {
            IValueTextBox textBox = sender as IValueTextBox;
            if (textBox == null)
                return;

            Stylise(textBox);
        }

        void textBox_HandleDestroyed(object sender, EventArgs e)
        {
            IValueTextBox textBox = sender as IValueTextBox;
            if (textBox == null)
                return;

            this.DeregisterTextBox(textBox);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Wire the events of a TextBox to this styliser.
        /// </summary>
        /// <param name="textBox">
        /// The TextBox to be wired.
        /// </param>
        private void WireTextBox(IValueTextBox textBox)
        {
            ((TextBox)textBox).HandleDestroyed += new EventHandler(textBox_HandleDestroyed);
            textBox.ValueChanged += new EventHandler(textBox_ValueChanged);
        }

        /// <summary>
        /// Unwire the events of a TextBox from this styliser.
        /// </summary>
        /// <param name="textBox">
        /// The TextBox to be unwired.
        /// </param>
        private void UnwireTextBox(IValueTextBox textBox)
        {
            textBox.ValueChanged -= new EventHandler(textBox_ValueChanged);
            ((TextBox)textBox).HandleDestroyed -= new EventHandler(textBox_HandleDestroyed);
        }

        /// <summary>
        /// Wire the events of a TextBoxStyle to this styliser.
        /// </summary>
        /// <param name="style">
        /// The TextBoxStyle to be wired.
        /// </param>
        private void WireStyle(TextBoxStyle style)
        {
            style.BackColorChanged += new EventHandler(style_Changed);
            style.ForeColorChanged += new EventHandler(style_Changed);
            style.FontChanged += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Unwire the events of a TextBoxStyle from this styliser.
        /// </summary>
        /// <param name="style">
        /// The TextBoxStyle to be unwired.
        /// </param>
        private void UnwireStyle(TextBoxStyle style)
        {
            style.BackColorChanged += new EventHandler(style_Changed);
            style.ForeColorChanged += new EventHandler(style_Changed);
            style.FontChanged += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Applies the appripriate style to all text boxes registered with the
        /// styliser.
        /// </summary>
        private void StyliseAllRegisteredTextBoxes()
        {
            foreach (IValueTextBox textBox in textBoxStyles.Keys)
            {
                Stylise(textBox);
            }
        }

        /// <summary>
        /// Applies the appropriate style to the
        /// <see cref="IValueTextBox"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="IValueTextBox"/> to which the style
        /// is to be applied.
        /// </param>
        private void Stylise(IValueTextBox textBox)
        {
            TextBoxStyle defaultStyle = GetDefaultStyle(textBox);

            IComparable value = textBox.Value;
            if (value == null)
            {
                TextBoxStyle.ApplyStyles(textBox as TextBox, this.NullStyle, defaultStyle);
            }
            else
            {
                int result;
                if (NumericHelper.TypeSafeComparisonToZero(value, out result))
                {
                    if (result > 0)
                    {
                        TextBoxStyle.ApplyStyles(textBox as TextBox, this.PositiveStyle, defaultStyle);
                    }
                    else if (result < 0)
                    {
                        TextBoxStyle.ApplyStyles(textBox as TextBox, this.NegativeStyle, defaultStyle);
                    }
                    else if (result == 0)
                    {
                        TextBoxStyle.ApplyStyles(textBox as TextBox, this.ZeroStyle, defaultStyle);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the default style for a textBox.
        /// </summary>
        /// <param name="textBox">
        /// The text box for which the default style is required.
        /// </param>
        /// <returns>
        /// The default style for the text box; or 'null' one could not be
        /// found.
        /// </returns>
        private TextBoxStyle GetDefaultStyle(IValueTextBox textBox)
        {
            TextBoxStyle defaultStyle = null;
            if (this.textBoxStyles.ContainsKey(textBox))
                defaultStyle = this.textBoxStyles[textBox];

            return defaultStyle;
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
            return this.MemberwiseClone();
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
