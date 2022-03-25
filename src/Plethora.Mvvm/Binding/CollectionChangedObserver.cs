using JetBrains.Annotations;
using System;
using System.Collections.Specialized;

namespace Plethora.Mvvm.Binding
{
    public class CollectionChangedObserver<T, TValue> : BindingObserverElementBase<T, TValue>
    {
        private readonly int? index;
        private readonly Func<T, TValue> getFunc;

        public CollectionChangedObserver(
            [NotNull] string[] indexerArguments,
            [NotNull] Func<T, TValue> getFunc)
        {
            if (getFunc == null)
                throw new ArgumentNullException(nameof(getFunc));

            this.getFunc = getFunc;

            if (indexerArguments.Length == 1)
            {
                if (int.TryParse(indexerArguments[0], out int index))
                {
                    this.index = index;
                }
            }
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
            bool invokeValueChanged = false;
            if (this.index.HasValue)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        invokeValueChanged = (e.NewStartingIndex <= this.index);
                        break;

                    case NotifyCollectionChangedAction.Move:
                        invokeValueChanged =
                            (e.OldStartingIndex == this.index) ||
                            (e.NewStartingIndex == this.index) ||
                            ((e.OldStartingIndex < this.index) && (this.index < e.NewStartingIndex)) ||
                            ((e.NewStartingIndex < this.index) && (this.index < e.OldStartingIndex));
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        invokeValueChanged = (e.OldStartingIndex == this.index);
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        invokeValueChanged = (e.OldStartingIndex == this.index);
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        invokeValueChanged = true;
                        break;
                }
            }
            else
            {
                // Fallback to signal whenever the collection changes
                invokeValueChanged = true;
            }

            if (invokeValueChanged)
            {
                this.OnValueChanged();
            }
        }
    }
}
