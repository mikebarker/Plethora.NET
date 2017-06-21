using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Context.Action
{
    public interface IActionsAdapter
    {
        [NotNull]
        IEnumerable<IAction> Convert([NotNull] IEnumerable<IAction> actions);
    }
}
