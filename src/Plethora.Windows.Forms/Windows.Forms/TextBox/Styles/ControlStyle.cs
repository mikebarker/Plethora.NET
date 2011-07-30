using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// Style to be applied to a <see cref="Control"/>.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ControlStyle
    {
        #region Events

        private static readonly object Changed_EventKey = new object();

        /// <summary>
        /// Raised when any of the values of this <see cref="ControlStyle"/> have changed.
        /// </summary>
        public event EventHandler Changed
        {
            add { this.Events.AddHandler(Changed_EventKey, value); }
            remove { this.Events.RemoveHandler(Changed_EventKey, value); }
        }


        protected virtual void OnChanged(EventArgs e)
        {
            var handler = this.Events[Changed_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Properties

        private EventHandlerList events;
        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                {
                    Interlocked.CompareExchange(ref this.events, new EventHandlerList(), null);
                }
                return this.events;
            }
        }

        #region ForeColor Property

        /// <summary>
        /// Raised when <see cref="ForeColor"/> changes.
        /// </summary>
        /// <seealso cref="ForeColor"/>
        public event EventHandler ForeColorChanged;

        private Color foreColor = Color.Empty;

        /// <summary>
        /// Gets and sets the foreground colour for this style.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(typeof(Color), "Empty")]
        [Description("The foreground colour for this style.")]
        public virtual Color ForeColor
        {
            get { return foreColor; }
            set
            {
                if (value == foreColor)
                    return;

                foreColor = value;
                OnForeColorChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="ForeColorChanged"/> event.
        /// </summary>
        protected void OnForeColorChanged()
        {
            EventHandler handlers = this.ForeColorChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region BackColor Property

        /// <summary>
        /// Raised when <see cref="BackColor"/> changes.
        /// </summary>
        /// <seealso cref="BackColor"/>
        public event EventHandler BackColorChanged;

        private Color backColor = Color.Empty;

        /// <summary>
        /// Gets and sets the foreground colour for this style.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(typeof(Color), "Empty")]
        [Description("The foreground colour for this style.")]
        public virtual Color BackColor
        {
            get { return backColor; }
            set
            {
                if (value == backColor)
                    return;

                backColor = value;
                OnBackColorChanged();
            }
        }

        /// <summary>
        /// Raises the <see cref="BackColorChanged"/> event.
        /// </summary>
        protected void OnBackColorChanged()
        {
            EventHandler handlers = this.BackColorChanged;
            if (handlers != null)
                handlers(this, EventArgs.Empty);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontName Property

        private static readonly object FontNameChanged_EventKey = new object();
        private const string FontName_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="FontName"/> has changed.
        /// </summary>
        public event EventHandler FontNameChanged
        {
            add { this.Events.AddHandler(FontNameChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontNameChanged_EventKey, value); }
        }

        private string fontName = FontName_DefaultValue;

        /// <summary>
        /// Gets and sets the name of the font to applied for this style.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(FontName_DefaultValue)]
        [Description("The name of the font to applied for this style.")]
        public virtual string FontName
        {
            get { return fontName; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length == 0)
                        value = null;
                }

                if (this.fontName == value)
                    return;

                this.fontName = value;
                this.OnFontNameChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontNameChanged(EventArgs e)
        {
            var handler = this.Events[FontNameChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontSize Property

        private static readonly object FontSizeChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="FontSize"/> has changed.
        /// </summary>
        public event EventHandler FontSizeChanged
        {
            add { this.Events.AddHandler(FontSizeChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontSizeChanged_EventKey, value); }
        }

        private float? fontSize = null;

        /// <summary>
        /// Gets and sets the size of the font to applied for this style.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("The size of the font to applied for this style.")]
        public virtual float? FontSize
        {
            get { return fontSize; }
            set
            {
                if (value.HasValue && value.Value <= 0)
                    throw new ArgumentOutOfRangeException("value", value,
                        ResourceProvider.ArgMustBeGreaterThanZero("value"));

                if (this.fontSize == value)
                    return;

                this.fontSize = value;
                this.OnFontSizeChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontSizeChanged(EventArgs e)
        {
            var handler = this.Events[FontSizeChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontBold Property

        private static readonly object FontBoldChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="FontBold"/> has changed.
        /// </summary>
        public event EventHandler FontBoldChanged
        {
            add { this.Events.AddHandler(FontBoldChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontBoldChanged_EventKey, value); }
        }

        private bool? fontBold = null;

        /// <summary>
        /// Gets and sets a flag indicating whether the font to applied for this style is bold.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("A flag indicating whether the font to applied for this style is bold.")]
        public virtual bool? FontBold
        {
            get { return fontBold; }
            set
            {
                if (this.fontBold == value)
                    return;

                this.fontBold = value;
                this.OnFontBoldChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontBoldChanged(EventArgs e)
        {
            var handler = this.Events[FontBoldChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontItalic Property

        private static readonly object FontItalicChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="FontItalic"/> has changed.
        /// </summary>
        public event EventHandler FontItalicChanged
        {
            add { this.Events.AddHandler(FontItalicChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontItalicChanged_EventKey, value); }
        }

        private bool? fontItalic = null;

        /// <summary>
        /// Gets and sets a flag indicating whether the font to applied for this style is italic.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("A flag indicating whether the font to applied for this style is italic.")]
        public virtual bool? FontItalic
        {
            get { return fontItalic; }
            set
            {
                if (this.fontItalic == value)
                    return;

                this.fontItalic = value;
                this.OnFontItalicChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontItalicChanged(EventArgs e)
        {
            var handler = this.Events[FontItalicChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontUnderline Property

        private static readonly object FontUnderlineChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="FontUnderline"/> has changed.
        /// </summary>
        public event EventHandler FontUnderlineChanged
        {
            add { this.Events.AddHandler(FontUnderlineChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontUnderlineChanged_EventKey, value); }
        }

        private bool? fontUnderline = null;

        /// <summary>
        /// Gets and sets a flag indicating whether the font to applied for this style is underlined.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("A flag indicating whether the font to applied for this style is underlined.")]
        public virtual bool? FontUnderline
        {
            get { return fontUnderline; }
            set
            {
                if (this.fontUnderline == value)
                    return;

                this.fontUnderline = value;
                this.OnFontUnderlineChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontUnderlineChanged(EventArgs e)
        {
            var handler = this.Events[FontUnderlineChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion

        #region FontStrikeout Property

        private static readonly object FontStrikeoutChanged_EventKey = new object();

        /// <summary>
        /// Raised when the value of <see cref="FontStrikeout"/> has changed.
        /// </summary>
        public event EventHandler FontStrikeoutChanged
        {
            add { this.Events.AddHandler(FontStrikeoutChanged_EventKey, value); }
            remove { this.Events.RemoveHandler(FontStrikeoutChanged_EventKey, value); }
        }

        private bool? fontStrikeout = null;

        /// <summary>
        /// Gets and sets a flag indicating whether the font to applied for this style has a horizontal line through it.
        /// </summary>
        [Browsable(true)]
        [Category(ControlAttributes.Category.Appearance)]
        [DefaultValue(null)]
        [Description("A flag indicating whether the font to applied for this style has a horizontal line through it.")]
        public virtual bool? FontStrikeout
        {
            get { return fontStrikeout; }
            set
            {
                if (this.fontStrikeout == value)
                    return;

                this.fontStrikeout = value;
                this.OnFontStrikeoutChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnFontStrikeoutChanged(EventArgs e)
        {
            var handler = this.Events[FontStrikeoutChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);

            OnChanged(EventArgs.Empty);
        }

        #endregion
        #endregion

        #region Public Methods

        /// <summary>
        /// Applies this stlye to the text box.
        /// </summary>
        /// <param name="control">
        /// The control to which the style must be applied.
        /// </param>
        public virtual void ApplyStyle(Control control)
        {
            if (this.ForeColor != Color.Empty)
                control.ForeColor = this.ForeColor;

            if (this.BackColor != Color.Empty)
                control.BackColor = this.BackColor;


            var controlFont = control.Font;
            bool fontChangeRequired = false;

            if (!fontChangeRequired && (this.FontName != null) && (!string.Equals(this.FontName, controlFont.Name)))
                fontChangeRequired = true;

            if (!fontChangeRequired && (this.FontSize != null) && (this.FontSize != controlFont.Size))
                fontChangeRequired = true;

            if (!fontChangeRequired && (this.FontBold != null) && (this.FontBold != controlFont.Bold))
                fontChangeRequired = true;

            if (!fontChangeRequired && (this.FontItalic != null) && (this.FontItalic != controlFont.Italic))
                fontChangeRequired = true;

            if (!fontChangeRequired && (this.FontUnderline != null) && (this.FontUnderline != controlFont.Underline))
                fontChangeRequired = true;

            if (!fontChangeRequired && (this.FontStrikeout != null) && (this.FontStrikeout != controlFont.Strikeout))
                fontChangeRequired = true;

            if (fontChangeRequired)
            {
                FontStyle style = controlFont.Style;
                if (FontBold != null)
                    style = SetFontStyleFlag(style, this.FontBold.Value, FontStyle.Bold);

                if (FontItalic != null)
                    style = SetFontStyleFlag(style, this.FontItalic.Value, FontStyle.Italic);

                if (FontUnderline != null)
                    style = SetFontStyleFlag(style, this.FontUnderline.Value, FontStyle.Underline);

                if (FontStrikeout != null)
                    style = SetFontStyleFlag(style, this.FontStrikeout.Value, FontStyle.Strikeout);

                control.Font = new Font(
                    FontName ?? controlFont.Name,
                    FontSize ?? controlFont.Size,
                    style);
            }
        }

        /// <summary>
        /// Set the default values of all properties from <see cref="control"/>.
        /// </summary>
        /// <param name="control">The control from which to copy property values.</param>
        public virtual void GetPropertyValues(Control control)
        {
            this.ForeColor = control.ForeColor;
            this.BackColor = control.BackColor;
            this.FontName = control.Font.Name;
            this.FontSize = control.Font.Size;
            this.FontBold = control.Font.Bold;
            this.FontItalic = control.Font.Italic;
            this.FontUnderline = control.Font.Underline;
            this.FontStrikeout = control.Font.Strikeout;
        }

        /// <summary>
        /// Override the unset properties of <see cref="style"/> using this <see cref="ControlStyle"/>
        /// values.
        /// </summary>
        /// <param name="style">
        /// The <see cref="ControlStyle"/> for which the properties are to be overridden.
        /// </param>
        public virtual void OverrideUnsetValues(ControlStyle style)
        {
            if (style.ForeColor == Color.Empty)
                style.ForeColor = this.ForeColor;

            if (style.BackColor == Color.Empty)
                style.BackColor = this.BackColor;

            if (style.FontName == null)
                style.FontName = this.FontName;

            if (style.FontSize == null)
                style.FontSize = this.FontSize;

            if (style.FontBold == null)
                style.FontBold = this.FontBold;

            if (style.FontItalic == null)
                style.FontItalic = this.FontItalic;

            if (style.FontUnderline == null)
                style.FontUnderline = this.FontUnderline;

            if (style.FontStrikeout == null)
                style.FontStrikeout = this.FontStrikeout;
        }
        #endregion

        #region Static Methods

        private static FontStyle SetFontStyleFlag(FontStyle originalValue, bool flag, FontStyle flagIfTrue)
        {
            if (flag)
                return originalValue | flagIfTrue;
            else
                return originalValue & ~flagIfTrue;
        }

        /// <summary>
        ///  Applies the list of styles to the text box.
        /// </summary>
        /// <param name="control">
        ///  The control to which the styles must be applied.
        /// </param>
        /// <param name="styles">
        ///  The list of styles to be applied to the text box.
        /// </param>
        /// <remarks>
        ///  The styles are applied in decresing preference order through the list.
        /// </remarks>
        public static void ApplyStyles<TStyle>(Control control, params TStyle[] styles)
            where TStyle : ControlStyle, new()
        {
            var combinedStyle = CombineStyles(styles);
            if (combinedStyle != null)
                combinedStyle.ApplyStyle(control);
        }

        private static TStyle CombineStyles<TStyle>(params TStyle[] styles)
            where TStyle : ControlStyle, new()
        {
            if (styles == null)
                return null;

            if (styles.Length == 0)
                return null;

            TStyle rtnStyle = null;
            foreach (var style in styles)
            {
                if (style == null)
                    continue;

                //Set only on the first non-null style
                if (rtnStyle == null)
                    rtnStyle = new TStyle();

                style.OverrideUnsetValues(rtnStyle);
            }

            return rtnStyle;
        }
        #endregion
    }
}
