using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Format;
using Plethora.Win32;

namespace Plethora.Windows.Forms
{
    //TODO: Write a designer to set default styliser

    /// <summary>
    /// TextBox which restricts user input to values of a type derived from <see cref="IComparable"/>.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   When binding to the <see cref="Value"/> property of a <see cref="ComparableTextBox{T}"/>
    ///   the 'formattingEnabled' must be specified as true.
    ///   <example>
    ///    <code>comparableTextBox.DataBindings.Add("Value", myClass, "InnerValue", true);</code>
    ///   </example>
    ///  </para>
    /// </remarks>
    [DefaultBindingProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    public abstract class ComparableTextBox<T> : RestrictedTextBox, IValueProvider
    {
        #region Static Methods

        private static void ValidateGenericArgument()
        {
            ValidateGenericArgument(typeof(T));
        }

        private static void ValidateGenericArgument(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                ValidateGenericArgument(type.GetGenericArguments()[0]);
                return;
            }

            if (typeof(IComparable<T>).IsAssignableFrom(type))
                return;

            if (typeof(IComparable).IsAssignableFrom(type))
                return;

            //TODO: error message
            throw new InvalidCastException("Generic argument must implement IComparable, IComparable<T>, or inherit from Nullable<IComparable<T>>, or Nullable<IComparable>");
        }
        #endregion

        #region Fields

        private EditMode editMode;
        private bool isTextUnChanged = false;
        private bool isUpdateValueFromTextChange = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ComparableTextBox{T}"/> class.
        /// </summary>
        protected ComparableTextBox()
        {
            ValidateGenericArgument();

            this.minValue = MinOfT;
            this.maxValue = MaxOfT;
            this.value = default(T);
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

                T result;
                bool isValid = TryParseAndValidate(value, out result);
                if (!isValid)
                    throw new ArgumentException(ResourceProvider.ArgInvalid(@"Text"), "value");

                //Setting the value sets the text using the base class.
                this.Value = result;
            }
        }

        /// <summary>
        /// Validate the input to test for a valid <typeparamref name="T"/> value for this
        /// <see cref="ComparableTextBox{T}"/>.
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
                T parsedValue;
                if (!TryParse(textTrim, out parsedValue))
                    return false;

                return ValidateValue(parsedValue, false);
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
                        isTextUnChanged = true;
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

        protected override void OnTextChanged(EventArgs e)
        {
            isTextUnChanged = false;

            isUpdateValueFromTextChange = true;
            try
            { TrySetValueFromText(); }
            finally
            { isUpdateValueFromTextChange = false; }

            base.OnTextChanged(e);
        }
        #endregion

        #region Properties

        #region FormatParser

        /// <summary>
        /// Raised when <see cref="FormatParser"/> changes.
        /// </summary>
        /// <seealso cref="FormatParser"/>
        public event EventHandler FormatParserChanged;

        private IFormatParserPartial<T> formatParser = null;

