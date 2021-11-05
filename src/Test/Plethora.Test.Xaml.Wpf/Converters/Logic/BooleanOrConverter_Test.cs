using System.Globalization;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Logic
{
    [TestClass]
    public class BooleanOrConverter_Test
    {
        [TestMethod]
        public void TrueTrue()
        {
            //setup
            bool a = true;
            bool b = true;
            BooleanOrConverter converter = new BooleanOrConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TrueFalse()
        {
            //setup
            bool a = true;
            bool b = false;
            BooleanOrConverter converter = new BooleanOrConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FalseFalse()
        {
            //setup
            bool a = false;
            bool b = false;
            BooleanOrConverter converter = new BooleanOrConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Fail_OneValue()
        {
            //setup
            bool a = false;
            BooleanOrConverter converter = new BooleanOrConverter();

            //exec
            object result = converter.Convert(
                new object[] {a},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Fail_ManyValues()
        {
            //setup
            bool a = false;
            bool b = false;
            bool c = false;
            BooleanOrConverter converter = new BooleanOrConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b, c},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }
    }
}
