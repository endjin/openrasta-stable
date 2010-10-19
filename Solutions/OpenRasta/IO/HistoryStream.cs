namespace OpenRasta.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// Provides a stream over non-seekable streams that buffers all read calls
    /// and provide a seekable recent history of the stream.
    /// </summary>
    public class HistoryStream : Stream
    {
        private readonly byte[] buffer;
        private readonly byte[] tempBuffer;
        private int bufferLength;
        private int bufferPosition;

        public HistoryStream(Stream baseStream) : this(baseStream, 4096)
        {
        }

        public HistoryStream(Stream baseStream, int bufferSize)
        {
            this.buffer = new byte[bufferSize];
            this.tempBuffer = new byte[bufferSize];
            this.UnderlyingStream = baseStream;
        }

        public int BufferSize
        {
            get { return this.buffer.Length; }
        }

        public override bool CanRead
        {
            get { return this.UnderlyingStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public Stream UnderlyingStream { get; private set; }

        public override void Flush()
        {
            // do nothing
        }

        public override int Read(byte[] targetBuffer, int offset, int count)
        {
            if (count > this.buffer.Length)
            {
                count = this.buffer.Length;
            }

            int existingBytesInBuffer = this.bufferLength - this.bufferPosition;

            int unusedBytesInBuffer = this.buffer.Length - this.bufferLength;

            int extraBytesNeededForRead = count - existingBytesInBuffer;

            if (extraBytesNeededForRead > 0)
            {
                if (extraBytesNeededForRead <= unusedBytesInBuffer)
                {
                    int readBytes = this.UnderlyingStream.Read(this.buffer, this.bufferLength, extraBytesNeededForRead);
                    this.bufferLength += readBytes;
                    var numberOfBytesToSend = Math.Min(count, this.bufferLength - this.bufferPosition);
                    Buffer.BlockCopy(this.buffer, this.bufferPosition, targetBuffer, offset, numberOfBytesToSend);
                    this.bufferPosition += numberOfBytesToSend;
                    return numberOfBytesToSend;
                }

                if (extraBytesNeededForRead > unusedBytesInBuffer)
                {
                    int newlyReadBytesFromStream = this.UnderlyingStream.Read(this.tempBuffer, 0, extraBytesNeededForRead);

                    // now check we're not in a case where the read is actually smaller than the avail space
                    if (newlyReadBytesFromStream < unusedBytesInBuffer)
                    {
                        // copy back in our buffer
                        Buffer.BlockCopy(this.tempBuffer, 0, this.buffer, this.bufferLength, newlyReadBytesFromStream);
                        this.bufferLength += newlyReadBytesFromStream;
                        var numberOfBytesToSend = Math.Min(count, this.bufferLength - this.bufferPosition);
                        Buffer.BlockCopy(this.buffer, this.bufferPosition, targetBuffer, offset, numberOfBytesToSend);
                        this.bufferPosition += numberOfBytesToSend;
                        return numberOfBytesToSend;
                    }

                    // not enough space for storing in available space, trim the beginning
                    int additionalBufferSizeRequired = newlyReadBytesFromStream - unusedBytesInBuffer;
                    int remainingBufferLength = this.bufferLength - additionalBufferSizeRequired;
                    
                    Buffer.BlockCopy(this.buffer, additionalBufferSizeRequired, this.buffer, 0, remainingBufferLength);
                    Buffer.BlockCopy(this.tempBuffer, 0, this.buffer, remainingBufferLength, newlyReadBytesFromStream);
                    
                    this.bufferPosition -= this.bufferLength - remainingBufferLength;
                    this.bufferLength = remainingBufferLength + newlyReadBytesFromStream;

                    // finally copy from our new buffer
                    int bytesSentBack = this.bufferLength - this.bufferPosition;
                    bytesSentBack = count < bytesSentBack ? count : bytesSentBack;
                    Buffer.BlockCopy(this.buffer, this.bufferPosition, targetBuffer, offset, bytesSentBack);
                    this.bufferPosition += bytesSentBack;
                    
                    return bytesSentBack;
                }
            }

            Buffer.BlockCopy(this.buffer, this.bufferPosition, targetBuffer, offset, count);
            this.bufferPosition += count;
            
            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin != SeekOrigin.Current)
            {
                throw new NotSupportedException("You cannot seek in this way");
            }

            int requestedPosition = this.bufferPosition + (int)offset;

            // TODO: A seek beyond the buffer length could trigger a read
            if (requestedPosition < 0 || requestedPosition > this.bufferLength)
            {
                throw new InvalidOperationException("You cannot seek further than the amount of data available in the buffer");
            }

            this.bufferPosition = requestedPosition;
            
            return -1;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("The history stream only works in read-only mode");
        }
    }
}