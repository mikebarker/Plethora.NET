using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.Test.MockClasses
{
    class MockTextReader : TextReader
    {
        private readonly StringBuilder sb = new StringBuilder();
        private readonly AutoResetEvent textReadyEvent = new AutoResetEvent(false);
        private int currentIndex = 0;

        public void AppendText(string str)
        {
            sb.Append(str);
            textReadyEvent.Set();
        }

        public override int Read(char[] buffer, int index, int count)
        {
            var actualCount = 0;

            if (!(this.currentIndex < this.sb.Length))
            {
                textReadyEvent.WaitOne();
            }

            int i = 0;
            while ((actualCount < count) && (this.currentIndex < this.sb.Length))
            {
                buffer[i++] = this.sb[this.currentIndex++];
                actualCount++;
            }

            return actualCount;
        }

        public override Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return Task.Run(() => this.Read(buffer, index, count));
        }
    }
}
