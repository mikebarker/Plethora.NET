using System.Collections.Generic;

namespace Plethora.Context
{
    public interface IContextActionFactory
    {
        IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName);
    }
}
