using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Plethora.fqi;

namespace Plethora.Test.fqi
{
    [TestFixture]
    public class MultiIndexedCollectionExtension_Test
    {
        private MultiIndexedCollection<DateTime> collection;

        [SetUp]
        public void SetUp()
        {
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month);

            collection = new MultiIndexedCollection<DateTime>(spec)
                                 {
                                     new DateTime(2009, 01, 02),
                                     new DateTime(2009, 01, 02),
                                     new DateTime(2009, 01, 02),
                                     new DateTime(2009, 01, 01),
                                     new DateTime(2009, 02, 01),
                                     new DateTime(2009, 03, 01),
                                     new DateTime(2009, 04, 01),
                                     new DateTime(2009, 05, 01),
                                     new DateTime(2009, 06, 01),
                                     new DateTime(2009, 07, 01),
                                     new DateTime(2009, 08, 01)
                                 };
        }

        [Test]
        public void SingleElement()
        {
            //Setup
            Expression<Func<DateTime, bool>> expression = date => (date.Month == 5);
            Func<DateTime, bool> func = expression.Compile();

            //Execute
            var fqiLimited = collection.Where(expression).Single();
            var linqLimited = collection.AsEnumerable().Where(func).Single();

            //Test
            Assert.AreEqual(fqiLimited, linqLimited);
        }

        [Test]
        public void MulitpleElements()
        {
            //Setup
            Expression<Func<DateTime, bool>> expression = date => (date.Month >= 6);
            Func<DateTime, bool> func = expression.Compile();

            //Execute
            var fqiLimited = collection.Where(expression).ToArray();
            var linqLimited = collection.AsEnumerable().Where(func).ToArray();

            //Test
            Assert.AreEqual(fqiLimited.Count(), linqLimited.Count());
            foreach (DateTime linqDate in linqLimited)
            {
                Assert.IsTrue(fqiLimited.Contains(linqDate));
            }
        }

        [Test]
        public void DuplicatedElements()
        {
            //Setup
            Expression<Func<DateTime, bool>> expression = date => (date.Month == 1);
            Func<DateTime, bool> func = expression.Compile();

            //Execute
            var fqiLimited = collection.Where(expression).ToArray();
            var linqLimited = collection.AsEnumerable().Where(func).ToArray();

            //Test
            Assert.AreEqual(fqiLimited.Count(), linqLimited.Count());
            foreach (DateTime linqDate in linqLimited)
            {
                Assert.IsTrue(fqiLimited.Contains(linqDate));
            }
        }
    }
}
