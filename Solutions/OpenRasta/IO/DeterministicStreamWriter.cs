namespace OpenRasta.IO
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Implements a StreamWriter that does not close or dispose the stream when it doesn't own it.
    /// </summary>
    public class DeterministicStreamWriter : StreamWriter
    {
        private readonly StreamActionOnDispose closeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeterministicStreamWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="encoding">The encoding used when writing to the stream.</param>
        /// <param name="action">The action to take for the stream when the writer is closed.</param>
        public DeterministicStreamWriter(Stream stream, Encoding encoding, StreamActionOnDispose action)
            : base(stream, encoding)
        {
            this.closeAction = action;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (BaseStream != null && disposing)
                {
                    Flush();
                }
            }
            finally
            {
                if (this.closeAction == StreamActionOnDispose.Close && BaseStream != null && disposing)
                {
                    BaseStream.Close();
                }
            }
        }
    }
}