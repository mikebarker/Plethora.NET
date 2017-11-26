using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to AND two boolean values.
    /// </summary>
    /// <remarks>
    /// Requires two boolean values.
    /// Returns (value[0] & value[1])
    /// </remarks>
    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            if (!(values[0] is bool))
                return DependencyProperty.UnsetValue;

            if (!(values[1] is bool))
                return DependencyProperty.UnsetValue;

            bool result = BooleanLogicConverter.And((bool)values[0], (bool)values[1]);
            return result;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
