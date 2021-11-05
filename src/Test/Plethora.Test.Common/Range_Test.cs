using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Plethora.Test
{
    [TestClass]
    public class Range_Test
    {
        #region IsInRange

        [TestMethod]
        public void IsInRange_BeforeMin_NotInRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = range.IsInRange(0);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInRange_MinInclusive_InRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = range.IsInRange(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInRange_MinExclusive_NotInRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, false, 10, false);

            // Action
            var result = range.IsInRange(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInRange_BetweenMinAndMaxInclusive_InRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, true, 10, true);

            // Action
            var result = range.IsInRange(5);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInRange_BetweenMinAndMaxExclusive_InRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, false, 10, false);

            // Action
            var result = range.IsInRange(5);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInRange_MaxInclusive_InRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = range.IsInRange(10);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsInRange_MaxExclusive_NotInRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, false, 10, false);

            // Action
            var result = range.IsInRange(10);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInRange_AfterMax_NotInRange()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = range.IsInRange(11);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Subtract

        [TestMethod]
        public void Subtract_SameRange_Empty()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = RangeHelper.Subtract(range, range);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Subtract_EqualRange_Empty()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(1, 10);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Subtract_LargerRange()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(0, 100);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Subtract_DiffersByMinInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, true, 10, true);
            Range<long> range2 = new Range<long>(1, false, 10, true);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(1, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(1, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_DiffersByMaxInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, true, 10, true);
            Range<long> range2 = new Range<long>(1, true, 10, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(10, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_SmallerRange_MinMaxInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(3, 8);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(2, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(1, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(3, r.Max);
            Assert.AreEqual(false, r.MaxInclusive);

            r = result.ElementAt(1);
            Assert.AreEqual(8, r.Min);
            Assert.AreEqual(false, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }
        [TestMethod]
        public void Subtract_SmallerRange_MinMaxExclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(3, false, 8, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(2, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(1, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(3, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);

            r = result.ElementAt(1);
            Assert.AreEqual(8, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_RangeEntirelyBefore()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(1, 2);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_RangeEntirelyAfter()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(20, 50);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MinInclusiveSubtractMaxInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(1, true, 5, true);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(false, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MinInclusiveSubtractMaxExclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(1, true, 5, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MinExclusiveSubtractMaxInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, false, 10, true);
            Range<long> range2 = new Range<long>(1, true, 5, true);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(false, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MinExclusiveSubtractMaxExclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, false, 10, true);
            Range<long> range2 = new Range<long>(1, true, 5, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(false, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }


        [TestMethod]
        public void Subtract_MaxInclusiveSubtractMinInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(10, true, 50, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(false, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MaxInclusiveSubtractMinExclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, 10);
            Range<long> range2 = new Range<long>(10, false, 50, true);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(true, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MaxExclusiveSubtractMinInclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, true, 10, false);
            Range<long> range2 = new Range<long>(10, true, 50, false);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(false, r.MaxInclusive);
        }

        [TestMethod]
        public void Subtract_MaxExclusiveSubtractMinExclusive()
        {
            // Arrange
            Range<long> range1 = new Range<long>(5, true, 10, false);
            Range<long> range2 = new Range<long>(10, false, 50, true);

            // Action
            var result = RangeHelper.Subtract(range1, range2);

            // Assert
            Assert.AreEqual(1, result.Count);

            Range<long> r = result.ElementAt(0);
            Assert.AreEqual(5, r.Min);
            Assert.AreEqual(true, r.MinInclusive);
            Assert.AreEqual(10, r.Max);
            Assert.AreEqual(false, r.MaxInclusive);
        }

        #endregion

        #region Equals

        [TestMethod]
        public void Equals_SameRange_True()
        {
            // Arrange
            Range<long> range = new Range<long>(1, 10);

            // Action
            var result = range.Equals(range);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_EqualRange_True()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(1, 10);

            // Action
            var result = range1.Equals(range2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_MinDiffer_False()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(2, 10);

            // Action
            var result = range1.Equals(range2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_MinInclusiveDiffer_False()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(1, false, 10, true);

            // Action
            var result = range1.Equals(range2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_MaxDiffer_False()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(1, 9);

            // Action
            var result = range1.Equals(range2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_MaxInclusiveDiffer_False()
        {
            // Arrange
            Range<long> range1 = new Range<long>(1, 10);
            Range<long> range2 = new Range<long>(1, true, 10, false);

            // Action
            var result = range1.Equals(range2);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion
    }
}
