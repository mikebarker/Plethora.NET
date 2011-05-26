using System;
using NUnit.Framework;

namespace Plethora.fqi.Test
{
    [TestFixture]
    public class PredicateExtensions_Test
    {
        private Func<int, bool> predicate0 = null;
        private Func<int, bool> predicate1 = null;

        [SetUp]
        public void SetUp()
        {
            predicate0 = i => (i >= 5);
            predicate1 = i => (i < 10);
        }

        [Test]
        public void And()
        {
            //Setup

            //Execute
            var predicateTotal = PredicateExtensions.And(predicate0, predicate1);

            //Test
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 && result1, resultTotal);
        }

        [Test]
        public void Or()
        {
            //Setup

            //Execute
            var predicateTotal = PredicateExtensions.Or(predicate0, predicate1);

            //Test
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 || result1, resultTotal);
        }

        [Test]
        public void XOr()
        {
            //Setup

            //Execute
            var predicateTotal = PredicateExtensions.XOr(predicate0, predicate1);

            //Test
            const int arg = 8;
            var result0 = predicate0(arg);
            var result1 = predicate1(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(result0 ^ result1, resultTotal);
        }
        [Test]
        public void Not()
        {
            //Setup

            //Execute
            var predicateTotal = PredicateExtensions.Not(predicate0);

            //Test
            const int arg = 8;
            var result0 = predicate0(arg);
            var resultTotal = predicateTotal(arg);

            Assert.AreEqual(!result0, resultTotal);
        }
    }
}
