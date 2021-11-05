using System;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsEqualConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool result = Equals(value, parameter);
            return result;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
