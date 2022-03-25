using System;

namespace Plethora.Mvvm.Binding
{


    public interface IBindingSetter
    {
        void SetObserved(object observed);
    }

    public interface IBindingSetter<T> : IBindingSetter
    {
        void SetObserved(T observed);
    }

    public interface IBindingObserver
    {
        event EventHandler ValueChanged;

        bool TryGetValue(out object value);
    }

    public interface IBindingObserver<TValue> : IBindingObserver
    {
        bool TryGetValue(out TValue value);
    }

    public interface IBindingObserverElement : IBindingSetter, IBindingObserver
    {
        void SetParent(IBindingObserver parent);
    }

    public interface IBindingObserverElement<T, TValue> : IBindingObserverElement, IBindingSetter<T>, IBindingObserver<TValue>
    {
        void SetParent(IBindingObserver<T> parent);
    }
}
