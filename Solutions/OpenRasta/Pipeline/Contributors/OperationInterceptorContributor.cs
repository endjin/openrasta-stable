namespace OpenRasta.Pipeline.Contributors
{
    #region Using Directives

    using System.Linq;

    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.OperationModel.Interceptors;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.OperationModel.Interceptors;
    using OpenRasta.Pipeline;

    #endregion

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