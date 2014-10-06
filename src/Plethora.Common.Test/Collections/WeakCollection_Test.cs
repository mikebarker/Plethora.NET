using System;
using NUnit.Framework;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    //TODO: These test cases must be cleaned-up

    [TestFixture]
    public class WeakCollection_Test
    {
        [Test]
        public void Test1()
        { 
            WeakCollection<object> weakCollection = new WeakCollection<object>();

            object o1 = new object();
            object o2 = new object();
            object o3 = new object();

            weakCollection.Add(o1);
            weakCollection.Add(o2);
            weakCollection.Add(o3);
            weakCollection.Add(new object());
            weakCollection.Add(new object());

            GC.Collect(2, GCCollectionMode.Forced);

            int count = 0;
            foreach (object o in weakCollection)
            {
                count++;
            }

            Assert.AreEqual(3, count);

            GC.KeepAlive(o1);
            GC.KeepAlive(o2);
            GC.KeepAlive(o3);
        }

        [Test]
        public void Test2()
        { 
            WeakCollection<object> weakCollection = new WeakCollection<object>();

            weakCollection.Add(new object());
            weakCollection.Add(new object());
            weakCollection.Add(new object());

            GC.Collect(2, GCCollectionMode.Forced);

            int count = 0;
            foreach (object o in weakCollection)
            {
                count++;
            }

            Assert.AreEqual(0, count);
        }


    }
}
