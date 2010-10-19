namespace OpenRasta.Diagnostics
{
    using OpenRasta.Contracts.Diagnostics;

    public static class LoggerExtensions
    {
        public static ILogger Safe(this ILogger logger)
        {
            if (logger == null)
            {
                return NullLogger.Instance;
            }

            return logger;
        }
    }
}