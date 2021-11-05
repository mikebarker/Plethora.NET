using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.fqi;

namespace Plethora.Test.fqi
{
    [TestClass]
    public class MultiIndexedCollectionExtension_Test
    {
        private MultiIndexedCollection<DateTime> collection;

        [TestInitialize]
        public void SetUp()
        {
            var spec = new MultiIndexSpecification<DateTime>();
            spec
                .AddIndex(false, r => r.Year).Then(r => r.Month);

            this.collection = new MultiIndexedCollection<DateTime>(spec)
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

        [TestMethod]
        public void SingleElement()
        {
            // Arrange
            Expression<Func<DateTime, bool>> expression = date => (date.Month == 5);
            Func<DateTime, bool> func = expression.Compile();

            // Action
            var fqiLimited = this.collection.Where(expression).Single();
            var linqLimited = this.collection.AsEnumerable().Where(func).Single();

            // Assert
            Assert.AreEqual(fqiLimited, linqLimited);
        }

        [TestMethod]
        public void MulitpleElements()
        {
            // Arrange
            Expression<Func<DateTime, bool>> expression = date => (date.Month >= 6);
            Func<DateTime, bool> func = expression.Compile();

            // Action
            var fqiLimited = this.collection.Where(expression).ToArray();
            var linqLimited = this.collection.AsEnumerable().Where(func).ToArray();

            // Assert
            Assert.AreEqual(fqiLimited.Count(), linqLimited.Count());
            foreach (DateTime linqDate in linqLimited)
            {
                Assert.IsTrue(fqiLimited.Contains(linqDate));
            }
        }

        [TestMethod]
        public void DuplicatedElements()
        {
            // Arrange
            Expression<Func<DateTime, bool>> expression = date => (date.Month == 1);
            Func<DateTime, bool> func = expression.Compile();

            // Action
            var fqiLimited = this.collection.Where(expression).ToArray();
            var linqLimited = this.collection.AsEnumerable().Where(func).ToArray();

            // Assert
            Assert.AreEqual(fqiLimited.Count(), linqLimited.Count());
            foreach (DateTime linqDate in linqLimited)
            {
                Assert.IsTrue(fqiLimited.Contains(linqDate));
            }
        }
    }
}
