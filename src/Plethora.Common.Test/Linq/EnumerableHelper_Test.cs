using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Linq.Test
{
    [TestFixture]
    public class EnumerableHelper_Test
    {
        private const int COUNT = 5;
        private IEnumerable<object> enumerable;
        private int enumerableAccessCount;

        [SetUp]
        public void SetUp()
        {
            enumerable = GetEnumerable();
            enumerableAccessCount = 0;
        }

        #region CacheResult

        [Test]
        public void CacheResult_Access2()
        {
            //exec
            var cacheResult = enumerable.CacheResult();

            var enumerator = cacheResult.GetEnumerator();
            enumerator.MoveNext();
            enumerator.MoveNext();

            //test
            Assert.AreEqual(2, enumerableAccessCount);
        }

        [Test]
        public void CacheResult_Access2_Repeat()
        {
            //exec
            var cacheResult = enumerable.CacheResult();

            var enumeratorA = cacheResult.GetEnumerator();
            enumeratorA.MoveNext();
            enumeratorA.MoveNext();

            var enumeratorB = cacheResult.GetEnumerator();
            enumeratorB.MoveNext();
            enumeratorB.MoveNext();

            //test
            Assert.AreEqual(2, enumerableAccessCount);
        }

        [Test]
        public void CacheResult_ReturnsAll()
        {
            //exec
            var cacheResult = enumerable.CacheResult();

            //test
            var count = cacheResult.Count();
            Assert.AreEqual(COUNT, count);
        }

        [Test]
        public void CacheResult_ReleaseResource()
        {
            //exec
            var weakEnum = new WeakReference(GetEnumerable());
            IEnumerable<object> cacheResult;
            try
            {
                cacheResult=((IEnumerable<object>)weakEnum.Target).CacheResult();
            }
            catch (ArgumentNullException)
            {
                Assert.Ignore("Enumerable was garbage-collected before test could complete.");
                return;
            }

            //test
            //GC before before full read
            GC.Collect(2);
            Assert.IsTrue(weakEnum.IsAlive);

            //Full read
            int hash = cacheResult.Sum(o => o.GetHashCode());

            //GC before after full read
            GC.Collect(2);
            Assert.IsFalse(weakEnum.IsAlive);
        }

        [Test]
        public void CacheResult_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).CacheResult();

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IsCountXxx

        #region IsCount

        [Test]
        public void IsCount_True()
        {
            //exec
            bool result = enumerable.IsCount(COUNT);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCount_False_Zero()
        {
            //exec
            bool result = enumerable.IsCount(0);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [Test]
        public void IsCount_False_LessThanSize()
        {
            //exec
            const int count = 3;
            bool result = enumerable.IsCount(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [Test]
        public void IsCount_False_MoreThanSize()
        {
            //exec
            const int count = 15;
            bool result = enumerable.IsCount(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCount_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).IsCount(1);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IsCount_Fail_Negative()
        {
            try
            {
                //exec
                enumerable.IsCount(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IsCountGreaterThan

        [Test]
        public void IsCountGreaterThan_False_Equal()
        {
            //exec
            bool result = enumerable.IsCountGreaterThan(COUNT);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThan_True_Zero()
        {
            //exec
            bool result = enumerable.IsCountGreaterThan(0);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThan_True_LessThanSize()
        {
            //exec
            const int count = 3;
            bool result = enumerable.IsCountGreaterThan(count);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThan_False_MoreThanSize()
        {
            //exec
            const int count = 15;
            bool result = enumerable.IsCountGreaterThan(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThan_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).IsCountGreaterThan(1);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IsCountGreaterThan_Fail_Negative()
        {
            try
            {
                //exec
                enumerable.IsCountGreaterThan(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IsCountGreaterThanOrEqualTo

        [Test]
        public void IsCountGreaterThanOrEqualTo_True_Equal()
        {
            //exec
            bool result = enumerable.IsCountGreaterThanOrEqualTo(COUNT);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThanOrEqualTo_True_Zero()
        {
            //exec
            bool result = enumerable.IsCountGreaterThanOrEqualTo(0);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThanOrEqualTo_True_LessThanSize()
        {
            //exec
            const int count = 3;
            bool result = enumerable.IsCountGreaterThanOrEqualTo(count);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThanOrEqualTo_False_MoreThanSize()
        {
            //exec
            const int count = 15;
            bool result = enumerable.IsCountGreaterThanOrEqualTo(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountGreaterThanOrEqualTo_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).IsCountGreaterThanOrEqualTo(1);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IsCountGreaterThanOrEqualTo_Fail_Negative()
        {
            try
            {
                //exec
                enumerable.IsCountGreaterThanOrEqualTo(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IsCountLessThan

        [Test]
        public void IsCountLessThan_False_Equal()
        {
            //exec
            bool result = enumerable.IsCountLessThan(COUNT);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThan_False_Zero()
        {
            //exec
            bool result = enumerable.IsCountLessThan(0);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThan_False_LessThanSize()
        {
            //exec
            const int count = 3;
            bool result = enumerable.IsCountLessThan(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThan_True_MoreThanSize()
        {
            //exec
            const int count = 15;
            bool result = enumerable.IsCountLessThan(count);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThan_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).IsCountLessThan(1);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IsCountLessThan_Fail_Negative()
        {
            try
            {
                //exec
                enumerable.IsCountLessThan(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region IsCountLessThanOrEqualTo

        [Test]
        public void IsCountLessThanOrEqualTo_True_Equal()
        {
            //exec
            bool result = enumerable.IsCountLessThanOrEqualTo(COUNT);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThanOrEqualTo_False_Zero()
        {
            //exec
            bool result = enumerable.IsCountLessThanOrEqualTo(0);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(1, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThanOrEqualTo_False_LessThanSize()
        {
            //exec
            const int count = 3;
            bool result = enumerable.IsCountLessThanOrEqualTo(count);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(count + 1, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThanOrEqualTo_True_MoreThanSize()
        {
            //exec
            const int count = 15;
            bool result = enumerable.IsCountLessThanOrEqualTo(count);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(COUNT, enumerableAccessCount);
        }

        [Test]
        public void IsCountLessThanOrEqualTo_Fail_Null()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).IsCountLessThanOrEqualTo(1);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void IsCountLessThanOrEqualTo_Fail_Negative()
        {
            try
            {
                //exec
                enumerable.IsCountLessThanOrEqualTo(-1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion
        #endregion

        #region ForEach

        [Test]
        public void ForEach()
        {
            //exec
            object prevO = null;
            int count = 0;
            enumerable.ForEach(o =>
                                   {
                                       if (prevO == o)
                                           Assert.Fail("Repeated element");

                                       count++;
                                   });

            //test
            Assert.AreEqual(COUNT, count);
        }

        [Test]
        public void ForEach_WithIndex()
        {
            //exec
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

            //test
            Assert.AreEqual(COUNT, count);
            Assert.AreEqual(count - 1, maxI);
        }

        [Test]
        public void ForEach_Fail_EumNull()
        {
            try
            {
                //exec
                ((IEnumerable<object>)null).ForEach(o => { });
                
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ForEach_Fail_ActionNull()
        {
            try
            {
                //exec
                enumerable.ForEach(((Action<object>)null));

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region Flattern

        [Test]
        public void Flatten()
        {
            //Setup
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

            //exec
            var allElements = tree.Flatten(treeElement => treeElement.Children);

            //test
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

        [Test]
        public void Flatten_NullElement()
        {
            //Setup
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

            //exec
            var allElements = tree.Flatten(treeElement => treeElement.Children);

            //test
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

        [Test]
        public void Singularity()
        {
            //setup
            var o = new object();

            //exec
            var enumO = o.Singularity();

            //test
            Assert.AreEqual(1, enumO.Count());
            Assert.AreEqual(o, enumO.First());
        }

        [Test]
        public void Singularity_Null()
        {
            //setup
            object o = null;

            //exec
            var enumO = o.Singularity();

            //test
            Assert.AreEqual(1, enumO.Count());
            Assert.AreEqual(o, enumO.First());
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
