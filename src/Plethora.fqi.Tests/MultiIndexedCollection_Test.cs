using System;
using NUnit.Framework;
using Plethora.fqi;

namespace Plethora.Test.fqi
{
    [TestFixture]
    public class MultiIndexedCollection_Test
    {
        private MultiIndexedCollection<DateTime> collection;

        [SetUp]
        public void SetUp()
        {
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            collection = new MultiIndexedCollection<DateTime>(spec);
        }

        [Test]
        public void UniqueSpecification()
        {
            //Setup
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(true, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            //Execute
            collection = new MultiIndexedCollection<DateTime>(spec);

            //Test
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));
            Assert.Throws(typeof (ArgumentException), delegate
                                                          {
                                                              collection.Add(new DateTime(2009, 01, 02));
                                                          });
        }

        [Test]
        public void DuplicateSpecification()
        {
            //Setup
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month).Then(r => r.Day);

            //Execute
            collection = new MultiIndexedCollection<DateTime>(spec);

            //Test
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));
            collection.Add(new DateTime(2009, 01, 02));
        }

        [Test]
        public void Add()
        {
            //Setup

            //Execute
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            //Test
            Assert.AreEqual(collection.Count, 2);
        }

        [Test]
        public void Clear()
        {
            //Setup
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            Assert.AreEqual(collection.Count, 2);

            //Execute
            collection.Clear();
            
            //Test
            Assert.AreEqual(collection.Count, 0);
        }

        [Test]
        public void Contains()
        {
            //Setup
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            //Execute
            bool result = collection.Contains(new DateTime(2009, 01, 02));

            //Test
            Assert.IsTrue(result);
        }

        [Test]
        public void GetEnumerator()
        {
            //Setup
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            //Execute
            int count = 0;
            foreach (DateTime time in collection)
            {
                count++;
            }

            //Test
            Assert.AreEqual(count, collection.Count);
        }

        [Test]
        public void Remove()
        {
            //Setup
            collection.Add(new DateTime(2009, 01, 01));
            collection.Add(new DateTime(2009, 01, 02));

            Assert.AreEqual(collection.Count, 2);

            //Execute
            bool result = collection.Remove(new DateTime(2009, 01, 02));

            //Test
            Assert.IsTrue(result);
            Assert.AreEqual(collection.Count, 1);

            //Execute
            result = collection.Remove(new DateTime(2009, 01, 02));

            //Test
            Assert.IsFalse(result);
            Assert.AreEqual(collection.Count, 1);
        }



    }
}
