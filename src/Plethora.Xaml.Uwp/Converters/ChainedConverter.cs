using Plethora.Xaml.Converters;
using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace Plethora.Xaml.Uwp.Converters
{
    /// <inheritdoc/>
    public class ConverterEntry : ConverterEntry<IValueConverter>
    {
    }

    /// <inheritdoc/>
    [ContentProperty(Name = nameof(Entries))]
    public class ChainedConverter : ChainedConverterBase<IValueConverter, ValueConversionAttribute, ConverterEntry, string>, IValueConverter
    {
        protected override object Convert(IValueConverter valueConverter, object value, Type targetType, object parameter, string language)
        {
            return valueConverter.Convert(value, targetType, parameter, language);
        }

        protected override object ConvertBack(IValueConverter valueConverter, object value, Type targetType, object parameter, string language)
        {
            return valueConverter.ConvertBack(value, targetType, parameter, language);
        }

        protected override Type GetSourceType(ValueConversionAttribute valueConversionAttribute) => valueConversionAttribute.SourceType;

        protected override Type GetTargetType(ValueConversionAttribute valueConversionAttribute) => valueConversionAttribute.TargetType;
    }
}
