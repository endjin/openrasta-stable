namespace OpenRasta.Configuration.Extensions
{
    using OpenRasta.Codecs.Json;
    using OpenRasta.Contracts.Configuration.Fluent;

    public static class JsonConfigurationExtensions
    {
        public static ICodecDefinition AsJsonDataContract(this ICodecParentDefinition codecParent)
        {
            return codecParent.TranscodedBy<JsonDataContractCodec>();
        }
    }
}