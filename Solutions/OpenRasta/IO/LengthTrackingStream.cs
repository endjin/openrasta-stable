namespace OpenRasta.IO
{
    using System.IO;

    /// <summary>
    /// Provides a stream that can keep track of how much data was written to a non-seekable stream.
    /// </summary>
    public class LengthTrackingStream : WrapperStream
    {
        private long length;

        public LengthTrackingStream(Stream underlyingStream) : base(underlyingStream)
        {
        }

        public override long Length
        {
            get
            {
                if (base.CanSeek)
                {
                    return base.Length;
                }
                
                return this.length;
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            
            if (!CanSeek)
            {
                this.length += count;
            }
        }
    }
}