﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Plethora.Mvvm.Bindings
{
    public interface IBindingObserver
    {
        event EventHandler? ValueChanging;

        event EventHandler? ValueChanged;

        void SetObserved(object? observed);

        bool TryGetValue([MaybeNullWhen(false)] out object value);
    }

    /// <summary>
    /// A single element in a binding expression, which can be chained to a parent to form complex bindings.
    /// </summary>
    public interface IBindingObserverElement : IBindingObserver
    {
        void SetParent(IBindingObserver? parent);
    }
}
