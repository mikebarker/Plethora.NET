using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Linq;

namespace Plethora.Test.Linq
{
    [TestFixture]
    public class ListHelper_Test
    {
        private readonly IList<Tuple<int, string>> orderedList = new List<Tuple<int, string>>
            {
                new Tuple<int, string>(21, "Twenty One"),
                new Tuple<int, string>(22, "Twenty Two"),
                new Tuple<int, string>(23, "Twenty Three"),
                new Tuple<int, string>(24, "Twenty Four"),
                //25 missing
                //26 missing
                new Tuple<int, string>(27, "Twenty Seven"),
                new Tuple<int, string>(28, "Twenty Eight"),
                new Tuple<int, string>(29, "Twenty Nine"),
            };

        [Test]
        public void BinarySearch_Found()
        {
            //exec
            int index = orderedList.BinarySearch(tuple => tuple.Item1, 24);

            //test
            Assert.AreEqual(3, index);
        }

        [Test]
        public void BinarySearch_NotFound()
        {
            //exec
            int index = orderedList.BinarySearch(tuple => tuple.Item1, 25);

            //test
            Assert.IsTrue(index < 0);
            Assert.AreEqual(4, ~index);
        }

        [Test]
        public void SubList_Index()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubList(2).ToList();

            //test
            Assert.AreEqual(5, subList.Count);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
            Assert.AreEqual(28, subList[3].Item1);
            Assert.AreEqual("Twenty Eight", subList[3].Item2);
            Assert.AreEqual(29, subList[4].Item1);
            Assert.AreEqual("Twenty Nine", subList[4].Item2);
        }

        [Test]
        public void SubList_Index_Count()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubList(2, 3).ToList();

            //test
            Assert.AreEqual(3, subList.Count);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
        }

        [Test]
        public void SubList_Error_IndexPastEnd()
        {
            try
            {
                //exec
                orderedList.SubList(7);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.NotNull(ex);
            }
        }

        [Test]
        public void SubList_Error_CountPastEnd()
        {
            try
            {
                //exec
                orderedList.SubList(3, 7);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.NotNull(ex);
            }
        }



        [Test]
        public void SubListOrEmpty_Index()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2).ToList();

            //test
            Assert.AreEqual(5, subList.Count);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
            Assert.AreEqual(28, subList[3].Item1);
            Assert.AreEqual("Twenty Eight", subList[3].Item2);
            Assert.AreEqual(29, subList[4].Item1);
            Assert.AreEqual("Twenty Nine", subList[4].Item2);
        }

        [Test]
        public void SubListOrEmpty_IndexPastEnd()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(7).ToList();

            //test
            Assert.AreEqual(0, subList.Count);
        }

        [Test]
        public void SubListOrEmpty_Index_Count()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2, 3).ToList();

            //test
            Assert.AreEqual(3, subList.Count);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
        }

        [Test]
        public void SubListOrEmpty_CountPastEnd()
        {
            //exec
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2, 7).ToList();

            //test
            Assert.AreEqual(5, subList.Count);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
            Assert.AreEqual(28, subList[3].Item1);
            Assert.AreEqual("Twenty Eight", subList[3].Item2);
            Assert.AreEqual(29, subList[4].Item1);
            Assert.AreEqual("Twenty Nine", subList[4].Item2);
        }
    }
}
