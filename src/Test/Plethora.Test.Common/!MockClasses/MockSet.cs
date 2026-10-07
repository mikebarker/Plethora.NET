using System.Linq;
using Plethora.Collections.Sets;

namespace Plethora.Test.MockClasses;

class MockSetCore<T>(params T[] elements) : BaseSetImpl<T>
{
    #region Implementation of ISetCore<T>

    public override bool Contains(T element)
    {
        return elements.Contains(element);
    }

    public override bool? IsEmpty
    {
        get { return !elements.Any(); }
    }

    #endregion
}
