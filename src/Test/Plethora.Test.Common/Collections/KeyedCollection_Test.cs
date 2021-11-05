using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class KeyedCollection_Test
    {
        private KeyedCollection<long, Person> keyedCollection;

        public KeyedCollection_Test()
        {
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Person.Bob_Jameson);
            keyedCollection.Add(Person.Fred_Carlile);
            keyedCollection.Add(Person.Amy_Cathson);
            //Person.Jill_Dorrman not added
        }

        #region Constructors

        [TestMethod]
        public void ctor_KeySelector()
        {
            // Action
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID);

            // Assert
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(0, keyedCollection.Count);
        }

        [TestMethod]
        public void ctor_KeySelector_Fail_Null()
        {
            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerable()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            // Action
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, array);

            // Assert
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerable_Fail_NullSelector()
        {
            // Arrange
            Person[] array = new[] {Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson};

            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(null, array);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerable_Fail_NullEnumerable()
        {
            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(person => person.ID, (IEnumerable<Person>)null);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerableComparer()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            // Action
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, array, equalityComparer);

            // Assert
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullSelector()
        {
            // Arrange
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(null, array, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullEnumerable()
        {
            // Arrange
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(person => person.ID, null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }

        [TestMethod]
        public void ctor_KeySelectorComparer()
        {
            // Arrange
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            // Action
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, equalityComparer);

            // Assert
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(0, keyedCollection.Count);
        }

        [TestMethod]
        public void ctor_KeySelectorComparer_Fail_NullSelector()
        {
            // Arrange
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                // Action
                keyedCollection = new KeyedCollection<long, Person>(null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
            }
        }
        #endregion

        #region Add

        [TestMethod]
        public void Add()
        {
            // Arrange
            int preAddCount = keyedCollection.Count;
            long id = Person.Jill_Dorrman.ID;

            // Action
            keyedCollection.Add(Person.Jill_Dorrman);

            // Assert
            Assert.AreEqual(preAddCount + 1, keyedCollection.Count);
            Assert.AreEqual(Person.Jill_Dorrman, keyedCollection[id]);
        }

        [TestMethod]
        public void Add_Fail_Null()
        {
            try
            {
                keyedCollection.Add(null);

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
                keyedCollection.Add(Person.Bob_Jameson);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Add_Fail_DifferentObjectSameKey()
        {
            // Arrange
            long bobId = Person.Bob_Jameson.ID;
            Person Jane_Doe = new Person(bobId, "Doe", "Jane", new DateTime(1976, 03, 15));

            try
            {
                // Action
                keyedCollection.Add(Jane_Doe);

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
            keyedCollection.Clear();

            // Assert
            Assert.AreEqual(0, keyedCollection.Count);
        }
        #endregion

        #region Contains

        [TestMethod]
        public void Contains_True()
        {
            // Action
            bool result = keyedCollection.Contains(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_False()
        {
            // Action
            bool result = keyedCollection.Contains(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contains_Fail_Null()
        {
            try
            {
                // Action
                keyedCollection.Contains(null);

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
            // Arrange
            long id = Person.Bob_Jameson.ID;

            // Action
            bool result = keyedCollection.ContainsKey(id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsKey_False()
        {
            // Arrange
            long id = Person.Jill_Dorrman.ID;

            // Action
            bool result = keyedCollection.ContainsKey(id);

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
            keyedCollection.CopyTo(array, 0);

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
            keyedCollection.CopyTo(array, 2);

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
            int count = keyedCollection.Count;

            // Assert
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [TestMethod]
        public void GetEnumerator()
        {
            // Action
            var enumerator = keyedCollection.GetEnumerator();

            // Assert
            Assert.IsNotNull(enumerator);

            int i = 0;
            foreach (var person in keyedCollection)
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
            bool isReadonly = keyedCollection.IsReadOnly;

            // Assert
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Keys

        [TestMethod]
        public void Keys()
        {
            // Arrange
            var ids = new List<long>();
            ids.Add(Person.Bob_Jameson.ID);
            ids.Add(Person.Fred_Carlile.ID);
            ids.Add(Person.Amy_Cathson.ID);

            // Action
            var keys = keyedCollection.Keys;

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
            bool result = keyedCollection.Remove(Person.Bob_Jameson);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [TestMethod]
        public void Remove_NotInCollection()
        {
            // Action
            bool result = keyedCollection.Remove(Person.Jill_Dorrman);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [TestMethod]
        public void Remove_Fail_Null()
        {
            try
            {
                // Action
                keyedCollection.Remove(null);

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
            bool result = keyedCollection.RemoveKey(Person.Bob_Jameson.ID);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [TestMethod]
        public void RemoveKey_NotInCollection()
        {
            // Action
            bool result = keyedCollection.RemoveKey(Person.Jill_Dorrman.ID);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(3, keyedCollection.Count);
        }
        #endregion

        #region TryGetValue

        [TestMethod]
        public void TryGetValue_InCollection()
        {
            // Arrange
            long bob_ID = Person.Bob_Jameson.ID;

            // Action
            Person person;
            bool result = keyedCollection.TryGetValue(bob_ID, out person);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Person.Bob_Jameson, person);
        }

        [TestMethod]
        public void TryGetValue_NotInCollection()
        {
            // Arrange
            long jill_ID = Person.Jill_Dorrman.ID;

            // Action
            Person person;
            bool result = keyedCollection.TryGetValue(jill_ID, out person);

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
            keyedCollection.Upsert(Person.Jill_Dorrman);

            // Assert
            Assert.AreEqual(4, keyedCollection.Count);
            Assert.IsTrue(keyedCollection.Contains(Person.Jill_Dorrman));
        }

        [TestMethod]
        public void Upsert_InCollection()
        {
            // Action
            keyedCollection.Upsert(Person.Bob_Jameson);

            // Assert
            Assert.AreEqual(3, keyedCollection.Count);
        }
        #endregion

        #region AsReadOnly

        [TestMethod]
        public void AsReadOnly()
        {
            // Action
            IKeyedCollection<long, Person> readonlyCollection =
                keyedCollection.AsReadOnly();

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
            IDictionary<long, Person> readonlyDictionary =
                keyedCollection.AsReadOnlyDictionary();

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
