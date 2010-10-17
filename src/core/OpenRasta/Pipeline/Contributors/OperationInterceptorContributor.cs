namespace OpenRasta.Pipeline.Contributors
{
    using System.Linq;

    using OpenRasta.DI;
    using OpenRasta.OperationModel;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.Pipeline;
    using OpenRasta.Web;

    public class OperationInterceptorContributor : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;

        public OperationInterceptorContributor(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.WrapOperations)
                .After<KnownStages.IRequestDecoding>()
                .And
                .Before<KnownStages.IOperationExecution>();
        }

        private PipelineContinuation WrapOperations(ICommunicationContext context)
        {
            context.PipelineData.Operations = from op in context.PipelineData.Operations
                                              let interceptors = this.resolver.Resolve<IOperationInterceptorProvider>().GetInterceptors(op)
                                              select (IOperation)new OperationWithInterceptors(op, interceptors);

            return PipelineContinuation.Continue;
        }
    }
}