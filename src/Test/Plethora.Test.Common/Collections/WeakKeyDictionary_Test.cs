using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class WeakKeyDictionary_Test
    {
        [TestMethod]
        public void ContainsKey_KeysCollected()
        {
            // Arrange
            var dictionary = new WeakKeyDictionary<StringContainer, double>();
            var alpha = new StringContainer("alpha");
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateDictionary(dictionary);
            dictionary.Add(zeta, 6);
            dictionary.Add(eta, 7);

            // Assert
            Assert.IsTrue(dictionary.ContainsKey(alpha));
            Assert.IsTrue(dictionary.ContainsKey(zeta));

            // Action
            // Garbage-collection should collect the items inserted within PopulateDictionary,
            // but the zeta and eta elements should remain in the dictionary
            GC.Collect(2, GCCollectionMode.Forced);

            // Assert
            Assert.IsFalse(dictionary.ContainsKey(alpha));
            Assert.IsTrue(dictionary.ContainsKey(zeta));

            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }

        [TestMethod]
        public void TryGetValueTryGetValue_KeysCollected()
        {
            // Arrange
            var dictionary = new WeakKeyDictionary<StringContainer, double>();
            var alpha = new StringContainer("alpha");
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateDictionary(dictionary);
            dictionary.Add(zeta, 6);
            dictionary.Add(eta, 7);

            double value;
            bool result;

            // Assert
            result = dictionary.TryGetValue(alpha, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(1, value);

            result = dictionary.TryGetValue(zeta, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(6, value);

            // Action
            // Garbage-collection should collect the items inserted within PopulateDictionary,
            // but the zeta and eta elements should remain in the dictionary
            GC.Collect(2, GCCollectionMode.Forced);

            // Assert
            result = dictionary.TryGetValue(alpha, out value);
            Assert.IsFalse(result);
            Assert.AreEqual(default(double), value);

            result = dictionary.TryGetValue(zeta, out value);
            Assert.IsTrue(result);
            Assert.AreEqual(6, value);

            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }

        [TestMethod]
        public void Count_KeysCollected()
        {
            // Arrange
            var dictionary = new WeakKeyDictionary<StringContainer, double>();
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateDictionary(dictionary);
            dictionary.Add(zeta, 6);
            dictionary.Add(eta, 7);

            // Assert
            Assert.AreEqual(7, dictionary.Count);

            // Action
            // Garbage-collection should collect the items inserted within PopulateDictionary,
            // but the zeta and eta elements should remain in the dictionary
            GC.Collect(2, GCCollectionMode.Forced);

            // Assert
            Assert.AreEqual(2, dictionary.Count);

            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            var dictionary = new WeakKeyDictionary<StringContainer, double>();
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateDictionary(dictionary);
            dictionary.Add(zeta, 6);
            dictionary.Add(eta, 7);

            // Action
            dictionary.Clear();

            // Assert
            Assert.AreEqual(0, dictionary.Count);

            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            var dictionary = new WeakKeyDictionary<StringContainer, double>();
            var alpha = new StringContainer("alpha");
            var zeta = new StringContainer("zeta");
            var eta = new StringContainer("eta");

            PopulateDictionary(dictionary);
            dictionary.Add(zeta, 6);
            dictionary.Add(eta, 7);

            // Assert
            Assert.AreEqual(7, dictionary.Count);

            // Action
            dictionary.Remove(alpha);

            // Assert
            Assert.AreEqual(6, dictionary.Count);

            GC.KeepAlive(zeta);
            GC.KeepAlive(eta);
        }


        private static void PopulateDictionary(WeakKeyDictionary<StringContainer, double> dictionary)
        {
            dictionary.Add(new StringContainer("alpha"), 1);
            dictionary.Add(new StringContainer("beta"), 2);
            dictionary.Add(new StringContainer("gamma"), 3);
            dictionary.Add(new StringContainer("delta"), 4);
            dictionary.Add(new StringContainer("epsilon"), 5);
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
