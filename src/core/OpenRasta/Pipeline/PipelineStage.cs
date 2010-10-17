namespace OpenRasta.Pipeline
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using OpenRasta.Collections;

    public class PipelineStage : IEnumerable<ContributorCall>
    {
        private readonly ResumableIterator<ContributorCall, Type> enumerator;

        public PipelineStage(IPipeline pipeline)
        {
            this.enumerator = new ResumableIterator<ContributorCall, Type>(
                new List<ContributorCall>(pipeline.CallGraph).GetEnumerator(),
                x => x.Target != null ? x.Target.GetType() : null,
                (contributorType, key) => key != null && key.IsAssignableFrom(contributorType));
        }

        public PipelineStage(PipelineRunner pipeline, PipelineStage ownerStage)
            : this(pipeline)
        {
            this.OwnerStage = ownerStage;
        }

        public PipelineContinuation CurrentState { get; set; }

        public PipelineStage OwnerStage { get; set; }

        public bool ResumeFrom<T>() where T : IPipelineContributor
        {
            return this.enumerator.ResumeFrom(typeof(T));
        }

        public void SuspendAfter<T>() where T : IPipelineContributor
        {
            this.enumerator.SuspendAfter(typeof(T));
        }

        public IEnumerator<ContributorCall> GetEnumerator()
        {
            return this.enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}