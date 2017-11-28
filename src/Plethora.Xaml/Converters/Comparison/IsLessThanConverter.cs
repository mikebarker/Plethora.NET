using System.Windows.Data;

namespace Plethora.Xaml.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsLessThanConverter : ComparisonConverterBase
    {
        protected override bool Compare(int compareResult)
        {
            return (compareResult < 0);
        }
    }
}
