using Plethora.Mvvm.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Plethora.Xaml.Uwp.Converters
{
    /// <inheritdoc/>
    [ValueConversion(typeof(IViewModel), typeof(FrameworkElement))]
    public class ViewConverter : ViewConverterBase<string, UnsetValueProvider>, IValueConverter
    {
    }
}
