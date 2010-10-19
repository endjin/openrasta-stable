namespace OpenRasta.Diagnostics
{
    #region Using Directives

    using System.Diagnostics;

    using OpenRasta.Contracts.Diagnostics;

    #endregion

    public class TraceSourceLogger<T> : TraceSourceLogger, ILogger<T> where T : ILogSource
    {
        public TraceSourceLogger() : base(new TraceSource(LogSource<T>.Category))
        {
        }
    }
}