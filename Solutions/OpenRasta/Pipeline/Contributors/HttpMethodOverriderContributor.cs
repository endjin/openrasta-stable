namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Exceptions;

    #endregion

    /// <summary>
    /// Supports the use of the X-HTTP-Method-Override header to override the verb used
    /// by OpenRasta for processing.
    /// </summary>
    /// <remarks>Clients that can add http headers may not support other verbs than POST (Flash and Silverlight for example). With the X-HTTP-Method-Override header, OpenRasta will process the request as if it was made using a genuine http verb.</remarks>
    public class HttpMethodOverriderContributor : IPipelineContributor
    {
        private const string HttpMethodOverride = "X-HTTP-Method-Override";
        
        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.OverrideHttpVerb).Before<KnownStages.IHandlerSelection>();
        }

        public PipelineContinuation OverrideHttpVerb(ICommunicationContext context)
        {
            if (context.Request.Headers[HttpMethodOverride] != null)
            {
                if (context.Request.HttpMethod != "POST")
                {
                    context.ServerErrors.Add(new MethodIsNotPostError(context.Request.HttpMethod));

                    return PipelineContinuation.Abort;
                }

                context.Request.HttpMethod = context.Request.Headers[HttpMethodOverride];
            }

            return PipelineContinuation.Continue;
        }

        public class MethodIsNotPostError : ErrorFrom<HttpMethodOverriderContributor>
        {
            public MethodIsNotPostError(string requestedMethod)
            {
                Title = "Overriding the http method is not supported on method " + requestedMethod;
                Message =
                    "The X-HTTP-Method-Override http header can only be added to requests that are sent as a POST.\r\n"
                    + "Http methods are case-sensitive, make sure the method is in all upper-case.";
            }
        }
    }
}