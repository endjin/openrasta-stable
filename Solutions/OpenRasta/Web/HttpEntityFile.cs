namespace OpenRasta.Web
{
    #region Using Directives

    using System.IO;

    using OpenRasta.Contracts.IO;
    using OpenRasta.Contracts.Web;

    #endregion

    public class HttpEntityFile : IFile
    {
        private readonly IHttpEntity entity;

        public HttpEntityFile(IHttpEntity entity)
        {
            this.entity = entity;
        }

        public MediaType ContentType
        {
            get { return this.entity.ContentType ?? MediaType.ApplicationOctetStream; }
        }

        public string FileName
        {
            get { return this.entity.Headers.ContentDisposition != null ? this.entity.Headers.ContentDisposition.FileName : null; }
        }

        public long Length
        {
            get { return this.entity.Stream.Length; }
        }

        public Stream OpenStream()
        {
            return this.entity.Stream;
        }
    }
}