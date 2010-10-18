namespace OpenRasta.Configuration.Fluent
{
    using OpenRasta.Web;

    public interface ICodecDefinition : IRepeatableDefinition<ICodecParentDefinition>
    {
        ICodecWithMediaTypeDefinition ForMediaType(MediaType mediaType);
    }
}