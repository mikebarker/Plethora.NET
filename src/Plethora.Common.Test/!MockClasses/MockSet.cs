using System.Collections.Generic;
using System.Linq;
using Plethora.Collections.Sets;

namespace Plethora.Test.MockClasses
{
    class MockSet<T> : BaseSetImpl<T>
    {
        private readonly IEnumerable<T> elements;

        public MockSet(params T[] elements)
        {
            this.elements = elements;
        }

        #region Implementation of ISet<T>

        public override bool Contains(T element)
        {
            return this.elements.Contains(element);
        }

        #endregion
    }
}
