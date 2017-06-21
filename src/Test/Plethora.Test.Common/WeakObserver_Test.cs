using System;
using NUnit.Framework;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test
{
    [TestFixture]
    public class WeakObserver_Test
    {
        [Test]
        public void WeakObserver_OnNext()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //exec
            publisher.OnNext(123);

            //test
            Assert.AreEqual(1, onNextCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [Test]
        public void WeakObserver_OnError()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //exec
            publisher.OnError(new Exception());

            //test
            Assert.AreEqual(1, onErrorCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [Test]
        public void WeakObserver_OnCompleted()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //exec
            publisher.OnCompleted();

            //test
            Assert.AreEqual(1, onCompleteCount);
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }


        [Test]
        public void WeakObserver_KeptAlive()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //exec
            GC.Collect(2, GCCollectionMode.Forced); // Force a full GC
            publisher.OnNext(123);

            //test
            Assert.IsTrue(publisher.HasObservers);

            GC.KeepAlive(subscriber);
        }

        [Test]
        public void WeakObserver_Unsubscribe()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //test
            Assert.IsTrue(publisher.HasObservers);

            subscription.Dispose();

            Assert.IsFalse(publisher.HasObservers);
        }

        [Test]
        public void WeakObserver_Collected()
        {
            //setup
            int onNextCount = 0;
            int onErrorCount = 0;
            int onCompleteCount = 0;

            Subject<int> publisher = new Subject<int>();
            Observer<int> subscriber = new Observer<int>(
                delegate { onNextCount++; },
                delegate { onErrorCount++; },
                delegate { onCompleteCount++; });


            var subscription = publisher.WeakSubscribe(subscriber);

            //exec
            subscriber = null;  // Do not reference
            GC.Collect(2, GCCollectionMode.Forced); // Force a full GC
            publisher.OnNext(123);

            //test
            Assert.IsFalse(publisher.HasObservers);
        }

    }
}
