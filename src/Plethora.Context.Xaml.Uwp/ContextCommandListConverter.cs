using JetBrains.Annotations;
using Plethora.Context.Action;
using Plethora.Xaml.Uwp.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Data;

namespace Plethora.Context
{
    [ValueConversion(typeof(IEnumerable<IAction>), typeof(IEnumerable<ICommand>))]
    public sealed class ContextCommandListConverter : IValueConverter
    {
        [ContractAnnotation("value: null => null; value: notnull => notnull")]
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IEnumerable<IAction> actions = value as IEnumerable<IAction>;
            if (actions == null)
                return null;

            return actions.Select(action => ContextCommandHelper.AsCommand(action));
        }

        [ContractAnnotation("=> halt")]
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
