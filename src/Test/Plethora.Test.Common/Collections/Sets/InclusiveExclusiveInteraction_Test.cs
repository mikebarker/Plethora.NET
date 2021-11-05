using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Sets;

namespace Plethora.Test.Collections.Sets
{
    [TestClass]
    public class InclusiveExclusiveInteraction_Test
    {
        private readonly InclusiveSet<int> A = new InclusiveSet<int>(1, 2, 3, 6);
        private readonly ExclusiveSet<int> B = new ExclusiveSet<int>(3, 4, 5);

        [TestMethod]
        public void Inc_Subtract_Exc()
        {
            // Action
            var A_minus_B = A.Subtract(B);

            // Assert
            Assert.IsFalse(A_minus_B.Contains(0));
            Assert.IsFalse(A_minus_B.Contains(1));
            Assert.IsFalse(A_minus_B.Contains(2));
            Assert.IsTrue(A_minus_B.Contains(3));
            Assert.IsFalse(A_minus_B.Contains(4));
            Assert.IsFalse(A_minus_B.Contains(5));
            Assert.IsFalse(A_minus_B.Contains(6));
            Assert.IsFalse(A_minus_B.Contains(7));
            Assert.IsFalse(A_minus_B.Contains(8));
            Assert.IsFalse(A_minus_B.Contains(9));
        }

        [TestMethod]
        public void Exc_Subtract_Inc()
        {
            // Action
            var B_minus_A = B.Subtract(A);

            // Assert
            Assert.IsTrue(B_minus_A.Contains(0));
            Assert.IsFalse(B_minus_A.Contains(1));
            Assert.IsFalse(B_minus_A.Contains(2));
            Assert.IsFalse(B_minus_A.Contains(3));
            Assert.IsFalse(B_minus_A.Contains(4));
            Assert.IsFalse(B_minus_A.Contains(5));
            Assert.IsFalse(B_minus_A.Contains(6));
            Assert.IsTrue(B_minus_A.Contains(7));
            Assert.IsTrue(B_minus_A.Contains(8));
            Assert.IsTrue(B_minus_A.Contains(9));
        }


        [TestMethod]
        public void Inc_Intersect_Exc()
        {
            // Action
            var A_n_B = A.Intersect(B);

            // Assert
            Assert.IsFalse(A_n_B.Contains(0));
            Assert.IsTrue(A_n_B.Contains(1));
            Assert.IsTrue(A_n_B.Contains(2));
            Assert.IsFalse(A_n_B.Contains(3));
            Assert.IsFalse(A_n_B.Contains(4));
            Assert.IsFalse(A_n_B.Contains(5));
            Assert.IsTrue(A_n_B.Contains(6));
            Assert.IsFalse(A_n_B.Contains(7));
            Assert.IsFalse(A_n_B.Contains(8));
            Assert.IsFalse(A_n_B.Contains(9));
        }

        [TestMethod]
        public void Exc_Intersect_Inc()
        {
            // Action
            var B_n_A = B.Intersect(A);

            // Assert
            Assert.IsFalse(B_n_A.Contains(0));
            Assert.IsTrue(B_n_A.Contains(1));
            Assert.IsTrue(B_n_A.Contains(2));
            Assert.IsFalse(B_n_A.Contains(3));
            Assert.IsFalse(B_n_A.Contains(4));
            Assert.IsFalse(B_n_A.Contains(5));
            Assert.IsTrue(B_n_A.Contains(6));
            Assert.IsFalse(B_n_A.Contains(7));
            Assert.IsFalse(B_n_A.Contains(8));
            Assert.IsFalse(B_n_A.Contains(9));
        }


        [TestMethod]
        public void Inc_Union_Exc()
        {
            // Action
            var A_u_B = A.Union(B);

            // Assert
            Assert.IsTrue(A_u_B.Contains(0));
            Assert.IsTrue(A_u_B.Contains(1));
            Assert.IsTrue(A_u_B.Contains(2));
            Assert.IsTrue(A_u_B.Contains(3));
            Assert.IsFalse(A_u_B.Contains(4));
            Assert.IsFalse(A_u_B.Contains(5));
            Assert.IsTrue(A_u_B.Contains(6));
            Assert.IsTrue(A_u_B.Contains(7));
            Assert.IsTrue(A_u_B.Contains(8));
            Assert.IsTrue(A_u_B.Contains(9));
        }

        [TestMethod]
        public void Exc_Union_Inc()
        {
            // Action
            var B_u_A = B.Union(A);

            // Assert
            Assert.IsTrue(B_u_A.Contains(0));
            Assert.IsTrue(B_u_A.Contains(1));
            Assert.IsTrue(B_u_A.Contains(2));
            Assert.IsTrue(B_u_A.Contains(3));
            Assert.IsFalse(B_u_A.Contains(4));
            Assert.IsFalse(B_u_A.Contains(5));
            Assert.IsTrue(B_u_A.Contains(6));
            Assert.IsTrue(B_u_A.Contains(7));
            Assert.IsTrue(B_u_A.Contains(8));
            Assert.IsTrue(B_u_A.Contains(9));
        }
    }
}
