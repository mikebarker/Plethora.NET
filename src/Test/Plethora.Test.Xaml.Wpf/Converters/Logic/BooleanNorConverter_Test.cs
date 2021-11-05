using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Logic
{
    [TestClass]
    public class BooleanNotConverter_Test
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
