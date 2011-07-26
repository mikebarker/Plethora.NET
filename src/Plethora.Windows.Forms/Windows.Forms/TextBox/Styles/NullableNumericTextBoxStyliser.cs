using System;
using System.ComponentModel;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// A component which can be used to apply styles to a TextBox which
    /// implements <see cref="IComparableValueProvider"/>
    /// </summary>
    [TypeConverter(typeof(NullableNumericTextBoxStyliser.Converter))]
    public partial class NullableNumericTextBoxStyliser : NumericTextBoxStyliser
    {
        #region Properties

        #region NullStyle Property

        private static readonly object NullStyleChanged_EventKey = new object();

        /// <summary>
        /// Raised when <see cref="NullStyle"/> changes.
        /// </summary>
        /// <seealso cref="NullStyle"/>
        public event EventHandler NullStyleChanged
        {
            add { base.Events.AddHandler(NullStyleChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(NullStyleChanged_EventKey, value); }
        }

        private TextBoxStyle nullStyle = null;

        /// <summary>
        /// Gets and sets the style to be applied when the text box's value is null.
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The style to be applied when the text box's value is null.")]
        public TextBoxStyle NullStyle
        {
            get { return nullStyle; }
            set
            {
                if (value == nullStyle)
                    return;

                if (nullStyle != null)
                    UnwireStyleEvents(nullStyle);

                nullStyle = value;
                OnNullStyleChanged();

                if (nullStyle != null)
                    WireStyleEvents(nullStyle);
            }
        }

        /// <summary>
        /// Raises the <see cref="NullStyleChanged"/> event.
        /// </summary>
        protected void OnNullStyleChanged()
        {
            StyliseAllRegisteredTextBoxes();

            EventHandler handlers = (EventHandler)base.Events[NullStyleChanged_EventKey];
            if (handlers != null)
                handlers(this, EventArgs.Empty);
        }

        #endregion
        #endregion

        protected override TextBoxStyle GetRequiredStyle(IComparableValueProvider numericTextBox)
        {
            IComparable value = numericTextBox.Value;
            if (value == null)
                return nullStyle;

            return base.GetRequiredStyle(numericTextBox);
        }
    }

    /// <summary>
    /// A <see cref="NumericTextBoxStyliser"/> presented as a component.
    /// </summary>
    [Designer(typeof(NullableNumericTextBoxStyliserComponent.Designer))]
    public partial class NullableNumericTextBoxStyliserComponent : NullableNumericTextBoxStyliser, IComponent
    {
        #region Fields

        private readonly Component component = new Component();
        #endregion

        #region Constructors

        public NullableNumericTextBoxStyliserComponent()
        {
        }

        public NullableNumericTextBoxStyliserComponent(IContainer container)
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
