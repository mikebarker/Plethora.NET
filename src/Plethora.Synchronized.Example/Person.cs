using System;

namespace Plethora.Synchronized.Example
{
    /// <summary>
    /// Mock class which is used to demonstrate the sync collection.
    /// </summary>
    public class Person : ICloneable
    {
        private static int IdTracker = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person(string firstName, string surname, DateTime dateOfBirth, string address)
            : this(++IdTracker, firstName, surname, dateOfBirth, address)
        {
        }

        private Person(int id, string firstName, string surname, DateTime dateOfBirth, string address)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.Surname = surname;
            this.DateOfBirth = dateOfBirth;
            this.Address = address;
        }

        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        #region Implementation of ICloneable

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public Person Clone()
        {
            return new Person(this.FirstName, this.Surname, this.DateOfBirth, this.Address);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}; {1} {2}; {3}; {4}",
                    this.Id,
                    this.FirstName,
                    this.Surname,
                    this.DateOfBirth.ToString("yyyy-MM-dd"),
                    this.Address);
        }
    }
}
