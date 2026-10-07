using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections;

[TestClass]
public class WeakCollection_Test
{
    [TestMethod]
    public void SomeItemsRemainAfterCollection()
    {
        // Arrange
        WeakCollection<StringContainer> weakCollection = new();
        StringContainer alpha = new("alpha");
        StringContainer zeta = new("zeta");
        StringContainer eta = new("eta");

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
        collection.Add(new("alpha"));
        collection.Add(new("beta"));
        collection.Add(new("gamma"));
        collection.Add(new("delta"));
        collection.Add(new("epsilon"));
    }

    #region Private classes

    private class StringContainer(string text)
    {
        private readonly string text = text;

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
