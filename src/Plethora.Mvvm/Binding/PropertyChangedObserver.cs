using JetBrains.Annotations;
using System;
using System.ComponentModel;

namespace Plethora.Mvvm.Binding
{
    public class PropertyChangedObserver<T, TProperty> : BindingObserverElementBase<T, TProperty>
    {
        private readonly Func<T, TProperty> getFunc;
        private readonly string propertyName;

        public PropertyChangedObserver(
            [NotNull] string propertyName,
            [NotNull] Func<T, TProperty> getFunc)
        {
            if (getFunc == null)
                throw new ArgumentNullException(nameof(getFunc));

            this.getFunc = getFunc;
            this.propertyName = propertyName;
        }


        public override bool TryGetValue(out TProperty property)
        {
            if (this.Observed == null)
            {
                property = default(TProperty);
                return false;
            }

            property = getFunc(this.Observed);
            return true;
        }

        protected override void AddChangeListener()
        {
            if (this.Observed is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += ObservedPropertyChanged;
            }
        }

        protected override void RemoveChangeListener()
        {
            if (this.Observed is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= ObservedPropertyChanged;
            }
        }

        private void ObservedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, this.propertyName))
            {
                this.OnValueChanged();
            }
        }
    }
}
