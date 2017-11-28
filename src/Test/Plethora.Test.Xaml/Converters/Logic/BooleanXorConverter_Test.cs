using System.Globalization;
using System.Windows;

using NUnit.Framework;

using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters.Logic
{
    [TestFixture]
    public class BooleanXorConverter_Test
    {
        [Test]
        public void TrueTrue()
        {
            //setup
            bool a = true;
            bool b = true;
            BooleanXorConverter converter = new BooleanXorConverter();

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
        public void TrueFalse()
        {
            //setup
            bool a = true;
            bool b = false;
            BooleanXorConverter converter = new BooleanXorConverter();

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
            BooleanXorConverter converter = new BooleanXorConverter();

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
            BooleanXorConverter converter = new BooleanXorConverter();

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
            BooleanXorConverter converter = new BooleanXorConverter();

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
