using System;
using JetBrains.Annotations;

namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// An <see cref="Attribute"/> which denotes a static member from which to retrieve
    /// the default value of a <see cref="ModelBase"/> property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The static member must be public, and may be either a field or a property.
    /// </para>
    /// <para>
    /// The attribute is utilised as follows:
    /// <example><code><![CDATA[
    ///     public sealed class Person : ModelBase
    ///     {
    ///         [DefaultValueProvider(typeof(CountryProvider), "UnitedKingdom")]
    ///         public Country CountryOfResidence
    ///         {
    ///             get { return this.GetValue(() => this.CountryOfResidence); }
    ///             set { this.SetValue(() => this.CountryOfResidence, value); }
    ///         }
    ///     }
    /// 
    ///     private static class CountryProvider
    ///     {
    ///         private static readonly Country UnitedKingdom = new Country("GB", "United Kingdom");
    ///     }
    /// ]]></code></example>
    /// </para>
    /// </remarks>
    /// <see cref="ModelBase"/>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DefaultValueProviderAttribute : Attribute
    {
        private readonly Type type;
        private readonly string staticMemberName;

        /// <summary>
        /// Initialise a new instance of the <see cref="DefaultValueProviderAttribute"/> class.
        /// </summary>
        /// <param name="type">
        /// The type from which the default value must be retrieved.
        /// </param>
        /// <param name="staticMemberName">
        /// The static field or property name for which the default value must be retrieved from <paramref name="type"/>.
        /// </param>
        public DefaultValueProviderAttribute(
            [NotNull] Type type,
            [NotNull] string staticMemberName)
        {
            this.type = type;
            this.staticMemberName = staticMemberName;
        }

        /// <summary>
        /// The type from which the default value must be retrieved.
        /// </summary>
        [NotNull]
        public Type Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// The static field or property name for which the default value must be retrieved from <see name="DefaultValueProviderAttribute.Type"/>.
        /// </summary>
        [NotNull]
        public string StaticMemberName
        {
            get { return this.staticMemberName; }
        }
    }
}
