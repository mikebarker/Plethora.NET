using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Test.ExpressionAide
{
    [TestClass]
    public class LambdaKeyerTest
    {
        [TestMethod]
        public void SameInstanceMatch()
        {
            Expression<Func<bool>> expression = () => true;

            string key1 = LambdaKeyerEx.GetKey(expression);
            string key2 = LambdaKeyerEx.GetKey(expression);

            Assert.AreEqual(key1, key2);
        }

        [TestMethod]
        public void EqualExpressionsMatch()
        {
            Expression<Func<bool>> expression1 = () => true;
            Expression<Func<bool>> expression2 = () => true;

            string key1 = LambdaKeyerEx.GetKey(expression1);
            string key2 = LambdaKeyerEx.GetKey(expression2);

            Assert.AreEqual(key1, key2);
        }

        [TestMethod]
        public void DifferentExpressionsDontMatch()
        {
            Expression<Func<bool>> expression1 = () => true;
            Expression<Func<bool>> expression2 = () => false;

            string key1 = LambdaKeyerEx.GetKey(expression1);
            string key2 = LambdaKeyerEx.GetKey(expression2);

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void ComplexExpressions()
        {
            Expression<Func<DateTime, int, long>> expression1 = (dt, i) => (long)(dt.Year + i);
            Expression<Func<DateTime, int, long>> expression2 = (dt, i) => (long)(dt.Year + i);

            string key1 = LambdaKeyerEx.GetKey(expression1);
            string key2 = LambdaKeyerEx.GetKey(expression2);

            Assert.AreEqual(key1, key2);
        }
    }
}
