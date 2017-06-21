using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NUnit.Framework;
using Plethora.Test.ExtensionClasses;

namespace Plethora.Test.ExpressionAide
{
    [TestFixture]
    public class ExpressionDuplicator_Test
    {
        private ExpressionDuplicatorEx duplicator;

        [SetUp]
        public void SetUp()
        {
            duplicator = new ExpressionDuplicatorEx();
        }

        [Test]
        public void DuplicateAdd()
        {
            //Setup
            Expression<Func<int, int, int>> expression = (i, j) => i + j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(1, 2), duplicate.Compile()(1, 2));
        }

        [Test]
        public void DuplicateAnd()
        {
            //Setup
            Expression<Func<bool, bool, bool>> expression = (i, j) => i & j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true, false), duplicate.Compile()(true, false));
        }

        [Test]
        public void DuplicateAndAlso()
        {
            //Setup
            Expression<Func<bool, bool, bool>> expression = (i, j) => i && j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true, false), duplicate.Compile()(true, false));
        }

        [Test]
        public void DuplicateArrayIndex()
        {
            //Setup
            Expression<Func<float[], int, float>> expression = (arr, i) => arr[i];

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            float[] array = new float[] { 1f, 2f, 3f, 4f, 5f };
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(array, 2), duplicate.Compile()(array, 2));
        }

        [Test]
        public void DuplicateArrayLength()
        {
            //Setup
            Expression<Func<float[], int>> expression = (arr) => arr.Length;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            float[] array = new float[] { 1f, 2f, 3f, 4f, 5f };
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(array), duplicate.Compile()(array));
        }

        [Test]
        public void DuplicateCall_NoArgs()
        {
            //Setup
            Expression<Func<bool>> expression = () => MethodWithNoArg();

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(), duplicate.Compile()());
        }

        [Test]
        public void DuplicateCall_Args()
        {
            //Setup
            Expression<Func<bool, bool>> expression = (b) => MethodWithArg(b);

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true), duplicate.Compile()(true));
        }

        [Test]
        public void DuplicateCoalesce()
        {
            //Setup
            Expression<Func<int?, int>> expression = (i) => i ?? 0;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(null), duplicate.Compile()(null));
        }

        [Test]
        public void DuplicateConditional()
        {
            //Setup
            Expression<Func<int, int>> expression = (i) => (i < 0) ? -1 : 1;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(2), duplicate.Compile()(2));
        }

        [Test]
        public void DuplicateConstant()
        {
            //Setup
            Expression<Func<bool>> expression = () => true;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(), duplicate.Compile()());
        }

        [Test]
        public void DuplicateConvert()
        {
            //Setup
            Expression<Func<bool, object>> expression = (b) => (object)b;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true), duplicate.Compile()(true));
        }

        [Test]
        public void DuplicateDivide()
        {
            //Setup
            Expression<Func<float, float, float>> expression = (i, j) => i / j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(1f, 2f), duplicate.Compile()(1f, 2f));
        }

        [Test]
        public void DuplicateEqual()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i == j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(1, 2), duplicate.Compile()(1, 2));
        }

        [Test]
        public void DuplicateExclusiveOr()
        {
            //Setup
            Expression<Func<int, int, int>> expression = (i, j) => i ^ j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(7, 3), duplicate.Compile()(7, 3));
        }

        [Test]
        public void DuplicateGreaterThan()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i > j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(7, 3), duplicate.Compile()(7, 3));
        }

        [Test]
        public void DuplicateGreaterThanOrEqual()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i >= j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(7, 3), duplicate.Compile()(7, 3));
        }

        [Test]
        public void DuplicateInvoke()
        {
            //Setup
            Func<bool> @delegate = MethodWithNoArg;
            Expression<Func<bool>> expression = () => @delegate();

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(), duplicate.Compile()());
        }

        [Test]
        public void DuplicateLeftShift()
        {
            //Setup
            Expression<Func<int, int>> expression = (i) => i << 2;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(5), duplicate.Compile()(5));
        }

        [Test]
        public void DuplicateLessThan()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i < j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(7, 3), duplicate.Compile()(7, 3));
        }

        [Test]
        public void DuplicateLessThanOrEqual()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i <= j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(7, 3), duplicate.Compile()(7, 3));
        }

        [Test]
        public void DuplicateListInit()
        {
            //Setup
            Expression<Func<List<string>>> expression = () => new List<string> {"Harry", "Fred"};

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(), duplicate.Compile()());
        }

        [Test]
        public void DuplicateMemberAccess()
        {
            //Setup
            Expression<Func<DateTime, int>> expression = (dt) => dt.Year;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            DateTime date = new DateTime(2009, 01, 01);
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(date), duplicate.Compile()(date));
        }

        [Test]
        public void DuplicateMemberInit()
        {
            //Setup
            Expression<Func<Class>> expression = () => new Class() {Field = 2};

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()().Field, duplicate.Compile()().Field);
        }

        [Test]
        public void DuplicateModulo()
        {
            //Setup
            Expression<Func<int, int, int>> expression = (i, j) => i % j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(23, 5), duplicate.Compile()(23, 5));
        }

        [Test]
        public void DuplicateMultiply()
        {
            //Setup
            Expression<Func<float, float, float>> expression = (i, j) => i * j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(2f, 5f), duplicate.Compile()(2f, 5f));
        }

        [Test]
        public void DuplicateNegate()
        {
            //Setup
            Expression<Func<int, int>> expression = (i) => -i;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(2), duplicate.Compile()(2));
        }

        [Test]
        public void DuplicateNew()
        {
            //Setup
            Expression<Func<Class>> expression = () => new Class();

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
        }

        [Test]
        public void DuplicateNewArrayInit()
        {
            //Setup
            Expression<Func<int[]>> expression = () => new int[] {1, 2, 3, 4};

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(), duplicate.Compile()());
        }

        [Test]
        public void DuplicateNot()
        {
            //Setup
            Expression<Func<bool, bool>> expression = (i) => !i;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true), duplicate.Compile()(true));
        }

        [Test]
        public void DuplicateNotEqual()
        {
            //Setup
            Expression<Func<int, int, bool>> expression = (i, j) => i != j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(1, 3), duplicate.Compile()(1, 3));
        }

        [Test]
        public void DuplicateOr()
        {
            //Setup
            Expression<Func<bool, bool, bool>> expression = (i, j) => i | j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true, false), duplicate.Compile()(true, false));
        }

        [Test]
        public void DuplicateOrElse()
        {
            //Setup
            Expression<Func<bool, bool, bool>> expression = (i, j) => i || j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(true, false), duplicate.Compile()(true, false));
        }

        [Test]
        public void DuplicateRightShift()
        {
            //Setup
            Expression<Func<int, int>> expression = (i) => i >> 2;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(5), duplicate.Compile()(5));
        }

        [Test]
        public void DuplicateSubtract()
        {
            //Setup
            Expression<Func<int, int, int>> expression = (i, j) => i - j;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(1, 2), duplicate.Compile()(1, 2));
        }

        [Test]
        public void DuplicateTypeAs()
        {
            //Setup
            Expression<Func<IInterface, Class>> expression = (@interface) => @interface as Class;

            //Execute
            var duplicate = duplicator.Duplicate(expression);

            //Test
            IInterface interf = new Class();
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(interf), duplicate.Compile()(interf));
        }

        [Test]
        public void DuplicateTypeIs()
        {
            //Setup
            Expression<Func<IInterface, bool>> expression = (@interface) => @interface is Class;

            //Execute
            Expression<Func<IInterface, bool>> duplicate = duplicator.Duplicate(expression);

            //Test
            IInterface interf = new Class();
            Assert.AreNotSame(expression, duplicate);
            Assert.AreEqual(expression.ToString(), duplicate.ToString());
            Assert.AreEqual(expression.Compile()(interf), duplicate.Compile()(interf));
        }


        #region Private Methods

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
