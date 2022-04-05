using JetBrains.Annotations;
using System;

namespace Plethora.Mvvm.Binding
{
    /// <summary>
    /// A single element in a binding expression, which can be chained to a parent to form complex bindings.
    /// </summary>
    public abstract class BindingObserverElementBase : IBindingObserverElement
    {
        private readonly BindingElementDefinition bindingElementDefinition;
        private readonly IGetterProvider getterProvider;
        private IBindingObserver parent;
        private object observed;
        private Func<object, object> getter;

        protected BindingObserverElementBase(
            [NotNull] BindingElementDefinition bindingElementDefinition,
            [NotNull] IGetterProvider getterProvider)
        {
            if (bindingElementDefinition == null)
                throw new ArgumentNullException(nameof(bindingElementDefinition));

            if (getterProvider == null)
                throw new ArgumentNullException(nameof(getterProvider));

            this.bindingElementDefinition = bindingElementDefinition;
            this.getterProvider = getterProvider;
        }

        protected object Observed => this.observed;

        private Func<object, object> Getter
        {
            get
            {
                if (this.getter == null)
                {
                    this.getter = this.getterProvider.AcquireGetter(this.Observed.GetType(), this.bindingElementDefinition);
                }

                return this.getter;
            }
        }

        #region ValueChanging Event

        public event EventHandler ValueChanging;

        protected virtual void OnValueChanging()
        {
            this.ValueChanging?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region ValueChanged Event

        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public bool TryGetValue(out object value)
        {
            if (ReferenceEquals(this.Observed, null))
            {
                value = null;
                return false;
            }

            value = this.Getter(this.Observed);
            return true;
        }

        public void SetParent(IBindingObserver parent)
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

            if (this.parent != null)
            {
                this.parent.ValueChanged += HandleParentValueChanged;

                this.HandleParentValueChanged();
            }
            else
            {
                this.SetObserved(null);
            }
        }

        private void HandleParentValueChanging(object sender, EventArgs e)
        {
            this.HandleParentValueChanging();
        }

        private void HandleParentValueChanging()
        {
            this.OnValueChanging();
        }

        private void HandleParentValueChanged(object sender, EventArgs e)
        {
            this.HandleParentValueChanged();
        }

        private void HandleParentValueChanged()
        {
            if (this.parent.TryGetValue(out object parentProperty))
            {
                this.SetObserved(parentProperty);
            }
            else
            {
                this.SetObserved(null);
            }
        }

        public void SetObserved([CanBeNull] object observed)
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
