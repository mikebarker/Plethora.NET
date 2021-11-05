using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Comparison
{
    [TestClass]
    public class IsNullConverter_Test
    {
        [TestMethod]
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

        [TestMethod]
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
