using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class AutoSortedList_Test
    {
        private AutoSortedList<Person> autoSortedList;

        public AutoSortedList_Test()
        {
            PresetDuplicatesError();
        }

        #region Constructors

        [TestMethod]
        public void ctor_Empty()
        {
            // Action
            this.autoSortedList = new AutoSortedList<Person>();

            // Assert
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [TestMethod]
        public void ctor_Comparer()
        {
            // Action
            this.autoSortedList = new AutoSortedList<Person>(new Person.NameComparer());

            // Assert
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [TestMethod]
        public void ctor_Comparer_Fail_Null()
        {
            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_DuplicatesPolicyComparer()
        {
            // Action
            this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Error, new Person.NameComparer());

            // Assert
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(0, this.autoSortedList.Count);
        }

        [TestMethod]
        public void ctor_DuplicatesPolicyComparer_Fail_InvalidDuplicatesPolicy()
        {
            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>((DuplicatesPolicy)78, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void ctor_DuplicatesPolicyComparer_Fail_NullComparer()
        {
            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(DuplicatesPolicy.Error, null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_EnumerableDuplicatesPolicyComparer()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            // Action
            this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

            // Assert
            Assert.IsNotNull(this.autoSortedList);
            Assert.AreEqual(3, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[2]);
        }

        [TestMethod]
        public void ctor_EnumerableDuplicatesPolicyComparer_Fail_NullEnumerable()
        {
            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(null, DuplicatesPolicy.Error, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_EnumerableDuplicatesPolicyComparer_Fail_InvalidDuplicatesPolicy()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(array, (DuplicatesPolicy)78, new Person.NameComparer());

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullComparer()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerableComparer_Fail_DuplicateError()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Bob_Jameson, Person.Amy_Cathson };

            try
            {
                // Action
                this.autoSortedList = new AutoSortedList<Person>(array, DuplicatesPolicy.Error, new Person.NameComparer());

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }
        #endregion

        #region Add

        [TestMethod]
        public void Add()
        {
            // Arrange
            int preAddCount = this.autoSortedList.Count;

            // Action
            this.autoSortedList.Add(Person.Jill_Dorrman);

            // Assert
            Assert.AreEqual(preAddCount + 1, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Jill_Dorrman, this.autoSortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[3]);
        }

        [TestMethod]
        public void Add_Fail_Null()
        {
            try
            {
                // Action
                this.autoSortedList.Add(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void Add_DuplicateError()
        {
            // Arrange
            PresetDuplicatesError();

            try
            {
                // Action
                this.autoSortedList.Add(Person.Bob_Jameson2);

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void Add_DuplicateReplace()
        {
            // Arrange
            PresetDuplicatesReplace();
            int preAddCount = this.autoSortedList.Count;

            // Action
            this.autoSortedList.Add(Person.Bob_Jameson2);

            // Assert
            Assert.AreEqual(preAddCount, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreNotEqual(Person.Bob_Jameson, this.autoSortedList[2]);
            Assert.AreEqual(Person.Bob_Jameson2, this.autoSortedList[2]);
        }

        [TestMethod]
        public void Add_DuplicateIgnor()
        {
            // Arrange
            PresetDuplicatesIgnor();
            int preAddCount = this.autoSortedList.Count;

            // Action
            this.autoSortedList.Add(Person.Bob_Jameson2);

            // Assert
            Assert.AreEqual(preAddCount, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[2]);
            Assert.AreNotEqual(Person.Bob_Jameson2, this.autoSortedList[2]);
        }

        [TestMethod]
        public void Add_DuplicateAllow()
        {
            // Arrange
            PresetDuplicatesAllow();
            int preAddCount = this.autoSortedList.Count;

            // Action
            this.autoSortedList.Add(Person.Bob_Jameson2);

            // Assert
            Assert.AreEqual(preAddCount + 1, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Amy_Cathson, this.autoSortedList[1]);
            Assert.IsTrue(
                (this.autoSortedList[2].Equals(Person.Bob_Jameson) && this.autoSortedList[3].Equals(Person.Bob_Jameson2)) ||
                (this.autoSortedList[2].Equals(Person.Bob_Jameson2) && this.autoSortedList[3].Equals(Person.Bob_Jameson)) );
        }
        #endregion

        #region Clear

        [TestMethod]
        public void Clear()
        {
            // Action
            this.autoSortedList.Clear();

            // Assert
            Assert.AreEqual(0, this.autoSortedList.Count);
        }
        #endregion

        #region Contains

        [TestMethod]
        public void Contains_True()
        {
            // Action
            bool result = this.autoSortedList.Contains(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_False()
        {
            // Action
            bool result = this.autoSortedList.Contains(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_Fail_Null()
        {
            try
            {
                // Action
                this.autoSortedList.Contains(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region CopyTo

        [TestMethod]
        public void CopyTo()
        {
            // Arrange
            Person[] array = new Person[3];

            // Action
            this.autoSortedList.CopyTo(array, 0);

            // Assert
            Assert.AreEqual(0, Array.IndexOf(array, Person.Fred_Carlile));
            Assert.AreEqual(1, Array.IndexOf(array, Person.Amy_Cathson));
            Assert.AreEqual(2, Array.IndexOf(array, Person.Bob_Jameson));
        }

        [TestMethod]
        public void CopyTo_NonZero()
        {
            // Arrange
            Person[] array = new Person[5];

            // Action
            this.autoSortedList.CopyTo(array, 2);

            // Assert
            Assert.AreEqual(2, Array.IndexOf(array, Person.Fred_Carlile));
            Assert.AreEqual(3, Array.IndexOf(array, Person.Amy_Cathson));
            Assert.AreEqual(4, Array.IndexOf(array, Person.Bob_Jameson));
        }
        #endregion

        #region Count

        [TestMethod]
        public void Count()
        {
            // Action
            int count = this.autoSortedList.Count;

            // Assert
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [TestMethod]
        public void GetEnumerator()
        {
            // Action
            var enumerator = this.autoSortedList.GetEnumerator();

            // Assert
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

        [TestMethod]
        public void IsReadOnly()
        {
            // Action
            bool isReadonly = ((IList<Person>)this.autoSortedList).IsReadOnly;

            // Assert
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Remove

        [TestMethod]
        public void Remove_InCollection()
        {
            // Action
            bool result = this.autoSortedList.Remove(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, this.autoSortedList.Count);
        }

        [TestMethod]
        public void Remove_NotInCollection()
        {
            // Action
            bool result = this.autoSortedList.Remove(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, this.autoSortedList.Count);
        }

        [TestMethod]
        public void Remove_Fail_Null()
        {
            try
            {
                // Action
                this.autoSortedList.Remove(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region RemoveAt

        [TestMethod]
        public void RemoveAt_InCollection()
        {
            // Action
            this.autoSortedList.RemoveAt(1);

            // Assert
            Assert.AreEqual(2, this.autoSortedList.Count);
            Assert.AreEqual(Person.Fred_Carlile, this.autoSortedList[0]);
            Assert.AreEqual(Person.Bob_Jameson, this.autoSortedList[1]);
        }

        [TestMethod]
        public void RemoveAt_Fail_TooLarge()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveAt(5);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void RemoveAt_Fail_Negative()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveAt(-2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        #endregion

        #region RemoveAll

        [TestMethod]
        public void RemoveAll()
        {
            // Action
            int count = this.autoSortedList.RemoveAll(person => person.FamilyName.StartsWith("C"));

            // Assert
            Assert.AreEqual(2, count);
            Assert.AreEqual(1, this.autoSortedList.Count);
        }

        [TestMethod]
        public void RemoveAll_None()
        {
            // Action
            int count = this.autoSortedList.RemoveAll(person => false);

            // Assert
            Assert.AreEqual(0, count);
            Assert.AreEqual(3, this.autoSortedList.Count);
        }

        [TestMethod]
        public void RemoveAll_Fail_Null()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveAll(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region RemoveRange

        [TestMethod]
        public void RemoveRange()
        {
            // Action
            this.autoSortedList.RemoveRange(0, 2);

            // Assert
            Assert.AreEqual(1, this.autoSortedList.Count);
        }

        [TestMethod]
        public void RemoveRange_Fail_IndexInvalid()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveRange(-1, 2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void RemoveRange_Fail_CountInvalid()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveRange(0, -2);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void RemoveRange_Fail_IndexCountInvalid()
        {
            try
            {
                // Action
                this.autoSortedList.RemoveRange(1, 5);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
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
