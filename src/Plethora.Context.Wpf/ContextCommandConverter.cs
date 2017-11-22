using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

using JetBrains.Annotations;

using Plethora.Context.Action;

namespace Plethora.Context
{
    [ValueConversion(typeof(IAction), typeof(ICommand))]
    public sealed class ContextCommandConverter : IValueConverter
    {
        [ContractAnnotation("value: null => null; value: notnull => notnull")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IAction action = value as IAction;
            if (action == null)
                return null;

            return ContextCommandHelper.AsCommand(action);
        }

        [ContractAnnotation("=> halt")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
