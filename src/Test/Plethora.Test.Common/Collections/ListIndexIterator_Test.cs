using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class ListIndexIterator_Test
    {
        private readonly List<int> list = Enumerable.Range(0, 10).ToList();

        [TestMethod]
        public void ctor_Fail_NullList()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexIterator<int> itterator = new ListIndexIterator<int>(null, 3, 4);
            }
            catch (ArgumentNullException)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [TestMethod]
        public void ctor_Fail_StartLessThan0()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, -2, 4);
            }
            catch (ArgumentOutOfRangeException)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [TestMethod]
        public void ctor_Fail_CountLessThan0()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, -2);
            }
            catch (ArgumentOutOfRangeException)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [TestMethod]
        public void ctor_Fail_CountPlusStartGreaterThanListLength()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 20);
            }
            catch (ArgumentException)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [TestMethod]
        public void Empty()
        {
            //Exec
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 0);

            // Assert
            Assert.AreEqual(0, itterator.Count());
        }

        [TestMethod]
        public void SingleElement()
        {
            //Exec
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 1);

            // Assert
            Assert.AreEqual(1, itterator.Count());

            bool areEqual = itterator.SequenceEqual(new[] {3});
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void MultipleElements()
        {
            //Exec
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            // Assert
            Assert.AreEqual(4, itterator.Count());

            bool areEqual = itterator.SequenceEqual(new[] { 3, 4, 5, 6 });
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void Contains_True()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            bool result = itterator.Contains(5);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_False()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            bool result = itterator.Contains(1);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_EdgeCases()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            bool beforeStart = itterator.Contains(2);
            bool atStart = itterator.Contains(3);
            bool atEnd = itterator.Contains(6);
            bool afterEnd = itterator.Contains(7);


            // Assert
            Assert.IsFalse(beforeStart);
            Assert.IsTrue(atStart);
            Assert.IsTrue(atEnd);
            Assert.IsFalse(afterEnd);
        }

        [TestMethod]
        public void CopyTo_ZeroIndex()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            int[] array = new int[10];
            itterator.CopyTo(array, 0);

            // Assert
            Assert.AreEqual(3, array[0]);
            Assert.AreEqual(4, array[1]);
            Assert.AreEqual(5, array[2]);
            Assert.AreEqual(6, array[3]);
        }

        [TestMethod]
        public void CopyTo_NonZeroIndex()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            int[] array = new int[10];
            itterator.CopyTo(array, 5);

            // Assert
            Assert.AreEqual(3, array[5]);
            Assert.AreEqual(4, array[6]);
            Assert.AreEqual(5, array[7]);
            Assert.AreEqual(6, array[8]);
        }

        [TestMethod]
        public void Count()
        {
            // Arrange
            ListIndexIterator<int> itterator = new ListIndexIterator<int>(list, 3, 4);

            //Exec
            var count = itterator.Count;

            // Assert
            Assert.AreEqual(4, count);
        }
    }
}
