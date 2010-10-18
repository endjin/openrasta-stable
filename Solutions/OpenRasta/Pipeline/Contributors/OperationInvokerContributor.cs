namespace OpenRasta.Pipeline.Contributors
{
    using OpenRasta.DI;
    using OpenRasta.OperationModel;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    public class OperationInvokerContributor : KnownStages.IOperationExecution
    {
        private readonly IDependencyResolver resolver;

        public OperationInvokerContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.ExecuteOperations).After<KnownStages.IRequestDecoding>();
        }

        private PipelineContinuation ExecuteOperations(ICommunicationContext context)
        {
            var executor = this.resolver.Resolve<IOperationExecutor>();
            
            try
            {
                context.OperationResult = executor.Execute(context.PipelineData.Operations);
            }
            catch (InterceptorException)
            {
                if (context.OperationResult != null)
                {
                    return PipelineContinuation.RenderNow;
                }

                throw;
            }

            return PipelineContinuation.Continue;
        }
    }
}