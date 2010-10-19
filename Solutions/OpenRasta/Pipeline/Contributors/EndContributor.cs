namespace OpenRasta.Pipeline.Contributors
{
    using System.Linq;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;
    using OpenRasta.Web;

    public class EndContributor : KnownStages.IEnd
    {
        public void Initialize(IPipeline pipelineRunner)
        {
            var notification = pipelineRunner.Notify(this.ReturnFinished);
            IPipelineExecutionOrderAnd and = null;

            foreach (var contributor in pipelineRunner.Contributors.Where(x => x != this))
            {
                if (and == null)
                {
                    and = notification.After(contributor.GetType());
                }
                else
                {
                    and = and.And.After(contributor.GetType());
                }
            }
        }

        private PipelineContinuation ReturnFinished(ICommunicationContext arg)
        {
            return PipelineContinuation.Finished;
        }
    }
}