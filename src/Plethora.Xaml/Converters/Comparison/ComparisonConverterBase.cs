using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public abstract class ComparisonConverterBase : IValueConverter, IMultiValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Comparer comparer = new Comparer(culture);

            int compareResult = this.Compare(value, parameter, comparer);
            bool result = this.Compare(compareResult);
            return result;
        }

        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
                return DependencyProperty.UnsetValue;

            if (values.Length != 2)
                return DependencyProperty.UnsetValue;

            IComparer comparer = parameter is IComparer
                ? (IComparer)parameter
                : new Comparer(culture);

            int compareResult = this.Compare(values[0], values[1], comparer);
            bool result = this.Compare(compareResult);
            return result;
        }

        protected int Compare(object lvalue, object rvalue, IComparer comparer)
        {
            int result = comparer.Compare(lvalue, rvalue);
            return result;
        }

        protected abstract bool Compare(int compareResult);

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
