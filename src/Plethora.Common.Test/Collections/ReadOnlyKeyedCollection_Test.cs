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
        private readonly Person Bob_Jameson = new Person("Jameson", "Bob", new DateTime(1964, 03, 14));
        private readonly Person Fred_Carlile = new Person("Carlile", "Fred", new DateTime(1975, 11, 07));
        private readonly Person Amy_Cathson = new Person("Cathson", "Amy", new DateTime(1984, 02, 21));
        private readonly Person Jill_Dorrman = new Person("Dorrman", "Jill", new DateTime(1978, 05, 08));

        [SetUp]
        public void SetUp()
        {
            var keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Bob_Jameson);
            keyedCollection.Add(Fred_Carlile);
            keyedCollection.Add(Amy_Cathson);
            //Jill_Dorrman not added

            readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(keyedCollection);
        }

        #region Constructors

        [Test]
        public void ctor()
        {
            var keyedCollection = new KeyedCollection<long, Person>(person => person.ID);
            keyedCollection.Add(Bob_Jameson);
            keyedCollection.Add(Fred_Carlile);
            keyedCollection.Add(Amy_Cathson);

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
            bool result = readonlyKeyedCollection.Contains(Bob_Jameson);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Contains_False()
        {
            //exec
            bool result = readonlyKeyedCollection.Contains(Jill_Dorrman);

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
            long id = Bob_Jameson.ID;

            //exec
            bool result = readonlyKeyedCollection.ContainsKey(id);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void ContainsKey_False()
        {
            //setup
            long id = Jill_Dorrman.ID;

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
            readonlyKeyedCollection.CopyTo(array, 2);

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
            ids.Add(Bob_Jameson.ID);
            ids.Add(Fred_Carlile.ID);
            ids.Add(Amy_Cathson.ID);

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
            long bob_ID = Bob_Jameson.ID;

            //exec
            Person person;
            bool result = readonlyKeyedCollection.TryGetValue(bob_ID, out person);

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
            bool result = readonlyKeyedCollection.TryGetValue(jill_ID, out person);

            //test
            Assert.IsFalse(result);
            Assert.IsNull(person);
        }
        #endregion
    }
}
