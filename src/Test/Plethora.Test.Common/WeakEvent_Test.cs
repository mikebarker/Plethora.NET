using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test;

[TestClass]
public class WeakEvent_Test
{
    [TestMethod]
    public void SubscribeFromEvent()
    {
        // Arrange
        Publisher publisher = new();

        int callbackCount = 0;
        Subscriber subscriber = new(() => callbackCount++);

        // Action
        publisher.Event += subscriber.Callback;

        // Assert
        Assert.IsFalse(publisher.EventIsEmpty);
    }

    [TestMethod]
    public void UnsubscribeFromEvent()
    {
        // Arrange
        Publisher publisher = new();

        int callbackCount = 0;
        Subscriber subscriber = new(() => callbackCount++);

        // Action
        publisher.Event += subscriber.Callback;

        // Assert
        Assert.IsFalse(publisher.EventIsEmpty);

        publisher.Event -= subscriber.Callback;

        Assert.IsTrue(publisher.EventIsEmpty);
    }

    [TestMethod]
    public void TargetKeptAlive()
    {
        // Arrange
        Publisher publisher = new();

        int callbackCount = 0;
        Subscriber subscriber = new(() => callbackCount++);

        // Action
        publisher.Event += subscriber.Callback;

        publisher.TriggerEvent();
        publisher.TriggerEvent();

        // Assert
        Assert.AreEqual(2, callbackCount);
        Assert.IsFalse(publisher.EventIsEmpty);
        GC.KeepAlive(subscriber);
    }

    [TestMethod]
    public void TargetCollected()
    {
        // Arrange
        Publisher publisher = new();

        int callbackCount = 0;

        Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
        {
            Subscriber subscriber = new(() => callbackCount++);

            // Action
            publisher.Event += subscriber.Callback;

            // Assert
            publisher.TriggerEvent();
            publisher.TriggerEvent();

            Assert.AreEqual(2, callbackCount);
            Assert.IsFalse(publisher.EventIsEmpty);
            GC.KeepAlive(subscriber);

            subscriber = null;
        };
        setup();


        // Action
        GC.Collect(2);

        // Assert
        publisher.TriggerEvent();

        Assert.AreEqual(2, callbackCount); // unchanged from above
        Assert.IsTrue(publisher.EventIsEmpty);
    }




    public class Subscriber(Action action)
    {
        public void Callback(object o, EventArgs e)
        {
            action();
        }
    }

    public class Publisher
    {
        private readonly WeakEvent<EventHandler> weakEvent = new();

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