namespace OpenRasta.Configuration.Fluent
{
    using System;

    using OpenRasta.Codecs;

    public interface ICodecParentDefinition : INoIzObject
    {
        ICodecDefinition TranscodedBy<TCodec>(object configuration) where TCodec : ICodec;

        ICodecDefinition TranscodedBy(Type type, object configuration);
    }
}