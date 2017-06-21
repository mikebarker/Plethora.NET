using System;
using System.Collections.Generic;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Non-generic interface used for storing <see cref="ModelProperty{T}"/>.
    /// </summary>
    internal interface IModelProperty : ITrackChanges
    {
        bool IsUnchangedDefaultValue(IModelPropertyMetadata propertyMetadata);
    }

    /// <summary>
    /// Generic implementation of <see cref="IModelPropertyMetadata"/>
    /// </summary>
    /// <typeparam name="T">
    /// The data type of this property.
    /// </typeparam>
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    internal sealed class ModelProperty<T> : IModelProperty
    {
        private T originalValue;
        private T currentValue;

        public ModelProperty([CanBeNull] T value)
        {
            this.originalValue = value;
            this.currentValue = value;
        }

        [CanBeNull]
        public T OriginalValue
        {
            get { return this.originalValue; }
            set { this.originalValue = value; }
        }

        [CanBeNull]
        public T Value
        {
            get { return this.currentValue; }
            set { this.currentValue = value; }
        }

        public bool HasChanged
        {
            get
            {
                return !EqualityComparer<T>.Default.Equals(this.OriginalValue, this.Value);
            }
        }

        public void Commit()
        {
            this.originalValue = this.currentValue;
        }

        public void Rollback()
        {
            this.currentValue = this.originalValue;
        }

        bool IModelProperty.IsUnchangedDefaultValue([CanBeNull] IModelPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata != null && !(propertyMetadata is ModelPropertyMetadata<T>))
            {
                throw new ArgumentException(
                    string.Format(nameof(propertyMetadata) + " is not of the expected type {0}", typeof(ModelPropertyMetadata<T>)),
                    nameof(propertyMetadata));
            }

            return this.IsUnchangedDefaultValue((ModelPropertyMetadata<T>)propertyMetadata);
        }

        public bool IsUnchangedDefaultValue([CanBeNull] ModelPropertyMetadata<T> propertyMetadata)
        {
            T defaultValue = ModelPropertyMetadataHelper.GetDefaultValueSafe(propertyMetadata);

            return
                EqualityComparer<T>.Default.Equals(this.originalValue, defaultValue) &&
                EqualityComparer<T>.Default.Equals(this.currentValue, defaultValue);
        }
    }
}
