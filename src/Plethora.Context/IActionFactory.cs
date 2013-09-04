using System.Collections.Generic;

namespace Plethora.Context
{
    public interface IActionFactory
    {
        IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName);
    }
}
