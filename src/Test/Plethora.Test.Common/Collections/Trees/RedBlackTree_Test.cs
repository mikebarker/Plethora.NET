using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees
{
    [TestFixture]
    public class RedBlackTree_Test
    {
        [Test]
        public void Add()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            const string key = "Harry";
            const int value = 7;

            // exec
            tree.Add(key, value);

            // test
            Assert.AreEqual(tree.Count, 1);
            Assert.AreEqual(tree[key], value);

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void Add_Multiple()
        {
            // setup
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertRedBlackTreeRules(tree);
            }
        }

        [Test]
        public void Add_MultipleReverse()
        {
            // setup
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.Reverse();

            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertRedBlackTreeRules(tree);
            }
        }

        [Test]
        public void Add_MultipleRandomOrder()
        {
            // setup
            Random rand = new Random(1778); // has no real-world value but makes tests repeatable.
            RedBlackTree<int, int> tree = new RedBlackTree<int, int>();

            IEnumerable<int> values = Enumerable.Range(1, 100);
            values = values.OrderBy(x => rand.Next());

            foreach (int value in values)
            {
                // exec
                tree.Add(value, value);

                // test
                AssertRedBlackTreeRules(tree);
            }
        }

        [Test]
        public void AddDuplicate()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void Clear()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void ContainsKey()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            Assert.IsTrue(tree.ContainsKey("Mark"));

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void Remove()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            bool result = tree.Remove("Mark");
            Assert.IsTrue(result);
            Assert.AreEqual(tree.Count, 2);

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void Remove_DoesNotExist()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // test
            bool result = tree.Remove("Fred");
            Assert.IsFalse(result);
            Assert.AreEqual(tree.Count, 3);

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void Remove_Multiple()
        {
            // setup
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

        [Test]
        public void Remove_MultipleRandomOrder()
        {
            // setup
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

        [Test]
        public void TryGetValue_Exists()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void TryGetValue_NotExists()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_Exists()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_NotExists()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void TryGetValueEx_AddEx()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void GetPairEnumerator()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

            IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
            IList<int> values = new List<int> { 7, 12, 14 };

            tree.Add(keys[0], values[0]);
            tree.Add(keys[1], values[1]);
            tree.Add(keys[2], values[2]);

            // exec
            IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

            // test
            Assert.IsNotNull(enumerator);

            AssertRedBlackTreeRules(tree);
        }

        [Test]
        public void AreDuplicatesAllowed()
        {
            // setup
            RedBlackTree<string, int> tree = new RedBlackTree<string, int>();

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
