using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Plethora.Format;
using Plethora.Windows.Forms.Styles;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// TextBox which restricts user input to numeric values.
    /// </summary>
    public abstract class NullableNumericTextBox<T> : ComparableTextBox<T?>
        where T : struct, IFormattable, IComparable, IConvertible, IEquatable<T>, IComparable<T>
    {
        #region Constructors

        protected NullableNumericTextBox()
        {
            this.TextAlign = HorizontalAlignment.Right;
            this.FormatParser = NullableNumericFormatParser<T>.Default;

            SetTextFromValue();
        }
        #endregion

        #region Override Methods

        /// <summary>
        /// Gets and sets the numeric styliser which governs the style of this text box.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("The styliser which governs the style of this text box.")]
        public new virtual NumericTextBoxStyliser Styliser
        {
            get { return (NumericTextBoxStyliser)StyliserInternal; }
            set { StyliserInternal = value; }
        }

        protected override TextBoxStyliser StyliserInternal
        {
            get { return base.StyliserInternal; }
            set
            {
                if ((value != null) && !(value is NumericTextBoxStyliser))
                    throw new InvalidCastException(ResourceProvider.InvalidCast());

                base.StyliserInternal = value;
            }
        }

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
        /// Consider the case where <see cref="NumericTextBox{T}.MinValue"/> is 25. The user wants to
        /// type the number 347. As the user types 3 this function must return true.
        /// </example>
        protected override bool ValidateValuePartial(T? validateValue)
        {
            //NOTE: Must allow for the user to type in the value, so can't simply
            //      test that the value lies between minValue and maxValue.

            // (minValue < 0) && (minValue > validateValue)
            if ((Comparer<T?>.Default.Compare(MinValue, Zero) < 0) &&
                (Comparer<T?>.Default.Compare(MinValue, validateValue) > 0))
                return false;

            // (maxValue > 0) && (maxValue < validateValue)
            if ((Comparer<T?>.Default.Compare(MaxValue, Zero) > 0) &&
                (Comparer<T?>.Default.Compare(MaxValue, validateValue) < 0))
                return false;

            return true;
        }
        #endregion

        /// <summary>
        /// Gets the value of zero for the type <typeparam name="T"/>.
        /// </summary>
        protected virtual T Zero
        {
            get { return default(T); }
        }
    }
}
