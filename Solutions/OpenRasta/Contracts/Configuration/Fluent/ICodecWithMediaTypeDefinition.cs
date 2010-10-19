namespace OpenRasta.Contracts.Configuration.Fluent
{
    public interface ICodecWithMediaTypeDefinition : ICodecDefinition
    {
        ICodecWithMediaTypeDefinition ForExtension(string extension);
    }
}