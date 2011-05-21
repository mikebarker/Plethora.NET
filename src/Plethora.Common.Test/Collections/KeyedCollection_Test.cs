﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Collections.Test
{
    [TestFixture]
    public class KeyedCollection_Test
    {
        private KeyedCollection<long, Person> keyedCollection;
        private readonly Person Bob_Jameson = new Person("Jameson", "Bob", new DateTime(1964, 03, 14));
        private readonly Person Fred_Carlile = new Person("Carlile", "Fred", new DateTime(1975, 11, 07));
        private readonly Person Amy_Cathson = new Person("Cathson", "Amy", new DateTime(1984, 02, 21));
        private readonly Person Jill_Dorrman = new Person("Dorrman", "Jill", new DateTime(1978, 05, 08));

        [SetUp]
        public void SetUp()
        {
            keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Bob_Jameson);
            keyedCollection.Add(Fred_Carlile);
            keyedCollection.Add(Amy_Cathson);
            //Jill_Dorrman not added
        }

        #region Add

        [Test]
        public void Add()
        {
            //setup
            int preAddCount = keyedCollection.Count;
            long id = Jill_Dorrman.ID;

            //exec
            keyedCollection.Add(Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, keyedCollection.Count);
            Assert.AreEqual(Jill_Dorrman, keyedCollection[id]);
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
                keyedCollection.Add(Bob_Jameson);

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
            bool result = keyedCollection.Contains(Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = keyedCollection.Contains(Jill_Dorrman);

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
            long id = Bob_Jameson.ID;

            //exec
            bool result = keyedCollection.ContainsKey(id);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //setup
            long id = Jill_Dorrman.ID;

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
            Assert.IsTrue(Array.IndexOf(array, Bob_Jameson) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Fred_Carlile) >= 0);
            Assert.IsTrue(Array.IndexOf(array, Amy_Cathson) >= 0);
            Assert.IsFalse(Array.IndexOf(array, Jill_Dorrman) >= 0);
        }

        [Test]
        public void CopyTo_NonZero()
        {
            //setup
            Person[] array = new Person[5];

            //exec
            keyedCollection.CopyTo(array, 2);

            //test
            Assert.IsTrue(Array.IndexOf(array, Bob_Jameson) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Fred_Carlile) >= 2);
            Assert.IsTrue(Array.IndexOf(array, Amy_Cathson) >= 2);
            Assert.IsFalse(Array.IndexOf(array, Jill_Dorrman) >= 2);
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
                if ((person != Bob_Jameson) &&
                    (person != Fred_Carlile) &&
                    (person != Amy_Cathson))
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
            ids.Add(Bob_Jameson.ID);
            ids.Add(Fred_Carlile.ID);
            ids.Add(Amy_Cathson.ID);

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
            bool result = keyedCollection.Remove(Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = keyedCollection.Remove(Jill_Dorrman);

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
            bool result = keyedCollection.RemoveKey(Bob_Jameson.ID);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, keyedCollection.Count);
        }

        [Test]
        public void RemoveKey_NotInCollection()
        {
            //exec
            bool result = keyedCollection.RemoveKey(Jill_Dorrman.ID);

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
            long bob_ID = Bob_Jameson.ID;

            //exec
            Person person;
            bool result = keyedCollection.TryGetValue(bob_ID, out person);

            //test
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Bob_Jameson, person);
        }

        [Test]
        public void TryGetValue_NotInCollection()
        {
            //setup
            long jill_ID = Jill_Dorrman.ID;

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
            keyedCollection.Upsert(Jill_Dorrman);

            //test
            Assert.AreEqual(4, keyedCollection.Count);
            Assert.IsTrue(keyedCollection.Contains(Jill_Dorrman));
        }

        [Test]
        public void Upsert_InCollection()
        {
            //exec
            keyedCollection.Upsert(Bob_Jameson);

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
                    Bob_Jameson,
                    Fred_Carlile,
                    Amy_Cathson
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
                    Bob_Jameson,
                    Bob_Jameson,
                    Fred_Carlile,
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
