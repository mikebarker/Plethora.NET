using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plethora.IO
{
    /// <summary>
    /// <see cref="TextWriter"/> for casting a message to multiple TextWriters.
    /// </summary>
    public sealed class MulticastWriter : TextWriter
    {
        #region Fields

        private readonly TextWriter[] writers;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MulticastWriter"/> class.
        /// </summary>
        public MulticastWriter(params TextWriter[] textWriters)
            : this((IEnumerable<TextWriter>)textWriters)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MulticastWriter"/> class.
        /// </summary>
        public MulticastWriter(IEnumerable<TextWriter> textWriters)
        {
            //Validation
            if (textWriters == null)
                throw new ArgumentNullException(nameof(textWriters));


            this.writers = textWriters.ToArray();
        }
        #endregion

        #region TextWriter Override

        /// <summary>
        /// Gets the Encoding in which the output is written.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                if (this.writers.Length > 0)
                    return this.writers.First().Encoding;

                return Encoding.Unicode;
            }
        }

        /// <summary>
        /// Writes a character to the registered TextWriters.
        /// </summary>
        /// <param name="value">
        /// The character to write.
        /// </param>
        public override void Write(char value)
        {
            this.Execute(writer => writer.Write(value));
        }

        /// <summary>
        /// Writes a character array to the registered TextWriters.
        /// </summary>
        /// <param name="buffer">
        /// The character array to write to the text stream.
        /// </param>
        public override void Write(char[]? buffer)
        {
            if (buffer == null)
                return;

            this.Execute(writer => writer.Write(buffer));
        }

        /// <summary>
        /// Writes a character array to the registered TextWriters.
        /// </summary>
        /// <param name="buffer">
        /// The character array to write data from.
        /// </param>
        /// <param name="index">
        /// The starting index in the buffer.
        /// </param>
        /// <param name="count">
        /// The number of characters to write.
        /// </param>
        public override void Write(char[] buffer, int index, int count)
        {
            if (buffer == null)
                return;

            this.Execute(writer => writer.Write(buffer, index, count));
        }

        /// <summary>
        /// Writes a string to the registered TextWriters.
        /// </summary>
        /// <param name="value">
        /// The string to write.
        /// </param>
        public override void Write(string? value)
        {
            if (value == null)
                return;

            this.Execute(writer => writer.Write(value));
        }

        /// <summary>
        /// Writes a character to the text string or stream asynchronously.
        /// </summary>
        /// <param name="value">The character to write to the text stream.</param>
        /// <returns>
        /// A task that represents the asynchronous write operation.
        /// </returns>
        public override Task WriteAsync(char value)
        {
            return this.ExecuteAsync(writer => writer.WriteAsync(value));
        }

        /// <summary>
        /// Writes a subarray of characters to the text string or stream asynchronously.
        /// </summary>
        /// <param name="buffer">The character array to write data from.</param>
        /// <param name="index">The character position in the buffer at which to start retrieving data.</param>
        /// <param name="count">The number of characters to write.</param>
        /// <returns>
        /// A task that represents the asynchronous write operation.
        /// </returns>
        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            if (buffer == null)
                return Task.CompletedTask;

            return this.ExecuteAsync(writer => writer.WriteAsync(buffer, index, count));
        }

        /// <summary>
        /// Writes a string to the text string or stream asynchronously.
        /// </summary>
        /// <param name="value">The string to write. If value is null, nothing is written to the text stream.</param>
        /// <returns>
        /// A task that represents the asynchronous write operation.
        /// </returns>
        public override Task WriteAsync(string? value)
        {
            if (value == null)
                return Task.CompletedTask;

            return this.ExecuteAsync(writer => writer.WriteAsync(value));
        }

        #endregion

        private void Execute(Action<TextWriter> action)
        {
            for (int i = 0; i < writers.Length; i++)
            {
                TextWriter writer = this.writers[i];
                action(writer);
            }
        }

        private Task ExecuteAsync(Func<TextWriter, Task> func)
        {
            Task[] tasks = new Task[this.writers.Length];
            for (int i = 0; i < writers.Length; i++)
            {
                TextWriter writer = this.writers[i];
                tasks[i] = func(writer);
            }

            return Task.WhenAll(tasks);
        }
    }
}
