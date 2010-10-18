namespace OpenRasta.Configuration
{
    using OpenRasta.Configuration.Fluent;
    using OpenRasta.Web;

    public static class CodecDefinitionExtensions
    {
        public static ICodecWithMediaTypeDefinition ForMediaType(this ICodecDefinition codecDefinition, string mediaType)
        {
            return codecDefinition.ForMediaType(new MediaType(mediaType));
        }
    }
}