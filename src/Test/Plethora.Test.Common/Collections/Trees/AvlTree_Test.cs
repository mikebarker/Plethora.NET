using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestClass]
    public class AvlTree_Test
    {
        [TestMethod]
        public void Add()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            // Action
            tree.Add(key, value);

            // Assert
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(tree[key], value);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void Add_Multiple()
        {
            // Arrange
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertAvlTreeRules(tree);
            }
        }

        [TestMethod]
        public void Add_MultipleReverse()
        {
            // Arrange
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.Reverse();

            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertAvlTreeRules(tree);
            }
        }

        [TestMethod]
        public void Add_MultipleRandomOrder()
        {
            // Arrange
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.
            AvlTree<int, int> tree = new AvlTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.OrderBy(x => rand.Next());

            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertAvlTreeRules(tree);
            }
        }

        [TestMethod]
        public void AddDuplicate()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            tree.Add(key, value);

            // Action
            try
            {
                tree.Add(key, value + 1);

                Assert.Fail();
            }
            catch(ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Itterate()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            Assert.AreEqual(tree.Count, 3);
            foreach (KeyValuePair<string, int> pair in tree)
            {
                Assert.IsTrue(keys.Contains(pair.Key));
                Assert.IsTrue(values.Contains(pair.Value));
            }

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            Assert.AreEqual(tree.Count, 3);

            // Action
            tree.Clear();

            // Assert
            Assert.AreEqual(tree.Count, 0);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void ContainsKey()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            Assert.IsTrue(tree.ContainsKey("Mark"));

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            bool result = tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(tree.Count, 2);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void Remove_DoesNotExist()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            bool result = tree.Remove("Fred");
            Assert.IsFalse(result);
            Assert.AreEqual(tree.Count, 3);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void Remove_Multiple()
        {
            // Arrange
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

        [TestMethod]
        public void Remove_MultipleRandomOrder()
        {
            // Arrange
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

        [TestMethod]
        public void TryGetValue_Exists()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            int value;
            bool result = tree.TryGetValue("Mark", out value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, 12);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValue_NotExists()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            int value;
            bool result = tree.TryGetValue("Xylophone", out value);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(value, default(int));

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_Exists()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Mark", out value, out locationInfo);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, 12);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_NotExists()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx("Xylophone", out value, out locationInfo);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);
            Assert.AreEqual(value, default(int));

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_AddEx()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            const string key = "Xylophone";
            int value;
            object locationInfo;
            bool result = tree.TryGetValueEx(key, out value, out locationInfo);

            Assert.IsFalse(result);
            Assert.IsNotNull(locationInfo);

            tree.AddEx(key, 42, locationInfo);

            // Assert
            Assert.AreEqual(tree[key], 42);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void GetPairEnumerator()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

            // Assert
            Assert.IsNotNull(enumerator);

            AssertAvlTreeRules(tree);
        }

        [TestMethod]
        public void AreDuplicatesAllowed()
        {
            // Arrange
            AvlTree<string, int> tree = new AvlTree<string, int>();

            // Assert
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
