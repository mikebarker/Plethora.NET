using System;
using System.Globalization;
using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, null))
                return true;

            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
