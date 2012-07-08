using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Plethora.Collections;
using Timer = System.Threading.Timer;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// A class used to apply styles to a <see cref="TextBox"/>.
    /// </summary>
    [TypeConverter(typeof(TextBoxStyliser.Converter))]
    public partial class TextBoxStyliser
    {
        #region Static Members

        private static TextBoxStyliser instance;

        /// <summary>
        /// Gets the default instance of <see cref="TextBoxStyliser"/>.
        /// </summary>
        public static TextBoxStyliser Default
        {
            get
            {
                if (instance == null)
                    instance = new TextBoxStyliser();

                return instance;
            }
        }

        #endregion

        #region Fields

        //Serves as the list of registered text boxes, as well as recording their associated default styles.
        private readonly WeakKeyDictionary<TextBox, TextBoxStyle> textBoxStyles =
            new WeakKeyDictionary<TextBox, TextBoxStyle>();

        private EventHandlerList events;
        #endregion

        #region Constructors

        public TextBoxStyliser()
        {
            this.stylesCleanupTimer = new Timer(StylesCleanup, null, LOW_ACTIVITY_TIMER, LOW_ACTIVITY_TIMER);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Register a TextBox with this <see cref="TextBoxStyliser"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="TextBox"/> to be registered with this instance.
        /// </param>
        public virtual void RegisterTextBox(TextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            lock (textBoxStyles)
            {
                if (!textBoxStyles.ContainsKey(textBox))
                {
                    textBoxStyles.Add(textBox, TextBoxStyle.CreateStyle(textBox));
                    Stylise(textBox);
                    WireTextBoxEvents(textBox);
                }
            }
        }

        /// <summary>
        /// Deregister a TextBox from this <see cref="TextBoxStyliser"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="TextBox"/> to be deregistered with this instance.
        /// </param>
        public virtual void DeregisterTextBox(TextBox textBox)
        {
            //Validation
            if (textBox == null)
                throw new ArgumentNullException("textBox");


            TextBoxStyle defaultStyle;
            lock (textBoxStyles)
            {
                if (textBoxStyles.TryGetValue(textBox, out defaultStyle))
                {
                    UnwireTextBoxEvents(textBox);
                    textBoxStyles.Remove(textBox);
                }
            }

            if (defaultStyle != null)
                defaultStyle.ApplyStyle(textBox);
        }
        #endregion

        #region Event Handlers

        void style_Changed(object sender, EventArgs e)
        {
            StyliseAllRegisteredTextBoxes();
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;

            Stylise(textBox);
        }

        void textBox_HandleDestroyed(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null)
                return;

            this.DeregisterTextBox(textBox);
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// Gets the list of event handlers that are attached to this class.
        /// </summary>
        /// <value>
        /// An <see cref="EventHandlerList"/> that provides the delegates for this component
        /// </value>
        protected EventHandlerList Events
        {
            get
            {
                if (this.events == null)
                {
                    this.events = new EventHandlerList();
                }
                return this.events;
            }
        }

        /// <summary>
        /// Wire the events of a TextBox to this styliser.
        /// </summary>
        /// <param name="textBox">
        /// The TextBox to be wired.
        /// </param>
        protected virtual void WireTextBoxEvents(TextBox textBox)
        {
            textBox.HandleDestroyed += new EventHandler(textBox_HandleDestroyed);
            textBox.TextChanged += new EventHandler(textBox_TextChanged);
        }

        /// <summary>
        /// Unwire the events of a TextBox from this styliser.
        /// </summary>
        /// <param name="textBox">
        /// The TextBox to be unwired.
        /// </param>
        protected virtual void UnwireTextBoxEvents(TextBox textBox)
        {
            textBox.TextChanged -= new EventHandler(textBox_TextChanged);
            textBox.HandleDestroyed -= new EventHandler(textBox_HandleDestroyed);
        }

        /// <summary>
        /// Wire the events of a TextBoxStyle to this styliser.
        /// </summary>
        /// <param name="style">
        /// The TextBoxStyle to be wired.
        /// </param>
        protected virtual void WireStyleEvents(TextBoxStyle style)
        {
            style.Changed += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Unwire the events of a TextBoxStyle from this styliser.
        /// </summary>
        /// <param name="style">
        /// The TextBoxStyle to be unwired.
        /// </param>
        protected virtual void UnwireStyleEvents(TextBoxStyle style)
        {
            style.Changed += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Applies the appropriate style to all text boxes registered with the
        /// styliser.
        /// </summary>
        protected void StyliseAllRegisteredTextBoxes()
        {
            lock (textBoxStyles)
            {
                foreach (TextBox textBox in textBoxStyles.Keys)
                {
                    Stylise(textBox);
                }
            }
        }

        /// <summary>
        /// Applies the appropriate style to the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="TextBox"/> to which the style is to be applied.
        /// </param>
        private void Stylise(TextBox textBox)
        {
            TextBoxStyle.ApplyStyles(textBox,
                GetRequiredStyle(textBox),
                GetDefaultStyle(textBox));
        }

        protected virtual TextBoxStyle GetRequiredStyle(TextBox textBox)
        {
            return null;
        }

        /// <summary>
        /// Gets the default style for a textBox.
        /// </summary>
        /// <param name="textBox">
        /// The text box for which the default style is required.
        /// </param>
        /// <returns>
        /// The default style for the text box; or 'null' one could not be
        /// found.
        /// </returns>
        protected TextBoxStyle GetDefaultStyle(TextBox textBox)
        {
            TextBoxStyle defaultStyle;
            lock (textBoxStyles)
            {
                if (!this.textBoxStyles.TryGetValue(textBox, out defaultStyle))
                    return null;
            }
            return defaultStyle;
        }
        #endregion

        #region Private Members

        #region textBoxStyles Cleanup

        private const int LOW_ACTIVITY_TIMER = 5 * 60 * 1000; // 5 min
        private const int HIGH_ACTIVITY_TIMER = 2 * 1000;     // 2 sec

        private readonly Timer stylesCleanupTimer;
        private int inCleanUp = 0;

        private void StylesCleanup(object state)
        {
            if (Interlocked.CompareExchange(ref inCleanUp, 1, 0) != 0)
                return;

            try
            {
                bool anyCleanup;
                lock(textBoxStyles)
                {
                    anyCleanup = textBoxStyles.TrimExcess();
                }

                if (anyCleanup)
                    stylesCleanupTimer.Change(HIGH_ACTIVITY_TIMER, HIGH_ACTIVITY_TIMER);
                else
                    stylesCleanupTimer.Change(LOW_ACTIVITY_TIMER, LOW_ACTIVITY_TIMER);
            }
            finally
            {
                inCleanUp = 0;
            }
        }
        #endregion
        #endregion
    }
}
