using System;
using System.Diagnostics.CodeAnalysis;

namespace Plethora.Mvvm.Bindings
{
    /// <summary>
    /// A single element in a binding expression, which can be chained to a parent to form complex bindings.
    /// </summary>
    public abstract class BindingObserverElementBase : IBindingObserverElement
    {
        private readonly BindingElementDefinition bindingElementDefinition;
        private readonly IGetterProvider getterProvider;
        private IBindingObserver? parent;
        private object? observed;
        private Func<object, object>? getter;

        protected BindingObserverElementBase(
            BindingElementDefinition bindingElementDefinition,
            IGetterProvider getterProvider)
        {
            ArgumentNullException.ThrowIfNull(bindingElementDefinition);
            ArgumentNullException.ThrowIfNull(getterProvider);

            this.bindingElementDefinition = bindingElementDefinition;
            this.getterProvider = getterProvider;
        }

        protected object? Observed => this.observed;

        private Func<object, object> Getter
        {
            get
            {
                if (this.getter is null)
                {
                    if (this.Observed is null)
                    {
                        throw new InvalidOperationException("Observed object is null.");
                    }

                    this.getter = this.getterProvider.AcquireGetter(this.Observed!.GetType(), this.bindingElementDefinition);
                }

                return this.getter;
            }
        }

        #region ValueChanging Event

        public event EventHandler? ValueChanging;

        protected virtual void OnValueChanging()
        {
            this.ValueChanging?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ValueChanged Event

        public event EventHandler? ValueChanged;

        protected virtual void OnValueChanged()
        {
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public bool TryGetValue([MaybeNullWhen(false)] out object value)
        {
            if (this.Observed is null)
            {
                value = null;
                return false;
            }

            value = this.Getter(this.Observed);
            return true;
        }

        public void SetParent(IBindingObserver? parent)
        {
            if (ReferenceEquals(this.parent, parent))
            {
                return;
            }

            if (this.parent != null)
            {
                this.parent.ValueChanged -= HandleParentValueChanged;
            }

            this.parent = parent;

            if (this.parent is not null)
            {
                this.parent.ValueChanged += HandleParentValueChanged;

                this.HandleParentValueChanged();
            }
            else
            {
                this.SetObserved(null);
            }
        }

        private void HandleParentValueChanging(object? sender, EventArgs e)
        {
            this.HandleParentValueChanging();
        }

        private void HandleParentValueChanging()
        {
            this.OnValueChanging();
        }

        private void HandleParentValueChanged(object? sender, EventArgs e)
        {
            this.HandleParentValueChanged();
        }

        private void HandleParentValueChanged()
        {
            if (this.parent!.TryGetValue(out var parentProperty))
            {
                this.SetObserved(parentProperty);
            }
            else
            {
                this.SetObserved(null);
            }
        }

        public void SetObserved(object? observed)
        {
            if (ReferenceEquals(this.observed, observed))
            {
                return;
            }

            this.RemoveChangeListener();

            this.OnValueChanging();

            this.observed = observed;
            this.getter = null;

            this.AddChangeListener();

            this.OnValueChanged();
        }

        protected abstract void AddChangeListener();
        protected abstract void RemoveChangeListener();
    }
}
