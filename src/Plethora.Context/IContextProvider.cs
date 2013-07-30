using System;
using System.Collections.Generic;

namespace Plethora.Context
{
    public interface IContextProvider
    {
        event EventHandler EnterContext;

        event EventHandler LeaveContext;

        event EventHandler ContextChanged;

        IEnumerable<ContextInfo> Contexts { get; }
    }
}
