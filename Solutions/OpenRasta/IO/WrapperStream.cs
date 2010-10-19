namespace OpenRasta.IO
{
    using System.IO;

    public class WrapperStream : Stream
    {
        public WrapperStream(Stream underlyingStream)
        {
            this.UnderlyingStream = underlyingStream;
        }

        public override bool CanRead
        {
            get { return this.UnderlyingStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this.UnderlyingStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this.UnderlyingStream.CanWrite; }
        }

        public override long Length
        {
            get { return this.UnderlyingStream.Length; }
        }

        public override long Position
        {
            get { return this.UnderlyingStream.Position; }
            set { this.UnderlyingStream.Position = value; }
        }

        protected Stream UnderlyingStream { get; private set; }

        public override void Flush()
        {
            this.UnderlyingStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this.UnderlyingStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this.UnderlyingStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this.UnderlyingStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.UnderlyingStream.Write(buffer, offset, count);
        }
    }
}