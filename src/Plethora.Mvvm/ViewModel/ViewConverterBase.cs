using Plethora.Xaml.Converters;
using System;

namespace Plethora.Mvvm.ViewModel
{
    /// <summary>
    /// A converter which converts an <see cref="IViewModel"/> to its associated view.
    /// </summary>
    /// <typeparam name="TGlobalization"></typeparam>
    public abstract class ViewConverterBase<TGlobalization, TUnsetValueProvider> : ConverterBase<TGlobalization, TUnsetValueProvider>
        where TUnsetValueProvider : IUnsetValueProvider, new()
    {
        /// <summary>
        /// Gets the view from the supplied <see cref="IViewModel"/>.
        /// </summary>
        /// <param name="value">The <see cref="IViewModel"/> from which to get the view.</param>
        public sealed override object Convert(object value, Type targetType, object parameter, TGlobalization globalization)
        {
            if (value is IViewModel viewModel)
            {
                return viewModel.View;
            }

            return this.UnsetValue;
        }
    }
}
