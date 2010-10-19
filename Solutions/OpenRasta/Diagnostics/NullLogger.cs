namespace OpenRasta.Diagnostics
{
    #region Using Directives

    using System;

    using OpenRasta.Contracts.Diagnostics;

    #endregion

    public class NullLogger : ILogger
    {
        private static readonly ILogger InternalInstance = new NullLogger();
        private static readonly OperationCookie Cookie = new OperationCookie();

        public static ILogger Instance
        {
            get { return InternalInstance; }
        }

        public IDisposable Operation(object source, string name)
        {
            return Cookie;
        }

        public void WriteDebug(string message, params object[] format)
        {
        }

        public void WriteError(string message, params object[] format)
        {
        }

        public void WriteException(Exception e)
        {
        }

        public void WriteInfo(string message, params object[] format)
        {
        }

        public void WriteWarning(string message, params object[] format)
        {
        }

        private class OperationCookie : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}