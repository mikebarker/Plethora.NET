using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.fqi;

namespace Plethora.Test.fqi
{
    [TestClass]
    public class MultiIndexedCollection_Test
    {
        private MultiIndexedCollection<DateTime> collection;

        [TestInitialize]
        public void SetUp()
        {
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            collection = new MultiIndexedCollection<DateTime>(spec);
        }

        [TestMethod]
        public void UniqueSpecification()
        {
            // Arrange
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(true, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            // Action
            collection = new MultiIndexedCollection<DateTime>(spec);

            // Assert
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));
            Assert.ThrowsException<ArgumentException>(() =>
            {
                collection.Add(new DateTime(2009, 01, 02));
            });
        }

        [TestMethod]
        public void DuplicateSpecification()
        {
            // Arrange
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            // Action
            collection = new MultiIndexedCollection<DateTime>(spec);

            // Assert
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));
            collection.Add(new DateTime(2009, 01, 02));
        }

        [TestMethod]
        public void Add()
        {
            // Arrange

            // Action
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            // Assert
            Assert.AreEqual(collection.Count, 2);
        }

        [TestMethod]
        public void Clear()
        {
            // Arrange
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            Assert.AreEqual(collection.Count, 2);

            // Action
            collection.Clear();
            
            // Assert
            Assert.AreEqual(collection.Count, 0);
        }

        [TestMethod]
        public void Contains()
        {
            // Arrange
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            // Action
            bool result = collection.Contains(new DateTime(2009, 01, 02));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetEnumerator()
        {
            // Arrange
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            // Action
            int count = 0;
            foreach (DateTime time in collection)
            {
                count++;
            }

            // Assert
            Assert.AreEqual(count, collection.Count);
        }

        [TestMethod]
        public void Remove()
        {
            // Arrange
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            Assert.AreEqual(collection.Count, 2);

            // Action
            bool result = collection.Remove(new DateTime(2009, 01, 02));

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(collection.Count, 1);

            // Action
            result = collection.Remove(new DateTime(2009, 01, 02));

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(collection.Count, 1);
        }



    }
}
