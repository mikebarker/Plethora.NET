using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    /// <summary>
    /// The boolean operators the can be used with the <see cref="BooleanLogicConverter"/>.
    /// </summary>
    public enum BooleanLogicOperator
    {
        And,
        Or,
        Xor
    }

    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to apply a logical operator (AND, OR, or XOR) to two boolean values.
    /// </summary>
    /// <remarks>
    /// Requires two boolean values.
    /// The parameter must be one of <see cref="BooleanLogicOperator"/> representating the operation required.
    /// </remarks>
    public class BooleanLogicConverter : IMultiValueConverter
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

            if (!(parameter is BooleanLogicOperator))
                return DependencyProperty.UnsetValue;

            switch ((BooleanLogicOperator)parameter)
            {
                case BooleanLogicOperator.And:
                {
                    bool result = BooleanLogicConverter.And((bool)values[0], (bool)values[1]);
                    return result;
                }

                case BooleanLogicOperator.Or:
                {
                    bool result = BooleanLogicConverter.Or((bool)values[0], (bool)values[1]);
                    return result;
                }

                case BooleanLogicOperator.Xor:
                {
                    bool result = BooleanLogicConverter.Xor((bool)values[0], (bool)values[1]);
                    return result;
                }

                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
