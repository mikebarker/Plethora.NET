using System.IO;

namespace Plethora.Context.Help.Streaming
{
    public interface IDataStreamCapture<TData>
    {
        TData CaptureDataFromStream(Stream stream);
    }
}
