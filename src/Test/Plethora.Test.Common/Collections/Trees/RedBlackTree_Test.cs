using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestClass]
    public class RedBlackTree_Test
    {
        [TestMethod]
        public void Add()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            // Action
            tree.Add(key, value);

            // Assert
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(tree[key], value);

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void Add_Multiple()
        {
            // Arrange
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertRedBlackTreeRules(tree);
            }
        }

        [TestMethod]
        public void Add_MultipleReverse()
        {
            // Arrange
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.Reverse();

            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertRedBlackTreeRules(tree);
            }
        }

        [TestMethod]
        public void Add_MultipleRandomOrder()
        {
            // Arrange
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.OrderBy(x => rand.Next());

            foreach (int value in values)
            {
                // Action
                tree.Add(value, value);

                // Assert
                AssertRedBlackTreeRules(tree);
            }
        }

        [TestMethod]
        public void AddDuplicate()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void ContainsKey()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            Assert.IsTrue(tree.ContainsKey("Mark"));

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            bool result = tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(tree.Count, 2);

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void Remove_DoesNotExist()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Assert
            bool result = tree.Remove("Fred");
            Assert.IsFalse(result);
            Assert.AreEqual(tree.Count, 3);

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void Remove_Multiple()
        {
            // Arrange
            const int count = 100;

            IEnumerable<int> values = Enumerable.Range(1, count);
            foreach (int deleteValue in values)
            {
                RedBlackTree<int, int> tree = new RedBlackTree<int, int>();
                foreach (int value in values)
                {
                    tree.Add(value, value);
                }

                bool result = tree.Remove(deleteValue);

                Assert.IsTrue(result);
                Assert.AreEqual(tree.Count, count - 1);
                Assert.IsFalse(tree.ContainsKey(deleteValue));
                AssertRedBlackTreeRules(tree);
            }
        }

        [TestMethod]
        public void Remove_MultipleRandomOrder()
        {
            // Arrange
            const int count = 100;
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.

            IEnumerable<int> values = Enumerable.Range(1, count);
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();
            foreach (int value in values)
            {
                tree.Add(value, value);
            }

            foreach (int deleteValue in values.OrderBy(x => rand.Next()))
            {
                tree.Remove(deleteValue);

                AssertRedBlackTreeRules(tree);
            }
        }

        [TestMethod]
        public void TryGetValue_Exists()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValue_NotExists()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_Exists()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_NotExists()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void TryGetValueEx_AddEx()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void GetPairEnumerator()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // Action
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

            // Assert
            Assert.IsNotNull(enumerator);

            AssertRedBlackTreeRules(tree);
        }

        [TestMethod]
        public void AreDuplicatesAllowed()
        {
            // Arrange
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

        private static string GetColor<TKey, TValue>(BinaryTree<TKey, TValue>.Node node)
        {
            Type nodeType = node.GetType();
            PropertyInfo colorProperty = nodeType.GetProperty("Color", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object objColor = colorProperty.GetValue(node);

            string color = objColor.ToString();
            return color;
        }

        private static void AssertRedBlackTreeRules<TKey, TValue>(RedBlackTree<TKey, TValue> tree)
        {
            var root = tree.Root;
            if (root == null)
                return;


            AssertBinary(root, GetComparer(tree));

            // 1. Each node is red or black

            // 2. Root is black
            Assert.AreEqual(Black, GetColor(root), "Root is not Black.");

            // 3. All leaves are black

            // 4. If a node is red, then both its children are black
            AssertIfANodeIsRedThenItsChildrenAreBlack(root);

            // 5. Every path from a given node to any of its leaf nodes contains the same number of black nodes
            AssertBlackHeight(root);
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

        private static void AssertIfANodeIsRedThenItsChildrenAreBlack<TKey, TValue>(BinaryTree<TKey, TValue>.Node node)
        {
            if (GetColor(node) == Red)
            {
                var left = node.Left;
                bool isLeftBlack = (left == null) || (GetColor(left) == Black);

                Assert.IsTrue(isLeftBlack, "The left node is not black for node [key = {0}]", node.Key);

                var right = node.Right;
                bool isRightBlack = (right == null) || (GetColor(right) == Black);

                Assert.IsTrue(isRightBlack, "The right node is not black for node [key = {0}]", node.Key);
            }

            if (node.Left != null)
                AssertIfANodeIsRedThenItsChildrenAreBlack(node.Left);

            if (node.Right != null)
                AssertIfANodeIsRedThenItsChildrenAreBlack(node.Right);
        }

        private static int AssertBlackHeight<TKey, TValue>(BinaryTree<TKey, TValue>.Node node)
        {
            int leftBlackHeight = (node.Left == null)
                ? 1
                : AssertBlackHeight(node.Left);

            int rightBlackHeight = (node.Right == null)
                ? 1
                : AssertBlackHeight(node.Right);


            Assert.IsTrue(leftBlackHeight == rightBlackHeight, "The left and right black-heights are not equal for node [key = {0}]", node.Key);

            int thisBlackHeight = leftBlackHeight;
            if (GetColor(node) == Black)
                thisBlackHeight += 1;

            return thisBlackHeight;
        }


        private const string Black = "Black";
        private const string Red = "Red";

        #endregion
    }
}
