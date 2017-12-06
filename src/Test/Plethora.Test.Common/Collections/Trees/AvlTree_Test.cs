using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestFixture]
    public class AvlTree_Test
    {
        [Test]
        public void Add()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            // exec
            tree.Add(key, value);

            // test
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(tree[key], value);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void Add_Multiple()
        {
            // setup
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertAvlTreeRules(tree);
            }
        }

        [Test]
        public void Add_MultipleReverse()
        {
            // setup
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.Reverse();

            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertAvlTreeRules(tree);
            }
        }

        [Test]
        public void Add_MultipleRandomOrder()
        {
            // setup
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.OrderBy(x => rand.Next());

            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertAvlTreeRules(tree);
            }
        }

        [Test]
        public void AddDuplicate()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            tree.Add(key, value);

            // exec
            try
            {
                tree.Add(key, value + 1);

                Assert.Fail();
            }
            catch(ArgumentException)
            {
            }
        }

        [Test]
        public void Itterate()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            Assert.AreEqual(tree.Count, 3);
            foreach (KeyValuePair<string, int> pair in tree)
            {
                Assert.IsTrue(keys.Contains(pair.Key));
                Assert.IsTrue(values.Contains(pair.Value));
            }

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void Clear()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            Assert.AreEqual(tree.Count, 3);

            // exec
            tree.Clear();

            // test
            Assert.AreEqual(tree.Count, 0);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void ContainsKey()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            Assert.IsTrue(tree.ContainsKey("Mark"));

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void Remove()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            bool result = tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(tree.Count, 2);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void Remove_DoesNotExist()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            bool result = tree.Remove("Fred");
            Assert.IsFalse(result);
            Assert.AreEqual(tree.Count, 3);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void Remove_Multiple()
        {
            // setup
            const int count = 100;

            IEnumerable<int> values = Enumerable.Range(1, count);
            foreach (int deleteValue in values)
            {
                AvlTree<int, int> tree = new AvlTree<int, int>();
                foreach (int value in values)
                {
                    tree.Add(value, value);
                }

                bool result = tree.Remove(deleteValue);

                Assert.IsTrue(result);
                Assert.AreEqual(tree.Count, count - 1);
                Assert.IsFalse(tree.ContainsKey(deleteValue));
                AssertAvlTreeRules(tree);
            }
        }

        [Test]
        public void Remove_MultipleRandomOrder()
        {
            // setup
            const int count = 100;
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.

            IEnumerable<int> values = Enumerable.Range(1, count);
            AvlTree<int, int> tree = new AvlTree<int, int>();
            foreach (int value in values)
            {
                tree.Add(value, value);
            }

            foreach (int deleteValue in values.OrderBy(x => rand.Next()))
            {
                tree.Remove(deleteValue);

                AssertAvlTreeRules(tree);
            }
        }

        [Test]
        public void TryGetValue_Exists()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            int value;
            bool result = tree.TryGetValue("Mark", out value);

            // test
            Assert.IsTrue(result);
            Assert.AreEqual(value, 12);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void TryGetValue_NotExists()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            int value;
            bool result = tree.TryGetValue("Xylophone", out value);

            // test
            Assert.IsFalse(result);
            Assert.AreEqual(value, default(int));

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_Exists()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Mark", out value, out locationInfo);

            // test
            Assert.IsTrue(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, 12);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_NotExists()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Xylophone", out value, out locationInfo);

            // test
            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, default(int));

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_AddEx()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            const string key = "Xylophone";
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx(key, out value, out locationInfo);

            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);

            tree.AddEx(key, 42, locationInfo);

            // test
            Assert.AreEqual(tree[key], 42);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void GetPairEnumerator()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

            // test
            Assert.IsNotNull(enumerator);

            AssertAvlTreeRules(tree);
        }

        [Test]
        public void AreDuplicatesAllowed()
        {
            // setup
            AvlTree<string, int> tree = new AvlTree<string, int>();

            // test
            Assert.IsFalse(tree.AreDuplicatesAllowed);
        }

        #region

        private static IComparer<TKey> GetComparer<TKey, TValue>(BinaryTree<TKey, TValue> tree)
        {
            Type treeType = typeof(BinaryTree<TKey, TValue>);
            FieldInfo comparerField = treeType.GetField("comparer", BindingFlags.NonPublic | BindingFlags.Instance);

            object objComparer = comparerField.GetValue(tree);

            IComparer<TKey> comparer = (IComparer<TKey>)objComparer;
            return comparer;
        }

        private static void AssertAvlTreeRules<TKey, TValue>(AvlTree<TKey, TValue> tree)
        {
            var root = tree.Root;
            if (root == null)
                return;

            AssertBinary(root, GetComparer(tree));

            AssertBalanceFactor(root);
        }

        private static void AssertBinary<TKey, TValue>(BinaryTree<TKey, TValue>.Node node, IComparer<TKey> comparer)
        {
            if (node.Left != null)
            {
                int result = comparer.Compare(node.Left.Key, node.Key);
                Assert.IsTrue(result < 0);

                AssertBinary(node.Left, comparer);
            }

            if (node.Right != null)
            {
                int result = comparer.Compare(node.Key, node.Right.Key);
                Assert.IsTrue(result < 0);

                AssertBinary(node.Right, comparer);
            }
        }

        private static int AssertBalanceFactor<TKey, TValue>(BinaryTree<TKey, TValue>.Node node)
        {
            int leftHeight = (node.Left == null)
                ? -1
                : AssertBalanceFactor<TKey, TValue>(node.Left);

            int rightHeight = (node.Right == null)
                ? -1
                : AssertBalanceFactor<TKey, TValue>(node.Right);


            int balanceFactor = rightHeight - leftHeight;

            bool isNearBalanced = (-1 <= balanceFactor) && (balanceFactor <= 1);
            Assert.IsTrue(isNearBalanced, "The node [key = {0}] is not balanced.", node.Key);

            int height = Math.Max(leftHeight, rightHeight) + 1;
            Assert.AreEqual(node.Height, height);

            return height;
        }

        #endregion
    }
}
