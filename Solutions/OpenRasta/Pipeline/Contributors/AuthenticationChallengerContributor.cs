namespace OpenRasta.Pipeline.Contributors
{
    using OpenRasta.Authentication;
    using OpenRasta.DI;
    using OpenRasta.Diagnostics;
    using OpenRasta.Web;

    public class AuthenticationChallengerContributor : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;

        public AuthenticationChallengerContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public ILogger Log { get; set; }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ChallengeIfUnauthorized)
                .After<KnownStages.IOperationExecution>()
                .And
                .Before<KnownStages.IResponseCoding>();
        }

        private PipelineContinuation ChallengeIfUnauthorized(ICommunicationContext context)
        {
            if (context.OperationResult is OperationResult.Unauthorized)
            {
                var supportedSchemes = this.resolver.ResolveAll<IAuthenticationScheme>();

                foreach (var scheme in supportedSchemes)
                {
                    scheme.Challenge(context.Response);
                }
            }

            return PipelineContinuation.Continue;
        }
    }
}