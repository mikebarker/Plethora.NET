using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Plethora.Mvvm.Binding
{
    public class IndexerArgument
    {
        public IndexerArgument(
            [NotNull] string value,
            [CanBeNull] Type type = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            this.Value = value;
            this.Type = type;
        }

        [NotNull] 
        public string Value { get; }
        
        [CanBeNull]
        public Type Type { get; }
    }

    public class IndexerBindingElement : BindingElement
    {
        public IndexerBindingElement(
            [NotNull, ItemNotNull] IndexerArgument[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            this.Arguments = arguments;
        }

        public IndexerArgument[] Arguments { get; }

        /// <summary>
        /// Creates a <see cref="CollectionChangedObserver{T, TProperty}"/> which represents the observer
        /// for this BindingElement.
        /// </summary>
        /// <returns>
        /// A <see cref="CollectionChangedObserver{T, TProperty}"/>.
        /// </returns>
        public override IBindingObserverElement CreateObserver(Type observedType)
        {
            var getter = CreateGetter(observedType, this.Arguments);

            string[] indexerArguments = this.Arguments
                .Select(arg => arg.Value)
                .ToArray();

            var propertyChangedObserver = CreateCollectionChangedObserver(indexerArguments, getter);
            var result = (IBindingObserverElement)propertyChangedObserver;
            return result;
        }

        /// <summary>
        /// Creates a <see cref="Func{T, TResult}"/> which will retrieve the value from an observed object.
        /// </summary>
        /// <returns>
        /// A <see cref="Func{T, TResult}"/>.
        /// </returns>
        private static object CreateGetter(Type observedType, IndexerArgument[] arguments)
        {
            var tuple = TryGetIndexerProperty(observedType, arguments);

            var indexerProperty = tuple.Item1;
            var argumentValues = tuple.Item2;

            Func<ParameterExpression, Expression> getIndexerPropertyExpressionFactory = (parameterExp) =>
                {
                    var argumentExps = argumentValues
                        .Select(value => Expression.Constant(value))
                        .ToArray();

                    var propertyExp = Expression.Property(parameterExp, indexerProperty, argumentExps);
                    return propertyExp;
                };

            var getter = BindingElement.CreateGetter(
                observedType,
                indexerProperty.PropertyType,
                getIndexerPropertyExpressionFactory);

            return getter;
        }

        /// <summary>
        /// Creates a <see cref="CollectionChangedObserver{T, TProperty}"/> which represents the observer
        /// for this BindingElement.
        /// </summary>
        /// <returns>
        /// A <see cref="CollectionChangedObserver{T, TProperty}"/>.
        /// </returns>
        private static object CreateCollectionChangedObserver(string[] indexerArguments, object getter)
        {
            // Call the ctor of CollectionChangedObserver<T, TValue>
            var genericArguments = getter.GetType().GetGenericArguments();

            var observedType = genericArguments[0];
            var propertyType = genericArguments[1];

            var ctor = typeof(CollectionChangedObserver<,>)
                .MakeGenericType(observedType, propertyType)
                .GetConstructors()
                .Single();

            var propertyChangedObserver = ctor.Invoke(new object[] { indexerArguments, getter });
            return propertyChangedObserver;
        }

        private static Tuple<PropertyInfo, object[]> TryGetIndexerProperty(Type type, IndexerArgument[] arguments)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var indexParameters = property.GetIndexParameters();
                if (AreMatch(indexParameters, arguments, out object[] argumentValues))
                {
                    return Tuple.Create(property, argumentValues);
                }
            }

            return null;
        }

        private static bool AreMatch(ParameterInfo[] indexParameters, IndexerArgument[] arguments, out object[] argumentValues)
        {
            if (indexParameters.Length != arguments.Length)
            {
                argumentValues = null;
                return false;
            }

            argumentValues = new object[indexParameters.Length];
            for (int i = 0; i < indexParameters.Length; i++)
            {
                var indexParameter = indexParameters[i];
                var argument = arguments[i];

                if (!AreMatch(indexParameter, argument, out argumentValues[i]))
                {
                    argumentValues = null;
                    return false;
                }
            }

            return true;
        }

        private static bool AreMatch(ParameterInfo indexParameter, IndexerArgument argument, out object value)
        {
            if (argument.Type != null)
            {
                if (indexParameter.ParameterType.IsAssignableFrom(argument.Type))
                {
                    var converter = TypeDescriptor.GetConverter(argument.Type);
                    value = converter.ConvertFromInvariantString(argument.Value);
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            else
            {
                try
                {
                    var converter = TypeDescriptor.GetConverter(indexParameter.ParameterType);
                    value = converter.ConvertFromInvariantString(argument.Value);
                    return true;
                }
                catch
                {
                    value = null;
                    return false;
                }
            }
        }
    }
}
