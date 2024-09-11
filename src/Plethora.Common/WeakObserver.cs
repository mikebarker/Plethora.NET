using System;

namespace Plethora
{
    /// <summary>
    /// A weak wrapper for <see cref="IObserver{T}"/> objects.
    /// </summary>
    /// <typeparam name="T">The type of the object that provides notification information.</typeparam>
    /// <seealso cref="WeakSubscriptionHelper.WeakSubscribe{T}(IObservable{T}, IObserver{T})"/>
    public class WeakObserver<T> : IObserver<T>
    {
        private WeakReference<IObserver<T>>? innerObserver;
        private Action? onObserverCollected;

        /// <summary>
        /// Initialises a new instance of the <see cref="WeakObserver{T}"/> class.
        /// </summary>
        /// <param name="observer"></param>
        /// <param name="onObserverCollected"></param>
        public WeakObserver(IObserver<T> observer, Action onObserverCollected)
        {
            //Validation
            ArgumentNullException.ThrowIfNull(observer);
            ArgumentNullException.ThrowIfNull(onObserverCollected);

            this.innerObserver = new(observer);
            this.onObserverCollected = onObserverCollected;
        }


        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="value">The current notification information.</param>
        public void OnNext(T value)
        {
            var observer = this.GetObserver();
            if (observer is null)
                return;

            observer.OnNext(value);
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            var observer = this.GetObserver();
            if (observer is null)
                return;

            observer.OnError(error);
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            var observer = this.GetObserver();
            if (observer is null)
                return;

            observer.OnCompleted();
        }


        /// <summary>
        /// Dereferences the weak reference and cleans up the references and subscription when the 
        /// inner observer has been collected.
        /// </summary>
        /// <returns>
        /// The internal observer; or <see langword="null"/> if it has been collected.
        /// </returns>
        private IObserver<T>? GetObserver()
        {
            if (this.innerObserver is null)
                return null;

            if (!this.innerObserver.TryGetTarget(out var target))
            {
                this.onObserverCollected?.Invoke();

                this.innerObserver = null;
                this.onObserverCollected = null;
                return null;
            }

            return target;
        }
    }

    /// <summary>
    /// Provides static methods for working with weakly referenced <see cref="IObserver{T}"/> instances.
    /// </summary>
    public static class WeakSubscriptionHelper
    {
        /// <summary>
        /// Creates a weak wrapper around an <see cref="IObserver{T}"/> instances, and notifies the provider that an observer is to receive notifications.
        /// </summary>
        /// <typeparam name="T">The type of the object that provides notification information.</typeparam>
        /// <param name="observable">The object that is to provide notifications.</param>
        /// <param name="observer">The object that is to receive notifications.</param>
        /// <returns>
        /// A reference to an interface that allows observers to stop receiving notifications before the provider has finished sending them.
        /// </returns>
        public static IDisposable WeakSubscribe<T>(this IObservable<T> observable, IObserver<T> observer)
        {
            //Notice that the call to the Dispose method is required within the constructor of the WeakObserver, which
            // occurs before the disposable has been created. Therefore, we create a DisposableContainer. This allows
            // the reference to the IDisposable to be set after construction, but also allows its Dispose method to be
            // utilised before the inner IDisposable has been set.

            DisposableContainer disposableContainer = new();

            WeakObserver<T> weakObserver = new(
                observer,
                disposableContainer.Dispose);

            IDisposable disposable = observable.Subscribe(weakObserver);
            disposableContainer.SetDisposable(disposable);

            return disposableContainer;
        }

        /// <summary>
        /// Container class for <see cref="IDisposable"/> which allows the inner disposable to be set after construction.
        /// </summary>
        private sealed class DisposableContainer : IDisposable
        {
            private IDisposable? disposable;

            public void SetDisposable(IDisposable innerDisposable)
            {
                this.disposable = innerDisposable;
            }

            #region Implementation of IDisposable

            bool disposed = false;

            ~DisposableContainer()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private void Dispose(bool disposing)
            {
                if (this.disposed)
                    return;

                if (disposing)
                {
                    // Free any other managed objects here. 
                    if (this.disposable is not null)
                    {
                        this.disposable.Dispose();
                        this.disposable = null;
                    }
                }

                // Free any unmanaged objects here. 
                this.disposed = true;
            }

            #endregion
        }
    }
}
