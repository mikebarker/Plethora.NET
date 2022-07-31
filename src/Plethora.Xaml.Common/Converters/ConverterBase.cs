using System;

namespace Plethora.Xaml.Converters
{
    public abstract class ConverterBase<TGlobalization, TUnsetValueProvider>
        where TUnsetValueProvider : IUnsetValueProvider, new()
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="globalization">The globalization to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public abstract object Convert(object value, Type targetType, object parameter, TGlobalization globalization);

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="globalization">The globalization to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used
        /// </returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, TGlobalization globalization)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the unset value for the supported framework of the implementing class.
        /// </summary>
        protected object UnsetValue
        {
            get
            {
                return new TUnsetValueProvider().UnsetValue;
            }
        }
    }
}
