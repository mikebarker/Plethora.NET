using JetBrains.Annotations;
using System;
using System.Collections.Specialized;

namespace Plethora.Mvvm.Binding
{
    public class CollectionChangedObserver<T, TValue> : BindingObserverElementBase<T, TValue>
    {
        private readonly Func<T, TValue> getFunc;

        public CollectionChangedObserver(
            [NotNull] Func<T, TValue> getFunc)
        {
            if (getFunc == null)
                throw new ArgumentNullException(nameof(getFunc));

            this.getFunc = getFunc;
        }


        public override bool TryGetValue(out TValue value)
        {
            if (this.Observed == null)
            {
                value = default(TValue);
                return false;
            }

            value = getFunc(this.Observed);
            return true;
        }

        protected override void AddChangeListener()
        {
            if (this.Observed is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += ObservedCollectionChanged;
            }
        }

        protected override void RemoveChangeListener()
        {
            if (this.Observed is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= ObservedCollectionChanged;
            }
        }

        private void ObservedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    // TODO: Make this less chatty
                    this.OnValueChanged();
                    break;
            }
        }
    }
}
