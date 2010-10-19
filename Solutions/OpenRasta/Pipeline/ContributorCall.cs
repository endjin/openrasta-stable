namespace OpenRasta.Pipeline
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Pipeline;
    using OpenRasta.Contracts.Web;

    #endregion

    public class ContributorCall
    {
        private IPipelineContributor target;

        public ContributorCall()
        {
            this.ContributorTypeName = "Unknown";
        }

        public ContributorCall(IPipelineContributor target, Func<ICommunicationContext, PipelineContinuation> action, string description)
        {
            this.Action = action;
            this.ContributorTypeName = description;
            this.Target = target;
        }

        public string ContributorTypeName { get; set; }

        public IPipelineContributor Target
        {
            get
            {
                return this.target;
            }

            set
            {
                this.target = value;

                if (this.target != null && this.ContributorTypeName == null)
                {
                    this.ContributorTypeName = this.target.GetType().Name;
                }
            }
        }

        public Func<ICommunicationContext, PipelineContinuation> Action { get; set; }
    }
}