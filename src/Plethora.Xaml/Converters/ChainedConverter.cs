using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// An entry in the <see cref="ChainedConverter"/>.
    /// </summary>
    public class ConverterEntry
    {
        /// <summary>
        /// This value may be specified as the <see cref="ConverterParameter"/> property
        /// to allow the <see cref="ChainedConverter"/>'s parameter to be passed to the <see cref="Converter"/>.
        /// </summary>
        public static object GroupParameter { get; } = new object();

        public IValueConverter Converter { get; set; }

        public object ConverterParameter { get; set; }
    }

    /// <summary>
    /// A converter which nests multiple converters together into a chain.
    /// </summary>
    /// <remarks>
    /// The chained converters are called passing the value from the first to the last entry in <see cref="Entries"/>. During calls
    /// to the nested converters the <see cref="ConverterEntry.ConverterParameter"/> will be passed as the parameter. 
    /// Specifying <see cref="ConverterEntry.GroupParameter"/> as the <see cref="ConverterEntry.ConverterParameter"/> will cause
    /// the parameter passed into the <see cref="ChainedConverter.Convert"/> method to be passed to the nested converter.
    /// </remarks>
    /// <example>
    /// The following example demonstrates how the <see cref="ChainedConverter"/> may be used:
    /// <code><![CDATA[
    ///     <Window.Resources>
    ///         <system:Int32 x:Key="EqualityValue">128</system:Int32>
    ///         
    ///         <converters:IsEqualConverter x:Key="isEqualConverter" />
    ///         <converters:BooleanNotConverter x:Key="booleanNotConverter" />
    ///         <BooleanToVisibilityConverter x:Key="booleanToVisibilityConverter" />
    ///
    ///         <converters:ChainedConverter x:Key="isNotEqualToVisibilityConverter" >
    ///             <converters:ConverterEntry Converter="{StaticResource isEqualConverter}" ConverterParameter="{x:Static converters:ConverterEntry.GroupParameter}" />
    ///             <converters:ConverterEntry Converter="{StaticResource booleanNotConverter}" />
    ///             <converters:ConverterEntry Converter="{StaticResource booleanToVisibilityConverter}" />
    ///         </converters:ChainedConverter>
    ///     </Window.Resources>
    ///     
    ///     ...
    /// 
    ///     <!-- This textbox will be invisible if Value does not equal 128 -->
    ///     <TextBox Visibility="{Binding Value, Converter={StaticResource isNotEqualToVisibilityConverter}, ConverterParameter={StaticResource EqualityValue}}" />
    /// ]]></code>
    /// </example>
    [ContentProperty(nameof(Entries))]
    public class ChainedConverter : IValueConverter
    {
        private readonly List<ConverterEntry> entries = new List<ConverterEntry>();

        /// <summary>
        /// The list of chained converters.
        /// </summary>
        public List<ConverterEntry> Entries
        {
            get { return this.entries; }
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                ConverterEntry converterEntry = this.Entries[i];
                IValueConverter valueConverter = converterEntry.Converter;

                Type nextSourceType = typeof(object);
                if (i < this.Entries.Count - 1)
                {
                    ConverterEntry nextConverterEntry = this.Entries[i + 1];
                    IValueConverter nextConverter = nextConverterEntry.Converter;
                    object[] objValueConversionAttributes = nextConverter.GetType().GetCustomAttributes(typeof(ValueConversionAttribute), true);
                    if (objValueConversionAttributes.Length  == 1)
                    {
                        ValueConversionAttribute valueConversionAttribute = (ValueConversionAttribute)objValueConversionAttributes[0];
                        if (valueConversionAttribute.SourceType != null)
                        {
                            nextSourceType = valueConversionAttribute.SourceType;
                        }
                    }
                }

                object currentParameter = converterEntry.ConverterParameter;
                if (ReferenceEquals(currentParameter, ConverterEntry.GroupParameter))
                {
                    currentParameter = parameter;
                }

                value = valueConverter.Convert(value, nextSourceType, currentParameter, culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = this.Entries.Count - 1; i >= 0; i--)
            {
                ConverterEntry converterEntry = this.Entries[i];
                IValueConverter valueConverter = converterEntry.Converter;

                Type prevTargetType = typeof(object);
                if (i != 0)
                {
                    ConverterEntry prevConverterEntry = this.Entries[i + 1];
                    IValueConverter prevConverter = prevConverterEntry.Converter;
                    object[] objValueConversionAttributes = prevConverter.GetType().GetCustomAttributes(typeof(ValueConversionAttribute), true);
                    if (objValueConversionAttributes.Length == 1)
                    {
                        ValueConversionAttribute valueConversionAttribute = (ValueConversionAttribute)objValueConversionAttributes[0];
                        if (valueConversionAttribute.TargetType != null)
                        {
                            prevTargetType = valueConversionAttribute.TargetType;
                        }
                    }
                }

                object currentParameter = converterEntry.ConverterParameter;
                if (ReferenceEquals(currentParameter, ConverterEntry.GroupParameter))
                {
                    currentParameter = parameter;
                }

                value = valueConverter.ConvertBack(value, prevTargetType, currentParameter, culture);
            }

            return value;
        }
    }
}
