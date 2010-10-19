namespace OpenRasta.Contracts.Codecs
{
    using OpenRasta.Contracts.TypeSystem;
    using OpenRasta.Contracts.Web;

    public interface IMediaTypeReader : ICodec
    {
        object ReadFrom(IHttpEntity request, IType destinationType, string destinationName);
    }
}