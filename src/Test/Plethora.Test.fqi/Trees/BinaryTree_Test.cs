using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.fqi.Trees;

namespace Plethora.Test.fqi.Trees
{
    [TestFixture]
    public class BinaryTree_Test
    {
        BinaryTree<string, int> tree;

        [SetUp]
        public void SetUp()
        {
            this.tree = new BinaryTree<string, int>();
        }

        [Test]
        public void Add()
        {
            //Setup
            const string key = "Harry";
            const int value = 7;

            //Execute
            this.tree.Add(key, value);

            //Test
            Assert.AreEqual(this.tree.Count, 1);
            Assert.AreEqual(this.tree[key], value);
        }

        [Test]
        public void AddDuplicate()
        {
            //Setup
            const string key = "Harry";
            const int value = 7;

            //Execute
            this.tree.Add(key, value);
            Assert.Throws(typeof (ArgumentException), delegate { this.tree.Add(key, value + 1); });
        }

        [Test]
        public void Itterate()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Test
            Assert.AreEqual(this.tree.Count, 3);
            foreach (KeyValuePair<string, int> pair in this.tree)
            {
                Assert.IsTrue(keys.Contains(pair.Key));
                Assert.IsTrue(values.Contains(pair.Value));
            }
        }

        [Test]
        public void Clear()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            Assert.AreEqual(this.tree.Count, 3);

            //Execute
            this.tree.Clear();

            //Test
            Assert.AreEqual(this.tree.Count, 0);
        }

        [Test]
        public void ContainsKey()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Test
            Assert.IsTrue(this.tree.ContainsKey("Mark"));
        }

        [Test]
        public void Remove()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Test
            bool result = this.tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(this.tree.Count, 2);

            result = this.tree.Remove("Mark");
            Assert.IsFalse(result);
            Assert.AreEqual(this.tree.Count, 2);
        }

        [Test]
        public void TryGetValue_Exists()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            int value;
            bool result = this.tree.TryGetValue("Mark", out value);

            //Test
            Assert.IsTrue(result);
            Assert.AreEqual(value, 12);
        }

        [Test]
        public void TryGetValue_NotExists()
        {
            //Setup
            IList<string> keys = new List<string> {"Harry", "Mark", "Jeff"};
            IList<int> values = new List<int> {7, 12, 14};

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            int value;
            bool result = this.tree.TryGetValue("Xylophone", out value);

            //Test
            Assert.IsFalse(result);
            Assert.AreEqual(value, default(int));
        }

        [Test]
        public void TryGetValueEx_Exists()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx("Mark", out value, out locationInfo);

            //Test
            Assert.IsTrue(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, 12);
        }

        [Test]
        public void TryGetValueEx_NotExists()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx("Xylophone", out value, out locationInfo);

            //Test
            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, default(int));
        }

        [Test]
        public void TryGetValueEx_AddEx()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            const string key = "Xylophone";
            int value;
            object locationInfo;
            bool result = this.tree.TryGetValueEx(key, out value, out locationInfo);

            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);

            this.tree.AddEx(key, 42, locationInfo);

            //Test
            Assert.AreEqual(this.tree[key], 42);
        }

        [Test]
        public void GetValueEnumerator()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            this.tree.Add(keys[0], values[0]);
            this.tree.Add(keys[1], values[1]);
            this.tree.Add(keys[2], values[2]);

            //Execute
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = this.tree.GetPairEnumerator();

            //Test
            Assert.IsNotNull(enumerator);
        }

        [Test]
        public void AreDuplicatesAllowed()
        {
            Assert.IsFalse(this.tree.AreDuplicatesAllowed);
        }
    }
}