        /// <summary>
        /// Gets and sets the formatter for this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Data)]
        [Description("The formatter for this text box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [RefreshProperties(RefreshProperties.All)]
        public virtual IFormatParserPartial<T> FormatParser
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
        protected virtual void OnFormatParserChanged()
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

        private T value;

        /// <summary>
        /// Gets and sets the value represented by the text of this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Data)]
        [DefaultValue(0)]
        [Description("The value represented by the text of this text box.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [RefreshProperties(RefreshProperties.All)]
        public virtual T Value
        {
            get { return this.value; }
            set
            {
                // value != this.value
                if (!EqualityComparer<T>.Default.Equals(this.value,value))
                {
                    if (!ValidateValue(value, false))
                        throw new ArgumentException(ResourceProvider.ArgInvalid(@"value"), "value");

                    this.value = value;
                    OnValueChanged();
                }

                if (!isUpdateValueFromTextChange)
                {
                    SetTextFromValue();
                }
            }
        }

        object IValueProvider.Value
        {
            get { return this.Value; }
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        protected virtual void OnValueChanged()
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

        private T minValue;

        /// <summary>
        /// Gets and sets the minimum value which this text box can contain.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [Description("The minimum value which this text box can contain.")]
        public virtual T MinValue
        {
            get { return minValue; }
            set
            {
                // value > maxValue
                if (Comparer<T>.Default.Compare(value, maxValue) > 0)
                    throw new ArgumentOutOfRangeException("value", value,
                        ResourceProvider.ArgMustBeLessThanEqualTo(@"value", "MaxValue"));

                // value == minValue
                if (EqualityComparer<T>.Default.Equals(value, minValue))
                    return;

                minValue = value;
                OnMinValueChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MinValueChanged"/> event.
        /// </summary>
        protected virtual void OnMinValueChanged()
        {
            if (this.EditMode == EditMode.Display)
            {
                // this.Value < this.MinValue
                if (Comparer<T>.Default.Compare(this.Value, this.MinValue) < 0)
                    this.Value = this.MinValue;
            }

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

        private T maxValue;

        /// <summary>
        /// Gets and sets the maximum value which this text box can contain.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [Description("The maximum value which this text box can contain.")]
        public virtual T MaxValue
        {
            get { return maxValue; }
            set
            {
                // value < minValue
                if (Comparer<T>.Default.Compare(value, minValue) < 0)
                    throw new ArgumentOutOfRangeException("value", value,
                        ResourceProvider.ArgMustBeGreaterThanEqualTo(@"value", "MinValue"));

                // value == maxValue
                if (EqualityComparer<T>.Default.Equals(value, maxValue))
                    return;

                maxValue = value;
                OnMaxValueChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxValueChanged"/> event.
        /// </summary>
        protected virtual void OnMaxValueChanged()
        {
            if (this.EditMode == EditMode.Display)
            {
                // this.Value > this.MaxValue
                if (Comparer<T>.Default.Compare(this.Value, this.MaxValue) > 0)
                    this.Value = this.MaxValue;
            }

            EventHandler handlers = this.MaxValueChanged;
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
            private set
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
                            isTextUnChanged = true;
                            DisableStylising();
                            SetTextFromValue();
                            break;

                        case EditMode.Display:
                            EnableStylising();
                            if (!isTextUnChanged)
                            {
                                SetTextFromValue();
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        #region Event Handlers

        private void formatParser_Changed(object sender, EventArgs e)
        {
            SetTextFromValue();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Validate partial values of <typeparamref name="T"/>, allowing for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The <typeparamref name="T"/> value to be validated.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid, partial value of <typeparamref name="T"/>; else 'false'.
        /// </returns>
        /// <example>
        /// Consider the case where <see cref="MinValue"/> is 25. The user wants to
        /// type the number 347. As the user types 3 this function must return true.
        /// </example>
        /// <remarks>
        /// A simple implementation may simply return 'true'.
        /// </remarks>
        protected abstract bool ValidateValuePartial(T validateValue);

        /// <summary>
        /// Validate the <typeparamref name="T"/> value for this <see cref="ComparableTextBox{T}"/>,
        /// allowing for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The <typeparamref name="T"/> value to be validated.
        /// </param>
        /// <param name="partial">
        /// 'true' if the method must allow for the construction of valid values (i.e. as the user types), 
        /// not just the valid value themselves; else false.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid <typeparamref name="T"/> value; else 'false'.
        /// </returns>
        protected bool ValidateValue(T validateValue, bool partial)
        {
            if (validateValue == null)
                return true;

            if (partial)
            {
                //NOTE: Must allow for the user to type in the value, so can't simply
                //      test that the value lies between minValue and maxValue.

                return ValidateValuePartial(validateValue);
            }
            else
            {
                // (minValue > validateValue) || (maxValue < validateValue)
                if ((Comparer<T>.Default.Compare(MinValue, validateValue) > 0) ||
                    (Comparer<T>.Default.Compare(MaxValue, validateValue) < 0))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Validate the <typeparamref name="T"/> value for this <see cref="ComparableTextBox{T}"/>,
        /// allowing for the user to type in the value.
        /// </summary>
        /// <param name="validateValue">
        /// The <typeparamref name="T"/> value to be validated.
        /// </param>
        /// <returns>
        /// 'true' if the value represents a valid <typeparamref name="T"/> value; else 'false'.
        /// </returns>
        protected bool ValidateValue(T validateValue)
        {
            bool partialValidationRequired = (this.EditMode != EditMode.Edit);

            return ValidateValue(validateValue, partialValidationRequired);
        }

        /// <summary>
        /// Converts the string representation of a number to its <typeparamref name="T"/> equivalent.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="text">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <typeparamref name="T"/> value equivalent to the
        /// number contained in 'text', if the conversion succeeded, or the default value of type
        /// <typeparamref name="T"/> if the conversion failed.
        /// </param>
        /// <returns>
        /// 'true' if 'text' was converted successfully; otherwise, 'false'.
        /// </returns>
        protected bool TryParse(string text, out T result)
        {
            bool isValid = FormatParser.TryParse(
                text,
                out result);

            if (!isValid)
                result = default(T);

            return isValid;
        }

        /// <summary>
        /// Converts the string representation of a number to its <typeparamref name="T"/> 
        /// equivalent, testing if the resulting parsed value is valid for this
        /// <see cref="ComparableTextBox{T}"/>. A return value indicates whether the
        /// operation succeeded.
        /// </summary>
        /// <param name="text">A string containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the <typeparamref name="T"/> value equivalent to the
        /// number contained in 'text', if the conversion and validation succeeded,
        /// or the default value of <typeparamref name="T"/> if the conversion failed.
        /// </param>
        /// <returns>
        /// 'true' if 'text' was converted and validated successfully; otherwise,
        /// 'false'.
        /// </returns>
        protected bool TryParseAndValidate(string text, out T result)
        {
            bool isValid = TryParse(text, out result);
            if (!isValid)
                return false;

            isValid = this.ValidateValue(result);
            if (!isValid)
            {
                result = default(T);
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

            string text;
            if ((this.Value == null) && (displayNullValueAsEmptyString))
                text = string.Empty;
            else
                text = FormatParser.Format(this.Value);

            ForceText(text);
        }

        /// <summary>
        /// Sets the text property of this text box, according to the value.
        /// </summary>
        protected bool TrySetValueFromText()
        {
            string text = this.Text;

            T result;
            bool isValid = TryParseAndValidate(text, out result);
            if (isValid)
                this.Value = result;

            return isValid;
        }
        #endregion

        #region Private Methods

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

        /// <summary>
        /// Gets the minimum value for the type <typeparam name="T"/>.
        /// </summary>
        protected abstract T MinOfT { get; }

        /// <summary>
        /// Gets the maximum value for the type <typeparam name="T"/>.
        /// </summary>
        protected abstract T MaxOfT { get; }
    }
}
