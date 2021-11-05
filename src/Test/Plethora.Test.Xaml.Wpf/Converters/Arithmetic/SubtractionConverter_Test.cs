using System;
using System.Globalization;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Arithmetic
{
    [TestClass]
    public class SubtractionConverter_Test
    {
        [TestMethod]
        public void Fail_NullValues()
        {
            //setup
            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                null,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Fail_1Values()
        {
            //setup
            int a = 15;
            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Fail_3Values()
        {
            //setup
            int a = 15;
            int b = 4;
            int c = 1;
            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b, c },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Int32_Int32()
        {
            //setup
            int a = 15;
            int b = 4;
            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(a - b, result);
        }

        [TestMethod]
        public void Int32_Int32_ToInt64()
        {
            //setup
            int a = 15;
            int b = 4;
            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(Int64),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual((Int64)(a - b), result);
            Assert.AreEqual(typeof(Int64), result.GetType());
        }

        [TestMethod]
        public void DateTime_TimeSpan()
        {
            //setup
            DateTime a = new DateTime(2000, 01, 01);
            TimeSpan b = new TimeSpan(1, 0, 0, 0, 0);

            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(a - b, result);
        }

        [TestMethod]
        public void CustomOperator()
        {
            //setup
            Rational a = new Rational(1, 2);
            Rational b = new Rational(1, 3);

            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(a - b, result);
        }

        [TestMethod]
        public void Fail_InvalidOperator()
        {
            //setup
            DateTime a = new DateTime(2000, 01, 01);
            int b = 7;

            SubtractionConverter converter = new SubtractionConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }
    }
}
