using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Plethora.IO
{
    /// <summary>
    /// <see cref="TextWriter"/> for casting a message to multiple TextWriters.
    /// </summary>
    public sealed class MulticastWriter : TextWriter
    {
        #region Fields

        private readonly HashSet<TextWriter> writers;
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MulticastWriter"/> class.
        /// </summary>
        public MulticastWriter()
        {
            this.writers = new HashSet<TextWriter>();
        }

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


            this.writers = new HashSet<TextWriter>(textWriters);
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
                if (this.writers.Count > 0)
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
            foreach (TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }

        /// <summary>
        /// Writes a character array to the registered TextWriters.
        /// </summary>
        /// <param name="buffer">
        /// The character array to write to the text stream.
        /// </param>
        public override void Write(char[] buffer)
        {
            if (buffer == null)
                return;

            foreach (TextWriter writer in this.writers)
            {
                writer.Write(buffer);
            }
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

            foreach (TextWriter writer in this.writers)
            {
                writer.Write(buffer, index, count);
            }
        }

        /// <summary>
        /// Writes a string to the registered TextWriters.
        /// </summary>
        /// <param name="value">
        /// The string to write.
        /// </param>
        public override void Write(string value)
        {
            if (value == null)
                return;

            foreach (TextWriter writer in this.writers)
            {
                writer.Write(value);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Register a TextWriter with the <see cref="MulticastWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The TextWriter instance to be registered.
        /// </param>
        /// <remarks>
        /// Registered TextWriters will be written to when this
        /// <see cref="MulticastWriter"/> is written to.
        /// </remarks>
        public void RegisterWriter(TextWriter writer)
        {
            if (!this.writers.Contains(writer))
                this.writers.Add(writer);
        }

        /// <summary>
        /// Deregister a TextWriter from the <see cref="MulticastWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The TextWriter instance to be deregistered.
        /// </param>
        /// <remarks>
        /// Deregistered TextWriters will no longer be written to when this
        /// <see cref="MulticastWriter"/> is written to.
        /// </remarks>
        public void DeregisterWriter(TextWriter writer)
        {
            this.writers.Remove(writer);
        }
        #endregion
    }
}
