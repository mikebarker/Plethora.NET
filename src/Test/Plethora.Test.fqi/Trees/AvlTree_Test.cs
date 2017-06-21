using System;
using System.Collections.Generic;
using Plethora.fqi.Trees;
using NUnit.Framework;

namespace Plethora.Test.fqi.Trees
{
    [TestFixture]
    public class AvlTree_Test
    {
        AvlTree<string, int> tree;

        [SetUp]
        public void SetUp()
        {
            tree = new AvlTree<string, int>();
        }

        [Test]
        public void Add()
        {
            //Setup
            const string key = "Harry";
            const int value = 7;

            //Execute
            tree.Add(key, value);

            //Test
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(tree[key], value);
        }

        [Test]
        public void AddDuplicate()
        {
            //Setup
            const string key = "Harry";
            const int value = 7;

            //Execute
            tree.Add(key, value);
            Assert.Throws(typeof(ArgumentException), delegate { tree.Add(key, value + 1); });
        }

        [Test]
        public void Itterate()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Test
            Assert.AreEqual(tree.Count, 3);
            foreach (KeyValuePair<string, int> pair in tree)
            {
                Assert.IsTrue(keys.Contains(pair.Key));
                Assert.IsTrue(values.Contains(pair.Value));
            }
        }

        [Test]
        public void Clear()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            Assert.AreEqual(tree.Count, 3);

            //Execute
            tree.Clear();

            //Test
            Assert.AreEqual(tree.Count, 0);
        }

        [Test]
        public void ContainsKey()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Test
            Assert.IsTrue(tree.ContainsKey("Mark"));
        }

        [Test]
        public void Remove()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Test
            bool result = tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(tree.Count, 2);

            result = tree.Remove("Mark");
            Assert.IsFalse(result);
            Assert.AreEqual(tree.Count, 2);
        }

        [Test]
        public void TryGetValue_Exists()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            int value;
            bool result = tree.TryGetValue("Mark", out value);

            //Test
            Assert.IsTrue(result);
            Assert.AreEqual(value, 12);
        }

        [Test]
        public void TryGetValue_NotExists()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            int value;
            bool result = tree.TryGetValue("Xylophone", out value);

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

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Mark", out value, out locationInfo);

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

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Xylophone", out value, out locationInfo);

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

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            const string key = "Xylophone";
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx(key, out value, out locationInfo);

            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);

            tree.AddEx(key, 42, locationInfo);

            //Test
            Assert.AreEqual(tree[key], 42);
        }

        [Test]
        public void GetPairEnumerator()
        {
            //Setup
            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            //Execute
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

            //Test
            Assert.IsNotNull(enumerator);
        }

        [Test]
        public void AreDuplicatesAllowed()
        {
            Assert.IsFalse(tree.AreDuplicatesAllowed);
        }
    }
}
