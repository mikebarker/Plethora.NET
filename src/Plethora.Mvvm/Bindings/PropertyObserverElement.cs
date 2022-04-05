using JetBrains.Annotations;
using System;
using System.ComponentModel;

namespace Plethora.Mvvm.Bindings
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
            if (this.Observed is INotifyPropertyChanging notifyPropertyChanging)
            {
                notifyPropertyChanging.PropertyChanging += HandleObservedPropertyChanging;
            }

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

            if (this.Observed is INotifyPropertyChanging notifyPropertyChanging)
            {
                notifyPropertyChanging.PropertyChanging -= HandleObservedPropertyChanging;
            }
        }

        private void HandleObservedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, this.propertyDefinition.PropertyName))
            {
                this.OnValueChanged();
            }
        }

        private void HandleObservedPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (string.Equals(e.PropertyName, this.propertyDefinition.PropertyName))
            {
                this.OnValueChanging();
            }
        }
    }
}
