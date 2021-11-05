using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test
{
    [TestClass]
    public class WeakObserver_Test
    {
        [TestMethod]
        public void WeakObserver_OnNext()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            // Action
            publisher.OnNext(123);

            // Assert
            Assert.AreEqual(1, onNextCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void WeakObserver_OnError()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            // Action
            publisher.OnError(new Exception());

            // Assert
            Assert.AreEqual(1, onErrorCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void WeakObserver_OnCompleted()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            // Action
            publisher.OnCompleted();

            // Assert
            Assert.AreEqual(1, onCompleteCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }


        [TestMethod]
        public void WeakObserver_KeptAlive()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            // Action
            GC.Collect(2, GCCollectionMode.Forced); // Force a full GC
            publisher.OnNext(123);

            // Assert
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void WeakObserver_Unsubscribe()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            // Assert
            Assert.IsTrue(publisher.HasObservers);

            subscription.Dispose();

            Assert.IsFalse(publisher.HasObservers);
        }

        [TestMethod]
        public void WeakObserver_Collected()
        {
            // Arrange
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();

            Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
            {
                Observer<int> subscriber = new Observer<int>(
                    delegate { onNextCount++; },
                    delegate { onErrorCount++; },
                    delegate { onCompleteCount++; });


                var subscription = publisher.WeakSubscribe(subscriber);

                // Action
                subscriber = null;
            };
            setup();

            // Action
            GC.Collect(2, GCCollectionMode.Forced); // Force a full GC
            publisher.OnNext(123);

            // Assert
            Assert.IsFalse(publisher.HasObservers);
        }

    }
}
