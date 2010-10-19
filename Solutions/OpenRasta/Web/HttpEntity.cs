namespace OpenRasta.Web
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;
    using OpenRasta.IO;

    #endregion

    public class HttpEntity : IHttpEntity
    {
        public HttpEntity(HttpHeaderDictionary messageheaders, Stream entityBodyStream)
        {
            this.Headers = messageheaders;
            
            if (entityBodyStream != null)
            {
                Stream = new LengthTrackingStream(entityBodyStream);
            }
            
            this.Errors = new List<Error>();
        }

        public HttpEntity() : this(new HttpHeaderDictionary(), null)
        {
        }

        public string FileName
        {
            get { return null; }
        }

        public ICodec Codec { get; set; }

        public long? ContentLength
        {
            get { return this.Headers.ContentLength; } 
            set { this.Headers.ContentLength = value; }
        }

        public MediaType ContentType
        {
            get { return this.Headers.ContentType; }
            set { this.Headers.ContentType = value; }
        }

        public IList<Error> Errors { get; set; }

        public HttpHeaderDictionary Headers { get; private set; }

        public object Instance { get; set; }

        public Stream Stream { get; private set; }
    }
}