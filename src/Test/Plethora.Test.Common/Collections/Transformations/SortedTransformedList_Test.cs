using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Collections.Transformations;
using Plethora.Test.UtilityClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Plethora.Test.Collections.Transformations
{
    [TestClass]
    public class SortedTransformedList_Test
    {
        private static Func<Person, string> OrderByGivenName => (p) => p.GivenName;

        private static readonly Person[] InitialPeopleList =
        [
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Amy_Cathson,
                Person.Jill_Dorrman,
                Person.Katherine_Harold,
                Person.Harry_Porker,
        ];

        [TestMethod]
        public void Initialise_FilterApplied()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);


            // Action
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Assert
            var expectedResults = new Person[]
            {
                Person.Amy_Cathson,
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Harry_Porker,
                Person.Jill_Dorrman,
                Person.Katherine_Harold,
            };

            Assert.IsTrue(orderedList.SequenceEqual(expectedResults));
            Assert.AreEqual(expectedResults.Length, orderedList.Count);
        }

        [TestMethod]
        public void AddToSource()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            source.Add(Person.Jane_Doe);

            // Assert
            var expectedResults = new Person[]
            {
                Person.Amy_Cathson,
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Harry_Porker,
                Person.Jane_Doe,
                Person.Jill_Dorrman,
                Person.Katherine_Harold,
            };

            Assert.IsTrue(source.Contains(Person.Jane_Doe));
            Assert.IsTrue(orderedList.SequenceEqual(expectedResults));
        }

        [TestMethod]
        public void Add()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            orderedList.Add(Person.Jane_Doe);

            // Assert
            var expectedResults = new Person[]
            {
                Person.Amy_Cathson,
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Harry_Porker,
                Person.Jane_Doe,
                Person.Jill_Dorrman,
                Person.Katherine_Harold,
            };

            Assert.IsTrue(source.Contains(Person.Jane_Doe));
            Assert.IsTrue(orderedList.SequenceEqual(expectedResults));
        }

        [TestMethod]
        public void ClearSource()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            source.Clear();

            // Assert
            Assert.IsTrue(source.SequenceEqual(Array.Empty<Person>()));
            Assert.IsTrue(orderedList.SequenceEqual(Array.Empty<Person>()));

            Assert.AreEqual(0, orderedList.Count);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            orderedList.Clear();

            // Assert
            Assert.IsTrue(source.SequenceEqual(Array.Empty<Person>()));
            Assert.IsTrue(orderedList.SequenceEqual(Array.Empty<Person>()));

            Assert.AreEqual(0, orderedList.Count);
        }

        [TestMethod]
        public void Contains_Included()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            var result = orderedList.Contains(Person.Fred_Carlile);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Contains_Excluded()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            var result = orderedList.Contains(Person.Bob_Jameson2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveFromSource()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            source.Remove(Person.Jill_Dorrman);

            // Assert
            var expectedResults = new Person[]
            {
                Person.Amy_Cathson,
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Harry_Porker,
                Person.Katherine_Harold,
            };

            Assert.IsFalse(source.Contains(Person.Jill_Dorrman));
            Assert.IsTrue(orderedList.SequenceEqual(expectedResults));
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            ObservableCollection<Person> source = new(InitialPeopleList);
            SortedTransformedList<Person, string> orderedList = new(source, OrderByGivenName);

            // Action
            orderedList.Remove(Person.Jill_Dorrman);

            // Assert
            var expectedResults = new Person[]
            {
                Person.Amy_Cathson,
                Person.Bob_Jameson,
                Person.Fred_Carlile,
                Person.Harry_Porker,
                Person.Katherine_Harold,
            };

            Assert.IsFalse(source.Contains(Person.Jill_Dorrman));
            Assert.IsTrue(orderedList.SequenceEqual(expectedResults));
        }
    }
}
