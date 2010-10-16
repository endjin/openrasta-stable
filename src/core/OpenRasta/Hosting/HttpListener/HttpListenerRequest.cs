namespace OpenRasta.Hosting.HttpListener
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using OpenRasta.IO;
    using OpenRasta.Web;

    public class HttpListenerRequest : IRequest
    {
        private readonly HttpListenerCommunicationContext context;
        private readonly System.Net.HttpListenerRequest nativeRequest;
        private string httpMethod;

        public HttpListenerRequest(HttpListenerCommunicationContext context, System.Net.HttpListenerRequest request)
        {
            this.context = context;
            this.nativeRequest = request;
            Uri = this.nativeRequest.Url;
            this.CodecParameters = new List<string>();

            this.Headers = new HttpHeaderDictionary(this.nativeRequest.Headers);

            this.Entity = new HttpEntity(this.Headers, new HistoryStream(this.nativeRequest.InputStream));

            if (!string.IsNullOrEmpty(this.nativeRequest.ContentType))
            {
                this.Entity.ContentType = new MediaType(this.nativeRequest.ContentType);
            }
        }

        public IList<string> CodecParameters { get; private set; }

        public long? ContentLength
        {
            get { return this.Entity.ContentLength; }
            set { this.Entity.ContentLength = value; }
        }

        public IHttpEntity Entity { get; private set; }

        public HttpHeaderDictionary Headers { get; private set; }

        public string HttpMethod
        {
            get { return this.httpMethod ?? this.nativeRequest.HttpMethod; }
            set { this.httpMethod = value; }
        }

        public CultureInfo NegotiatedCulture { get; set; }

        public Uri Uri { get; set; }

        public string UriName { get; set; }
    }
}