﻿using System;

namespace Plethora.Test.UtilityClasses
{
    class Observer<T> : IObserver<T>
    {
        private readonly Action<T> onNext;
        private readonly Action<Exception> onError;
        private readonly Action onCompleted;

        public Observer(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            this.onNext = onNext;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            this.onNext(value);
        }

        public void OnError(Exception error)
        {
            this.onError(error);
        }

        public void OnCompleted()
        {
            this.onCompleted();
        }
    }
}
