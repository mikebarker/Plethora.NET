using Plethora.Xaml.Converters;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Plethora.Xaml.Wpf.Converters
{
    /// <inheritdoc/>
    public class ConverterEntry : ConverterEntry<IValueConverter>
    {
    }

    /// <inheritdoc/>
    [ContentProperty(nameof(Entries))]
    public class ChainedConverter : ChainedConverterBase<IValueConverter, ValueConversionAttribute, ConverterEntry, CultureInfo>, IValueConverter
    {
        protected override object Convert(IValueConverter valueConverter, object value, Type targetType, object parameter, CultureInfo culture)
        {
            return valueConverter.Convert(value, targetType, parameter, culture);
        }

        protected override object ConvertBack(IValueConverter valueConverter, object value, Type targetType, object parameter, CultureInfo culture)
        {
            return valueConverter.ConvertBack(value, targetType, parameter, culture);
        }

        protected override Type GetSourceType(ValueConversionAttribute valueConversionAttribute) => valueConversionAttribute.SourceType;

        protected override Type GetTargetType(ValueConversionAttribute valueConversionAttribute) => valueConversionAttribute.TargetType;
    }
}
