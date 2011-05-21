using System;

namespace Plethora.Test.ExtensionClasses
{
    public class Person
    {
        private static long lastId = 0;

        private readonly long id;
        private readonly string familyName;
        private readonly string givenName;
        private readonly DateTime dateOfBirth;

        public Person(string familyName, string givenName, DateTime dateOfBirth)
        {
            this.id = lastId++;
            this.familyName = familyName;
            this.givenName = givenName;
            this.dateOfBirth = dateOfBirth;
        }

        public long ID
        {
            get { return this.id; }
        }

        public string FamilyName
        {
            get { return this.familyName; }
        }

        public string GivenName
        {
            get { return this.givenName; }
        }

        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
        }

        public int Age
        {
            get { return dateOfBirth.Year - DateTime.Now.Year; }
        }

    }
}
