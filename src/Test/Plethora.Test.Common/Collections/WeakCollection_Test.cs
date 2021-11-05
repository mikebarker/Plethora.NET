using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class WeakCollection_Test
    {
        [TestMethod]
        public void SomeItemsRemainAfterCollection()
        {
            // Arrange
            WeakCollection<StringContainer> weakCollection = new WeakCollection<StringContainer>();
            var alpha = new StringContainer("alpha");
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateCollection(weakCollection);
            weakCollection.Add(zeta);
            weakCollection.Add(eta);

            // Assert
            Assert.AreEqual(7, weakCollection.Count);
            Assert.IsTrue(weakCollection.Contains(alpha));
            Assert.IsTrue(weakCollection.Contains(zeta));
            Assert.IsTrue(weakCollection.Contains(eta));

            // Action
            GC.Collect(2, GCCollectionMode.Forced);

            // Assert
            Assert.AreEqual(2, weakCollection.Count);
            Assert.IsFalse(weakCollection.Contains(alpha));
            Assert.IsTrue(weakCollection.Contains(zeta));
            Assert.IsTrue(weakCollection.Contains(eta));


            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }

        private static void PopulateCollection(WeakCollection<StringContainer> collection)
        {
            collection.Add(new StringContainer("alpha"));
            collection.Add(new StringContainer("beta"));
            collection.Add(new StringContainer("gamma"));
            collection.Add(new StringContainer("delta"));
            collection.Add(new StringContainer("epsilon"));
        }

        #region Private classes

        private class StringContainer
        {
            private readonly string text;

            public StringContainer(string text)
            {
                this.text = text;
            }

            public override bool Equals(object obj)
            {
                return obj is StringContainer other &&
                       text.Equals(other.text);
            }

            public override int GetHashCode()
            {
                return text.GetHashCode();
            }
        }

        #endregion
    }
}
