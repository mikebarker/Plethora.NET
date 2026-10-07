using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees;

[TestClass]
public class BinaryTree_Test
{
    [TestMethod]
    public void Add()
    {
        // Arrange
        BinaryTree<string, int> tree = new();

        const string key = "Harry";
        const int value = 7;

        // Action
        tree.Add(key, value);

        // Assert
        Assert.HasCount(1, tree);
        Assert.AreEqual(value, tree[key]);
    }

    [TestMethod]
    public void AddDuplicate()
    {
        // Arrange
        BinaryTree<string, int> tree = new();

        const string key = "Harry";
        const int value = 7;

        // Action
        tree.Add(key, value);
        Assert.Throws<ArgumentException>(() => tree.Add(key, value + 1));
    }

    [TestMethod]
    public void Itterate()
    {
        // Arrange
        BinaryTree<string, int> tree = new();

        List<string> keys = ["Harry", "Mark", "Jeff"];
        List<int> values = [7, 12, 14];

        tree.Add(keys[0], values[0]);
        tree.Add(keys[1], values[1]);
        tree.Add(keys[2], values[2]);

        // Assert
        Assert.HasCount(3, tree);
        foreach (KeyValuePair<string, int> pair in tree)
        {
            Assert.Contains(pair.Key, keys);
            Assert.Contains(pair.Value, values);
        }
    }

    [TestMethod]
    public void Clear()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        Assert.HasCount(3, tree);

        // Action
        tree.Clear();

        // Assert
        Assert.IsEmpty(tree);
    }

    [TestMethod]
    public void ContainsKey()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Assert
        Assert.IsTrue(tree.ContainsKey("Mark"));
    }

    [TestMethod]
    public void Remove()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Assert
        bool result = tree.Remove("Mark");
        Assert.IsTrue(result);
        Assert.HasCount(2, tree);

        result = tree.Remove("Mark");
        Assert.IsFalse(result);
        Assert.HasCount(2, tree);
    }

    [TestMethod]
    public void TryGetValue_Exists()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        int value;
        bool result = tree.TryGetValue("Mark", out value);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(12, value);
    }

    [TestMethod]
    public void TryGetValue_NotExists()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        int value;
        bool result = tree.TryGetValue("Xylophone", out value);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(default, value);
    }

    [TestMethod]
    public void TryGetValueEx_Exists()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        int value;
        object locationInfo;
        bool result = tree.TryGetValueEx("Mark", out value, out locationInfo);

        // Assert
        Assert.IsTrue(result);
        Assert.IsNotNull(locationInfo);
        Assert.AreEqual(12, value);
    }

    [TestMethod]
    public void TryGetValueEx_NotExists()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        int value;
        object locationInfo;
        bool result = tree.TryGetValueEx("Xylophone", out value, out locationInfo);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNotNull(locationInfo);
        Assert.AreEqual(default(int), value);
    }

    [TestMethod]
    public void TryGetValueEx_AddEx()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        const string key = "Xylophone";
        int value;
        object locationInfo;
        bool result = tree.TryGetValueEx(key, out value, out locationInfo);

        Assert.IsFalse(result);
        Assert.IsNotNull(locationInfo);

        tree.AddEx(key, 42, locationInfo);

        // Assert
        Assert.AreEqual(42, tree[key]);
    }

    [TestMethod]
    public void GetValueEnumerator()
    {
        // Arrange
        BinaryTree<string, int> tree = new()
        {
            { "Harry", 7 },
            { "Mark", 12 },
            { "Jeff", 14 },
        };

        // Action
        IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator = tree.GetPairEnumerator();

        // Assert
        Assert.IsNotNull(enumerator);
    }

    [TestMethod]
    public void AreDuplicatesAllowed()
    {
        BinaryTree<string, int> tree = new();

        Assert.IsFalse(tree.AreDuplicatesAllowed);
    }
}
