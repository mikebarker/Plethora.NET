using System;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Non-generic interface used for storing <see cref="ModelPropertyMetadata{T}"/>.
    /// </summary>
    internal interface IModelPropertyMetadata
    {
        [NotNull]
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
        private readonly T defaultValue;

        public ModelPropertyMetadata(
            [NotNull] string name,
            T defaultValue)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            this.name = name;
            this.defaultValue = defaultValue;
        }

        public string Name
        {
            get { return this.name; }
        }

        [CanBeNull]
        public T DefaultValue
        {
            get { return this.defaultValue; }
        }
    }

    internal static class ModelPropertyMetadataHelper
    {
        [CanBeNull]
        public static T GetDefaultValueSafe<T>([CanBeNull] this ModelPropertyMetadata<T> propertyMetadata)
        {
            if (propertyMetadata == null)
                return default(T);

            return propertyMetadata.DefaultValue;
        }
    }
}
