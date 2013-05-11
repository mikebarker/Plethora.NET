using System;
using System.ComponentModel;
using Plethora.Windows.Forms;

namespace Plethora.Format
{
    /// <summary>
    /// Format and parser for numeric data types.
    /// </summary>
    [CLSCompliant(false)]
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class NullableFormatParserBase<T> : Component, IFormatParserPartial<Nullable<T>>
        where T : struct
    {
        #region Fields

        private readonly IFormatParserPartial<T> innerFormatParser;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialise a new instance of the <see cref="NullableFormatParserBase{T}"/> class.
        /// </summary>
        /// <param name="formatParser">
        /// The <see cref="IFormatParserPartial{T}"/> to be used interally in this instance.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///  <para>
        ///   Thrown if <paramref name="formatParser"/> is 'null'.
        ///  </para>
        /// </exception>
        protected NullableFormatParserBase(IFormatParserPartial<T> formatParser)
        {
            //Validation
            if (formatParser == null)
                throw new ArgumentNullException("formatParser");


            this.innerFormatParser = formatParser;

            WireEvents();
        }
        #endregion

        #region IFormatParserPartial<Nullable<T>> Members

        /// <summary>
        /// Raised when the properties of the <see cref="IFormatParser{T}"/> are changed.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Converts the <typeparamref name="T"/>? value to its equivalent string
        /// representation.
        /// </summary>
        /// <param name="value">
        /// The value to be formatted as a string.
        /// </param>
        /// <returns>
        /// The string equivalent of <paramref name="value"/>.
        /// </returns>
        public virtual string Format(Nullable<T> value)
        {
            return (!value.HasValue)
                ? this.NullString
                : innerFormatParser.Format(value.Value);
        }

        /// <summary>
        /// Converts the string representation of a number to its equivalent
        /// <typeparamref name="T"/>? value.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="s">A string containing the value to convert.</param>
        /// <param name="result">
        ///  <para>
        ///   When this method returns, contains the <typeparamref name="T"/> value
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
        public virtual bool TryParse(string s, out T? result)
        {
            if ((s.Length == 0) || (string.Equals(s, this.NullString)))
            {
                result = null;
                return true;
            }
            else
            {
                T innerResult;
                bool canParse = innerFormatParser.TryParse(s, out innerResult);

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
                return innerFormatParser.CanParse(s, partial);
            }
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
        [Category(ControlAttributes.Category.Behavior)]
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

        #endregion

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="Changed"/> event.
        /// </summary>
        protected virtual void OnChanged()
        {
            EventHandler handlers = this.Changed;
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the inner <see cref="IFormatParserPartial{T}"/>.
        /// </summary>
        protected IFormatParserPartial<T>  InnerFormatParser
        {
            get { return this.innerFormatParser; }
        }

        #endregion

        #region Private Methods

        private void WireEvents()
        {
            this.innerFormatParser.Changed += innerFormatParser_Changed;
        }
        #endregion

        #region Event Handlers

        void innerFormatParser_Changed(object sender, EventArgs e)
        {
            OnChanged();
        }
        #endregion
    }
}
