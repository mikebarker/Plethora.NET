using System;
using System.Collections.Generic;
using System.Windows;

namespace Plethora.Context.Wpf
{
    public interface IWpfContextSource
    {
        event EventHandler ContextChanged;

        IEnumerable<ContextInfo> Contexts { get; }

        UIElement UIElement { get; set; }
    }
}
