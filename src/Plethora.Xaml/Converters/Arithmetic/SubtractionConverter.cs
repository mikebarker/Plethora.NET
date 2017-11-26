using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to subtract two numeric values.
    /// </summary>
    /// <remarks>
    /// Requires two numeric values.
    /// Returns (value[0] - value[1])
    /// </remarks>
    public class SubtractionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            object lvalue = values[0];
            object rvalue = values[1];

            try
            {
                object result = ArithmeticConverterHelper.ExecuteOperator(ArithmeticOperator.Subtruction, lvalue, rvalue);
                return result;
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }
            catch (InvalidOperationException)
            {
                return DependencyProperty.UnsetValue;
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }

        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
