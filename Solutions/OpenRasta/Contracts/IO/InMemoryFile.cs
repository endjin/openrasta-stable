namespace OpenRasta.Contracts.IO
{
    using System.IO;

    using OpenRasta.Web;

    public class InMemoryFile : IFile
    {
        private readonly Stream stream;
        
        public InMemoryFile() : this(new MemoryStream())
        {
        }

        public InMemoryFile(Stream stream)
        {
            this.stream = stream;
            this.ContentType = MediaType.ApplicationOctetStream;
        }

        public MediaType ContentType { get; set; }

        public string FileName { get; set; }

        public long Length { get; set; }

        public Stream OpenStream()
        {
            this.stream.Position = 0;
            return this.stream;
        }
    }
}