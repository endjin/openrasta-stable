namespace OpenRasta.Diagnostics
{
    using OpenRasta.Contracts;
    using OpenRasta.Exceptions;

    public class NullErrorCollector : IErrorCollector
    {
        private static readonly IErrorCollector InternalInstance = new NullErrorCollector();

        public static IErrorCollector Instance
        {
            get { return InternalInstance; }
        }

        public void AddServerError(Error error)
        {
        }
    }
}