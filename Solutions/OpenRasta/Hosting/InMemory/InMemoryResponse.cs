namespace OpenRasta.Hosting.InMemory
{
    #region Using Directives

    using System.IO;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    #endregion

    public class InMemoryResponse : IResponse
    {
        private readonly MemoryStream outputStream = new MemoryStream();

        public InMemoryResponse()
        {
            this.Headers = new HttpHeaderDictionary();
            this.Entity = new HttpEntity(this.Headers, this.outputStream);
        }

        public IHttpEntity Entity { get; set; }

        public HttpHeaderDictionary Headers { get; set; }

        public bool HeadersSent { get; private set; }

        public int StatusCode { get; set; }

        public void WriteHeaders()
        {
            this.HeadersSent = true;
        }
    }
}