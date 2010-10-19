namespace OpenRasta.Contracts.Configuration.Fluent
{
    public interface IResourceDefinition : INoIzObject
    {
        ICodecParentDefinition WithoutUri { get; }

        IUriDefinition AtUri(string uri);
    }
}