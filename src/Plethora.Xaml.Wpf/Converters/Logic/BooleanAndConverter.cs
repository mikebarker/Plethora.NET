﻿using System.Windows.Data;

namespace Plethora.Xaml.Wpf.Converters
{
    /// <summary>
    /// A <see cref="IMultiValueConverter"/> which acts to AND two boolean values.
    /// </summary>
    /// <remarks>
    /// Requires two boolean values.
    /// Returns (value[0] & value[1])
    /// </remarks>
    public class BooleanAndConverter : BooleanLogicConverterBase
    {
        protected override bool ApplyLogic(bool lvalue, bool rvalue)
        {
            bool result = lvalue & rvalue;
            return result;
        }
    }
}
