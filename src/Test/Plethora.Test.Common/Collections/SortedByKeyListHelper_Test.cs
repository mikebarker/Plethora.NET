using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections;

[TestClass]
public class SortedByKeyListHelper_Test
{
    [TestMethod]
    public void GetByKey_EmptyList()
    {
        // Arrange
        var list = CreateEmptyList();
        var date = new DateTime(2000, 01, 04);

        //Exec
        IEnumerable<Price> subList = list.GetByKey(date);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByKey_UniqueList()
    {
        // Arrange
        var list = CreateUniqueList();
        var date = new DateTime(2000, 01, 04);

        //Exec
        IEnumerable<Price> subList = list.GetByKey(date);


        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
    }

    [TestMethod]
    public void GetByKey_UniqueList_NotInRange()
    {
        // Arrange
        var list = CreateUniqueList();
        var date = new DateTime(2100, 01, 04);

        //Exec
        IEnumerable<Price> subList = list.GetByKey(date);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByKey_DuplicateList_MultipleResults()
    {
        // Arrange
        var list = CreateDuplicateList();
        var date = new DateTime(2000, 01, 04);

        //Exec
        IEnumerable<Price> subList = list.GetByKey(date);


        // Assert
        Assert.HasCount(2, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(1).DateTime);
    }


    [TestMethod]
    public void GetByRange_EmptyList_Inclusive()
    {
        // Arrange
        var list = CreateEmptyList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2000, 01, 07), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_EmptyList_Exclusive()
    {
        // Arrange
        var list = CreateEmptyList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2000, 01, 07), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_UniqueList_Inclusive()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2000, 01, 07), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(3, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
    }

    [TestMethod]
    public void GetByRange_UniqueList_Exclusive()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2000, 01, 07), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);

        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
    }

    [TestMethod]
    public void GetByRange_DuplicateList_Inclusive()
    {
        // Arrange
        var list = CreateDuplicateList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2000, 01, 07), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(5, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(3).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(4).DateTime);
    }

    [TestMethod]
    public void GetByRange_DuplicateList_Exclusive()
    {
        // Arrange
        var list = CreateDuplicateList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2000, 01, 07), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);

        // Assert
        Assert.HasCount(2, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
    }


    [TestMethod]
    public void GetByRange_Inclusive_MinUnavailable()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 05), true, new DateTime(2000, 01, 07), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(2, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MaxUnavailable()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2000, 01, 05), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MinBeforeStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(1900, 01, 01), true, new DateTime(2000, 01, 07), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(6, subList);

        Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(5).DateTime);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MaxBeforeStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(1900, 01, 01), true, new DateTime(1990, 12, 30), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MinPastEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2010, 01, 04), true, new DateTime(2100, 01, 01), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MaxPastEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2100, 01, 01), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(4, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(3).DateTime);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MaxAtEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), true, new DateTime(2000, 01, 08), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(4, subList);

        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(3).DateTime);
    }

    [TestMethod]
    public void GetByRange_Inclusive_MinAtStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 01), true, new DateTime(2000, 01, 08), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(7, subList);

        Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(5).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(6).DateTime);
    }


    [TestMethod]
    public void GetByRange_Exclusive_MinUnavailable()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 05), false, new DateTime(2000, 01, 07), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MaxUnavailable()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2000, 01, 05), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MinBeforeStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(1900, 01, 01), false, new DateTime(2000, 01, 07), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(5, subList);

        Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(3).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(4).DateTime);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MaxBeforeStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(1900, 01, 01), false, new DateTime(1990, 12, 30), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MinPastEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2010, 01, 04), false, new DateTime(2100, 01, 01), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.IsEmpty(subList);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MaxPastEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2100, 01, 01), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(3, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(2).DateTime);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MaxAtEnd()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 04), false, new DateTime(2000, 01, 08), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(2, subList);

        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(1).DateTime);
    }

    [TestMethod]
    public void GetByRange_Exclusive_MinAtStart()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 01), false, new DateTime(2000, 01, 08), false);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(5, subList);

        Assert.AreEqual(new DateTime(2000, 01, 02), subList.ElementAt(0).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 03), subList.ElementAt(1).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 04), subList.ElementAt(2).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 06), subList.ElementAt(3).DateTime);
        Assert.AreEqual(new DateTime(2000, 01, 07), subList.ElementAt(4).DateTime);
    }


    
    [TestMethod]
    public void GetByRange_FirstElement()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 01), true, new DateTime(2000, 01, 01), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 01), subList.ElementAt(0).DateTime);
    }

    [TestMethod]
    public void GetByRange_LastElement()
    {
        // Arrange
        var list = CreateUniqueList();
        var range = new Range<DateTime>(new DateTime(2000, 01, 08), true, new DateTime(2000, 01, 08), true);

        //Exec
        IEnumerable<Price> subList = list.GetByRange(range);


        // Assert
        Assert.HasCount(1, subList);

        Assert.AreEqual(new DateTime(2000, 01, 08), subList.ElementAt(0).DateTime);
    }


    #region Factory Methods

    private static SortedByKeyList<DateTime, Price> CreateEmptyList()
    {
        return new(p => p.DateTime, DuplicatesPolicy.Error, Comparer<DateTime>.Default);
    }

    private static SortedByKeyList<DateTime, Price> CreateUniqueList()
    {
        return new(p => p.DateTime, DuplicatesPolicy.Error, Comparer<DateTime>.Default)
        {
            new(new DateTime(2000, 01, 01)),
            new(new DateTime(2000, 01, 02)),
            new(new DateTime(2000, 01, 03)),
            new(new DateTime(2000, 01, 04)),
            //Nothing on the Fifth
            new(new DateTime(2000, 01, 06)),
            new(new DateTime(2000, 01, 07)),
            new(new DateTime(2000, 01, 08))
        };
    }

    private static SortedByKeyList<DateTime, Price> CreateDuplicateList()
    {
        return new(p => p.DateTime, DuplicatesPolicy.Allow, Comparer<DateTime>.Default)
        {
            new(new DateTime(2000, 01, 01)),
            new(new DateTime(2000, 01, 02)),
            new(new DateTime(2000, 01, 03)),
            new(new DateTime(2000, 01, 04)),
            new(new DateTime(2000, 01, 04)),
            //Nothing on the Fifth
            new(new DateTime(2000, 01, 06)),
            new(new DateTime(2000, 01, 06)),
            new(new DateTime(2000, 01, 07)),
            new(new DateTime(2000, 01, 08))
        };
    }
    #endregion

    #region Classes
    
    private class Price(DateTime dateTime)
    {
        public DateTime DateTime
        {
            get { return dateTime; }
        }
    }
    
    #endregion
}
