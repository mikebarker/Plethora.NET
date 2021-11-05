using System;
using System.Collections.Generic;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// This value may be specified as the <see cref="ConverterParameter"/> property
    /// to allow the <see cref="ChainedConverter"/>'s parameter to be passed to the <see cref="Converter"/>.
    /// </summary>
    public class GroupParameter
    {
    }

    /// <summary>
    /// An entry in the <see cref="ChainedConverter"/>.
    /// </summary>
    public abstract class ConverterEntry<TValueConverter>
    {
        public TValueConverter Converter { get; set; }

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
    ///         <converters:GroupParameter x:Key="GroupParameter" />
    ///         
    ///         <converters:IsEqualConverter x:Key="isEqualConverter" />
    ///         <converters:BooleanNotConverter x:Key="booleanNotConverter" />
    ///         <BooleanToVisibilityConverter x:Key="booleanToVisibilityConverter" />
    ///
    ///         <converters:ChainedConverter x:Key="isNotEqualToVisibilityConverter" >
    ///             <converters:ConverterEntry Converter="{StaticResource isEqualConverter}" ConverterParameter="{StaticResource GroupParameter}" />
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
    public abstract class ChainedConverterBase<TValueConverter, TValueConversionAttribute, TConverterEntry, TGlobalization>
        where TConverterEntry : ConverterEntry<TValueConverter>
    {
        private readonly List<TConverterEntry> entries = new List<TConverterEntry>();

        /// <summary>
        /// The list of chained converters.
        /// </summary>
        public List<TConverterEntry> Entries
        {
            get { return this.entries; }
        }


        public object Convert(object value, Type targetType, object parameter, TGlobalization globalization)
        {
            for (int i = 0; i < this.Entries.Count; i++)
            {
                ConverterEntry<TValueConverter> converterEntry = this.Entries[i];
                TValueConverter valueConverter = converterEntry.Converter;

                Type nextSourceType = typeof(object);
                if (i < this.Entries.Count - 1)
                {
                    ConverterEntry<TValueConverter> nextConverterEntry = this.Entries[i + 1];
                    TValueConverter nextConverter = nextConverterEntry.Converter;
                    object[] objValueConversionAttributes = nextConverter.GetType().GetCustomAttributes(typeof(TValueConversionAttribute), true);
                    if (objValueConversionAttributes.Length == 1)
                    {
                        TValueConversionAttribute valueConversionAttribute = (TValueConversionAttribute)objValueConversionAttributes[0];
                        Type attributeSourceType = GetSourceType(valueConversionAttribute);
                        if (attributeSourceType != null)
                        {
                            nextSourceType = attributeSourceType;
                        }
                    }
                }

                object currentParameter = converterEntry.ConverterParameter;
                if (currentParameter is GroupParameter)
                {
                    currentParameter = parameter;
                }

                value = Convert(valueConverter, value, nextSourceType, currentParameter, globalization);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, TGlobalization globalization)
        {
            for (int i = this.Entries.Count - 1; i >= 0; i--)
            {
                ConverterEntry<TValueConverter> converterEntry = this.Entries[i];
                TValueConverter valueConverter = converterEntry.Converter;

                Type prevTargetType = typeof(object);
                if (i != 0)
                {
                    ConverterEntry<TValueConverter> prevConverterEntry = this.Entries[i + 1];
                    TValueConverter prevConverter = prevConverterEntry.Converter;
                    object[] objValueConversionAttributes = prevConverter.GetType().GetCustomAttributes(typeof(TValueConversionAttribute), true);
                    if (objValueConversionAttributes.Length == 1)
                    {
                        TValueConversionAttribute valueConversionAttribute = (TValueConversionAttribute)objValueConversionAttributes[0];
                        var attributeTargetType = GetTargetType(valueConversionAttribute);
                        if (attributeTargetType != null)
                        {
                            prevTargetType = attributeTargetType;
                        }
                    }
                }

                object currentParameter = converterEntry.ConverterParameter;
                if (currentParameter is GroupParameter)
                {
                    currentParameter = parameter;
                }

                value = ConvertBack(valueConverter, value, prevTargetType, currentParameter, globalization);
            }

            return value;
        }

        protected abstract object Convert(TValueConverter valueConverter, object value, Type targetType, object parameter, TGlobalization globalization);

        protected abstract object ConvertBack(TValueConverter valueConverter, object value, Type targetType, object parameter, TGlobalization globalization);

        protected abstract Type GetSourceType(TValueConversionAttribute valueConversionAttribute);

        protected abstract Type GetTargetType(TValueConversionAttribute valueConversionAttribute);
    }
}
