using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.IO;
using Plethora.Test.MockClasses;

namespace Plethora.Test.IO
{
    /// <summary>
    /// Unit test class for the <see cref="MulticastWriter"/> class.
    /// </summary>
    [TestClass]
    public class MulticastWriter_Test
    {
        private readonly MockTextWriter writer1;
        private readonly MockTextWriter writer2;
        private MulticastWriter mw;

        public MulticastWriter_Test()
        {
            mw = new MulticastWriter();
            writer1 = new MockTextWriter();
            writer2 = new MockTextWriter();

            mw.RegisterWriter(writer1);
            mw.RegisterWriter(writer2);
        }

        [TestMethod]
        public void Encoding_Empty()
        {
            // Arrange
            mw = new MulticastWriter();

            // Action
            Encoding encoding = mw.Encoding;

            // Assert
            Assert.IsNotNull(encoding);
        }

        [TestMethod]
        public void Encoding()
        {
            // Action
            Encoding encoding = mw.Encoding;

            // Assert
            Assert.AreEqual(writer1.Encoding, encoding);
        }

        [TestMethod]
        public void Write_Char()
        {
            // Arrange
            char charac = 'c';

            // Action
            mw.Write(charac);

            // Assert
            Assert.AreEqual(charac.ToString(), writer1.CurrentText);
            Assert.AreEqual(charac.ToString(), writer2.CurrentText);
        }

        [TestMethod]
        public void Write_String()
        {
            // Arrange
            string text = "This is a test.";

            // Action
            mw.Write(text);

            // Assert
            Assert.AreEqual(text, writer1.CurrentText);
            Assert.AreEqual(text, writer2.CurrentText);
        }

        [TestMethod]
        public void Write_String_Null()
        {
            // Arrange
            string nullText = null;

            // Action
            mw.Write(nullText);

            // Assert
            Assert.AreEqual("", writer1.CurrentText);
            Assert.AreEqual("", writer2.CurrentText);
        }

        [TestMethod]
        public void DeregisterWriter()
        {
            // Action
            mw.DeregisterWriter(writer2);

            // Assert
            string text = "This is a test.";
            mw.Write(text);
            Assert.AreEqual(text, writer1.CurrentText);
            Assert.AreEqual("", writer2.CurrentText);
        }

    }
}
