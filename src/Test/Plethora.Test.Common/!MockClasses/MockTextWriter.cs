using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Plethora.Test.MockClasses
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

        public override Task WriteAsync(char value)
        {
            sb.Append(value);
            return Task.CompletedTask;
        }


        public string CurrentText
        {
            get { return sb.ToString(); }
        }
    }
}
