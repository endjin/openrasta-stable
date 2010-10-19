namespace OpenRasta.Contracts.Pipeline
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using OpenRasta.Contracts.Web;
    using OpenRasta.Pipeline;

    #endregion

    public interface IPipeline
    {
        bool IsInitialized { get; }

        IList<IPipelineContributor> Contributors { get; }

        IEnumerable<ContributorCall> CallGraph { get; }

        void Initialize();

        IPipelineExecutionOrder Notify(Func<ICommunicationContext, PipelineContinuation> notification);

        void Run(ICommunicationContext context);
    }
}