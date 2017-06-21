using System;
using System.Collections.Generic;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class ReadOnlyKeyedCollection_Test
    {
        private ReadOnlyKeyedCollection<long, Person> readonlyKeyedCollection;

        [SetUp]
        public void SetUp()
        {
            var keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Person.Bob_Jameson);
            keyedCollection.Add(Person.Fred_Carlile);
            keyedCollection.Add(Person.Amy_Cathson);
            //Jill_Dorrman not added

            readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(keyedCollection);
        }

        #region Constructors

        [Test]
        public void ctor()
        {
            var keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Person.Bob_Jameson);
            keyedCollection.Add(Person.Fred_Carlile);
            keyedCollection.Add(Person.Amy_Cathson);

            readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(keyedCollection);

            //test
            Assert.IsNotNull(readonlyKeyedCollection);
            Assert.IsTrue(readonlyKeyedCollection.IsReadOnly);
            Assert.AreEqual(keyedCollection.Count, readonlyKeyedCollection.Count);
        }

        [Test]
        public void ctor_Fail_Null()
        {
            try
            {
                //exec
                readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(null);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }
        #endregion

        #region Contains

        [Test]
        public void Contains_True()
        {
            //exec
            bool result = readonlyKeyedCollection.Contains(Person.Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = readonlyKeyedCollection.Contains(Person.Jill_Dorrman);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void Contains_Fail_Null()
        {
            try
            {
                //exec
                readonlyKeyedCollection.Contains(null);

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
            bool result = readonlyKeyedCollection.ContainsKey(id);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //setup
            long id = Person.Jill_Dorrman.ID;

            //exec
            bool result = readonlyKeyedCollection.ContainsKey(id);

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
            readonlyKeyedCollection.CopyTo(array, 0);

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
            readonlyKeyedCollection.CopyTo(array, 2);

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
            int count = readonlyKeyedCollection.Count;

            //test
            Assert.AreEqual(3, count);
        }
        #endregion

        #region GetEnumerator

        [Test]
        public void GetEnumerator()
        {
            //exec
            var enumerator = readonlyKeyedCollection.GetEnumerator();

            //test
            Assert.IsNotNull(enumerator);

            int i = 0;
            foreach (var person in readonlyKeyedCollection)
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
            bool isReadonly = readonlyKeyedCollection.IsReadOnly;

            //test
            Assert.IsTrue(isReadonly);
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
            var keys = readonlyKeyedCollection.Keys;

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

        #region TryGetValue

        [Test]
        public void TryGetValue_InCollection()
        {
            //setup
            long bob_ID = Person.Bob_Jameson.ID;

            //exec
            Person person;
            bool result = readonlyKeyedCollection.TryGetValue(bob_ID, out person);

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
            bool result = readonlyKeyedCollection.TryGetValue(jill_ID, out person);

            //test
            Assert.IsFalse(result);
            Assert.IsNull(person);
        }
        #endregion

        #region Indexor

        [Test]
        public void Indexor_InCollection()
        {
            //setup
            long fred_ID = Person.Fred_Carlile.ID;

            //exec
            Person person = readonlyKeyedCollection[fred_ID];

            //test
            Assert.IsNotNull(person);
            Assert.AreEqual(Person.Fred_Carlile, person);
        }

        [Test]
        public void Indexor_NotInCollection()
        {
            //setup
            long jill_ID = Person.Jill_Dorrman.ID;
            try
            {
                //exec
                Person person = readonlyKeyedCollection[jill_ID];

                Assert.Fail();
            }
            catch (KeyNotFoundException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        #endregion
    }
}
