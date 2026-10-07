using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class WeakDelegate_Test
    {
        [TestMethod]
        public void TargetKeptAlive_Action()
        {
            // Arrange
            int callbackCount = 0;
            bool targetCollected = false;
            Subscriber subscriber = new(() => callbackCount++);

            // Action
            var weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                subscriber.Callback,
                handler => { targetCollected = true; });

            weakDelegate(null, EventArgs.Empty);
            weakDelegate(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(2, callbackCount);
            Assert.IsFalse(targetCollected);

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void TargetKeptAlive_Func()
        {
            // Arrange
            int callbackCount = 0;
            bool targetCollected = false;
            FuncSubscriber subscriber = new(() => callbackCount++);

            // Action
            var weakDelegate = WeakDelegate.CreateWeakDelegate<FuncEventHandler, long>(
                subscriber.Callback,
                handler => { targetCollected = true; return -1L; });

            var result1 = weakDelegate(null, EventArgs.Empty);
            var result2 = weakDelegate(null, EventArgs.Empty);

            // Assert
            Assert.AreEqual(0, result1);
            Assert.AreEqual(1, result2);
            Assert.AreEqual(2, callbackCount);
            Assert.IsFalse(targetCollected);

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void TargetCollected_Action()
        {
            // Arrange
            Publisher publisher = new();

            int callbackCount = 0;
            bool targetCollected = false;

            Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
            {
                Subscriber subscriber = new(() => callbackCount++);

                // Action
                publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(
                    subscriber.Callback,
                    handler => { targetCollected = true; });

                // Assert
                publisher.TriggerEvent();
                publisher.TriggerEvent();

                GC.KeepAlive(subscriber);
                Assert.AreEqual(2, callbackCount);
                Assert.IsFalse(targetCollected);

                subscriber = null;
            };
            setup();
            GC.Collect(2);

            //test
            publisher.TriggerEvent();
            publisher.TriggerEvent();

            Assert.AreEqual(2, callbackCount);  // unchanged from above
            Assert.IsTrue(targetCollected);
        }

        [TestMethod]
        public void TargetCollected_Func()
        {
            // Arrange
            FuncPublisher publisher = new();

            int callbackCount = 0;
            bool targetCollected = false;

            Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
            {
                FuncSubscriber subscriber = new(() => callbackCount++);

                // Action
                publisher.Event += WeakDelegate.CreateWeakDelegate<FuncEventHandler, long>(
                    subscriber.Callback,
                    handler => { targetCollected = true; return -1L; });

                // Assert
                publisher.TriggerEvent();
                publisher.TriggerEvent();

                GC.KeepAlive(subscriber);
                Assert.AreEqual(2, callbackCount);
                Assert.IsFalse(targetCollected);

                subscriber = null;
            };
            setup();
            GC.Collect(2);

            //test
            publisher.TriggerEvent();
            publisher.TriggerEvent();

            Assert.AreEqual(2, callbackCount);  // unchanged from above
            Assert.IsTrue(targetCollected);
        }

        [TestMethod]
        public void TargetCollectedDelegateRemoved()
        {
            // Arrange
            Publisher publisher = new();

            int callbackCount = 0;

            Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
            {
                Subscriber subscriber = new(() => callbackCount++);

                // Action
                publisher.Event += WeakDelegate.CreateWeakDelegate<EventHandler>(
                    subscriber.Callback,
                    handler => publisher.Event -= handler);

                subscriber = null;
            };
            setup();
            GC.Collect(2);

            // Assert
            Assert.IsFalse(publisher.EventIsEmpty);

            publisher.TriggerEvent();

            Assert.IsTrue(publisher.EventIsEmpty);
        }

        [TestMethod]
        public void StaticMethod()
        {
            // Arrange
            int callbackCount = 0;
            StaticSubscriber.Action = () => callbackCount++;

            // Action
            var weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                StaticSubscriber.Callback,
                handler => { });

            var callbackDelegate = new EventHandler(StaticSubscriber.Callback);

            // Assert
            Assert.AreEqual(callbackDelegate, weakDelegate);
        }

        [TestMethod]
        public void Helper_IsTargetAlive_StaticMethod()
        {
            // Arrange
            int callbackCount = 0;
            StaticSubscriber.Action = () => callbackCount++;

            // Action
            var weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                StaticSubscriber.Callback,
                handler => { });

            // Assert
            Assert.IsTrue(weakDelegate.IsTargetAlive());
        }

        [TestMethod]
        public void Helper_IsTargetAlive_NotWeakDelegate()
        {
            // Arrange
            Publisher publisher = new();

            // Action
            Action @delegate = publisher.TriggerEvent;

            // Assert
            Assert.IsTrue(@delegate.IsTargetAlive());
        }

        [TestMethod]
        public void Helper_IsTargetAlive_TargetKeptAlive()
        {
            // Arrange
            Publisher publisher = new();

            Subscriber subscriber = new(() => { });

            // Action
            var weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                subscriber.Callback,
                handler => { });
            publisher.Event += weakDelegate;

            // Assert
            Assert.IsTrue(weakDelegate.IsTargetAlive());

            GC.KeepAlive(subscriber);
        }

        [TestMethod]
        public void Helper_IsTargetAlive_TargetCollected()
        {
            // Arrange
            Publisher publisher = new();

            EventHandler weakDelegate = null;
            int callbackCount = 0;

            Action setup = delegate () // .NET Core GC requires the value be out of scope, to be eligable for collection
            {
                Subscriber subscriber = new(() => callbackCount++);

                // Action
                weakDelegate = WeakDelegate.CreateWeakDelegate<EventHandler>(
                    subscriber.Callback,
                    handler => publisher.Event -= handler);
                publisher.Event += weakDelegate;

                subscriber = null;
            };
            setup();

            GC.Collect(2);

            // Assert
            Assert.IsFalse(weakDelegate.IsTargetAlive());
        }


        #region Private classes

        private static class StaticSubscriber
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

        private class Subscriber(Action action)
        {
            public void Callback(object o, EventArgs e)
            {
                action();
            }
        }

        private class Publisher
        {
            public event EventHandler Event;

            public void TriggerEvent()
            {
                Event?.Invoke(this, EventArgs.Empty);
            }

            public bool EventIsEmpty
            {
                get { return Event == null; }
            }
        }

        private delegate long FuncEventHandler(object sender, EventArgs e);

        private class FuncSubscriber(Func<long> func)
        {
            public long Callback(object o, EventArgs e)
            {
                return func();
            }
        }

        private class FuncPublisher
        {
            public event FuncEventHandler Event;

            public void TriggerEvent()
            {
                Event?.Invoke(this, EventArgs.Empty);
            }

            public bool EventIsEmpty
            {
                get { return Event == null; }
            }
        }

        #endregion
    }
}