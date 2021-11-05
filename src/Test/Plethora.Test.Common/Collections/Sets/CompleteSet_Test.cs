using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;

namespace Plethora.Test.Collections.Sets
{
    [TestClass]
    public class CompleteSet_Test
    {
        private readonly CompleteSet<int> A = CompleteSet<int>.Instance;
        private readonly InclusiveSet<int> B = new InclusiveSet<int>(3, 4, 5);

        [TestMethod]
        public void Contains_InSet()
        {
            // Action
            var result = A.Contains(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Subtract()
        {
            // Action
            var A_minus_B = A.Subtract(B);

            // Assert
            Assert.IsTrue(A_minus_B.Contains(0));
            Assert.IsTrue(A_minus_B.Contains(1));
            Assert.IsTrue(A_minus_B.Contains(2));
            Assert.IsFalse(A_minus_B.Contains(3));
            Assert.IsFalse(A_minus_B.Contains(4));
            Assert.IsFalse(A_minus_B.Contains(5));
            Assert.IsTrue(A_minus_B.Contains(6));
            Assert.IsTrue(A_minus_B.Contains(7));
            Assert.IsTrue(A_minus_B.Contains(8));
            Assert.IsTrue(A_minus_B.Contains(9));
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
            Assert.IsTrue(A_n_B.Contains(4));
            Assert.IsTrue(A_n_B.Contains(5));
            Assert.IsFalse(A_n_B.Contains(6));
            Assert.IsFalse(A_n_B.Contains(7));
            Assert.IsFalse(A_n_B.Contains(8));
            Assert.IsFalse(A_n_B.Contains(9));
        }

        [TestMethod]
        public void Union()
        {
            // Action
            var A_u_B = A.Union(B);

            // Assert
            Assert.IsTrue(A_u_B.Contains(0));
            Assert.IsTrue(A_u_B.Contains(1));
            Assert.IsTrue(A_u_B.Contains(2));
            Assert.IsTrue(A_u_B.Contains(3));
            Assert.IsTrue(A_u_B.Contains(4));
            Assert.IsTrue(A_u_B.Contains(5));
            Assert.IsTrue(A_u_B.Contains(6));
            Assert.IsTrue(A_u_B.Contains(7));
            Assert.IsTrue(A_u_B.Contains(8));
            Assert.IsTrue(A_u_B.Contains(9));
        }

        [TestMethod]
        public void Inverse()
        {
            // Action
            var notA = A.Inverse();

            // Assert
            Assert.IsFalse(notA.Contains(0));
            Assert.IsFalse(notA.Contains(1));
            Assert.IsFalse(notA.Contains(2));
            Assert.IsFalse(notA.Contains(3));
            Assert.IsFalse(notA.Contains(4));
            Assert.IsFalse(notA.Contains(5));
            Assert.IsFalse(notA.Contains(6));
            Assert.IsFalse(notA.Contains(7));
            Assert.IsFalse(notA.Contains(8));
            Assert.IsFalse(notA.Contains(9));
        }

        [TestMethod]
        public void IsEmpty()
        {
            // Assert
            Assert.IsFalse(A.IsEmpty);
        }
    }
}
