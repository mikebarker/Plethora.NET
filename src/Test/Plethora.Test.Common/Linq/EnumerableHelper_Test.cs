using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Linq;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Linq
{
    [TestClass]
    public class EnumerableHelper_Test
    {
        private const int COUNT = 5;
        private readonly IEnumerable<object> enumerable;
        private int enumerableAccessCount;

        public EnumerableHelper_Test()
        {
            enumerable = GetEnumerable();
            enumerableAccessCount = 0;
        }

        #region CacheResult

        [TestMethod]
        public void CacheResult_Access2()
        {
            // Action
            var cacheResult = enumerable.CacheResult();

            var enumerator = cacheResult.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();

            // Assert
            Assert.AreEqual(2, enumerableAccessCount);
        }

        [TestMethod]
        public void CacheResult_Access2_Repeat()
        {
            // Action
            var cacheResult = enumerable.CacheResult();

            var enumeratorA = cacheResult.GetEnumerator();
            enumeratorA.MoveNext();
            enumeratorA.MoveNext();

            var enumeratorB = cacheResult.GetEnumerator();
            enumeratorB.MoveNext();
            enumeratorB.MoveNext();

            // Assert
            Assert.AreEqual(2, enumerableAccessCount);
        }

        [TestMethod]
        public void CacheResult_ReturnsAll()
        {
            // Action
            var cacheResult = enumerable.CacheResult();

            // Assert
            var count = cacheResult.Count();
            Assert.AreEqual(COUNT, count);
        }

#if DEBUG
        [Ignore("This test will only run reliably under Release mode.")]
#endif
        [TestMethod]
        public void CacheResult_ReleaseResource()
        {
            // Action
            var weakEnum = new WeakReference(GetEnumerable());
            IEnumerable<object> cacheResult;
            try
            {
                cacheResult=((IEnumerable<object>)weakEnum.Target).CacheResult();
            }
            catch (ArgumentNullException)
            {
                Assert.Inconclusive("Enumerable was garbage-collected before test could complete.");
                return;
            }

            // Assert
            //GC before before full read
            GC.Collect(2);
            Assert.IsTrue(weakEnum.IsAlive);

            //Full read
            int hash = cacheResult.Sum(o => o.GetHashCode());

            //GC before after full read
            GC.Collect(2);
            Assert.IsFalse(weakEnum.IsAlive);
        }

        [TestMethod]
        public void CacheResult_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).CacheResult();

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region IsCountXxx

        #region IsCount

        [TestMethod]
        public void IsCount_True()
        {
            // Action
            bool result = enumerable.IsCount(COUNT);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCount_False_Zero()
        {
            // Action
            bool result = enumerable.IsCount(0);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCount_False_LessThanSize()
        {
            // Action
            const int count = 3;
            bool result = enumerable.IsCount(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCount_False_MoreThanSize()
        {
            // Action
            const int count = 15;
            bool result = enumerable.IsCount(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCount_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).IsCount(1);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IsCount_Fail_Negative()
        {
            try
            {
                // Action
                enumerable.IsCount(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IsCountGreaterThan

        [TestMethod]
        public void IsCountGreaterThan_False_Equal()
        {
            // Action
            bool result = enumerable.IsCountGreaterThan(COUNT);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThan_True_Zero()
        {
            // Action
            bool result = enumerable.IsCountGreaterThan(0);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThan_True_LessThanSize()
        {
            // Action
            const int count = 3;
            bool result = enumerable.IsCountGreaterThan(count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThan_False_MoreThanSize()
        {
            // Action
            const int count = 15;
            bool result = enumerable.IsCountGreaterThan(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThan_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).IsCountGreaterThan(1);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IsCountGreaterThan_Fail_Negative()
        {
            try
            {
                // Action
                enumerable.IsCountGreaterThan(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IsCountGreaterThanOrEqualTo

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_True_Equal()
        {
            // Action
            bool result = enumerable.IsCountGreaterThanOrEqualTo(COUNT);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_True_Zero()
        {
            // Action
            bool result = enumerable.IsCountGreaterThanOrEqualTo(0);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_True_LessThanSize()
        {
            // Action
            const int count = 3;
            bool result = enumerable.IsCountGreaterThanOrEqualTo(count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_False_MoreThanSize()
        {
            // Action
            const int count = 15;
            bool result = enumerable.IsCountGreaterThanOrEqualTo(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).IsCountGreaterThanOrEqualTo(1);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IsCountGreaterThanOrEqualTo_Fail_Negative()
        {
            try
            {
                // Action
                enumerable.IsCountGreaterThanOrEqualTo(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IsCountLessThan

        [TestMethod]
        public void IsCountLessThan_False_Equal()
        {
            // Action
            bool result = enumerable.IsCountLessThan(COUNT);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThan_False_Zero()
        {
            // Action
            bool result = enumerable.IsCountLessThan(0);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThan_False_LessThanSize()
        {
            // Action
            const int count = 3;
            bool result = enumerable.IsCountLessThan(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThan_True_MoreThanSize()
        {
            // Action
            const int count = 15;
            bool result = enumerable.IsCountLessThan(count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThan_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).IsCountLessThan(1);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IsCountLessThan_Fail_Negative()
        {
            try
            {
                // Action
                enumerable.IsCountLessThan(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region IsCountLessThanOrEqualTo

        [TestMethod]
        public void IsCountLessThanOrEqualTo_True_Equal()
        {
            // Action
            bool result = enumerable.IsCountLessThanOrEqualTo(COUNT);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThanOrEqualTo_False_Zero()
        {
            // Action
            bool result = enumerable.IsCountLessThanOrEqualTo(0);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThanOrEqualTo_False_LessThanSize()
        {
            // Action
            const int count = 3;
            bool result = enumerable.IsCountLessThanOrEqualTo(count);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThanOrEqualTo_True_MoreThanSize()
        {
            // Action
            const int count = 15;
            bool result = enumerable.IsCountLessThanOrEqualTo(count);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [TestMethod]
        public void IsCountLessThanOrEqualTo_Fail_Null()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).IsCountLessThanOrEqualTo(1);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void IsCountLessThanOrEqualTo_Fail_Negative()
        {
            try
            {
                // Action
                enumerable.IsCountLessThanOrEqualTo(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion
        #endregion

        #region ForEach

        [TestMethod]
        public void ForEach()
        {
            // Action
            object prevO = null;
            int count = 0;
            enumerable.ForEach(o =>
                                   {
                                       if (prevO == o)
                                           Assert.Fail("Repeated element");

                                       count++;
                                   });

            // Assert
            Assert.AreEqual(COUNT, count);
        }

        [TestMethod]
        public void ForEach_WithIndex()
        {
            // Action
            object prevO = null;
            int count = 0;
            int maxI = 0;
            enumerable.ForEach((o, i) =>
                                   {
                                       if (prevO == o)
                                           Assert.Fail("Repeated element");

                                       maxI = i;
                                       count++;
                                   });

            // Assert
            Assert.AreEqual(COUNT, count);
            Assert.AreEqual(count - 1, maxI);
        }

        [TestMethod]
        public void ForEach_Fail_EumNull()
        {
            try
            {
                // Action
                ((IEnumerable<object>)null).ForEach(o => { });
                
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ForEach_Fail_ActionNull()
        {
            try
            {
                // Action
                enumerable.ForEach(((Action<object>)null));

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region Flattern

        [TestMethod]
        public void Flatten()
        {
            // Arrange
            TreeElement root = new TreeElement();
            TreeElement childA = new TreeElement();
            TreeElement childB = new TreeElement();
            TreeElement childC = new TreeElement();
            TreeElement grandChildAA = new TreeElement();
            TreeElement grandChildAB = new TreeElement();
            TreeElement grandChildBA = new TreeElement();

            root.AddChild(childA);
            root.AddChild(childB);
            root.AddChild(childC);

            childA.AddChild(grandChildAA);
            childA.AddChild(grandChildAB);

            childB.AddChild(grandChildBA);

            Tree tree = new Tree(root);

            // Action
            var allElements = tree.Flatten(treeElement => treeElement.Children);

            // Assert
            Assert.AreEqual(7, allElements.Count());

            TreeElement[] mustMatch = new[]  //Order in which elements are returned
                {
                    root,
                    childA,
                    childB,
                    childC,
                    grandChildAA,
                    grandChildAB,
                    grandChildBA,
                };
            int i = 0;
            foreach (var element in allElements)
            {
                Assert.AreEqual(mustMatch[i], element);
                i++;
            }
        }

        [TestMethod]
        public void Flatten_NullElement()
        {
            // Arrange
            TreeElement root = new TreeElement();
            TreeElement childA = new TreeElement();
            TreeElement childB = new TreeElement();
            TreeElement childC = null;
            TreeElement grandChildAA = new TreeElement();
            TreeElement grandChildAB = new TreeElement();
            TreeElement grandChildBA = new TreeElement();

            root.AddChild(childA);
            root.AddChild(childB);
            root.AddChild(childC);

            childA.AddChild(grandChildAA);
            childA.AddChild(grandChildAB);

            childB.AddChild(grandChildBA);

            Tree tree = new Tree(root);

            // Action
            var allElements = tree.Flatten(treeElement => treeElement.Children);

            // Assert
            Assert.AreEqual(7, allElements.Count());

            TreeElement[] mustMatch = new[]  //Order in which elements are returned
                {
                    root,
                    childA,
                    childB,
                    childC,
                    grandChildAA,
                    grandChildAB,
                    grandChildBA,
                };
            int i = 0;
            foreach (var element in allElements)
            {
                Assert.AreEqual(mustMatch[i], element);
                i++;
            }
        }
        #endregion

        #region Singularity

        [TestMethod]
        public void Singularity()
        {
            // Arrange
            var o = new object();

            // Action
            var enumO = o.Singularity();

            // Assert
            Assert.AreEqual(1, enumO.Count());
            Assert.AreEqual(o, enumO.First());
        }

        [TestMethod]
        public void Singularity_Null()
        {
            // Arrange
            object o = null;

            // Action
            var enumO = o.Singularity();

            // Assert
            Assert.AreEqual(1, enumO.Count());
            Assert.AreEqual(o, enumO.First());
        }
        #endregion

        #region AsEnumerable

        [TestMethod]
        public void AsEnumerable()
        {
            // Arrange
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);
            IEnumerator<int> enumerator = enumerable.GetEnumerator();

            // Action
            IEnumerable<int> wrappedEnumerator = enumerator.AsEnumerable();

            // Assert
            Assert.IsTrue(wrappedEnumerator.SequenceEqual(enumerable));
        }

        [TestMethod]
        public void AsEnumerable_Fail_Null()
        {
            IEnumerator<int> enumerator = null;

            try
            {
                enumerator.AsEnumerable();
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region Private Methods

        private IEnumerable<object> GetEnumerable()
        {
            for (int i = 0; i < COUNT; i++)
            {
                enumerableAccessCount++;
                yield return new object();
            }
        }
        #endregion
    }
}
