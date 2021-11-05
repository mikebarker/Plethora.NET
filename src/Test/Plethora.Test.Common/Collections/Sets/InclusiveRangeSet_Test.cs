﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;
using System.Linq;

namespace Plethora.Test.Collections.Sets
{
    [TestClass]
    public class RangeInclusiveSet_Test
    {
        private readonly RangeInclusiveSet<int> A = new RangeInclusiveSet<int>(1, 10);
        private readonly RangeInclusiveSet<int> B = new RangeInclusiveSet<int>(5, 15);

        [TestMethod]
        public void Contains_InSet()
        {
            // Action
            var result = A.Contains(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_NotInSet()
        {
            // Action
            var result = A.Contains(14);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Subtract()
        {
            // Action
            var A_minus_B = A.Subtract(B);

            // Assert
            Assert.IsFalse(A_minus_B.Contains(0));
            Assert.IsTrue(A_minus_B.Contains(1));
            Assert.IsTrue(A_minus_B.Contains(2));
            Assert.IsTrue(A_minus_B.Contains(3));
            Assert.IsTrue(A_minus_B.Contains(4));
            Assert.IsFalse(A_minus_B.Contains(5));
            Assert.IsFalse(A_minus_B.Contains(6));
            Assert.IsFalse(A_minus_B.Contains(7));
            Assert.IsFalse(A_minus_B.Contains(8));
            Assert.IsFalse(A_minus_B.Contains(9));
            Assert.IsFalse(A_minus_B.Contains(10));
            Assert.IsFalse(A_minus_B.Contains(11));
            Assert.IsFalse(A_minus_B.Contains(12));
            Assert.IsFalse(A_minus_B.Contains(13));
            Assert.IsFalse(A_minus_B.Contains(14));
            Assert.IsFalse(A_minus_B.Contains(15));
            Assert.IsFalse(A_minus_B.Contains(16));
        }

        [TestMethod]
        public void SubtractMulti()
        {
            // Arrange
            RangeInclusiveSet<int> B = new RangeInclusiveSet<int>(5, 7);

            // Action
            var results = ((ISetCoreMultiSubtract<int>)A).Subtract(B);

            // Assert
            Assert.AreEqual(2, results.Count);

            var r = (RangeInclusiveSet<int>)results.ElementAt(0);
            Assert.AreEqual(1, r.Range.Min);
            Assert.AreEqual(true, r.Range.MinInclusive);
            Assert.AreEqual(5, r.Range.Max);
            Assert.AreEqual(false, r.Range.MaxInclusive);

            r = (RangeInclusiveSet<int>)results.ElementAt(1);
            Assert.AreEqual(7, r.Range.Min);
            Assert.AreEqual(false, r.Range.MinInclusive);
            Assert.AreEqual(10, r.Range.Max);
            Assert.AreEqual(true, r.Range.MaxInclusive);
        }

        [TestMethod]
        public void Intersect()
        {
            // Action
            var A_n_B = A.Intersect(B);

            // Assert
            Assert.IsFalse(A_n_B.Contains(0));
            Assert.IsFalse(A_n_B.Contains(1));
            Assert.IsFalse(A_n_B.Contains(2));
            Assert.IsFalse(A_n_B.Contains(3));
            Assert.IsFalse(A_n_B.Contains(4));
            Assert.IsTrue(A_n_B.Contains(5));
            Assert.IsTrue(A_n_B.Contains(6));
            Assert.IsTrue(A_n_B.Contains(7));
            Assert.IsTrue(A_n_B.Contains(8));
            Assert.IsTrue(A_n_B.Contains(9));
            Assert.IsTrue(A_n_B.Contains(10));
            Assert.IsFalse(A_n_B.Contains(11));
            Assert.IsFalse(A_n_B.Contains(12));
            Assert.IsFalse(A_n_B.Contains(13));
            Assert.IsFalse(A_n_B.Contains(14));
            Assert.IsFalse(A_n_B.Contains(15));
            Assert.IsFalse(A_n_B.Contains(16));
        }

        [TestMethod]
        public void Union()
        {
            // Action
            var A_u_B = A.Union(B);

            // Assert
            Assert.IsFalse(A_u_B.Contains(0));
            Assert.IsTrue(A_u_B.Contains(1));
            Assert.IsTrue(A_u_B.Contains(2));
            Assert.IsTrue(A_u_B.Contains(3));
            Assert.IsTrue(A_u_B.Contains(4));
            Assert.IsTrue(A_u_B.Contains(5));
            Assert.IsTrue(A_u_B.Contains(6));
            Assert.IsTrue(A_u_B.Contains(7));
            Assert.IsTrue(A_u_B.Contains(8));
            Assert.IsTrue(A_u_B.Contains(9));
            Assert.IsTrue(A_u_B.Contains(10));
            Assert.IsTrue(A_u_B.Contains(11));
            Assert.IsTrue(A_u_B.Contains(12));
            Assert.IsTrue(A_u_B.Contains(13));
            Assert.IsTrue(A_u_B.Contains(14));
            Assert.IsTrue(A_u_B.Contains(15));
            Assert.IsFalse(A_u_B.Contains(16));
        }

        [TestMethod]
        public void Inverse()
        {
            // Action
            var notA = A.Inverse();

            // Assert
            Assert.IsTrue(notA.Contains(0));
            Assert.IsFalse(notA.Contains(1));
            Assert.IsFalse(notA.Contains(2));
            Assert.IsFalse(notA.Contains(3));
            Assert.IsFalse(notA.Contains(4));
            Assert.IsFalse(notA.Contains(5));
            Assert.IsFalse(notA.Contains(6));
            Assert.IsFalse(notA.Contains(7));
            Assert.IsFalse(notA.Contains(8));
            Assert.IsFalse(notA.Contains(9));
            Assert.IsFalse(notA.Contains(10));
            Assert.IsTrue(notA.Contains(11));
            Assert.IsTrue(notA.Contains(12));
            Assert.IsTrue(notA.Contains(13));
            Assert.IsTrue(notA.Contains(14));
            Assert.IsTrue(notA.Contains(15));
            Assert.IsTrue(notA.Contains(16));
        }

        [TestMethod]
        public void IsEmpty()
        {
            // Action
            var result = A.IsEmpty;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsEmpty_MinMaxEqualExcluded()
        {
            // Arrange
            RangeInclusiveSet<int> A = new RangeInclusiveSet<int>(new Range<int>(7, false, 7, false));

            // Action
            var result = A.IsEmpty;

            // Assert
            Assert.IsTrue(result);
        }
    }
}
