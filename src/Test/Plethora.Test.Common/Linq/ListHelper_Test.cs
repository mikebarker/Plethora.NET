using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Linq;

namespace Plethora.Test.Linq
{
    [TestClass]
    public class ListHelper_Test
    {
        private readonly List<Tuple<int, string>> orderedList =
            [
                new(21, "Twenty One"),
                new(22, "Twenty Two"),
                new(23, "Twenty Three"),
                new(24, "Twenty Four"),
                //25 missing
                //26 missing
                new(27, "Twenty Seven"),
                new(28, "Twenty Eight"),
                new(29, "Twenty Nine"),
            ];

        [TestMethod]
        public void BinarySearch_Found()
        {
            // Action
            int index = orderedList.BinarySearch(tuple => tuple.Item1, 24);

            // Assert
            Assert.AreEqual(3, index);
        }

        [TestMethod]
        public void BinarySearch_NotFound()
        {
            // Action
            int index = orderedList.BinarySearch(tuple => tuple.Item1, 25);

            // Assert
            Assert.IsLessThan(0, index);
            Assert.AreEqual(4, ~index);
        }

        [TestMethod]
        public void SubList_Index()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubList(2).ToList();

            // Assert
            Assert.HasCount(5, subList);
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

        [TestMethod]
        public void SubList_Index_Count()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubList(2, 3).ToList();

            // Assert
            Assert.HasCount(3, subList);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
        }

        [TestMethod]
        public void SubList_Error_IndexPastEnd()
        {
            try
            {
                // Action
                orderedList.SubList(7);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void SubList_Error_CountPastEnd()
        {
            try
            {
                // Action
                orderedList.SubList(3, 7);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }



        [TestMethod]
        public void SubListOrEmpty_Index()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2).ToList();

            // Assert
            Assert.HasCount(5, subList);
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

        [TestMethod]
        public void SubListOrEmpty_IndexPastEnd()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(7).ToList();

            // Assert
            Assert.IsEmpty(subList);
        }

        [TestMethod]
        public void SubListOrEmpty_Index_Count()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2, 3).ToList();

            // Assert
            Assert.HasCount(3, subList);
            Assert.AreEqual(23, subList[0].Item1);
            Assert.AreEqual("Twenty Three", subList[0].Item2);
            Assert.AreEqual(24, subList[1].Item1);
            Assert.AreEqual("Twenty Four", subList[1].Item2);
            Assert.AreEqual(27, subList[2].Item1);
            Assert.AreEqual("Twenty Seven", subList[2].Item2);
        }

        [TestMethod]
        public void SubListOrEmpty_CountPastEnd()
        {
            // Action
            List<Tuple<int, string>> subList = orderedList.SubListOrEmpty(2, 7).ToList();

            // Assert
            Assert.HasCount(5, subList);
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
