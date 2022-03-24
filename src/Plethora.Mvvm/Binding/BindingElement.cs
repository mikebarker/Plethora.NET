using System;

namespace Plethora.Mvvm.Binding
{
    public abstract class BindingElement
    {
        public abstract IBindingObserverElement<object, object> CreateObserver();
    }
}
