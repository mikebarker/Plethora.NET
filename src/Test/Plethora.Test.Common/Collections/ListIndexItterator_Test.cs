using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Collections;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class ListIndexItterator_Test
    {
        private List<int> list;

        [SetUp]
        public void SetUp()
        {
            list = Enumerable.Range(0, 10).ToList();
        }

        [Test]
        public void ctor_Fail_NullList()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexItterator<int> itterator = new ListIndexItterator<int>(null, 3, 4);
            }
            catch (ArgumentNullException ex)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [Test]
        public void ctor_Fail_StartLessThan0()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, -2, 4);
            }
            catch (ArgumentOutOfRangeException  ex)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [Test]
        public void ctor_Fail_CountLessThan0()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, -2);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [Test]
        public void ctor_Fail_CountPlusStartGreaterThanListLength()
        {
            bool isCaught = false;
            try
            {
                //Exec
                ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 20);
            }
            catch (ArgumentException ex)
            {
                isCaught = true;
            }
            Assert.IsTrue(isCaught);
        }

        [Test]
        public void Empty()
        {
            //Exec
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 0);

            //Test
            Assert.AreEqual(0, itterator.Count());
        }

        [Test]
        public void SingleElement()
        {
            //Exec
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 1);

            //Test
            Assert.AreEqual(1, itterator.Count());

            bool areEqual = itterator.SequenceEqual(new[] {3});
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void MultipleElements()
        {
            //Exec
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Test
            Assert.AreEqual(4, itterator.Count());

            bool areEqual = itterator.SequenceEqual(new[] { 3, 4, 5, 6 });
            Assert.IsTrue(areEqual);
        }

        [Test]
        public void Contains_True()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            bool result = itterator.Contains(5);

            //Test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            bool result = itterator.Contains(1);

            //Test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_EdgeCases()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            bool beforeStart = itterator.Contains(2);
            bool atStart = itterator.Contains(3);
            bool atEnd = itterator.Contains(6);
            bool afterEnd = itterator.Contains(7);


            //Test
            Assert.IsFalse(beforeStart);
            Assert.IsTrue(atStart);
            Assert.IsTrue(atEnd);
            Assert.IsFalse(afterEnd);
        }

        [Test]
        public void CopyTo_ZeroIndex()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            int[] array = new int[10];
            itterator.CopyTo(array, 0);

            //Test
            Assert.AreEqual(3, array[0]);
            Assert.AreEqual(4, array[1]);
            Assert.AreEqual(5, array[2]);
            Assert.AreEqual(6, array[3]);
        }

        [Test]
        public void CopyTo_NonZeroIndex()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            int[] array = new int[10];
            itterator.CopyTo(array, 5);

            //Test
            Assert.AreEqual(3, array[5]);
            Assert.AreEqual(4, array[6]);
            Assert.AreEqual(5, array[7]);
            Assert.AreEqual(6, array[8]);
        }

        [Test]
        public void Count()
        {
            //Setup
            ListIndexItterator<int> itterator = new ListIndexItterator<int>(list, 3, 4);

            //Exec
            var count = itterator.Count;

            //Test
            Assert.AreEqual(4, count);
        }
    }
}
