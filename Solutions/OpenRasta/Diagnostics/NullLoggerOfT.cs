namespace OpenRasta.Diagnostics
{
    using OpenRasta.Contracts.Diagnostics;

    public class NullLogger<T> : NullLogger, ILogger<T> where T : class, ILogSource
    {
        private static readonly ILogger<T> InternalInstance = new NullLogger<T>();

        public new static ILogger<T> Instance
        {
            get { return InternalInstance; }
        }
    }
}