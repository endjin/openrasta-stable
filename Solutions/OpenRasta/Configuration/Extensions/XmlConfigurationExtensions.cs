namespace OpenRasta.Configuration.Extensions
{
    using OpenRasta.Codecs.Xml;
    using OpenRasta.Contracts.Configuration.Fluent;

    public static class XmlConfigurationExtensions
    {
        public static ICodecDefinition AsXmlDataContract(this ICodecParentDefinition codecParent)
        {
            return codecParent.TranscodedBy<XmlDataContractCodec>();
        }
    }
}