using System;
using System.Collections.Generic;
using System.Text;

namespace Plethora.Mvvm.Binding
{
    public class BindingObserver<T, TValue> : IBindingSetter<T>, IBindingObserver<TValue>
    {
        private readonly IBindingSetter<T> root;
        private readonly IBindingObserver<TValue> leaf;

        public BindingObserver(
            IBindingSetter<T> root,
            IBindingObserver<TValue> leaf)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (leaf == null)
                throw new ArgumentNullException(nameof(leaf));

            this.root = root;
            this.leaf = leaf;
        }

        public void SetObserved(T observed)
        {
            this.root.SetObserved(observed);
        }

        public event EventHandler ValueChanged
        {
            add { this.leaf.ValueChanged += value; }
            remove { this.leaf.ValueChanged += value; }
        }

        public bool TryGetValue(out TValue value)
        {
            return this.leaf.TryGetValue(out value);
        }
    }
}
