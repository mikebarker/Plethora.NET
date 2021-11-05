using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.IO;
using Plethora.Test._UtilityClasses;
using Plethora.Test.MockClasses;
using System;
using System.Threading;

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
            TextReaderHelper.CopyTo(reader, writer);

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
