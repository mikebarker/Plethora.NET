using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;
using Plethora.Spacial;
using Plethora.Test.UtilityClasses;
using System;
using System.Linq;

namespace Plethora.Test.Spacial
{
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
            Assert.AreEqual(1, results.Count());

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
            Assert.AreEqual(2, results.Count());

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
            Assert.AreEqual(3, results.Count());

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
}
