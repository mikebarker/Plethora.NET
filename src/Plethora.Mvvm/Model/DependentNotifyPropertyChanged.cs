using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    ///         [DependsOn("DateOfBirth")]
    ///         public DateSpan Age
    ///         {
    ///             get
    ///             {
    ///                 DateSpan age = DateTime.Today.SubtractDateSpan(this.DateOfBirth);
    ///                 return age;
    ///             }
    ///         }
    ///
    ///         [DependsOn("Age")]
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

        private static readonly ReaderWriterLockSlim dependencyMapLock = new ReaderWriterLockSlim();
        private static readonly MruDictionary<Type, IReadOnlyDictionary<string, IReadOnlyCollection<string>>> dependencyMapByType =
            new MruDictionary<Type, IReadOnlyDictionary<string, IReadOnlyCollection<string>>>(maxEntries: 1024);

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
        private static IReadOnlyDictionary<string, IReadOnlyCollection<string>> GetDependencyMapForType([NotNull] Type type)
        {
            IReadOnlyDictionary<string, IReadOnlyCollection<string>> map;
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
        private static IReadOnlyDictionary<string, IReadOnlyCollection<string>> CreateDependencyMapForType([NotNull] Type type)
        {
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            Dictionary<string, List<string>> forwardMap = new Dictionary<string, List<string>>();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object[] customAttributes = propertyInfo.GetCustomAttributes(true);
                IEnumerable<DependsOnAttribute> dependsOnAttributes = customAttributes.OfType<DependsOnAttribute>();

                string propertyName = propertyInfo.Name;
                foreach (DependsOnAttribute dependsOnAttribute in dependsOnAttributes)
                {
                    string dependsOnPropertyName = dependsOnAttribute.DependsOnPropertyName;

                    if (!dependsOnAttribute.SkipValidation)
                    {
                        ValidateProperty(type, dependsOnPropertyName);
                    }

                    List<string> list;
                    if (!forwardMap.TryGetValue(dependsOnPropertyName, out list))
                    {
                        list = new List<string>();
                        forwardMap[dependsOnPropertyName] = list;
                    }
                    list.Add(propertyName);
                }
            }


            Dictionary<string, IReadOnlyCollection<string>> dependencyMap = new Dictionary<string, IReadOnlyCollection<string>>();

            foreach (string propertyName in forwardMap.Keys)
            {
                HashSet<string> set = new HashSet<string>();
                PopulateDependencySet(set, forwardMap, propertyName);

                dependencyMap.Add(propertyName, new ReadOnlyCollectionWrapper<string>(set));
            }

            return dependencyMap;
        }

        private static void PopulateDependencySet(
            [NotNull] HashSet<string> set,
            [NotNull] Dictionary<string, List<string>> forwardMap,
            [NotNull] string propertyName)
        {
            List<string> list;
            if (forwardMap.TryGetValue(propertyName, out list))
            {
                foreach (string name in list)
                {
                    bool added = set.Add(name);
                    if (added)
                    {
                        PopulateDependencySet(set, forwardMap, name);
                    }
                }
            }
        }

        [Conditional("DEBUG")]
        private static void ValidateProperty(Type type, string propertyName)
        {
            PropertyInfo propertyInfo = type.GetProperty(
                propertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propertyInfo == null)
                throw new ArgumentException(string.Format("The property '{0}' is not an instance property of the type {1}.", propertyName, type.Name));
        }

        #endregion

        [CanBeNull]
        private readonly IReadOnlyDictionary<string, IReadOnlyCollection<string>> dependencyMap;

        /// <summary>
        /// Initialises a new instance of the <see cref="DependentNotifyPropertyChanged"/> class.
        /// </summary>
        protected DependentNotifyPropertyChanged()
        {
            this.dependencyMap = GetDependencyMapForType(this.GetType());
        }

        /// <summary>
        /// Raises the <see cref="NotifyPropertyChanged.InternalPropertyChanging"/> event.
        /// </summary>
        /// <remarks>
        /// If other properties have the <see cref="DependsOnAttribute"/> defined, specifying <paramref name="e.PropertyName"/>
        /// as their dependent property name then the <see cref="NotifyPropertyChanged.InternalPropertyChanging"/> event is
        /// also raised for these properties.
        /// </remarks>
        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            base.OnPropertyChanging(e);

            if (this.IsNotifying)
            {
                if (this.dependencyMap != null)
                {
                    IReadOnlyCollection<string> list;
                    if (this.dependencyMap.TryGetValue(e.PropertyName, out list))
                    {
                        foreach (string propertyName in list)
                        {
                            base.OnPropertyChanging(new DependentPropertyChangingEventArgs(propertyName, e.PropertyName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="INotifyPropertyChanged.PropertyChanged"/> event.
        /// </summary>
        /// <remarks>
        /// If other properties have the <see cref="DependsOnAttribute"/> defined, specifying <paramref name="e.PropertyName"/>
        /// as their dependent property name then the <see cref="INotifyPropertyChanged.PropertyChanged"/> event is
        /// also raised for these properties.
        /// </remarks>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (this.IsNotifying)
            { 
                if (this.dependencyMap != null)
                {
                    IReadOnlyCollection<string> list;
                    if (this.dependencyMap.TryGetValue(e.PropertyName, out list))
                    {
                        foreach (string propertyName in list)
                        {
                            base.OnPropertyChanged(new DependentPropertyChangedEventArgs(propertyName, e.PropertyName));
                        }
                    }
                }
            }
        }
    }
}
