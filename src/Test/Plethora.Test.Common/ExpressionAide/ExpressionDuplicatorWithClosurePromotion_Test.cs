using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Test.ExpressionAide
{
    [TestClass]
    public class ExpressionDuplicatorWithClosurePromotion_Test
    {
        private ExpressionDuplicatorWithClosurePromotionEx duplicator = new ExpressionDuplicatorWithClosurePromotionEx();

        [TestMethod]
        public void PromoteClosuresWithOutClosure()
        {
            // Arrange
            Expression<Func<int>> expression = () => 9;

            // Action
            var executor = duplicator.PromoteClosures(expression);

            // Assert
            Assert.AreEqual(expression.Compile()(), executor.Execute(expression));
        }

        [TestMethod]
        public void PromoteClosuresWithClosure()
        {
            // Arrange
            int i = 9;
            Expression<Func<int>> expression = () => i;

            // Action
            var executor = duplicator.PromoteClosures(expression);

            // Assert
            Assert.AreEqual(expression.Compile()(), executor.Execute(expression));
        }

        [TestMethod]
        public void PromoteClosuresWithClosure_DataChange()
        {
            // Arrange
            Expression<Func<int>> origExpression = GetFunc(2009);
            Expression<Func<int>> otherExpression = GetFunc(2008);

            // Action
            var executor = duplicator.PromoteClosures(origExpression);

            // Assert
            var origFunc = origExpression.Compile();
            int origResult = origFunc();

            var otherFunc = otherExpression.Compile();
            int otherResult = otherFunc();

            int otherExecuteResult = executor.Execute(otherExpression);

            Assert.AreNotEqual(origResult, otherExecuteResult);
            Assert.AreEqual(otherResult, otherExecuteResult);
        }


        static Expression<Func<int>> GetFunc(int i)
        {
            return () => i;
        }
    }
}
