using System.Globalization;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters
{
    [TestClass]
    public class ConditionalConverter_Test
    {
        [TestMethod]
        public void Condition_True()
        {
            //setup
            bool t = true;
            int x = 1;
            int y = 2;
            ConditionalConverter converter = new ConditionalConverter();

            //exec
            object result = converter.Convert(
                new object[] {t, x, y},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(x, result);
        }

        [TestMethod]
        public void Condition_False()
        {
            //setup
            bool t = false;
            int x = 1;
            int y = 2;
            ConditionalConverter converter = new ConditionalConverter();

            //exec
            object result = converter.Convert(
                new object[] {t, x, y},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(y, result);
        }

        [TestMethod]
        public void Condition_Fail_NotBoolean()
        {
            //setup
            object t = new object();
            int x = 1;
            int y = 2;
            ConditionalConverter converter = new ConditionalConverter();

            //exec
            object result = converter.Convert(
                new object[] {t, x},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Condition_Fail_TooFewValues()
        {
            //setup
            bool t = false;
            int x = 1;
            int y = 2;
            ConditionalConverter converter = new ConditionalConverter();

            //exec
            object result = converter.Convert(
                new object[] {t, x},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }

        [TestMethod]
        public void Condition_Fail_TooManyValues()
        {
            //setup
            bool t = false;
            int x = 1;
            int y = 2;
            int z = 3;
            ConditionalConverter converter = new ConditionalConverter();

            //exec
            object result = converter.Convert(
                new object[] {t, x, y, z},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DependencyProperty.UnsetValue, result);
        }
    }
}
