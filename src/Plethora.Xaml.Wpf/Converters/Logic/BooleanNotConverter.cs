using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
