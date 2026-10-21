using System;
using System.IO;

using Plethora.Context.Help.Factory;
using Plethora.Context.Help.Streaming;

namespace Plethora.Context.Help.LocalFileSystem;

/// <summary>
/// An implementation of <see cref="IHelpAccessor{TKey, TData}"/> which supports files stored on the local file system.
/// </summary>
/// <remarks>
/// The help key is the path of the file.
/// </remarks>
/// <seealso cref="LocalFileSystemHelpKeyer"/>
public class LocalFileSystemHelpAccessor<TData> : IHelpAccessor<string, TData>
{
    private readonly IDataStreamCapture<TData> dataStreamCapture;

    public LocalFileSystemHelpAccessor(IDataStreamCapture<TData> dataStreamCapture)
    {
        ArgumentNullException.ThrowIfNull(dataStreamCapture);

        this.dataStreamCapture = dataStreamCapture;
    }

    public TData GetData(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            using Stream stream = new FileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read);
            TData data = this.dataStreamCapture.CaptureDataFromStream(stream);
            return data;
        }
        catch (IOException)
        {
            return default!;
        }
    }
}
