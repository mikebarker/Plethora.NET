using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;

namespace Plethora.Test.Collections.Sets
{
    [TestClass]
    public class ExclusiveSet_Test
    {
        private readonly ExclusiveSet<int> A = new ExclusiveSet<int>(1, 2, 3, 6);
        private readonly ExclusiveSet<int> B = new ExclusiveSet<int>(3, 4, 5);

        [TestMethod]
        public void Contains_NotInSet()
        {
            // Action
            var result = A.Contains(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_InSet()
        {
            // Action
            var result = A.Contains(4);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Subtract()
        {
            // Action
            var A_minus_B = A.Subtract(B);

            // Assert
            Assert.IsFalse(A_minus_B.Contains(0));
            Assert.IsFalse(A_minus_B.Contains(1));
            Assert.IsFalse(A_minus_B.Contains(2));
            Assert.IsFalse(A_minus_B.Contains(3));
            Assert.IsTrue(A_minus_B.Contains(4));
            Assert.IsTrue(A_minus_B.Contains(5));
            Assert.IsFalse(A_minus_B.Contains(6));
            Assert.IsFalse(A_minus_B.Contains(7));
            Assert.IsFalse(A_minus_B.Contains(8));
            Assert.IsFalse(A_minus_B.Contains(9));
        }

        [TestMethod]
        public void Intersect()
        {
            // Action
            var A_n_B = A.Intersect(B);

            // Assert
            Assert.IsTrue(A_n_B.Contains(0));
            Assert.IsFalse(A_n_B.Contains(1));
            Assert.IsFalse(A_n_B.Contains(2));
            Assert.IsFalse(A_n_B.Contains(3));
            Assert.IsFalse(A_n_B.Contains(4));
            Assert.IsFalse(A_n_B.Contains(5));
            Assert.IsFalse(A_n_B.Contains(6));
            Assert.IsTrue(A_n_B.Contains(7));
            Assert.IsTrue(A_n_B.Contains(8));
            Assert.IsTrue(A_n_B.Contains(9));
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
            Assert.IsFalse(A_u_B.Contains(3));
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
            Assert.IsTrue(notA.Contains(1));
            Assert.IsTrue(notA.Contains(2));
            Assert.IsTrue(notA.Contains(3));
            Assert.IsFalse(notA.Contains(4));
            Assert.IsFalse(notA.Contains(5));
            Assert.IsTrue(notA.Contains(6));
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
