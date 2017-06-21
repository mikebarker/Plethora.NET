using System;
using System.Collections.Generic;
using System.ComponentModel;

using NUnit.Framework;

using Plethora.Test.Mvvm._UtilityClasses;

namespace Plethora.Test.Mvvm.Model
{
    [TestFixture]
    public class ModelBase_Tests
    {
        [Test]
        public void NoChanges()
        {
            // setup
            Person person = new Person();

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(0, propertiesChanged.Count);
            Assert.AreEqual(default(Guid), person.Id);
            Assert.AreEqual("", person.GivenName);  // [DefaultValue("")] used, not default(string)
            Assert.AreEqual("", person.FamilyName);  // [DefaultValue("")] used, not default(string)
            Assert.AreEqual(default(DateTime), person.DateOfBirth);
            Assert.AreEqual(Country.UnitedKingdom, person.CountryOfResidence);
        }

        [Test]
        public void NoChangesSetOriginalValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(0, propertiesChanged.Count);
            Assert.AreEqual("John", person.GivenName);
            Assert.AreEqual("Smith", person.FamilyName);
            Assert.AreEqual(new DateTime(2000, 01, 01), person.DateOfBirth);
            Assert.AreEqual(Country.UnitedKingdom, person.CountryOfResidence);  // [DefaultValueProvider(typeof(Country), "UnitedKingdom")]
        }

        [Test]
        public void SetChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec
            person.FamilyName = "Brown";

            //test
            Assert.AreEqual(true, person.HasChanged);
            Assert.AreEqual(2, propertiesChanged.Count);
            Assert.AreEqual("FamilyName", propertiesChanged[0].PropertyName);
            Assert.AreEqual("HasChanged", propertiesChanged[1].PropertyName);
            Assert.AreEqual("Brown", person.FamilyName);
        }

        [Test]
        public void SetUnchangedValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec
            person.FamilyName = "Smith";

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(0, propertiesChanged.Count);
            Assert.AreEqual("Smith", person.FamilyName);
        }

        [Test]
        public void UndoChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec
            person.FamilyName = "Brown";
            person.FamilyName = "Smith"; //Restore to original value

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(4, propertiesChanged.Count);
            Assert.AreEqual("FamilyName", propertiesChanged[0].PropertyName);
            Assert.AreEqual("HasChanged", propertiesChanged[1].PropertyName);
            Assert.AreEqual("FamilyName", propertiesChanged[2].PropertyName);
            Assert.AreEqual("HasChanged", propertiesChanged[3].PropertyName);
            Assert.AreEqual("Smith", person.FamilyName);
        }

        [Test]
        public void DependsOnChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            //exec
            person.DateOfBirth = new DateTime(1990, 07, 14);

            //test
            Assert.AreEqual(true, person.HasChanged);
            Assert.AreEqual(4, propertiesChanged.Count);
            Assert.AreEqual("DateOfBirth", propertiesChanged[0].PropertyName);
            Assert.AreEqual("Age", propertiesChanged[1].PropertyName);
            Assert.AreEqual("YearsToCentenary", propertiesChanged[2].PropertyName);
            Assert.AreEqual("HasChanged", propertiesChanged[3].PropertyName);
            Assert.AreEqual(new DateTime(1990, 07, 14), person.DateOfBirth);
        }

        [Test]
        public void Rollback()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            person.GivenName = "Fred";
            person.FamilyName = "Brown";
            person.DateOfBirth = new DateTime(1900, 07, 14);

            propertiesChanged.Clear();

            //exec
            person.Rollback();

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(6, propertiesChanged.Count);
            Assert.AreEqual("GivenName", propertiesChanged[0].PropertyName);
            Assert.AreEqual("FamilyName", propertiesChanged[1].PropertyName);
            Assert.AreEqual("DateOfBirth", propertiesChanged[2].PropertyName);
            Assert.AreEqual("Age", propertiesChanged[3].PropertyName);
            Assert.AreEqual("YearsToCentenary", propertiesChanged[4].PropertyName);
            Assert.AreEqual("HasChanged", propertiesChanged[5].PropertyName);

            Assert.AreEqual("John", person.GivenName);
            Assert.AreEqual("Smith", person.FamilyName);
            Assert.AreEqual(new DateTime(2000, 01, 01), person.DateOfBirth);
        }

        [Test]
        public void Commit()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<PropertyChangedEventArgs> propertiesChanged = new List<PropertyChangedEventArgs>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e); };

            person.GivenName = "Fred";
            person.FamilyName = "Brown";
            person.DateOfBirth = new DateTime(1900, 07, 14);

            propertiesChanged.Clear();

            //exec
            person.Commit();

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(1, propertiesChanged.Count);
            Assert.AreEqual("HasChanged", propertiesChanged[0].PropertyName);

            Assert.AreEqual("Fred", person.GivenName);
            Assert.AreEqual("Brown", person.FamilyName);
            Assert.AreEqual(new DateTime(1900, 07, 14), person.DateOfBirth);
        }


    }
}
