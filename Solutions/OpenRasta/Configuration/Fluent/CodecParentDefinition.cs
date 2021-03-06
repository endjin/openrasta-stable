namespace OpenRasta.Configuration.Fluent
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.Configuration.Fluent;

    #endregion

    public class CodecParentDefinition : ICodecParentDefinition
    {
        private readonly ResourceDefinition resourceDefinition;

        public CodecParentDefinition(ResourceDefinition registration)
        {
            this.resourceDefinition = registration;
        }

        public ICodecDefinition TranscodedBy<TCodec>(object configuration) where TCodec : ICodec
        {
            return this.resourceDefinition.TranscodedBy<TCodec>(configuration);
        }

        public ICodecDefinition TranscodedBy(Type type, object configuration)
        {
            return this.resourceDefinition.TranscodedBy(type, configuration);
        }
    }
}