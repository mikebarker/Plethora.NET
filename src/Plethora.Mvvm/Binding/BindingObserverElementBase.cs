using JetBrains.Annotations;
using System;

namespace Plethora.Mvvm.Binding
{
    public abstract class BindingObserverElementBase<T, TValue> : IBindingObserverElement<T, TValue>
    {
        private IBindingObserver<T> parent;
        private T observed;

        #region PropertyChanged Event

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public abstract bool TryGetValue(out TValue value);

        public void SetParent(IBindingObserver<T> parent)
        {
            if (ReferenceEquals(this.parent, parent))
            {
                return;
            }

            if (this.parent != null)
            {
                this.parent.ValueChanged -= ParentValueChanged;
            }

            this.parent = parent;

            if (this.parent != null)
            {
                this.parent.ValueChanged += ParentValueChanged;

                if (this.parent.TryGetValue(out T value))
                {
                    this.SetObserved(value);
                }
            }
        }

        private void ParentValueChanged(object sender, EventArgs e)
        {
            if (this.parent.TryGetValue(out T parentProperty))
            {
                this.SetObserved(parentProperty);
            }
            else
            {
                this.SetObserved(default(T));
            }
        }

        public void SetObserved([CanBeNull] T observed)
        {
            if (ReferenceEquals(this.observed, observed))
            {
                return;
            }

            this.RemoveChangeListener();

            this.observed = observed;

            this.AddChangeListener();

            this.OnValueChanged();
        }

        protected T Observed => this.observed;

        protected abstract void AddChangeListener();
        protected abstract void RemoveChangeListener();
    }
}
