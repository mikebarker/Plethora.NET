using System.Globalization;

using NUnit.Framework;

using Plethora.Test.Xaml.UtilityClasses;
using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters.Comparison
{
    [TestFixture]
    public class IsLessThanOrEqualToConverter_Test
    {
        [Test]
        public void Convert_Single_AreEqual()
        {
            //setup
            int value = 13;
            int parameter = 13;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_Single_IsLessThan()
        {
            //setup
            int value = 4;
            int parameter = 13;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_Single_IsGreaterThan()
        {
            //setup
            int value = 13;
            int parameter = 4;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Convert_Multi_AreEqual_DefaultComparer()
        {
            //setup
            int a = 13;
            int b = 13;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

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
        public void Convert_Multi_IsLessThan_DefaultComparer()
        {
            //setup
            int a = 4;
            int b = 13;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_Multi_IsGreaterThan_DefaultComparer()
        {
            //setup
            int a = 13;
            int b = 4;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Convert_Multi_AreEqual_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Bob_Jameson2;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_Multi_IsLessThan_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Harry_Porker;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_Multi_IsGreaterThan_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Amy_Cathson;
            IsLessThanOrEqualToConverter converter = new IsLessThanOrEqualToConverter();

            //exec
            object result = converter.Convert(
                new object[] { a, b },
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }
    }
}
