namespace OpenRasta.Pipeline.Contributors
{
    using System.Collections.Generic;
    using System.Linq;

    using OpenRasta.Contracts.Diagnostics;
    using OpenRasta.Contracts.OperationModel;
    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Diagnostics;
    using OpenRasta.OperationModel;
    using OpenRasta.Pipeline;
    using OpenRasta.Pipeline.Diagnostics;
    using OpenRasta.Web;

    public class OperationCreatorContributor : KnownStages.IOperationCreation
    {
        private readonly IOperationCreator creator;

        public OperationCreatorContributor(IOperationCreator creator)
        {
            this.creator = creator;
            this.Logger = NullLogger<PipelineLogSource>.Instance;
        }

        public ILogger<PipelineLogSource> Logger { get; set; }

        public void Initialize(IPipeline pipelineRunner)
        {
            pipelineRunner.Notify(this.CreateOperations).After<KnownStages.IHandlerSelection>();
        }

        private PipelineContinuation CreateOperations(ICommunicationContext context)
        {
            if (context.PipelineData.SelectedHandlers != null)
            {
                context.PipelineData.Operations = this.creator.CreateOperations(context.PipelineData.SelectedHandlers).ToList();
                this.LogOperations(context.PipelineData.Operations);
                
                if (context.PipelineData.Operations.Count() == 0)
                {
                    context.OperationResult = this.CreateMethodNotAllowed(context);
                
                    return PipelineContinuation.RenderNow;
                }
            }

            return PipelineContinuation.Continue;
        }

        private OperationResult.MethodNotAllowed CreateMethodNotAllowed(ICommunicationContext context)
        {
            return new OperationResult.MethodNotAllowed(context.Request.Uri, context.Request.HttpMethod, context.PipelineData.ResourceKey);
        }

        private void LogOperations(IEnumerable<IOperation> operations)
        {
            if (operations.Count() > 0)
            {
                foreach (var operation in operations)
                {
                    this.Logger.WriteDebug(
                        "Created operation named {0} wth signature {1}", operation.Name, operation.ToString());
                }
            }
            else
            {
                this.Logger.WriteDebug("No operation was created.");
            }
        }
    }
}