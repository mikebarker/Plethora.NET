using Plethora.Xaml.Converters;
using System.Windows;

namespace Plethora.Xaml.Wpf.Converters
{
    public class UnsetValueProvider : IUnsetValueProvider
    {
        object IUnsetValueProvider.UnsetValue => DependencyProperty.UnsetValue;
    }
}
