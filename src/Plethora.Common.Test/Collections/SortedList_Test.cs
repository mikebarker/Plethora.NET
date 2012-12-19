using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class SortedList_Test
    {
        private SortedList<Person> sortedList;

        [SetUp]
        public void SetUp()
        {
            PresetDuplicatesError();
        }

        #region Constructors

        [Test]
        public void ctor_Empty()
        {
            //exec
            sortedList = new SortedList<Person>();

            //test
            Assert.IsNotNull(sortedList);
            Assert.AreEqual(0, sortedList.Count);
        }

        [Test]
        public void ctor_Comparer()
        {
            //exec
            sortedList = new SortedList<Person>(new Person.NameComparer());

            //test
            Assert.IsNotNull(sortedList);
            Assert.AreEqual(0, sortedList.Count);
        }

        [Test]
        public void ctor_Comparer_Fail_Null()
        {
            try
            {
                //exec
                sortedList = new SortedList<Person>(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_DuplicatesPolicyComparer()
        {
            //exec
            sortedList = new SortedList<Person>(DuplicatesPolicy.Error, new Person.NameComparer());

            //test
            Assert.IsNotNull(sortedList);
            Assert.AreEqual(0, sortedList.Count);
        }

        [Test]
        public void ctor_DuplicatesPolicyComparer_Fail_InvalidDuplicatesPolicy()
        {
            try
            {
                //exec
                sortedList = new SortedList<Person>((DuplicatesPolicy)78, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_DuplicatesPolicyComparer_Fail_NullComparer()
        {
            try
            {
                //exec
                sortedList = new SortedList<Person>(DuplicatesPolicy.Error, null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_EnumerableDuplicatesPolicyComparer()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            //exec
            sortedList = new SortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

            //test
            Assert.IsNotNull(sortedList);
            Assert.AreEqual(3, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, sortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, sortedList[2]);
        }

        [Test]
        public void ctor_EnumerableDuplicatesPolicyComparer_Fail_NullEnumerable()
        {
            try
            {
                //exec
                sortedList = new SortedList<Person>(null, DuplicatesPolicy.Error, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_EnumerableDuplicatesPolicyComparer_Fail_InvalidDuplicatesPolicy()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            try
            {
                //exec
                sortedList = new SortedList<Person>(array, (DuplicatesPolicy)78, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullComparer()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            try
            {
                //exec
                sortedList = new SortedList<Person>(array, DuplicatesPolicy.Error, null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerableComparer_Fail_DuplicateError()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Bob_Jameson, Person.Amy_Cathson };

            try
            {
                //exec
                sortedList = new SortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region Add

        [Test]
        public void Add()
        {
            //setup
            int preAddCount = sortedList.Count;

            //exec
            sortedList.Add(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, sortedList[1]);
            Assert.AreEqual(Person.Jill_Dorrman, sortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson, sortedList[3]);
        }

        [Test]
        public void Add_Fail_Null()
        {
            try
            {
                //exec
                sortedList.Add(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Add_DuplicateError()
        {
            //setup
            PresetDuplicatesError();

            try
            {
                //exec
                sortedList.Add(Person.Bob_Jameson2);

                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Add_DuplicateReplace()
        {
            //setup
            PresetDuplicatesReplace();
            int preAddCount = sortedList.Count;

            //exec
            sortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, sortedList[1]);
            Assert.AreNotEqual(Person.Bob_Jameson, sortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson2, sortedList[2]);
        }

        [Test]
        public void Add_DuplicateIgnor()
        {
            //setup
            PresetDuplicatesIgnor();
            int preAddCount = sortedList.Count;

            //exec
            sortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, sortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, sortedList[2]);
            Assert.AreNotEqual(Person.Bob_Jameson2, sortedList[2]);
        }

        [Test]
        public void Add_DuplicateAllow()
        {
            //setup
            PresetDuplicatesAllow();
            int preAddCount = sortedList.Count;

            //exec
            sortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount + 1, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, sortedList[1]);
            Assert.IsTrue(
                (sortedList[2].Equals(Person.Bob_Jameson) && sortedList[3].Equals(Person.Bob_Jameson2)) ||
                (sortedList[2].Equals(Person.Bob_Jameson2) && sortedList[3].Equals(Person.Bob_Jameson)) );
        }
        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            //exec
            sortedList.Clear();

            //test
            Assert.AreEqual(0, sortedList.Count);
        }
        #endregion

        #region Contains

        [Test]
        public void Contains_True()
        {
            //exec
            bool result = sortedList.Contains(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = sortedList.Contains(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_Fail_Null()
        {
            try
            {
                //exec
                sortedList.Contains(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region CopyTo

        [Test]
        public void CopyTo()
        {
            //setup
            Person[] array = new Person[3];

            //exec
            sortedList.CopyTo(array, 0);

            //test
            Assert.AreEqual(0, Array.IndexOf(array, Person.Fred_Carlile));
            Assert.AreEqual(1, Array.IndexOf(array, Person.Amy_Cathson));
            Assert.AreEqual(2, Array.IndexOf(array, Person.Bob_Jameson));
        }

        [Test]
        public void CopyTo_NonZero()
        {
            //setup
            Person[] array = new Person[5];

            //exec
            sortedList.CopyTo(array, 2);

            //test
            Assert.AreEqual(2, Array.IndexOf(array, Person.Fred_Carlile));
            Assert.AreEqual(3, Array.IndexOf(array, Person.Amy_Cathson));
            Assert.AreEqual(4, Array.IndexOf(array, Person.Bob_Jameson));
        }
        #endregion

        #region Count

        [Test]
        public void Count()
        {
            //exec
            int count = sortedList.Count;

            //test
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [Test]
        public void GetEnumerator()
        {
            //exec
            var enumerator = sortedList.GetEnumerator();

            //test
            Assert.IsNotNull(enumerator);

            int i = 0;
            foreach (var person in sortedList)
            {
                if ((person != Person.Bob_Jameson) &&
                    (person != Person.Fred_Carlile) &&
                    (person != Person.Amy_Cathson))
                {
                    Assert.Fail("Invalid person in itteration.");
                }
                i++;
            }
            Assert.AreEqual(3, i);
        }
        #endregion

        #region IsReadOnly

        [Test]
        public void IsReadOnly()
        {
            //exec
            bool isReadonly = ((IList<Person>)sortedList).IsReadOnly;

            //test
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Remove

        [Test]
        public void Remove_InCollection()
        {
            //exec
            bool result = sortedList.Remove(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, sortedList.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = sortedList.Remove(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, sortedList.Count);
        }

        [Test]
        public void Remove_Fail_Null()
        {
            try
            {
                //exec
                sortedList.Remove(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region RemoveAt

        [Test]
        public void RemoveAt_InCollection()
        {
            //exec
            sortedList.RemoveAt(1);

            //test
            Assert.AreEqual(2, sortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, sortedList[0]);
            Assert.AreEqual(Person.Bob_Jameson, sortedList[1]);
        }

        [Test]
        public void RemoveAt_Fail_TooLarge()
        {
            try
            {
                //exec
                sortedList.RemoveAt(5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void RemoveAt_Fail_Negative()
        {
            try
            {
                //exec
                sortedList.RemoveAt(-2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region RemoveAll

        [Test]
        public void RemoveAll()
        {
            //exec
            int count = sortedList.RemoveAll(person => person.FamilyName.StartsWith("C"));

            //test
            Assert.AreEqual(2, count);
            Assert.AreEqual(1, sortedList.Count);
        }

        [Test]
        public void RemoveAll_None()
        {
            //exec
            int count = sortedList.RemoveAll(person => false);

            //test
            Assert.AreEqual(0, count);
            Assert.AreEqual(3, sortedList.Count);
        }

        [Test]
        public void RemoveAll_Fail_Null()
        {
            try
            {
                //exec
                sortedList.RemoveAll(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region RemoveRange

        [Test]
        public void RemoveRange()
        {
            //exec
            sortedList.RemoveRange(0, 2);

            //test
            Assert.AreEqual(1, sortedList.Count);
        }

        [Test]
        public void RemoveRange_Fail_IndexInvalid()
        {
            try
            {
                //exec
                sortedList.RemoveRange(-1, 2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void RemoveRange_Fail_CountInvalid()
        {
            try
            {
                //exec
                sortedList.RemoveRange(0, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void RemoveRange_Fail_IndexCountInvalid()
        {
            try
            {
                //exec
                sortedList.RemoveRange(1, 5);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion


        #region Private Methods

        private void PresetDuplicatesError()
        {
            sortedList = new SortedList<Person>(DuplicatesPolicy.Error, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesIgnor()
        {
            sortedList = new SortedList<Person>(DuplicatesPolicy.Ignor, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesReplace()
        {
            sortedList = new SortedList<Person>(DuplicatesPolicy.Replace, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesAllow()
        {
            sortedList = new SortedList<Person>(DuplicatesPolicy.Allow, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetPopulate()
        {
            sortedList.Add(Person.Bob_Jameson);
            sortedList.Add(Person.Fred_Carlile);
            sortedList.Add(Person.Amy_Cathson);
            //Person.Jill_Dorrman not added
        }
        #endregion
    }
}
