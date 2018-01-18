using System.IO;

namespace Plethora.Context.Help.Streaming
{
    public class TextStreamCapture : IDataStreamCapture<string>
    {
        public string CaptureDataFromStream(Stream stream)
        {
            using (TextReader reader = new StreamReader(stream))
            {
                string data = reader.ReadToEnd();
                return data;
            }
        }
    }
}
