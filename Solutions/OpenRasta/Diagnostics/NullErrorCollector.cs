namespace OpenRasta.Diagnostics
{
    public class NullErrorCollector : IErrorCollector
    {
        private static readonly IErrorCollector InternalInstance = new NullErrorCollector();

        public NullErrorCollector()
        {
        }

        public static IErrorCollector Instance
        {
            get { return InternalInstance; }
        }

        public void AddServerError(Error error)
        {
        }
    }
}