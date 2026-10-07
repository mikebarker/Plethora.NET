using System;

namespace Plethora.Test.UtilityClasses;

/// <summary>
/// A light-weight implementation of the Reactive Extensions' observer, so that the library doesn't have to be referenced.
/// </summary>
class Observer<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted) : IObserver<T>
{
    public void OnNext(T value)
    {
        onNext(value);
    }

    public void OnError(Exception error)
    {
        onError(error);
    }

    public void OnCompleted()
    {
        onCompleted();
    }
}
