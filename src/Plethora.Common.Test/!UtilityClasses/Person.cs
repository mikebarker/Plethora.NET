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

        // Two "Bob Jameson"
        // "Fred Carlile" and "Amy Cathson" share DateOfBirths
        // "Jill Dorrman" and "Jane Doe" share IDs
        public static readonly Person Bob_Jameson = new Person(0, "Jameson", "Bob", new DateTime(1964, 03, 14));
        public static readonly Person Bob_Jameson2 = new Person(10, "Jameson", "Bob", new DateTime(1998, 07, 13));
        public static readonly Person Fred_Carlile = new Person(1, "Carlile", "Fred", new DateTime(1975, 11, 07));
        public static readonly Person Amy_Cathson = new Person(2, "Cathson", "Amy", new DateTime(1975, 11, 07));
        public static readonly Person Jill_Dorrman = new Person(3, "Dorrman", "Jill", new DateTime(1978, 05, 08));
        public static readonly Person Jane_Doe = new Person(3, "Doe", "Jane", new DateTime(1976, 03, 15));
        public static readonly Person Katherine_Harold = new Person(4, "Harold", "Katherine", new DateTime(1984, 02, 21));
        public static readonly Person Harry_Porker = new Person(5, "Porker", "Harry", new DateTime(1978, 05, 08));


        private static long lastId = 0;

        private readonly long id;
        private readonly string familyName;
        private readonly string givenName;
        private readonly DateTime dateOfBirth;

        public Person(string familyName, string givenName, DateTime dateOfBirth)
            : this(lastId++, familyName, givenName, dateOfBirth)
        {
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
