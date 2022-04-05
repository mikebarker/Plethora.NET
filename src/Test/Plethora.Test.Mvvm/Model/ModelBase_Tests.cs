using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Test.Mvvm._UtilityClasses;

namespace Plethora.Test.Mvvm.Model
{
    [TestClass]
    public class ModelBase_Tests
    {
        [TestMethod]
        public void NoChanges()
        {
            // setup
            Person person = new Person();

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

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

        [TestMethod]
        public void NoChangesSetOriginalValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(0, propertiesChanged.Count);
            Assert.AreEqual("John", person.GivenName);
            Assert.AreEqual("Smith", person.FamilyName);
            Assert.AreEqual(new DateTime(2000, 01, 01), person.DateOfBirth);
            Assert.AreEqual(Country.UnitedKingdom, person.CountryOfResidence);  // [DefaultValueProvider(typeof(Country), "UnitedKingdom")]
        }

        [TestMethod]
        public void SetChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.FamilyName = "Brown";

            //test
            Assert.AreEqual(true, person.HasChanged);
            Assert.AreEqual(2, propertiesChanged.Count);
            Assert.IsTrue(propertiesChanged.Contains("FamilyName"));
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));
            Assert.AreEqual("Brown", person.FamilyName);
        }

        [TestMethod]
        public void SetUnchangedValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.FamilyName = "Smith";

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(0, propertiesChanged.Count);
            Assert.AreEqual("Smith", person.FamilyName);
        }

        [TestMethod]
        public void UndoChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.FamilyName = "Brown";
            person.FamilyName = "Smith"; //Restore to original value

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(4, propertiesChanged.Count);
            Assert.IsTrue(propertiesChanged.Contains("FamilyName"));
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));
            Assert.IsTrue(propertiesChanged.Contains("FamilyName"));
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));
            Assert.AreEqual("Smith", person.FamilyName);
        }

        [TestMethod]
        public void DependsOnChangeValue()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.DateOfBirth = new DateTime(1990, 07, 14);

            //test
            Assert.AreEqual(true, person.HasChanged);
            Assert.AreEqual(4, propertiesChanged.Count);
            Assert.IsTrue(propertiesChanged.Contains("DateOfBirth"));
            Assert.IsTrue(propertiesChanged.Contains("Age"));
            Assert.IsTrue(propertiesChanged.Contains("YearsToCentenary"));
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));
            Assert.AreEqual(new DateTime(1990, 07, 14), person.DateOfBirth);
        }

        [TestMethod]
        public void Rollback()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            person.GivenName = "Fred";
            person.FamilyName = "Brown";
            person.DateOfBirth = new DateTime(1900, 07, 14);

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.Rollback();

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(6, propertiesChanged.Count);
            Assert.IsTrue(propertiesChanged.Contains("GivenName"));
            Assert.IsTrue(propertiesChanged.Contains("FamilyName"));
            Assert.IsTrue(propertiesChanged.Contains("DateOfBirth"));
            Assert.IsTrue(propertiesChanged.Contains("Age"));
            Assert.IsTrue(propertiesChanged.Contains("YearsToCentenary"));
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));

            Assert.AreEqual("John", person.GivenName);
            Assert.AreEqual("Smith", person.FamilyName);
            Assert.AreEqual(new DateTime(2000, 01, 01), person.DateOfBirth);
        }

        [TestMethod]
        public void Commit()
        {
            // setup
            Person person = new Person(
                Guid.NewGuid(),
                "John",
                "Smith",
                new DateTime(2000, 01, 01));

            person.GivenName = "Fred";
            person.FamilyName = "Brown";
            person.DateOfBirth = new DateTime(1900, 07, 14);

            List<string> propertiesChanged = new List<string>();
            person.PropertyChanged += (sender, e) => { propertiesChanged.Add(e.PropertyName); };

            //exec
            person.Commit();

            //test
            Assert.AreEqual(false, person.HasChanged);
            Assert.AreEqual(1, propertiesChanged.Count);
            Assert.IsTrue(propertiesChanged.Contains("HasChanged"));

            Assert.AreEqual("Fred", person.GivenName);
            Assert.AreEqual("Brown", person.FamilyName);
            Assert.AreEqual(new DateTime(1900, 07, 14), person.DateOfBirth);
        }


    }
}
