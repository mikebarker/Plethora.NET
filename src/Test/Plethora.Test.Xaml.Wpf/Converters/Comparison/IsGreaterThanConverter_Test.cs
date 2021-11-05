using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Test.Xaml.UtilityClasses;
using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Comparison
{
    [TestClass]
    public class IsGreaterThanConverter_Test
    {
        [TestMethod]
        public void Convert_Single_AreEqual()
        {
            //setup
            int value = 13;
            int parameter = 13;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Single_IsLessThan()
        {
            //setup
            int value = 4;
            int parameter = 13;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Single_IsGreaterThan()
        {
            //setup
            int value = 13;
            int parameter = 4;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Convert_Multi_AreEqual_DefaultComparer()
        {
            //setup
            int a = 13;
            int b = 13;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

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
        public void Convert_Multi_IsLessThan_DefaultComparer()
        {
            //setup
            int a = 4;
            int b = 13;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Multi_IsGreaterThan_DefaultComparer()
        {
            //setup
            int a = 13;
            int b = 4;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Convert_Multi_AreEqual_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Bob_Jameson2;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Multi_IsLessThan_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Harry_Porker;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Convert_Multi_IsGreaterThan_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Amy_Cathson;
            IsGreaterThanConverter converter = new IsGreaterThanConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }
    }
}
