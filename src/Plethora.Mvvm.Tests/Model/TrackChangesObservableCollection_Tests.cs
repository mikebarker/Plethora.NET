using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

using NUnit.Framework;

using Plethora.Mvvm.Model;
using Plethora.Test.Mvvm._UtilityClasses;

// ReSharper disable ArrangeThisQualifier

namespace Plethora.Test.Mvvm.Model
{
    [TestFixture]
    public class TrackChangesObservableCollection_Tests
    {
        private Person JohnSmith;
        private Person JaneDoe;
        private Person FredBrown;

        [SetUp]
        public void SetUp()
        {
            JohnSmith = new Person(Guid.NewGuid(), "John", "Smith", new DateTime(2000, 01, 01));
            JaneDoe = new Person(Guid.NewGuid(), "Jane", "Doe", new DateTime(1994, 12, 23));
            FredBrown = new Person(Guid.NewGuid(), "Fred", "Brown", new DateTime(1990, 07, 14));
        }

        [Test]
        public void NoItems()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(); 

            //exec

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void Add()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Add(JohnSmith);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);

            Assert.AreEqual(1, people.Added.Count);
            Assert.AreSame(JohnSmith, people.Added.First());
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void AddAndUpdate()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };
            
            //exec
            people.Add(JohnSmith);
            JohnSmith.DateOfBirth = new DateTime(2010, 07, 24);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);

            Assert.AreEqual(1, people.Added.Count);
            Assert.AreSame(JohnSmith, people.Added.First());
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void AddAndRemove()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };
            
            //exec
            people.Add(JohnSmith);
            people.Remove(JohnSmith);

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[1].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void AddAndClear()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Add(JohnSmith);
            people.Clear();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[1].Action);

            Assert.AreEqual(0, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void Remove()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);

            Assert.AreEqual(2, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(1, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.First());
        }

        [Test]
        public void RemoveAndAdd()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);
            people.Add(JohnSmith);

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[1].Action);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void RemoveAndUpdate()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);
            JohnSmith.DateOfBirth = new DateTime(2010, 07, 24);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);

            Assert.AreEqual(2, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(1, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.First());
        }

        [Test]
        public void RemoveUpdateAndAdd()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);
            JohnSmith.DateOfBirth = new DateTime(2010, 07, 24);
            people.Add(this.JohnSmith);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(5, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[1].Action);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(1, people.Updated.Count);
            Assert.AreSame(JohnSmith, people.Updated.First());
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void SetItem()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            Person davidJones = new Person(Guid.NewGuid(), "David", "Jones", new DateTime(2010, 07, 24));
            people[0] = davidJones;

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(2, propertyChangedEvents.Count);
            Assert.AreEqual("Item[]", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Replace, collectionChangedEvents[0].Action);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(1, people.Added.Count);
            Assert.AreSame(davidJones, people.Added.First());
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(1, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.First());
        }

        [Test]
        public void Update()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(1, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual(0, collectionChangedEvents.Count);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(1, people.Updated.Count);
            Assert.AreSame(JohnSmith, people.Updated.First());
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void UpdateAndRevert_OrginalValue()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            JohnSmith.DateOfBirth = new DateTime(2000, 01, 01); // restore original value

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(2, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual(0, collectionChangedEvents.Count);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void UpdateAndRevert_Rollback()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            JohnSmith.Rollback();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(2, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual(0, collectionChangedEvents.Count);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void UpdateAndRemove()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            people.Remove(JohnSmith);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);

            Assert.AreEqual(2, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(1, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.First());
        }

        [Test]
        public void UpdateRemoveAndAdd()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            people.Remove(JohnSmith);
            people.Add(this.JohnSmith);

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(5, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[1].Action);

            Assert.AreEqual(3, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(1, people.Updated.Count);
            Assert.AreSame(JohnSmith, people.Updated.First());
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void UpdateAndClear()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07, 24);
            people.Clear();

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(4, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(3, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.ElementAt(0));
            Assert.AreSame(JaneDoe, people.Removed.ElementAt(1));
            Assert.AreSame(FredBrown, people.Removed.ElementAt(2));
        }

        [Test]
        public void Clear()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Clear();

            //test
            Assert.AreEqual(true, people.HasChanged);
            Assert.AreEqual(3, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Count);
            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(3, people.Removed.Count);
            Assert.AreSame(JohnSmith, people.Removed.ElementAt(0));
            Assert.AreSame(JaneDoe, people.Removed.ElementAt(1));
            Assert.AreSame(FredBrown, people.Removed.ElementAt(2));
        }


        [Test]
        public void RollBack_Add()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Add(JohnSmith);
            people.Rollback();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[1].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void RollBack_Clear()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Clear();
            people.Rollback();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[1].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void RollBack_Remove()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);
            people.Rollback();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(6, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[4].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[5].PropertyName);
            Assert.AreEqual(2, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[1].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void RollBack_Update()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            people.Rollback();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(4, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Count", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);

            Assert.AreEqual(false, JohnSmith.HasChanged);
        }


        [Test]
        public void Commit_Add()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>();

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Add(JohnSmith);
            people.Commit();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(4, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void Commit_Clear()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Clear();
            people.Commit();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(4, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Reset, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void Commit_Remove()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            people.Remove(JohnSmith);
            people.Commit();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(4, propertyChangedEvents.Count);
            Assert.AreEqual("Count", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("Item[]", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[2].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[3].PropertyName);
            Assert.AreEqual(1, collectionChangedEvents.Count);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionChangedEvents[0].Action);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }

        [Test]
        public void Commit_Update()
        {
            //setup
            TrackChangesObservableCollection<Person> people = new TrackChangesObservableCollection<Person>(new[] { JohnSmith, JaneDoe, FredBrown });

            List<PropertyChangedEventArgs> propertyChangedEvents = new List<PropertyChangedEventArgs>();
            ((INotifyPropertyChanged)people).PropertyChanged += (sender, e) => propertyChangedEvents.Add(e);

            List<NotifyCollectionChangedEventArgs> collectionChangedEvents = new List<NotifyCollectionChangedEventArgs>();
            people.CollectionChanged += (sender, e) => { collectionChangedEvents.Add(e); };

            //exec
            JohnSmith.DateOfBirth = new DateTime(2010, 07,24);
            people.Commit();

            //test
            Assert.AreEqual(false, people.HasChanged);
            Assert.AreEqual(2, propertyChangedEvents.Count);
            Assert.AreEqual("HasChanged", propertyChangedEvents[0].PropertyName);
            Assert.AreEqual("HasChanged", propertyChangedEvents[1].PropertyName);
            Assert.AreEqual(0, collectionChangedEvents.Count);

            Assert.AreEqual(0, people.Added.Count);
            Assert.AreEqual(0, people.Updated.Count);
            Assert.AreEqual(0, people.Removed.Count);
        }
    }
}
