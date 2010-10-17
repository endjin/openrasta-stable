namespace OpenRasta.OperationModel
{
    using OpenRasta.Pipeline;

    public interface IOperationCodecSelector : IOperationProcessor<KnownStages.ICodecRequestSelection>
    {
    }
}