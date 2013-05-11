using System;
using System.ComponentModel;
using System.Globalization;
using Plethora.Windows.Forms;

namespace Plethora.Format
{
    /// <summary>
    /// Format and parser for nullable numeric data types.
    /// </summary>
    /// <typeparam name="T">
    ///  <para>
    ///   The type which is to be formatted and parsed.
    ///  </para>
    ///  <para>
    ///   In addition to implementing IFormattable and IConvertible, the type
    ///   <typeparamref name="T"/> must implement a public instance TryParse
    ///   method with the following signature:
    ///   <code>
    ///     bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out T t)
    ///   </code>
    ///   Construction will fail if this is not the case.
    ///  </para>
    /// </typeparam>
    /// <seealso cref="NumericFormatParser{T}"/>
    [CLSCompliant(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class NullableNumericFormatParser<T> : NullableFormatParserBase<T>, ICloneable
        where T : struct, IFormattable, IConvertible, IComparable, IComparable<T>
    {
        #region Static Members

        private static readonly NullableNumericFormatParser<T> defaultFormatParser = new NullableNumericFormatParser<T>();

        /// <summary>
        /// Gets the default <see cref="NumericFormatParser{T}"/>.
        /// </summary>
        public static NullableNumericFormatParser<T> Default
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
        public NullableNumericFormatParser()
            : this(NumericFormatParser<T>.DEFAULT_FORMAT, CultureInfo.CurrentCulture, NumericFormatParser<T>.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableNumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="format">
        /// The formatting string used to format values.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="format"/> is 'null'.
        /// </exception>
        public NullableNumericFormatParser(string format)
            : this(format, CultureInfo.CurrentCulture, NumericFormatParser<T>.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableNumericFormatParser{T}"/> class.
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
        public NullableNumericFormatParser(string format, IFormatProvider provider)
            : this(format, provider, NumericFormatParser<T>.DEFAULT_STYLES)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableNumericFormatParser{T}"/> class.
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
        ///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles' value.
        /// </exception>
        public NullableNumericFormatParser(string format, NumberStyles styles)
            : this(format, CultureInfo.CurrentCulture, styles)
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableNumericFormatParser{T}"/> class.
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
        ///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles' value.
        /// </exception>
        public NullableNumericFormatParser(string format, IFormatProvider provider, NumberStyles styles)
            : this(new NumericFormatParser<T>(format, provider, styles))
        {
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableNumericFormatParser{T}"/> class.
        /// </summary>
        /// <param name="formatParser">
        /// The <see cref="NumericFormatParser{T}"/> to be used interally in this instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <para>
        ///   Thrown if <paramref name="formatParser"/> is 'null'.
        ///  </para>
        /// </exception>
        public NullableNumericFormatParser(NumericFormatParser<T> formatParser)
            : base(formatParser)
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
        public NullableNumericFormatParser<T> Clone()
        {
            return new NullableNumericFormatParser<T>(this.InnerFormatParser.Clone());
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
        [DefaultValue(NumericFormatParser<T>.DEFAULT_FORMAT)]
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
        [DefaultValue(NumericFormatParser<T>.DEFAULT_STYLES)]
        [Description("The NumberStyle to be used by this NumericFormatParser.")]
        public virtual NumberStyles Styles
        {
            get { return this.InnerFormatParser.Styles; }
            set { this.InnerFormatParser.Styles = value; }
        }

        #endregion

        /// <summary>
        /// Gets and sets the suffix which will be applied to user input if no suffix
        /// is provided.
        /// </summary>
        /// <value>
        ///  <para>
        ///   'null' or empty string indicates no suffix should be applied.
        ///  </para>
        /// </value>
        /// <remarks>
        /// The suffix should be defined using <see cref="AddSuffix"/>.
        /// </remarks>
        /// <seealso cref="AddSuffix"/>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(NumericFormatParser<T>.DEFAULT_ASSUMED_SUFFIX)]
        [Description("The suffix which will be applied to user input if no suffix is provided.")]
        public virtual string AssumedSuffix
        {
            get { return this.InnerFormatParser.AssumedSuffix; }
            set { this.InnerFormatParser.AssumedSuffix = value; }
        }
        #endregion

        #region Public Methods

        #region Suffixes

        /// <summary>
        /// Adds a suffix to this format parser.
        /// </summary>
        /// <param name="suffix">
        /// The textual representation of the suffix.
        /// </param>
        /// <param name="multiplier">
        /// The multiplication value of the suffix.
        /// </param>
        /// <remarks>
        ///  <para>
        ///   A suffix can be appended to the string representation of value to
        ///   allow the value to be multiplied by some factor.
        ///  </para>
        /// <example>
        ///  <para>
        ///   A percentage may be displayed using the suffix "%" and the multiplier value of 0.01.
        ///  </para>
        ///  <para>
        ///   The following code demonstrates how the percent suffix may be added:
        ///   <code>
        ///    SuffixMultipliers.Add("%", 0.01);
        ///   </code>
        ///  </para>
        /// </example>
        /// </remarks>
        /// <seealso cref="AssumedSuffix"/>
        /// <seealso cref="RemoveSuffix"/>
        /// <seealso cref="ListSuffixes"/>
        /// <seealso cref="TryGetSuffixValue"/>
        public void AddSuffix(string suffix, double multiplier)
        {
            this.InnerFormatParser.AddSuffix(suffix, multiplier);
        }

        /// <summary>
        /// Removes a suffix from this format parser.
        /// </summary>
        /// <param name="suffix">
        /// The suffix to be removed.
        /// </param>
        /// <returns>
        /// true if the suffix is successfully found and removed; otherwise, false.
        /// This method returns false if suffix has not been previously defined.
        /// </returns>
        /// <seealso cref="AddSuffix"/>
        /// <seealso cref="ListSuffixes"/>
        /// <seealso cref="TryGetSuffixValue"/>
        public bool RemoveSuffix(string suffix)
        {
            return this.InnerFormatParser.RemoveSuffix(suffix);
        }

        /// <summary>
        /// Returns a list of the suffixes which have been defined in this
        /// format parser.
        /// </summary>
        /// <returns>
        /// An array of strings contained the textual representation of the
        /// defined suffixes.
        /// </returns>
        /// <seealso cref="AddSuffix"/>
        /// <seealso cref="RemoveSuffix"/>
        /// <seealso cref="TryGetSuffixValue"/>
        public string[] ListSuffixes()
        {
            return this.InnerFormatParser.ListSuffixes();
        }

        /// <summary>
        /// Gets the muliplication factor for a specified suffix.
        /// </summary>
        /// <param name="suffix">
        /// The suffix for which the multiplier is required.
        /// </param>
        /// <param name="multiplier">
        /// The multiplier of the specified suffix.
        /// </param>
        /// <returns>
        /// true if the suffix is defined in the format parser; else false.
        /// </returns>
        /// <seealso cref="AddSuffix"/>
        /// <seealso cref="RemoveSuffix"/>
        /// <seealso cref="ListSuffixes"/>
        public bool TryGetSuffixValue(string suffix, out double multiplier)
        {
            return this.InnerFormatParser.TryGetSuffixValue(suffix, out multiplier);
        }
        #endregion
        #endregion

        #region Protected Members

        /// <summary>
        /// Gets the inner <see cref="DateTimeFormatParser"/>.
        /// </summary>
        protected new NumericFormatParser<T> InnerFormatParser
        {
            get { return (NumericFormatParser<T>)base.InnerFormatParser; }
        }

        #endregion
    }
}
