namespace OpenRasta.Contracts.Pipeline
{
    public interface IPipelineContributor
    {
        void Initialize(IPipeline pipelineRunner);
    }
}