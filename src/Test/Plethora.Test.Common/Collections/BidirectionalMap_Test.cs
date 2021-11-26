using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class BidirectionalMap_Test
    {
        [TestMethod]
        public void Add_NewPair()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            bidirectionalMap.Add(6, "six");

            // Assert
            Assert.AreEqual(6, bidirectionalMap.Count);
            Assert.AreEqual("six", bidirectionalMap[6]);
            Assert.AreEqual(6, bidirectionalMap["six"]);
        }

        [TestMethod]
        public void Add_ExistingPair_NoEffect()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            bidirectionalMap.Add(1, "one");

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void Add_MismatchedPairByForwardKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            try
            {
                bidirectionalMap.Add(1, "six");

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void Add_MismatchedPairByReverseKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            try
            {
                bidirectionalMap.Add(6, "one");

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
        }


        [TestMethod]
        public void Clear()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            bidirectionalMap.Clear();

            // Assert
            Assert.AreEqual(0, bidirectionalMap.Count);
            Assert.IsFalse(bidirectionalMap.Contains(1));
            Assert.IsFalse(bidirectionalMap.ContainsReverse("one"));
        }


        [TestMethod]
        public void Contains_InCollection_True()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Contains(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_NotInCollection_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Contains(6);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsReverse_InCollection_True()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.ContainsReverse("one");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsReverse_NotInCollection_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.ContainsReverse("six");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_PairInCollection_True()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Contains(1, "one");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_PairNotInCollection_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Contains(6, "six");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_PairMismatch_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Contains(1, "six");

            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        public void Lookup_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Lookup(1);

            // Assert
            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void Lookup_NotInCollection_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            try
            {
                var result = bidirectionalMap.Lookup(6);

                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
        }

        [TestMethod]
        public void LookupReverse_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.LookupReverse("one");

            // Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void LookupReverse_NotInCollection_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            try
            {
                var result = bidirectionalMap.LookupReverse("six");

                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
        }


        [TestMethod]
        public void Remove_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Remove(1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, bidirectionalMap.Count);
            Assert.IsFalse(bidirectionalMap.Contains(1));
            Assert.IsFalse(bidirectionalMap.ContainsReverse("one"));
        }

        [TestMethod]
        public void Remove_NotInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Remove(6);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void RemoveReverse_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.RemoveReverse("one");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, bidirectionalMap.Count);
            Assert.IsFalse(bidirectionalMap.Contains(1));
            Assert.IsFalse(bidirectionalMap.ContainsReverse("one"));
        }

        [TestMethod]
        public void RemoveReverse_NotInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.RemoveReverse("six");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void Remove_PairInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Remove(1, "one");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, bidirectionalMap.Count);
            Assert.IsFalse(bidirectionalMap.Contains(1));
            Assert.IsFalse(bidirectionalMap.ContainsReverse("one"));
        }

        [TestMethod]
        public void Remove_PairNotInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.Remove(6, "six");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void Remove_PairMismatch_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            try
            {
                var result = bidirectionalMap.Remove(1, "six");

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
            Assert.IsTrue(bidirectionalMap.Contains(1));
            Assert.IsTrue(bidirectionalMap.ContainsReverse("one"));
        }


        [TestMethod]
        public void ReverseDictionary()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            IReadOnlyDictionary<string, int> result = bidirectionalMap.ReverseDictionary();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, result["one"]);
        }


        [TestMethod]
        public void TryGetValue_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.TryGetValue(1, out var t2);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("one", t2);
        }

        [TestMethod]
        public void TryGetValue_NotInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.TryGetValue(6, out var t2);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(default(string), t2);
        }


        [TestMethod]
        public void TryGetValueReverse_InCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.TryGetValueReverse("one", out var t1);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, t1);
        }

        [TestMethod]
        public void TryGetValueReverse_NotInCollection()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();

            // Action
            var result = bidirectionalMap.TryGetValueReverse("six", out var t1);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(default(int), t1);
        }


        [TestMethod]
        public void Iteration()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            List<int> t1List = new();
            List<string> t2List = new();

            // Action
            foreach (var pair in bidirectionalMap)
            {
                int t1 = pair.Key;
                string t2 = pair.Value;

                t1List.Add(t1);
                t2List.Add(t2);
            }

            Assert.IsTrue(Enumerable.SequenceEqual(t1List, new[] { 1, 2, 3, 4, 5 }));
            Assert.IsTrue(Enumerable.SequenceEqual(t2List, new[] { "one", "two", "three", "four", "five" }));
        }


        #region IReadOnlyDictionary

        [TestMethod]
        public void IReadOnlyDictionary_Keys()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IReadOnlyDictionary<int, string> readOnlyDictionary = (IReadOnlyDictionary<int, string>)bidirectionalMap;

            // Action
            var result = readOnlyDictionary.Keys;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Enumerable.SequenceEqual(result, new[] { 1, 2, 3, 4, 5 }));
        }

        [TestMethod]
        public void IReadOnlyDictionary_Values()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IReadOnlyDictionary<int, string> readOnlyDictionary = (IReadOnlyDictionary<int, string>)bidirectionalMap;

            // Action
            var result = readOnlyDictionary.Values;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Enumerable.SequenceEqual(result, new[] { "one", "two", "three", "four", "five" }));
        }

        [TestMethod]
        public void IReadOnlyDictionary_ContainsKey()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IReadOnlyDictionary<int, string> readOnlyDictionary = (IReadOnlyDictionary<int, string>)bidirectionalMap;

            // Action

            // Assert
            Assert.IsTrue(readOnlyDictionary.ContainsKey(1));
            Assert.IsFalse(readOnlyDictionary.ContainsKey(6));
        }

        #endregion

        #region IDictionary

        [TestMethod]
        public void IDictionary_Keys()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IDictionary<int, string> dictionary = (IDictionary<int, string>)bidirectionalMap;

            // Action
            var result = dictionary.Keys;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Enumerable.SequenceEqual(result, new[] { 1, 2, 3, 4, 5 }));
        }

        [TestMethod]
        public void IDictionary_Values()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IDictionary<int, string> dictionary = (IDictionary<int, string>)bidirectionalMap;

            // Action
            var result = dictionary.Values;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(Enumerable.SequenceEqual(result, new[] { "one", "two", "three", "four", "five" }));
        }

        [TestMethod]
        public void IDictionary_Indexer_get()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IDictionary<int, string> dictionary = (IDictionary<int, string>)bidirectionalMap;

            // Action
            var result = dictionary[1];

            // Assert
            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void IDictionary_Indexer_set_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IDictionary<int, string> dictionary = (IDictionary<int, string>)bidirectionalMap;

            // Action
            try
            {
                dictionary[1] = "six";

                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }

            // Assert
            Assert.AreEqual("one", bidirectionalMap[1]);
        }

        [TestMethod]
        public void IDictionary_ContainsKey()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IDictionary<int, string> dictionary = (IDictionary<int, string>)bidirectionalMap;

            // Action

            // Assert
            Assert.IsTrue(dictionary.ContainsKey(1));
            Assert.IsFalse(dictionary.ContainsKey(6));
        }

        #endregion

        #region ICollection

        [TestMethod]
        public void ICollection_IsReadOnly()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.IsReadOnly;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ICollection_Add_PairNew()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            collection.Add(new KeyValuePair<int, string>(6, "six"));

            // Assert
            Assert.AreEqual(6, bidirectionalMap.Count);
            Assert.AreEqual("six", bidirectionalMap[6]);
            Assert.AreEqual(6, bidirectionalMap["six"]);
        }

        [TestMethod]
        public void ICollection_Add_PairExisting_NoEffect()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            collection.Add(new KeyValuePair<int, string>(1, "one"));

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void ICollection_Add_PairMismatchByForwardKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            try
            {
                collection.Add(new KeyValuePair<int, string>(1, "six"));
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
            Assert.AreEqual("one", bidirectionalMap[1]);
            Assert.AreEqual(1, bidirectionalMap["one"]);
            Assert.IsFalse(bidirectionalMap.ContainsReverse("six"));
        }

        [TestMethod]
        public void ICollection_Add_PairMismatchByReverseKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            try
            {
                collection.Add(new KeyValuePair<int, string>(6, "one"));
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
            Assert.AreEqual("one", bidirectionalMap[1]);
            Assert.AreEqual(1, bidirectionalMap["one"]);
            Assert.IsFalse(bidirectionalMap.Contains(6));
        }

        [TestMethod]
        public void ICollection_Contains_PairInCollection_True()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Contains(new KeyValuePair<int, string>(1, "one"));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ICollection_Contains_PairNotInCollection_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Contains(new KeyValuePair<int, string>(6, "six"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ICollection_Contains_PairMismatchByForwardKey_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Contains(new KeyValuePair<int, string>(1, "six"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ICollection_Contains_PairMismatchByReverseKey_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Contains(new KeyValuePair<int, string>(6, "one"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ICollection_CopyTo()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            KeyValuePair<int, string>[] array = new KeyValuePair<int, string>[bidirectionalMap.Count];

            // Action
            collection.CopyTo(array, 0);

            // Assert
            Assert.AreEqual(1, array[0].Key);
            Assert.AreEqual("one", array[0].Value);
            Assert.AreEqual(5, array[4].Key);
            Assert.AreEqual("five", array[4].Value);
        }

        [TestMethod]
        public void ICollection_Remove_PairInCollection_True()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Remove(new KeyValuePair<int, string>(1, "one"));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, bidirectionalMap.Count);
            Assert.IsFalse(bidirectionalMap.Contains(1));
            Assert.IsFalse(bidirectionalMap.ContainsReverse("one"));
        }

        [TestMethod]
        public void ICollection_Remove_PairNotInCollection_False()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            var result = collection.Remove(new KeyValuePair<int, string>(6, "six"));

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5, bidirectionalMap.Count);
        }

        [TestMethod]
        public void ICollection_Remove_PairMismatchByForwardKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            try
            {
                var result = collection.Remove(new KeyValuePair<int, string>(1, "six"));

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
            Assert.IsTrue(bidirectionalMap.Contains(1));
            Assert.IsTrue(bidirectionalMap.ContainsReverse("one"));
        }

        [TestMethod]
        public void ICollection_Remove_PairMismatchByReverseKey_Throws()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            ICollection<KeyValuePair<int, string>> collection = (ICollection<KeyValuePair<int, string>>)bidirectionalMap;

            // Action
            try
            {
                var result = collection.Remove(new KeyValuePair<int, string>(6, "one"));

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(5, bidirectionalMap.Count);
            Assert.IsTrue(bidirectionalMap.Contains(1));
            Assert.IsTrue(bidirectionalMap.ContainsReverse("one"));
        }

        #endregion

        #region IEnumerable

        [TestMethod]
        public void IEnumerable_IsReadOnly()
        {
            // Arrange
            BidirectionalMap<int, string> bidirectionalMap = CreatePopulatedMap();
            IEnumerable enumerable = (IEnumerable)bidirectionalMap;

            // Action
            var result = enumerable.GetEnumerator();

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion



        private static BidirectionalMap<int, string> CreatePopulatedMap()
        {
            BidirectionalMap<int, string> bidirectionalMap = new();
            bidirectionalMap.Add(1, "one");
            bidirectionalMap.Add(2, "two");
            bidirectionalMap.Add(3, "three");
            bidirectionalMap.Add(4, "four");
            bidirectionalMap.Add(5, "five");

            return bidirectionalMap;
        }
    }
}
