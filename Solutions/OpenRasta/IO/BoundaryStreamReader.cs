namespace OpenRasta.IO
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Text;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.IO.Diagnostics;
    using OpenRasta.Diagnostics;
    using OpenRasta.Extensions;

    #endregion

    public class BoundaryStreamReader
    {
        private readonly byte[] beginBoundary;
        private readonly string beginBoundaryAsString;
        private readonly byte[] localBuffer;
        private readonly byte[] newLine = new byte[] { 13, 10 };
        private int localBufferLength;
        private BoundarySubStream previousStream;

        public BoundaryStreamReader(string boundary, Stream baseStream)
            : this(boundary, baseStream, Encoding.ASCII)
        {
        }

        public BoundaryStreamReader(string boundary, Stream baseStream, Encoding streamEncoding)
            : this(boundary, baseStream, streamEncoding, 4096)
        {
        }

        public BoundaryStreamReader(string boundary, Stream baseStream, Encoding streamEncoding, int bufferLength)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }

            if (!baseStream.CanSeek || !baseStream.CanRead)
            {
                throw new ArgumentException("baseStream must be a seekable readable stream.");
            }

            if (bufferLength < boundary.Length + 6)
            {
                throw new ArgumentOutOfRangeException(
                    "bufferLength",
                    "The buffer needs to be big enough to contain the boundary and control characters (6 bytes)");
            }

            this.Log = NullLogger<IOLogSource>.Instance;
            this.BaseStream = baseStream;

            // by default if unspecified an encoding should be ascii
            // some people are of the opinion that utf-8 should be parsed by default
            // or that it should depend on the source page.
            // Need to test what browsers do in the wild.
            Encoding = streamEncoding;
            Encoding.GetBytes("--" + boundary);
            this.beginBoundary = Encoding.GetBytes("\r\n--" + boundary);
            this.localBuffer = new byte[bufferLength];
            this.beginBoundaryAsString = "--" + boundary;
            this.AtPreamble = true;
        }

        public bool AtBoundary { get; private set; }

        public bool AtEndBoundary { get; private set; }

        public bool AtPreamble { get; private set; }

        public Stream BaseStream { get; private set; }

        public Encoding Encoding { get; private set; }

        public ILogger Log { get; set; }

        public Stream GetNextPart()
        {
            if (this.AtEndBoundary)
            {
                return null;
            }

            this.SkipPreamble();

            if (this.AtEndBoundary)
            {
                return null;
            }

            this.TerminateExistingStream();

            return this.previousStream = new BoundarySubStream(this);
        }

        /// <summary>
        /// Used only to parse boundaries and headers. ASCII always.
        /// </summary>
        /// <returns></returns>
        public string ReadLine()
        {
            bool temp;
            return this.ReadLine(out temp);
        }

        public string ReadLine(out bool crlfFound)
        {
            this.TerminateExistingStream();
            if (this.AtEndBoundary)
            {
                crlfFound = true;
                return null;
            }

            var toConvert = this.ReadUntil(this.newLine, true, out crlfFound);
            
            if (toConvert == null)
            {
                this.AtEndBoundary = true; // reached the end of the steam
                return null;
            }

            string convertedLine = toConvert.Length == 0 ? string.Empty : Encoding.GetString(toConvert).TrimEnd();
            
            if (string.Compare(convertedLine, this.beginBoundaryAsString, StringComparison.OrdinalIgnoreCase) == 0)
            {
                this.AtPreamble = false;
                this.AtBoundary = true;
                
                return convertedLine;
            }

            if (string.Compare(convertedLine, this.beginBoundaryAsString + "--", StringComparison.OrdinalIgnoreCase) == 0)
            {
                this.AtPreamble = false;
                this.AtBoundary = true;
                this.AtEndBoundary = true;

                return convertedLine;
            }

            return convertedLine;
        }

        public byte[] ReadNextPart()
        {
            if (this.AtEndBoundary)
            {
                return new byte[0];
            }

            var part = new MemoryStream();
            long count = this.ReadNextPart(part, true);
            var result = new byte[count];
            Buffer.BlockCopy(part.GetBuffer(), 0, result, 0, (int)count);
            
            return result;
        }

        public long ReadNextPart(Stream destinationStream, bool continueToNextBoundaryOnEmptyRead)
        {
            if (this.AtEndBoundary)
            {
                return 0;
            }

            this.TerminateExistingStream();

            if (this.AtEndBoundary)
            {
                return 0;
            }

            long bytesRead = 0;
            long lastRead;

            if (this.TryReadPreamble(destinationStream, ref bytesRead))
            {
                return bytesRead;
            }

            bool markerFound;

            while ((lastRead = this.ReadUntil(destinationStream, this.beginBoundary, false, out markerFound)) >= 0)
            {
                bytesRead += lastRead;
                
                if (markerFound)
                {
                    this.BaseStream.Read(new byte[2], 0, 2);

                    string line = this.ReadLine();

                    if (this.AtBoundary || this.AtEndBoundary)
                    {
                        // no data between boundaries
                        if (bytesRead == 0 && continueToNextBoundaryOnEmptyRead) 
                        {
                            continue;
                        }

                        break;
                    }

                    destinationStream.Write(this.newLine, 0, 2);
                    var encodedLine = Encoding.GetBytes(line);
                    destinationStream.Write(encodedLine, 0, encodedLine.Length);
                    destinationStream.Write(this.newLine, 0, 2);
                    bytesRead += encodedLine.Length + 4;
                }
                else
                {
                    break;
                }
            }

            return bytesRead;
        }

        public long SeekToNextPart()
        {
            this.Log.WriteDebug("Seeking to next available part");
            if (this.AtEndBoundary)
            {
                return 0;
            }

            if (this.AtPreamble)
            {
                this.SkipPreamble();
                return 0;
            }

            return this.ReadNextPart(Stream.Null, false);
        }

        private byte[] ReadUntil(byte[] marker, bool swallowMarker, out bool markerFound)
        {
            var dataToSendBack = new MemoryStream();

            long count = this.ReadUntil(dataToSendBack, marker, swallowMarker, out markerFound);
            if (count == 0 && markerFound)
            {
                return new byte[0];
            }

            if (count == 0 && !markerFound)
            {
                return null;
            }

            return dataToSendBack.ToArray();
        }

        private int ReadUntil(byte[] buffer, int offset, int count, byte[] marker, out MatchState lastMatch)
        {
            lastMatch = MatchState.NotFound;
            int maxReadLength = count > this.localBuffer.Length - this.localBufferLength ? this.localBuffer.Length - this.localBufferLength : count;

            int totalRead = 0;
            int lastReadCount = this.BaseStream.Read(this.localBuffer, this.localBufferLength, maxReadLength);
            
            if (lastReadCount > 0)
            {
                var searchResult = this.localBuffer.Match(0L, marker, 0, lastReadCount + this.localBufferLength);
                lastMatch = searchResult.State;
                
                if (searchResult.State == MatchState.Found)
                {
                    if (searchResult.Index > 0)
                    {
                        Buffer.BlockCopy(this.localBuffer, 0, buffer, offset, (int)searchResult.Index);
                    }

                    long leftOver = (this.localBufferLength + lastReadCount) - searchResult.Index;
                    this.BaseStream.Seek(leftOver * -1, SeekOrigin.Current);
                    this.localBufferLength = 0;
                    totalRead = (int)searchResult.Index;
                }
                else if (searchResult.State == MatchState.NotFound)
                {
                    totalRead = lastReadCount + this.localBufferLength;
                    Buffer.BlockCopy(this.localBuffer, 0, buffer, offset, totalRead);

                    this.localBufferLength = 0;
                }
                else if (searchResult.State == MatchState.Truncated)
                {
                    totalRead = (int)searchResult.Index;
                    Buffer.BlockCopy(this.localBuffer, 0, buffer, offset, totalRead);

                    int datalength = lastReadCount + this.localBufferLength;
                    int leftover = datalength - (int)searchResult.Index;
                    Buffer.BlockCopy(this.localBuffer, (int)searchResult.Index, this.localBuffer, 0, leftover);
                    this.localBufferLength = leftover;
                }
            }
            else
            {
                Buffer.BlockCopy(this.localBuffer, 0, buffer, offset, this.localBufferLength);
                totalRead = this.localBufferLength;
                this.localBufferLength = 0;
            }

            return totalRead;
        }

        private long ReadUntil(Stream destinationStream, byte[] marker, bool swallowMarker, out bool markerFound)
        {
            var buffer = new byte[4096];

            markerFound = false;
            this.AtBoundary = false;
            this.AtEndBoundary = false;
            int lastReadCount;
            long totalCount = 0;
            MatchState lastState;

            while ((lastReadCount = this.ReadUntil(buffer, 0, buffer.Length, marker, out lastState)) > 0)
            {
                destinationStream.Write(buffer, 0, lastReadCount);
                totalCount += lastReadCount;
            }

            if (lastState == MatchState.Found)
            {
                markerFound = true;
                if (swallowMarker)
                {
                    this.BaseStream.Seek(marker.Length, SeekOrigin.Current);
                }
            }

            return totalCount;
        }

        private void SkipPreamble()
        {
            this.Log.WriteDebug("Skip the preamble. AtPreamble was {0}.", this.AtPreamble);
            long preambleSize = 0;
            bool preambleRead = false;
            
            if (this.AtPreamble)
            {
                preambleRead = this.TryReadPreamble(Stream.Null, ref preambleSize);
            }

            this.Log.WriteDebug("Preamble found: {0} of size {1}", preambleRead, preambleSize);
        }

        private void TerminateExistingStream()
        {
            this.Log.WriteDebug("TerminateExistingStream(), previous stream was " + this.previousStream == null ? "null" : "not null");
            
            if (this.previousStream != null)
            {
                this.previousStream.AtEnd = true;
                this.previousStream = null;
                this.SeekToNextPart();
            }
        }

        private bool TryReadPreamble(Stream destinationStream, ref long bytesRead)
        {
            bool wasAtPreamble = this.AtPreamble;

            bool preambleRead = false;
            bool lastPreambleCrLfPending = false;

            while (this.AtPreamble)
            {
                bool crlfFound;
                string currentLine = this.ReadLine(out crlfFound);
                
                if (currentLine == null)
                {
                    break;
                }
                
                lastPreambleCrLfPending = crlfFound;
                
                if (!this.AtBoundary)
                {
                    if (currentLine != string.Empty)
                    {
                        if (preambleRead)
                        {
                            destinationStream.Write(this.newLine);
                            bytesRead += 2;
                        }

                        var encodedLine = Encoding.GetBytes(currentLine);
                        destinationStream.Write(encodedLine);
                        bytesRead += encodedLine.Length;
                    }

                    preambleRead = true;
                }
                else
                {
                    lastPreambleCrLfPending = false;
                }
            }

            if (wasAtPreamble && lastPreambleCrLfPending && bytesRead > 0)
            {
                destinationStream.Write(this.newLine);
                bytesRead += 2;
            }

            return wasAtPreamble;
        }

        private class BoundarySubStream : Stream
        {
            private readonly BoundaryStreamReader reader;

            public BoundarySubStream(BoundaryStreamReader reader)
            {
                this.reader = reader;
            }

            public bool AtEnd { get; set; }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { return true; }
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            /// <exception cref="NotSupportedException"><c>NotSupportedException</c>.</exception>
            public override long Length
            {
                get { throw new NotSupportedException(); }
            }

            /// <exception cref="NotSupportedException"><c>NotSupportedException</c>.</exception>
            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (this.AtEnd)
                {
                    return 0;
                }

                MatchState state;
                int resultCount = this.reader.ReadUntil(buffer, offset, count, this.reader.beginBoundary, out state);

                // reached a boundary
                if (state == MatchState.Found) 
                {
                    this.AtEnd = true;
                }

                return resultCount;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }
        }
    }
}