using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;
using Plethora.fqi.Trees;

namespace Plethora.Test.fqi.Trees;

[TestClass]
public class AvlMultiTree_Test
{
    AvlMultiTree<string, int> tree;

    [TestInitialize]
    public void SetUp()
    {
        this.tree = new();
    }

    [TestMethod]
    public void Add()
    {
        // Arrange
        const string key = "Harry";
        const int value = 7;

        // Action
        this.tree.Add(key, value);

        // Assert
        Assert.AreEqual(1, this.tree.Count);
        Assert.AreEqual(value, this.tree[key]);
    }

    [TestMethod]
    public void AddDuplicate()
    {
        // Arrange
        const string key = "Harry";
        const int value = 7;

        // Action
        this.tree.Add(key, value);
        this.tree.Add(key, value + 1);
    }

    [TestMethod]
    public void Itterate()
    {
        // Arrange
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

        this.tree.Add(keys[0], values[0]);
        this.tree.Add(keys[1], values[1]);
        this.tree.Add(keys[2], values[2]);

        // Assert
        Assert.AreEqual(3, this.tree.Count);
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
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

        this.tree.Add(keys[0], values[0]);
        this.tree.Add(keys[1], values[1]);
        this.tree.Add(keys[2], values[2]);

        Assert.AreEqual(3, this.tree.Count);

        // Action
        this.tree.Clear();

        // Assert
        Assert.AreEqual(0, this.tree.Count);
    }

    [TestMethod]
    public void ContainsKey()
    {
        // Arrange
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

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
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

        this.tree.Add(keys[0], values[0]);
        this.tree.Add(keys[1], values[1]);
        this.tree.Add(keys[2], values[2]);

        // Assert
        bool result = this.tree.Remove("Mark");
        Assert.IsTrue(result);
        Assert.AreEqual(2, this.tree.Count);

        result = this.tree.Remove("Mark");
        Assert.IsFalse(result);
        Assert.AreEqual(2, this.tree.Count);
    }

    [TestMethod]
    public void TryGetValue_Exists()
    {
        // Arrange
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

        this.tree.Add(keys[0], values[0]);
        this.tree.Add(keys[1], values[1]);
        this.tree.Add(keys[2], values[2]);

        // Action
        int value;
        bool result = this.tree.TryGetValue("Mark", out value);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(12, value);
    }

    [TestMethod]
    public void TryGetValue_NotExists()
    {
        // Arrange
        IList<string> keys = new List<string> { "Harry", "Mark", "Jeff" };
        IList<int> values = new List<int> { 7, 12, 14 };

        this.tree.Add(keys[0], values[0]);
        this.tree.Add(keys[1], values[1]);
        this.tree.Add(keys[2], values[2]);

        // Action
        int value;
        bool result = this.tree.TryGetValue("Xylophone", out value);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(default(int), value);
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
        Assert.AreEqual(12, value);
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
        Assert.AreEqual(default(int), value);
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
        Assert.AreEqual(42, this.tree[key]);
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
        Assert.IsTrue(this.tree.AreDuplicatesAllowed);
    }
}
