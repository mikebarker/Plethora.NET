using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Test.UtilityClasses;

namespace Plethora.Collections.Test
{
    [TestFixture]
    public class AutoKeyedCollection_Test
    {
        AutoKeyedCollection<Person> autoKeyedCollection = new AutoKeyedCollection<Person>();
        private readonly Person Bob_Jameson = new Person("Jameson", "Bob", new DateTime(1964, 03, 14));
        private readonly Person Fred_Carlile = new Person("Carlile", "Fred", new DateTime(1975, 11, 07));
        private readonly Person Amy_Cathson = new Person("Cathson", "Amy", new DateTime(1984, 02, 21));
        private readonly Person Jill_Dorrman = new Person("Dorrman", "Jill", new DateTime(1978, 05, 08));

        [SetUp]
        public void SetUp()
        {
            autoKeyedCollection = new AutoKeyedCollection<Person>();
            autoKeyedCollection.Add(Bob_Jameson);
            autoKeyedCollection.Add(Fred_Carlile);
            autoKeyedCollection.Add(Amy_Cathson);
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
            Person[] array = new[] { Bob_Jameson, Fred_Carlile, Amy_Cathson };

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
            Person[] array = new[] { Bob_Jameson, Fred_Carlile, Amy_Cathson };
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
            long id = Jill_Dorrman.ID;

            //exec
            autoKeyedCollection.Add(Jill_Dorrman);

            //test
            Assert.AreEqual(preAddCount + 1, autoKeyedCollection.Count);
            Assert.AreEqual(Jill_Dorrman, autoKeyedCollection[Jill_Dorrman]);
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
                autoKeyedCollection.Add(Bob_Jameson);

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
            bool result = autoKeyedCollection.Contains(Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = autoKeyedCollection.Contains(Jill_Dorrman);

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
            bool result = autoKeyedCollection.ContainsKey(Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //exec
            bool result = autoKeyedCollection.ContainsKey(Jill_Dorrman);

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
            autoKeyedCollection.CopyTo(array, 2);

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
            ids.Add(Bob_Jameson);
            ids.Add(Fred_Carlile);
            ids.Add(Amy_Cathson);

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
            bool result = autoKeyedCollection.Remove(Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [Test]
        public void Remove_NotInCollection()
        {
            //exec
            bool result = autoKeyedCollection.Remove(Jill_Dorrman);

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
            bool result = autoKeyedCollection.RemoveKey(Bob_Jameson);

            //test
            Assert.IsTrue(result);
            Assert.AreEqual(2, autoKeyedCollection.Count);
        }

        [Test]
        public void RemoveKey_NotInCollection()
        {
            //exec
            bool result = autoKeyedCollection.RemoveKey(Jill_Dorrman);

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
            bool result = autoKeyedCollection.TryGetValue(Bob_Jameson, out person);

            //test
            Assert.IsTrue(result);
            Assert.IsNotNull(person);
            Assert.AreEqual(Bob_Jameson, person);
        }

        [Test]
        public void TryGetValue_NotInCollection()
        {
            //exec
            Person person;
            bool result = autoKeyedCollection.TryGetValue(Jill_Dorrman, out person);

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
            autoKeyedCollection.Upsert(Jill_Dorrman);

            //test
            Assert.AreEqual(4, autoKeyedCollection.Count);
            Assert.IsTrue(autoKeyedCollection.Contains(Jill_Dorrman));
        }

        [Test]
        public void Upsert_InCollection()
        {
            //exec
            autoKeyedCollection.Upsert(Bob_Jameson);

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
