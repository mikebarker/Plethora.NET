using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsEqualConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = Equals(value, parameter);
            return result;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            if (parameter is IEqualityComparer)
            {
                bool result = ((IEqualityComparer)parameter).Equals(values[0], values[1]);
                return result;
            }
            else if (parameter is IComparer)
            {
                int result = ((IComparer)parameter).Compare(values[0], values[1]);
                return (result == 0);
            }
            else
            {
                bool result = object.Equals(values[0], values[1]);
                return result;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        object[] IMultiValueConverter.ConvertBack(object values, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
