using System;

namespace Plethora.Mvvm.Binding
{
    public interface IBindingSetter<T>
    {
        void SetObserved(T observed);
    }

    public interface IBindingObserver<TValue>
    {
        event EventHandler ValueChanged;

        bool TryGetValue(out TValue value);
    }

    public interface IBindingObserverElement<T, TValue> : IBindingSetter<T>, IBindingObserver<TValue>
    {
        void SetParent(IBindingObserver<T> parent);
    }
}
