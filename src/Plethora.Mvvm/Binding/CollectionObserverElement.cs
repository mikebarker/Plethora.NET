using JetBrains.Annotations;
using System;
using System.Collections.Specialized;

namespace Plethora.Mvvm.Binding
{
    /// <summary>
    /// A collection observer in a binding expression.
    /// </summary>
    public class CollectionObserverElement : BindingObserverElementBase
    {
        private readonly int? index;

        public CollectionObserverElement(
            [NotNull] IndexerBindingElementDefinition indexerDefinition,
            [NotNull] IGetterProvider getterProvider)
            : base(indexerDefinition, getterProvider)
        {
            if (indexerDefinition == null)
                throw new ArgumentNullException(nameof(indexerDefinition));

            if (indexerDefinition.Arguments.Length == 1)
            {
                if (int.TryParse(indexerDefinition.Arguments[0].Value, out int index))
                {
                    this.index = index;
                }
            }
        }

        protected override void AddChangeListener()
        {
            if (this.Observed is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += HandleObservedCollectionChanged;
            }
        }

        protected override void RemoveChangeListener()
        {
            if (this.Observed is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= HandleObservedCollectionChanged;
            }
        }

        private void HandleObservedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
