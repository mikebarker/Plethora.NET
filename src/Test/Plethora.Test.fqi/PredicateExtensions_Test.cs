using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.fqi;

namespace Plethora.Test.fqi
{
    [TestClass]
    public class PredicateExtensions_Test
    {
        private Func<int, bool> predicate0 = null;
        private Func<int, bool> predicate1 = null;

        [TestInitialize]
        public void SetUp()
        {
            predicate0 = i => (i >= 5);
            predicate1 = i => (i < 10);
        }

        [TestMethod]
        public void And()
        {
            // Arrange

            // Action
            var predicateTotal = PredicateExtensions.And(predicate0, predicate1);

            // Assert
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 && result1, resultTotal);
        }

        [TestMethod]
        public void Or()
        {
            // Arrange

            // Action
            var predicateTotal = PredicateExtensions.Or(predicate0, predicate1);

            // Assert
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 || result1, resultTotal);
        }

        [TestMethod]
        public void XOr()
        {
            // Arrange

            // Action
            var predicateTotal = PredicateExtensions.XOr(predicate0, predicate1);

            // Assert
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 ^ result1, resultTotal);
        }
        [TestMethod]
        public void Not()
        {
            // Arrange

            // Action
            var predicateTotal = PredicateExtensions.Not(predicate0);

            // Assert
            const int arg = 8;
            var result0 = predicate0(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(!result0, resultTotal);
        }
    }
}
