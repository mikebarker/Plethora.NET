using System.Globalization;

using NUnit.Framework;

using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters.Logic
{
    [TestFixture]
    public class BooleanNotConverter_Test
    {
        [Test]
        public void Convert_True()
        {
            //setup
            bool a = true;
            BooleanNotConverter converter = new BooleanNotConverter();

            //exec
            object result = converter.Convert(
                a,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Convert_False()
        {
            //setup
            bool a = false;
            BooleanNotConverter converter = new BooleanNotConverter();

            //exec
            object result = converter.Convert(
                a,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void ConvertBack_True()
        {
            //setup
            bool a = true;
            BooleanNotConverter converter = new BooleanNotConverter();

            //exec
            object result = converter.ConvertBack(
                a,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ConvertBack_False()
        {
            //setup
            bool a = false;
            BooleanNotConverter converter = new BooleanNotConverter();

            //exec
            object result = converter.ConvertBack(
                a,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }
    }
}
