using JetBrains.Annotations;
using Plethora.Context.Action;
using Plethora.Xaml.Uwp.Converters;
using System;
using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace Plethora.Context
{
    [ValueConversion(typeof(IAction), typeof(ICommand))]
    public sealed class ContextCommandConverter : IValueConverter
    {
        [ContractAnnotation("value: null => null; value: notnull => notnull")]
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IAction action = value as IAction;
            if (action == null)
                return null;

            return ContextCommandHelper.AsCommand(action);
        }

        [ContractAnnotation("=> halt")]
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
