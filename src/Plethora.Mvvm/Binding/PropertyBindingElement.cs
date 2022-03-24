using JetBrains.Annotations;
using System;
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

        public override IBindingObserverElement<object, object> CreateObserver()
        {
            Func<object, object> getter = (obj) =>
            {
                var type = obj.GetType();
                var property = type.GetProperty(this.PropertyName);
                var value = property.GetValue(obj);

                return value;
            };

            return new PropertyChangedObserver<object, object>(this.PropertyName, getter);
        }

        public IBindingObserverElement<T, object> CreateObserver<T>()
        {
            var type = typeof(T);
            var property = type.GetProperty(this.PropertyName);

            // Compile the getter
            var parameterExp = Expression.Parameter(type);
            var propertyExp = Expression.Property(parameterExp, property);

            Expression bodyExp = propertyExp;
            if (property.PropertyType != typeof(object))
            {
                var conversionExp = Expression.Convert(propertyExp, typeof(object));
                bodyExp = conversionExp;
            }

            var lambda = Expression.Lambda<Func<T, object>>(propertyExp, parameterExp);

            Func<T, object> getter = getter = lambda.Compile();

            return new PropertyChangedObserver<T, object>(this.PropertyName, getter);
        }
    }
}
