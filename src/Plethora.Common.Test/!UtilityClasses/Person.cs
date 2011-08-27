using System;
using System.Collections.Generic;

namespace Plethora.Test.UtilityClasses
{
    public class Person
    {
        public class NameComparer : IComparer<Person>
        {
            #region Implementation of IComparer<Person>

            public int Compare(Person x, Person y)
            {
                int result;
                result = string.Compare(x.FamilyName, y.FamilyName);
                if (result != 0)
                    return result;

                result = string.Compare(x.GivenName, y.GivenName);
                return result;
            }

            #endregion
        }

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

        public Person(long id, string familyName, string givenName, DateTime dateOfBirth)
        {
            this.id = id;
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

        public override string ToString()
        {
            return "Person [" + ID + ": " + FamilyName + ", " + GivenName + "]";
        }
    }
}
