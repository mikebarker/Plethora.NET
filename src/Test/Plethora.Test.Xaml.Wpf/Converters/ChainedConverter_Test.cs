using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Plethora.Xaml.Converters;
using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters
{
    [TestClass]
    public class ChainedConverter_Test
    {
        [TestMethod]
        public void IsNotNullToVisibile_Null()
        {
            // setup
            object value = null;

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new IsNullConverter(), ConverterParameter = null});
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new BooleanNotConverter(), ConverterParameter = null});
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new BooleanToVisibilityConverter(), ConverterParameter = null});

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(Visibility),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(Visibility.Collapsed, result);
        }

        [TestMethod]
        public void IsNotNullToVisibile_NotNull()
        {
            // setup
            object value = new object();

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new IsNullConverter(), ConverterParameter = null});
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new BooleanNotConverter(), ConverterParameter = null});
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = new BooleanToVisibilityConverter(), ConverterParameter = null});

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(Visibility),
                null,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(Visibility.Visible, result);
        }

        [TestMethod]
        public void ConverterParameter()
        {
            // setup
            object value = new object();
            object innerParameter = new object();
            object parameter = new object();

            object convertedValue = new object();

            Mock<IValueConverter> innerConverterMock = new Mock<IValueConverter>();
            innerConverterMock
                .Setup(m => m.Convert(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()))
                .Returns(convertedValue);

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = innerConverterMock.Object, ConverterParameter = innerParameter});

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreSame(convertedValue, result);
            innerConverterMock.Verify(m => m.Convert(value, typeof(object), innerParameter, CultureInfo.InvariantCulture), Times.Once);
        }

        [TestMethod]
        public void ConverterParameter_GroupParameterSubstitution()
        {
            // setup
            object value = new object();
            object parameter = new object();

            object convertedValue = new object();

            Mock<IValueConverter> innerConverterMock = new Mock<IValueConverter>();
            innerConverterMock
                .Setup(m => m.Convert(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()))
                .Returns(convertedValue);

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = innerConverterMock.Object, ConverterParameter = new GroupParameter() });

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreSame(convertedValue, result);
            innerConverterMock.Verify(m => m.Convert(value, typeof(object), parameter, CultureInfo.InvariantCulture), Times.Once);
        }
    }
}
