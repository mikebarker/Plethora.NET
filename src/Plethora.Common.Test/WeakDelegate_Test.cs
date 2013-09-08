using System;
using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class WeakDelegate_Test
    {
        [Test]
        public void TargetKeptAlive()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            bool targetCollected = false;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(
                subscriber.Callback,
                handler => { targetCollected = true; });

            publisher.TriggerEvent();
            publisher.TriggerEvent();

            //test
            GC.KeepAlive(subscriber);
            Assert.AreEqual(2, callbackCount);
            Assert.AreEqual(false, targetCollected);
        }

        [Test]
        public void TargetCollected()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            bool targetCollected = false;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(
                subscriber.Callback,
                handler => { targetCollected = true; });


            //test
            publisher.TriggerEvent();
            publisher.TriggerEvent();

            GC.KeepAlive(subscriber);
            Assert.AreEqual(callbackCount, 2);
            Assert.AreEqual(false, targetCollected);

            subscriber = null;
            GC.Collect(2);

            publisher.TriggerEvent();
            publisher.TriggerEvent();

            Assert.AreEqual(callbackCount, 2);
            Assert.AreEqual(true, targetCollected);
        }

        [Test]
        public void TargetCollectedDelegateRemoved()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(
                subscriber.Callback,
                handler => publisher.Event -= handler);

            subscriber = null;
            GC.Collect(2);


            //test
            Assert.AreEqual(false, publisher.EventIsEmpty);

            publisher.TriggerEvent();

            Assert.AreEqual(true, publisher.EventIsEmpty);
        }

        [Test]
        public void StaticMethod()
        {
            //setup
            int callbackCount = 0;
            StaticSubscriber.Action = () => callbackCount++;

            //exec
            var weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                StaticSubscriber.Callback,
                handler => { });

            var callbackDelegate = new EventHandler(StaticSubscriber.Callback);

            //test
            Assert.AreEqual(callbackDelegate, weakDelegate);
        }




        public static class StaticSubscriber
        {
            public static Action Action
            {
                get;
                set;
            }

            public static void Callback(object o, EventArgs e)
            {
                Action();
            }
        }

        public class Subscriber
        {
            private readonly Action action;

            public Subscriber(Action action)
            {
                this.action = action;
            }

            public void Callback(object o, EventArgs e)
            {
                this.action();
            }
        }

        public class Publisher
        {
            public event EventHandler Event;

            public void TriggerEvent()
            {
                var handler = Event;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

            public bool EventIsEmpty
            {
                get { return (Event == null); }
            }
        }
    }
}