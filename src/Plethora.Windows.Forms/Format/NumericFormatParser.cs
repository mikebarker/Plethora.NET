using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Plethora.Format
{
	/// <summary>
	/// Format and parser for numeric data types.
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
	/// <seealso cref="NullableNumericFormatParser{T}"/>
	[CLSCompliant(false)]
	public class NumericFormatParser<T> : Component, IFormatParserPartial<T>, ICloneable
		where T : IFormattable, IConvertible, IComparable
	{
		#region Static Members

		private readonly static NumericFormatParser<T> defaultFormatParser = new NumericFormatParser<T>();

		/// <summary>
		/// Gets the default <see cref="NumericFormatParser{T}"/>.
		/// </summary>
		public static NumericFormatParser<T> Default
		{
			get { return defaultFormatParser; }
		}
		#endregion

		#region Constants

		/// <summary>
		/// The default value to be used for <see cref="AssumedSuffix"/>
		/// </summary>
		protected internal const string DEFAULT_ASSUMED_SUFFIX = null;

		internal const int POSITIVE_FORMAT_SUB_STRING_INDEX = 0;
		internal const int NEGATIVE_FORMAT_SUB_STRING_INDEX = 1;
		internal const int     ZERO_FORMAT_SUB_STRING_INDEX = 2;
		#endregion

		#region Delegates

		/// <summary>
		/// The TryParse Delegate for type <typeparamref name="T"/>.
		/// </summary>
		/// <param name="s">A string containing a number to convert.</param>
		/// <param name="styles">
		/// A bitwise combination of NumberStyles values that indicates the permitted
		/// format of <paramref name="s"/>.
		/// </param>
		/// <param name="provider">
		/// An <see cref="IFormatProvider"/> that supplies culture-specific formatting
		/// information about <paramref name="s"/>.
		/// </param>
		/// <param name="t">
		/// When this method returns, contains the numeric value or symbol contained
		/// in s, if the conversion succeeded, or zero if the conversion failed.
		/// </param>
		/// <returns>
		/// 'true' if s was converted successfully; otherwise, 'false'.
		/// </returns>
		private delegate bool TryParseDelegate(string s, NumberStyles styles, IFormatProvider provider, out T t);
		#endregion

		#region Fields

        private static readonly TryParseDelegate tryParse;

		/// <summary>
		/// Array of suffixes which should not be explicitly considered when
		/// formatting values.
		/// </summary>
		/// <remarks>
		/// This array allows for suffixes, such as percentages [%], which are
		/// specifically handled by the .ToString() method.
		/// </remarks>
		private readonly static string[] suffixFormatExclusions = new string[] { "%" };
		private readonly Dictionary<string, double> suffixMultipliers = new Dictionary<string, double>();
		private string assumedSuffix = DEFAULT_ASSUMED_SUFFIX;

		private readonly bool initialised = false;

        private Regex regexParseNumeric = null;

		private string formatSubStringPositive;
		private string formatSubStringNegative;
		private string formatSubStringZero;
		#endregion

        #region Constructors

        /// <summary>
        /// Called the first time the class is accessed.
        /// </summary>
        static NumericFormatParser()
        {
            tryParse = GetTryParseDelegate();
        }

		/// <summary>
		/// Initialise a new instance of the <see cref="NumericFormatParser{T}"/> class.
		/// </summary>
		/// <remarks>
		/// The default formatting string is utilised to format values.
		/// </remarks>
		public NumericFormatParser()
			: this(DEFAULT_FORMAT, CultureInfo.CurrentCulture, DEFAULT_STYLES)
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
		public NumericFormatParser(string format)
			: this(format, CultureInfo.CurrentCulture, DEFAULT_STYLES)
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
		public NumericFormatParser(string format, IFormatProvider provider)
			: this(format, provider, DEFAULT_STYLES)
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
		///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles'
		///   value.
		/// </exception>
		public NumericFormatParser(string format, NumberStyles styles)
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
		///   Thrown if <paramref name="styles"/> is not a valid 'NumberStyles'
		///   value.
		/// </exception>
		public NumericFormatParser(string format, IFormatProvider provider, NumberStyles styles)
		{
			//Validation
			if (format == null)
				throw new ArgumentNullException("format");

			if (provider == null)
				throw new ArgumentNullException("provider");

			if (Enum.IsDefined(typeof(NumberStyles), styles))
				throw new ArgumentOutOfRangeException("styles", styles,
					string.Format("The value '{1}' is not valid for the enum '{0}'.",
					typeof(NumberStyles),
					styles));


            this.FormatString = format;
			this.provider = provider;

			this.styles = MaskStyles(styles);

			WireEvents();

            this.initialised = true;
		}

        private NumericFormatParser(NumericFormatParser<T> numericFormatParser)
        {
            //Validation
            if (numericFormatParser == null)
                throw new ArgumentNullException("numericFormatParser");


            this.suffixMultipliers = new Dictionary<string,double>(numericFormatParser.suffixMultipliers);
            this.AssumedSuffix = numericFormatParser.AssumedSuffix;

            this.regexParseNumeric = numericFormatParser.regexParseNumeric;

            this.FormatString = numericFormatParser.FormatString;
            this.Provider = numericFormatParser.Provider;
            this.Styles = numericFormatParser.Styles;

            WireEvents();

            this.initialised = true;
        }
		#endregion

		#region IFormatParserPartial<T> Members

		/// <summary>
		/// Raised when the properties of the <see cref="IFormatParser{T}"/> are changed.
		/// </summary>
		public event EventHandler Changed;

		/// <summary>
		/// Converts the <typeparamref name="T"/> value to its equivalent string
		/// representation.
		/// </summary>
		/// <param name="value">
		/// The value to be formatted as a string.
		/// </param>
		/// <param name="state">
		/// Ignorred.
		/// </param>
		/// <returns>
		/// The string equivalent of <paramref name="value"/>.
		/// </returns>
		public virtual string Format(T value, object state)
		{
			double multiplier;
			bool suffixFound;
			string formatSubString = this.FormatSubStringForValue(value);
			ConsiderSuffixes(formatSubString, true, out multiplier, out suffixFound);

			IFormattable formattableValue = value;
			if (multiplier != 1.0)
			{
				//Since multiplier is of type double, the resulting division will
				// cast to double. Done here to prevent the necessity for inheritance.
				double dblValue = value.ToDouble(null);
				dblValue = dblValue/multiplier;
				formattableValue = (IFormattable) Convert.ChangeType(dblValue, typeof (T));
			}

			return formattableValue.ToString(this.FormatString, this.Provider);
		}

		/// <summary>
		/// Converts the string representation of a number to its equivalent
		/// Double value. A return value indicates whether the operation succeeded.
		/// </summary>
		/// <param name="s">A string containing the value to convert.</param>
		/// <param name="result">
		///  <para>
		///   When this method returns, contains the Double value equivalent to the
		///   string contained in 's', if the conversion succeeded.
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
		public virtual bool TryParse(string s, out T result)
		{
			double multiplier;
			bool suffixFound;
			s = ConsiderSuffixes(s, false, out multiplier, out suffixFound);

			if (!suffixFound)
                multiplier = AssumedMultiplier;

			bool parseSuccessful = tryParse(s, this.Styles, this.Provider, out result);

			if (parseSuccessful)
			{
				if (multiplier != 1.0)
				{
					double dblResult = result.ToDouble(null);
					dblResult = dblResult * multiplier;
					result = (T) Convert.ChangeType(dblResult, typeof (T));
				}
			}

			return parseSuccessful;
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
            if (partial)
            {
                return CanParsePartial(s);
            }
            else
            {
                T result;
                return TryParse(s, out result);
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
        public NumericFormatParser<T> Clone()
        {
            return new NumericFormatParser<T>(this);
        }
        #endregion

		#region Properties

		#region FormatString Property

		/// <summary>
		/// Raised when <see cref="FormatString"/> changes.
		/// </summary>
		/// <seealso cref="FormatString"/>
		public event EventHandler FormatStringChanged;

		/// <summary>
		/// The default value of <see cref="FormatString"/>.
		/// </summary>
		protected internal const string DEFAULT_FORMAT = "";

		/// <summary>
		/// Gets and sets the format string for this
		/// <see cref="NumericFormatParser{T}"/>.
		/// </summary>
		[Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(DEFAULT_FORMAT)]
		[Description("The format string for this formatter.")]
		public string FormatString
		{
			get { return JoinFormatString(); }
			set
			{
				if (value == FormatString)
					return;

				SplitFormatString(value);
				OnFormatStringChanged();
			}
		}

		/// <summary>
		/// Raises the <see cref="FormatStringChanged"/> event.
		/// </summary>
		protected void OnFormatStringChanged()
		{
			if (!initialised)
				return;

			EventHandler handlers = this.FormatStringChanged;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
		}

		#region FormatSubStrings

		/// <summary>
		/// The positive format sub string.
		/// </summary>
		protected string FormatSubStringPositive
		{
			get { return formatSubStringPositive; }
			set { formatSubStringPositive = value; }
		}

		/// <summary>
		/// The negative format sub string.
		/// </summary>
		protected string FormatSubStringNegative
		{
			get { return formatSubStringNegative; }
			set { formatSubStringNegative = value; }
		}

		/// <summary>
		/// The zero format sub string.
		/// </summary>
		protected string FormatSubStringZero
		{
			get { return formatSubStringZero; }
			set { formatSubStringZero = value; }
		}
		#endregion
		#endregion

		#region Provider Property

		/// <summary>
		/// Raised when <see cref="Provider"/> changes.
		/// </summary>
		/// <seealso cref="Provider"/>
		public event EventHandler ProviderChanged;

		private IFormatProvider provider = null;

		/// <summary>
		/// Gets and sets the provider for this <see cref="NumericFormatParser{T}"/>
		/// </summary>
		[Browsable(false)]
		public virtual IFormatProvider Provider
		{
			get { return provider; }
			set
			{
				if (value == provider)
					return;

				provider = value;
				OnProviderChanged();
			}
		}

		/// <summary>
		/// Raises the <see cref="ProviderChanged"/> event.
		/// </summary>
		protected void OnProviderChanged()
		{
			if (!initialised)
				return;

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
		/// The default value of <see cref="Styles"/>.
		/// </summary>
		protected internal const NumberStyles DEFAULT_STYLES =
			NumberStyles.AllowCurrencySymbol | NumberStyles.AllowParentheses |
			NumberStyles.AllowThousands | NumberStyles.Integer | NumberStyles.Float;

		private NumberStyles styles = DEFAULT_STYLES;

		/// <summary>
		/// Gets and sets the NumberStyle to be used by this <see cref="NumericFormatParser{T}"/>
		/// </summary>
		[Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(DEFAULT_STYLES)]
		[Description("The NumberStyle to be used by this NumericFormatParser.")]
		public virtual NumberStyles Styles
		{
			get { return styles; }
			set
			{
				if (value == styles)
					return;

                regexParseNumeric = null;
				styles = MaskStyles(value);
				OnStylesChanged();
			}
		}

		/// <summary>
		/// Raises the <see cref="StylesChanged"/> event.
		/// </summary>
		protected void OnStylesChanged()
		{
			if (!initialised)
				return;

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
        /// <seealso cref="AddSuffix"/>
        [Browsable(true)]
		[Category("Behaviour")]
		[DefaultValue(DEFAULT_ASSUMED_SUFFIX)]
		[Description("The suffix which will be applied to user input if no suffix is provided.")]
		public virtual string AssumedSuffix
		{
			get { return assumedSuffix; }
			set
			{
				if ((value != null) && (value.Length == 0))
					value = null;

				if (value == assumedSuffix)
					return;

				assumedSuffix = value;
			}
		}

		/// <summary>
		/// Gets the default multiplier corresponding to <see cref="AssumedSuffix"/>.
		/// </summary>
        protected double AssumedMultiplier
		{
			get
			{
				if (AssumedSuffix == null)
					return 1.0;

                double value;
                if (!suffixMultipliers.TryGetValue(AssumedSuffix, out value))
                    return 1.0;
                else
                    return value;
			}
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
            if (suffix == null)
                throw new ArgumentNullException("suffix");

            if (suffix.Length == 0)
                throw new ArgumentException("suffix may not be an empty string.");


            this.regexParseNumeric = null;
            this.suffixMultipliers.Add(suffix, multiplier);
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
            if (suffix == null)
                throw new ArgumentNullException("suffix");

            if (suffix.Length == 0)
                throw new ArgumentException("suffix may not be an empty string.");


            this.regexParseNumeric = null;
            return this.suffixMultipliers.Remove(suffix);
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
            Dictionary<string, double>.KeyCollection keys = suffixMultipliers.Keys;

            string[] keyArray = new string[keys.Count];
            keys.CopyTo(keyArray, 0);

            return keyArray;
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
            if (suffix == null)
                throw new ArgumentNullException("suffix");

            if (suffix.Length == 0)
                throw new ArgumentException("suffix may not be an empty string.");


            return this.suffixMultipliers.TryGetValue(suffix, out multiplier);
        }
        #endregion
        #endregion

        #region Protected Methods

        /// <summary>
		/// Raises the <see cref="Changed"/>.
		/// </summary>
		protected virtual void OnChanged()
		{
			if (!initialised)
				return;

			EventHandler handlers = this.Changed;
			if (handlers != null)
				handlers(this, EventArgs.Empty);
		}

		/// <summary>
		/// Ensure property changes are propagated.
		/// </summary>
		protected void WireEvents()
		{
			this.FormatStringChanged += new EventHandler(FormatParser_FormatStringChanged);
			this.StylesChanged += new EventHandler(FormatParser_StylesChanged);
			this.ProviderChanged += new EventHandler(FormatParser_ProviderChanged);
		}

		/// <summary>
		/// Applies the SuffixMultipliers to the input string, and returns the
		/// total multiplier value for parsing.
		/// </summary>
		/// <param name="input">
		/// The input string to be searched for suffixes.
		/// </param>
		/// <param name="ignorExclusions">
		/// 'true' if the method should not consider the Exclusions list whilst
		/// calculating the multiplier; else 'false'.
		/// </param>
		/// <param name="multiplier">
		/// Output. The multipler to be applied when parsing input.
		/// </param>
		/// <param name="suffixFound">
		/// Output. 'true' if at least one suffix was found in the input string; else 'false'.
		/// </param>
		/// <returns>
		/// 'input' with the suffixes removed.
		/// </returns>
		protected string ConsiderSuffixes(string input, bool ignorExclusions, out double multiplier, out bool suffixFound)
		{
			// ----------------------------------------------------------------
			// KNOWN PROBLEM: 2008-01-16
			// ----------------------------------------------------------------
			// If multiple suffixes exist in input, and one suffix is a
			// component of another the suffix will be applied erroneously.
			// ----------------------------------------------------------------
			// e.g. Consider the two suffixes:
			//   m     = 0.001
			//   milli = 0.001
			// Because the first suffix, 'm', is found within the second suffix
			// the multiplier for m will be applied, as well as that for milli.
			// ----------------------------------------------------------------

			const string SUFFIX_REGEX = @"(?<=.*){0}(?=\D*)";

			multiplier = 1;
			suffixFound = false;

			if (input == null)
				return null;

			string rtn = input;
			foreach (KeyValuePair<string, double> suffixMultiplier in this.suffixMultipliers)
			{
				string suffixPattern = suffixMultiplier.Key;
				if (ignorExclusions && IsSuffixInExclusionList(suffixPattern))
					continue;

				Regex regex = new Regex(string.Format(SUFFIX_REGEX, suffixPattern));
				string tmp = regex.Replace(rtn, "");

				int suffixCount = (rtn.Length - tmp.Length) / (suffixPattern.Length);
				if (suffixCount > 0)
					suffixFound = true;

				multiplier *= Math.Pow(suffixMultiplier.Value, suffixCount);
				rtn = tmp;
			}

			return rtn;
		}

		/// <summary>
		/// Returns a value indiating whether the provided suffix is located in the exclusions list.
		/// </summary>
		/// <param name="suffix">
		/// The suffix to be tested to determine if it is in the exclusions list.
		/// </param>
		/// <returns>
		/// 'true' if the suffix is found in the exclusions list; else 'false'.
		/// </returns>
		protected static bool IsSuffixInExclusionList(string suffix)
		{
			return (Array.IndexOf(suffixFormatExclusions, suffix) >= 0);
		}

		/// <summary>
		/// Joins the sub components of the format string into a single format
		/// string, which can be used in ToString conversions.
		/// </summary>
		/// <returns>
		/// The string containing the single format string; or 'null' if the
		/// sub components all contain 'null'.
		/// </returns>
		protected virtual string JoinFormatString()
		{
			StringBuilder sb = null;

			if (FormatSubStringPositive != null)
			{
				sb = new StringBuilder(FormatSubStringPositive.Length*3);
				sb.Append(FormatSubStringPositive);

				if (FormatSubStringNegative != null)
				{
					sb.Append(";");
					sb.Append(FormatSubStringNegative);

					if (FormatSubStringZero != null)
					{
						sb.Append(";");
						sb.Append(FormatSubStringZero);
					}
				}
			}

			if (sb == null)
				return null;
			else
				return sb.ToString();
		}

		/// <summary>
		/// Splits a single format string into the sub components of the format
		/// string.
		/// </summary>
		/// <param name="formatString">
		/// The format string to be split.
		/// </param>
		protected virtual void SplitFormatString(string formatString)
		{
			FormatSubStringPositive = null;
			FormatSubStringNegative = null;
			FormatSubStringZero = null;

			if (formatString == null)
				return;

			string[] subFormatStrings = formatString.Split(';');

			if (subFormatStrings.Length >= POSITIVE_FORMAT_SUB_STRING_INDEX + 1)
				FormatSubStringPositive = subFormatStrings[POSITIVE_FORMAT_SUB_STRING_INDEX];

			if (subFormatStrings.Length >= NEGATIVE_FORMAT_SUB_STRING_INDEX + 1)
				FormatSubStringNegative = subFormatStrings[NEGATIVE_FORMAT_SUB_STRING_INDEX];

			if (subFormatStrings.Length >= ZERO_FORMAT_SUB_STRING_INDEX + 1)
				FormatSubStringZero = subFormatStrings[ZERO_FORMAT_SUB_STRING_INDEX];
		}

		/// <summary>
		/// Retrieves the format sub string which will govern how this value is
		/// displayed.
		/// </summary>
		/// <param name="value">
		/// The value for which the format sub string is required.
		/// </param>
		/// <returns>
		///  <para>
		///   If value is positive, <see cref="FormatSubStringPositive"/>.
		///  </para>
		///  <para>
		///   If value is negative, <see cref="FormatSubStringNegative"/>, or
		///   <see cref="FormatSubStringPositive"/> is the negative sub string
		///   is null.
		///  </para>
		///  <para>
		///   If value is zero, <see cref="FormatSubStringZero"/>, or
		///   <see cref="FormatSubStringPositive"/> is the zero sub string
		///   is null.
		///  </para>
		/// </returns>
		protected string FormatSubStringForValue(T value)
		{
			int result = value.CompareTo(Convert.ChangeType(0, typeof(T)));

			if (result == 0)
			{
				if (this.FormatSubStringZero != null)
					return this.FormatSubStringZero;
				else
					return this.FormatSubStringPositive;
			}
			else if (result == -1)
			{
				if (this.FormatSubStringNegative != null)
					return this.FormatSubStringNegative;
				else
					return this.FormatSubStringPositive;
			}
			else
			{
				return this.FormatSubStringPositive;
			}
		}

        /// <summary>
        /// Tests if the partial representation of a numeric value can be considered
        /// to be a valid user input.
        /// </summary>
        /// <param name="s">
        /// The input string to be tested.
        /// </param>
        /// <returns>
        /// 'true' if <paramref name="s"/> is a valid input; else 'false'.
        /// </returns>
        protected virtual bool CanParsePartial(string s)
        {
            return this.RegexParseNumeric.IsMatch(s);
        }
        #endregion

		#region Private Methods

		/// <summary>
		/// Gets a delegate to the the TryParse method of the type <typeparamref name="T"/>.
		/// </summary>
		/// <returns>
		/// A delegate to the the TryParse method of the type <typeparamref name="T"/>.
		/// </returns>
		private static TryParseDelegate GetTryParseDelegate()
		{
			try
			{
				MethodInfo tryParseMethod = GetTryParseMethod();
				return (TryParseDelegate) Delegate.CreateDelegate(typeof (TryParseDelegate), tryParseMethod);
			}
			catch
			{
				throw new ArgumentException(string.Format(
									"Type {0} does not contain a method " +
									"bool TryParse(string s, NumberStyles styles, IFormatProvider provider, out {0} t).",
									typeof(T).Name));
			}
		}

		/// <summary>
		/// Gets the MethodInfo for the TryParse method of the type <typeparamref name="T"/>.
		/// </summary>
		/// <returns>
		/// The MethodInfo for the TryParse method of the type <typeparamref name="T"/>.
		/// </returns>
		private static MethodInfo GetTryParseMethod()
		{
			Type type = typeof(T);
			MethodInfo tryParse = type.GetMethod(
				"TryParse",
				BindingFlags.Public | BindingFlags.Static,
				null,
				new Type[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), type.MakeByRefType() },
				null);

			return tryParse;
		}

        /// <summary>
        /// Gets the Regex which can be used to test for a partial parse of a
        /// numeric input.
        /// </summary>
        private Regex RegexParseNumeric
        {
            get
            {
                if (regexParseNumeric == null)
                {
                    regexParseNumeric = new Regex(
                        GenerateRegexParseNumericPattern(),
                        RegexOptions.Compiled);
                }

                return regexParseNumeric;
            }
        }

        /// <summary>
        /// Generates the regex pattern which can be used to test for numerics
        /// according to <see cref="Styles"/>.
        /// </summary>
        /// <returns>
        /// A string containing the regex pattern for <see cref="Styles"/>.
        /// </returns>
        private string GenerateRegexParseNumericPattern()
        {
            NumberFormatInfo numberFormat = NumberFormatInfo.CurrentInfo;

            //Test for the positive or negative sign
            string regexPatternSign =
                @"(" + LiteralToRegexPartial(numberFormat.PositiveSign) +
                @"|" + LiteralToRegexPartial(numberFormat.NegativeSign) + @")";

            //Test for leading sign identifiers, either "+", "-" or the open brace.
            //Remember that the close brace can only be used if the open brace is specified.
            string regexPatternSignIdentifierPrefix =
                @"((?<notParenthesis>)" + regexPatternSign + @"|\()?";

            //Test for the close brace if it can be specified.
            string regexPatternSignIdentifierSuffix =
                @"(?(notParenthesis)|\))?";

            //Test for digits, allowing for groupings
            string regexPatternDigits =
                @"(\d(" + numberFormat.NumberGroupSeparator + @"?\d*)*)?";

            //Test for decimal digits
            string regexPatternDecimal =
                @"(\.\d*)?";

            //Test for a specified exponent
            string regexPatternExponent =
                @"((?<=\d)([Ee]" + regexPatternSign + @"?\d*))?";

            //Suffixes
            string regexPatternSuffixes = "";
            Dictionary<string, double>.KeyCollection keys = suffixMultipliers.Keys;
            if (keys.Count > 0)
            {
                StringBuilder sbRegexPatternSuffixList = new StringBuilder();
                foreach (string suffix in keys)
                {
                    sbRegexPatternSuffixList.Append(LiteralToRegexPartial(suffix) + @"|");
                }
                regexPatternSuffixes = @"\s*(" + sbRegexPatternSuffixList.ToString() + @")*";
            }

            //Concatenate the patterns as required.
            StringBuilder sbRegexParsePattern = new StringBuilder();

            if ((this.Styles & NumberStyles.AllowLeadingWhite) == NumberStyles.AllowLeadingWhite)
                sbRegexParsePattern.Append(@"\s*");

            if ((this.Styles & NumberStyles.AllowThousands) == NumberStyles.AllowThousands)
                sbRegexParsePattern.Append(regexPatternDigits);
            else
                sbRegexParsePattern.Append(@"\d*");

            if ((this.Styles & NumberStyles.AllowDecimalPoint) == NumberStyles.AllowDecimalPoint)
                sbRegexParsePattern.Append(regexPatternDecimal);

            if ((this.Styles & NumberStyles.AllowExponent) == NumberStyles.AllowExponent)
                sbRegexParsePattern.Append(regexPatternExponent);

            if ((this.Styles & NumberStyles.AllowTrailingWhite) == NumberStyles.AllowTrailingWhite)
                sbRegexParsePattern.Append(@"\s*");

            sbRegexParsePattern.Append(regexPatternSuffixes);

            if ((this.Styles & NumberStyles.AllowParentheses) == NumberStyles.AllowParentheses)
            {
                sbRegexParsePattern.Insert(0, regexPatternSignIdentifierPrefix);
                sbRegexParsePattern.Append(regexPatternSignIdentifierSuffix);
            }
            else
            {
                sbRegexParsePattern.Insert(0, regexPatternSign);
            }

            sbRegexParsePattern.Insert(0, @"^");
            sbRegexParsePattern.Append(@"$");

            return sbRegexParsePattern.ToString();
        }

        /// <summary>
        /// Creates a string which can be used in a regular expression to test
        /// if a string has been partially entered.
        /// </summary>
        /// <param name="s">
        /// The original string for which a regular expression is required to
        /// test for partial strings.
        /// </param>
        /// <returns>
        /// A string which can be used by a regular expression to test for a
        /// partially entered representation of <paramref name="s"/>
        /// </returns>
        /// <example>
        /// Consider the case where the string <paramref name="s"/> is "<c>this</c>".
        /// The returned value will be:<br/>
        /// "<c>t(h(i(s)?)?)?</c>"
        /// </example>
        private static string LiteralToRegexPartial(string s)
        {
            const string REGEX_SPECIAL_CHARS = @"\.^$*+?{}[]()";

            StringBuilder regexPartial = new StringBuilder((s.Length * 4) - 3);
            for (int i = s.Length - 1; i >= 0; i--)
            {
                string c = s[i].ToString();
                if (REGEX_SPECIAL_CHARS.Contains(c))
                    c = @"\" + c;

                if (regexPartial.Length == 0)
                {
                    regexPartial.Append(c);
                }
                else
                {
                    regexPartial.Insert(0, c + "(");
                    regexPartial.Append(")?");
                }
            }

            return regexPartial.ToString();
        }

        /// <summary>
        /// Ensures that number styles which can not be parsed are not included.
        /// </summary>
        /// <param name="numberStyles">
        /// The NumberStyle to be masked.
        /// </param>
        /// <returns>
        /// A masked version of <paramref name="numberStyles"/> which only allows
        /// NumberStyles which can be parsed to be specified.
        /// </returns>
        /// <example>
        /// When <typeparamref name="T"/> is specified as int, the NumberStyles
        /// AllowDecimalPoint and AllowExponent are masked.
        /// </example>
        private NumberStyles MaskStyles(NumberStyles numberStyles)
        {
            T t;
            bool result;

            //Mask decimal
            result = tryParse("1.1", numberStyles, this.provider, out t);
            if (!result)
                numberStyles &= (~NumberStyles.AllowDecimalPoint);

            //Mask exponent
            result = tryParse("1e1", numberStyles, this.provider, out t);
            if (!result)
                numberStyles &= (~NumberStyles.AllowExponent);

            return numberStyles;
        }
        #endregion

		#region Event Handlers

		private void FormatParser_ProviderChanged(object sender, EventArgs e)
		{
			OnChanged();
		}

		private void FormatParser_StylesChanged(object sender, EventArgs e)
		{
			OnChanged();
		}

		private void FormatParser_FormatStringChanged(object sender, EventArgs e)
		{
			OnChanged();
		}
		#endregion
	}
}
