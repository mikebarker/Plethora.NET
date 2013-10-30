using System.Collections.Generic;
using System.Linq;
using Plethora.Collections.Sets;

namespace Plethora.Test.MockClasses
{
    class MockSetCore<T> : BaseSetImpl<T>
    {
        private readonly IEnumerable<T> elements;

        public MockSetCore(params T[] elements)
        {
            this.elements = elements;
        }

        #region Implementation of ISetCore<T>

        public override bool Contains(T element)
        {
            return this.elements.Contains(element);
        }

        public override bool? IsEmpty
        {
            get { return !elements.Any(); }
        }

        #endregion
    }
}
