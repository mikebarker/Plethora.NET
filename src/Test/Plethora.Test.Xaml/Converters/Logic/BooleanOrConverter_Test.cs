using System.Globalization;
using System.Windows;

using NUnit.Framework;

using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters.Logic
{
    [TestFixture]
    public class BooleanOrConverter_Test
    {
        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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
