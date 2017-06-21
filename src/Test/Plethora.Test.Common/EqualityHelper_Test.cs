using System;
using NUnit.Framework;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test
{
    [TestFixture]
    public class EqualityHelper_Test
    {
        private EqualityHelper<Person> equalityHelper;

        public static readonly Person Henry_Baker = new Person(46, "Baker", "Henry", new DateTime(1964, 03, 14));
        public static readonly Person Henry_Baker2 = new Person(72, "Baker", "Henry", new DateTime(1964, 03, 14));


        [SetUp]
        public void SetUp()
        {
            //Equality is treated on only first name, surname, and date of birth... and not ID
            this.equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName,
                person => person.DateOfBirth);
        }

        [Test]
        public void Equals_TrueIdentical()
        {
            //exec
            var result = equalityHelper.Equals(Henry_Baker, Henry_Baker);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_True()
        {
            //exec
            var result = equalityHelper.Equals(Henry_Baker, Henry_Baker2);

            //test
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_False()
        {
            //exec
            var result = equalityHelper.Equals(Henry_Baker, Person.Bob_Jameson);

            //test
            Assert.IsFalse(result);
        }

        [Test]
        public void GetHashCode_()
        {
            //exec
            var resultBob = equalityHelper.GetHashCode(Person.Bob_Jameson);
            var resultFred = equalityHelper.GetHashCode(Person.Fred_Carlile);

            //test
            Assert.AreNotEqual(0, resultBob);
            Assert.AreNotEqual(0, resultFred);
            Assert.AreNotEqual(resultBob, resultFred);
        }

        [Test]
        public void GetToString()
        {
            //exec
            var result = equalityHelper.GetToString(Person.Bob_Jameson);

            //test
            string dob = Person.Bob_Jameson.DateOfBirth.ToString("u"); //utilises sortable format
            Assert.AreEqual("Person {FamilyName=Jameson; GivenName=Bob; DateOfBirth=" + dob + "}", result);
        }

        [Test]
        public void TwoPropertiesOnly()
        {
            //setup
            this.equalityHelper = EqualityHelper<Person>.Create(
                person => person.FamilyName,
                person => person.GivenName);
            
            //exec
            var result = equalityHelper.Equals(Person.Bob_Jameson, Person.Bob_Jameson2);

            //test
            Assert.IsTrue(result);


            //exec
            var hashCode = equalityHelper.GetHashCode(Person.Bob_Jameson);

            //test
            Assert.AreNotEqual(0, hashCode);


            //exec
            var toString = equalityHelper.GetToString(Person.Bob_Jameson);

            //test
            Assert.AreNotEqual("Person {FamilyName=Jameson; GivenName=Bob }", toString);
        }
    }
}
