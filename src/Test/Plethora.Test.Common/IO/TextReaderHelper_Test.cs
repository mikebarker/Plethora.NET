using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.IO;
using Plethora.Test._UtilityClasses;
using Plethora.Test.MockClasses;
using System;

namespace Plethora.Test.IO
{
    [TestClass]
    public class TextReaderHelper_Test
    {
        [TestMethod]
        public void CopyTo()
        {
            MockTextReader reader = new MockTextReader();
            MockTextWriter writer = new MockTextWriter();

            // Action
#pragma warning disable CS0618 // Type or member is obsolete
            TextReaderHelper.CopyTo(reader, writer);
#pragma warning restore CS0618 // Type or member is obsolete

            // Assert
            Assert.AreEqual("", writer.CurrentText);

            // Action
            reader.AppendText("Hello");

            // Assert
            var result = Wait.For(() => writer.CurrentText == "Hello", TimeSpan.FromSeconds(1));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CopyToAsync()
        {
            MockTextReader reader = new MockTextReader();
            MockTextWriter writer = new MockTextWriter();

            // Action
            var task = TextReaderHelper.CopyToAsync(reader, writer);

            // Assert
            Assert.AreEqual("", writer.CurrentText);

            // Action
            reader.AppendText("Hello");

            // Assert
            var result = Wait.For(() => writer.CurrentText == "Hello", TimeSpan.FromSeconds(1));
            Assert.IsTrue(result);
        }
    }
}
