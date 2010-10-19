namespace OpenRasta.Codecs
{
    using OpenRasta.Binding;
    using OpenRasta.Codecs.Attributes;
    using OpenRasta.Contracts.Binding;
    using OpenRasta.Contracts.Codecs;
    using OpenRasta.Contracts.DI;
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;
    using OpenRasta.DI;
    using OpenRasta.TypeSystem;
    using OpenRasta.Web;

    [MediaType("multipart/form-data;q=0.5")]
    [SupportedType(typeof(object))]
    public class MultipartFormDataKeyedValuesCodec : AbstractMultipartFormDataCodec, IKeyedValuesMediaTypeReader<IMultipartHttpEntity>
    {
        public MultipartFormDataKeyedValuesCodec(ICommunicationContext context, ICodecRepository codecs, IDependencyResolver container, ITypeSystem typeSystem, IObjectBinderLocator binderLocator)
            : base(context, codecs, container, typeSystem, binderLocator)
        {
        }
    }
}