using System;

namespace Plethora
{
    public class WeakObserver<T> : IObserver<T>
    {
        private WeakReference<IObserver<T>> innerObserver;
        private Action onObserverCollected;

        public WeakObserver(IObserver<T> observer, Action onObserverCollected)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            if (onObserverCollected == null)
                throw new ArgumentNullException("onObserverCollected");

            this.innerObserver = new WeakReference<IObserver<T>>(observer);
            this.onObserverCollected = onObserverCollected;
        }


        public void OnNext(T value)
        {
            IObserver<T> observer = GetObserver();
            if (observer == null)
                return;

            observer.OnNext(value);
        }

        public void OnError(Exception error)
        {
            IObserver<T> observer = GetObserver();
            if (observer == null)
                return;

            observer.OnError(error);
        }

        public void OnCompleted()
        {
            IObserver<T> observer = GetObserver();
            if (observer == null)
                return;

            observer.OnCompleted();
        }

        private IObserver<T> GetObserver()
        {
            if (innerObserver == null)
                return null;

            IObserver<T> target = innerObserver.Target;
            if (target == null)
            {
                if (onObserverCollected != null)
                    onObserverCollected();

                innerObserver = null;
                onObserverCollected = null;
                return null;
            }

            return target;
        }
    }

    public static class WeakSubscriptionHelper
    {
        public static IDisposable WeakSubscribe<T>(this IObservable<T> observable, IObserver<T> observer)
        {
            DisposableContainer disposableContainer = new DisposableContainer();

            WeakObserver<T> weakObserver = new WeakObserver<T>(
                observer,
                () => disposableContainer.Dispose());

            IDisposable disposable = observable.Subscribe(weakObserver);
            disposableContainer.SetDisposable(disposable);

            return disposableContainer;
        }

        private sealed class DisposableContainer : IDisposable
        {
            private IDisposable disposable;

            public void SetDisposable(IDisposable innerDisposable)
            {
                this.disposable = innerDisposable;
            }

            #region Implementation of IDisposable

            bool disposed = false;

            ~DisposableContainer()
            {
                Dispose(false);
            }

            // Public implementation of Dispose pattern callable by consumers. 
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            // Protected implementation of Dispose pattern. 
            private void Dispose(bool disposing)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    // Free any other managed objects here. 
                    if (this.disposable != null)
                    {
                        this.disposable.Dispose();
                        this.disposable = null;
                    }
                }

                // Free any unmanaged objects here. 
                disposed = true;
            }

            #endregion
        }
    }
}
