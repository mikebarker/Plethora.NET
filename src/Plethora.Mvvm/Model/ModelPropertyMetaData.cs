using System;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Non-generic interface used for storing <see cref="ModelPropertyMetaData{T}"/>.
    /// </summary>
    internal interface IModelPropertyMetaData
    {
        [NotNull]
        string Name { get; }
    }

    /// <summary>
    /// Generic implementation of <see cref="IModelPropertyMetaData"/>
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the property of this meta-data.
    /// </typeparam>
    internal sealed class ModelPropertyMetaData<T> : IModelPropertyMetaData
    {
        private readonly string name;
        private readonly T defaultValue;

        public ModelPropertyMetaData(
            [NotNull] string name,
            T defaultValue)
        {
            if (name == null)
                throw new ArgumentNullException("name");

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
            get { return defaultValue; }
        }
    }

    internal static class ModelPropertyMetaDataHelper
    {
        [CanBeNull]
        public static T GetDefaultValueSafe<T>([CanBeNull] this ModelPropertyMetaData<T> propertyMetaData)
        {
            if (propertyMetaData == null)
                return default(T);

            return propertyMetaData.DefaultValue;
        }
    }
}
