using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters
{
    [TestClass]
    public class PrependListConverter_Test
    {
        [TestMethod]
        public void Single_PrependItem()
        {
            //setup
            int prependvalue = 0;
            List<int> list = new List<int> {1, 2, 3, 4, 5};
            PrependListConverter converter = new PrependListConverter();

            //exec
            object result = converter.Convert(
                list,
                typeof(object),
                prependvalue,
                CultureInfo.InvariantCulture);

            //test
            Assert.IsInstanceOfType(result, typeof(IEnumerable));

            List<object> enumerable = ((IEnumerable)result).Cast<object>().ToList();
            Assert.AreEqual(6, enumerable.Count);
            Assert.AreEqual(0, enumerable[0]);
            Assert.AreEqual(1, enumerable[1]);
            Assert.AreEqual(2, enumerable[2]);
            Assert.AreEqual(3, enumerable[3]);
            Assert.AreEqual(4, enumerable[4]);
            Assert.AreEqual(5, enumerable[5]);
        }

        [TestMethod]
        public void Single_PrependList()
        {
            //setup
            int[] prependvalue = { -2, -1, 0 };
            List<int> list = new List<int> {1, 2, 3, 4, 5};
            PrependListConverter converter = new PrependListConverter();

            //exec
            object result = converter.Convert(
                list,
                typeof(object),
                prependvalue,
                CultureInfo.InvariantCulture);

            //test
            Assert.IsInstanceOfType(result, typeof(IEnumerable));

            List<object> enumerable = ((IEnumerable)result).Cast<object>().ToList();
            Assert.AreEqual(8, enumerable.Count);
            Assert.AreEqual(-2, enumerable[0]);
            Assert.AreEqual(-1, enumerable[1]);
            Assert.AreEqual(0, enumerable[2]);
            Assert.AreEqual(1, enumerable[3]);
            Assert.AreEqual(2, enumerable[4]);
            Assert.AreEqual(3, enumerable[5]);
            Assert.AreEqual(4, enumerable[6]);
            Assert.AreEqual(5, enumerable[7]);
        }

        [TestMethod]
        public void Single_PrependString()
        {
            //setup
            string prependvalue = "omega";
            List<string> list = new List<string> {"alpha", "beta", "gamma"};
            PrependListConverter converter = new PrependListConverter();

            //exec
            object result = converter.Convert(
                list,
                typeof(object),
                prependvalue,
                CultureInfo.InvariantCulture);

            //test
            Assert.IsInstanceOfType(result, typeof(IEnumerable));

            List<object> enumerable = ((IEnumerable)result).Cast<object>().ToList();
            Assert.AreEqual(4, enumerable.Count);
            Assert.AreEqual("omega", enumerable[0]);
            Assert.AreEqual("alpha", enumerable[1]);
            Assert.AreEqual("beta", enumerable[2]);
            Assert.AreEqual("gamma", enumerable[3]);
        }

        [TestMethod]
        public void Multi_PrependItem()
        {
            //setup
            int a = 0;
            List<int> b = new List<int> { 1, 2, 3, 4, 5 };
            PrependListConverter converter = new PrependListConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b}, 
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.IsInstanceOfType(result, typeof(IEnumerable));

            List<object> enumerable = ((IEnumerable)result).Cast<object>().ToList();
            Assert.AreEqual(6, enumerable.Count);
            Assert.AreEqual(0, enumerable[0]);
            Assert.AreEqual(1, enumerable[1]);
            Assert.AreEqual(2, enumerable[2]);
            Assert.AreEqual(3, enumerable[3]);
            Assert.AreEqual(4, enumerable[4]);
            Assert.AreEqual(5, enumerable[5]);
        }

        [TestMethod]
        public void Multi_PrependItems()
        {
            //setup
            int a = -2;
            int b = -1;
            int c = 0;
            List<int> d = new List<int> { 1, 2, 3, 4, 5 };
            PrependListConverter converter = new PrependListConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b, c, d}, 
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.IsInstanceOfType(result, typeof(IEnumerable));

            List<object> enumerable = ((IEnumerable)result).Cast<object>().ToList();
            Assert.AreEqual(8, enumerable.Count);
            Assert.AreEqual(-2, enumerable[0]);
            Assert.AreEqual(-1, enumerable[1]);
            Assert.AreEqual(0, enumerable[2]);
            Assert.AreEqual(1, enumerable[3]);
            Assert.AreEqual(2, enumerable[4]);
            Assert.AreEqual(3, enumerable[5]);
            Assert.AreEqual(4, enumerable[6]);
            Assert.AreEqual(5, enumerable[7]);
        }
    }
}
