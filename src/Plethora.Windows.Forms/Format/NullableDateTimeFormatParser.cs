using System;
using System.ComponentModel;
using System.Globalization;
using Plethora.Windows.Forms;

namespace Plethora.Format
{
    /// <summary>
    /// Format and parser for <see cref="DateTime"/>.
    /// </summary>
    [CLSCompliant(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class NullableDateTimeFormatParser : NullableFormatParserBase<DateTime>, ICloneable
    {
        #region Static Members

        private static readonly NullableDateTimeFormatParser defaultFormatParser = new NullableDateTimeFormatParser();

        /// <summary>
        /// Gets the default <see cref="NullableDateTimeFormatParser"/>.
        /// </summary>
        public static NullableDateTimeFormatParser Default
        {
            get { return defaultFormatParser; }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
        /// </summary>
        /// <remarks>
        /// The default formatting string is utilised to format values.
        /// </remarks>
        public NullableDateTimeFormatParser()
            : this(DateTimeFormatParser.DEFAULT_FORMAT, CultureInfo.CurrentCulture, DateTimeFormatParser.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The formatting string used to format values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="format"/> is 'null'.
        /// </exception>
        public NullableDateTimeFormatParser(string format)
            : this(format, CultureInfo.CurrentCulture, DateTimeFormatParser.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The formatting string used to format values.
        /// </param>
        /// <param name="provider">
        /// The IFormatProvider to be used in the Parse and TryParse methods.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <para>
        ///   Thrown if <paramref name="format"/> is 'null'.
        ///  </para>
        ///  <para>
        ///   Thrown if <paramref name="provider"/> is 'null'.
        ///  </para>
        /// </exception>
        public NullableDateTimeFormatParser(string format, IFormatProvider provider)
            : this(format, provider, DateTimeFormatParser.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The formatting string used to format values.
        /// </param>
        /// <param name="styles">
        /// The NumberStyles to be used in the Parse and TryParse methods.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Thrown if <paramref name="format"/> is 'null'.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown if <paramref name="styles"/> is not a valid 'DateTimeStyles' value.
        /// </exception>
        public NullableDateTimeFormatParser(string format, DateTimeStyles styles)
            : this(format, CultureInfo.CurrentCulture, styles)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The formatting string used to format values.
        /// </param>
        /// <param name="provider">
        /// The IFormatProvider to be used in the Parse and TryParse methods.
        /// </param>
        /// <param name="styles">
        /// The NumberStyles to be used in the Parse and TryParse methods.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <para>
        ///   Thrown if <paramref name="format"/> is 'null'.
        ///  </para>
        ///  <para>
        ///   Thrown if <paramref name="provider"/> is 'null'.
        ///  </para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown if <paramref name="styles"/> is not a valid 'DateTimeStyles' value.
        /// </exception>
        public NullableDateTimeFormatParser(string format, IFormatProvider provider, DateTimeStyles styles)
            : this(new DateTimeFormatParser(format, provider, styles))
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableDateTimeFormatParser"/> class.
        /// </summary>
        /// <param name="formatParser">
        /// The <see cref="DateTimeFormatParser"/> to be used interally in this instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <para>
        ///   Thrown if <paramref name="formatParser"/> is 'null'.
        ///  </para>
        /// </exception>
        public NullableDateTimeFormatParser(DateTimeFormatParser formatParser)
            : base(formatParser)
        {
        }

        protected NullableDateTimeFormatParser(NullableDateTimeFormatParser nullableDateTimeFormatParser)
            : base(nullableDateTimeFormatParser.InnerFormatParser.Clone())
        {
        }
        #endregion

        #region ICloneable Implementation

        /// <summary>
        /// Returns a deep clone of this instance.
        /// </summary>
        /// <returns>
        /// A clone of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns a deep clone of this instance.
        /// </summary>
        /// <returns>
        /// A clone of this instance.
        /// </returns>
        public NullableDateTimeFormatParser Clone()
        {
            return new NullableDateTimeFormatParser(this);
        }
        #endregion

        #region Properties

        #region FormatString Property

        /// <summary>
        /// Raised when <see cref="FormatString"/> changes.
        /// </summary>
        /// <seealso cref="FormatString"/>
        public event EventHandler FormatStringChanged
        {
            add { this.InnerFormatParser.FormatStringChanged += value; }
            remove { this.InnerFormatParser.FormatStringChanged -= value; }
        }

        /// <summary>
        /// Gets and sets the format string for this
        /// <see cref="NumericFormatParser{T}"/>.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(DateTimeFormatParser.DEFAULT_FORMAT)]
        [Description("The format string for this formatter.")]
        public string FormatString
        {
            get { return this.InnerFormatParser.FormatString; }
            set { this.InnerFormatParser.FormatString = value; }
        }
        #endregion

        #region Provider Property

        /// <summary>
        /// Raised when <see cref="Provider"/> changes.
        /// </summary>
        /// <seealso cref="Provider"/>
        public event EventHandler ProviderChanged
        {
            add { this.InnerFormatParser.ProviderChanged += value; }
            remove { this.InnerFormatParser.ProviderChanged -= value; }
        }

        /// <summary>
        /// Gets and sets the provider for this <see cref="NumericFormatParser{T}"/>
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider Provider
        {
            get { return this.InnerFormatParser.Provider; }
            set { this.InnerFormatParser.Provider = value; }
        }

        #endregion

        #region Styles Property

        /// <summary>
        /// Raised when <see cref="Styles"/> changes.
        /// </summary>
        /// <seealso cref="Styles"/>
        public event EventHandler StylesChanged
        {
            add { this.InnerFormatParser.StylesChanged += value; }
            remove { this.InnerFormatParser.StylesChanged -= value; }
        }

        /// <summary>
        /// Gets and sets the NumberStyle to be used by this <see cref="NumericFormatParser{T}"/>
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(DateTimeFormatParser.DEFAULT_STYLES)]
        [Description("The NumberStyle to be used by this NumericFormatParser.")]
        public virtual DateTimeStyles Styles
        {
            get { return this.InnerFormatParser.Styles; }
            set { this.InnerFormatParser.Styles = value; }
        }

        #endregion

        #endregion

        #region Protected Members

        /// <summary>
        /// Gets the inner <see cref="DateTimeFormatParser"/>.
        /// </summary>
        protected new DateTimeFormatParser InnerFormatParser
        {
            get { return (DateTimeFormatParser)base.InnerFormatParser; }
        }

        #endregion
    }
}
