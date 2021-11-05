using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Test.ExpressionAide
{
    [TestClass]
    public class ExpressionDuplicator_Test
    {
        private readonly ExpressionDuplicatorEx duplicator = new ExpressionDuplicatorEx();

        [TestMethod]
        public void DuplicateAdd()
        {
            // Arrange
            Expression<Func<int, int, int>> expression = (i, j) => i + j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1, 2), duplicate.Compile().Invoke(1, 2));
        }

        [TestMethod]
        public void DuplicateAnd()
        {
            // Arrange
            Expression<Func<bool, bool, bool>> expression = (i, j) => i & j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true, false), duplicate.Compile().Invoke(true, false));
        }

        [TestMethod]
        public void DuplicateAndAlso()
        {
            // Arrange
            Expression<Func<bool, bool, bool>> expression = (i, j) => i && j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true, false), duplicate.Compile().Invoke(true, false));
        }

        [TestMethod]
        public void DuplicateArrayIndex()
        {
            // Arrange
            Expression<Func<float[], int, float>> expression = (arr, i) => arr[i];

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            float[] array = new float[] { 1f, 2f, 3f, 4f, 5f };
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(array, 2), duplicate.Compile().Invoke(array, 2));
        }

        [TestMethod]
        public void DuplicateArrayLength()
        {
            // Arrange
            Expression<Func<float[], int>> expression = (arr) => arr.Length;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            float[] array = new float[] { 1f, 2f, 3f, 4f, 5f };
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(array), duplicate.Compile().Invoke(array));
        }

        [TestMethod]
        public void DuplicateCall_NoArgs()
        {
            // Arrange
            Expression<Func<bool>> expression = () => MethodWithNoArg();

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(), duplicate.Compile().Invoke());
        }

        [TestMethod]
        public void DuplicateCall_Args()
        {
            // Arrange
            Expression<Func<bool, bool>> expression = (b) => MethodWithArg(b);

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true), duplicate.Compile().Invoke(true));
        }

        [TestMethod]
        public void DuplicateCoalesce()
        {
            // Arrange
            Expression<Func<int?, int>> expression = (i) => i ?? 0;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(null), duplicate.Compile().Invoke(null));
        }

        [TestMethod]
        public void DuplicateConditional()
        {
            // Arrange
            Expression<Func<int, int>> expression = (i) => (i < 0) ? -1 : 1;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(2), duplicate.Compile().Invoke(2));
        }

        [TestMethod]
        public void DuplicateConstant()
        {
            // Arrange
            Expression<Func<bool>> expression = () => true;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(), duplicate.Compile().Invoke());
        }

        [TestMethod]
        public void DuplicateConvert()
        {
            // Arrange
            Expression<Func<bool, object>> expression = (b) => (object)b;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true), duplicate.Compile().Invoke(true));
        }

        [TestMethod]
        public void DuplicateDivide()
        {
            // Arrange
            Expression<Func<float, float, float>> expression = (i, j) => i / j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1f, 2f), duplicate.Compile().Invoke(1f, 2f));
        }

        [TestMethod]
        public void DuplicateEqual()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i == j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1, 2), duplicate.Compile().Invoke(1, 2));
        }

        [TestMethod]
        public void DuplicateExclusiveOr()
        {
            // Arrange
            Expression<Func<int, int, int>> expression = (i, j) => i ^ j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(7, 3), duplicate.Compile().Invoke(7, 3));
        }

        [TestMethod]
        public void DuplicateGreaterThan()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i > j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(7, 3), duplicate.Compile().Invoke(7, 3));
        }

        [TestMethod]
        public void DuplicateGreaterThanOrEqual()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i >= j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(7, 3), duplicate.Compile().Invoke(7, 3));
        }

        [TestMethod]
        public void DuplicateInvoke()
        {
            // Arrange
            Func<bool> @delegate = MethodWithNoArg;
            Expression<Func<bool>> expression = () => @delegate();

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(), duplicate.Compile().Invoke());
        }

        [TestMethod]
        public void DuplicateLeftShift()
        {
            // Arrange
            Expression<Func<int, int>> expression = (i) => i << 2;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(5), duplicate.Compile().Invoke(5));
        }

        [TestMethod]
        public void DuplicateLessThan()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i < j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(7, 3), duplicate.Compile().Invoke(7, 3));
        }

        [TestMethod]
        public void DuplicateLessThanOrEqual()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i <= j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(7, 3), duplicate.Compile().Invoke(7, 3));
        }

        [TestMethod]
        public void DuplicateListInit()
        {
            // Arrange
            Expression<Func<List<string>>> expression = () => new List<string> {"Harry", "Fred"};

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            AssertListsAreEqual(expression.Compile().Invoke(), duplicate.Compile().Invoke());
        }

        [TestMethod]
        public void DuplicateMemberAccess()
        {
            // Arrange
            Expression<Func<DateTime, int>> expression = (dt) => dt.Year;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            DateTime date = new DateTime(2009, 01, 01);
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(date), duplicate.Compile().Invoke(date));
        }

        [TestMethod]
        public void DuplicateMemberInit()
        {
            // Arrange
            Expression<Func<Class>> expression = () => new Class() {Field = 2};

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke().Field, duplicate.Compile().Invoke().Field);
        }

        [TestMethod]
        public void DuplicateModulo()
        {
            // Arrange
            Expression<Func<int, int, int>> expression = (i, j) => i % j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(23, 5), duplicate.Compile().Invoke(23, 5));
        }

        [TestMethod]
        public void DuplicateMultiply()
        {
            // Arrange
            Expression<Func<float, float, float>> expression = (i, j) => i * j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(2f, 5f), duplicate.Compile().Invoke(2f, 5f));
        }

        [TestMethod]
        public void DuplicateNegate()
        {
            // Arrange
            Expression<Func<int, int>> expression = (i) => -i;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(2), duplicate.Compile().Invoke(2));
        }

        [TestMethod]
        public void DuplicateNew()
        {
            // Arrange
            Expression<Func<Class>> expression = () => new Class();

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
        }

        [TestMethod]
        public void DuplicateNewArrayInit()
        {
            // Arrange
            Expression<Func<int[]>> expression = () => new int[] {1, 2, 3, 4};

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            AssertListsAreEqual(expression.Compile().Invoke(), duplicate.Compile().Invoke());
        }

        [TestMethod]
        public void DuplicateNot()
        {
            // Arrange
            Expression<Func<bool, bool>> expression = (i) => !i;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true), duplicate.Compile().Invoke(true));
        }

        [TestMethod]
        public void DuplicateNotEqual()
        {
            // Arrange
            Expression<Func<int, int, bool>> expression = (i, j) => i != j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1, 3), duplicate.Compile().Invoke(1, 3));
        }

        [TestMethod]
        public void DuplicateOr()
        {
            // Arrange
            Expression<Func<bool, bool, bool>> expression = (i, j) => i | j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true, false), duplicate.Compile().Invoke(true, false));
        }

        [TestMethod]
        public void DuplicateOrElse()
        {
            // Arrange
            Expression<Func<bool, bool, bool>> expression = (i, j) => i || j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(true, false), duplicate.Compile().Invoke(true, false));
        }

        [TestMethod]
        public void DuplicateRightShift()
        {
            // Arrange
            Expression<Func<int, int>> expression = (i) => i >> 2;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(5), duplicate.Compile().Invoke(5));
        }

        [TestMethod]
        public void DuplicateSubtract()
        {
            // Arrange
            Expression<Func<int, int, int>> expression = (i, j) => i - j;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(1, 2), duplicate.Compile().Invoke(1, 2));
        }

        [TestMethod]
        public void DuplicateTypeAs()
        {
            // Arrange
            Expression<Func<IInterface, Class>> expression = (@interface) => @interface as Class;

            // Action
            var duplicate = duplicator.Duplicate(expression);

            // Assert
            IInterface interf = new Class();
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(interf), duplicate.Compile().Invoke(interf));
        }

        [TestMethod]
        public void DuplicateTypeIs()
        {
            // Arrange
            Expression<Func<IInterface, bool>> expression = (@interface) => @interface is Class;

            // Action
            Expression<Func<IInterface, bool>> duplicate = duplicator.Duplicate(expression);

            // Assert
            IInterface interf = new Class();
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile().Invoke(interf), duplicate.Compile().Invoke(interf));
        }


        #region Private Methods

        private static void AssertListsAreEqual<T>(IList<T> expected, IList<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        private static bool MethodWithNoArg()
        {
            return true;
        }

        private static bool MethodWithArg(bool arg)
        {
            return arg;
        }

        private interface IInterface
        {
            int Field { get; }
        }

        private class Class : IInterface
        {
            public int Field { get; set; }
        }
        #endregion
    }
}
