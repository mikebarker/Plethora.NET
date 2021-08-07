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
            this.SetOriginalValue(id, nameof(Id));
            this.SetOriginalValue(givenName, nameof(GivenName));
            this.SetOriginalValue(familyName, nameof(FamilyName));
            this.SetOriginalValue(dateOfBirth, nameof(DateOfBirth));
        }

        public Guid Id
        {
            get { return this.GetValue<Guid>(); }
            set { this.SetValue(value); }
        }

        [DefaultValue("")]
        public string GivenName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        [DefaultValue("")]
        public string FamilyName
        {
            get { return this.GetValue<string>(); }
            set { this.SetValue(value); }
        }

        public DateTime DateOfBirth
        {
            get { return this.GetValue<DateTime>(); }
            set { this.SetValue(value); }
        }

        [DefaultValueProvider(typeof(Country), "UnitedKingdom")]
        public Country CountryOfResidence
        {
            get { return this.GetValue<Country>(); }
            set { this.SetValue(value); }            
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
