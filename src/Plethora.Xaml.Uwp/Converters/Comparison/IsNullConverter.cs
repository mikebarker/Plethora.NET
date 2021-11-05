using System;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (ReferenceEquals(value, null))
                return true;

            return false;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
