using System;
using System.IO;

using Plethora.Context.Help.Factory;

namespace Plethora.Context.Help.LocalFileSystem
{
    /// <summary>
    /// An implementation of <see cref="IHelpAccessor{TKey, TData}"/> which supports files stored on the local file system.
    /// </summary>
    /// <remarks>
    /// The help key is the path of the file.
    /// </remarks>
    /// <seealso cref="LocalFileSystemHelpKeyer"/>
    public class LocalFileSystemHelpAccessor : IHelpAccessor<string, string>
    {
        public string GetData(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            try
            {
                using (Stream stream = new FileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string data = reader.ReadToEnd();
                    return data;
                }
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
}
