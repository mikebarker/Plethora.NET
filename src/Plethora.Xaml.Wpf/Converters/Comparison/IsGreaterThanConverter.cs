using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsGreaterThanConverter : ComparisonConverterBase
    {
        protected override bool Compare(int compareResult)
        {
            return (compareResult > 0);
        }
    }
}
