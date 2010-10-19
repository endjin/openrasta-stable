namespace OpenRasta.Contracts.Web
{
    using OpenRasta.Web;

    public interface IHttpMessage
    {
        IHttpEntity Entity { get; }

        HttpHeaderDictionary Headers { get; }
    }
}