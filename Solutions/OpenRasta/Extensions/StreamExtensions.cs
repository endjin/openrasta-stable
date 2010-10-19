namespace OpenRasta.Extensions
{
    using System;
    using System.IO;

    public static class StreamExtensions
    {
        public static long CopyTo(this Stream stream, Stream destinationStream)
        {
            var buffer = new byte[4096];
            int readCount = 0;
            long totalWritten = 0;

            while ((readCount = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalWritten += readCount;
                destinationStream.Write(buffer, 0, readCount);
            }

            return totalWritten;
        }

        public static byte[] ReadToEnd(this Stream stream)
        {
            var streamToReturn = stream as MemoryStream;
            
            if (streamToReturn == null)
            {
                streamToReturn = new MemoryStream();
                stream.CopyTo(streamToReturn);
                streamToReturn.Position = 0;
            }

            var destinationBytes = new byte[streamToReturn.Length - streamToReturn.Position];
            
            Buffer.BlockCopy(
                streamToReturn.GetBuffer(),
                (int)streamToReturn.Position,
                destinationBytes, 
                0, 
                (int)(streamToReturn.Length - streamToReturn.Position));
            
            return destinationBytes;
        }

        public static void Write(this Stream stream, byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
        }
    }
}