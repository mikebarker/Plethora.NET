using System;
using System.ComponentModel;

using Plethora.Mvvm.Model;

namespace Plethora.Test.Mvvm._UtilityClasses
{
    class Person : ModelBase
    {
        public Person()
        {
        }

        public Person(
            Guid id,
            string givenName,
            string familyName,
            DateTime dateOfBirth)
        {
            this.SetOriginalValue(() => this.Id, id);
            this.SetOriginalValue(() => this.GivenName, givenName);
            this.SetOriginalValue(() => this.FamilyName, familyName);
            this.SetOriginalValue(() => this.DateOfBirth, dateOfBirth);
        }

        public Guid Id
        {
            get { return this.GetValue(() => this.Id); }
            set { this.SetValue(() => this.Id, value); }
        }

        [DefaultValue("")]
        public string GivenName
        {
            get { return this.GetValue(() => this.GivenName); }
            set { this.SetValue(() => this.GivenName, value); }
        }

        [DefaultValue("")]
        public string FamilyName
        {
            get { return this.GetValue(() => this.FamilyName); }
            set { this.SetValue(() => this.FamilyName, value); }
        }

        public DateTime DateOfBirth
        {
            get { return this.GetValue(() => this.DateOfBirth); }
            set { this.SetValue(() => this.DateOfBirth, value); }
        }

        [DefaultValueProvider(typeof(Country), "UnitedKingdom")]
        public Country CountryOfResidence
        {
            get { return this.GetValue(() => this.CountryOfResidence); }
            set { this.SetValue(() => this.CountryOfResidence, value); }            
        }

        [DependsOn("DateOfBirth")]
        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - this.DateOfBirth.Year;
                return age;
            }
        }

        [DependsOn("Age")]
        public int YearsToCentenary
        {
            get
            {
                return 100 - this.Age;
            }
        }
    }
}
