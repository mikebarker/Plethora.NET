using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Plethora.Context.Help.Factory;

namespace Plethora.Context.Help.LocalFileSystem
{
    /// <summary>
    /// An implementation of <see cref="IHelpKeyer{TKey}"/> which supports files stored on the local file system.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///    The help key is the path of the file.
    ///  </para>
    ///  <para>
    ///   The key is produced by extending the root path by the context's name.
    ///   Dots [.] are replaced by path separators [\].
    ///  </para>
    /// </remarks>
    /// <seealso cref="LocalFileSystemHelpAccessor"/>
    public class LocalFileSystemHelpKeyer : IHelpKeyer<string>
    {
        private readonly FileSystemWatcher fileSystemWatcher;

        private readonly ReaderWriterLockSlim rwLock = new ReaderWriterLockSlim();
        private volatile Dictionary<string, string> keyPathMap;


        public LocalFileSystemHelpKeyer(string root, string filePattern)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            if (filePattern == null)
                throw new ArgumentNullException(nameof(filePattern));


            this.fileSystemWatcher = new FileSystemWatcher(Path.GetFullPath(root), filePattern);

            this.RefreshFileCache();

            this.fileSystemWatcher.Created += this.FileSystemWatcher_Change;
            this.fileSystemWatcher.Deleted += this.FileSystemWatcher_Change;
            this.fileSystemWatcher.Renamed += this.FileSystemWatcher_Change;

        }

        public string GetHelpKey(ContextInfo context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            Dictionary<string, string> map;

            this.rwLock.EnterReadLock();
            try
            {
                map = this.keyPathMap;
            }
            finally
            {
                this.rwLock.ExitReadLock();
            }

            string path;
            if (!map.TryGetValue(context.Name, out path))
                return null;

            return path;
        }

        private void FileSystemWatcher_Change(object sender, FileSystemEventArgs e)
        {
            this.RefreshFileCache();
        }

        private void RefreshFileCache()
        {
            this.rwLock.EnterWriteLock();
            try
            {
                string root = this.fileSystemWatcher.Path;
                string filePattern = this.fileSystemWatcher.Filter;

                string[] filePathes = Directory.GetFiles(root, filePattern, SearchOption.AllDirectories);

                Dictionary<string, string> map = filePathes
                    .ToDictionary(path => GetContextKey(path, root));

                this.keyPathMap = map;
            }
            finally
            {
                this.rwLock.ExitWriteLock();
            }
        }

        private static string GetContextKey(string path, string root)
        {
            string key = path;

            if (key.StartsWith(root))
                key = key.Substring(root.Length);

            if ((key.Length > 1) && ((key[0] == Path.DirectorySeparatorChar) || (key[0] == Path.AltDirectorySeparatorChar)))
                key = key.Substring(1);

            string extension = Path.GetExtension(path);
            if (extension != null)
                key = key.Substring(0, key.Length - extension.Length);

            return key;
        }
    }
}
