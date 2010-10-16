namespace OpenRasta.Configuration.Fluent.Implementation
{
    using System;

    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Web;

    public class CodecDefinition : ICodecDefinition
    {
        private readonly CodecModel codecRegistration;

        public CodecDefinition(ResourceDefinition resourceDefinition, Type codecType, object configuration)
        {
            ResourceDefinition = resourceDefinition;
            this.codecRegistration = new CodecModel(codecType, configuration);
            ResourceDefinition.Registration.Codecs.Add(this.codecRegistration);
        }

        public ICodecParentDefinition And
        {
            get { return ResourceDefinition; }
        }

        public ResourceDefinition ResourceDefinition { get; set; }

        public ICodecWithMediaTypeDefinition ForMediaType(MediaType mediaType)
        {
            var model = new MediaTypeModel { MediaType = mediaType };
            this.codecRegistration.MediaTypes.Add(model);

            return new CodecMediaTypeDefinition(this, model);
        }
    }
}