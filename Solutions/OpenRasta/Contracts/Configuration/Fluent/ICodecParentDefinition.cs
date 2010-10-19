namespace OpenRasta.Contracts.Configuration.Fluent
{
    using System;

    using OpenRasta.Contracts.Codecs;

    public interface ICodecParentDefinition : INoIzObject
    {
        ICodecDefinition TranscodedBy<TCodec>(object configuration) where TCodec : ICodec;

        ICodecDefinition TranscodedBy(Type type, object configuration);
    }
}