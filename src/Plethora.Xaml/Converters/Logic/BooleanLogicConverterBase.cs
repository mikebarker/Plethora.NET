using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to apply a logical operators to two boolean values.
    /// </summary>
    /// <remarks>
    /// Requires two boolean values.
    /// </remarks>
    public abstract class BooleanLogicConverterBase : IMultiValueConverter
    {
        public static bool And(bool value1, bool value2)
        {
            bool result = value1 & value2;
            return result;
        }

        public static bool Or(bool value1, bool value2)
        {
            bool result = value1 | value2;
            return result;
        }

        public static bool Xor(bool value1, bool value2)
        {
            bool result = value1 ^ value2;
            return result;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            if (!(values[0] is bool))
                return DependencyProperty.UnsetValue;

            if (!(values[1] is bool))
                return DependencyProperty.UnsetValue;


            bool result = this.ApplyLogic((bool)values[0], (bool)values[1]);
            return result;
        }

        protected abstract bool ApplyLogic(bool lvalue, bool rvalue);

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
