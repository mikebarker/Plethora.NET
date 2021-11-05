using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class SortedByKeyList_Test
    {
        private SortedByKeyList<DateTime, Person> sortedList = new SortedByKeyList<DateTime, Person>(person => person.DateOfBirth, DuplicatesPolicy.Allow);

        public SortedByKeyList_Test()
        {
            sortedList.Add(Person.Bob_Jameson);
            sortedList.Add(Person.Bob_Jameson2);
            sortedList.Add(Person.Fred_Carlile);
            sortedList.Add(Person.Amy_Cathson);
            sortedList.Add(Person.Jill_Dorrman);
        }

        [TestMethod]
        public void Remove_SingleElement()
        {
            // Arrange
            sortedList = new SortedByKeyList<DateTime, Person>(person => person.DateOfBirth, DuplicatesPolicy.Allow);
            sortedList.Add(Person.Bob_Jameson);

            //Exec
            sortedList.Remove(Person.Bob_Jameson);

            // Assert
            Assert.AreEqual(0, sortedList.Count);
        }

        [TestMethod]
        public void Remove()
        {
            //Exec
            sortedList.Remove(Person.Bob_Jameson);

            // Assert
            Assert.AreEqual(4, sortedList.Count);
        }
    }
}
