using System.Collections.Generic;

namespace Plethora.Cache
{
    public interface IArgument<TData, TArgument>
    {
        bool IsOverlapped(
            TArgument B,
            out IEnumerable<TArgument> notInB);

        bool IsDataIncluded(TData data);
    }
}
