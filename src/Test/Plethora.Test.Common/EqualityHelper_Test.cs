using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test
{
    [TestClass]
    public class EqualityHelper_Test
    {
        private static readonly Person Henry_Baker = new Person(46, "Baker", "Henry", new DateTime(1964, 03, 14));
        private static readonly Person Henry_Baker2 = new Person(72, "Baker", "Henry", new DateTime(1964, 03, 14));

        [TestMethod]
        public void Equals_TrueIdentical()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);

            // Action
            var result = equalityHelper.Equals(Henry_Baker, Henry_Baker);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_True()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);

            // Action
            var result = equalityHelper.Equals(Henry_Baker, Henry_Baker2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_False()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);

            // Action
            var result = equalityHelper.Equals(Henry_Baker, Person.Bob_Jameson);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetHashCode_()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);

            // Action
            var resultBob = equalityHelper.GetHashCode(Person.Bob_Jameson);
            var resultFred = equalityHelper.GetHashCode(Person.Fred_Carlile);

            // Assert
            Assert.AreNotEqual(0, resultBob);
            Assert.AreNotEqual(0, resultFred);
            Assert.AreNotEqual(resultBob, resultFred);
        }

        [TestMethod]
        public void GetToString()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);

            // Action
            var result = equalityHelper.GetToString(Person.Bob_Jameson);

            // Assert
            string dob = Person.Bob_Jameson.DateOfBirth.ToString("u"); //utilises sortable format
            Assert.AreEqual("Person {FamilyName=Jameson; GivenName=Bob; DateOfBirth=" + dob + "}", result);
        }

        [TestMethod]
        public void TwoPropertiesOnly()
        {
            // Arrange
            var equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName);
            
            // Action
            var result = equalityHelper.Equals(Person.Bob_Jameson, Person.Bob_Jameson2);

            // Assert
            Assert.IsTrue(result);


            // Action
            var hashCode = equalityHelper.GetHashCode(Person.Bob_Jameson);

            // Assert
            Assert.AreNotEqual(0, hashCode);


            // Action
            var toString = equalityHelper.GetToString(Person.Bob_Jameson);

            // Assert
            Assert.AreNotEqual("Person {FamilyName=Jameson; GivenName=Bob }", toString);
        }
    }
}
