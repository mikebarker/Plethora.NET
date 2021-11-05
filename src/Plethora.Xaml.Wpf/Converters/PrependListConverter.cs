using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    [ValueConversion(typeof(IEnumerable), typeof(IEnumerable))]
    public class PrependListConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<object> enumerableValue;
            if (value is IEnumerable<object>)
                enumerableValue = ((IEnumerable<object>)value);
            else if (value is IEnumerable)
                enumerableValue = ((IEnumerable)value).Cast<object>();
            else
                return DependencyProperty.UnsetValue;

            IEnumerable<object> enumerableParameter;
            if (parameter is IEnumerable<object>)
                enumerableParameter = ((IEnumerable<object>)parameter);
            else if (parameter is string)   // Special case: string is also an IEnumerable (of char)
                enumerableParameter = new[] { parameter };
            else if (parameter is IEnumerable)
                enumerableParameter = ((IEnumerable)parameter).Cast<object>();
            else
                enumerableParameter = new[] {parameter};

            return enumerableParameter.Concat(enumerableValue);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 0)
                return DependencyProperty.UnsetValue;

            object lastValue = values[values.Length - 1];

            IEnumerable<object> enumerableLastValue;
            if (lastValue is IEnumerable<object>)
                enumerableLastValue = ((IEnumerable<object>)lastValue);
            else if (lastValue is IEnumerable)
                enumerableLastValue = ((IEnumerable)lastValue).Cast<object>();
            else
                return DependencyProperty.UnsetValue;


            IEnumerable<object> prependEnumerable = values
                .Take(values.Length - 1);

            return prependEnumerable.Concat(enumerableLastValue);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
