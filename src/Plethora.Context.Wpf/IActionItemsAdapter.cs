using System.Collections.Generic;
using Plethora.Context.Action;

namespace Plethora.Context.Wpf
{
    public interface IActionItemsAdapter
    {
        IEnumerable<IAction> Convert(IEnumerable<IAction> actions);
    }
}
