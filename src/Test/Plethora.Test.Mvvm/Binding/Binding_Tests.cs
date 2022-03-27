using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Mvvm.Binding;
using Plethora.Test.Mvvm._UtilityClasses;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Plethora.Test.Mvvm.Binding
{
    [TestClass]
    public class Binding_Tests
    {
        // ============================================
        // Parse
        // ============================================
        [TestMethod]
        public void Parse_OneProperty()
        {
            // Action
            var elements = Plethora.Mvvm.Binding.Binding.Parse("DateOfBirth");
            var list = elements.ToList();

            // Assert
            Assert.AreEqual(1, list.Count);

            Assert.IsTrue(list[0] is PropertyBindingElementDefinition);
            Assert.AreEqual("DateOfBirth", ((PropertyBindingElementDefinition)list[0]).PropertyName);
        }

        [TestMethod]
        public void Parse_MultipleProperties()
        {
            // Action
            var elements = Plethora.Mvvm.Binding.Binding.Parse("DateOfBirth.Date.Year");
            var list = elements.ToList();

            // Assert
            Assert.AreEqual(3, list.Count);

            Assert.IsTrue(list[0] is PropertyBindingElementDefinition);
            Assert.AreEqual("DateOfBirth", ((PropertyBindingElementDefinition)list[0]).PropertyName);

            Assert.IsTrue(list[1] is PropertyBindingElementDefinition);
            Assert.AreEqual("Date", ((PropertyBindingElementDefinition)list[1]).PropertyName);

            Assert.IsTrue(list[2] is PropertyBindingElementDefinition);
            Assert.AreEqual("Year", ((PropertyBindingElementDefinition)list[2]).PropertyName);
        }

        [TestMethod]
        public void Parse_OneIndexer()
        {
            // Action
            var elements = Plethora.Mvvm.Binding.Binding.Parse("[0]");
            var list = elements.ToList();

            // Assert
            Assert.AreEqual(1, list.Count);

            Assert.IsTrue(list[0] is IndexerBindingElementDefinition);
            Assert.AreEqual(1, ((IndexerBindingElementDefinition)list[0]).Arguments.Length);
            Assert.AreEqual("0", ((IndexerBindingElementDefinition)list[0]).Arguments[0].Value);
            Assert.AreEqual(null, ((IndexerBindingElementDefinition)list[0]).Arguments[0].Type);
        }

        [TestMethod]
        public void Parse_MultipleIndexer()
        {
            // Action
            var elements = Plethora.Mvvm.Binding.Binding.Parse("[0][Sheep]");
            var list = elements.ToList();

            // Assert
            Assert.AreEqual(2, list.Count);

            Assert.IsTrue(list[0] is IndexerBindingElementDefinition);
            Assert.AreEqual(1, ((IndexerBindingElementDefinition)list[0]).Arguments.Length);
            Assert.AreEqual("0", ((IndexerBindingElementDefinition)list[0]).Arguments[0].Value);
            Assert.AreEqual(null, ((IndexerBindingElementDefinition)list[0]).Arguments[0].Type);

            Assert.IsTrue(list[1] is IndexerBindingElementDefinition);
            Assert.AreEqual(1, ((IndexerBindingElementDefinition)list[1]).Arguments.Length);
            Assert.AreEqual("Sheep", ((IndexerBindingElementDefinition)list[1]).Arguments[0].Value);
            Assert.AreEqual(null, ((IndexerBindingElementDefinition)list[1]).Arguments[0].Type);
        }

        [TestMethod]
        public void Parse_PropertyAndIndexer()
        {
            // Action
            var elements = Plethora.Mvvm.Binding.Binding.Parse("Children[0].Age");
            var list = elements.ToList();

            // Assert
            Assert.AreEqual(3, list.Count);

            Assert.IsTrue(list[0] is PropertyBindingElementDefinition);
            Assert.AreEqual("Children", ((PropertyBindingElementDefinition)list[0]).PropertyName);

            Assert.IsTrue(list[1] is IndexerBindingElementDefinition);
            Assert.AreEqual(1, ((IndexerBindingElementDefinition)list[1]).Arguments.Length);
            Assert.AreEqual("0", ((IndexerBindingElementDefinition)list[1]).Arguments[0].Value);
            Assert.AreEqual(null, ((IndexerBindingElementDefinition)list[1]).Arguments[0].Type);

            Assert.IsTrue(list[2] is PropertyBindingElementDefinition);
            Assert.AreEqual("Age", ((PropertyBindingElementDefinition)list[2]).PropertyName);
        }

        // ============================================
        // CreateObserver
        // ============================================
        [TestMethod]
        public void CreateObserver_PropertiesChanged()
        {
            // Arrange
            Person person = new Person();
            var elements = Plethora.Mvvm.Binding.Binding.Parse("DateOfBirth");
            var observer = Plethora.Mvvm.Binding.Binding.CreateObserver(person, elements);

            bool isValueChanged = false;
            observer.ValueChanged += (sender, e) => { isValueChanged = true; };

            // Action
            person.DateOfBirth = System.DateTime.Now;

            // Assert
            Assert.IsTrue(isValueChanged);
        }

        [TestMethod]
        public void CreateObserver_PropertiesChanged_OtherProperty()
        {
            // Arrange
            Person person = new Person();
            var elements = Plethora.Mvvm.Binding.Binding.Parse("DateOfBirth");
            var observer = Plethora.Mvvm.Binding.Binding.CreateObserver(person, elements);

            bool isValueChanged = false;
            observer.ValueChanged += (sender, e) => { isValueChanged = true; };

            // Action
            person.FamilyName = "Wooly";

            // Assert
            Assert.IsFalse(isValueChanged);
        }

        [TestMethod]
        public void CreateObserver_PropertiesChanged_Multiple()
        {
            // Arrange
            Person person = new Person();
            var elements = Plethora.Mvvm.Binding.Binding.Parse("DateOfBirth.TimeOfDay.TotalSeconds");
            var observer = Plethora.Mvvm.Binding.Binding.CreateObserver(person, elements);

            bool isValueChanged = false;
            observer.ValueChanged += (sender, e) => { isValueChanged = true; };

            // Action
            person.DateOfBirth = System.DateTime.Now;

            // Assert
            Assert.IsTrue(isValueChanged);
        }

        [TestMethod]
        public void CreateObserver_CollectionChanged()
        {
            // Arrange
            Person professor = new Person();

            Lesson lesson1 = new Lesson("East wing", TimeSpan.FromHours(1.5));
            Lesson lesson2 = new Lesson("East wing", TimeSpan.FromHours(1));
            Lesson lesson3 = new Lesson("West wing", TimeSpan.FromHours(2));

            Module module = new Module(professor, new[] { lesson1, lesson2, lesson3 });

            var elements = Plethora.Mvvm.Binding.Binding.Parse("Lessons[0].Location");
            var observer = Plethora.Mvvm.Binding.Binding.CreateObserver(module, elements);

            bool isValueChanged = false;
            observer.ValueChanged += (sender, e) => { isValueChanged = true; };

            bool result;
            object location;

            // Assert
            Assert.IsFalse(isValueChanged);

            result = observer.TryGetValue(out location);
            Assert.IsTrue(result);
            Assert.AreEqual("East wing", location);

            // Action
            module.Lessons[0] = new Lesson("Quad", TimeSpan.FromMinutes(45));

            // Assert
            Assert.IsTrue(isValueChanged);

            result = observer.TryGetValue(out location);
            Assert.IsTrue(result);
            Assert.AreEqual("Quad", location);
        }

        [TestMethod]
        public void CreateObserver_CollectionChanged_OtherIndex()
        {
            // Arrange
            Person professor = new Person();

            Lesson lesson1 = new Lesson("East wing", TimeSpan.FromHours(1.5));
            Lesson lesson2 = new Lesson("East wing", TimeSpan.FromHours(1));
            Lesson lesson3 = new Lesson("West wing", TimeSpan.FromHours(2));

            Module module = new Module(professor, new[] { lesson1, lesson2, lesson3 });

            var elements = Plethora.Mvvm.Binding.Binding.Parse("Lessons[0].Location");
            var observer = Plethora.Mvvm.Binding.Binding.CreateObserver(module, elements);

            bool isValueChanged = false;
            observer.ValueChanged += (sender, e) => { isValueChanged = true; };

            bool result;
            object location;

            // Assert
            Assert.IsFalse(isValueChanged);

            result = observer.TryGetValue(out location);
            Assert.IsTrue(result);
            Assert.AreEqual("East wing", location);

            // Action
            module.Lessons[1] = new Lesson("Quad", TimeSpan.FromMinutes(45));

            // Assert
            Assert.IsFalse(isValueChanged);
        }



        class Lesson
        {
            public Lesson(string location, TimeSpan duration)
            {
                Location = location;
                Duration = duration;
            }

            public string Location { get; }
            public TimeSpan Duration { get; }
        }

        class Module
        {
            public Module(Person professor, Lesson[] lessons)
            {
                Professor = professor;
                Lessons = new ObservableCollection<Lesson>(lessons);
            }

            public Person Professor { get; }

            public ObservableCollection<Lesson> Lessons { get; }
        }

    }
}
