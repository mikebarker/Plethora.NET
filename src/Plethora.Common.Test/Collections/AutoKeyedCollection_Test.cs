using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class AutoKeyedCollection_Test
    {
        AutoKeyedCollection<Person> autoKeyedCollection = new AutoKeyedCollection<Person>();

        [SetUp]
        public void SetUp()
        {
            autoKeyedCollection = new AutoKeyedCollection<Person>();
            autoKeyedCollection.Add(Person.Bob_Jameson);
            autoKeyedCollection.Add(Person.Fred_Carlile);
            autoKeyedCollection.Add(Person.Amy_Cathson);
            //Jill_Dorrman not added
        }

        #region Constructors

        [Test]
        public void ctor()
        {
            //exec
            autoKeyedCollection = new AutoKeyedCollection<Person>();

            //test
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }

        [Test]
        public void ctor_Enumerable()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            //exec
            autoKeyedCollection = new AutoKeyedCollection<Person>(array);

            //test
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [Test]
        public void ctor_Enumerable_Fail_NullEnumerable()
        {
            try
            {
                //exec
                autoKeyedCollection = new AutoKeyedCollection<Person>((IEnumerable<Person>)null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_EnumerableComparer()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            //exec
            autoKeyedCollection = new AutoKeyedCollection<Person>(array, equalityComparer);

            //test
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [Test]
        public void ctor_EnumerableComparer_Fail_NullEnumerable()
        {
            //setup
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            try
            {
                //exec
                autoKeyedCollection = new AutoKeyedCollection<Person>(null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_Comparer()
        {
            //setup
            EqualityComparer<Person> equalityComparer = EqualityComparer<Person>.Default;

            //exec
            autoKeyedCollection = new AutoKeyedCollection<Person>(equalityComparer);

            //test
            Assert.IsNotNull(autoKeyedCollection);
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }
        #endregion

        #region Add

        [Test]
        public void Add()
        {
            //setup
            int preAddCount = autoKeyedCollection.Count;
            long id = Person.Jill_Dorrman.ID;

            //exec
            autoKeyedCollection.Add(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, autoKeyedCollection.Count);
            Assert.AreEqual(Person.Jill_Dorrman, autoKeyedCollection[Person.Jill_Dorrman]);
        }

        [Test]
        public void Add_Fail_Null()
        {
            try
            {
                autoKeyedCollection.Add(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Add_Fail_Duplicate()
        {
            try
            {
                autoKeyedCollection.Add(Person.Bob_Jameson);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region Clear

        [Test]
        public void Clear()
        {
            //exec
            autoKeyedCollection.Clear();

            //test
            Assert.AreEqual(0, autoKeyedCollection.Count);
        }
        #endregion

        #region Contains

        [Test]
        public void Contains_True()
        {
            //exec
            bool result = autoKeyedCollection.Contains(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = autoKeyedCollection.Contains(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_Fail_Null()
        {
            try
            {
                //exec
                autoKeyedCollection.Contains(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region ContainsKey

        [Test]
        public void ContainsKey_True()
        {
            //exec
            bool result = autoKeyedCollection.ContainsKey(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //exec
            bool result = autoKeyedCollection.ContainsKey(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }
        #endregion

        #region CopyTo

        [Test]
        public void CopyTo()
        {
            //setup
            Person[] array = new Person[3];

            //exec
            autoKeyedCollection.CopyTo(array, 0);

            //test
            Assert.IsTrue(Array.IndexOf(array, Person.Bob_Jameson) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Person.Fred_Carlile) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Person.Amy_Cathson) >= 0);
            Assert.IsFalse(Array.IndexOf(array, Person.Jill_Dorrman) >= 0);
        }

        [Test]
        public void CopyTo_NonZero()
        {
            //setup
            Person[] array = new Person[5];

            //exec
            autoKeyedCollection.CopyTo(array, 2);

            //test
            Assert.IsTrue(Array.IndexOf(array, Person.Bob_Jameson) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Person.Fred_Carlile) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Person.Amy_Cathson) >= 2);
            Assert.IsFalse(Array.IndexOf(array, Person.Jill_Dorrman) >= 2);
        }
        #endregion

        #region Count

        [Test]
        public void Count()
        {
            //exec
            int count = autoKeyedCollection.Count;

            //test
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [Test]
        public void GetEnumerator()
        {
            //exec
            var enumerator = autoKeyedCollection.GetEnumerator();

            //test
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

        [Test]
        public void IsReadOnly()
        {
            //exec
            bool isReadonly = autoKeyedCollection.IsReadOnly;

            //test
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Keys

        [Test]
        public void Keys()
        {
            //setup
            var ids = new List<Person>();
            ids.Add(Person.Bob_Jameson);
            ids.Add(Person.Fred_Carlile);
            ids.Add(Person.Amy_Cathson);

            //exec
            var keys = autoKeyedCollection.Keys;

            //test
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

        [Test]
        public void Remove_InCollection()
        {
            //exec
            bool result = autoKeyedCollection.Remove(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = autoKeyedCollection.Remove(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }

        [Test]
        public void Remove_Fail_Null()
        {
            try
            {
                //exec
                autoKeyedCollection.Remove(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region RemoveKey

        [Test]
        public void RemoveKey_InCollection()
        {
            //exec
            bool result = autoKeyedCollection.RemoveKey(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [Test]
        public void RemoveKey_NotInCollection()
        {
            //exec
            bool result = autoKeyedCollection.RemoveKey(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }
        #endregion

        #region TryGetValue

        [Test]
        public void TryGetValue_InCollection()
        {
            //exec
            Person person;
            bool result = autoKeyedCollection.TryGetValue(Person.Bob_Jameson, out person);

            //test
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Person.Bob_Jameson, person);
        }

        [Test]
        public void TryGetValue_NotInCollection()
        {
            //exec
            Person person;
            bool result = autoKeyedCollection.TryGetValue(Person.Jill_Dorrman, out person);

            //test
            Assert.IsFalse(result);
            Assert.IsNull(person);
        }
        #endregion

        #region Upsert

        [Test]
        public void Upsert_NotInCollection()
        {
            //exec
            autoKeyedCollection.Upsert(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(4, autoKeyedCollection.Count);
            Assert.IsTrue(autoKeyedCollection.Contains(Person.Jill_Dorrman));
        }

        [Test]
        public void Upsert_InCollection()
        {
            //exec
            autoKeyedCollection.Upsert(Person.Bob_Jameson);

            //test
            Assert.AreEqual(3, autoKeyedCollection.Count);
        }
        #endregion

        #region AsReadOnly

        [Test]
        public void AsReadOnly()
        {
            //exec
            IKeyedCollection<Person, Person> readonlyCollection =
                autoKeyedCollection.AsReadOnly();

            //test
            Assert.IsNotNull(readonlyCollection);
            Assert.IsTrue(readonlyCollection.IsReadOnly);
        }
        #endregion

        #region AsReadOnlyDictionary

        [Test]
        public void AsReadOnlyDictionary()
        {
            //exec
            IDictionary<Person, Person> readonlyDictionary =
                autoKeyedCollection.AsReadOnlyDictionary();

            //test
            Assert.IsNotNull(readonlyDictionary);
            Assert.IsTrue(readonlyDictionary.IsReadOnly);
        }
        #endregion

        #region ToKeyedCollection Extension Method

        [Test]
        public void ToKeyedCollection()
        {
            //setup
            IEnumerable<Person> people = new List<Person>
                {
                    Person.Bob_Jameson,
                    Person.Fred_Carlile,
                    Person.Amy_Cathson
                };

            //exec
            KeyedCollection<long, Person> collection = people.ToKeyedCollection(person => person.ID);

            //test
            Assert.IsNotNull(collection);
            Assert.AreEqual(3, collection.Count);
        }

        [Test]
        public void ToKeyedCollection_Fail_Duplicate()
        {
            //setup
            IEnumerable<Person> people = new List<Person>
                {
                    Person.Bob_Jameson,
                    Person.Bob_Jameson,
                    Person.Fred_Carlile,
                };

            try
            {
                //exec
                people.ToKeyedCollection(person => person.ID);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion
    }
}
