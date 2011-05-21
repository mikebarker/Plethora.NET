using System.IO;
using System.Text;

namespace ChocolateBox.Test.MockClasses
{
    class MockTextWriter : TextWriter
    {
        private readonly StringBuilder sb = new StringBuilder();

        public override Encoding Encoding
        {
            get { return Encoding.Unicode; }
        }

        public override void Write(char value)
        {
            sb.Append(value);
        }


        public string CurrentText
        {
            get { return sb.ToString(); }
        }
    }
}
