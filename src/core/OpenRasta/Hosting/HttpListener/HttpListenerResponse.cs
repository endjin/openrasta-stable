namespace OpenRasta.Hosting.HttpListener
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;

    using OpenRasta.Web;

    public class HttpListenerResponse : IResponse
    {
        private readonly HttpListenerCommunicationContext context;
        private readonly System.Net.HttpListenerResponse nativeResponse;
        private readonly MemoryStream tempStream = new MemoryStream();

        public HttpListenerResponse(HttpListenerCommunicationContext context, System.Net.HttpListenerResponse response)
        {
            this.context = context;
            this.nativeResponse = response;
            this.Headers = new HttpHeaderDictionary();
            this.Entity = new HttpEntity(this.Headers, this.tempStream);
            this.nativeResponse.SendChunked = false;
        }

        public IHttpEntity Entity { get; set; }

        public HttpHeaderDictionary Headers { get; private set; }

        public bool HeadersSent { get; private set; }

        public int StatusCode
        {
            get { return this.nativeResponse.StatusCode; }
            set { this.nativeResponse.StatusCode = value; }
        }

        public void WriteHeaders()
        {
            if (this.HeadersSent)
            {
                throw new InvalidOperationException("The headers have already been sent.");
            }

            this.nativeResponse.Headers.Clear();
            
            foreach (var header in this.Headers.Where(h => h.Key != "Content-Length"))
            {
                try
                {
                    this.nativeResponse.AddHeader(header.Key, header.Value);
                }
                catch (Exception ex)
                {
                    if (this.context != null)
                    {
                        this.context.ServerErrors.Add(new Error { Message = ex.ToString() });
                    }
                }
            }

            this.HeadersSent = true;
            this.nativeResponse.ContentLength64 = this.Headers.ContentLength.GetValueOrDefault();

            // Guard against a possible HttpListenerException : The specified network name is no longer available
            try
            {
                this.tempStream.WriteTo(this.nativeResponse.OutputStream);
            }
            catch (HttpListenerException ex)
            {
                if (this.context != null)
                {
                    this.context.ServerErrors.Add(new Error { Message = ex.ToString() });
                }
            }
        }
    }
}