using System;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Format;
using Plethora.Win32;
using Plethora.Windows.Forms.Styles;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to nullable real numeric values.
    /// </summary>
    /// <remarks>
    ///	 <para>
    ///	  When binding to the <see cref="Value"/> property of a <see cref="NullableIntegerTextBox"/>
    ///	  the 'formattingEnabled' must be specified as true.
    ///	  <example>
    ///	   <code>NullableIntegerTextBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///	  </example>
    ///	 </para>
    /// </remarks>
    [DefaultBindingProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    public class NullableIntegerTextBox : RestrictedTextBox, IValueTextBox
    {
        #region Fields

        private EditMode editMode;
        private bool isTextUnChanged = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NullableIntegerTextBox"/> class.
        /// </summary>
        public NullableIntegerTextBox()
        {
            this.TextAlign = HorizontalAlignment.Right;
            this.FormatParser = NullableNumericFormatParser<Int64>.Default;
            this.Styliser = TextBoxStyliser.Default;
            SetTextFromValue();
            WireEvents();
        }

        #endregion

        #region RestrictedTextBox Overrides

        /// <summary>
        /// Gets or sets the current text in the text box.
        /// </summary>
        /// <seealso cref="Value"/>
        public override string Text
        {
            get { return base.Text; }
            set
            {
                // Early exit to handle the creation of the control by the form designer.
                if ((this.EditMode == EditMode.Design) && String.Equals(value, this.Name))
                    return;

                Int64? result;
                bool isValid = TryParseAndValidate(value, out result);
                if (!isValid)
                    throw new ArgumentException(ResourceProvider.ArgInvalid(@"Text"), "value");

                //Setting the value sets the text using the base class.
                this.Value = result;
            }
        }

        /// <summary>
        /// Validate the input to test for a valid Int64? value for this
        /// <see cref="NullableIntegerTextBox"/>.
        /// </summary>
        /// <param name="text">
        /// The string value to be validated.
        /// </param>
        /// <param name="partial">
        /// 'true' if the string is still being entered by the user; else,
        /// 'false'.
        /// </param>
        /// <returns>
        /// 'true' the string is a valid text for the Text property; else,
        /// 'false'.
        /// </returns>
        protected override bool ValidateText(string text, bool partial)
        {
            string textTrim = text.Trim();

            if (partial)
            {
                if (this.EditMode == EditMode.Display)
                    return true;

                return FormatParser.CanParse(textTrim, true);
            }
            else
            {
                Int64? parsedValue;
                if (!TryParse(textTrim, out parsedValue))
                    return false;

                return ValidateValue(parsedValue, partial);
            }
        }

        /// <summary />
        protected override bool ProcessKeyMessage(ref Message m)
        {
            bool keyMessageHandled = false;

            if (m.Msg == Win32Msg.WM_CHAR)
            {
                Keys keyUnModified = (Keys)m.WParam;

                switch (keyUnModified)
                {
                    case Keys.Escape:
                        SetTextFromValue();
                        SelectAll();
                        IsTextUnChanged = true;
                        keyMessageHandled = true;
                        break;
                }
            }

            if (keyMessageHandled)
                return true;
            else
                return base.ProcessKeyMessage(ref m);
        }

        /// <summary />
        protected override void OnEnter(EventArgs e)
        {
            this.EditMode = EditMode.Edit;

            base.OnEnter(e);
        }

        /// <summary />
        protected override void OnValidating(CancelEventArgs e)
        {
            bool isValid = TrySetValueFromText();
            if (isValid)
            {
                base.OnValidating(e);
            }
            else
            {
                ProvideValidationFeedback(false);
                e.Cancel = true;
            }
        }

        /// <summary />
        protected override void OnValidated(EventArgs e)
        {
            this.EditMode = EditMode.Display;

            base.OnValidated(e);
        }
        #endregion

        #region IValueTextBox Implementation

        /// <summary>
        /// The value represented by this text box.
        /// </summary>
        IComparable IValueTextBox.Value
        {
            get { return this.Value; }
        }
        #endregion

        #region Properties

        #region FormatParser

        /// <summary>
        /// Raised when <see cref="FormatParser"/> changes.
        /// </summary>
        /// <seealso cref="FormatParser"/>
        public event EventHandler FormatParserChanged;

        private IFormatParserPartial<Int64?> formatParser = null;

        /// <summary>
        /// Gets and sets the formatter for this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Data)]
        [Description("The formatter for this text box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [RefreshProperties(RefreshProperties.All)]
        public IFormatParserPartial<Int64?> FormatParser
        {
            get { return this.formatParser; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (this.formatParser != value)
                {
                    UnWireFormatParser();
                    this.formatParser = value;
                    WireFormatParser();
                    OnFormatParserChanged();
                }
                SetTextFromValue();
            }
        }

        /// <summary>
        /// Raises the <see cref="FormatParserChanged"/> event.
        /// </summary>
        protected void OnFormatParserChanged()
        {
            EventHandler handlers = this.FormatParserChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region Value Property

        /// <summary>
        /// Raised when <see cref="Value"/> changes.
        /// </summary>
        /// <seealso cref="Value"/>
        public event EventHandler ValueChanged;

        private const Int64 DEFAULT_VALUE = 0;

        private Int64? value = DEFAULT_VALUE;

        /// <summary>
        /// Gets and sets the value represented by the text of this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Data)]
        [DefaultValue(DEFAULT_VALUE)]
        [Description("The value represented by the text of this text box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [RefreshProperties(RefreshProperties.All)]
        public Int64? Value
        {
            get { return this.value; }
            set
            {
                if (this.value != value)
                {
                    if (!ValidateValue(value, false))
                        throw new ArgumentException(ResourceProvider.ArgInvalid(@"value"), "value");

                    this.value = value;
                    OnValueChanged();
                }
                SetTextFromValue();
            }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        protected void OnValueChanged()
        {
            EventHandler handlers = this.ValueChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region MinValue Property

        /// <summary>
        /// Raised when <see cref="MinValue"/> changes.
        /// </summary>
        /// <seealso cref="MinValue"/>
        public event EventHandler MinValueChanged;

        private Int64 minValue = Int64.MinValue;

        /// <summary>
        /// Gets and sets the minimum value which this text box can contain.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(Int64.MinValue)]
        [Description("The minimum value which this text box can contain.")]
        public Int64 MinValue
        {
            get { return minValue; }
            set
            {
                if (value > maxValue)
                    throw new ArgumentOutOfRangeException("value", value,
                        ResourceProvider.ArgMustBeLessThanEqualTo("value", "MaxValue"));

                if (value == minValue)
                    return;

                minValue = value;
                OnMinValueChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MinValueChanged"/> event.
        /// </summary>
        protected void OnMinValueChanged()
        {
            EventHandler handlers = this.MinValueChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region MaxValue Property

        /// <summary>
        /// Raised after <see cref="MaxValue"/> has changed.
        /// </summary>
        /// <seealso cref="MaxValue"/>
        public event EventHandler MaxValueChanged;

        private Int64 maxValue = Int64.MaxValue;

        /// <summary>
        /// Gets and sets the maximum value which this text box can contain.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(Int64.MaxValue)]
        [Description("The maximum value which this text box can contain.")]
        public Int64 MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value < minValue)
                    throw new ArgumentOutOfRangeException("value", value,
                        ResourceProvider.ArgMustBeGreaterThanEqualTo("value", "MinValue"));

                if (value == maxValue)
                    return;

                maxValue = value;
                OnMaxValueChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxValueChanged"/> event.
        /// </summary>
        protected void OnMaxValueChanged()
        {
            EventHandler handlers = this.MaxValueChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region Styliser Property

        /// <summary>
        /// Raised when <see cref="Styliser"/> changes.
        /// </summary>
        /// <seealso cref="Styliser"/>
        public event EventHandler StyliserChanged;

        private TextBoxStyliser styliser = null;

        /// <summary>
        /// Gets and sets the styliser which governs the style of this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("The styliser which governs the style of this text box.")]
        public TextBoxStyliser Styliser
        {
            get { return styliser; }
            set
            {
                if (value == styliser)
                    return;

                if (styliser != null)
                    styliser.DeregisterTextBox(this);

                styliser = value;

                if (styliser != null)
                    styliser.RegisterTextBox(this);

                OnStyliserChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="StyliserChanged"/> event.
        /// </summary>
        protected void OnStyliserChanged()
        {
            EventHandler handlers = this.StyliserChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Gets and sets the edit mode of this text box.
        /// </summary>
        [Browsable(false)]
        protected EditMode EditMode
        {
            get
            {
                if (this.IsDesignMode())
                    return EditMode.Design;

                return this.editMode;
            }
            set
            {
                //Validation
                if ((value != EditMode.Display) && (value != EditMode.Edit))
                    throw new ArgumentException(ResourceProvider.ArgInvalid(@"value"), "value");


                EditMode previousEditMode = this.EditMode;

                this.editMode = value;

                if (this.EditMode != previousEditMode)
                {
                    switch (this.EditMode)
                    {
                        case EditMode.Edit:
                            IsTextUnChanged = true;
                            DisableStyles();
                            SetTextFromValue();
                            break;

                        case EditMode.Display:
                            EnableStyles();
                            if (!IsTextUnChanged)
                            {
                                SetTextFromValue();
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets a value indicating whether the text has changed since
        /// editting began.
        /// </summary>
        [Browsable(false)]
        private bool IsTextUnChanged
        {
            get { return this.isTextUnChanged; }
            set { this.isTextUnChanged = value; }
        }
        #endregion

        #region Event Handlers

        private void NullableIntegerTextBox_MaxValueChanged(object sender, EventArgs e)
        {
            if (this.EditMode == EditMode.Display)
            {
                if (this.Value > this.MaxValue)
                    this.Value = this.MaxValue;
            }
        }

        private void NullableIntegerTextBox_MinValueChanged(object sender, EventArgs e)
        {
            if (this.EditMode == EditMode.Display)
            {
                if (this.Value < this.MinValue)
                    this.Value = this.MinValue;
            }
        }

        private void NullableIntegerTextBox_TextChanged(object sender, EventArgs e)
        {
            IsTextUnChanged = false;
        }

        private void formatParser_Changed(object sender, EventArgs e)
        {
            SetTextFromValue();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Validate the Int64? value for this <see cref="NullableIntegerTextBox"/>, allowing
        /// for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The Int64? value to be validated.
        /// </param>
        /// <param name="partial">
        /// 'true' if the method must allow for the construction of valid values, not just the
        /// valid value themselves; else false.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid Int64? value; else 'false'.
        /// </returns>
        /// <example>
        /// Consider the case where <see cref="MinValue"/> is 25. The user wants to
        /// type the number 347. As the user types 3 this function must return true if
        /// <paramref name="partial"/> is 'true'.
        /// </example>
        protected virtual bool ValidateValue(Int64? validateValue, bool partial)
        {
            if (validateValue == null)
                return true;

            if (partial)
            {
                //NOTE: Must allow for the user to type in the value, so can't simply
                //      test that the value lies between minValue and maxValue.
                if ((minValue < 0) && (validateValue < minValue) ||
                    (maxValue > 0) && (validateValue > maxValue))
                    return false;
            }
            else
            {
                if ((validateValue < minValue) || (validateValue > maxValue))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validate the Int64? value for this <see cref="NullableIntegerTextBox"/>, allowing
        /// for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The Int64? value to be validated.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid Int64? value; else 'false'.
        /// </returns>
        protected bool ValidateValue(Int64? validateValue)
        {
            bool partialValidationRequired = (this.EditMode != EditMode.Edit);

            return ValidateValue(validateValue, partialValidationRequired);
        }

        /// <summary>
        /// Converts the string representation of a number to its Int64? equivalent.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="text">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the Int64? value equivalent to the
        /// number contained in 'text', if the conversion succeeded, or zero if the
        /// conversion failed.
        /// </param>
        /// <returns>
        /// 'true' if 'text' was converted successfully; otherwise, 'false'.
        /// </returns>
        protected virtual bool TryParse(string text, out Int64? result)
        {
            bool isValid = FormatParser.TryParse(
                text,
                out result);

            if (!isValid)
                result = DEFAULT_VALUE;

            return isValid;
        }

        /// <summary>
        /// Converts the string representation of a number to its Int64? equivalent,
        /// testing if the resulting parsed value is valid for this
        /// <see cref="NullableIntegerTextBox"/>. A return value indicates whether the
        /// operation succeeded.
        /// </summary>
        /// <param name="text">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the Int64? value equivalent to the
        /// number contained in 'text', if the conversion and validation succeeded,
        /// or zero if the conversion failed.
        /// </param>
        /// <returns>
        /// 'true' if 'text' was converted and validated successfully; otherwise,
        /// 'false'.
        /// </returns>
        protected bool TryParseAndValidate(string text, out Int64? result)
        {
            bool isValid = TryParse(text, out result);
            if (!isValid)
                return false;

            isValid = this.ValidateValue(result);
            if (!isValid)
            {
                result = DEFAULT_VALUE;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the text property of this text box, according to the value.
        /// </summary>
        protected void SetTextFromValue()
        {
            bool displayNullValueAsEmptyString = (this.EditMode != EditMode.Display);

            ForceText(FormatParser.Format(this.Value, displayNullValueAsEmptyString));
        }

        /// <summary>
        /// Sets the text property of this text box, according to the value.
        /// </summary>
        protected bool TrySetValueFromText()
        {
            string text = this.Text;

            Int64? result;
            bool isValid = TryParseAndValidate(text, out result);
            if (isValid)
                this.Value = result;

            return isValid;
        }

        /// <summary>
        /// Disable stylising on this text box.
        /// </summary>
        protected void DisableStyles()
        {
            this.Styliser.DeregisterTextBox(this);
        }

        /// <summary>
        /// Enable stylising on this text box.
        /// </summary>
        protected void EnableStyles()
        {
            this.Styliser.RegisterTextBox(this);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Wires the required events.
        /// </summary>
        private void WireEvents()
        {
            this.MaxValueChanged += NullableIntegerTextBox_MaxValueChanged;
            this.MinValueChanged += NullableIntegerTextBox_MinValueChanged;

            this.TextChanged += NullableIntegerTextBox_TextChanged;
        }

        /// <summary>
        /// Wires the events of the format parser.
        /// </summary>
        private void WireFormatParser()
        {
            if (this.formatParser == null)
                return;

            this.formatParser.Changed += formatParser_Changed;
        }

        /// <summary>
        /// Unwires the events of the format parser.
        /// </summary>
        private void UnWireFormatParser()
        {
            if (this.formatParser == null)
                return;

            this.formatParser.Changed -= formatParser_Changed;
        }
        #endregion

    }
}
