﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

using Plethora.Collections;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Base class for a class change tracking property changes, allowing rollback and commit.
    /// </summary>
    public abstract class ModelBase : DependentNotifyPropertyChanged, INotifyPropertyChanged, INotifyPropertyChanging, ITrackChanges
    {
        #region Static Members

        private static readonly ReaderWriterLockSlim propertyMetadataLock = new();
        private static readonly MruDictionary<Type, IReadOnlyDictionary<string, IModelPropertyMetadata>> propertyMetadataByType = new(maxEntries: 1024);

        /// <summary>
        /// Gets and sets a tuning parameter which determines the maximum number of types for which the property meta-data map
        /// will cache data.
        /// </summary>
        /// <remarks>
        /// If there is a high volatility of the types access via this class, and the cache often drops elements, consider increasing
        /// the maximum entries limit (at the cost of additional memory).
        /// </remarks>
        public static int PropertyMetadataTypeMapMaxEntries
        {
            get { return propertyMetadataByType.MaxEntries; }
            set { propertyMetadataByType.SetMaxEntriesAndWatermark(value, null); }
        }

        private static IReadOnlyDictionary<string, IModelPropertyMetadata>? GetPropertyMetadataForType(Type type)
        {
            IReadOnlyDictionary<string, IModelPropertyMetadata>? metadata;
            bool result;

            propertyMetadataLock.EnterReadLock();
            try
            {
                result = propertyMetadataByType.TryGetValue(type, out metadata);
            }
            finally
            {
                propertyMetadataLock.ExitReadLock();
            }

            if (!result)
            {
                metadata = CreatePropertyMetadataForType(type);

                propertyMetadataLock.EnterWriteLock();
                try
                {
                    propertyMetadataByType[type] = metadata;
                }
                finally
                {
                    propertyMetadataLock.ExitWriteLock();
                }
            }

            return metadata;
        }

        private static IReadOnlyDictionary<string, IModelPropertyMetadata> CreatePropertyMetadataForType(Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, IModelPropertyMetadata> metadata = new();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string propertyName = propertyInfo.Name;

                object? defaultValue = null;

                object[] attributes = propertyInfo.GetCustomAttributes(true);
                foreach (Attribute attribute in attributes)
                {
                    if (attribute is DefaultValueAttribute)
                    {
                        defaultValue = ((DefaultValueAttribute)attribute).Value;
                        ValidateDefaultValue(defaultValue, propertyInfo);
                        break;
                    }
                    else if (attribute is DefaultValueProviderAttribute)
                    {
                        Type providerType = ((DefaultValueProviderAttribute)attribute).Type;
                        string staticMemberName = ((DefaultValueProviderAttribute)attribute).StaticMemberName;

                        FieldInfo? providerFieldInfo = providerType.GetField(staticMemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                        if (providerFieldInfo != null)
                        {
                            defaultValue = providerFieldInfo.GetValue(null);
                            ValidateDefaultValue(defaultValue, propertyInfo);
                            break;
                        }
                        else
                        {
                            PropertyInfo? providerPropertyInfo = providerType.GetProperty(staticMemberName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                            if (providerPropertyInfo != null)
                            {
                                defaultValue = providerPropertyInfo.GetValue(null);
                                ValidateDefaultValue(defaultValue, propertyInfo);
                                break;
                            }
                            else
                            {
                                throw new ArgumentException(ResourceProvider.CantFindStaticMember(staticMemberName, providerType));
                            }
                        }
                    }
                }

                if (defaultValue != null)
                {
                    Type modelPropertyMetadataType = typeof(ModelPropertyMetadata<>).MakeGenericType(propertyInfo.PropertyType);
                    IModelPropertyMetadata? modelPropertyMetadata = (IModelPropertyMetadata?)Activator.CreateInstance(modelPropertyMetadataType,
                        propertyName,
                        defaultValue);

                    if (modelPropertyMetadata is not null)
                    {
                        metadata.Add(propertyName, modelPropertyMetadata);
                    }
                }
            }

            return metadata;
        }

        [Conditional("DEBUG")]
        private static void ValidateDefaultValue(object? defaultValue, PropertyInfo propertyInfo)
        {
            if (defaultValue == null)
            {
                if (!propertyInfo.PropertyType.IsNullable())
                {
                    throw new ArgumentException(ResourceProvider.DefaultPropertyValueNotOfType(
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        null));
                }
            }
            else if (!propertyInfo.PropertyType.IsInstanceOfType(defaultValue))
            {
                throw new ArgumentException(ResourceProvider.DefaultPropertyValueNotOfType(
                    propertyInfo.Name,
                    propertyInfo.PropertyType,
                    defaultValue));
            }
        }

        public static object? CreateDefaultValueOfType(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        #endregion

        private readonly Dictionary<string, IModelProperty> modelPropertiesByName = new();
        private bool hasChanged = false;


        #region PropertyChanging Event

        /// <summary>
        /// Raised when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging
        {
            add { base.InternalPropertyChanging += value; }
            remove { base.InternalPropertyChanging -= value; }
        }

        #endregion

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The data type of the property.</typeparam>
        /// <param name="propertyName">The name of the property for which the value is to be retrieved.</param>
        /// <returns>
        /// The value of the property.
        /// </returns>
        /// <remarks>
        /// Properties should be implemented using a syntax similar to:
        ///  <example>
        ///  <code><![CDATA[
        ///     public string Name
        ///     {
        ///         get { return this.GetValue(); }
        ///         set { this.SetValue(value); }
        ///     }
        ///  ]]></code>
        ///  </example>
        /// </remarks>
        /// <seealso cref="SetValue{T}"/>
        public T? GetValue<T>([CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            ModelProperty<T>? modelProperty;
            if (!this.TryGetModelProperty(propertyName, out modelProperty))
            {
                T? defaultValue = this.GetDefaultValue<T>(propertyName);
                return defaultValue;
            }

            return modelProperty.Value;
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The data type of the property.</typeparam>
        /// <param name="propertyName">The name of the property for which the value is to be set.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>
        /// true if the value of the property was changed; otherwise false.
        /// </returns>
        /// <remarks>
        /// Properties should be implemented using a syntax similar to:
        ///  <example>
        ///  <code><![CDATA[
        ///     public string Name
        ///     {
        ///         get { return this.GetValue(); }
        ///         set { this.SetValue(value); }
        ///     }
        ///  ]]></code>
        ///  </example>
        /// </remarks>
        /// <seealso cref="GetValue{T}"/>
        public bool SetValue<T>(T? value, [CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            ModelProperty<T>? modelProperty;
            if (!this.TryGetModelProperty(propertyName, out modelProperty))
            {
                T? defaultValue = this.GetDefaultValue<T>(propertyName);

                if (EqualityComparer<T>.Default.Equals(value, defaultValue))
                {
                    return false;
                }
                else
                {
                    modelProperty = new ModelProperty<T>(defaultValue);
                    this.modelPropertiesByName[propertyName] = modelProperty;
                }
            }

            if (EqualityComparer<T>.Default.Equals(modelProperty.Value, value))
            {
                return false;
            }
            else
            {
                bool raiseHasChanged = false;

                this.OnPropertyChanging(propertyName);
                modelProperty.Value = value;

                if (modelProperty.HasChanged)
                {
                    if (this.hasChanged != true)
                    {
                        this.OnPropertyChanging(new DependentPropertyChangingEventArgs(nameof(this.HasChanged), propertyName));
                        this.hasChanged = true;
                        raiseHasChanged = true;
                    }
                }
                else
                {
                    if (this.hasChanged)
                    {
                        bool newHasChanged = this.modelPropertiesByName.Values.Any(mp => mp.HasChanged);
                        if (this.hasChanged != newHasChanged)
                        {
                            this.OnPropertyChanging(new DependentPropertyChangingEventArgs(nameof(this.HasChanged), propertyName));
                            this.hasChanged = newHasChanged;
                            raiseHasChanged = true;
                        }
                    }
                }
                this.OnPropertyChanged(propertyName);

                if(raiseHasChanged)
                    this.OnPropertyChanged(new DependentPropertyChangedEventArgs(nameof(this.HasChanged), propertyName));

                return true;
            }
        }

        /// <summary>
        /// Gets the original value of a property.
        /// </summary>
        /// <typeparam name="T">The data type of the property.</typeparam>
        /// <param name="propertyName">The name of the property for which the original value is to be retrieved.</param>
        /// <returns>
        /// The value of the property.
        /// </returns>
        /// <seealso cref="SetOriginalValue{T}"/>
        public T? GetOriginalValue<T>([CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            ModelProperty<T>? modelProperty;
            if (!this.TryGetModelProperty(propertyName, out modelProperty))
            {
                T? defaultValue = this.GetDefaultValue<T>(propertyName);
                return defaultValue;
            }

            return modelProperty.OriginalValue;
        }

        /// <summary>
        /// Sets the original value of a property.
        /// </summary>
        /// <typeparam name="T">The data type of the property.</typeparam>
        /// <param name="propertyName">The name of the property for which the original value is to be set.</param>
        /// <param name="originalValue">The original value of the property.</param>
        /// <returns>
        /// true if the original value of the property was changed; otherwise false.
        /// </returns>
        /// <remarks>
        /// Setting the original value will also set the current value.
        /// </remarks>
        /// <seealso cref="GetOriginalValue{T}"/>
        public bool SetOriginalValue<T>(T? originalValue, [CallerMemberName] string? propertyName = null)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            T? defaultValue = this.GetDefaultValue<T>(propertyName);

            ModelProperty<T>? modelProperty;
            if (!this.TryGetModelProperty(propertyName, out modelProperty))
            {
                if (EqualityComparer<T>.Default.Equals(originalValue, defaultValue))
                {
                    return false;
                }
                else
                {
                    this.OnPropertyChanging(propertyName);

                    modelProperty = new ModelProperty<T>(originalValue);
                    this.modelPropertiesByName[propertyName] = modelProperty;
                }
            }
            else
            { 
                if (EqualityComparer<T>.Default.Equals(modelProperty.OriginalValue, originalValue) &&
                    EqualityComparer<T>.Default.Equals(modelProperty.Value, originalValue))
                {
                    return false;
                }
                else if (EqualityComparer<T>.Default.Equals(originalValue, defaultValue))
                {
                    this.OnPropertyChanging(propertyName);

                    this.modelPropertiesByName.Remove(propertyName);
                }
                else
                {
                    this.OnPropertyChanging(propertyName);

                    modelProperty.OriginalValue = originalValue;
                    modelProperty.Value = modelProperty.OriginalValue;
                }
            }

            bool raiseHasChanged = false;
            if (this.hasChanged)
            {
                bool newHasChanged = this.modelPropertiesByName.Values.Any(mp => mp.HasChanged);
                if (this.hasChanged != newHasChanged)
                {
                    this.OnPropertyChanging(new DependentPropertyChangingEventArgs(nameof(this.HasChanged), propertyName));
                    this.hasChanged = newHasChanged;
                    raiseHasChanged = true;
                }
            }

            this.OnPropertyChanged(propertyName);
            if (raiseHasChanged)
                this.OnPropertyChanged(new DependentPropertyChangedEventArgs(nameof(this.HasChanged), propertyName));

            return true;
        }


        /// <summary>
        /// Gets a flag indicating whether any of the properties of this instance have changed.
        /// </summary>
        /// <value>
        /// true if any of the properties of this instance have changed; otherwise false.
        /// </value>
        public virtual bool HasChanged
        {
            get { return this.hasChanged; }
        }

        /// <summary>
        /// Sets all properties original value to their current value.
        /// </summary>
        public void Commit()
        {
            foreach (var pair in this.modelPropertiesByName)
            {
                IModelProperty modelProperty = pair.Value;

                if (modelProperty.HasChanged)
                {
                    modelProperty.Commit();
                }
            }

            this.CompressModelProperties();

            if (this.hasChanged)
            {
                this.OnPropertyChanging(new DependentPropertyChangingEventArgs(nameof(this.HasChanged), null));
                this.hasChanged = false;
                this.OnPropertyChanged(new DependentPropertyChangedEventArgs(nameof(this.HasChanged), null));
            }
        }

        /// <summary>
        /// Sets all properties to their original value.
        /// </summary>
        public void Rollback()
        {
            List<string> changedPropertyNames = new List<string>();

            foreach (var pair in this.modelPropertiesByName)
            {
                string propertyName = pair.Key;
                IModelProperty modelProperty = pair.Value;

                if (modelProperty.HasChanged)
                {
                    this.OnPropertyChanging(propertyName);
                    modelProperty.Rollback();
                    changedPropertyNames.Add(propertyName);
                }
            }

            this.CompressModelProperties();

            if (this.hasChanged)
            {
                this.OnPropertyChanging(new DependentPropertyChangingEventArgs(nameof(this.HasChanged), null));
                this.hasChanged = false;
                changedPropertyNames.Add(nameof(this.HasChanged));
            }

            foreach (string propertyName in changedPropertyNames)
            {
                this.OnPropertyChanged(propertyName);
            }
        }


        private void CompressModelProperties()
        {
            List<string> defaultProperties = this.modelPropertiesByName
                .Where(pair => this.IsDefault(pair.Key, pair.Value))
                .Select(pair => pair.Key)
                .ToList();

            foreach (string propertyName in defaultProperties)
            {
                this.modelPropertiesByName.Remove(propertyName);
            }
        }

        private bool IsDefault(string propertyName, IModelProperty modelProperty)
        {
            var metadata = this.GetMetadata(propertyName);

            return modelProperty.IsUnchangedDefaultValue(metadata);
        }

        private T? GetDefaultValue<T>(string propertyName)
        {
            var metadata = this.GetMetadata(propertyName);

            var defaultValue = ((ModelPropertyMetadata<T>?)metadata).GetDefaultValueSafe();
            return defaultValue;
        }

        private IModelPropertyMetadata? GetMetadata(string propertyName)
        {
            IReadOnlyDictionary<string, IModelPropertyMetadata>? typeMetadata = ModelBase.GetPropertyMetadataForType(this.GetType());

            IModelPropertyMetadata? propertyMetadata = null;
            if (typeMetadata is not null)
            {
                typeMetadata.TryGetValue(propertyName, out propertyMetadata);
            }

            return propertyMetadata;
        }

        private bool TryGetModelProperty<T>(string propertyName, [MaybeNullWhen(false)] out ModelProperty<T> modelProperty)
        {
            if (!this.modelPropertiesByName.TryGetValue(propertyName, out var iModelProperty))
            {
                modelProperty = null;
                return false;
            }

            modelProperty = (ModelProperty<T>)iModelProperty;
            return true;
        }
    }
}
