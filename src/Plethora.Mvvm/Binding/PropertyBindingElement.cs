using JetBrains.Annotations;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Plethora.Mvvm.Binding
{
    public class PropertyBindingElement : BindingElement
    {
        public PropertyBindingElement(
            [NotNull] string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            this.PropertyName = propertyName;
        }

        [NotNull]
        public string PropertyName { get; }

        /// <summary>
        /// Creates a <see cref="PropertyChangedObserver{T, TProperty}"/> which represents the observer
        /// for this BindingElement.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyChangedObserver{T, TProperty}"/>.
        /// </returns>
        public override IBindingObserverElement CreateObserver(Type observedType)
        {
            var getter = CreateGetter(observedType, this.PropertyName);

            var propertyChangedObserver = CreatePropertyChangedObserver(this.PropertyName, getter);
            var result = (IBindingObserverElement)propertyChangedObserver;
            return result;
        }

        /// <summary>
        /// Creates a <see cref="Func{T, TResult}"/> which will retrieve the value from an observed object.
        /// </summary>
        /// <returns>
        /// A <see cref="Func{T, TResult}"/>.
        /// </returns>
        private static object CreateGetter(Type observedType, string propertyName)
        {
            var property = observedType.GetProperty(propertyName);

            Func<ParameterExpression, Expression> getPropertyExpressionFactory = (parameterExp) =>
                {
                    var propertyExp = Expression.Property(parameterExp, property);
                    return propertyExp;
                };

            var getter = CreateGetter(
                observedType,
                property.PropertyType,
                getPropertyExpressionFactory);

            return getter;
        }

        /// <summary>
        /// Creates a <see cref="PropertyChangedObserver{T, TProperty}"/> which represents the observer
        /// for this BindingElement.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyChangedObserver{T, TProperty}"/>.
        /// </returns>
        private static object CreatePropertyChangedObserver(string propertyName, object getter)
        {
            // Call the ctor of PropertyChangedObserver<T, TValue>
            var genericArguments = getter.GetType().GetGenericArguments();

            var observedType = genericArguments[0];
            var propertyType = genericArguments[1];

            var ctor = typeof(PropertyChangedObserver<,>)
                .MakeGenericType(observedType, propertyType)
                .GetConstructors()
                .Single();

            var propertyChangedObserver = ctor.Invoke(new object[] { propertyName, getter });
            return propertyChangedObserver;
        }
    }
}
