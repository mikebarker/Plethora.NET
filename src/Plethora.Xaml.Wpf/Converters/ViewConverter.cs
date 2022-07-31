using Plethora.Mvvm.ViewModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    /// <inheritdoc/>
    [ValueConversion(typeof(IViewModel), typeof(FrameworkElement))]
    public class ViewConverter : ViewConverterBase<CultureInfo, UnsetValueProvider>, IValueConverter
    {
    }
}
