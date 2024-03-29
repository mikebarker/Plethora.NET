﻿using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Plethora.Mvvm.Bindings
{
    /// <summary>
    /// Defines a single indexer element in a binding.
    /// </summary>
    public class IndexerBindingElementDefinition : BindingElementDefinition
    {
        public readonly struct Argument
        {
            public Argument(
                string value,
                Type? type = null)
            {
                ArgumentNullException.ThrowIfNull(value);

                this.Value = value;
                this.Type = type;
            }

            public string Value { get; }
        
            public Type? Type { get; }

            #region Equality

            public override bool Equals(object? obj)
            {
                if (obj is null) return false;

                if (obj is not Argument other)
                    return false;

                return this.Equals(other);
            }

            public readonly bool Equals(Argument other)
            {
                return
                    string.Equals(this.Value, other.Value) &&
                    Type.Equals(this.Type, other.Type);
            }

            public override readonly int GetHashCode()
            {
                return HashCodeHelper.GetHashCode(this.Value, this.Type);
            }

            #endregion
        }

        public IndexerBindingElementDefinition(
            Argument[] arguments)
        {
            ArgumentNullException.ThrowIfNull(arguments);

            this.Arguments = arguments;
        }

        public Argument[] Arguments { get; }

        public override IBindingObserverElement CreateObserver(IGetterProvider getterProvider)
        {
            return new CollectionObserverElement(this, getterProvider);
        }

        protected override Expression CreateGetterExpression(Type observedType, Expression observedExpression)
        {
            var tuple = TryGetIndexerProperty(observedType, this.Arguments);
            if (tuple is null)
            {
                throw new InvalidOperationException($"Indexer not found on type '{observedType}'.");
            }

            var indexerProperty = tuple.Item1;
            var argumentValues = tuple.Item2;

            var argumentExps = argumentValues
                    .Select(value => Expression.Constant(value))
                    .ToArray();

            var getPropertyExpression = Expression.Property(observedExpression, indexerProperty, argumentExps);
            return getPropertyExpression;
        }

        private static Tuple<PropertyInfo, object?[]>? TryGetIndexerProperty(Type type, Argument[] arguments)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var indexParameters = property.GetIndexParameters();
                if (AreMatch(indexParameters, arguments, out var argumentValues))
                {
                    return Tuple.Create(property, argumentValues);
                }
            }

            return null;
        }

        private static bool AreMatch(ParameterInfo[] indexParameters, Argument[] arguments, [MaybeNullWhen(false)] out object?[] argumentValues)
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

        private static bool AreMatch(ParameterInfo indexParameter, Argument argument, out object? value)
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


        #region Equality

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return this.Equals(obj as IndexerBindingElementDefinition);
        }

        public bool Equals(IndexerBindingElementDefinition? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Enumerable.SequenceEqual(this.Arguments, other.Arguments);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetEnumerableHashCode(this.Arguments);
        }

        #endregion
    }
}
