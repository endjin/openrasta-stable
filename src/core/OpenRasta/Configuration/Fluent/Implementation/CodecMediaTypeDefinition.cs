namespace OpenRasta.Configuration.Fluent.Implementation
{
    using OpenRasta.Configuration.MetaModel;
    using OpenRasta.Web;

    public class CodecMediaTypeDefinition : ICodecWithMediaTypeDefinition
    {
        private readonly MediaTypeModel model;
        private readonly CodecDefinition parent;

        public CodecMediaTypeDefinition(CodecDefinition parent, MediaTypeModel model)
        {
            this.parent = parent;
            this.model = model;
        }

        public ICodecParentDefinition And
        {
            get { return this.parent.And; }
        }

        public ICodecWithMediaTypeDefinition ForMediaType(MediaType mediaType)
        {
            return this.parent.ForMediaType(mediaType);
        }

        public ICodecWithMediaTypeDefinition ForExtension(string extension)
        {
            this.model.Extensions.Add(extension);

            return this;
        }
    }
}