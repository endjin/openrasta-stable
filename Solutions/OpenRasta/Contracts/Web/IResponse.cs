namespace OpenRasta.Contracts.Web
{
    public interface IResponse : IHttpMessage
    {
        bool HeadersSent { get; }

        int StatusCode { get; set; }

        void WriteHeaders();
    }
}