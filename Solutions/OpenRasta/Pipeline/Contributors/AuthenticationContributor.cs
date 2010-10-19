namespace OpenRasta.Pipeline.Contributors
{
    using System;
    using System.Linq;
    using System.Security.Principal;

    using OpenRasta.Authentication;
    using OpenRasta.Contracts.Authentication;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Web;

    public class AuthenticationContributor : KnownStages.IAuthentication
    {
        private readonly IDependencyResolver resolver;

        public AuthenticationContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public ILogger Log { get; set; }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.AuthoriseRequest)
                .After<KnownStages.IBegin>()
                .And
                .Before<KnownStages.IHandlerSelection>();
        }

        private static string ExtractRequestedAuthScheme(IRequest request)
        {
            var authRequestHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authRequestHeader))
            {
                return null;
            }

            var requestedAuthSchemeName = authRequestHeader.Split(' ')[0];

            if (string.IsNullOrEmpty(requestedAuthSchemeName))
            {
                return null;
            }

            return requestedAuthSchemeName;
        }

        private static IPrincipal CreatePrincipal(AuthenticationResult.Success success, IAuthenticationScheme scheme)
        {
            var identity = new GenericIdentity(success.Username, scheme.Name);

            return new GenericPrincipal(identity, success.Roles);
        }

        private PipelineContinuation AuthoriseRequest(ICommunicationContext context)
        {
            var requestedAuthSchemeName = ExtractRequestedAuthScheme(context.Request);

            if (requestedAuthSchemeName == null)
            {
                return PipelineContinuation.Continue;
            }

            var authenticators = this.resolver.ResolveAll<IAuthenticationScheme>();

            var schemeToUse = authenticators.SingleOrDefault(scheme => string.Equals(scheme.Name, requestedAuthSchemeName, StringComparison.InvariantCultureIgnoreCase));

            if (schemeToUse == null)
            {
                context.Response.Headers["Warning"] = "Unsupported Authentication Scheme";
                
                return PipelineContinuation.Continue;
            }

            var authResult = schemeToUse.Authenticate(context.Request);

            if (authResult is AuthenticationResult.Success)
            {
                var success = authResult as AuthenticationResult.Success;
                context.User = CreatePrincipal(success, schemeToUse);
            }

            if (authResult is AuthenticationResult.MalformedCredentials)
            {
                context.OperationResult = new OperationResult.BadRequest();
                context.Response.Headers["Warning"] = "Malformed credentials";
                
                return PipelineContinuation.RenderNow;
            }

            if (authResult is AuthenticationResult.Failed)
            {
                context.OperationResult = new OperationResult.Unauthorized();
            }

            return PipelineContinuation.Continue;
        }
    }
}