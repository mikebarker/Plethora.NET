using System;
using System.Collections;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public abstract class ComparisonConverterBase : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Comparer comparer = new Comparer(CultureInfo.CurrentCulture);

            int compareResult = this.Compare(value, parameter, comparer);
            bool result = this.Compare(compareResult);
            return result;
        }

        protected int Compare(object lvalue, object rvalue, IComparer comparer)
        {
            int result = comparer.Compare(lvalue, rvalue);
            return result;
        }

        protected abstract bool Compare(int compareResult);

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
