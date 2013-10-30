using NUnit.Framework;
using Plethora.Collections.Sets;

namespace Plethora.Test.Collections.Sets
{
    [TestFixture]
    public class CompleteSet_Test
    {
        private CompleteSet<int> A;
        private InclusiveSet<int> B;

        [SetUp]
        public void SetUp()
        {
            A = CompleteSet<int>.Instance;
            B = new InclusiveSet<int>(3, 4, 5);
        }

        [Test]
        public void Contains_InSet()
        {
            //exec
            var result = A.Contains(1);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Subtract()
        {
            //exec
            var A_minus_B = A.Subtract(B);

            //test
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

        [Test]
        public void Intersect()
        {
            //exec
            var A_n_B = A.Intersect(B);

            //test
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

        [Test]
        public void Union()
        {
            //exec
            var A_u_B = A.Union(B);

            //test
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

        [Test]
        public void Inverse()
        {
            //exec
            var notA = A.Inverse();

            //test
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
    }
}
