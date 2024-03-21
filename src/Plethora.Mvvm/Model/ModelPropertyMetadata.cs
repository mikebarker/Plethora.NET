using System;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Non-generic interface used for storing <see cref="ModelPropertyMetadata{T}"/>.
    /// </summary>
    internal interface IModelPropertyMetadata
    {
        string Name { get; }
    }

    /// <summary>
    /// Generic implementation of <see cref="IModelPropertyMetadata"/>
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the property of this meta-data.
    /// </typeparam>
    internal sealed class ModelPropertyMetadata<T> : IModelPropertyMetadata
    {
        private readonly string name;
        private readonly T? defaultValue;

        public ModelPropertyMetadata(
            string name,
            T? defaultValue)
        {
            ArgumentNullException.ThrowIfNull(name);

            this.name = name;
            this.defaultValue = defaultValue;
        }

        public string Name
        {
            get { return this.name; }
        }

        public T? DefaultValue
        {
            get { return this.defaultValue; }
        }
    }

    internal static class ModelPropertyMetadataHelper
    {
        public static T? GetDefaultValueSafe<T>(this ModelPropertyMetadata<T>? propertyMetadata)
        {
            if (propertyMetadata == null)
                return default;

            return propertyMetadata.DefaultValue;
        }
    }
}
