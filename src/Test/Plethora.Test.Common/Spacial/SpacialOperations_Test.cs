using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;
using Plethora.Spacial;
using Plethora.Test.UtilityClasses;
using System;
using System.Linq;

namespace Plethora.Test.Spacial;

[TestClass]
public class SpacialOperations_Test
{
    [TestMethod]
    public void SpacialRegion_Subtract_CompleteDimensionRemoved()
    {
        // Arrange
        var regionA = new SpaceRegion(
            new InclusiveSet<long>(1, 2, 3, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Jan01, Dates.Dec31)));

        var regionB = new SpaceRegion(
            new InclusiveSet<long>(2, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Jan01, Dates.Dec31)));

        // Action
        var results = SpacialOperations.Subtract(regionA, regionB);

        //Assert
        Assert.HasCount(1, results);

        SpaceRegion region = results.ElementAt(0);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(1, 3), new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Dec31)),
            region);
    }

    [TestMethod]
    public void SpacialRegion_Subtract_PartialDimensionsRemain_2()
    {
        // Arrange
        var regionA = new SpaceRegion(
            new InclusiveSet<long>(1, 2, 3, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Jan01, true, Dates.Dec31, true)));

        var regionB = new SpaceRegion(
            new InclusiveSet<long>(2, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Jan01, true, Dates.May15, false)));

        // Action
        var results = SpacialOperations.Subtract(regionA, regionB);

        //Assert
        Assert.HasCount(2, results);

        SpaceRegion region = results.ElementAt(0);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(1, 3), new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Dec31)),
            region);

        region = results.ElementAt(1);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(2, 4), new RangeInclusiveSet<DateTime>(Dates.May15, Dates.Dec31)),
            region);
    }

    [TestMethod]
    public void SpacialRegion_Subtract_PartialDimensionsRemain_1()
    {
        // Arrange
        var regionA = new SpaceRegion(
            new InclusiveSet<long>(1, 2, 3, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Jan01, true, Dates.Dec31, true)));

        var regionB = new SpaceRegion(
            new InclusiveSet<long>(2, 4),
            new RangeInclusiveSet<DateTime>(new Range<DateTime>(Dates.Apr01, false, Dates.Aug31, false)));

        // Action
        var results = SpacialOperations.Subtract(regionA, regionB);

        //Assert
        Assert.HasCount(3, results);

        SpaceRegion region = results.ElementAt(0);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(1, 3), new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Dec31)),
            region);

        region = results.ElementAt(1);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(2, 4), new RangeInclusiveSet<DateTime>(Dates.Jan01, Dates.Apr01)),
            region);

        region = results.ElementAt(2);
        AssertAreEqual(
            new SpaceRegion(new InclusiveSet<long>(2, 4), new RangeInclusiveSet<DateTime>(Dates.Aug31, Dates.Dec31)),
            region);
    }



    [TestMethod]
    public void SpacialRegion_Subtract_OverlappingRegionExtendsBeyondRegionA()
    {
        // Arrange
        var regionA = new SpaceRegion<int, int>(
            new RangeInclusiveSet<int>(new Range<int>(0, true, 10, true)),
            new RangeInclusiveSet<int>(new Range<int>(0, true, 10, true)));

        var regionB = new SpaceRegion<int, int>(
            new RangeInclusiveSet<int>(new Range<int>(5, true, 15, true)),
            new RangeInclusiveSet<int>(new Range<int>(5, true, 15, true)));

        // Action
        var results = SpacialOperations.Subtract(regionA, regionB).ToArray();

        // Assert
        for (int x = -1; x <= 16; x++)
        {
            for (int y = -1; y <= 16; y++)
            {
                bool expected = SpacialOperations.IsPointInRegion(
                    Tuple.Create(x, y),
                    regionA) &&
                    !SpacialOperations.IsPointInRegion(
                        Tuple.Create(x, y),
                        regionB);

                bool actual = results.Any(region => SpacialOperations.IsPointInRegion(
                    Tuple.Create(x, y),
                    region));

                Assert.AreEqual(expected, actual, $"Unexpected subtraction result at ({x}, {y}).");
            }
        }
    }

    #region Helper Methods

    private static void AssertAreEqual(SpaceRegion expected, SpaceRegion actual)
    {
        AssertAreEqual(
            (InclusiveSet<long>)expected.Dimensions[0],
            (InclusiveSet<long>)actual.Dimensions[0]);

        AssertAreEqual(
            (RangeInclusiveSet<DateTime>)expected.Dimensions[1],
            (RangeInclusiveSet<DateTime>)actual.Dimensions[1]);
    }

    private static void AssertAreEqual<T>(InclusiveSet<T> expected, InclusiveSet<T> actual)
    {
        var expectedElements = expected.IncludedElements.OrderBy(i => i).ToList();
        var actualElements = actual.IncludedElements.OrderBy(i => i).ToList();

        Assert.AreEqual(expectedElements.Count, actualElements.Count);
        for (int i = 0; i < expectedElements.Count; i++)
        {
            Assert.AreEqual(expectedElements[i], actualElements[i]);
        }
    }

    private static void AssertAreEqual<T>(RangeInclusiveSet<T> expected, RangeInclusiveSet<T> actual)
    {
        var expectedRange = expected.Range;
        var actualRange = actual.Range;

        Assert.AreEqual(expectedRange, actualRange);
    }

    #endregion
}
