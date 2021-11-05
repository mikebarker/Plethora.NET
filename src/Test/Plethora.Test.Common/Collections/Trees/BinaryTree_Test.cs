using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestClass]
    public class BinaryTree_Test
    {
        private readonly BinaryTree<string, int> tree = new BinaryTree<string, int>();

        [TestMethod]
        public void Add()
        {
            // Arrange
            const string key = "Harry";
            const int value = 7;

            // Action
            this.tree.Add(key, value);

            // Assert
            Assert.AreEqual(this.tree.Count, 1);
            Assert.AreEqual(this.tree[key], value);
        }

        [TestMethod]
        public void AddDuplicate()
        {
            // Arrange
            const string key = "Harry";
            const int value = 7;

            // Action
            this.tree.Add(key, value);
            Assert.ThrowsException<ArgumentException>(() => this.tree.Add(key, value + 1));
        }

        [TestMethod]
        public void Itterate()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Assert
            Assert.AreEqual(this.tree.Count, 3);
            foreach (KeyValuePair<string, int> pair in this.tree)
            {
                Assert.IsTrue(keys.Contains(pair.Key));
                Assert.IsTrue(values.Contains(pair.Value));
            }
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            Assert.AreEqual(this.tree.Count, 3);

            // Action
            this.tree.Clear();

            // Assert
            Assert.AreEqual(this.tree.Count, 0);
        }

        [TestMethod]
        public void ContainsKey()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Assert
            Assert.IsTrue(this.tree.ContainsKey("Mark"));
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Assert
            bool result = this.tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(this.tree.Count, 2);

            result = this.tree.Remove("Mark");
            Assert.IsFalse(result);
            Assert.AreEqual(this.tree.Count, 2);
        }

        [TestMethod]
        public void TryGetValue_Exists()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            int value;
            bool result = this.tree.TryGetValue("Mark", out value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, 12);
        }

        [TestMethod]
        public void TryGetValue_NotExists()
        {
            // Arrange
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            int value;
            bool result = this.tree.TryGetValue("Xylophone", out value);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(value, default(int));
        }

        [TestMethod]
        public void TryGetValueEx_Exists()
        {
            // Arrange
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx("Mark", out value, out locationInfo);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, 12);
        }

        [TestMethod]
        public void TryGetValueEx_NotExists()
        {
            // Arrange
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx("Xylophone", out value, out locationInfo);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, default(int));
        }

        [TestMethod]
        public void TryGetValueEx_AddEx()
        {
            // Arrange
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            const string key = "Xylophone";
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx(key, out value, out locationInfo);

            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);

            this.tree.AddEx(key, 42, locationInfo);

            // Assert
            Assert.AreEqual(this.tree[key], 42);
        }

        [TestMethod]
        public void GetValueEnumerator()
        {
            // Arrange
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            // Action
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = this.tree.GetPairEnumerator();

            // Assert
            Assert.IsNotNull(enumerator);
        }

        [TestMethod]
        public void AreDuplicatesAllowed()
        {
            Assert.IsFalse(this.tree.AreDuplicatesAllowed);
        }
    }
}
