using System.ComponentModel;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Provides data for the <see cref="INotifyPropertyChanged.PropertyChanged"/> event
    /// where the event is raised as a result of another property changing.
    /// </summary>
    public class DependentPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        private readonly string originPropertyName;

        /// <summary>
        /// Initialise a new instance of the <see cref="DependentPropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property whose value is changing.</param>
        /// <param name="originPropertyName">The name of the property which changed causing this property change.</param>
        public DependentPropertyChangedEventArgs([CanBeNull] string propertyName, [CanBeNull] string originPropertyName)
            : base(propertyName)
        {
            this.originPropertyName = originPropertyName;
        }

        /// <summary>
        /// The name of the property which changed causing this property change.
        /// </summary>
        [CanBeNull]
        public string OriginPropertyName
        {
            get { return this.originPropertyName; }
        }
    }
}
