namespace OpenRasta.Configuration.Extensions
{
    using System;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Configuration.Fluent;

    public static class CodecParentDefinitionExtensions
    {
        public static ICodecDefinition TranscodedBy<TCodec>(this ICodecParentDefinition parent) where TCodec : ICodec
        {
            return parent.TranscodedBy<TCodec>(null);
        }

        public static ICodecDefinition TranscodedBy(this ICodecParentDefinition parent, Type codecType)
        {
            return parent.TranscodedBy(codecType, null);
        }
    }
}