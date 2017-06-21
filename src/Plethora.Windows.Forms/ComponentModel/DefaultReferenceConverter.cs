using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Plethora.ComponentModel
{
    [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
    public class DefaultReferenceConverter : ReferenceConverter
    {
        #region Fields

        private static readonly string defaultString;
        private static readonly string noneString;

        private readonly bool allowInherit;
        private readonly Type type;
        private readonly PropertyInfo defaultProperty;
        private object defaultInstance;

        #endregion

        #region Constructors

        static DefaultReferenceConverter()
        {
            var refConverter = new ReferenceConverter(typeof(object));
            noneString = refConverter.ConvertToString(null);
            defaultString = "(default)";
        }

        public DefaultReferenceConverter(Type type, string defaultPropertyName, bool allowInherit)
            : base(type)
        {
            //Validation
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (defaultPropertyName == null)
                throw new ArgumentNullException(nameof(defaultPropertyName));


            this.allowInherit = allowInherit;
            this.type = type;

            //Find the first default property
            const BindingFlags defaultPropertyFlags = BindingFlags.Static | BindingFlags.Public;
            if (allowInherit)
            {
                var hierarchyType = type;
                while (hierarchyType != null)
                {
                    PropertyInfo property = type.GetProperty(
                        defaultPropertyName,
                        defaultPropertyFlags);

                    if (property != null)
                    {
                        this.defaultProperty = property;
                        break;
                    }

                    hierarchyType = type.BaseType;
                }
            }
            else
            {
                this.defaultProperty = type.GetProperty(
                    defaultPropertyName,
                    defaultPropertyFlags);
            }

            if (defaultProperty == null)
            {
                throw new ArgumentException(string.Format("Could not find public, static property {0} in type {1}.",
                    defaultPropertyName, type));
            }
        }
        #endregion

        #region Properties

        protected object DefaultInstance
        {
            get
            {
                if (this.defaultInstance == null)
                {
                    this.defaultInstance = defaultProperty.GetValue(null, null);
                }
                return this.defaultInstance;
            }
        }
        #endregion

        #region Overrides of ReferenceConverter

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string str = (string)value;
                if (string.IsNullOrEmpty(str) || string.Equals(str, noneString))
                    return null;
                if (string.Equals(str, defaultString))
                    return DefaultInstance;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }

            if (value.GetType() == destinationType)
            {
                return value;
            }

            bool isofType = allowInherit
                         ? type.IsInstanceOfType(value)
                         : type == value.GetType();

            if (isofType)
            {
                if (ReferenceEquals(value, DefaultInstance))
                {
                    if (destinationType == typeof(InstanceDescriptor))
                    {
                        return new InstanceDescriptor(defaultProperty, null);
                    }
                    if (destinationType == typeof(string))
                    {
                        return defaultString;
                    }
                }
                else
                {
                    if ((destinationType == typeof(InstanceDescriptor)))
                    {
                        return GetConstructorInstanceDescriptor(value);
                    }
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var collection = base.GetStandardValues(context);
            if (collection == null)
            {
                return new StandardValuesCollection(
                    new[] { noneString, defaultString });
            }

            var list = collection.Cast<object>().ToList();
            list.Insert(1, defaultString);
            return new StandardValuesCollection(list);
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Returns the <see cref="InstanceDescriptor"/> required to recreate the
        /// <paramref name="value"/> parameter.
        /// </summary>
        /// <param name="value">The instance for which the InstanceDescriptor is required.</param>
        /// <returns>
        /// The <see cref="InstanceDescriptor"/> for the <paramref name="value"/> parameter.
        /// </returns>
        /// <remarks>
        /// By default returns the an <see cref="InstanceDescriptor"/> calling the default constructor.
        /// </remarks>
        protected virtual InstanceDescriptor GetConstructorInstanceDescriptor(object value)
        {
            //Using the GetType (not this.type property) allows the converter to work with derived types
            Type valueType = value.GetType();
            ConstructorInfo ctorInfo = valueType.GetConstructor(new Type[0]);
            return new InstanceDescriptor(ctorInfo, null);
        }
        #endregion
    }
}
