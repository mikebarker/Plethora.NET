using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to multiply two numeric values.
    /// </summary>
    /// <remarks>
    /// Requires two numeric values.
    /// Returns (value[0] * value[1])
    /// </remarks>
    public class MultiplicationConverter : IMultiValueConverter
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
                ArithmeticConverter.ConvertForOperation(ref lvalue, ref rvalue);
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }

            if (lvalue is int)
            {
                int result = ((int)lvalue) * ((int)rvalue);
                return result;
            }
            if (lvalue is uint)
            {
                uint result = ((uint)lvalue) * ((uint)rvalue);
                return result;
            }
            if (lvalue is long)
            {
                long result = ((long)lvalue) * ((long)rvalue);
                return result;
            }
            if (lvalue is ulong)
            {
                ulong result = ((ulong)lvalue) * ((ulong)rvalue);
                return result;
            }
            if (lvalue is float)
            {
                float result = ((float)lvalue) * ((float)rvalue);
                return result;
            }
            if (lvalue is double)
            {
                double result = ((double)lvalue) * ((double)rvalue);
                return result;
            }
            if (lvalue is decimal)
            {
                decimal result = ((decimal)lvalue) * ((decimal)rvalue);
                return result;
            }

            return DependencyProperty.UnsetValue;
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
