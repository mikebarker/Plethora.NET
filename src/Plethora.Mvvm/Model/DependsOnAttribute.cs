using System;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// An <see cref="Attribute"/> which can be applied to properties to signify that they are
    /// dependent on another property.
    /// </summary>
    /// <see cref="DependentNotifyPropertyChanged"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class DependsOnAttribute : Attribute
    {
        private readonly string path;

        /// <summary>
        /// Initialises a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="path">The path of the dependent property.</param>
        public DependsOnAttribute(
            [NotNull] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            this.path = path;
        }

        /// <summary>
        /// The path of the dependent property.
        /// </summary>
        [NotNull]
        public string Path
        {
            get { return this.path; }
        }
    }
}
