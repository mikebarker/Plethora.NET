using System;
using System.Globalization;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Arithmetic
{
    [TestClass]
    public class ModulusConverter_Test
    {
        [TestMethod]
        public void Fail_NullValues()
        {
            //setup
            ModulusConverter converter = new ModulusConverter();

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
            ModulusConverter converter = new ModulusConverter();

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
            ModulusConverter converter = new ModulusConverter();

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
            ModulusConverter converter = new ModulusConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(a % b, result);
        }

        [TestMethod]
        public void Int32_Int32_ToInt64()
        {
            //setup
            int a = 15;
            int b = 4;
            ModulusConverter converter = new ModulusConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(Int64),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual((Int64)(a % b), result);
            Assert.AreEqual(typeof(Int64), result.GetType());
        }

        [TestMethod]
        public void CustomOperator()
        {
            //setup
            Rational a = new Rational(1, 2);
            Rational b = new Rational(1, 3);

            ModulusConverter converter = new ModulusConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(a % b, result);
        }

        [TestMethod]
        public void Fail_InvalidOperator()
        {
            //setup
            DateTime a = new DateTime(2000, 01, 01);
            int b = 7;

            ModulusConverter converter = new ModulusConverter();

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
