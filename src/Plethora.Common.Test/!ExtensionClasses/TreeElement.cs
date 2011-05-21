using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Test.ExtensionClasses
{
    class Tree : IEnumerable<TreeElement>
    {
        private readonly TreeElement root;

        public Tree(TreeElement root)
        {
            this.root = root;
        }

        #region Implementation of IEnumerable<TreeElement>

        public IEnumerator<TreeElement> GetEnumerator()
        {
            return Enumerable.Repeat(root, 1).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    class TreeElement
    {
        private readonly List<TreeElement> children = new List<TreeElement>();

        public void AddChild(TreeElement element)
        {
            children.Add(element);
        }

        public IEnumerable<TreeElement> Children
        {
            get { return children; }
        }
    }
}
