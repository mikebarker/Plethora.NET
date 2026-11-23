using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections;

[TestClass]
public class SubList_Test
{
    private readonly List<int> list = Enumerable.Range(0, 10).ToList();

    [TestMethod]
    public void ctor_Fail_NullList()
    {
        Assert.Throws<ArgumentNullException>(() => new SubList<int>(null, 3, 4));
    }

    [TestMethod]
    public void ctor_Fail_StartLessThan0()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubList<int>(list, -2, 4));
    }

    [TestMethod]
    public void ctor_Fail_CountLessThan0()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SubList<int>(list, 3, -2));
    }

    [TestMethod]
    public void ctor_Fail_CountPlusStartGreaterThanListLength()
    {
        Assert.Throws<ArgumentException>(() => new SubList<int>(list, 3, 20));
    }

    [TestMethod]
    public void Empty()
    {
        //Exec
        SubList<int> subList = new(list, 3, 0);

        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void SingleElement()
    {
        //Exec
        SubList<int> subList = new(list, 3, 1);

        // Assert
        Assert.HasCount(1, subList);

        bool areEqual = subList.SequenceEqual([3]);
        Assert.IsTrue(areEqual);
    }

    [TestMethod]
    public void MultipleElements()
    {
        //Exec
        SubList<int> subList = new(list, 3, 4);

        // Assert
        Assert.HasCount(4, subList);

        bool areEqual = subList.SequenceEqual([3, 4, 5, 6]);
        Assert.IsTrue(areEqual);
    }

    [TestMethod]
    public void Contains_True()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        bool result = subList.Contains(5);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Contains_False()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        bool result = subList.Contains(1);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Contains_EdgeCases()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        bool beforeStart = subList.Contains(2);
        bool atStart = subList.Contains(3);
        bool atEnd = subList.Contains(6);
        bool afterEnd = subList.Contains(7);


        // Assert
        Assert.IsFalse(beforeStart);
        Assert.IsTrue(atStart);
        Assert.IsTrue(atEnd);
        Assert.IsFalse(afterEnd);
    }

    [TestMethod]
    public void CopyTo_ZeroIndex()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        int[] array = new int[10];
        subList.CopyTo(array, 0);

        // Assert
        Assert.AreEqual(3, array[0]);
        Assert.AreEqual(4, array[1]);
        Assert.AreEqual(5, array[2]);
        Assert.AreEqual(6, array[3]);
    }

    [TestMethod]
    public void CopyTo_NonZeroIndex()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        int[] array = new int[10];
        subList.CopyTo(array, 5);

        // Assert
        Assert.AreEqual(3, array[5]);
        Assert.AreEqual(4, array[6]);
        Assert.AreEqual(5, array[7]);
        Assert.AreEqual(6, array[8]);
    }

    [TestMethod]
    public void Count()
    {
        // Arrange
        SubList<int> subList = new(list, 3, 4);

        //Exec
        var count = subList.Count;

        // Assert
        Assert.AreEqual(4, count);
    }
}
