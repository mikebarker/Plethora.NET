using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Collections.Trees;

namespace Plethora.Test.Collections.Trees;

[TestClass]
public class BinaryTreeEnumerator_Test
{
    private IKeyLimitedEnumerator<string, KeyValuePair<string, int>> enumerator;

    public BinaryTreeEnumerator_Test()
    {
        BinaryTree<string, int> tree = new BinaryTree<string, int>
                   {
                       {"Harry", 7},
                       {"Mark", 12},
                       {"Jeff", 14}
                   };

        this.enumerator = tree.GetPairEnumerator();
    }

    [TestMethod]
    public void All()
    {
        // Arrange

        // Action and Test
        int count = 0;
        while(this.enumerator.MoveNext())
        {
            int currentValue = this.enumerator.Current.Value;

            if (count == 0)
                Assert.AreEqual(7, currentValue);  //Harry
            if (count == 1)
                Assert.AreEqual(14, currentValue); //Jeff
            if (count == 2)
                Assert.AreEqual(12, currentValue); //Mark

            count++;
        }

        Assert.AreEqual(3, count);
    }

    [TestMethod]
    public void LimitMin()
    {
        // Arrange
        this.enumerator.Min = "I"; // excludes "Harry"

        // Action and Test
        int count = 0;
        while(this.enumerator.MoveNext())
        {
            int currentValue = this.enumerator.Current.Value;

            if (count == 0)
                Assert.AreEqual(14, currentValue); //Jeff
            if (count == 1)
                Assert.AreEqual(12, currentValue); //Mark

            count++;
        }

        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public void LimitMax()
    {
        // Arrange
        this.enumerator.Max = "L"; // excludes "Mark"

        // Action and Test
        int count = 0;
        while (this.enumerator.MoveNext())
        {
            int currentValue = this.enumerator.Current.Value;

            if (count == 0)
                Assert.AreEqual(7, currentValue);  //Harry
            if (count == 1)
                Assert.AreEqual(14, currentValue); //Jeff

            count++;
        }

        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public void LimitMinAndMax()
    {
        // Arrange
        this.enumerator.Min = "I"; // excludes "Harry"
        this.enumerator.Max = "L"; // excludes "Mark"

        // Action and Test
        int count = 0;
        while (this.enumerator.MoveNext())
        {
            int currentValue = this.enumerator.Current.Value;

            if (count == 0)
                Assert.AreEqual(14, currentValue); //Jeff

            count++;
        }

        Assert.AreEqual(1, count);
    }

}
