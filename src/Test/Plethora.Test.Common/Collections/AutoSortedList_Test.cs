using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class AutoSortedList_Test
    {
        private AutoSortedList<Person> autoSortedList;

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
            this.autoSortedList = new AutoSortedList<Person>();

            //test
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [Test]
        public void ctor_Comparer()
        {
            //exec
            this.autoSortedList = new AutoSortedList<Person>(new Person.NameComparer());

            //test
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [Test]
        public void ctor_Comparer_Fail_Null()
        {
            try
            {
                //exec
                this.autoSortedList = new AutoSortedList<Person>(null);

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
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Error, new Person.NameComparer());

            //test
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [Test]
        public void ctor_DuplicatesPolicyComparer_Fail_InvalidDuplicatesPolicy()
        {
            try
            {
                //exec
                this.autoSortedList = new AutoSortedList<Person>((DuplicatesPolicy)78, new Person.NameComparer());

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
                this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Error, null);

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
            this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

            //test
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(3, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[2]);
        }

        [Test]
        public void ctor_EnumerableDuplicatesPolicyComparer_Fail_NullEnumerable()
        {
            try
            {
                //exec
                this.autoSortedList = new AutoSortedList<Person>(null, DuplicatesPolicy.Error, new Person.NameComparer());

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
                this.autoSortedList = new AutoSortedList<Person>(array, (DuplicatesPolicy)78, new Person.NameComparer());

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
                this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, null);

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
                this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

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
            int preAddCount = this.autoSortedList.Count;

            //exec
            this.autoSortedList.Add(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Jill_Dorrman, this.autoSortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[3]);
        }

        [Test]
        public void Add_Fail_Null()
        {
            try
            {
                //exec
                this.autoSortedList.Add(null);

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
                this.autoSortedList.Add(Person.Bob_Jameson2);

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
            int preAddCount = this.autoSortedList.Count;

            //exec
            this.autoSortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreNotEqual(Person.Bob_Jameson, this.autoSortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson2, this.autoSortedList[2]);
        }

        [Test]
        public void Add_DuplicateIgnor()
        {
            //setup
            PresetDuplicatesIgnor();
            int preAddCount = this.autoSortedList.Count;

            //exec
            this.autoSortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[2]);
            Assert.AreNotEqual(Person.Bob_Jameson2, this.autoSortedList[2]);
        }

        [Test]
        public void Add_DuplicateAllow()
        {
            //setup
            PresetDuplicatesAllow();
            int preAddCount = this.autoSortedList.Count;

            //exec
            this.autoSortedList.Add(Person.Bob_Jameson2);

            //test
            Assert.AreEqual(preAddCount + 1, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.IsTrue(
                (this.autoSortedList[2].Equals(Person.Bob_Jameson) && this.autoSortedList[3].Equals(Person.Bob_Jameson2)) ||
                (this.autoSortedList[2].Equals(Person.Bob_Jameson2) && this.autoSortedList[3].Equals(Person.Bob_Jameson)) );
        }
        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            //exec
            this.autoSortedList.Clear();

            //test
            Assert.AreEqual(0, this.autoSortedList.Count);
        }
        #endregion

        #region Contains

        [Test]
        public void Contains_True()
        {
            //exec
            bool result = this.autoSortedList.Contains(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = this.autoSortedList.Contains(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_Fail_Null()
        {
            try
            {
                //exec
                this.autoSortedList.Contains(null);

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
            this.autoSortedList.CopyTo(array, 0);

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
            this.autoSortedList.CopyTo(array, 2);

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
            int count = this.autoSortedList.Count;

            //test
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [Test]
        public void GetEnumerator()
        {
            //exec
            var enumerator = this.autoSortedList.GetEnumerator();

            //test
            Assert.IsNotNull(enumerator);

            int i = 0;
            foreach (var person in this.autoSortedList)
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
            bool isReadonly = ((IList<Person>)this.autoSortedList).IsReadOnly;

            //test
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Remove

        [Test]
        public void Remove_InCollection()
        {
            //exec
            bool result = this.autoSortedList.Remove(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, this.autoSortedList.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = this.autoSortedList.Remove(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, this.autoSortedList.Count);
        }

        [Test]
        public void Remove_Fail_Null()
        {
            try
            {
                //exec
                this.autoSortedList.Remove(null);

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
            this.autoSortedList.RemoveAt(1);

            //test
            Assert.AreEqual(2, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[1]);
        }

        [Test]
        public void RemoveAt_Fail_TooLarge()
        {
            try
            {
                //exec
                this.autoSortedList.RemoveAt(5);

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
                this.autoSortedList.RemoveAt(-2);

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
            int count = this.autoSortedList.RemoveAll(person => person.FamilyName.StartsWith("C"));

            //test
            Assert.AreEqual(2, count);
            Assert.AreEqual(1, this.autoSortedList.Count);
        }

        [Test]
        public void RemoveAll_None()
        {
            //exec
            int count = this.autoSortedList.RemoveAll(person => false);

            //test
            Assert.AreEqual(0, count);
            Assert.AreEqual(3, this.autoSortedList.Count);
        }

        [Test]
        public void RemoveAll_Fail_Null()
        {
            try
            {
                //exec
                this.autoSortedList.RemoveAll(null);

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
            this.autoSortedList.RemoveRange(0, 2);

            //test
            Assert.AreEqual(1, this.autoSortedList.Count);
        }

        [Test]
        public void RemoveRange_Fail_IndexInvalid()
        {
            try
            {
                //exec
                this.autoSortedList.RemoveRange(-1, 2);

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
                this.autoSortedList.RemoveRange(0, -2);

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
                this.autoSortedList.RemoveRange(1, 5);

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
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Error, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesIgnor()
        {
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Ignor, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesReplace()
        {
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Replace, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetDuplicatesAllow()
        {
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Allow, new Person.NameComparer());
            PresetPopulate();
        }

        private void PresetPopulate()
        {
            this.autoSortedList.Add(Person.Bob_Jameson);
            this.autoSortedList.Add(Person.Fred_Carlile);
            this.autoSortedList.Add(Person.Amy_Cathson);
            //Person.Jill_Dorrman not added
        }
        #endregion
    }
}
