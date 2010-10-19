namespace OpenRasta.Contracts
{
    using OpenRasta.Exceptions;

    public interface IErrorCollector
    {
        void AddServerError(Error error);
    }
}