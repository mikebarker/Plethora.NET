using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plethora.IO
{
    public static class TextReaderHelper
    {
        /// <summary>
        /// Copies the content of a <see cref="TextReader"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="reader">The originating <see cref="TextReader"/>.</param>
        /// <param name="writer">The desitination <see cref="TextWriter"/> to which the data must be copied.</param>
        /// <returns>
        /// A <see cref="Task"/> which completes when all the content from <paramref name="reader"/> has been
        /// written to <paramref name="writer"/>.
        /// </returns>
        public static Task CopyToAsync(this TextReader reader, TextWriter writer)
        {
            return CopyToAsync(reader, writer, null);
        }

        /// <summary>
        /// Copies the content of a <see cref="TextReader"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="reader">The originating <see cref="TextReader"/>.</param>
        /// <param name="writer">The desitination <see cref="TextWriter"/> to which the data must be copied.</param>
        /// <param name="interceptAction">
        /// An action to be performed as data is read from the reader before it is written to the writer.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> which completes when all the content from <paramref name="reader"/> has been
        /// written to <paramref name="writer"/>.
        /// </returns>
        public static async Task CopyToAsync(this TextReader reader, TextWriter writer, Action<char[], int, int> interceptAction)
        {
            //Validation
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (writer == null)
                throw new ArgumentNullException(nameof(writer));

            //Use the default internal stream buffer size.
            const int BUFFER_SIZE = 4096;

            int bytes;
            char[] buffer = new char[BUFFER_SIZE];
            while ((bytes = await reader.ReadAsync(buffer, 0, BUFFER_SIZE).ConfigureAwait(false)) > 0)
            {
                if (interceptAction != null)
                    interceptAction(buffer, 0, bytes);

                await writer.WriteAsync(buffer, 0, bytes).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Copies the content of a <see cref="TextReader"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="reader">The originating <see cref="TextReader"/>.</param>
        /// <param name="writer">The desitination <see cref="TextWriter"/> to which the data must be copied.</param>
        /// <remarks>
        /// This is a non-blocking function and returns immidiately. Reads from <paramref name="reader"/> and
        /// writes to <paramref name="writer"/> are performed on a separate thread.
        /// </remarks>
        [Obsolete("Prefer the async version of this fucntion.")]
        public static void CopyTo(this TextReader reader, TextWriter writer)
        {
            CopyTo(reader, writer, null);
        }

        /// <summary>
        /// Copies the content of a <see cref="TextReader"/> to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="reader">The originating <see cref="TextReader"/>.</param>
        /// <param name="writer">The desitination <see cref="TextWriter"/> to which the data must be copied.</param>
        /// <param name="interceptAction">
        /// An action to be performed as data is read from the reader before it is written to the writer.
        /// </param>
        /// <remarks>
        /// This is a non-blocking function and returns immidiately. Reads from <paramref name="reader"/> and
        /// writes to <paramref name="writer"/> are performed on a separate thread.
        /// </remarks>
        [Obsolete("Prefer the async version of this fucntion.")]
        public static void CopyTo(this TextReader reader, TextWriter writer, Action<char[], int, int>  interceptAction)
        {
            //Validation
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (writer == null)
                throw new ArgumentNullException(nameof(writer));


            ThreadStart copyToDelegate = delegate
                {
                    //Use the default internal stream buffer size.
                    const int BUFFER_SIZE = 4096;

                    int bytes;
                    char[] buffer = new char[BUFFER_SIZE];
                    while ((bytes = reader.Read(buffer, 0, BUFFER_SIZE)) > 0)
                    {
                        if (interceptAction != null)
                            interceptAction(buffer, 0, bytes);
    
                        writer.Write(buffer, 0, bytes);
                    }
                };

            Thread thread = new Thread(copyToDelegate);
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
