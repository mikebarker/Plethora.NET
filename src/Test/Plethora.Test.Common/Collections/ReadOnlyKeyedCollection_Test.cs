using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections;

[TestClass]
public class ReadOnlyKeyedCollection_Test
{
    private ReadOnlyKeyedCollection<long, Person> readonlyKeyedCollection;

    public ReadOnlyKeyedCollection_Test()
    {
        KeyedCollection<long, Person> keyedCollection = new(person => person.ID)
        {
            Person.Bob_Jameson,
            Person.Fred_Carlile,
            Person.Amy_Cathson
            //Jill_Dorrman not added
        };

        readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(keyedCollection);
    }

    #region Constructors

    [TestMethod]
    public void ctor()
    {
        KeyedCollection<long, Person> keyedCollection = new(person => person.ID)
        {
            Person.Bob_Jameson,
            Person.Fred_Carlile,
            Person.Amy_Cathson
        };

        readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(keyedCollection);

        // Assert
        Assert.IsNotNull(readonlyKeyedCollection);
        Assert.IsTrue(((ICollection<Person>)readonlyKeyedCollection).IsReadOnly);
        Assert.AreEqual(keyedCollection.Count, readonlyKeyedCollection.Count);
    }

    [TestMethod]
    public void ctor_Fail_Null()
    {
        try
        {
            // Action
            readonlyKeyedCollection = new ReadOnlyKeyedCollection<long, Person>(null);

            Assert.Fail();
        }
        catch (ArgumentNullException)
        {
        }
    }
    #endregion

    #region Contains

    [TestMethod]
    public void Contains_True()
    {
        // Action
        bool result = readonlyKeyedCollection.Contains(Person.Bob_Jameson);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Contains_False()
    {
        // Action
        bool result = readonlyKeyedCollection.Contains(Person.Jill_Dorrman);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Contains_Fail_Null()
    {
        try
        {
            // Action
            readonlyKeyedCollection.Contains(null);

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
        bool result = readonlyKeyedCollection.ContainsKey(id);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ContainsKey_False()
    {
        // Arrange
        long id = Person.Jill_Dorrman.ID;

        // Action
        bool result = readonlyKeyedCollection.ContainsKey(id);

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
        readonlyKeyedCollection.CopyTo(array, 0);

        // Assert
        Assert.IsGreaterThanOrEqualTo(0, Array.IndexOf(array, Person.Bob_Jameson));
        Assert.IsGreaterThanOrEqualTo(0, Array.IndexOf(array, Person.Fred_Carlile));
        Assert.IsGreaterThanOrEqualTo(0, Array.IndexOf(array, Person.Amy_Cathson));
        Assert.IsLessThan(0, Array.IndexOf(array, Person.Jill_Dorrman));
    }

    [TestMethod]
    public void CopyTo_NonZero()
    {
        // Arrange
        Person[] array = new Person[5];

        // Action
        readonlyKeyedCollection.CopyTo(array, 2);

        // Assert
        Assert.IsGreaterThanOrEqualTo(2, Array.IndexOf(array, Person.Bob_Jameson));
        Assert.IsGreaterThanOrEqualTo(2, Array.IndexOf(array, Person.Fred_Carlile));
        Assert.IsGreaterThanOrEqualTo(2, Array.IndexOf(array, Person.Amy_Cathson));
        Assert.IsLessThan(2, Array.IndexOf(array, Person.Jill_Dorrman));
    }
    #endregion

    #region Count

    [TestMethod]
    public void Count()
    {
        // Action
        int count = readonlyKeyedCollection.Count;

        // Assert
        Assert.AreEqual(3, count);
    }
    #endregion

    #region GetEnumerator

    [TestMethod]
    public void GetEnumerator()
    {
        // Action
        var enumerator = readonlyKeyedCollection.GetEnumerator();

        // Assert
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

    [TestMethod]
    public void IsReadOnly()
    {
        // Action
        bool isReadonly = ((ICollection<Person>)readonlyKeyedCollection).IsReadOnly;

        // Assert
        Assert.IsTrue(isReadonly);
    }
    #endregion

    #region Keys

    [TestMethod]
    public void Keys()
    {
        // Arrange
        List<long> ids =
        [
            Person.Bob_Jameson.ID,
            Person.Fred_Carlile.ID,
            Person.Amy_Cathson.ID
        ];

        // Action
        var keys = readonlyKeyedCollection.Keys;

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

    #region TryGetValue

    [TestMethod]
    public void TryGetValue_InCollection()
    {
        // Arrange
        long bob_ID = Person.Bob_Jameson.ID;

        // Action
        Person person;
        bool result = readonlyKeyedCollection.TryGetValue(bob_ID, out person);

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
        bool result = readonlyKeyedCollection.TryGetValue(jill_ID, out person);

        // Assert
        Assert.IsFalse(result);
        Assert.IsNull(person);
    }
    #endregion

    #region Indexor

    [TestMethod]
    public void Indexor_InCollection()
    {
        // Arrange
        long fred_ID = Person.Fred_Carlile.ID;

        // Action
        Person person = readonlyKeyedCollection[fred_ID];

        // Assert
        Assert.IsNotNull(person);
        Assert.AreEqual(Person.Fred_Carlile, person);
    }

    [TestMethod]
    public void Indexor_NotInCollection()
    {
        // Arrange
        long jill_ID = Person.Jill_Dorrman.ID;
        try
        {
            // Action
            Person person = readonlyKeyedCollection[jill_ID];

            Assert.Fail();
        }
        catch (KeyNotFoundException)
        {
        }
    }

    #endregion
}
