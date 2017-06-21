using System;
using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class WeakEvent_Test
    {
        [Test]
        public void SubscribeFromEvent()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += subscriber.Callback;

            //test
            Assert.AreEqual(false, publisher.EventIsEmpty);
        }

        [Test]
        public void UnsubscribeFromEvent()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += subscriber.Callback;

            //test
            Assert.AreEqual(false, publisher.EventIsEmpty);

            publisher.Event -= subscriber.Callback;

            Assert.AreEqual(true, publisher.EventIsEmpty);
        }

        [Test]
        public void TargetKeptAlive()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += subscriber.Callback;

            publisher.TriggerEvent();
            publisher.TriggerEvent();

            //test
            Assert.AreEqual(2, callbackCount);
            Assert.AreEqual(false, publisher.EventIsEmpty);
            GC.KeepAlive(subscriber);
        }

        [Test]
        public void TargetCollected()
        {
            //setup
            Publisher publisher = new Publisher();

            int callbackCount = 0;
            Subscriber subscriber = new Subscriber(() => callbackCount++);

            //exec
            publisher.Event += subscriber.Callback;


            //test
            publisher.TriggerEvent();
            publisher.TriggerEvent();

            Assert.AreEqual(2, callbackCount);
            Assert.AreEqual(false, publisher.EventIsEmpty);
            GC.KeepAlive(subscriber);

            subscriber = null;
            GC.Collect(2);

            publisher.TriggerEvent();

            Assert.AreEqual(2, callbackCount);
            Assert.AreEqual(true, publisher.EventIsEmpty);
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
            private readonly WeakEvent<EventHandler> weakEvent = new WeakEvent<EventHandler>();

            public event EventHandler Event
            {
                add { weakEvent.Add(value); }
                remove { weakEvent.Remove(value); }
            }


            public void TriggerEvent()
            {
                foreach (var handler in weakEvent.GetInvocationList())
                {
                    handler(this, EventArgs.Empty);
                }
            }

            public bool EventIsEmpty
            {
                get
                {
                    var list = weakEvent.GetInvocationList();
                    return ((list == null) || (list.Length == 0));
                }
            }
        }
    }
}