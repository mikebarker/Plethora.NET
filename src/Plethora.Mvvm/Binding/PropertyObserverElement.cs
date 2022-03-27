using JetBrains.Annotations;
using System;
using System.ComponentModel;

namespace Plethora.Mvvm.Binding
{
    /// <summary>
    /// A property observer in a binding expression.
    /// </summary>
    public class PropertyObserverElement : BindingObserverElementBase
    {
        private readonly PropertyBindingElementDefinition propertyDefinition;

        public PropertyObserverElement(
            [NotNull] PropertyBindingElementDefinition propertyDefinition,
            [NotNull] IGetterProvider getterProvider)
            : base(propertyDefinition, getterProvider)
        {
            if (propertyDefinition == null)
                throw new ArgumentNullException(nameof(propertyDefinition));

            this.propertyDefinition = propertyDefinition;
        }


        protected override void AddChangeListener()
        {
            if (this.Observed is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += HandleObservedPropertyChanged;
            }
        }

        protected override void RemoveChangeListener()
        {
            if (this.Observed is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= HandleObservedPropertyChanged;
            }
        }

        private void HandleObservedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, this.propertyDefinition.PropertyName))
            {
                this.OnValueChanged();
            }
        }
    }
}
