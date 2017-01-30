using System.Collections.Generic;

namespace Plethora.Context.Action
{
    public interface IActionsAdapter
    {
        IEnumerable<IAction> Convert(IEnumerable<IAction> actions);
    }
}
