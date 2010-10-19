namespace OpenRasta.Hosting.InMemory
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    #endregion

    public class InMemoryCommunicationContext : ICommunicationContext
    {
        public InMemoryCommunicationContext()
        {
            this.ApplicationBaseUri = new Uri("http://local");
            this.Request = new InMemoryRequest();
            this.Response = new InMemoryResponse();
            this.ServerErrors = new List<Error>();

            PipelineData = new PipelineData();
        }

        public Uri ApplicationBaseUri { get; set; }

        public OperationResult OperationResult { get; set; }

        public PipelineData PipelineData { get; set; }

        public IRequest Request { get; set; }

        public IResponse Response { get; set; }

        public IList<Error> ServerErrors { get; set; }

        public IPrincipal User { get; set; }
    }
}