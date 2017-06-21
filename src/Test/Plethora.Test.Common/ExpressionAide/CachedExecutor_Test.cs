using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Plethora.ExpressionAide;

namespace Plethora.Test.ExpressionAide
{
    [TestFixture]
    public class CachedExecutor_Test
    {
        [Test]
        public void Execute()
        {
            //Setup
            Expression<Func<int>> expression = () => 9;

            //Execute
            int result1 = expression.Compile()();
            int result2 = CachedExecutor.Execute(expression);

            //Test
            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void Execute_Repeated()
        {
            //Setup
            Expression<Func<int>> expression = () => 9;

            //Execute
            int result1 = CachedExecutor.Execute(expression);
            int result2 = CachedExecutor.Execute(expression);

            //Test
            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void Execute_SameExpressions()
        {
            //Setup
            Expression<Func<int>> expression1 = () => 9;
            Expression<Func<int>> expression2 = () => 9;

            //Execute
            int result1 = CachedExecutor.Execute(expression1);
            int result2 = CachedExecutor.Execute(expression2);

            //Test
            Assert.AreEqual(result1, result2);
        }

        [Test]
        public void Execute_WithDataChange()
        {
            //Setup
            Expression<Func<int>> origExpression = GetFunc(2009);
            Expression<Func<int>> otherExpression = GetFunc(2008);

            //Execute
            int origResultCompile = origExpression.Compile()();
            int origResultExecute = CachedExecutor.Execute(origExpression);

            int otherResultCompile = otherExpression.Compile()();
            int otherResultExecute = CachedExecutor.Execute(otherExpression);

            //Test
            Assert.AreEqual(origResultCompile, origResultExecute);
            Assert.AreEqual(otherResultCompile, otherResultExecute);
            Assert.AreNotEqual(origResultExecute, otherResultExecute);
        }




        static Expression<Func<int>> GetFunc(int i)
        {
            return () => i;
        }
    }
}
