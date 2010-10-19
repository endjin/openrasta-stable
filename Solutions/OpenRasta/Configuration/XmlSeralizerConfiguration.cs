namespace OpenRasta.Configuration
{
    #region Using Directives

    using System.Xml.Serialization;

    using OpenRasta.Codecs.Application.xml;
    using OpenRasta.Contracts.Configuration.Fluent;

    #endregion

    public static class XmlSeralizerConfiguration
    {
        public static ICodecDefinition AsXmlSerializer(this ICodecParentDefinition parentDefinition)
        {
            return parentDefinition.TranscodedBy<XmlSerializerCodec>(null);
        }
    }
}