using System;
using NUnit.Framework;

namespace Plethora.Test
{
    [TestFixture]
    public class WeakReference_Test
    {
        [Test]
        public void ReferenceCollected()
        {
            //init
            WeakReference<object> weakReference = new WeakReference<object>(new object());

            //exec
            GC.Collect(2);

            //test
            Assert.IsFalse(weakReference.IsAlive);
            Assert.IsNull(weakReference.Target);
        }

        [Test]
        public void ReferenceNotCollected()
        {
            //init
            var o = new object();
            WeakReference<object> weakReference = new WeakReference<object>(o);

            //exec
            GC.Collect(2);

            //test
            Assert.IsTrue(weakReference.IsAlive);
            Assert.IsNotNull(weakReference.Target);

            GC.KeepAlive(o);
        }

        [Test]
        public void HashCodeDoesNotChange()
        {
            //init
            WeakReference<object> weakReference = new WeakReference<object>(new object());

            //exec
            var hash1 = weakReference.GetHashCode();
            GC.Collect(2);
            if (weakReference.IsAlive)
                Assert.Ignore("Reference not collected."); // Ensure the reference was collected

            var hash2 = weakReference.GetHashCode();

            //test
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void Equailty()
        {
            //init
            //Reference the same object
            var o = new object();
            WeakReference<object> weakReference1 = new WeakReference<object>(o);
            WeakReference<object> weakReference2 = new WeakReference<object>(o);

            //test
            Assert.IsTrue(weakReference1.Equals(weakReference2));
        }

        [Test]
        public void InEquailty()
        {
            //init
            //Reference different objects
            WeakReference<object> weakReference1 = new WeakReference<object>(new object());
            WeakReference<object> weakReference2 = new WeakReference<object>(new object());

            //test
            Assert.IsFalse(weakReference1.Equals(weakReference2));
        }
    }
}
