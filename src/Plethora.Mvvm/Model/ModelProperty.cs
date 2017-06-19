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
        bool IsUnchangedDefaultValue(IModelPropertyMetaData propertyMetaData);
    }

    /// <summary>
    /// Generic implementation of <see cref="IModelPropertyMetaData"/>
    /// </summary>
    /// <typeparam name="T">
    /// The data type of this property.
    /// </typeparam>
    [DebuggerDisplay("{Value}")]
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

        bool IModelProperty.IsUnchangedDefaultValue([CanBeNull] IModelPropertyMetaData propertyMetaData)
        {
            if (propertyMetaData != null && !(propertyMetaData is ModelPropertyMetaData<T>))
            {
                throw new ArgumentException(
                    string.Format("propertyMetaData is not of the expected type {0}", typeof(ModelPropertyMetaData<T>)),
                    "propertyMetaData");
            }

            return this.IsUnchangedDefaultValue((ModelPropertyMetaData<T>)propertyMetaData);
        }

        public bool IsUnchangedDefaultValue([CanBeNull] ModelPropertyMetaData<T> propertyMetaData)
        {
            T defaultValue = ModelPropertyMetaDataHelper.GetDefaultValueSafe(propertyMetaData);

            return
                EqualityComparer<T>.Default.Equals(this.originalValue, defaultValue) &&
                EqualityComparer<T>.Default.Equals(this.currentValue, defaultValue);
        }
    }
}
