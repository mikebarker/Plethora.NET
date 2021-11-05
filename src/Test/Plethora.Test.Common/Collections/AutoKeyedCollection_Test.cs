using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class AutoKeyedCollection_Test
    {
        private AutoKeyedCollection<Person> autoKeyedCollection = new AutoKeyedCollection<Person>();

        public AutoKeyedCollection_Test()
        {
            autoKeyedCollection = new AutoKeyedCollection<Person>();
            autoKeyedCollection.Add(Person.Bob_Jameson);
            autoKeyedCollection.Add(Person.Fred_Carlile);
            autoKeyedCollection.Add(Person.Amy_Cathson);
            //Jill_Dorrman not added
        }

        #region Constructors

        [TestMethod]
        public void ctor()
        {
            // Action
            autoKeyedCollection = new AutoKeyedCollection<Person>();

            // Assert
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void ctor_Enumerable()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            // Action
            autoKeyedCollection = new AutoKeyedCollection<Person>(array);

            // Assert
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void ctor_Enumerable_Fail_NullEnumerable()
        {
            try
            {
                // Action
                autoKeyedCollection = new AutoKeyedCollection<Person>((IEnumerable<Person>)null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_EnumerableComparer()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            // Action
            autoKeyedCollection = new AutoKeyedCollection<Person>(array, equalityComparer);

            // Assert
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void ctor_EnumerableComparer_Fail_NullEnumerable()
        {
            // Arrange
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            try
            {
                // Action
                autoKeyedCollection = new AutoKeyedCollection<Person>(null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_Comparer()
        {
            // Arrange
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            // Action
            autoKeyedCollection = new AutoKeyedCollection<Person>(equalityComparer);

            // Assert
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }
        #endregion

        #region Add

        [TestMethod]
        public void Add()
        {
            // Arrange
            int preAddCount = autoKeyedCollection.Count;
            long id = Person.Jill_Dorrman.ID;

            // Action
            autoKeyedCollection.Add(Person.Jill_Dorrman);

            // Assert
            Assert.AreEqual(preAddCount + 1, autoKeyedCollection.Count);
            Assert.AreEqual(Person.Jill_Dorrman, autoKeyedCollection[Person.Jill_Dorrman]);
        }

        [TestMethod]
        public void Add_Fail_Null()
        {
            try
            {
                autoKeyedCollection.Add(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void Add_Fail_Duplicate()
        {
            try
            {
                autoKeyedCollection.Add(Person.Bob_Jameson);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }
        #endregion

        #region Clear

        [TestMethod]
        public void Clear()
        {
            // Action
            autoKeyedCollection.Clear();

            // Assert
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }
        #endregion

        #region Contains

        [TestMethod]
        public void Contains_True()
        {
            // Action
            bool result = autoKeyedCollection.Contains(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_False()
        {
            // Action
            bool result = autoKeyedCollection.Contains(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_Fail_Null()
        {
            try
            {
                // Action
                autoKeyedCollection.Contains(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region ContainsKey

        [TestMethod]
        public void ContainsKey_True()
        {
            // Action
            bool result = autoKeyedCollection.ContainsKey(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsKey_False()
        {
            // Action
            bool result = autoKeyedCollection.ContainsKey(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region CopyTo

        [TestMethod]
        public void CopyTo()
        {
            // Arrange
            Person[] array = new Person[3];

            // Action
            autoKeyedCollection.CopyTo(array, 0);

            // Assert
            Assert.IsTrue(Array.IndexOf(array, Person.Bob_Jameson) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Person.Fred_Carlile) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Person.Amy_Cathson) >= 0);
            Assert.IsFalse(Array.IndexOf(array, Person.Jill_Dorrman) >= 0);
        }

        [TestMethod]
        public void CopyTo_NonZero()
        {
            // Arrange
            Person[] array = new Person[5];

            // Action
            autoKeyedCollection.CopyTo(array, 2);

            // Assert
            Assert.IsTrue(Array.IndexOf(array, Person.Bob_Jameson) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Person.Fred_Carlile) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Person.Amy_Cathson) >= 2);
            Assert.IsFalse(Array.IndexOf(array, Person.Jill_Dorrman) >= 2);
        }
        #endregion

        #region Count

        [TestMethod]
        public void Count()
        {
            // Action
            int count = autoKeyedCollection.Count;

            // Assert
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [TestMethod]
        public void GetEnumerator()
        {
            // Action
            var enumerator = autoKeyedCollection.GetEnumerator();

            // Assert
            Assert.IsNotNull(enumerator);

            int i = 0;
            foreach (var person in autoKeyedCollection)
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
            bool isReadonly = autoKeyedCollection.IsReadOnly;

            // Assert
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Keys

        [TestMethod]
        public void Keys()
        {
            // Arrange
            var ids = new List<Person>();
            ids.Add(Person.Bob_Jameson);
            ids.Add(Person.Fred_Carlile);
            ids.Add(Person.Amy_Cathson);

            // Action
            var keys = autoKeyedCollection.Keys;

            // Assert
            int i = 0;
            foreach (var key in keys)
            {
                bool result = ids.Remove(key);
                if (!result)
                    Assert.Fail("Invalid ID in itteration.");

                i++;
            }
            Assert.AreEqual(3, i);
        }
        #endregion

        #region Remove

        [TestMethod]
        public void Remove_InCollection()
        {
            // Action
            bool result = autoKeyedCollection.Remove(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void Remove_NotInCollection()
        {
            // Action
            bool result = autoKeyedCollection.Remove(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void Remove_Fail_Null()
        {
            try
            {
                // Action
                autoKeyedCollection.Remove(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region RemoveKey

        [TestMethod]
        public void RemoveKey_InCollection()
        {
            // Action
            bool result = autoKeyedCollection.RemoveKey(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [TestMethod]
        public void RemoveKey_NotInCollection()
        {
            // Action
            bool result = autoKeyedCollection.RemoveKey(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }
        #endregion

        #region TryGetValue

        [TestMethod]
        public void TryGetValue_InCollection()
        {
            // Action
            Person person;
            bool result = autoKeyedCollection.TryGetValue(Person.Bob_Jameson, out person);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Person.Bob_Jameson, person);
        }

        [TestMethod]
        public void TryGetValue_NotInCollection()
        {
            // Action
            Person person;
            bool result = autoKeyedCollection.TryGetValue(Person.Jill_Dorrman, out person);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(person);
        }
        #endregion

        #region Upsert

        [TestMethod]
        public void Upsert_NotInCollection()
        {
            // Action
            autoKeyedCollection.Upsert(Person.Jill_Dorrman);

            // Assert
            Assert.AreEqual(4, autoKeyedCollection.Count);
            Assert.IsTrue(autoKeyedCollection.Contains(Person.Jill_Dorrman));
        }

        [TestMethod]
        public void Upsert_InCollection()
        {
            // Action
            autoKeyedCollection.Upsert(Person.Bob_Jameson);

            // Assert
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }
        #endregion

        #region AsReadOnly

        [TestMethod]
        public void AsReadOnly()
        {
            // Action
            IKeyedCollection<Person, Person> readonlyCollection =
                autoKeyedCollection.AsReadOnly();

            // Assert
            Assert.IsNotNull(readonlyCollection);
            Assert.IsTrue(readonlyCollection.IsReadOnly);
        }
        #endregion

        #region AsReadOnlyDictionary

        [TestMethod]
        public void AsReadOnlyDictionary()
        {
            // Action
            IDictionary<Person, Person> readonlyDictionary =
                autoKeyedCollection.AsReadOnlyDictionary();

            // Assert
            Assert.IsNotNull(readonlyDictionary);
            Assert.IsTrue(readonlyDictionary.IsReadOnly);
        }
        #endregion

        #region ToKeyedCollection Extension Method

        [TestMethod]
        public void ToKeyedCollection()
        {
            // Arrange
            IEnumerable<Person> people = new List<Person>
                {
                    Person.Bob_Jameson,
                    Person.Fred_Carlile,
                    Person.Amy_Cathson
                };

            // Action
            KeyedCollection<long, Person> collection = people.ToKeyedCollection(person => person.ID);

            // Assert
            Assert.IsNotNull(collection);
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void ToKeyedCollection_Fail_Duplicate()
        {
            // Arrange
            IEnumerable<Person> people = new List<Person>
                {
                    Person.Bob_Jameson,
                    Person.Bob_Jameson,
                    Person.Fred_Carlile,
                };

            try
            {
                // Action
                people.ToKeyedCollection(person => person.ID);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }
        #endregion
    }
}
