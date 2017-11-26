using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which represents the conditional operator (t ? x : y)
    /// </summary>
    /// <remarks>
    /// Requires three values. The first must be a boolean.
    /// Returns (value[0] ? value[1] : value[2])
    /// </remarks>
    public class ConditionalConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 3)
                return DependencyProperty.UnsetValue;

            if (!(values[0] is bool))
                return DependencyProperty.UnsetValue;

            bool condition = (bool)values[0];

            object result = condition
                ? values[1]
                : values[2];

            return result;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
