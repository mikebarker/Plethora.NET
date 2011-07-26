using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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

        private readonly HashSet<TextBox> textBoxes = new HashSet<TextBox>();

        private EventHandlerList events;
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


            if (!textBoxes.Contains(textBox))
            {
                textBoxes.Add(textBox);
                Stylise(textBox);
                WireTextBoxEvents(textBox);
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


            if (textBoxes.Contains(textBox))
            {
                UnwireTextBoxEvents(textBox);
                textBoxes.Remove(textBox);
            }
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
            style.BackColorChanged += new EventHandler(style_Changed);
            style.ForeColorChanged += new EventHandler(style_Changed);
            style.FontChanged += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Unwire the events of a TextBoxStyle from this styliser.
        /// </summary>
        /// <param name="style">
        /// The TextBoxStyle to be unwired.
        /// </param>
        protected virtual void UnwireStyleEvents(TextBoxStyle style)
        {
            style.BackColorChanged += new EventHandler(style_Changed);
            style.ForeColorChanged += new EventHandler(style_Changed);
            style.FontChanged += new EventHandler(style_Changed);
        }

        /// <summary>
        /// Applies the appripriate style to all text boxes registered with the
        /// styliser.
        /// </summary>
        protected void StyliseAllRegisteredTextBoxes()
        {
            foreach (TextBox textBox in textBoxes)
            {
                Stylise(textBox);
            }
        }

        /// <summary>
        /// Applies the appropriate style to the
        /// <see cref="IValueTextBox"/>.
        /// </summary>
        /// <param name="textBox">
        /// The <see cref="TextBox"/> to which the style is to be applied.
        /// </param>
        private void Stylise(TextBox textBox)
        {
            TextBoxStyle style = GetRequiredStyle(textBox);

            if (style != null)
            {
                style.ApplyStyle(textBox);
            }
        }

        protected virtual TextBoxStyle GetRequiredStyle(TextBox textBox)
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// A <see cref="TextBoxStyliser"/> presented as a component.
    /// </summary>
    /// <remarks>
    /// If <see cref="TextBoxStyliser"/> implemented the <see cref="IComponent"/> interface,
    /// or inheritted from the <see cref="Component"/> class, the
    /// <see cref="System.ComponentModel.Design.Serialization.InstanceDescriptor"/> used in the
    /// converter (see <see cref="Plethora.ComponentModel.DefaultReferenceConverter"/>) would not
    /// render the default instance correctly, due to code access security (cas) permissions.
    /// </remarks>
    public class TextBoxStyliserComponent : TextBoxStyliser, IComponent
    {
        #region Fields

        private readonly Component component = new Component();
        #endregion

        #region Constructors

        public TextBoxStyliserComponent()
        {
        }

        public TextBoxStyliserComponent(IContainer container)
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
        public ISite Site
        {
            get { return component.Site; }
            set { component.Site = value; }
        }

        public event EventHandler Disposed
        {
            add { component.Disposed += value; }
            remove { component.Disposed -= value; }
        }

        #endregion
    }
}
