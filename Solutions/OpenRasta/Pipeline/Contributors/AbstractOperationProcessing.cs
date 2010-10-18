namespace OpenRasta.Pipeline.Contributors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.DI;
    using OpenRasta.OperationModel;
    using OpenRasta.Web;

    public abstract class AbstractOperationProcessing<TProcessor, TStage> : IPipelineContributor
        where TProcessor : IOperationProcessor<TStage>
        where TStage : IPipelineContributor
    {
        private readonly IDependencyResolver resolver;

        protected AbstractOperationProcessing(IDependencyResolver resolver)
        {
            this.resolver = resolver;
        }

        public virtual PipelineContinuation ProcessOperations(ICommunicationContext context)
        {
            context.PipelineData.Operations = this.ProcessOperations(context.PipelineData.Operations).ToList();

            if (context.PipelineData.Operations.Count() == 0)
            {
                return this.OnOperationsEmpty(context);
            }

            return this.OnOperationProcessingComplete(context.PipelineData.Operations) ?? PipelineContinuation.Continue;
        }

        public virtual IEnumerable<IOperation> ProcessOperations(IEnumerable<IOperation> operations)
        {
            var chain = this.GetMethods().Chain();

            return chain == null ? new IOperation[0] : chain(operations);
        }

        public virtual void Initialize(IPipeline pipelineRunner)
        {
            this.InitializeWhen(pipelineRunner.Notify(this.ProcessOperations));
        }

        protected abstract void InitializeWhen(IPipelineExecutionOrder pipeline);

        protected virtual PipelineContinuation? OnOperationProcessingComplete(IEnumerable<IOperation> ops)
        {
            return null;
        }

        protected virtual PipelineContinuation OnOperationsEmpty(ICommunicationContext context)
        {
            context.OperationResult = new OperationResult.MethodNotAllowed();

            return PipelineContinuation.RenderNow;
        }

        private IEnumerable<Func<IEnumerable<IOperation>, IEnumerable<IOperation>>> GetMethods()
        {
            var operationProcessors = this.resolver.ResolveAll<TProcessor>();

            foreach (var filter in operationProcessors)
            {
                yield return filter.Process;
            }
        }
    }
}