using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    [ValueConversion(typeof(IEnumerable), typeof(IEnumerable))]
    public class PrependListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
