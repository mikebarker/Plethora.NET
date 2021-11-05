﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;
using Plethora.Test.MockClasses;

namespace Plethora.Test.Collections.Sets
{
    [TestClass]
    public class BaseSetImpl_Test
    {
        private readonly BaseSetImpl<int> A = new MockSetCore<int>(1, 2, 3, 6);
        private readonly BaseSetImpl<int> B = new MockSetCore<int>(3, 4, 5);

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
            Assert.IsFalse(A_u_B.Contains(7));
            Assert.IsFalse(A_u_B.Contains(8));
            Assert.IsFalse(A_u_B.Contains(9));
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
            Assert.IsTrue(A_n_B.Contains(3));
            Assert.IsFalse(A_n_B.Contains(4));
            Assert.IsFalse(A_n_B.Contains(5));
            Assert.IsFalse(A_n_B.Contains(6));
            Assert.IsFalse(A_n_B.Contains(7));
            Assert.IsFalse(A_n_B.Contains(8));
            Assert.IsFalse(A_n_B.Contains(9));
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
            Assert.IsFalse(A_minus_B.Contains(3));
            Assert.IsFalse(A_minus_B.Contains(4));
            Assert.IsFalse(A_minus_B.Contains(5));
            Assert.IsTrue(A_minus_B.Contains(6));
            Assert.IsFalse(A_minus_B.Contains(7));
            Assert.IsFalse(A_minus_B.Contains(8));
            Assert.IsFalse(A_minus_B.Contains(9));
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
            Assert.IsTrue(notA.Contains(4));
            Assert.IsTrue(notA.Contains(5));
            Assert.IsFalse(notA.Contains(6));
            Assert.IsTrue(notA.Contains(7));
            Assert.IsTrue(notA.Contains(8));
            Assert.IsTrue(notA.Contains(9));
        }
    }
}
