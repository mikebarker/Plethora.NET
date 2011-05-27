using System;
using System.Linq.Expressions;
using NUnit.Framework;
using Plethora.Test.ExtensionClasses;

namespace Plethora.ExpressionAide.Test
{
    [TestFixture]
    public class ExpressionDuplicatorWithClosurePromotion_Test
    {
        private ExpressionDuplicatorWithClosurePromotionEx duplicator;

        [SetUp]
        public void SetUp()
        {
            duplicator = new ExpressionDuplicatorWithClosurePromotionEx();
        }

        [Test]
        public void PromoteClosuresWithOutClosure()
        {
            //Setup
            Expression<Func<int>> expression = () => 9;

            //Execute
            var executor = duplicator.PromoteClosures(expression);

            //Test
            Assert.AreEqual(expression.Compile()(), executor.Execute(expression));
        }

        [Test]
        public void PromoteClosuresWithClosure()
        {
            //Setup
            int i = 9;
            Expression<Func<int>> expression = () => i;

            //Execute
            var executor = duplicator.PromoteClosures(expression);

            //Test
            Assert.AreEqual(expression.Compile()(), executor.Execute(expression));
        }

        [Test]
        public void PromoteClosuresWithClosure_DataChange()
        {
            //Setup
            Expression<Func<int>> origExpression = GetFunc(2009);
            Expression<Func<int>> otherExpression = GetFunc(2008);

            //Execute
            var executor = duplicator.PromoteClosures(origExpression);

            //Test
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
