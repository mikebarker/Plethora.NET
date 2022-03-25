using JetBrains.Annotations;
using System;
using System.Linq;

namespace Plethora.Mvvm.Binding
{
    public class BindingObserver<T, TValue> : IBindingSetter<T>, IBindingObserver<TValue>
    {
        private readonly IBindingSetter<T> root;
        private readonly IBindingObserver<TValue> leaf;

        public BindingObserver(
            [NotNull] IBindingSetter<T> root,
            [NotNull] IBindingObserver<TValue> leaf)
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

        void IBindingSetter.SetObserved([NotNull] object observed)
        {
            this.root.SetObserved(observed);
        }

        bool IBindingObserver.TryGetValue(out object value)
        {
            var result = this.leaf.TryGetValue(out TValue tvalue);
            value = tvalue;
            return result;
        }
    }

    public static class BindingObserver
    {
        public static IBindingObserver Create(IBindingSetter root, IBindingObserver leaf)
        {
            var observedType = root.GetType().GetGenericArguments()[0];

            var genericInterface = leaf.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType)
                .Where(iface => iface.GetGenericTypeDefinition() == typeof(IBindingObserver<>))
                .Single();

            var valueType = genericInterface.GetGenericArguments()[0];

            var bindingObserverType = typeof(BindingObserver<,>).MakeGenericType(observedType, valueType);
            var bindingObserverObj = bindingObserverType.GetConstructors().Single().Invoke(new object[] { root, leaf });
            var bindingObserver = (IBindingObserver)bindingObserverObj;
            return bindingObserver;
        }
    }
}
