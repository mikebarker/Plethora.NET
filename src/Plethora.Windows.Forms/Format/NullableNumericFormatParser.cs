using System;
using System.ComponentModel;
using System.Globalization;

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
    public class NullableNumericFormatParser<T> : Component, IFormatParserPartial<Nullable<T>>, ICloneable
		where T : struct, IFormattable, IConvertible, IComparable
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

		#region Fields

		private readonly NumericFormatParser<T> innerNumericFormatParser;
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
		///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles'
		///   value.
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
		///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles'
		///   value.
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
		private NullableNumericFormatParser(NumericFormatParser<T> formatParser)
		{
			//Validation
			if (formatParser == null)
				throw new ArgumentNullException("formatParser");


			this.innerNumericFormatParser = formatParser;

			WireEvents();
		}

        private NullableNumericFormatParser(NullableNumericFormatParser<T> nullableNumericFormatParser)
            : this(nullableNumericFormatParser.innerNumericFormatParser.Clone())
        {
            this.NullString = nullableNumericFormatParser.NullString;
        }
		#endregion

		#region Factory Methods

		/// <summary>
		/// Creates a <see cref="NullableNumericFormatParser{T}"/> wrapper for
		/// a specific <see cref="NumericFormatParser{T}"/>.
		/// </summary>
		/// <param name="formatParser">
		/// The <see cref="NumericFormatParser{T}"/> to be wrapped. 
		/// </param>
		/// <returns>
		/// The <see cref="NullableNumericFormatParser{T}"/> wrapper around the
		/// <see cref="NumericFormatParser{T}"/>.
		/// </returns>
		public static NullableNumericFormatParser<T> NullableWrapper(NumericFormatParser<T> formatParser)
		{
			return new NullableNumericFormatParser<T>(formatParser);
		}
		#endregion

        #region IFormatParser<T> Members

        /// <summary>
		/// Raised when the properties of the <see cref="IFormatParser{T}"/> are
		/// changed.
		/// </summary>
		public event EventHandler Changed;

        /// <summary>
		/// Converts the <typeparamref name="T"/>? value to its equivalent string
		/// representation.
		/// </summary>
		/// <param name="value">
		/// The value to be formatted as a string.
		/// </param>
		/// <param name="state">
		/// 'true' if null values of <paramref name="value"/> should be formatted
		/// as an empty string; 'false' if null values of <paramref name="value"/>
		/// should be formatted according to the property of
		/// <see cref="NullString"/>.
		/// </param>
		/// <returns>
		/// The string equivalent of <paramref name="value"/>.
		/// </returns>
		public virtual string Format(Nullable<T> value, object state)
		{
			if (value.HasValue)
			{
				return innerNumericFormatParser.Format(value.Value, state);
			}
			else
			{
				bool nullAsEmpty = true;
				if (state is bool)
					nullAsEmpty = (bool)state;

				if (nullAsEmpty)
					return string.Empty;
				else
					return this.NullString;
			}
		}

        /// <summary>
        /// Converts the string representation of a number to its equivalent
        /// numeric value. A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="result">
        ///  <para>
        ///   When this method returns, contains the <typeparamref name="T"/>? value
        ///   equivalent to the string contained in 's', if the conversion succeeded.
        ///  </para>
        ///  <para>
        ///   Each implementation defines its own return value for 'result' in the
        ///   case where the conversion was not successful. Usually this will be 0
        ///   for numeric values, and 'null' for reference types.
        ///  </para>
        /// </param>
        /// <returns>
        /// 'true' if 's' was converted successfully; otherwise, 'false'.
        /// </returns>
        public virtual bool TryParse(string s, out Nullable<T> result)
        {
            if ((s.Length == 0) || (string.Equals(s, this.NullString)))
            {
                result = null;
                return true;
            }
            else
            {
                T innerResult;
                bool canParse = innerNumericFormatParser.TryParse(s, out innerResult);

                result = innerResult;
                return canParse;
            }
        }

        /// <summary>
        /// Determines whether a string representation of a number can be converted
        /// to its equivalent value of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="partial">
        ///  <para>
        ///   'true' if the string is only a partial representation of
        ///   type <typeparamref name="T"/>; else, 'false'.
        ///  </para>
        ///  <para>
        ///   This parameter maybe 'true' to represent a text value still being
        ///   entered by a user, indicating that <paramref name="s"/> is not yet
        ///   the final representation to be parsed.
        ///  </para>
        /// </param>
        /// <returns>
        /// 'true' if <paramref name="s"/> cen be converted successfully;
        /// otherwise, 'false'.
        /// </returns>
        public virtual bool CanParse(string s, bool partial)
        {
            if ((s.Length == 0) || (string.Equals(s, this.NullString)))
            {
                return true;
            }
            else
            {
                return innerNumericFormatParser.CanParse(s, partial);
            }
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
            return new NullableNumericFormatParser<T>(this);
        }
        #endregion

		#region Properties

        #region NullString Property

        /// <summary>
        /// Raised when <see cref="NullString"/> changes.
        /// </summary>
        /// <seealso cref="NullString"/>
        public event EventHandler NullStringChanged;

        /// <summary>
        /// The default value of <see cref="NullString"/>.
        /// </summary>
        protected const string DEFAULT_NULL_STRING = "";

        private string nullString = DEFAULT_NULL_STRING;

        /// <summary>
        /// Gets and sets the string to be displayed when formatting a null value.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(DEFAULT_NULL_STRING)]
        [Description("The string to be displayed when formatting a null value.")]
        public string NullString
        {
            get { return nullString; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value == nullString)
                    return;

                nullString = value;
                OnNullStringChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="NullStringChanged"/> event.
        /// </summary>
        protected void OnNullStringChanged()
        {
            EventHandler handlers = this.NullStringChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged();
        }
        #endregion

		#region FormatString Property

		/// <summary>
		/// Raised when <see cref="FormatString"/> changes.
		/// </summary>
		/// <seealso cref="FormatString"/>
		public event EventHandler FormatStringChanged;

		/// <summary>
		/// Gets and sets the format string for this
		/// <see cref="NumericFormatParser{T}"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(NumericFormatParser<T>.DEFAULT_FORMAT)]
		[Description("The format string for this formatter.")]
		public virtual string FormatString
		{
            get { return innerNumericFormatParser.FormatString; }
            set
            {
                if (value == FormatString)
                    return;

                innerNumericFormatParser.FormatString = value;
            }
		}

		/// <summary>
		/// Raises the <see cref="FormatStringChanged"/> event.
		/// </summary>
		protected void OnFormatStringChanged()
		{
			EventHandler handlers = this.FormatStringChanged;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
		}

		#endregion

		#region Provider Property

		/// <summary>
		/// Raised when <see cref="Provider"/> changes.
		/// </summary>
		/// <seealso cref="Provider"/>
		public event EventHandler ProviderChanged;

		/// <summary>
		/// Gets and sets the provider for this <see cref="NumericFormatParser{T}"/>
		/// </summary>
		[Browsable(false)]
		public virtual IFormatProvider Provider
		{
			get { return innerNumericFormatParser.Provider; }
			set { innerNumericFormatParser.Provider = value; }
		}

		/// <summary>
		/// Raises the <see cref="ProviderChanged"/> event.
		/// </summary>
		protected void OnProviderChanged()
		{
			EventHandler handlers = this.ProviderChanged;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
		}

		#endregion

		#region Styles Property

		/// <summary>
		/// Raised when <see cref="Styles"/> changes.
		/// </summary>
		/// <seealso cref="Styles"/>
		public event EventHandler StylesChanged;

		/// <summary>
		/// Gets and sets the NumberStyle to be used by this <see cref="NumericFormatParser{T}"/>
		/// </summary>
		[Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(NumericFormatParser<T>.DEFAULT_STYLES)]
		[Description("The NumberStyle to be used by this NumericFormatParser.")]
		public virtual NumberStyles Styles
		{
			get { return innerNumericFormatParser.Styles; }
			set { innerNumericFormatParser.Styles = value; }
		}

		/// <summary>
		/// Raises the <see cref="FormatStringChanged"/> event.
		/// </summary>
		protected void OnStylesChanged()
		{
			EventHandler handlers = this.StylesChanged;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
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
		[Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(NumericFormatParser<T>.DEFAULT_ASSUMED_SUFFIX)]
		[Description("The suffix which will be applied to user input if no suffix is provided.")]
        public virtual string AssumedSuffix
		{
			get { return innerNumericFormatParser.AssumedSuffix; }
			set { innerNumericFormatParser.AssumedSuffix = value; }
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
            innerNumericFormatParser.AddSuffix(suffix, multiplier);
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
            return innerNumericFormatParser.RemoveSuffix(suffix);
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
            return innerNumericFormatParser.ListSuffixes();
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
            return innerNumericFormatParser.TryGetSuffixValue(suffix, out multiplier);
        }
        #endregion
        #endregion

        #region Protected Methods

		/// <summary>
		/// Raises the <see cref="Changed"/> event.
		/// </summary>
		protected void OnChanged()
		{
			EventHandler handlers = this.Changed;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
		}

		#endregion

		#region Private Methods

		private void WireEvents()
		{
			this.innerNumericFormatParser.Changed += innerNumericFormatParser_Changed;
			this.innerNumericFormatParser.FormatStringChanged += innerNumericFormatParser_FormatStringChanged;
			this.innerNumericFormatParser.ProviderChanged += innerNumericFormatParser_ProviderChanged;
			this.innerNumericFormatParser.StylesChanged += innerNumericFormatParser_StylesChanged;
		}
		#endregion

		#region Event Handlers

		void innerNumericFormatParser_Changed(object sender, EventArgs e)
		{
			OnChanged();
		}

		void innerNumericFormatParser_FormatStringChanged(object sender, EventArgs e)
		{
			OnFormatStringChanged();
		}

		void innerNumericFormatParser_ProviderChanged(object sender, EventArgs e)
		{
			OnProviderChanged();
		}

		void innerNumericFormatParser_StylesChanged(object sender, EventArgs e)
		{
			OnStylesChanged();
		}
		#endregion
    }
}
