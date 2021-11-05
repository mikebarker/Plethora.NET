using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Plethora.Windows.Forms;

namespace Plethora.Format
{
    /// <summary>
    /// Format and parser for numeric data types.
    /// </summary>
    [CLSCompliant(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public class DateTimeFormatParser : Component, IFormatParserPartial<DateTime>, ICloneable
    {
        #region Static Members

        private static readonly DateTimeFormatParser defaultFormatParser = new DateTimeFormatParser();

        /// <summary>
        /// Gets the default <see cref="DateTimeFormatParser"/>.
        /// </summary>
        public static DateTimeFormatParser Default
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
        public DateTimeFormatParser()
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
        public DateTimeFormatParser(string format)
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
        public DateTimeFormatParser(string format, IFormatProvider provider)
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
        public DateTimeFormatParser(string format, DateTimeStyles styles)
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
        public DateTimeFormatParser(string format, IFormatProvider provider, DateTimeStyles styles)
        {
            //Validation
            if (format == null)
                throw new ArgumentNullException(nameof(format));

            if (provider == null)
                throw new ArgumentNullException(nameof(provider));


            this.FormatString = format;
            this.provider = provider;
            this.styles = styles;
        }
        #endregion

        #region IFormatParserPartial<T> Members

        /// <summary>
        /// Raised when the properties of the <see cref="IFormatParser{T}"/> are changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Converts the <see cref="DateTime"/> value to its equivalent string
        /// representation.
        /// </summary>
        /// <param name="value">
        /// The value to be formatted as a string.
        /// </param>
        /// <returns>
        /// The string equivalent of <paramref name="value"/>.
        /// </returns>
        public virtual string Format(DateTime value)
        {
            return value.ToString(this.FormatString, this.Provider);
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
        public virtual bool TryParse(string s, out DateTime result)
        {
            return DateTime.TryParse(s, this.Provider, this.Styles, out result);
        }

        /// <summary>
        /// Determines whether a string representation of a number can be converted
        /// to its equivalent value of type <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="partial">
        ///  <para>
        ///   'true' if the string is only a partial representation of
        ///   type <see cref="DateTime"/>; else, 'false'.
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
                DateTime result;
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
        public DateTimeFormatParser Clone()
        {
            var clone = new DateTimeFormatParser(
                this.formatString,
                this.provider,
                this.styles);

            return clone;
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
        protected internal const string DEFAULT_FORMAT = "d";

        private string formatString = DEFAULT_FORMAT;

        /// <summary>
        /// Gets and sets the format string for this
        /// <see cref="NumericFormatParser{T}"/>.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [Description("The format string for this formatter.")]
        public string FormatString
        {
            get { return formatString; }
            set
            {
                if (string.Equals(value, formatString))
                    return;

                this.formatString = value;
                OnFormatStringChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="FormatStringChanged"/> event.
        /// </summary>
        protected virtual void OnFormatStringChanged()
        {
            EventHandler handlers = this.FormatStringChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged();
        }
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
        protected virtual void OnProviderChanged()
        {
            EventHandler handlers = this.ProviderChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged();
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
        protected internal const DateTimeStyles DEFAULT_STYLES = DateTimeStyles.None;

        private DateTimeStyles styles = DEFAULT_STYLES;

        /// <summary>
        /// Gets and sets the NumberStyle to be used by this <see cref="NumericFormatParser{T}"/>
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Behavior)]
        [DefaultValue(DEFAULT_STYLES)]
        [Description("The NumberStyle to be used by this NumericFormatParser.")]
        public virtual DateTimeStyles Styles
        {
            get { return styles; }
            set
            {
                if (value == styles)
                    return;

                styles = value;
                OnStylesChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="StylesChanged"/> event.
        /// </summary>
        protected virtual void OnStylesChanged()
        {
            EventHandler handlers = this.StylesChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged();
        }

        #endregion

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Changed"/>.
        /// </summary>
        protected virtual void OnChanged()
        {
            EventHandler handlers = this.Changed;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
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
            return ValidatePartialDateTime(s);
        }
        #endregion

        #region Private Methods

        protected bool ValidatePartialDateTime(string partialDateTime)
        {
            return true;

            //TODO: Too restrictive (can't parse "12 Sep 2012")
            if ((styles & DateTimeStyles.AllowLeadingWhite) == DateTimeStyles.AllowLeadingWhite)
                partialDateTime = partialDateTime.TrimStart();

            if ((styles & DateTimeStyles.AllowTrailingWhite) == DateTimeStyles.AllowTrailingWhite)
                partialDateTime = partialDateTime.TrimEnd();

            bool allowWhiteSpace = (styles & DateTimeStyles.AllowInnerWhite) == DateTimeStyles.AllowInnerWhite;

            string[] portions = null;
            foreach (var separator in DateTimeSeparators)
            {
                if (partialDateTime.Contains(separator))
                {
                    StringSplitOptions options = StringSplitOptions.None;
                    if ((allowWhiteSpace) && (char.IsWhiteSpace(separator)))
                    {
                        options = StringSplitOptions.RemoveEmptyEntries;
                    }

                    portions = partialDateTime.Split(new[] { separator }, options);
                    break;
                }
            }

            string partialDate;
            string partialTime;
            if (portions == null)
            {
                //No separator found, only working with date portion
                partialDate = partialDateTime;
                partialTime = null;
            }
            else if (portions.Length == 1)
            {
                //Only working with date portion
                partialDate = portions[0];
                partialTime = null;
            }
            else if (portions.Length == 2)
            {
                partialDate = portions[0];
                partialTime = portions[1];
            }
            else // portions.Length > 2
            {
                // There is more than one date-time separator, therefore not a valid DateTime
                return false;
            }

            bool result = true;

            if (partialDate != null)
                result = result && ValidateDate(partialDate);

            if (partialTime != null)
                result = result && ValidateTime(partialTime);

            return result;
        }

        private static bool ValidateDate(string partialDate)
        {
            string[] portions = null;
            foreach (var separator in DateSeparators)
            {
                if (partialDate.Contains(separator))
                {
                    portions = partialDate.Split(new[] { separator }, StringSplitOptions.None);
                    break;
                }
            }

            if (portions == null)
            {
                portions = new[] { partialDate };
            }

            return
                ValidateYMD(portions) ||
                ValidateDMY(portions) ||
                ValidateMDY(portions);
        }

        private static bool ValidateYMD(string[] ymd)
        {
            var length = ymd.Length;
            if (length == 1)
            {
                return
                    ValidateYear(ymd[0], true);
            }
            else if (length == 2)
            {
                return
                    ValidateYear(ymd[0], false) &&
                    ValidateMonth(ymd[1], true);
            }
            else if (length == 3)
            {
                return
                    ValidateYear(ymd[0], false) &&
                    ValidateMonth(ymd[1], false) &&
                    ValidateDay(ymd[2], true);
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateDMY(string[] dmy)
        {
            var length = dmy.Length;
            if (length == 1)
            {
                return
                    ValidateDay(dmy[0], true);
            }
            else if (length == 2)
            {
                return
                    ValidateDay(dmy[0], false) &&
                    ValidateMonth(dmy[1], true);
            }
            else if (length == 3)
            {
                return
                    ValidateDay(dmy[0], false) &&
                    ValidateMonth(dmy[1], false) &&
                    ValidateYear(dmy[2], true);
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateMDY(string[] mdy)
        {
            var length = mdy.Length;
            if (length == 1)
            {
                return
                    ValidateMonth(mdy[0], true);
            }
            else if (length == 2)
            {
                return
                    ValidateMonth(mdy[0], false) &&
                    ValidateDay(mdy[1], true);
            }
            else if (length == 3)
            {
                return
                    ValidateMonth(mdy[0], false) &&
                    ValidateDay(mdy[1], false) &&
                    ValidateYear(mdy[2], true);
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateYear(string partialYear, bool isLastPortion)
        {
            if (partialYear.Length == 2)
                return true; //represent year in YY format

            if (partialYear.Length < 4)
                return isLastPortion;

            if (partialYear.Length > 4)
                return false;

            int year;
            bool result = int.TryParse(partialYear, out year);
            if (!result)
                return false;

            return ((DateTime.MinValue.Year <= year) && (year <= DateTime.MaxValue.Year));
        }

        private static bool ValidateMonth(string partialMonth, bool isLastPortion)
        {
            if (partialMonth.Length == 0)
                return isLastPortion;

            if (partialMonth.Length == 1)
            {
                if (string.Equals(partialMonth, "0"))
                    return isLastPortion;
            }

            int month;
            bool result = int.TryParse(partialMonth, out month);
            if (result)
            {
                if (partialMonth.Length >= 3)
                    return false;

                return ((1 <= month) && (month <= 12));
            }
            else
            {
                partialMonth = partialMonth.ToUpper(CultureInfo.CurrentCulture);
                return MonthNames
                    .Any(monthName => monthName.StartsWith(partialMonth));
            }
        }

        private static bool ValidateDay(string partialDay, bool isLastPortion)
        {
            if (partialDay.Length == 0)
                return isLastPortion;

            if (partialDay.Length == 1)
            {
                if (string.Equals(partialDay, "0"))
                    return isLastPortion;
            }

            if (partialDay.Length > 2)
                return false;

            int day;
            bool result = int.TryParse(partialDay, out day);
            if (!result)
                return false;

            return ((1 <= day) && (day <= 31));
        }


        private static bool ValidateTime(string partialTime)
        {
            if (partialTime.Length == 0)
                return true;

            var timePortions = partialTime.Split(':');
            if (timePortions.Length == 1)
            {
                return
                    ValidateHour(timePortions[0], true);
            }
            else if (timePortions.Length == 2)
            {
                return
                    ValidateHour(timePortions[0], false) &&
                    ValidateMinute(timePortions[1], true);
            }
            else if (timePortions.Length == 3)
            {
                return
                    ValidateHour(timePortions[0], false) &&
                    ValidateMinute(timePortions[1], false) &&
                    ValidateSecond(timePortions[2], true);
            }
            else
            {
                return false;
            }
        }

        private static bool ValidateHour(string partialHour, bool isLastPortion)
        {
            if (partialHour.Length == 0)
                return isLastPortion;

            if (partialHour.Length > 2)
                return false;

            int hours;
            bool result = int.TryParse(partialHour, out hours);
            if (!result)
                return false;

            return ((0 <= hours) && (hours <= 24));
        }

        private static bool ValidateMinute(string partialMinute, bool isLastPortion)
        {
            if (partialMinute.Length == 0)
                return isLastPortion;

            if (partialMinute.Length > 2)
                return false;

            int minutes;
            bool result = int.TryParse(partialMinute, out minutes);
            if (!result)
                return false;

            return ((0 <= minutes) && (minutes < 60));
        }

        private static bool ValidateSecond(string partialSecond, bool isLastPortion)
        {
            if (partialSecond.Length == 0)
                return isLastPortion;

            float seconds;
            bool result = float.TryParse(partialSecond, out seconds);
            if (!result)
                return false;

            return ((0.0f <= seconds) && (seconds < 60.0f));
        }


        private static List<string> monthNames;
        private static IEnumerable<string> MonthNames
        {
            get
            {
                if (monthNames == null)
                {
                    monthNames = new List<string>(24);

                    var dateTimeFormatInfo = DateTimeFormatInfo.CurrentInfo;
                    var uiCulture = CultureInfo.CurrentCulture;
                    for (int i = 1; i < 12; i++)
                    {
                        monthNames.Add(dateTimeFormatInfo.GetMonthName(i).ToUpper(uiCulture));
                        monthNames.Add(dateTimeFormatInfo.GetAbbreviatedMonthName(i).ToUpper(uiCulture));
                    }

                    monthNames.TrimExcess();
                }
                return monthNames;
            }
        }

        private static List<string> dateSeparators;
        private static IEnumerable<string> DateSeparators
        {
            get
            {
                if (dateSeparators == null)
                {
                    dateSeparators = new List<string> { @"-", @"\", @" " };

                    string formatInfoSeparator = DateTimeFormatInfo.CurrentInfo.DateSeparator;
                    if (!dateSeparators.Contains(formatInfoSeparator))
                    {
                        dateSeparators.Add(formatInfoSeparator);
                    }

                    dateSeparators.TrimExcess();
                }
                return dateSeparators;
            }
        }

        private readonly static char[] DateTimeSeparators = new[] { 'Z', 'T', ' ' };
        #endregion
    }
}
