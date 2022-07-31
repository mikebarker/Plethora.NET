using Plethora.Xaml.Converters;
using Windows.UI.Xaml;

namespace Plethora.Xaml.Uwp.Converters
{
    public class UnsetValueProvider : IUnsetValueProvider
    {
        object IUnsetValueProvider.UnsetValue => DependencyProperty.UnsetValue;
    }
}
