using System.Globalization;

using NUnit.Framework;

using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters.Comparison
{
    [TestFixture]
    public class IsNullConverter_Test
    {
        [Test]
        public void Convert_Null()
        {
            //setup
            object value = null;
            IsNullConverter converter = new IsNullConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Convert_NotNull()
        {
            //setup
            object value = new object();
            IsNullConverter converter = new IsNullConverter();

            //exec
            object result = converter.Convert(
                value,
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(false, result);
        }
    }
}
