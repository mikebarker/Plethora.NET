using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using NUnit.Framework;

using Plethora.Xaml.Converters;

namespace Plethora.Test.Xaml.Converters
{
    [TestFixture]
    public class ChainedConverter_Test
    {
        [Test]
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

        [Test]
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

        [Test]
        public void ConverterParameter()
        {
            // setup
            object value = new object();
            object innerParameter = new object();
            object parameter = new object();

            MockConverter innerConverter = new MockConverter();

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = innerConverter, ConverterParameter = innerParameter});

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(value, innerConverter.Value);
            Assert.AreEqual(typeof(object), innerConverter.TargetType);
            Assert.AreEqual(innerParameter, innerConverter.Parameter);
            Assert.AreEqual(CultureInfo.InvariantCulture, innerConverter.Culture);
        }

        [Test]
        public void ConverterParameter_GroupParameterSubstitution()
        {
            // setup
            object value = new object();
            object parameter = new object();

            MockConverter innerConverter = new MockConverter();

            ChainedConverter chainedConverter = new ChainedConverter();
            chainedConverter.Entries.Add(new ConverterEntry() { Converter = innerConverter, ConverterParameter = ConverterEntry.GroupParameter});

            //exec
            object result = chainedConverter.Convert(
                value,
                typeof(object),
                parameter,
                CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(value, innerConverter.Value);
            Assert.AreEqual(typeof(object), innerConverter.TargetType);
            Assert.AreEqual(parameter, innerConverter.Parameter);
            Assert.AreEqual(CultureInfo.InvariantCulture, innerConverter.Culture);
        }
    }
}
