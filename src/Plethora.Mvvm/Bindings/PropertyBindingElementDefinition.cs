using JetBrains.Annotations;
using System;
using System.Linq.Expressions;

namespace Plethora.Mvvm.Bindings
{
    /// <summary>
    /// Defines a single property element in a binding.
    /// </summary>
    public class PropertyBindingElementDefinition : BindingElementDefinition
    {
        public PropertyBindingElementDefinition(
            [NotNull] string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            this.PropertyName = propertyName;
        }

        [NotNull]
        public string PropertyName { get; }

        public override IBindingObserverElement CreateObserver(IGetterProvider getterProvider)
        {
            return new PropertyObserverElement(this, getterProvider);
        }

        protected override Expression CreateGetterExpression(Type observedType, Expression observedExpression)
        {
            var property = observedType.GetProperty(this.PropertyName);

            var getPropertyExpression = Expression.Property(observedExpression, property);
            return getPropertyExpression;
        }

        #region Equality

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            return this.Equals(obj as PropertyBindingElementDefinition);
        }

        public bool Equals(PropertyBindingElementDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(this.PropertyName, other.PropertyName);
        }

        public override int GetHashCode()
        {
            return this.PropertyName.GetHashCode();
        }

        #endregion
    }
}
