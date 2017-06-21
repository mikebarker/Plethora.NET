using System;
using NUnit.Framework;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestFixture]
    public class SortedByKeyList_Test
    {
        private SortedByKeyList<DateTime, Person> sortedList;

        [SetUp]
        public void SetUp()
        {
            sortedList = new SortedByKeyList<DateTime, Person>(person => person.DateOfBirth, DuplicatesPolicy.Allow);
            sortedList.Add(Person.Bob_Jameson);
            sortedList.Add(Person.Bob_Jameson2);
            sortedList.Add(Person.Fred_Carlile);
            sortedList.Add(Person.Amy_Cathson);
            sortedList.Add(Person.Jill_Dorrman);
        }

        [Test]
        public void Remove_SingleElement()
        {
            //Setup
            sortedList = new SortedByKeyList<DateTime, Person>(person => person.DateOfBirth, DuplicatesPolicy.Allow);
            sortedList.Add(Person.Bob_Jameson);

            //Exec
            sortedList.Remove(Person.Bob_Jameson);

            //Test
            Assert.AreEqual(0, sortedList.Count);
        }

        [Test]
        public void Remove()
        {
            //Exec
            sortedList.Remove(Person.Bob_Jameson);

            //Test
            Assert.AreEqual(4, sortedList.Count);
        }
    }
}
