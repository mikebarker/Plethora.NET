using System;

using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// An <see cref="Attribute"/> which can be applied to properties to signify that they are
    /// dependent on another property.
    /// </summary>
    /// <see cref="DependentNotifyPropertyChangedImpl"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DependsOnAttribute : Attribute
    {
        private readonly string dependsOnPropertyName;
        private readonly bool skipValidation;

        /// <summary>
        /// Initialises a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="dependsOnPropertyName">The name of the dependent property.</param>
        /// <param name="skipValidation">
        /// Determines whether the <see cref="DependsOnPropertyName"/> should be validated as being a valid
        /// instance property of the type containing the usage.
        /// </param>
        public DependsOnAttribute(
            [NotNull] string dependsOnPropertyName,
            bool skipValidation = false)
        {
            if (dependsOnPropertyName == null)
                throw new ArgumentNullException("dependsOnPropertyName");

            this.dependsOnPropertyName = dependsOnPropertyName;
            this.skipValidation = skipValidation;
        }

        /// <summary>
        /// The name of the dependent property.
        /// </summary>
        [NotNull]
        public string DependsOnPropertyName
        {
            get { return this.dependsOnPropertyName; }
        }

        /// <summary>
        /// Determines whether the <see cref="DependsOnPropertyName"/> should be validated as being a valid
        /// instance property of the type containing the usage.
        /// </summary>
        /// <remarks>
        /// Validation is only executed when the assembly is compiled with the compilation symbol "DEBUG" defined.
        /// </remarks>
        public bool SkipValidation
        {
            get { return this.skipValidation; }
        }
    }
}
