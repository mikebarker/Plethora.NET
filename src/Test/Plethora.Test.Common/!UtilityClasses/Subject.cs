using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Test.UtilityClasses;

/// <summary>
/// A light version of the Reactive Extensions' Subject class, so that the library doesn't have to be referenced.
/// </summary>
class Subject<T> : IObservable<T>, IObserver<T>
{
    readonly List<IObserver<T>> observers = new();


    #region Implementation of IObservable<T>

    public IDisposable Subscribe(IObserver<T> observer)
    {
        this.observers.Add(observer);
        return new Unsubscriber(this, observer);
    }

    #endregion

    #region Implementation of IObserver<T>

    public void OnNext(T value)
    {
        //Create a copy of the list to allow the subscription list to be modified
        // whilst enumerating.
        var list = this.observers.ToList();

        foreach (var observer in list)
        {
            observer.OnNext(value);
        }
    }

    public void OnError(Exception error)
    {
        //Create a copy of the list to allow the subscription list to be modified
        // whilst enumerating.
        var list = this.observers.ToList();

        foreach (var observer in list)
        {
            observer.OnError(error);
        }
    }

    public void OnCompleted()
    {
        //Create a copy of the list to allow the subscription list to be modified
        // whilst enumerating.
        var list = this.observers.ToList();

        foreach (var observer in list)
        {
            observer.OnCompleted();
        }
    }

    #endregion

    public bool HasObservers
    {
        get { return this.observers.Count > 0; }
    }

    private class Unsubscriber(Subject<T> subject, IObserver<T> observer) : IDisposable
    {
        public void Dispose()
        {
            subject.observers.Remove(observer);
        }
    }
}
