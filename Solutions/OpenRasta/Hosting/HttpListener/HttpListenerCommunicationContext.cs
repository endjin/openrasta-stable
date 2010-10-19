namespace OpenRasta.Hosting.HttpListener
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Principal;

    using OpenRasta.Contracts.Hosting;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;
    using OpenRasta.Extensions;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    public class HttpListenerCommunicationContext : ICommunicationContext
    {
        private readonly IHost host;
        private readonly HttpListenerContext nativeContext;

        public HttpListenerCommunicationContext(IHost host, HttpListenerContext nativeContext)
        {
            this.ServerErrors = new List<Error>();
            PipelineData = new PipelineData();
            this.host = host;
            this.nativeContext = nativeContext;
            this.User = nativeContext.User;
            this.Request = new HttpListenerRequest(this, nativeContext.Request);
            this.Response = new HttpListenerResponse(this, nativeContext.Response);
        }

        public Uri ApplicationBaseUri
        {
            get
            {
                var request = this.nativeContext.Request;

                string baseUri = "{0}://{1}{2}/".With(
                    request.Url.Scheme,
                    request.Url.Host,
                    request.Url.IsDefaultPort ? string.Empty : ":" + request.Url.Port);

                var appBaseUri = new Uri(new Uri(baseUri, UriKind.Absolute), new Uri(this.host.ApplicationVirtualPath, UriKind.Relative));
                
                return appBaseUri;
            }
        }

        public OperationResult OperationResult { get; set; }

        public PipelineData PipelineData { get; set; }

        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }

        public IList<Error> ServerErrors { get; private set; }

        public IPrincipal User { get; set; }
    }
}