using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to apply an arithmetic operation to two values.
    /// </summary>
    public abstract class ArithmeticConverterBase : IMultiValueConverter
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
                ArithmeticConverterHelper.ConvertForOperation(ref lvalue, ref rvalue);
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }

            object result;
            try
            {
                result = this.Operate(lvalue, rvalue);
            }
            catch (InvalidOperationException)
            {
                return DependencyProperty.UnsetValue;
            }
            catch (Exception) // Any exception can be thrown by custom operators
            {
                return DependencyProperty.UnsetValue;
            }

            if (targetType != null)
            {
                result = ArithmeticConverterHelper.ConvertType(result, targetType, culture);
            }

            return result;
        }

        protected abstract object Operate(object lvalue, object rvalue);

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
