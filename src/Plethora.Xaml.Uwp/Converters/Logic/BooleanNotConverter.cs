using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    /// <summary>
    /// A <see cref="IValueConverter"/> which acts to NOT a boolean values.
    /// </summary>
    /// <remarks>
    /// Requires a boolean value.
    /// Returns (!value)
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
