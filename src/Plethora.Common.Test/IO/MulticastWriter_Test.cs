using System.Text;
using NUnit.Framework;
using Plethora.IO;
using Plethora.Test.MockClasses;

namespace Plethora.Test.IO
{
    /// <summary>
    /// Unit test class for the <see cref="MulticastWriter"/> class.
    /// </summary>
    [TestFixture]
    public class MulticastWriter_Test
    {
        MulticastWriter mw;
        MockTextWriter writer1;
        MockTextWriter writer2;

        [SetUp]
        public void Setup()
        {
            mw = new MulticastWriter();
            writer1 = new MockTextWriter();
            writer2 = new MockTextWriter();

            mw.RegisterWriter(writer1);
            mw.RegisterWriter(writer2);
        }

        [Test]
        public void Encoding_Empty()
        {
            //init
            mw = new MulticastWriter();

            //exec
            Encoding encoding = mw.Encoding;

            //test
            Assert.IsNotNull(encoding);
        }

        [Test]
        public void Encoding()
        {
            //exec
            Encoding encoding = mw.Encoding;

            //test
            Assert.AreEqual(writer1.Encoding, encoding);
        }

        [Test]
        public void Write_Char()
        {
            //init
            char charac = 'c';

            //exec
            mw.Write(charac);

            //test
            Assert.AreEqual(charac.ToString(), writer1.CurrentText);
            Assert.AreEqual(charac.ToString(), writer2.CurrentText);
        }

        [Test]
        public void Write_String()
        {
            //init
            string text = "This is a test.";

            //exec
            mw.Write(text);

            //test
            Assert.AreEqual(text, writer1.CurrentText);
            Assert.AreEqual(text, writer2.CurrentText);
        }

        [Test]
        public void Write_String_Null()
        {
            //init
            string nullText = null;

            //exec
            mw.Write(nullText);

            //test
            Assert.AreEqual("", writer1.CurrentText);
            Assert.AreEqual("", writer2.CurrentText);
        }

        [Test]
        public void DeregisterWriter()
        {
            //exec
            mw.DeregisterWriter(writer2);

            //test
            string text = "This is a test.";
            mw.Write(text);
            Assert.AreEqual(text, writer1.CurrentText);
            Assert.AreEqual("", writer2.CurrentText);
        }

    }
}
