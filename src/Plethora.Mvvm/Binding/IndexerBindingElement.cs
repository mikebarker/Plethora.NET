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

        public override IBindingObserverElement<object, object> CreateObserver()
        {
            Func<object, object> getter = (obj) =>
            {
                var type = obj.GetType();
                var tuple = TryGetIndexerProperty(type, this.Arguments);
                if (tuple != null)
                {
                    var indexerProperty = tuple.Item1;
                    var argumentValues = tuple.Item2;
                    var result = indexerProperty.GetValue(obj, argumentValues);
                    return result;
                }

                throw new InvalidOperationException();
            };


            return new CollectionChangedObserver<object, object>(getter);
        }

        public IBindingObserverElement<T, object> CreateObserver<T>()
        {
            var type = typeof(T);
            var tuple = TryGetIndexerProperty(type, this.Arguments);
            if (tuple != null)
            {
                var indexerProperty = tuple.Item1;
                var argumentValues = tuple.Item2;
    
                // Compile the getter
                var parameterExp = Expression.Parameter(type);

                var argumentExps = argumentValues
                    .Select(value => Expression.Constant(value))
                    .ToArray();

                var propertyExp = Expression.Property(parameterExp, indexerProperty, argumentExps);
                Expression bodyExp = propertyExp;
    
                if (indexerProperty.PropertyType != typeof(object))
                {
                    var conversionExp = Expression.Convert(propertyExp, typeof(object));
                    bodyExp = conversionExp;
                }

                var lambda = Expression.Lambda<Func<T, object>>(propertyExp, parameterExp);

                Func<T, object> getter = getter = lambda.Compile();

                return new CollectionChangedObserver<T, object>(getter);
            }

            throw new InvalidOperationException();
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
