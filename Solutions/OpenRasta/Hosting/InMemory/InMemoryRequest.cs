namespace OpenRasta.Hosting.InMemory
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    public class InMemoryRequest : IRequest
    {
        public InMemoryRequest()
        {
            this.Headers = new HttpHeaderDictionary();
            this.Entity = new HttpEntity(this.Headers, new MemoryStream());
            this.CodecParameters = new List<string>();
        }

        public IList<string> CodecParameters { get; private set; }

        public IHttpEntity Entity { get; set; }

        public HttpHeaderDictionary Headers { get; set; }

        public string HttpMethod { get; set; }

        public CultureInfo NegotiatedCulture { get; set; }

        public Uri Uri { get; set; }

        public string UriName { get; set; }
    }
}