using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class KeyedCollection_Test
    {
        private KeyedCollection<long, Person> keyedCollection;

        [SetUp]
        public void SetUp()
        {
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Person.Bob_Jameson);
            keyedCollection.Add(Person.Fred_Carlile);
            keyedCollection.Add(Person.Amy_Cathson);
            //Person.Jill_Dorrman not added
        }

        #region Constructors

        [Test]
        public void ctor_KeySelector()
        {
            //exec
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID);

            //test
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(0, keyedCollection.Count);
        }

        [Test]
        public void ctor_KeySelector_Fail_Null()
        {
            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerable()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };

            //exec
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, array);

            //test
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [Test]
        public void ctor_KeySelectorEnumerable_Fail_NullSelector()
        {
            //setup
            Person[] array = new[] {Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson};

            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(null, array);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerable_Fail_NullEnumerable()
        {
            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(person => person.ID, (IEnumerable<Person>)null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerableComparer()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            //exec
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, array, equalityComparer);

            //test
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [Test]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullSelector()
        {
            //setup
            Person[] array = new[] { Person.Bob_Jameson, Person.Fred_Carlile, Person.Amy_Cathson };
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(null, array, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorEnumerableComparer_Fail_NullEnumerable()
        {
            //setup
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(person => person.ID, null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ctor_KeySelectorComparer()
        {
            //setup
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            //exec
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID, equalityComparer);

            //test
            Assert.IsNotNull(keyedCollection);
            Assert.AreEqual(0, keyedCollection.Count);
        }

        [Test]
        public void ctor_KeySelectorComparer_Fail_NullSelector()
        {
            //setup
            EqualityComparer<long> equalityComparer = EqualityComparer<long>.Default;

            try
            {
                //exec
                keyedCollection = new KeyedCollection<long, Person>(null, equalityComparer);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
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
            int preAddCount = keyedCollection.Count;
            long id = Person.Jill_Dorrman.ID;

            //exec
            keyedCollection.Add(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, keyedCollection.Count);
            Assert.AreEqual(Person.Jill_Dorrman, keyedCollection[id]);
        }

        [Test]
        public void Add_Fail_Null()
        {
            try
            {
                keyedCollection.Add(null);

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
                keyedCollection.Add(Person.Bob_Jameson);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void Add_Fail_DifferentObjectSameKey()
        {
            //setup
            long bobId = Person.Bob_Jameson.ID;
            Person Jane_Doe = new Person(bobId, "Doe", "Jane", new DateTime(1976, 03, 15));

            try
            {
                //exec
                keyedCollection.Add(Jane_Doe);

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
            keyedCollection.Clear();

            //test
            Assert.AreEqual(0, keyedCollection.Count);
        }
        #endregion

        #region Contains

        [Test]
        public void Contains_True()
        {
            //exec
            bool result = keyedCollection.Contains(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = keyedCollection.Contains(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_Fail_Null()
        {
            try
            {
                //exec
                keyedCollection.Contains(null);

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
            //setup
            long id = Person.Bob_Jameson.ID;

            //exec
            bool result = keyedCollection.ContainsKey(id);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //setup
            long id = Person.Jill_Dorrman.ID;

            //exec
            bool result = keyedCollection.ContainsKey(id);

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
            keyedCollection.CopyTo(array, 0);

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
            keyedCollection.CopyTo(array, 2);

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
            int count = keyedCollection.Count;

            //test
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [Test]
        public void GetEnumerator()
        {
            //exec
            var enumerator = keyedCollection.GetEnumerator();

            //test
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

        [Test]
        public void IsReadOnly()
        {
            //exec
            bool isReadonly = keyedCollection.IsReadOnly;

            //test
            Assert.IsFalse(isReadonly);
        }
        #endregion

        #region Keys

        [Test]
        public void Keys()
        {
            //setup
            var ids = new List<long>();
            ids.Add(Person.Bob_Jameson.ID);
            ids.Add(Person.Fred_Carlile.ID);
            ids.Add(Person.Amy_Cathson.ID);

            //exec
            var keys = keyedCollection.Keys;

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
            bool result = keyedCollection.Remove(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = keyedCollection.Remove(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, keyedCollection.Count);
        }

        [Test]
        public void Remove_Fail_Null()
        {
            try
            {
                //exec
                keyedCollection.Remove(null);

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
            bool result = keyedCollection.RemoveKey(Person.Bob_Jameson.ID);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [Test]
        public void RemoveKey_NotInCollection()
        {
            //exec
            bool result = keyedCollection.RemoveKey(Person.Jill_Dorrman.ID);

            //test
            Assert.IsFalse(result);
            Assert.AreEqual(3, keyedCollection.Count);
        }
        #endregion

        #region TryGetValue

        [Test]
        public void TryGetValue_InCollection()
        {
            //setup
            long bob_ID = Person.Bob_Jameson.ID;

            //exec
            Person person;
            bool result = keyedCollection.TryGetValue(bob_ID, out person);

            //test
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Person.Bob_Jameson, person);
        }

        [Test]
        public void TryGetValue_NotInCollection()
        {
            //setup
            long jill_ID = Person.Jill_Dorrman.ID;

            //exec
            Person person;
            bool result = keyedCollection.TryGetValue(jill_ID, out person);

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
            keyedCollection.Upsert(Person.Jill_Dorrman);

            //test
            Assert.AreEqual(4, keyedCollection.Count);
            Assert.IsTrue(keyedCollection.Contains(Person.Jill_Dorrman));
        }

        [Test]
        public void Upsert_InCollection()
        {
            //exec
            keyedCollection.Upsert(Person.Bob_Jameson);

            //test
            Assert.AreEqual(3, keyedCollection.Count);
        }
        #endregion

        #region AsReadOnly

        [Test]
        public void AsReadOnly()
        {
            //exec
            IKeyedCollection<long, Person> readonlyCollection =
                keyedCollection.AsReadOnly();

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
            IDictionary<long, Person> readonlyDictionary =
                keyedCollection.AsReadOnlyDictionary();

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
