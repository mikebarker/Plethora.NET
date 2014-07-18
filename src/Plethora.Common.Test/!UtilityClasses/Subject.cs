using System;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Test.UtilityClasses
{
    class Subject<T> : IObservable<T>, IObserver<T>
    {
        readonly List<IObserver<T>> observers = new List<IObserver<T>>();


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

        private class Unsubscriber : IDisposable
        {
            private readonly Subject<T> subject;
            private readonly IObserver<T> observer;

            public Unsubscriber(Subject<T> subject, IObserver<T> observer)
            {
                this.subject = subject;
                this.observer = observer;
            }

            public void Dispose()
            {
                this.subject.observers.Remove(this.observer);
            }
        }
    }
}
