using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.ExpressionAide;

namespace Plethora.Test.ExpressionAide
{
    [TestClass]
    public class CachedExecutor_Test
    {
        [TestMethod]
        public void Execute()
        {
            // Arrange
            Expression<Func<int>> expression = () => 9;

            // Action
            int result1 = expression.Compile()();
            int result2 = CachedExecutor.Execute(expression);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void Execute_Repeated()
        {
            // Arrange
            Expression<Func<int>> expression = () => 9;

            // Action
            int result1 = CachedExecutor.Execute(expression);
            int result2 = CachedExecutor.Execute(expression);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void Execute_SameExpressions()
        {
            // Arrange
            Expression<Func<int>> expression1 = () => 9;
            Expression<Func<int>> expression2 = () => 9;

            // Action
            int result1 = CachedExecutor.Execute(expression1);
            int result2 = CachedExecutor.Execute(expression2);

            // Assert
            Assert.AreEqual(result1, result2);
        }

        [TestMethod]
        public void Execute_WithDataChange()
        {
            // Arrange
            Expression<Func<int>> origExpression = GetFunc(2009);
            Expression<Func<int>> otherExpression = GetFunc(2008);

            // Action
            int origResultCompile = origExpression.Compile()();
            int origResultExecute = CachedExecutor.Execute(origExpression);

            int otherResultCompile = otherExpression.Compile()();
            int otherResultExecute = CachedExecutor.Execute(otherExpression);

            // Assert
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
