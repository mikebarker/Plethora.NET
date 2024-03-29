﻿using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Test.Xaml.UtilityClasses;
using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Comparison
{
    [TestClass]
    public class IsEqualConverter_Test
    {
        [TestMethod]
        public void Convert_Single_AreEqual()
        {
            //setup
            int value = 13;
            int parameter = 13;
            IsEqualConverter converter = new IsEqualConverter();

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
        public void Convert_Single_AreNotEqual()
        {
            //setup
            int value = 45;
            int parameter = 13;
            IsEqualConverter converter = new IsEqualConverter();

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
        public void Convert_Multi_AreEqual_DefaultComparer()
        {
            //setup
            int a = 13;
            int b = 13;
            IsEqualConverter converter = new IsEqualConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Convert_Multi_AreNotEqual_DefaultComparer()
        {
            //setup
            int a = 45;
            int b = 13;
            IsEqualConverter converter = new IsEqualConverter();

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
        public void Convert_Multi_AreEqual_WithComparer()
        {
            //setup
            Person a = Person.Bob_Jameson;
            Person b = Person.Bob_Jameson2;
            IsEqualConverter converter = new IsEqualConverter();

            //exec
            object result = converter.Convert(
                new object[] {a, b},
                typeof(object),
                new Person.NameComparer(),
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Convert_Multi_AreNotEqual_WithComparer()
        {
            //setup
            Person a = Person.Amy_Cathson;
            Person b = Person.Bob_Jameson;
            IsEqualConverter converter = new IsEqualConverter();

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
