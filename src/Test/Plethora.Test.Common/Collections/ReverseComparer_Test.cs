using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class ReverseComparer_Test
    {
        [TestMethod]
        public void Equal()
        {
            // Arrange
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            // Action
            var reverseResult = reverseComparer.Compare(1, 1);

            // Assert
            Assert.IsTrue(reverseResult == 0);
        }

        [TestMethod]
        public void GreaterThan()
        {
            // Arrange
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            // Action
            var reverseResult = reverseComparer.Compare(1, 2);

            // Assert
            Assert.IsTrue(reverseResult > 0);
        }

        [TestMethod]
        public void LessThan()
        {
            // Arrange
            IComparer<int> comparer = Comparer<int>.Default;
            IComparer<int> reverseComparer = comparer.Reverse();

            // Action
            var reverseResult = reverseComparer.Compare(2, 1);

            // Assert
            Assert.IsTrue(reverseResult < 0);
        }
    }
}
