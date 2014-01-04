using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class ReverseComparer_Test
    {
        [Test]
        public void Equal()
        {
            //setup
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            //exec
            var reverseResult = reverseComparer.Compare(1, 1);

            //test
            Assert.IsTrue(reverseResult == 0);
        }

        [Test]
        public void GreaterThan()
        {
            //setup
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            //exec
            var reverseResult = reverseComparer.Compare(1, 2);

            //test
            Assert.IsTrue(reverseResult > 0);
        }

        [Test]
        public void LessThan()
        {
            //setup
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            //exec
            var reverseResult = reverseComparer.Compare(2, 1);

            //test
            Assert.IsTrue(reverseResult < 0);
        }
    }
}
