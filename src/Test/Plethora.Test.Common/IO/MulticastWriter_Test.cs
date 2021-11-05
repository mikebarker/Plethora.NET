using System.Text;
using System.Threading.Tasks;
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
            writer1 = new MockTextWriter();
            writer2 = new MockTextWriter();

            mw = new MulticastWriter(
                writer1,
                writer2);
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
            Assert.AreEqual("c", writer1.CurrentText);
            Assert.AreEqual("c", writer2.CurrentText);
        }

        [TestMethod]
        public void Write_String()
        {
            // Arrange
            string text = "This is a test.";

            // Action
            mw.Write(text);

            // Assert
            Assert.AreEqual("This is a test.", writer1.CurrentText);
            Assert.AreEqual("This is a test.", writer2.CurrentText);
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
        public async Task WriteAsync_Char()
        {
            // Arrange
            char charac = 'c';

            // Action
            await mw.WriteAsync(charac).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("c", writer1.CurrentText);
            Assert.AreEqual("c", writer2.CurrentText);
        }

        [TestMethod]
        public async Task WriteAsync_String()
        {
            // Arrange
            string text = "This is a test.";

            // Action
            await mw.WriteAsync(text).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("This is a test.", writer1.CurrentText);
            Assert.AreEqual("This is a test.", writer2.CurrentText);
        }

        [TestMethod]
        public async Task WriteAsync_String_Null()
        {
            // Arrange
            string nullText = null;

            // Action
            await mw.WriteAsync(nullText).ConfigureAwait(false);

            // Assert
            Assert.AreEqual("", writer1.CurrentText);
            Assert.AreEqual("", writer2.CurrentText);
        }
    }
}
