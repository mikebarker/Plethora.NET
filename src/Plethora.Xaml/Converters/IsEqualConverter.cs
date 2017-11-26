using System;
using System.Globalization;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Equals(value, parameter))
                return true;

            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
