namespace OpenRasta.Contracts.Codecs
{
    using OpenRasta.Contracts.Web;

    public interface IMediaTypeWriter : ICodec
    {
        void WriteTo(object entity, IHttpEntity response, string[] codecParameters);
    }
}