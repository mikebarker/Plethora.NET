using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// A component which can be used to apply styles to a TextBox which
    /// implements <see cref="IValueProvider"/>
    /// </summary>
    [TypeConverter(typeof(NumericTextBoxStyliser.Converter))]
    public partial class NumericTextBoxStyliser : TextBoxStyliser
    {
        #region Static Members

        private static NumericTextBoxStyliser instance;

        /// <summary>
        /// Gets the default instance of <see cref="TextBoxStyliser"/>.
        /// </summary>
        public new static NumericTextBoxStyliser Default
        {
            get
            {
                //Combination of null check and interlock provides thread safety, with reduced
                // creation of new objects.
                if (instance == null)
                {
                    var positiveStyle = new TextBoxStyle();
                    var negativeStyle = new TextBoxStyle { ForeColor = Color.Red };
                    var zeroStyle = new TextBoxStyle { ForeColor = SystemColors.ControlLight };
                    var nullStyle = new TextBoxStyle { BackColor = SystemColors.ControlLight };

                    var defaultStyliser = new NumericTextBoxStyliser();
                    defaultStyliser.PositiveStyle = positiveStyle;
                    defaultStyliser.NegativeStyle = negativeStyle;
                    defaultStyliser.ZeroStyle = zeroStyle;
                    defaultStyliser.NullStyle = nullStyle;

                    Interlocked.CompareExchange(ref instance, defaultStyliser, null);
                }

                return instance;
            }
        }

        #endregion

        #region Properties

        #region PositiveStyle Property

        private static readonly object PositiveStyleChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="PositiveStyle"/> changes.
        /// </summary>
        /// <seealso cref="PositiveStyle"/>
        public event EventHandler PositiveStyleChanged
        {
            add { base.Events.AddHandler(PositiveStyleChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(PositiveStyleChanged_EventKey, value); }
        }

        private TextBoxStyle positiveStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when a text box's value is positive.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [Description("The style to be applied when a text box's value is positive.")]
        [DefaultValue(null)]
        public TextBoxStyle PositiveStyle
        {
            get { return positiveStyle; }
            set
            {
                if (value == positiveStyle)
                    return;

                if (positiveStyle != null)
                    UnwireStyleEvents(positiveStyle);

                positiveStyle = value;
                OnPositiveStyleChanged();

                if (positiveStyle != null)
                    WireStyleEvents(positiveStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="PositiveStyleChanged"/> event.
        /// </summary>
        protected void OnPositiveStyleChanged()
        {
            StyliseAllRegisteredTextBoxes();

            EventHandler handlers = (EventHandler)base.Events[PositiveStyleChanged_EventKey];
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region NegativeStyle Property

        private static readonly object NegativeStyleChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="NegativeStyle"/> changes.
        /// </summary>
        /// <seealso cref="NegativeStyle"/>
        public event EventHandler NegativeStyleChanged
        {
            add { base.Events.AddHandler(NegativeStyleChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(NegativeStyleChanged_EventKey, value); }
        }

        private TextBoxStyle negativeStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is negative.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [Description("The style to be applied when the text box's value is negative.")]
        [DefaultValue(null)]
        public TextBoxStyle NegativeStyle
        {
            get { return negativeStyle; }
            set
            {
                if (value == negativeStyle)
                    return;

                if (negativeStyle != null)
                    UnwireStyleEvents(negativeStyle);

                negativeStyle = value;
                OnNegativeStyleChanged();

                if (negativeStyle != null)
                    WireStyleEvents(negativeStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="NegativeStyleChanged"/> event.
        /// </summary>
        protected void OnNegativeStyleChanged()
        {
            StyliseAllRegisteredTextBoxes();

            EventHandler handlers = (EventHandler)base.Events[NegativeStyleChanged_EventKey];
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region ZeroStyle Property

        private static readonly object ZeroStyleChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="ZeroStyle"/> changes.
        /// </summary>
        /// <seealso cref="ZeroStyle"/>
        public event EventHandler ZeroStyleChanged
        {
            add { base.Events.AddHandler(ZeroStyleChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ZeroStyleChanged_EventKey, value); }
        }

        private TextBoxStyle zeroStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is zero.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [Description("The style to be applied when the text box's value is zero.")]
        [DefaultValue(null)]
        public TextBoxStyle ZeroStyle
        {
            get { return zeroStyle; }
            set
            {
                if (value == zeroStyle)
                    return;

                if (zeroStyle != null)
                    UnwireStyleEvents(zeroStyle);

                zeroStyle = value;
                OnZeroStyleChanged();

                if (zeroStyle != null)
                    WireStyleEvents(zeroStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="ZeroStyleChanged"/> event.
        /// </summary>
        protected void OnZeroStyleChanged()
        {
            StyliseAllRegisteredTextBoxes();

            EventHandler handlers = (EventHandler)base.Events[ZeroStyleChanged_EventKey];
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion

        #region NullStyle Property

        private static readonly object NullStyleChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="NullStyle"/> changes.
        /// </summary>
        /// <seealso cref="NullStyle"/>
        public event EventHandler NullStyleChanged
        {
            add { base.Events.AddHandler(NullStyleChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(NullStyleChanged_EventKey, value); }
        }

        private TextBoxStyle nullStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is null.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [Description("The style to be applied when the text box's value is null.")]
        [DefaultValue(null)]
        public TextBoxStyle NullStyle
        {
            get { return nullStyle; }
            set
            {
                if (value == nullStyle)
                    return;

                if (nullStyle != null)
                    UnwireStyleEvents(nullStyle);

                nullStyle = value;
                OnNullStyleChanged();

                if (nullStyle != null)
                    WireStyleEvents(nullStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="NullStyleChanged"/> event.
        /// </summary>
        protected void OnNullStyleChanged()
        {
            StyliseAllRegisteredTextBoxes();

            EventHandler handlers = (EventHandler)base.Events[NullStyleChanged_EventKey];
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
        /// The <see cref="TextBox"/> to be registered with this instance.
        /// </param>
        public override void RegisterTextBox(TextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));

            if (!(textBox is IValueProvider))
                throw new InvalidCastException(ResourceProvider.ArgMustBeOfType(nameof(textBox), typeof(IValueProvider)));


            base.RegisterTextBox(textBox);
        }

        /// <summary>
        /// Deregister a TextBox from this <see cref="TextBoxStyliser"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="TextBox"/> to be deregistered with this instance.
        /// </param>
        public override void DeregisterTextBox(TextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));

            if (!(textBox is IValueProvider))
                throw new InvalidCastException(ResourceProvider.ArgMustBeOfType(nameof(textBox), typeof (IValueProvider)));


            base.DeregisterTextBox(textBox);
        }
        #endregion

        protected override TextBoxStyle GetRequiredStyle(TextBox textBox)
        {
            var style = GetRequiredStyle((IValueProvider)textBox);
            if (style != null)
                return style;

            return base.GetRequiredStyle(textBox);
        }

        protected virtual TextBoxStyle GetRequiredStyle(IValueProvider numericTextBox)
        {
            object value = numericTextBox.Value;

            if (value == null)
                return nullStyle;

            int compareResult;
            if (NumericHelper.TypeSafeComparisonToZero(value, out compareResult))
            {
                if (compareResult == 0)
                {
                    return ZeroStyle;
                }
                else if (compareResult > 0)
                {
                    return PositiveStyle;
                }
                else if (compareResult < 0)
                {
                    return NegativeStyle;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// A <see cref="NumericTextBoxStyliser"/> presented as a component.
    /// </summary>
    [Designer(typeof(NumericTextBoxStyliserComponent.Designer))]
    public partial class NumericTextBoxStyliserComponent : NumericTextBoxStyliser, IComponent
    {
        #region Fields

        private readonly Component component = new Component();
        #endregion

        #region Constructors

        public NumericTextBoxStyliserComponent()
        {
        }

        public NumericTextBoxStyliserComponent(IContainer container)
            : this()
        {
            container.Add(this);
        }
        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            component.Dispose();
        }
        #endregion

        #region Implementation of IComponent

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> associated with the <see cref="IComponent"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="ISite"/> object associated with the component;
        /// or null, if the component does not have a site.
        /// </returns>
        [Browsable(false)]
        public ISite Site
        {
            get { return component.Site; }
            set { component.Site = value; }
        }

        [Browsable(false)]
        public event EventHandler Disposed
        {
            add { component.Disposed += value; }
            remove { component.Disposed -= value; }
        }

        #endregion
    }
}
