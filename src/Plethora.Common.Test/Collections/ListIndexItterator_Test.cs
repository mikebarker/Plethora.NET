using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Plethora.Collections.Test
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
        public void ctor_Fail_StartLessThen0()
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
        public void ctor_Fail_CountLessThen0()
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
    }
}
