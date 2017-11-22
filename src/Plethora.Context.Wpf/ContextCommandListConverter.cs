using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

using JetBrains.Annotations;

using Plethora.Context.Action;

namespace Plethora.Context
{
    [ValueConversion(typeof(IEnumerable<IAction>), typeof(IEnumerable<ICommand>))]
    public sealed class ContextCommandListConverter : IValueConverter
    {
        [ContractAnnotation("value: null => null; value: notnull => notnull")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<IAction> actions = value as IEnumerable<IAction>;
            if (actions == null)
                return null;

            return actions.Select(action => ContextCommandHelper.AsCommand(action));
        }

        [ContractAnnotation("=> halt")]
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
