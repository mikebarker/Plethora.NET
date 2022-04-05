using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

using JetBrains.Annotations;

using Plethora.Collections;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Base class which allows for the properties to use the <see cref="DependsOnAttribute"/> to
    /// denote a property for which the value is dependent on another property. When the <see cref="INotifyPropertyChanged.PropertyChanged"/>
    /// event is raised for the independent property, it is also raised for the dependent property.
    /// </summary>
    /// <remarks>
    /// The dependent property is defined as follows:
    /// <example><code><![CDATA[
    ///     public sealed class Person : DependentNotifyPropertyChange
    ///     {
    ///         private DateTime dateOfBirth;
    /// 
    ///         public DateTime DateOfBirth
    ///         {
    ///             get { return this.dateOfBirth; }
    ///             set
    ///             {
    ///                 if (this.dateOfBirth == value)
    ///                     return;
    /// 
    ///                 this.dateOfBirth = value;
    ///                 this.OnPropertyChanged("DateOfBirth");
    ///             }
    ///         }
    ///
    ///         [DependsOn(nameof(DateOfBirth))]
    ///         public DateSpan Age
    ///         {
    ///             get
    ///             {
    ///                 DateSpan age = DateTime.Today.SubtractDateSpan(this.DateOfBirth);
    ///                 return age;
    ///             }
    ///         }
    ///
    ///         [DependsOn(nameof(Age))]
    ///         public int YearsToCentenary
    ///         {
    ///             get
    ///             {
    ///                 return int.Max(100 - this.Age.Years, 0);
    ///             }
    ///         }
    ///     }
    /// ]]></code></example>
    /// 
    /// In this example, when the DateOfBirth property changes the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is raised for DateOfBirth, Age and YearsToCentenary.
    /// </remarks>
    public abstract class DependentNotifyPropertyChanged : NotifyPropertyChanged
    {
        #region Static Members

        private class BindingDefinition
        {
            public BindingDefinition(
                [NotNull, ItemNotNull] IEnumerable<Binding.BindingElementDefinition> elements)
            {
                if (elements == null)
                    throw new ArgumentNullException(nameof(elements));

                this.Elements = elements.ToList();
            }

            public IReadOnlyList<Binding.BindingElementDefinition> Elements { get; }
        }

        private static readonly ReaderWriterLockSlim dependencyMapLock = new ReaderWriterLockSlim();
        private static readonly MruDictionary<Type, IReadOnlyDictionary<string, IReadOnlyCollection<BindingDefinition>>> dependencyMapByType =
            new MruDictionary<Type, IReadOnlyDictionary<string, IReadOnlyCollection<BindingDefinition>>>(maxEntries: 1024);

        /// <summary>
        /// Gets and sets a tuning parameter which determines the maximum number of types for which the dependency map
        /// will cache data.
        /// </summary>
        /// <remarks>
        /// If there is a high volatility of the types access via this class, and the cache often drops elements, consider increasing
        /// the maximum entries limit (at the cost of additional memory).
        /// </remarks>
        public static int DependencyTypeMapMaxEntries
        {
            get { return dependencyMapByType.MaxEntries; }
            set { dependencyMapByType.SetMaxEntriesAndWatermark(value, null); }
        }

        [CanBeNull]
        private static IReadOnlyDictionary<string, IReadOnlyCollection<BindingDefinition>> GetDependencyMapForType([NotNull] Type type)
        {
            IReadOnlyDictionary<string, IReadOnlyCollection<BindingDefinition>> map;
            bool result;

            dependencyMapLock.EnterReadLock();
            try
            {
                result = dependencyMapByType.TryGetValue(type, out map);
            }
            finally
            {
                dependencyMapLock.ExitReadLock();
            }

            if (!result)
            {
                map = CreateDependencyMapForType(type);

                dependencyMapLock.EnterWriteLock();
                try
                {
                    if (map.Count == 0)
                        dependencyMapByType[type] = null;
                    else
                        dependencyMapByType[type] = map;
                }
                finally
                {
                    dependencyMapLock.ExitWriteLock();
                }
            }

            return map;
        }

        [NotNull]
        private static IReadOnlyDictionary<string, IReadOnlyCollection<BindingDefinition>> CreateDependencyMapForType([NotNull] Type type)
        {
            var bindingDefinitionsMap = new Dictionary<string, IReadOnlyCollection<BindingDefinition>>();

            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(true);
                IEnumerable<DependsOnAttribute> dependsOnAttributes = customAttributes.OfType<DependsOnAttribute>();

                string propertyName = propertyInfo.Name;
                foreach (DependsOnAttribute dependsOnAttribute in dependsOnAttributes)
                {
                    string dependsOnPath = dependsOnAttribute.Path;

                    var bindingElementDefinitions = Binding.Binding.Parse(dependsOnPath);

                    if (!bindingDefinitionsMap.TryGetValue(propertyName, out var list))
                    {
                        list = new List<BindingDefinition>();
                        bindingDefinitionsMap.Add(propertyName, list);
                    }
                    ((List<BindingDefinition>)list).Add(new BindingDefinition(bindingElementDefinitions));
                }
            }

            return bindingDefinitionsMap;
        }

        #endregion

        private readonly List<Binding.IBindingObserver> bindingObservers;

        /// <summary>
        /// Initialises a new instance of the <see cref="DependentNotifyPropertyChanged"/> class.
        /// </summary>
        protected DependentNotifyPropertyChanged()
        {
            var map = GetDependencyMapForType(this.GetType());
            foreach (var pair in map)
            {
                var propertyName = pair.Key;
                var bindingDefinitions = pair.Value;

                foreach (var bindingDefinition in bindingDefinitions)
                {
                    var observer = Binding.Binding.CreateObserver(this, bindingDefinition.Elements);

                    observer.ValueChanging += (sender, e) => { this.OnPropertyChanging(new DependentPropertyChangingEventArgs(propertyName, null)); };
                    observer.ValueChanged += (sender, e) => { this.OnPropertyChanged(new DependentPropertyChangedEventArgs(propertyName, null)); };

                    if (this.bindingObservers == null)
                    {
                        this.bindingObservers = new List<Binding.IBindingObserver>();
                    }
                    this.bindingObservers.Add(observer);
                }
            }
        }
    }
}
